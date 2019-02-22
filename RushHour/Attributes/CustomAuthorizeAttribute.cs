using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RushHour.Enums;
using RushHour.Helpers;

namespace RushHour.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class CustomAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        private CustomAuthorizeEnum AllowAccessTo { get; set; }

        public CustomAuthorizeAttribute(CustomAuthorizeEnum accessLevel)
        {
            this.AllowAccessTo = accessLevel;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any()
                || filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any())
            {
                return;
            }

            if (LoginUserSession.Current.IsAuthenticated)
            {
                if (this.AllowAccessTo.HasFlag(CustomAuthorizeEnum.Everyone))
                {
                    return;
                }

                if (!this.AllowAccessTo.HasFlag(CustomAuthorizeEnum.Admin) && LoginUserSession.Current.IsAdmin)
                {
                    filterContext.Controller.TempData["ErrorMessage"] = "You don't have rights to access this page";
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Home", action = "Index" }));
                    return;
                }

                if (!this.AllowAccessTo.HasFlag(CustomAuthorizeEnum.NonAdmin) && !LoginUserSession.Current.IsAdmin)
                {
                    filterContext.Controller.TempData["ErrorMessage"] = "You don't have rights to access this page";
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Home", action = "Index" }));
                    return;
                }
            }
            else
            {
                if (this.AllowAccessTo.HasFlag(CustomAuthorizeEnum.AnonymousUser) || AllowAccessTo.HasFlag(CustomAuthorizeEnum.Everyone))
                {
                    return;
                }
                else
                {
                    filterContext.Controller.TempData["ErrorMessage"] = "You have to be logged in in order to access this page";
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Account", action = "Login" }));
                    return;
                }
            }
        }
    }
}