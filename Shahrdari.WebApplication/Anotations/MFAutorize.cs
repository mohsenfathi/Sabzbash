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
    public class MFAutorize : ActionFilterAttribute
    {
        string _actionName, _controllerName;

        /// <summary>
        /// سازنده پیفرض
        /// </summary>
        /// <param name="ActionName">نام اکشن</param>
        /// <param name="ControllerName">نام کنترلر</param>
        public MFAutorize(string ActionName, string ControllerName)
        {
            _actionName = ActionName;
            _controllerName = ControllerName;
        }

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get { return _controllerName + " / " + _actionName; } }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            M_Personels user = null;
            M_PersonelRoles role = null;
            List<M_PersonelRoleValues> roleValue = null;

            if (filterContext.HttpContext.Request.Cookies[MFCookies.USER_KEY] != null)
            {
                string key = filterContext.HttpContext.Request.Cookies[MFCookies.USER_KEY].Value;
                B_Personels ub = new B_Personels();
                try
                {
                    user = ub.GetPersonels(key);
                }
                catch { }

                if (user == null)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Index", area = "Admin" }));
                    filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                }
                else
                {
                    B_PersonelRoles ru = new B_PersonelRoles();
                    role = ru.GetPersonelRoles(user.PersonelRoleId);
                    if (role == null)
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Dashboard", action = "NoPermission", area = "Admin" }));
                        filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                    }

                    if (role.HasFullControl == true)
                        return;

                    B_PersonelRoleValues bRoleValue = new B_PersonelRoleValues();
                    roleValue = bRoleValue.GetPersonelRoleValues(role.Id);
                    if (roleValue == null || roleValue.Count == 0)
                    {
                        if (role == null)
                        {
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Dashboard", action = "NoPermission", area = "Admin" }));
                            filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                        }
                    }

                    var rd = filterContext.HttpContext.Request.RequestContext.RouteData;
                    string currentAction = rd.GetRequiredString("action");
                    string currentController = rd.GetRequiredString("controller") + "Controller";


                    Assembly asm = Assembly.GetAssembly(typeof(Global));
                    var action = asm.GetTypes()
                            .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type) && type.Namespace.Contains("Shahrdari.WebApplication.Areas.Admin.Controllers"))
                            .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                            .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any()
                            && m.DeclaringType.Name == currentController && m.Name == currentAction)
                            .Select(x => new { Attributes = x.GetCustomAttributes().ToList() }).FirstOrDefault();
                        if (action != null && action.Attributes.Where(c => c.GetType().Name.ToUpper().IndexOf("HTTPPOST") != -1).Count() > 0)
                            return;

                    if (roleValue.Where(c => c.AccessName.ToUpper() == (currentController + " | " + currentAction).ToUpper()).Count() == 0)
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Dashboard", action = "NoPermission", area = "Admin" }));
                        filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
                    }
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Index", area = "Admin" }));
                filterContext.Result.ExecuteResult(filterContext.Controller.ControllerContext);
            }
        }
    }
}