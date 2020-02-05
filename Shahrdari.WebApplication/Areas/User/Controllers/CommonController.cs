using System.Web.Mvc;
using Shahrdari.WebApplication.Anotations;
using System;
using Shahrdari.Models.Loging;
using Shahrdari.Enums.Loging;
using Shahrdari.Logging;
using Shahrdari.Enums;
using Shahrdari.Models;
using Shahrdari.BussinessLayer;
using System.Web;

namespace Shahrdari.WebApplication.Areas.User.Controllers
{
    public class CommonController : BaseController
    {
        [MFUsersAutorize]
        public ActionResult Index()
        {
            return Redirect(Url.Action("Index", "Home"));
        }

        [MFUsersAutorize]
        public ActionResult Presentation()
        {
            return View(CurrentUser);
        }

        [MFUsersAutorize]
        public ActionResult AboutUs()
        {
            return View();
        }

        [HttpPost]
        [MFUsersAutorize]
        public ActionResult ListenEar(string EarText)
        {
            object result = "";
            try
            {
                M_ListenEar mEar = new M_ListenEar();
                mEar.CreationDate = DateTime.Now;
                mEar.IsRead = false;
                mEar.UserId = CurrentUser.Id;
                mEar.Description = EarText;
                result = new B_ListenEar().AddListenEar(mEar);
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_USER_WEB_APPLICATION, E_LogType.ERROR, ex);
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

        [MFUsersAutorize]
        public ActionResult News()
        {
            return View();
        }

        [MFUsersAutorize]
        public ActionResult Messages()
        {
            return View();
        }

        [MFUsersAutorize]
        public ActionResult UserSetting()
        {
            ViewBag.User = CurrentUser;
            return View();
        }

        [MFUsersAutorize]
        public ActionResult UserChangePassword()
        {
            return View();
        }

        [MFUsersAutorize]
        public ActionResult ContactSupport()
        {
            return View();
        }

        public ActionResult Error404()
        {
            return View();
        }

        [MFUsersAutorize]
        [HttpPost]
        public ActionResult EditUserSetting(string FirstName, string LastName, string BirthDate, string Tell, E_PublicCategory.GENDER Gender, E_PublicCategory.INSTITUTE_TYPE InstituteType, string PostalCode, string Email, string Password)
        {
            object result = "";
            try
            {
                var user = new M_Users
                {
                    BirthDate = string.IsNullOrEmpty(BirthDate) ? null : (DateTime?)(BirthDate.ConverShamsiToMiladi()),
                    DeactiveDate = CurrentUser.DeactiveDate,
                    DeletedDate = CurrentUser.DeletedDate,
                    Email = Email,
                    FirstName = FirstName,
                    Gender = Gender,
                    Id = CurrentUser.Id,
                    ImageName = CurrentUser.ImageName,
                    InstituteName = CurrentUser.InstituteName,
                    InstituteType = InstituteType,
                    InviteUserId = CurrentUser.InviteUserId,
                    IsActive = CurrentUser.IsActive,
                    IsDeleted = CurrentUser.IsDeleted,
                    LastName = LastName,
                    LastOnline = CurrentUser.LastOnline,
                    MobileNumber = CurrentUser.MobileNumber,
                    Password = Password,
                    PostalCode = PostalCode,
                    ReagentCode = CurrentUser.ReagentCode,
                    RegisterDate = CurrentUser.RegisterDate,
                    Tell = Tell,
                    UnicKey = CurrentUser.UnicKey,
                    UserType = CurrentUser.UserType
                };
                new B_Users().Edit(user);
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_USER_WEB_APPLICATION, E_LogType.ERROR, ex);
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
        
        [MFUsersAutorize]
        [HttpPost]
        public ActionResult UploadImage()
        {
            object result = "";
            try
            {
                string fileName = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                HttpPostedFileBase file = Request.Files[0];
                int fileSize = file.ContentLength;
                System.IO.Stream fileContent = file.InputStream;
                file.SaveAs(Server.MapPath("~/Areas/User/Images/Profiles/" + fileName));
                CurrentUser.ImageName = fileName;
                new B_Users().Edit(CurrentUser);
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
    }
}