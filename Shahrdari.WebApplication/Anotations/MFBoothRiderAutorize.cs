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
    public class MFBoothRiderAutorize : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.Cookies[MFCookies.BOOTH_RIDER_KEY] != null)
            {
                string key = filterContext.HttpContext.Request.Cookies[MFCookies.BOOTH_RIDER_KEY].Value;
                M_Personels user = null;
                var ub = new B_Personels();
                try
                {
                    user = ub.GetPersonels(key);
                }
                catch { }

                if (user != null)
                    return;
            }
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Default", action = "Login", area = "Booth" }));
            filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
        }
    }
}