using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Laptopshop.Models;

namespace Laptopshop.Areas.Admin.Controllers
{
    public class SuppliersController : Controller
    {
        private CShoeEntities db = new CShoeEntities();

        // GET: Admin/Suppliers
        public ActionResult Index()
        {
            var user = Session["user"];
            if (user == null)
            {
                return RedirectToAction("LoginAD", "LoginAdmin");
            }
            return View(db.Suppliers.ToList());
        }

        // GET: Admin/Suppliers/Details/5
        public ActionResult Details(string id)
        {
            var user = Session["user"];
            if (user == null)
            {
                return RedirectToAction("LoginAD", "LoginAdmin");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // GET: Admin/Suppliers/Create
        public ActionResult Create()
        {
            var user = Session["user"];
            if (user == null)
            {
                return RedirectToAction("LoginAD", "LoginAdmin");
            }
            var model = new Supplier();
            
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(Supplier model)
        {
            try
            {
                // upload hinh
                var f = Request.Files["UpPhoto"];
                if (f.ContentLength > 0)
                {

                    model.Logo = DateTime.Now.Ticks + "-" + f.FileName;
                    var path = "~/images/suppliers/" + model.Logo;
                    f.SaveAs(Server.MapPath(path));
                }
                else
                {
                    model.Logo = "Logo.png";
                }
                // dawng ky
                db.Suppliers.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index", "Suppliers");

                // Send welcome mail

            }
            catch
            {
                ModelState.AddModelError("", "Insert fails !");
            }
            return View(model);
        }
        // GET: Admin/Suppliers/Edit/5
        public ActionResult Edit(string id)
        {
            var user = Session["user"];
            if (user == null)
            {
                return RedirectToAction("LoginAD", "LoginAdmin");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: Admin/Suppliers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Logo,Email,Phone")] Supplier supplier)
        {
            try
            {
                // upload hinh
                var f = Request.Files["UpPhoto"];
                if (f.ContentLength > 0)
                {

                    supplier.Logo = DateTime.Now.Ticks + "-" + f.FileName;
                    var path = "~/images/suppliers/" + supplier.Logo;
                    f.SaveAs(Server.MapPath(path));
                }
                else
                {
                    supplier.Logo = "Logo.png";
                }
                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("", "Error");
            }
            return View(supplier);
        }

        // GET: Admin/Suppliers/Delete/5
        public ActionResult Delete(string id)
        {
            var user = Session["user"];
            if (user == null)
            {
                return RedirectToAction("LoginAD", "LoginAdmin");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: Admin/Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Supplier supplier = db.Suppliers.Find(id);
            db.Suppliers.Remove(supplier);
            db.SaveChanges();
            return RedirectToAction("Index" , "Suppliers");
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
