using FFR.Models;
using System;
using System.Collections.Generic;
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
        public ActionResult add()
        {
            return View();
        }
        public ActionResult delete(int? id)
        {
            if(id != null)
            {
                db.meal.Remove((from x in db.meal where x.Id == id select x).FirstOrDefault());
                db.SaveChanges();
            }
            List<meal> lst = (from x in db.meal select x).ToList();
            return View(lst);

        }
    }
}