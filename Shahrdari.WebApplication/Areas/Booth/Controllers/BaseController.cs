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

namespace Shahrdari.WebApplication.Areas.Booth.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// کاربر جاری که لاگین کرده
        /// </summary>
        public M_Personels CurrentUser { get; set; }

        protected string BaseUrl = System.Web.HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

        protected string RootPath = System.Web.HttpContext.Current.Server.MapPath("~");

        /// <summary>
        /// آدرس اتصال به SignalR
        /// </summary>
        protected string SignalRUrl = System.Configuration.ConfigurationManager.AppSettings["SignalRUrl"];

        public BaseController()
        {
            if (System.Web.HttpContext.Current.Request.Cookies[MFCookies.BOOTH_RIDER_KEY] != null)
            {
                string key = System.Web.HttpContext.Current.Request.Cookies[MFCookies.BOOTH_RIDER_KEY].Value;
                try
                {
                    CurrentUser = new B_Personels().GetPersonels(key);
                }
                catch { }
            }
            ViewBag.SignalRUrl = SignalRUrl;
            ViewBag.CurrentUser = CurrentUser;
            ViewBag.MasterIsDriver = CurrentUser == null ? "false" : CurrentUser.PersonelType == E_PublicCategory.PERSONEL_TYPE.DRIVER ? "true" : "false";
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