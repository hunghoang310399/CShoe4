using Laptopshop.DAO;
using Laptopshop.Models;
using System.Linq;
using PagedList;
using System.Web.Mvc;
using System.Collections.Generic;
using Laptopshop.Bean;
using Facebook;
using System.Web.Security;
using System;

namespace Laptopshop.Controllers
{

    public class HomeController : BaseController
    {

        private const string CartSession = "cart";
        public static SanPhamDAO sp = new SanPhamDAO();
        IQueryable<Product> ListSanPham = sp.ListSP();
       
        public ActionResult Index(int? trang)
        {
            //return View(ListSanPham);
            Session["BackUrl"] = "/Home/Index/";
            int sosptrentrang = 8;
            int stttrang = (trang ?? 1);
            return View(db.Products.ToList().OrderBy(x => x.Id).ToPagedList(stttrang, sosptrentrang));
        }


        private Uri RediredtUri

        {

            get

            {

                var uriBuilder = new UriBuilder(Request.Url);

                uriBuilder.Query = null;

                uriBuilder.Fragment = null;

                uriBuilder.Path = Url.Action("FacebookCallback");

                return uriBuilder.Uri;

            }

        }




        [AllowAnonymous]

        public ActionResult Facebook()

        {

            var fb = new FacebookClient();

            var loginUrl = fb.GetLoginUrl(new

            {




                client_id = "919134721597050",

                client_secret = "48322d66a0188fb4de97b85a4d0be9fc",

                redirect_uri = RediredtUri.AbsoluteUri,

                response_type = "code",

                scope = "email"



            });

            return Redirect(loginUrl.AbsoluteUri);

        }
        public ActionResult FacebookCallback(string code)

        {
            var fb = new FacebookClient();

            dynamic result = fb.Post("oauth/access_token", new

            {

                client_id = "919134721597050",

                client_secret = "48322d66a0188fb4de97b85a4d0be9fc",

                redirect_uri = RediredtUri.AbsoluteUri,

                code = code
            });

            var accessToken = result.access_token;

            Session["AccessToken"] = accessToken;

            fb.AccessToken = accessToken;

            dynamic me = fb.Get("me?fields=link,first_name,currency,last_name,email,gender,locale,timezone,verified,picture,age_range");

            string email = me.email;
            string userName = me.email;
            string firstname = me.first_name;
            string middlename = me.middle_name;
            string lastname = me.last_name;
            Session["email"] = me.email;
           
            var user = new Customer();
            user.Email = email;
            user.Id = firstname;
            user.Activated = true;
            user.Fullname = firstname + " " + lastname;
            user.Password = "tainganh";
            user.Photo = "User.png";
            var model = db.Customers.SingleOrDefault(x => x.Id == user.Id);
            if (model == null)
            {
                db.Customers.Add(user);
                Session["facebook"] = user;
                Session["first_name"] = me.first_name;

                Session["lastname"] = me.last_name;

                Session["picture"] = me.picture.data.url;
                db.SaveChanges();
            }

            Session["facebook"] = user;
            Session["first_name"] = me.first_name;

            Session["lastname"] = me.last_name;

            Session["picture"] = me.picture.data.url;
            FormsAuthentication.SetAuthCookie(email, false);
            var RequestUrl = Session["RequestUrl"] as String;
            if (RequestUrl != null)
            {
                return Redirect(RequestUrl);
            }
            
            return RedirectToAction("Index", "Home");

        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        //public ActionResult _Category()
        //{
        //    var model = db.Categories.ToList();
        //    return PartialView(model);
        //}
        public ActionResult _Supplier()
        {
            var model = db.Suppliers.ToList();
            return PartialView(model);
        }
        public ActionResult _Supp()
        {
            var model = db.Suppliers.ToList();
            return PartialView(model);
        }

    }
}