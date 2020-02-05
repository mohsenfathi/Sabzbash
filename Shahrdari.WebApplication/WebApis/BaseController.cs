using Shahrdari.BussinessLayer;
using Shahrdari.Enums;
using Shahrdari.Enums.Loging;
using Shahrdari.Factory.Log;
using Shahrdari.Logging;
using Shahrdari.Models.Loging;
using Shahrdari.Settings;
using Shahrdari.WebApplication.Classes.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Shahrdari.WebApplication.WebApis
{
    public class BaseController : ApiController
    {
        /// <summary>
        /// آدرس اتصال به SignalR
        /// </summary>
        protected string SignalRUrl = System.Configuration.ConfigurationManager.AppSettings["SignalRUrl"];

        /// <summary>
        /// آدرس مبدا
        /// </summary>
        protected string BaseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);

        protected string RootPath = HttpContext.Current.Server.MapPath("~");

        /// <summary>
        /// پیکربندی عملیات موفق
        /// </summary>
        /// <param name="Result">نتیجه عملیات انجام شده</param>
        /// <returns>خروجی برای عملیات موفق</returns>
        protected object PrepareSuccessResult(object Result)
        {
            return new { StatusCode = (int)E_ErrorCodes.SUCCESS, ErrorField = "", StatusMessage = "عملیات موفق", ResultObject = Result };
        }

        /// <summary>
        /// پیکربندی خروجی خطا
        /// </summary>
        /// <param name="Code">کد خطا</param>
        /// <param name="Message">پیغام خطا</param>
        /// <returns>خروجی برای خطا</returns>
        protected object PrepareFaildResult(string Message, string Code)
        {
            if (Code == null)
                Code = "";
            var tempCode = Code.Split(S_Seprators.ErrorFieldNameSeprator);
            return new { StatusCode = tempCode[0], ErrorField = tempCode.Count() > 1 ? tempCode[1] : "", StatusMessage = Message, ResultObject = "" };
        }

        /// <summary>
        /// اعتبارسنجی کاربر
        /// </summary>
        /// <returns>شناسه کاربر</returns>
        protected int validateUser()
        {
            int id = 0;
            try { id = int.Parse(Request.Headers.GetValues("UserId").FirstOrDefault()); }
            catch
            {
                throw F_ExeptionFactory.MakeExeption("کاربر مورد نظر در سیستم وجود ندارد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "UserId", Enums.Loging.E_LogType.SYSTEM_ERROR);
            }
            string unicKey = "";
            try { unicKey = Request.Headers.GetValues("UnicKey").FirstOrDefault(); }
            catch
            {
                throw F_ExeptionFactory.MakeExeption("کاربر مورد نظر در سیستم وجود ندارد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "UserId", Enums.Loging.E_LogType.SYSTEM_ERROR);
            }
            var user = new B_Users().GetUsers(id);
            if (user == null)
                throw F_ExeptionFactory.MakeExeption("کاربر مورد نظر در سیستم وجود ندارد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "UserId", Enums.Loging.E_LogType.SYSTEM_ERROR);
            if (user.UnicKey != unicKey)
                throw F_ExeptionFactory.MakeExeption("کاربر مورد نظر در سیستم وجود ندارد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "UserId", Enums.Loging.E_LogType.SYSTEM_ERROR);
            return id;
        }

        protected int validatePersonel()
        {
            int id = 0;
            try { id = int.Parse(Request.Headers.GetValues("PersonelId").FirstOrDefault()); }
            catch
            {
                throw F_ExeptionFactory.MakeExeption("کاربر مورد نظر در سیستم وجود ندارد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "UserId", Enums.Loging.E_LogType.SYSTEM_ERROR);
            }
            string unicKey = "";
            try { unicKey = Request.Headers.GetValues("UnicKey").FirstOrDefault(); }
            catch
            {
                throw F_ExeptionFactory.MakeExeption("کاربر مورد نظر در سیستم وجود ندارد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "UserId", Enums.Loging.E_LogType.SYSTEM_ERROR);
            }
            var user = new B_Personels().GetPersonelById(id);
            if (user == null)
                throw F_ExeptionFactory.MakeExeption("کاربر مورد نظر در سیستم وجود ندارد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "UserId", Enums.Loging.E_LogType.SYSTEM_ERROR);
            if (user.UnicKey != unicKey)
                throw F_ExeptionFactory.MakeExeption("کاربر مورد نظر در سیستم وجود ندارد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "UserId", Enums.Loging.E_LogType.SYSTEM_ERROR);
            return id;
        }

        /// <summary>
        /// اعتبارسنجی کاربر یا پرسنل
        /// </summary>
        /// <returns>شناسه کاربر اهراز شده و نوع آن</returns>
        protected Tuple<int, MFValidationUserRole> validateUserOrPersonel()
        {
            int id = 0;
            try { id = int.Parse(Request.Headers.GetValues("UserId").FirstOrDefault()); }
            catch
            {
                throw F_ExeptionFactory.MakeExeption("کاربر مورد نظر در سیستم وجود ندارد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "UserId", Enums.Loging.E_LogType.SYSTEM_ERROR);
            }
            string unicKey = "";
            try { unicKey = Request.Headers.GetValues("UnicKey").FirstOrDefault(); }
            catch
            {
                throw F_ExeptionFactory.MakeExeption("کاربر مورد نظر در سیستم وجود ندارد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "UserId", Enums.Loging.E_LogType.SYSTEM_ERROR);
            }

            var user = new B_Users().GetUsers(id);
            if (user != null && user.UnicKey == unicKey)
                return Tuple.Create(id, MFValidationUserRole.USER);
            var personel = new B_Personels().GetPersonels(unicKey, id);
            if (personel != null)
                return Tuple.Create(id, MFValidationUserRole.PERSONEL);
            throw F_ExeptionFactory.MakeExeption("کاربر مورد نظر در سیستم وجود ندارد",
                ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "UserId", Enums.Loging.E_LogType.SYSTEM_ERROR);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="Root"></param>
        /// <returns></returns>
        protected string GetImageAddress(string FileName,string Root)
        {
            if (string.IsNullOrEmpty(FileName))
                return Root.ToString() + "Default.jpg";
            return Root.ToString() + FileName;
        }

        [HttpPost]
        public IHttpActionResult ProfileAttachmentWithPostedFile()
        {
            object result = "";
            try
            {
                int userId = validateUser();
                var file = HttpContext.Current.Request.Files;
                if (file.Count > 0)
                {
                    if (file[0].ContentLength > 5242880)
                        throw F_ExeptionFactory.MakeExeption("شما مجاز به پیوست فایل تا 5 مگابایت میباشید", ((int)E_ErrorCodes.FILE) + S_Seprators.ErrorFieldNameSeprator.ToString() + "file"
                            , Enums.Loging.E_LogType.SYSTEM_ERROR);
                    if (File.Exists(new E_FTPRoutes(RootPath).USERS + "/" + userId + "." + file[0].FileName.Split('.')[1]))
                        File.Delete(new E_FTPRoutes(RootPath).USERS + "/" + userId + "." + file[0].FileName.Split('.')[1]);
                    file[0].SaveAs(new E_FTPRoutes(RootPath).USERS + "/" + userId + "." + file[0].FileName.Split('.')[1]);
                    return Json(PrepareSuccessResult(true));
                }
                else
                    return Json(PrepareSuccessResult(false));
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                if (ex.Source != E_LogType.SYSTEM_ERROR.ToString())
                    return Json(PrepareFaildResult("خطایی در سرور وجود دارد", (int)E_ErrorCodes.SERVER_ERROR + S_Seprators.ErrorFieldNameSeprator.ToString() + ""));
                return Json(PrepareFaildResult(exx.LogMessage, ex.HelpLink));
            }
        }

    }
}
