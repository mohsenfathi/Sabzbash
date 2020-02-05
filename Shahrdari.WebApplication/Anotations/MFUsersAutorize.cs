using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Shahrdari.Models;
using Shahrdari.BussinessLayer;
using Shahrdari.Enums;
using Shahrdari.WebApplication.Classes.Enums;
using System.Reflection;

namespace Shahrdari.WebApplication.Anotations
{
    public class MFUsersAutorize : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.Cookies[MFCookies.END_USER_KEY] != null)
            {
                string key = filterContext.HttpContext.Request.Cookies[MFCookies.END_USER_KEY].Value;
                M_Users user = null;
                B_Users ub = new B_Users();
                try
                {
                    user = ub.GetUsersByToken(key);
                }
                catch { }

                if (user != null)
                    return;
            }
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Index", area = "User" }));
            filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
        }
    }
}