using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Laptopshop.Models;
using PagedList;

namespace Laptopshop.Areas.Admin.Controllers
{
    public class QLFeedBackController : Controller
    {
        private CShoeEntities db = new CShoeEntities();
        // GET: Admin/QLFeedBack
        public ActionResult Index()
        {
            var user = Session["user"];
            if (user == null)
            {
                return RedirectToAction("LoginAD", "LoginAdmin");
            }
            var fb = db.Feedbacks.ToList();
            return View(fb.ToList().OrderBy(x => x.IDFeedback));
        }
        public ActionResult Edit(int id)
        {
            var user = Session["user"];
            if (user == null)
            {
                return RedirectToAction("LoginAD", "LoginAdmin");
            }
            Feedback hd = db.Feedbacks.Find(id);
            if (hd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            else
            {
                return View(hd);
            }
        }

        [HttpPost]
        public ActionResult Edit(Feedback ghcs)
        {
            if (ModelState.IsValid)
            {
                Feedback hd = db.Feedbacks.Find(ghcs.IDFeedback);
                hd.FBStatus = "Đã xử lý";
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(ghcs);
            }
        }
    }
}