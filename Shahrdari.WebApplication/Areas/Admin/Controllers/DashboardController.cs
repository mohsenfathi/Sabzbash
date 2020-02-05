using Shahrdari.BussinessLayer;
using Shahrdari.Enums;
using Shahrdari.ViewModels;
using Shahrdari.WebApplication.Anotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shahrdari.WebApplication.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        [MFAutorize("داشبورد", "داشبورد")]
        public ActionResult Index()
        {
            var bStatictics = new B_Statistics();
            var requestData = bStatictics.GetRequestStatisticsPastMonth();
            ViewBag.ReqData = "";
            ViewBag.ReqCategory = "";
            foreach (var li in requestData)
            {
                ViewBag.ReqData += ViewBag.ReqData == "" ? li.Item2 + "" : "," + li.Item2;
                ViewBag.ReqCategory += ViewBag.ReqCategory == "" ? "'" + li.Item1 + "'" : ",'" + li.Item1 + "'";
            }
            ViewBag.UserCount = bStatictics.GetUsersCount();

            var markers = "";
            foreach (var li in new B_ServicesRequests().GetFullLast30Request())
            {
                markers += "{ 'popupContent': '" + getMapPupWindow(li) + "', 'center': { 'lat': " + li.GeographicalCoordinates.Split(',')[0].Trim() + " ,'lng': " + li.GeographicalCoordinates.Split(',')[1].Trim() + " }, 'iconOpts': { 'iconUrl': '/Areas/Admin/Images/MapMarker" + (li.Status == Enums.E_PublicCategory.REQUEST_STATUS.NEW_REQUEST ? "New" : "Other") + ".png', 'iconRetinaUrl': '/Areas/Admin/Images/MapMarker" + (li.Status == Enums.E_PublicCategory.REQUEST_STATUS.NEW_REQUEST ? "New" : "Other") + ".png', 'iconSize': [30, 38] } },";
            }

            ViewBag.LastestRequest = markers;

            var chartTypeRes = new B_ServicesRequests().GetForChartType();
            var reqStatus = new B_PublicCategory().GetPublicCategory(E_PublicCategory.PUBLIC_CATEGORY_PARENT.REQUEST_STATUS);
            foreach (var li in chartTypeRes)
                ViewBag.ForChartType += "['" + reqStatus.Where(c => c.Id == (int)li.Item2).FirstOrDefault().Title + "', " + li.Item1 + "],";


            var chartTypePay = new B_UserPayment().GetForChartType();
            var chartStatusPay = new B_UserPayment().GetForChartStatus();
            var payType = new B_PublicCategory().GetPublicCategory(E_PublicCategory.PUBLIC_CATEGORY_PARENT.PAYMENT_TYPE);
            var payStatus = new B_PublicCategory().GetPublicCategory(E_PublicCategory.PUBLIC_CATEGORY_PARENT.PAYMENT_STATUS);
            foreach (var li in chartTypePay)
                ViewBag.ForChartPayType += "['" + payType.Where(c => c.Id == (int)li.Item2).FirstOrDefault().Title + "', " + li.Item1 + "],";
            foreach (var li in chartStatusPay)
                ViewBag.ForChartPayStatus += "['" + payStatus.Where(c => c.Id == (int)li.Item2).FirstOrDefault().Title + "', " + li.Item1 + "],";

            ViewBag.BoothCount = bStatictics.GetBoothCount();

            return View();
        }
        
        string getMapPupWindow(V_ServicesRequests request)
        {
            var reqStatus = new B_PublicCategory().GetPublicCategory(E_PublicCategory.PUBLIC_CATEGORY_PARENT.REQUEST_STATUS);
            return $"<table class=\"tbList\"><tr style=\"background:#FFF;\"><td><img src=\"/Areas/Admin/Images/Profile/{request.User.ImageName}\" style=\"border-radius:50px;width:100px;\" /></td></tr><tr style=\"background:#FFF;\"><td>{request.User.FirstName + "  " + request.User.LastName}</td></tr><tr style=\"background:#FFF;\"><td>{reqStatus.Where(c => c.Id == (int)request.Status).FirstOrDefault().Title}</td></tr><tr style=\"background:#FFF;\"><td>{request.CreateDate.ConvertToPesianDateName(true)}</td></tr></table>";
        }

        public ActionResult NoPermission()
        {
            return View();
        }
    }
}