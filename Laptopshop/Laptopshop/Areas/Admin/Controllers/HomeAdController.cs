using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laptopshop.Areas.Admin.Controllers
{
    public class HomeAdController : Controller
    {
        // GET: Admin/HomeAd
        public ActionResult Index()
        {
            var user = Session["user"];
            if (user == null)
            {
                return RedirectToAction("LoginAD", "LoginAdmin");
            }
            return View();
        }
    }
}