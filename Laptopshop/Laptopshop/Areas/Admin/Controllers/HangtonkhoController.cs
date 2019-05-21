using Laptopshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laptopshop.Areas.Admin.Controllers
{
    public class HangtonkhoController : Controller
    {
        // GET: Admin/Hangtonkho
        CShoeEntities db = new CShoeEntities();
        // GET: Admin/Thongke

        public ActionResult bySuppliers()
        {
            var user = Session["user"];
            if (user == null)
            {
                return RedirectToAction("LoginAD", "LoginAdmin");
            }
            var model = db.Products.GroupBy(p => p.Supplier).Select(g => new Report
            {
                Group = g.Key.Name,
                Count = g.Sum(p => p.Quantity),
                Sum = g.Sum(p => p.UnitPrice * p.Quantity),
                Min = g.Min(p => p.UnitPrice),
                Max = g.Max(p => p.UnitPrice),
                Avg = g.Average(p => p.UnitPrice)
            });
            return View(model);
        }
        public ActionResult byProducts()
        {
            var user = Session["user"];
            if (user == null)
            {
                return RedirectToAction("LoginAD", "LoginAdmin");
            }
            var model = db.Products.GroupBy(p => p.Name).Select(g => new Report
            {
                Group = g.Key,
                Count = g.Sum(p => p.Quantity),
                Sum = g.Sum(p => p.UnitPrice * p.Quantity),
                Min = g.Min(p => p.UnitPrice),
                Max = g.Max(p => p.UnitPrice),
                Avg = g.Average(p => p.UnitPrice)
            });
            return View(model);
        }
    }
}