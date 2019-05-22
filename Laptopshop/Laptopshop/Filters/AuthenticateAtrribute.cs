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
                var Uri = HttpContext.Current.Request.Url.AbsolutePath;
                HttpContext.Current.Session["RequestUrl"] = Uri;

                var url = new UrlHelper(HttpContext.Current.Request.RequestContext);
                HttpContext.Current.Response.Redirect(url.Action("Login", "Account"));
            }
            
        }
    }
}