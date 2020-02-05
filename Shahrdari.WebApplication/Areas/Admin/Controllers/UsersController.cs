using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shahrdari.BussinessLayer;
using Shahrdari.Enums;
using Shahrdari.Enums.Loging;
using Shahrdari.Logging;
using Shahrdari.Models;
using Shahrdari.Models.Loging;
using Shahrdari.WebApplication.Anotations;
using Shahrdari.WebApplication.Classes.Models;
using Shahrdari.ViewModels;

namespace Shahrdari.WebApplication.Areas.Admin.Controllers
{
    public class UsersController : BaseController
    {
        [MFAutorize("لیست کاربران", "کاربران")]
        [HttpGet]
        public ActionResult Index()
        {
            var users = new B_Users().GetUsers(1, "", "", "");
            foreach (var li in users)
                li.LastRequest = new B_ServicesRequests().GetLastRequestDateByUserId(li.Id);
            return View(users);
        }

        [MFAutorize("دریافت مقادیری از کاربران", "کاربران")]
        [HttpPost]
        public ActionResult GetUser(int PageNumber, string Name, string Family, string Mobile)
        {
            object result = "";
            try
            {
                result = new B_Users().GetUsers(PageNumber, Name, Family, Mobile);
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

        [MFAutorize("افزودن کاربر", "کاربران")]
        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.AnistetoType = new B_PublicCategory().GetPublicCategory(E_PublicCategory.PUBLIC_CATEGORY_PARENT.INSTITUTE_TYPE);
            return View();
        }

        [MFAutorize("افزودن کاربر", "کاربران")]
        [HttpPost]
        public ActionResult Add(M_Users Personel)
        {
            object result = "";
            try
            {
                var buser = new B_Users();
                buser.Add(Personel);
                result = "Success";
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

        [MFAutorize("ویرایش و حذف کاربر", "کاربران")]
        [HttpGet]
        public ActionResult Modify(int Id)
        {
            ViewBag.Route = new List<MFRoute>
            {
                new MFRoute{ ActionName = "Index", ControllerName="Users", Title = "لیست کاربران", Priority = 1 }
            };
            B_Users bUser = new B_Users();
            var user = bUser.GetUsers(Id);
            if (user == null || user.Id == CurrentUser.Id)
                return RedirectToAction("Index");
            ViewBag.AnistetoType = new B_PublicCategory().GetPublicCategory(E_PublicCategory.PUBLIC_CATEGORY_PARENT.INSTITUTE_TYPE);
            return View(user);
        }

        [MFAutorize("ویرایش و حذف کاربر", "کاربران")]
        [HttpPost]
        public ActionResult Modify(M_Users User)
        {
            object result = "";
            try
            {
                B_Users bUser = new B_Users();
                var oldUser = bUser.GetUsers(User.Id);
                if (oldUser.ImageName.ToLower() != "default.jpg" && oldUser.ImageName.ToLower() != User.ImageName)
                    System.IO.File.Delete(Server.MapPath("~/Areas/Admin/Images/Profile") + "/" + oldUser.ImageName);
                bUser.Edit(User);
                result = "Success";
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

        [MFAutorize("حذف کاربر", "کاربران")]
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            object result = "";
            try
            {
                B_Users bUser = new B_Users();
                bUser.Delete(Id);
                result = "Success";
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

        [MFAutorize("کاربران حذف شده", "کاربران")]
        [HttpGet]
        public ActionResult Deleted()
        {
            ViewBag.Route = new List<MFRoute>
            {
                new MFRoute{ ActionName = "Index", ControllerName="Users", Title = "لیست کاربران", Priority = 1 }
            };
            B_Users bPersonel = new B_Users();
            return View(bPersonel.GetDeletedUsers());
        }

        [MFAutorize("ابزگردانی کاربر", "کاربران")]
        [HttpPost]
        public ActionResult RevertUser(int Id)
        {
            object result = "";
            try
            {
                var bUser = new B_Users();
                bUser.RevertUser(Id);
                result = "Success";
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

        [MFAutorize("آپلود تصویر", "کاربران")]
        [HttpPost]
        public ActionResult UploadProfileImage()
        {
            object result = "";
            try
            {
                string fileName = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                HttpPostedFileBase file = Request.Files[0];
                int fileSize = file.ContentLength;
                System.IO.Stream fileContent = file.InputStream;
                file.SaveAs(Server.MapPath("~/Areas/Admin/Images/Profile") + "/" + fileName);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                Response.Cache.SetNoStore();
                result = fileName;
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

        [MFAutorize("دریافت جزئیات امتیاز ها", "کاربران")]
        [HttpPost]
        public ActionResult GetUserPointDetails(int UserId)
        {
            object result = "";
            try
            {
                var peymentReuest = new B_UserPayment().GetPayment(UserId);
                var peymentRequestItems = new B_ServicesRequestItems().GetItem(UserId, E_PublicCategory.SYSTEM_USER_TYPE.NORMAL_USER);
                var res = new List<DetaildPointModel>();
                double pointPay = 0, pointRecive = 0;
                var payStatus = new B_PublicCategory().GetPublicCategory(E_PublicCategory.PUBLIC_CATEGORY_PARENT.PAYMENT_STATUS);
                var payType = new B_PublicCategory().GetPublicCategory(E_PublicCategory.PUBLIC_CATEGORY_PARENT.PAYMENT_TYPE);

                foreach (var li in peymentReuest)
                {
                    res.Add(new DetaildPointModel
                    {
                        Title = payType.Where(c => c.Id == (int)li.Type).FirstOrDefault().Title,
                        Date = li.CreateDate.ConvertToPesianDateName(true),
                        Time = li.CreateDate.Hour + ":" + li.CreateDate.Minute + ":" + li.CreateDate.Second,
                        Score = (li.Point * -1).ToString(),
                        Status = payStatus.Where(c => c.Id == (int)li.Status).FirstOrDefault().Title,
                        RealDate = li.CreateDate
                    });
                    if (li.Status != E_PublicCategory.PAYMENT_STATUS.FAILED)
                        pointPay += li.Point * -1;
                }

                foreach (var li in peymentRequestItems)
                {
                    res.Add(new DetaildPointModel
                    {
                        Title = li.Title,
                        Date = li.CreateDate.ConvertToPesianDateName(true),
                        Time = li.CreateDate.Hour + ":" + li.CreateDate.Minute + ":" + li.CreateDate.Second,
                        Score = ((li.Value.HasValue ? li.Value.Value : 0) * li.ScorePerUnit).ToString(),
                        Status = "ثبت شده",
                        RealDate = li.CreateDate
                    });
                    pointRecive += (li.Value.HasValue ? li.Value.Value : 0) * li.ScorePerUnit;
                }

                var user = new B_Users().GetUsers(UserId);
                result = new
                {
                    PointSummery = new { PointPay = pointPay, PointRecive = pointRecive, PointTotal = pointRecive + pointPay },
                    PointDetails = res.OrderByDescending(c => c.RealDate).ToList(),
                    User = new { Name = user.FirstName + " " + user.LastName, RegisterDate = user.RegisterDate.ConvertToPesianDateName(true), Tell = user.MobileNumber }
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
    }

    public class DetaildPointModel
    {
        public string Title { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Score { get; set; }
        public string Status { get; set; }
        public DateTime RealDate { get; set; }
    }
}