﻿using FFR.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FFR.Controllers
{
    public class ManagementController : Controller
    {
        Database1Entities db = new Database1Entities();
        // GET: Management
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Add(FormCollection col)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(categories cat)
        {
            if (cat == null) return View();
            List<categories> l = (from x in db.categories where x.name == cat.name.Trim() select x).ToList();
            if(l.Count > 0)
            {
                ViewBag.message = "هذا الصنف موجود بالفعل";
                return View();
            }
            cat.name = cat.name.Trim();
            db.categories.Add(cat);
            db.SaveChanges();
            ViewBag.message = "تمت إضافة الصنف";
            return View();
        }
        string unique()
        {
            return DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.category_id = new SelectList(db.categories, "Id", "name");
            return View();
        }
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase UploadImage,FormCollection col)
        {
            string n = "";
            ViewBag.category_id = new SelectList(db.categories, "Id", "name");
            if (UploadImage.ContentLength > 0)
            {
                string filename = unique() + Path.GetExtension(UploadImage.FileName);
                string folder = Path.Combine(Server.MapPath("~/pics/"), filename);
                UploadImage.SaveAs(folder);
                n = filename;
            }
            else { return View(); }
            string mealname = col[1].Trim();
            List<meal> l = (from x in db.meal where x.name == mealname select x).ToList();
            if(l.Count > 0)
            {
                ViewBag.message = "هنالك وجبة بنفس الاسم" ;
                return View();
            }
            meal m = new meal();
            m.name = col[1].Trim();
            m.picture = n;
            m.price = Convert.ToInt32(col[2].Trim());
            m.discount_price = m.price;
            m.category_id = Convert.ToInt32(col[4].Trim());
            m.discription = col[3].Trim();
            db.meal.Add(m);
            db.SaveChanges();
            ViewBag.message = "تم إضافة الوجبة" ;
            return View();
        }
        public ActionResult delete(int? id)
        {
            if(id != null)
            {
               System.IO.File.Delete(Server.MapPath("~/pics/") + (from x in db.meal where x.Id == id select x.picture).FirstOrDefault());
                db.meal.Remove((from x in db.meal where x.Id == id select x).FirstOrDefault());
                db.SaveChanges();
            }
            List<meal> lst = (from x in db.meal select x).ToList();
            return View(lst);

        }
        public ActionResult Del_cat(int? id)
        {
            if(id != null)
            {
                db.categories.Remove((from x in db.categories where x.Id == id select x).FirstOrDefault());
                db.SaveChanges();
            }
            List<categories> lst = (from x in db.categories select x).ToList();
            return View(lst);

        }

        public ActionResult Edit_list()
        {
            List<meal>l = (from x in db.meal select x).ToList();
            return View(l);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            meal m = (from x in db.meal where x.Id == id select x).FirstOrDefault();
            ViewBag.category_id = new SelectList(db.categories, "Id", "name");
            return View(m);
        }
        [HttpPost]
        public ActionResult Edit(int id,meal mel,HttpPostedFileBase UploadImage)
        {
            if(UploadImage == null)
                mel.picture = (from x in db.meal where x.Id == mel.Id select x.picture).FirstOrDefault();
            
            else
            {
                System.IO.File.Delete(Server.MapPath("~/pics/") + (from x in db.meal where x.Id == id select x.picture).FirstOrDefault());
                if (UploadImage.ContentLength > 0)
                {
                    string filename = unique() + Path.GetExtension(UploadImage.FileName);
                    string folder = Path.Combine(Server.MapPath("~/pics/"), filename);
                    UploadImage.SaveAs(folder);
                    mel.picture = filename;
                }
            }
            db.meal.AddOrUpdate(mel);
            db.SaveChanges();
            ViewBag.category_id = new SelectList(db.categories, "Id", "name");
            meal m = (from x in db.meal where x.Id == id select x).FirstOrDefault();
            ViewBag.message = "تم تحديث بيانات الوجبة";
            return View(m);
        }
        public ActionResult delete_offer(int? id)
        {
            if (id != null)
            {
                meal m = (from x in db.meal where x.Id == id select x).FirstOrDefault();
                m.discount_price = m.price;
                db.meal.AddOrUpdate(m);
                db.SaveChanges();
                
            }
            List<meal> lst = (from x in db.meal where x.price != x.discount_price select x).ToList();
            return View(lst);
        }
    }
}