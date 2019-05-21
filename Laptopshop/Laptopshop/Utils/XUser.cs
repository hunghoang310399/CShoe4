using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class XUser
{
    public static bool Authenticated
    {
        get
        {
            var user = HttpContext.Current.Session["User"];
            return user != null;
        }
    }
    public static bool AuthenticatedFacebook
    {
        get
        {
            var user = HttpContext.Current.Session["AccessToken"];
            return user != null;
        }
    }
}