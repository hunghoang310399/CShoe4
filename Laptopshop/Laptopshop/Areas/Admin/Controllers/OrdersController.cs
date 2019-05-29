using Laptopshop.DAO;
using Laptopshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laptopshop.Areas.Admin.Controllers
{
    public class OrdersController : Controller
    {
        CShoeEntities db = new CShoeEntities();
        public ActionResult ListHoaDon()
        {
            var user = Session["user"];
            if (user == null)
            {
                return RedirectToAction("LoginAD", "LoginAdmin");
            }
            return View(db.Orders.ToList().OrderBy(x => x.Id));
        }

        public ActionResult Edit(int id)
        {
            var user = Session["user"];
            if (user == null)
            {
                return RedirectToAction("LoginAD", "LoginAdmin");
            }
            Order hd = db.Orders.Find(id);
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
        public ActionResult Edit(Order ghcs , string Command)
        {
            if (ModelState.IsValid)
            {
                Order hd = db.Orders.Find(ghcs.Id);
                if (Command == "TT1")
                {
                    hd.DeliveryStatus = "Đã giao hàng và khách hàng đã thanh toán đủ";
                }

                if (Command == "TT2")
                {
                    hd.DeliveryStatus = "Giao hàng không thành công";
                }
                if (Command == "TT3")
                {
                    hd.DeliveryStatus = "Đã hủy đơn hàng";
                }

                db.SaveChanges();
                return RedirectToAction("ListHoaDon");
            }
            else
            {
                return View(ghcs);
            }
        }
        public ActionResult Details(int Id)
        {
            var user = Session["user"];
            if (user == null)
            {
                return RedirectToAction("LoginAD", "LoginAdmin");
            }
            var model = db.Orders.Find(Id);
            return View(model);
        }
    }
}