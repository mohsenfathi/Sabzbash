using Shahrdari.WebApplication.Anotations;
using Shahrdari.BussinessLayer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Shahrdari.WebApplication.Classes.Enums;
using Shahrdari.Models;
using Shahrdari.WebApplication.Classes.Models;
using Shahrdari.Enums;

namespace Shahrdari.WebApplication.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// کاربر جاری که لاگین کرده
        /// </summary>
        public M_Personels CurrentUser { get; set; }

        /// <summary>
        /// آدرس اتصال به SignalR
        /// </summary>
        // protected string SignalRUrl = System.Configuration.ConfigurationManager.AppSettings["SignalRUrl"];

        public BaseController()
        {
            if (System.Web.HttpContext.Current.Request.Cookies[MFCookies.USER_KEY] != null)
            {
                M_PersonelRoles role = null;
                string key = System.Web.HttpContext.Current.Request.Cookies[MFCookies.USER_KEY].Value;
                B_Personels ub = new B_Personels();
                CurrentUser = ub.GetPersonels(key);
                if (CurrentUser == null)
                    return;
                B_PersonelRoles ru = new B_PersonelRoles();
                role = ru.GetPersonelRoles(CurrentUser.PersonelRoleId);
                if (role == null)
                    return;
                ViewBag.LayoutPersonel = CurrentUser;
                ViewBag.LayoutPersonelRole = role;
                ViewBag.LayoutNewRequestCount = new B_ServicesRequests().GetServicesRequestsCount(E_PublicCategory.REQUEST_STATUS.NEW_REQUEST);
                ViewBag.LayoutNewRedrawalCount = new B_UserPayment().GetPaymentCuontByStatus(E_PublicCategory.PAYMENT_STATUS.NEW);
            }
        }

        /// <summary>
        /// دریافت اکشن های مورد نیاز برای اعتبارسنجی
        /// </summary>
        /// <returns></returns>
        protected List<Tuple<string, string>> getActions()
        {
            Assembly asm = Assembly.GetAssembly(typeof(Global));
            var controlleractionlist = asm.GetTypes()
                    .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type) && type.Namespace.Contains("Shahrdari.WebApplication.Areas.Admin.Controllers"))
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                    .Select(x => new { Controller = x.DeclaringType.Name, Action = x.Name, ReturnType = x.ReturnType.Name, Attributes = x.GetCustomAttributes().ToList() })
                    .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();
            List<Tuple<string, string>> res = new List<Tuple<string, string>>();
            foreach (var item in controlleractionlist)
            {
                if (item.Attributes.Where(c => c.GetType().Name.ToUpper().IndexOf("HTTPPOST") != -1).Count() > 0)
                    continue;
                var arribute = (MFAutorize)item.Attributes.Where(c => c.GetType().Name.IndexOf("MFAutorize") != -1).FirstOrDefault();
                if (arribute != null)
                    res.Add(new Tuple<string, string>(arribute.Title, item.Controller + " | " + item.Action));
            }
            res = res.Distinct().OrderBy(c => c.Item1).ToList();
            return res;
        }

        /// <summary>
        /// خالی کرد گش مرورگر
        /// </summary>
        protected void ClearCache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }
    }
}