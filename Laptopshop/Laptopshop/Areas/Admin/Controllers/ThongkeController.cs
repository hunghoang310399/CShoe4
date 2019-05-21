using Laptopshop.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laptopshop.Areas.Admin.Controllers
{
    public class ThongkeController : Controller
    {
        CShoeEntities db = new CShoeEntities();
        // GET: Admin/Thongke
       
        public ActionResult byProduct(DateTime? From=null, DateTime? To=null)
        {
            var user = Session["user"];
            if (user == null)
            {
                return RedirectToAction("LoginAD", "LoginAdmin");
            }
           
            if (From == null)
            {
                From = DateTime.MinValue;
            }
            if (To == null)
            {
                To = DateTime.MaxValue;
            }
            var model = db.OrderDetails
                .Where(p=>p.Order.OrderDate>=From && p.Order.OrderDate<=To)
                .GroupBy(p => p.Product)
                .Select(g => new Report
            {
                Group = g.Key.Name,
                Count = g.Sum(p => p.Quantity),
                Sum = g.Sum(p => p.UnitPrice * p.Quantity*(1-p.Discount)),
                Min = g.Min(p => p.UnitPrice),
                Max = g.Max(p => p.UnitPrice),
                Avg = g.Average(p => p.UnitPrice)
            });
            return View(model);
        }
        public ActionResult bySupplier(DateTime? From = null, DateTime? To = null)
        {
            var user = Session["user"];
            if (user == null)
            {
                return RedirectToAction("LoginAD", "LoginAdmin");
            }

            if (From == null)
            {
                From = DateTime.MinValue;
            }
            if (To == null)
            {
                To = DateTime.MaxValue;
            }
            var model = db.OrderDetails
                .Where(p => p.Order.OrderDate >= From && p.Order.OrderDate <= To)
                .GroupBy(p => p.Product.Supplier)
                .Select(g => new Report
                {
                    Group = g.Key.Name,
                    Count = g.Sum(p => p.Quantity),
                    Sum = g.Sum(p => p.UnitPrice * p.Quantity * (1 - p.Discount)),
                    Min = g.Min(p => p.UnitPrice),
                    Max = g.Max(p => p.UnitPrice),
                    Avg = g.Average(p => p.UnitPrice)
                });
            return View(model);
        }
        public ActionResult byCustomer(DateTime? From = null, DateTime? To = null)
        {
            var user = Session["user"];
            if (user == null)
            {
                return RedirectToAction("LoginAD", "LoginAdmin");
            }
            if (From == null)
            {
                From = DateTime.MinValue;
            }
            if (To == null)
            {
                To = DateTime.MaxValue;
            }
            var model = db.OrderDetails
                 .Where(p => p.Order.OrderDate >= From && p.Order.OrderDate <= To)
                .GroupBy(p => p.Order.Customer)
                .Select(g => new Report
                {
                    Group = g.Key.Fullname,
                    Count = g.Sum(p => p.Quantity),
                    Sum = g.Sum(p => p.UnitPrice * p.Quantity * (1 - p.Discount)),
                    Min = g.Min(p => p.UnitPrice),
                    Max = g.Max(p => p.UnitPrice),
                    Avg = g.Average(p => p.UnitPrice)
                });
            return View( model);
        }
        public ActionResult Year()
        {
            var user = Session["user"];
            if (user == null)
            {
                return RedirectToAction("LoginAD", "LoginAdmin");
            }
            
            var model = db.OrderDetails
                .GroupBy(p => p.Order.OrderDate.Year)
                .Select(g => new Report
                {
                    yGroup = g.Key,
                    Count = g.Sum(p => p.Quantity),
                    Sum = g.Sum(p => p.UnitPrice * p.Quantity * (1 - p.Discount)),
                    Min = g.Min(p => p.UnitPrice),
                    Max = g.Max(p => p.UnitPrice),
                    Avg = g.Average(p => p.UnitPrice)
                });
            return View( model);
        }
        public ActionResult Weekly( DateTime? date)
        {
            //var user = Session["user"];
            //if (user == null)
            //{
            //    return RedirectToAction("LoginAD", "LoginAdmin");
            //}
            DateTime firstSunday = new DateTime(1753, 1, 7);
            DateTime now;
            if (date == null)
            {
                now = DateTime.Today;
                date = DateTime.Today;
            }
            else
            {
                now = date.Value;
            }

            DateTime weekly = now.AddDays(-7);
            ViewBag.Order = "From " + weekly + " to " + now;
            var model = db.OrderDetails
                .Where(p => p.Order.OrderDate >= weekly && p.Order.OrderDate <= date)
                .GroupBy(p=>p.Order.OrderDate)
                .Select(g => new Report
                {
                    weekly = g.Key,
                    Count = g.Sum(p => p.Quantity),
                    Sum = g.Sum(p => p.UnitPrice * p.Quantity * (1 - p.Discount)),
                    Min = g.Min(p => p.UnitPrice),
                    Max = g.Max(p => p.UnitPrice),
                    Avg = g.Average(p => p.UnitPrice)
                });
            return View(model);
        }
        public ActionResult Month(int? year)
        {
            var user = Session["user"];
            if (user == null)
            {
                return RedirectToAction("LoginAD", "LoginAdmin");
            }
            if (year == null)
            {
                year = DateTime.Now.Year;
            }
            ViewBag.year = year;
            var model = db.OrderDetails
                 .Where(p => p.Order.OrderDate.Year==year)
                .GroupBy(p => p.Order.OrderDate.Month)
                .Select(g => new Report
                {
                  
                    yGroup = g.Key,
                    Count = g.Sum(p => p.Quantity),
                    Sum = g.Sum(p => p.UnitPrice * p.Quantity * (1 - p.Discount)),
                    Min = g.Min(p => p.UnitPrice),
                    Max = g.Max(p => p.UnitPrice),
                    Avg = g.Average(p => p.UnitPrice)
                }).OrderBy(y=> y.yGroup);
            return View(model);
            
        }
        public ActionResult Qui(int? year)
        {
            var user = Session["user"];
            if (user == null)
            {
                return RedirectToAction("LoginAD", "LoginAdmin");
            }

            if (year == null)
            {
                year = DateTime.Now.Year;
            }
            ViewBag.year = year;
            var model = db.OrderDetails
                 .Where(p => p.Order.OrderDate.Year == year)
                .GroupBy(p => (p.Order.OrderDate.Month-1)/3+1)
                .Select(g => new Report
                {
                    yGroup = g.Key,
                    Count = g.Sum(p => p.Quantity),
                    Sum = g.Sum(p => p.UnitPrice * p.Quantity * (1 - p.Discount)),
                    Min = g.Min(p => p.UnitPrice),
                    Max = g.Max(p => p.UnitPrice),
                    Avg = g.Average(p => p.UnitPrice)
                }).OrderBy(y => y.yGroup);
            return View(model);
        }
    }
}