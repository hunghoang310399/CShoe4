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
            return View(fb.ToList());
        }
    }
}