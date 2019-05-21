using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook;
using Laptopshop.Filters;
using Laptopshop.Models;

namespace Laptopshop.Controllers
{
    [Authenticate]
    public class OrderController : BaseController
    {
        public ActionResult Detail(int Id)
        {
            var model = db.Orders.Find(Id);
            return View(model);
        }
        public ActionResult Checkout()
        {
            var model = new Order();
            if (XUser.Authenticated) {
                var user = Session["User"] as Customer;
               
                model.OrderDate = DateTime.Now;
                model.Amount = ShoppingCart.Cart.Amount;
                model.CustomerId = user.Id;
                model.Receiver = user.Fullname;
                model.RequireDate = DateTime.Now.AddDays(2);
                model.DeliveryStatus = "Delivering";
              
            }
            else{
                if (XUser.AuthenticatedFacebook)
                {
                    var user = Session["facebook"] as Customer;
                   
                    model.OrderDate = DateTime.Now;
                    model.Amount = ShoppingCart.Cart.Amount;
                    model.CustomerId = user.Id;
                    model.Receiver = user.Fullname;
                    model.RequireDate = DateTime.Now.AddDays(2);
                    model.DeliveryStatus = "Delivering";
                   
                }
                
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Checkout(Order model)
        {
            if (model.Address == null)
            {
                ViewBag.Address = "Vui lòng nhập thông tin để giao hàng";
            }
            var user = Session["User"] as Customer;
            try
            {
                db.Orders.Add(model);
                foreach (var p in ShoppingCart.Cart.Items)
                {
                    var detail = new OrderDetail
                    {
                        Order = model,
                        ProductId = p.Id,
                        Quantity = p.Quantity,
                        UnitPrice = p.UnitPrice,
                        Discount = p.Discount
                    };
                    db.OrderDetails.Add(detail);
                }
                db.SaveChanges();

                ModelState.AddModelError("", "Đặt hàng thành công!");
                ShoppingCart.Cart.Clear();
                return RedirectToAction("Detail", new { Id = model.Id });
            }
            catch
            {
                ModelState.AddModelError("", "Đặt hàng thất bại!");
            }

            return View(model);
        }
        public ActionResult List()
        {

            var user = Session["User"] as Customer;
            var model = db.Orders
                .Where(o => o.CustomerId == user.Id)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
            return View(model);
        }
        public ActionResult ListFacebook()
        {

            var facebook = Session["first_name"];
            var model = db.Orders
                .Where(o => o.CustomerId == facebook.ToString())
                .OrderByDescending(o => o.OrderDate)
                .ToList();
            return View("List", model);
        }
        public ActionResult Items()
        {
            var user = Session["User"] as Customer;

            var model = db.OrderDetails
                .Where(d => d.Order.CustomerId == user.Id)
                .GroupBy(d => d.Product)
                .Select(g => new { Product = g.Key, Count = g.Count() })
                .OrderByDescending(gg => gg.Count)
                .Select(p => p.Product)
                .ToList();
            return View("../Product/Ordered", model);
        }

        public ActionResult ItemsFacebook()
        {
            var facebook = Session["first_name"];

            var model = db.OrderDetails
                .Where(d => d.Order.CustomerId == facebook.ToString())
                .GroupBy(d => d.Product)
                .Select(g => new { Product = g.Key, Count = g.Count() })
                .OrderByDescending(gg => gg.Count)
                .Select(p => p.Product)
                .ToList();
            return View("../Product/Ordered", model);
        }
    }
}