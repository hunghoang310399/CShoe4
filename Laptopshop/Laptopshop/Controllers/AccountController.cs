using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laptopshop.Models;
using System.Data.Entity;
using Laptopshop.Filters;

namespace Laptopshop.Controllers
{
    public class AccountController : BaseController
    {
        public ActionResult Login()
        {
            var cookie = Request.Cookies["User"];
            if (cookie != null)
            {
                ViewBag.Id = cookie.Values["Id"];
                ViewBag.Password = cookie.Values["Password"];
                ViewBag.Remember = true;
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(String Id, String Password, bool Remember)
        {
            var user = db.Customers.Find(Id);
            if (user == null)
            {
                ViewBag.message = "Tên sử dụng không hợp lệ";
            }
            else if (user.Password != Password)
            {
                ViewBag.message = "Mật khẩu sử dụng không hợp lệ";
            }
            else if (user.Activated == false)
            {
                ViewBag.message = "Tài khoản không được kích hoạt !";
                //ModelState.AddModelError("", "Account is not activated !");
            }
            else
            {
                //ModelState.AddModelError("", "Login Sucessfully!");
                Session["User"] = user;
                Session["Id"] = Id;



                // Xu ly Remember me
                var cookie = new HttpCookie("User");
                if (Remember == true)
                {
                    cookie.Values["Id"] = Id;
                    cookie.Values["Password"] = Password;
                    cookie.Expires = DateTime.Now.AddMonths(1);
                }
                else
                {
                    cookie.Expires = DateTime.Now;
                }
                Response.Cookies.Add(cookie);

                //
                var RequestUrl = Session["RequestUrl"] as String;
                if (RequestUrl != null)
                {
                    return Redirect(RequestUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            
            return View();
        }
        [Authenticate]
        public ActionResult Logout()
        {
            Session.Remove("AccessToken");
            Session.Remove("User");
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Register()
        {
            var model = new Customer();
            model.Activated = false;
            return View(model);
        }
        [HttpPost]
        public ActionResult Register(Customer model)
        {
            try
            {
                // upload hinh
                var f = Request.Files["UpPhoto"];
                if (f.ContentLength > 0)
                {

                    model.Photo = DateTime.Now.Ticks + "-" + f.FileName;
                    var path = "~/images/Customers/" + model.Photo;
                    f.SaveAs(Server.MapPath(path));
                }
                else
                {
                    model.Photo = "";
                }
                // dawng ky
                db.Customers.Add(model);
                db.SaveChanges();
                //ModelState.AddModelError("", "Register successfully !");
                ViewBag.message = "Đăng ký thành công! Vui lòng truy cập email để kích hoạt tài khoản của bạn.";
                // Send welcome mail
                var Uri = Request.Url.AbsoluteUri.Replace("Register", "Activate/" + model.Id.ToBase64());
                String body = "kích hoạt tài khoản của bạn <a href='" + Uri + "'>Kích hoạt</a>";
                XMail.Send(model.Email, "Thư chào mừng", body);
            }
            catch
            {
                ModelState.AddModelError("", "Đăng ký thất bại !");
            }
            return View();
        }
        public ActionResult Activate(String Id)
        {
            var user = db.Customers.Find(Id.FromBase64());
            user.Activated = true;
            db.SaveChanges();
            return RedirectToAction("Login");
        }

        public ActionResult Forgot()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Forgot(String Id, String Email)
        {
            var user = db.Customers.Find(Id);
            if (user == null)
            {
                ViewBag.message = "Tên sử dụng không hợp lệ";
                //ModelState.AddModelError("", "Invalid username");
            }
            else if (Email != user.Email)
            {
                ViewBag.message = "Email sử dụng không hợp lệ";
            }
            else
            {
                ViewBag.message = "Mật khẩu của bạn đã được gửi đến hộp thư đến của bạn";
               
                XMail.Send(Email, "Quên mật khẩu", "Mật khẩu:" + user.Password);
            }
            return View();
        }
        [Authenticate]
        public ActionResult Change()
        {
            return View();
        }
        [Authenticate]
        [HttpPost]
        public ActionResult Change(String Id, String CurrentPassword, String NewPassword, String ConfirmNewPassword)
        {
            if (NewPassword != ConfirmNewPassword)
            {
                ViewBag.message = "Xác nhận mật khẩu mới không khớp";
                //ModelState.AddModelError("", "Confirm new password is not match");
            }
            else
            {
                var user = db.Customers.Find(Id);
                if (user == null)
                {
                    ViewBag.message = "Tên sử dụng không hợp lệ";
                   
                }
                else if (user.Password != CurrentPassword)
                {
                    ViewBag.message = "Mật khẩu hiện tại không hợp lệ";
                   
                }
                else
                {
                    user.Password = NewPassword;
                    db.SaveChanges();
                    ViewBag.message = "Thay đổi mật khẩu thành công";
                    //ModelState.AddModelError("", "Change password successfully");
                }
            }


            return View();
        }
        [Authenticate]
        public ActionResult Edit()
        {
            var model = Session["User"] as Customer;
            return View(model);
        }
        [Authenticate]
        [HttpPost]
        public ActionResult Edit(Customer model)
        {
            // upload hinh
            var f = Request.Files["UpPhoto"];
            if (f.ContentLength > 0)
            {
                if (model.Photo != "User.png")
                {
                    var path = "~/Images/Customers/" + model.Photo;
                    System.IO.File.Delete(Server.MapPath(path));
                }
                model.Photo = DateTime.Now.Ticks + "-" + f.FileName;
                var newPath = "~/Images/Customers/" + model.Photo;
                f.SaveAs(Server.MapPath(newPath));
            }
            // cap nhat
            try
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                Session["User"] = model;
                ViewBag.message = "Cập nhật thành công!";
                //ModelState.AddModelError("", "");
            }
            catch
            {
                ViewBag.message = "Cập nhật thất bại!";
                
            }
            return View(model);
        }

    }
}