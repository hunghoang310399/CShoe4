using Laptopshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laptopshop.Areas.Admin.Controllers
{
    public class LoginAdminController : Controller
    {
        CShoeEntities db = new CShoeEntities();
        // GET: Admin/LoginAdmin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoginAD()
        {

            return View();
        }
        [HttpPost]
        public ActionResult LoginAD(string username, string Password)
        {
            var user = db.Roles.Find(username);
            Session["username"] = user.Id;
            if (user.Id != username || user.Name != Password)
            {
                ViewBag.message = "Tài khoản không hợp lệ";
            }
            else
            {

                ViewBag.message = "Thành công";


                Session["user"] = user;

                return RedirectToAction("Index", "HomeAd");

            }



            return View();

        }
        public ActionResult Logout()
        {
            Session.Remove("user");
            return RedirectToAction("LoginAD","Admin/LoginAdmin");

        }
    }
}