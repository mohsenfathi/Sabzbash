using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shahrdari.BussinessLayer;
using Shahrdari.Models;
using Shahrdari.Enums;

namespace Shahrdari.WebApplication.Anotations
{
    public class MFSiteVisit : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.HttpMethod == "GET")
            {
                System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                string date = pc.GetYear(DateTime.Now) + "/" + pc.GetMonth(DateTime.Now) + "/" + pc.GetDayOfMonth(DateTime.Now);
                if (filterContext.HttpContext.Request.Cookies["SiteVisit"] == null || filterContext.HttpContext.Request.Cookies["SiteVisit"].Value != date.ToString())
                {
                    /*B_Visits bv = new B_Visits();
                    DateTime nowDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
                    var visit = bv.GetVisits(nowDate);
                    if (visit == null)
                        visit = new M_Visits { Date = nowDate, Id = 0, Value = 0 };
                    visit.Value++;
                    bv.UpdatetVisit(visit);
                    HttpCookie Coki = new HttpCookie("SiteVisit");
                    Coki.Value = date.ToString();
                    Coki.Expires = DateTime.Now.AddYears(1);
                    filterContext.HttpContext.Response.Cookies.Add(Coki);*/
                }
            }
        }
    }
}