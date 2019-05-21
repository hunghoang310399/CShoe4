using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc;
using Laptopshop.Models;
using PagedList;

namespace Laptopshop.Controllers
{
    public class FeedBackController : Controller
    {
        CShoeEntities db = new CShoeEntities();
        // GET: FeedBack
        [HttpGet]
        public ActionResult SendFeedback()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SendFeedback(Feedback model )
        {
            var fb = new Feedback();
            try
            {
                db.Feedbacks.Add(model);
                db.SaveChanges();
                ViewBag.message = "Gữi phản hồi thành công , chúng tôi sẽ liên hệ với bạn thời gian sớm nhất có thể ! ";
            }
            catch
            {

                ModelState.AddModelError("", "Gữi phản hồi thất bại !");
        
            }
              
            return View();

        }
    }
}