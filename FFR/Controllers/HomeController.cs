using FFR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FFR.Controllers
{
    public class HomeController : Controller
    {
        Database1Entities db = new Database1Entities();
        public ActionResult Index()
        {
            List<meal> lst = (from x in db.meal select x).ToList();
            List<categories> cats = (from x in db.categories select x).ToList();
            ViewBag.cats = cats;
            return View(lst);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult details(int id)
        {
            meal m = (from x in db.meal where x.Id == id select x).FirstOrDefault();
           // ViewBag.meal1 = m;
            return View(m);
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}