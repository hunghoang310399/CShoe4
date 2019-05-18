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