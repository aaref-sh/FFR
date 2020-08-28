using FFR.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FFR.Controllers
{
    public class HomeController : Controller
    {
        Database1Entities db = new Database1Entities();
        public ActionResult Index(int?id)
        {
            if (id == 1) Session.Clear();
            List<meal> lst = db.meals.ToList();
            List<category> cats = db.categories.ToList();
            ViewBag.cats = cats;
            return View(lst);
        }
        public ActionResult favorit()
        {
            if (Session["id"] == null) return RedirectToAction("access_denied");
            int user = Convert.ToInt32(Session["id"]);
            List<meal> mls = (from x in db.favorits where x.customer_id == user select x.meal).ToList();
            List<category> cats = (from x in mls select x.category).ToList();
            ViewBag.cats = cats;
            return View(mls);
        }
        public ActionResult details(int id,int? add)
        {
            bool added = false;
            int user = Convert.ToInt32(Session["id"]);
            meal m = (from x in db.meals where x.Id == id select x).FirstOrDefault();
            customer c = (from x in db.customers where x.Id == user select x).FirstOrDefault();
            favorit f = (from x in db.favorits where x.customer.Id == user && x.meal.Id == id select x).FirstOrDefault();
            if (f != null) added = true;
            if (f==null && add != null)
            {
                favorit fav = new favorit();
                fav.meal = m;
                fav.customer = c;
                db.favorits.Add(fav);
                db.SaveChanges();
                added = true;
            }
            else if(f!=null && add != null)
            {
                db.favorits.Remove(f);
                db.SaveChanges();
                added = false;
            }
            ViewBag.stat = added;
            return View(m);
        }
        [HttpGet]
        public ActionResult login_signup(){
            if (Session["admin"] != null) return RedirectToAction("Index", "Management", null);
            if (Session["id"] != null) return RedirectToAction("Index");
            return View();
        }
        [HttpPost]
        public ActionResult login_signup(string submit,FormCollection col)
        {
            if(submit == "تسجيل الدخول")
            {
                string name = col[1], pass = col[2];
                customer cus = (from x in db.customers where x.name.Trim() == name && x.password == pass select x).FirstOrDefault();
                if (name == "admin" && pass == "administrator") Session["admin"] = "1";
                else if (cus != null) Session["id"]= cus.Id;
                else
                {
                    ViewBag.message = "خطأ في اسم المستخدم أو كلمة المرور";
                    return View();
                }
            }
            else
            {
                string name = col[1], pass = col[4], phone = col[3], address = col[2];
                customer cus = new customer();
                cus.name = name.Trim();
                cus.password = pass;
                cus.phone = phone;
                cus.address = address;
                customer c = (from x in db.customers where x.name == name select x).FirstOrDefault();
                if (c != null)
                {
                    ViewBag.message = "اسم المستخدم موجود بالفعل";
                    return View();
                }
                db.customers.Add(cus);
                db.SaveChanges();
                int id = (from x in db.customers where x.name == name select x.Id).FirstOrDefault();
                Session["id"] = id;
                Session.Timeout = 3600;
            }
            return RedirectToAction("Index","Home",null);
        }
       
        static string center_id = "";
        public ActionResult chose()
        {
            if (Session["id"] == null) return RedirectToAction("access_denied");
            ViewBag.center_id = new SelectList(db.centers, "Id", "name");
            return View();
        } 
        [HttpPost]
        public ActionResult chose(FormCollection col)
        {
            center_id = col[1];
            return RedirectToAction("request");
        }
        [HttpGet]
        public ActionResult edit_profile()
        {
            if (Session["id"] == null) return RedirectToAction("access_denied");
            int user = Convert.ToInt32(Session["id"]);
            customer c = (from x in db.customers where x.Id == user select x).First();
            return View(c);
        }
        [HttpPost]
        public ActionResult edit_profile(customer c)
        {
            if (Session["id"] == null) return RedirectToAction("access_denied");
            db.customers.AddOrUpdate(c);
            db.SaveChanges();
            ViewBag.message = "تم تحديث بيانات الملف الشخصي";
            return View(c);
        }
        [HttpGet]
        public ActionResult request()
        {
            if (Session["id"] == null) return RedirectToAction("access_denied");
            int user = Convert.ToInt32(Session["id"]);
            ViewBag.reqs = (from x in db.requests where x.done != true && x.customer_id == user select x).ToList();
            ViewBag.cats = db.categories.ToList();
            ViewBag.favs = (from x in db.favorits where x.customer_id == user select x).ToList();
            double sum = 0;
            List<double> total = (from x in db.requests where x.done != true && x.customer_id == user select x.meal.price).ToList();
            foreach (var one in total) sum += one;
            ViewBag.total = sum;
            return View(db.meals.ToList());
        }
        static string mn = "";
        [HttpPost]
        public ActionResult request(FormCollection col)
        {
            string mn1 = mn;
            int user = Convert.ToInt32(Session["id"]);
            if (col[1] == "إلغاء الطلب")
            {
                int req = Convert.ToInt32(col.AllKeys[1]);
                request r = (from x in db.requests where x.Id == req select x).FirstOrDefault();
                db.requests.Remove(r);
                db.SaveChanges();
                ViewBag.message = "تم حذف الطلب";
            }
            else if(col[1]== "طلب")
            {
                request req = new request();
                req.center_id = Convert.ToInt32(center_id);
                req.customer_id = user;
                req.meal_id = Convert.ToInt32(col.AllKeys[1]);
                req.reqdate = DateTime.Now;
                db.requests.Add(req);
                db.SaveChanges();
                ViewBag.message = "تم الطلب";
            }
            else
            {
                mn1 = col[0];
            }
            double sum = 0;
            List<double> total = (from x in db.requests where x.done != true && x.customer_id == user select x.meal.price).ToList();
            foreach (var one in total) sum += one;
            ViewBag.srch = mn1;
            ViewBag.total = sum;
            ViewBag.favs = (from x in db.favorits where x.customer_id == user && x.meal.name.Contains(mn1) select x).ToList();
            ViewBag.center_id = new SelectList(db.centers, "Id", "name");
            ViewBag.reqs = (from x in db.requests where x.done != true && x.customer_id == user select x).ToList();
            ViewBag.cats = db.categories.ToList();
            List<meal> meals = (from x in db.meals where x.name.Contains(mn1) select x).ToList();
            return View(meals);
        }
        public ActionResult solds()
        {
            ViewBag.cats = db.categories.ToList();
            List<meal> meals = (from x in db.meals where x.price != x.discount_price select x).ToList();
            return View(meals);
        }
        public ActionResult access_denied()
        {
            return View();
        }
    }
}