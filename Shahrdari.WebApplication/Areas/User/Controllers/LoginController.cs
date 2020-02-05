using Shahrdari.Enums.Loging;
using Shahrdari.Enums;
using Shahrdari.Logging;
using Shahrdari.Models.Loging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shahrdari.Factory.Log;
using Shahrdari.Settings;
using Shahrdari.BussinessLayer;
using Shahrdari.Models;
using Shahrdari.WebApplication.Classes.Enums;

namespace Shahrdari.WebApplication.Areas.User.Controllers
{
    public class LoginController : BaseController
    {
        #region LOGIN PAGES
        public ActionResult Index()
        {
            ViewBag.HaveLearn = Request.QueryString["FromSite"];
            return View();
        }

        public ActionResult LoginPage()
        {
            return View();
        }

        public ActionResult Sms(string FromAddress,int Id)
        {
            ViewBag.FromUrl = FromAddress;
            ViewBag.Id = Id;
            return View();
        }

        public ActionResult PasswordChangePhone()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPasswordSendVerificationCode(string PhoneNumber)
        {
            object result = "";
            try
            {
                if (string.IsNullOrEmpty(PhoneNumber))
                    throw F_ExeptionFactory.MakeExeption("شماره تلفن وارد شده صحیح نیست",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PhoneNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (PhoneNumber.Length != 11)
                    throw F_ExeptionFactory.MakeExeption("شماره تلفن وارد شده صحیح نیست",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PhoneNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                var user = new B_Users().GetUsers(PhoneNumber);
                if (user == null)
                    throw F_ExeptionFactory.MakeExeption("شماره تلفن وارد شده در سیستم ثبت نشده است",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PhoneNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (user.IsDeleted == true)
                    throw F_ExeptionFactory.MakeExeption("چنین کاربری یافت نشد",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "User", Enums.Loging.E_LogType.SYSTEM_ERROR);
                int code = new B_SmsAuthorise().AddCode(PhoneNumber);
                if (code != 0)
                {
                    new ApplicationHelper.Sms().SendSms(PhoneNumber, code.ToString());
                    result = new B_SmsAuthorise().GetSmsDetailsId(PhoneNumber);
                }
                else
                    throw F_ExeptionFactory.MakeExeption("متاسفانه کد فعال سازی ارسال نشد. مجددا تلاش کنید",
                         ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "VerificationCode", E_LogType.SYSTEM_ERROR);
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

        public ActionResult PasswordChange(int Id)
        {
            ViewBag.Id = Id;
            return View("PasswordManage");
        }

        [HttpPost]
        public ActionResult ResetPasswordChangePassword(int Id,string NewPassword,string RetypeNewPassword)
        {
            object result = "";
            try
            {
                if (string.IsNullOrEmpty(NewPassword))
                    throw F_ExeptionFactory.MakeExeption("گذرواژه وارد شده صحیح نمی باشد",
                        ((int)E_ErrorCodes.USERNAME_OR_PASSWORD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Password", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (string.IsNullOrEmpty(RetypeNewPassword))
                    throw F_ExeptionFactory.MakeExeption("گذرواژه وارد شده صحیح نمی باشد",
                        ((int)E_ErrorCodes.USERNAME_OR_PASSWORD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Password", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (NewPassword != RetypeNewPassword)
                    throw F_ExeptionFactory.MakeExeption("گذرواژه های وارد شده باید یکسان باشند",
                        ((int)E_ErrorCodes.USERNAME_OR_PASSWORD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Password", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (Id == 0)
                    throw F_ExeptionFactory.MakeExeption("اطلاعات ورودی معتبر نمیباشد",
                        ((int)E_ErrorCodes.EMPTY_FIELD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "VerificationId", Enums.Loging.E_LogType.SYSTEM_ERROR);
                var smsAuthorize = new B_SmsAuthorise().GetSmsDetails(Id);
                if (smsAuthorize == null)
                    throw F_ExeptionFactory.MakeExeption("اطلاعات ورودی معتبر نمیباشد",
                        ((int)E_ErrorCodes.EMPTY_FIELD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "VerificationId", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (new B_Users().ChangePassword(smsAuthorize.PhoneNumber, NewPassword))
                    result = "Success";
                else
                    throw F_ExeptionFactory.MakeExeption("خطا در تغییر گذرواژه",
                        ((int)E_ErrorCodes.USERNAME_OR_PASSWORD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Password", Enums.Loging.E_LogType.SYSTEM_ERROR);
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


        #endregion LOGIN PAGES

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string Name, string Family, int InstituteType, string MobileNumber, string AsignCode,string Password)
        {
            object result = "";
            try
            {
                MobileNumber = B_PublicFunctions.ReplacePersianNums(MobileNumber);
                AsignCode = B_PublicFunctions.ReplacePersianNums(AsignCode);
                Password = B_PublicFunctions.ReplacePersianNums(Password);
                if (string.IsNullOrEmpty(Name))
                    throw F_ExeptionFactory.MakeExeption("لطفا نام را وارد کنید",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Name", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (string.IsNullOrEmpty(Family))
                    throw F_ExeptionFactory.MakeExeption("لطفا نام خانوادگی را وارد کنید",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Family", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (string.IsNullOrEmpty(MobileNumber))
                    throw F_ExeptionFactory.MakeExeption("لطفا شماره تلفن همراه را وارد کنید",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "MobileNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (string.IsNullOrEmpty(Name))
                    throw F_ExeptionFactory.MakeExeption("لطفا گذرواژه را وارد کنید",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Password", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if(!B_PublicFunctions.IsValidPhone(MobileNumber,true))
                    throw F_ExeptionFactory.MakeExeption("شماره تلفن همراه وارد شده صحیح نمیباشد",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "MobileNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                M_Users mUser = new M_Users();
                mUser.FirstName = Name;
                mUser.LastName = Family;
                mUser.InstituteType = (E_PublicCategory.INSTITUTE_TYPE)InstituteType;
                mUser.MobileNumber = MobileNumber;
                mUser.ReagentCode = string.IsNullOrEmpty(AsignCode) ? 0 : int.Parse(AsignCode);
                mUser.RegisterDate = DateTime.Now;
                mUser.LastOnline = DateTime.Now;
                mUser.IsActive = false;
                mUser.IsDeleted = false;
                mUser.UnicKey = Guid.NewGuid().ToString();
                mUser.Password = Password;
                mUser.UserType = E_PublicCategory.USER_TYPE.HOME_STORE;
                mUser.ReagentUserId = string.IsNullOrEmpty(AsignCode) ? null : (int?)(new B_Users().GetUserByReagentCode(int.Parse(AsignCode)).Id);
                var res = new B_Users().Add(mUser);

                if(mUser.ReagentUserId.HasValue)
                {
                    new B_ServicesRequestItems().Add(new M_ServicesRequestItems {
                        CategoryId = 0,
                        CreateDate = DateTime.Now,
                        ImageName = "Default.jpg",
                        IsFailed = false,
                        RequestId = -5001,
                        ScorePerUnit = 200,
                        ScorePerUnitDriver = 200,
                        Title = $"معرفی {mUser.FirstName} {mUser.LastName} به سیستم",
                        Unit = "عدد",
                        UserType = E_PublicCategory.SYSTEM_USER_TYPE.NORMAL_USER,
                        Value = 1,
                        UserId = mUser.ReagentUserId.Value
                    });
                }

                B_SmsAuthorise bSms = new B_SmsAuthorise();
                var code = bSms.AddCode(MobileNumber);
                new ApplicationHelper.Sms().SendSms(MobileNumber, code.ToString());
                result = bSms.GetSmsDetailsId(MobileNumber);
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

        [HttpPost]
        public ActionResult SendActivationCode(int Id)
        {
            object result = "";
            try
            {
                B_SmsAuthorise bSms = new B_SmsAuthorise();
                var sms = bSms.GetSmsDetails(Id);
                if (sms != null)
                {
                    var code = new B_SmsAuthorise().AddCode(sms.PhoneNumber);
                    new ApplicationHelper.Sms().SendSms(sms.PhoneNumber, code.ToString());
                    result = "Success";
                }
                else
                    throw F_ExeptionFactory.MakeExeption("متاسفانه کد فعال سازی ارسال نشد. مجددا تلاش کنید",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "VerificationCode", E_LogType.SYSTEM_ERROR);
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

        [HttpPost]
        public ActionResult ConfirmActivationCode(string Code, int Id)
        {
            object result = "";
            try
            {
                if (Id == 0)
                    throw F_ExeptionFactory.MakeExeption("شناسه وارد شده صحیح نمیباشد",
                        ((int)E_ErrorCodes.WRONG_ID) + S_Seprators.ErrorFieldNameSeprator.ToString() + "User", E_LogType.SYSTEM_ERROR);
                Code = Code.ReplacePersianNums();
                if (new B_SmsAuthorise().IsValidCode(Id,int.Parse(Code)))
                {
                    var res = new B_SmsAuthorise().GetSmsDetails(Id);
                    if (res != null)
                    {
                        new B_Users().ActiveUser(res.PhoneNumber);
                        new B_SmsAuthorise().DeleteVerificationCode(Id);
                    }
                    else
                        throw F_ExeptionFactory.MakeExeption("کد فعالسازی وارد شده صحیح نمیباشد",
                        ((int)E_ErrorCodes.WRONG_CODE) + S_Seprators.ErrorFieldNameSeprator.ToString() + "User", E_LogType.SYSTEM_ERROR);
                }
                else
                    throw F_ExeptionFactory.MakeExeption("کد فعالسازی وارد شده صحیح نمیباشد",
                    ((int)E_ErrorCodes.WRONG_CODE) + S_Seprators.ErrorFieldNameSeprator.ToString() + "User", E_LogType.SYSTEM_ERROR);
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
        
        [HttpPost]
        public ActionResult Login(string PhoneNumber,string Password)
        {
            object result = "";
            try
            {
                PhoneNumber = B_PublicFunctions.ReplacePersianNums(PhoneNumber);
                Password = B_PublicFunctions.ReplacePersianNums(Password);
                if (string.IsNullOrEmpty(PhoneNumber))
                    throw F_ExeptionFactory.MakeExeption("نام کاربری را وارد کنید",
                        ((int)E_ErrorCodes.USERNAME_OR_PASSWORD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "UserName", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (string.IsNullOrEmpty(Password))
                    throw F_ExeptionFactory.MakeExeption("گذرواژه را وارد کنید",
                        ((int)E_ErrorCodes.USERNAME_OR_PASSWORD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Password", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if(PhoneNumber.Length != 11)
                    throw F_ExeptionFactory.MakeExeption("گذرواژه وارد شده صحیح نمیباشد",
                        ((int)E_ErrorCodes.USERNAME_OR_PASSWORD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Password", Enums.Loging.E_LogType.SYSTEM_ERROR);
                var user = new B_Users().GetUsers(PhoneNumber);
                if (user == null)
                    throw F_ExeptionFactory.MakeExeption("چنین کاربری یافت نشد",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "User", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (user.IsDeleted == true)
                    throw F_ExeptionFactory.MakeExeption("چنین کاربری یافت نشد",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "User", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (user.Password == Password && user.IsActive == false)
                {
                    B_SmsAuthorise bSms = new B_SmsAuthorise();
                    var code = bSms.AddCode(PhoneNumber);
                    new ApplicationHelper.Sms().SendSms(PhoneNumber, code.ToString());
                    result = new {IsActive = false ,  SmsId = bSms.GetSmsDetailsId(PhoneNumber) , UserId = user.Id } ;
                }
                else if(user.Password == Password && user.IsActive)
                    result = new { IsActive = true, SmsId = 0, UserId = user.Id };
                else
                    throw F_ExeptionFactory.MakeExeption("نام کاربری یا گذرواژه صحیح نمیباشد",
                        ((int)E_ErrorCodes.USERNAME_OR_PASSWORD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "UserNameOrPassword", Enums.Loging.E_LogType.SYSTEM_ERROR);

                HttpCookie Coki = new HttpCookie(MFCookies.END_USER_KEY);
                Coki.Value = user.UnicKey.ToString();
                Coki.Expires = DateTime.Now.AddYears(1);
                Response.Cookies.Add(Coki);
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

        private object PrepareSuccessResult(object p)
        {
            throw new NotImplementedException();
        }
    }
}