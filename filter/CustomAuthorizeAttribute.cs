using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace College_Management_System.filter
{
    public class CustomAuthorizeAttribute:AuthorizeAttribute
    {
        private readonly string[] allowRoles;
        public CustomAuthorizeAttribute(params string[] roles)
        {
            this.allowRoles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if(!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }
            string UserRole = httpContext.Session["UserRole"].ToString();
            return allowRoles.Contains(UserRole);
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/home/login");
        }
    }
}