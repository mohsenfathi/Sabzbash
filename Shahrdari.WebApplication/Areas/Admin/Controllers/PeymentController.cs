using Shahrdari.BussinessLayer;
using Shahrdari.Enums;
using Shahrdari.Enums.Loging;
using Shahrdari.Logging;
using Shahrdari.Models.Loging;
using Shahrdari.WebApplication.Anotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shahrdari.WebApplication.Areas.Admin.Controllers
{
    public class PeymentController : BaseController
    {
        [MFAutorize("لیست درخواست های تسویه", "درخواست تسویه")]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.PayTypes = new B_PublicCategory().GetPublicCategory(E_PublicCategory.PUBLIC_CATEGORY_PARENT.PAYMENT_STATUS);
            ViewBag.PayStatus = new B_PublicCategory().GetPublicCategory(E_PublicCategory.PUBLIC_CATEGORY_PARENT.PAYMENT_TYPE);
            return View();
        }

        [MFAutorize("لیست درخواست های تسویه", "درخواست تسویه")]
        [HttpPost]
        public ActionResult GetPayments(int PageNumber, E_PublicCategory.PAYMENT_TYPE? Type, E_PublicCategory.PAYMENT_STATUS? Status)
        {
            object result = "";
            try
            {
                var payRequest = new B_UserPayment().GetPayment(PageNumber, Type, Status);
                var res = new List<object>();
                var payTypes = new B_PublicCategory().GetPublicCategory(E_PublicCategory.PUBLIC_CATEGORY_PARENT.PAYMENT_TYPE);
                var payStatus = new B_PublicCategory().GetPublicCategory(E_PublicCategory.PUBLIC_CATEGORY_PARENT.PAYMENT_STATUS);

                foreach (var li in payRequest)
                {
                    res.Add(new
                    {
                        Title = payTypes.Where(c => c.Id == (int)li.Type).FirstOrDefault().Title,
                        CreateDate = li.CreateDate.ConvertToPesianDateName(true),
                        Status = payStatus.Where(c => c.Id == (int)li.Status).FirstOrDefault().Title,
                        StatusCode = (int)li.Status,
                        Score = li.Point,
                        Id = li.Id
                    });
                }

                result = res;
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                result = "Error";
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                {
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                    result = exx;
                }
                L_Log.SubmitLog(exx);
            }
            return Json(result);
        }

        [MFAutorize("دریافت جزئیات درخواست", "درخواست تسویه")]
        [HttpPost]
        public ActionResult GetPeymentDetails(int PayId)
        {
            object result = "";
            try
            {
                var peymentReuest = new B_UserPayment().GetPaymentById(PayId);
                var payTypes = new B_PublicCategory().GetPublicCategory(E_PublicCategory.PUBLIC_CATEGORY_PARENT.PAYMENT_TYPE);
                var payStatus = new B_PublicCategory().GetPublicCategory(E_PublicCategory.PUBLIC_CATEGORY_PARENT.PAYMENT_STATUS);

                var account = new B_Accounts().GetAccounts(peymentReuest.AccountId);

                var user = new B_Users().GetUsers(peymentReuest.UserId);
                result = new
                {
                    Payment = new
                    {
                        Point = peymentReuest.Point,
                        CreateDate = peymentReuest.CreateDate.ConvertToPesianDateName(true),
                        EditDate = peymentReuest.ModifyDate.ConvertToPesianDateName(true),
                        Status = payStatus.Where(c => c.Id == (int)peymentReuest.Status).FirstOrDefault().Title,
                        StatusCode = (int)peymentReuest.Status,
                        Message = peymentReuest.Message,
                        Type = payTypes.Where(c => c.Id == (int)peymentReuest.Type).FirstOrDefault().Title
                    },
                    AccountInfo = new
                    {
                        Shaba = account == null ? "---" : account.ShabaNumber,
                        PayAccountOwner = account == null ? "---" : account.AccountHolderName,
                        BankName = account == null ? "---" : account.BankName
                    },
                    User = new { Name = user.FirstName + " " + user.LastName, RegisterDate = user.RegisterDate.ConvertToPesianDateName(true), Tell = user.MobileNumber, Image = user.ImageName }
                };
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                result = "Error";
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                {
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                    result = exx;
                }
                L_Log.SubmitLog(exx);
            }
            return Json(result);
        }
        
        [MFAutorize("ویرایسش وضعیت درخواست", "درخواست تسویه")]
        [HttpPost]
        public ActionResult ChangeState(E_PublicCategory.PAYMENT_STATUS Status, int PayId)
        {
            object result = "";
            try
            {
                var peyment = new B_UserPayment().ChangeState(Status, PayId);
                var user = new B_Users().GetUsers(peyment.UserId);
                if (Status == E_PublicCategory.PAYMENT_STATUS.FAILED)
                {
                    new B_ServicesRequestItems().Add(new Models.M_ServicesRequestItems
                    {
                        CreateDate = DateTime.Now,
                        ImageName = "Default.png",
                        RequestId = 0,
                        ScorePerUnit = 1,
                        Title = "بازگشت امتیاز به دلیل لغو درخواست برداشت امتیاز به شماره " + peyment.Id,
                        Unit = "امتیاز",
                        UserType = peyment.UserType,
                        Value = peyment.Point,
                        UserId = user.Id
                    });
                }
                result = true;
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                result = "Error";
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                {
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                    result = exx;
                }
                L_Log.SubmitLog(exx);
            }
            return Json(result);
        }
    }
}