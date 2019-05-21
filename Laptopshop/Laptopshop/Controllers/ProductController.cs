using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Laptopshop.Models;
using PagedList;

namespace Laptopshop.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }
        #region ListbyCategory
        //public ActionResult ListByCategory(int Id, int? trang)
        //{
        //    Session["BackUrl"] = "/Product/ListByCategory/" + Id;
        //    int sosptrentrang = 6;
        //    int stttrang = (trang ?? 1);
        //    var model = db.Products.Where(p => p.CategoryId == Id).ToList().OrderBy(x => x.Id).ToPagedList(stttrang, sosptrentrang);
        //    return View("List", model);
        //}
        public ActionResult ListBySupplier(String Id, int? trang)
        {
            Session["BackUrl"] = "~/Product/ListBySupplier/" + Id;
            int sosptrentrang = 6;
            int stttrang = (trang ?? 1);
            var model = db.Products.Where(p => p.SupplierId == Id).ToList();
            return View("List", model.OrderBy(x => x.Id).ToPagedList(stttrang, sosptrentrang));
        }
        #endregion

        #region ListByspecial
        public ActionResult ListBySpecial(String Id, int? trang)
        {
            int sosptrentrang = 6;
            int stttrang = (trang ?? 1);
            Session["BackUrl"] = "~/Product/ListBySpecial/" + Id;
            List<Product> model;
            switch (Id)
            {
                case "BEST":
                    model = db.Products
                        .OrderByDescending(p => p.OrderDetails.Sum(d => d.Quantity))
                        .Take(15)
                        .ToList();
                    break;
                case "LATEST":
                    model = db.Products.Where(p => p.Latest).ToList();
                    break;
                case "SPECIAL":
                    model = db.Products
                        .Where(p => p.Special)
                        .ToList();
                    break;
                case "VIEWS":
                    model = db.Products
                        .Where(p => p.Views > 0)
                        .OrderByDescending(p => p.Views)
                        .Take(12).ToList();
                    break;
                case "PROMO":
                    model = db.Products
                        .Where(p => p.Discount > 0)
                        .OrderByDescending(p => p.Discount)
                        .ToList();
                    break;
                default:
                    model = db.Products.ToList();
                    break;
            }
            return View("Specials", model.ToPagedList(stttrang, sosptrentrang));
        }
        #endregion
        #region Search
        public ActionResult Search(String Keywords, int? trang)
        {
            @ViewBag.Key = Keywords;
            Session["BackUrl"] = "~/Product/Search?Keywords=" + Keywords;
            int sosptrentrang = 6;
            int stttrang = (trang ?? 1);
            var model = db.Products
                .Where(p => p.Name.Contains(Keywords) ||
                    p.Supplier.Name.Contains(Keywords))
                .ToList().ToPagedList(stttrang, sosptrentrang); ;

            return View("Search1", model);
        }
        #endregion
        #region Detail
        public ActionResult Detail(int Id)
        {
            //Ghi nhớ sản phẩm đã xem
            var cookie = Request.Cookies["Views"];
            if (cookie == null)
            {
                cookie = new HttpCookie("Views");
            }
            cookie.Values[Id.ToString()] = Id.ToString();
            cookie.Expires = DateTime.Now.AddMonths(1);
            Response.Cookies.Add(cookie);

            var Ids = cookie.Values.AllKeys
                .Select(k => int.Parse(k))
                .ToList();
            ViewBag.Views = db.Products
                .Where(p => Ids.Contains(p.Id))
                .ToList();

            var model = db.Products.Find(Id);
            return View(model);
        }
        #endregion

    }
}