using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Laptopshop.Models;
using PagedList;

namespace Laptopshop.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private CShoeEntities db = new CShoeEntities();

        // GET: Admin/Products
        public ActionResult Index()
        {
            var user = Session["user"];
            if (user == null)
            {
                return RedirectToAction("LoginAD", "LoginAdmin");
            }

            var products = db.Products.Include(p => p.Supplier);
            return View(products.ToList());
        }

        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name");
            ViewBag.product_type = new SelectList(db.Sizes, "IDSize", "Size");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Product model)
        {
            try
            {
                // upload hinh
                var f = Request.Files["UpPhoto"];
                if (f.ContentLength > 0)
                {

                    model.Image = DateTime.Now.Ticks + "-" + f.FileName;
                    var path = "~/images/products/" + model.Image;
                    f.SaveAs(Server.MapPath(path));
                }
                else
                {
                    model.Image = "product.png";
                }
                // dawng ky
                db.Products.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index", "Products");

                // Send welcome mail

            }
            catch
            {
                ModelState.AddModelError("", "Chèn thất bại !");
            }
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", model.SupplierId);
            ViewBag.product_type = new SelectList(db.Sizes, "IDSize", "Size", model.IDSize);
            return View(model);
        }
            // GET: Admin/Products/Edit/5
            public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", product.SupplierId);
            ViewBag.product_type = new SelectList(db.Sizes, "IDSize", "Size", product.IDSize);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,Name,UnitBrief,UnitPrice,Image,ProductDate,Available,Description,SupplierId,Quantity,Discount,Special,Latest,Views")] Product product)
        {
            try
            {
                // upload hinh
                var f = Request.Files["UpPhoto"];
                if (f.ContentLength > 0)
                {

                    product.Image = DateTime.Now.Ticks + "-" + f.FileName;
                    var path = "~/images/products/" + product.Image;
                    f.SaveAs(Server.MapPath(path));
                }
                else
                {
                    product.Image = "product.png";
                }
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("", "Error");
            }
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", product.SupplierId);
            ViewBag.product_type = new SelectList(db.Sizes, "IDSize", "Size", product.IDSize);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
