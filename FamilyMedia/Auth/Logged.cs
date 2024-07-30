using FamilyMedia.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FamilyMedia.Auth
{
    public class Logged : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = (User)httpContext.Session["user"];
            if (user != null && user.Type.Equals("User"))
            {
                return true;
            }
            return false;
        }
    }
}