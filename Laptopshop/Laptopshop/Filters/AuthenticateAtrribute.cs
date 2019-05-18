using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Laptopshop.Filters
{
    public class AuthenticateAttribute: ActionFilterAttribute
    {
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
           
            if (!XUser.Authenticated && !XUser.AuthenticatedFacebook)
            {
                var Uri = HttpContext.Current.Request.Url.AbsoluteUri;
                HttpContext.Current.Session["RequestUrl"] = Uri;
                HttpContext.Current.Response.Redirect("/Account/Login");
            }
            
        }
    }
}