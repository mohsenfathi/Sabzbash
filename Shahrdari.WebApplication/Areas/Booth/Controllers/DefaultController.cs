using Shahrdari.WebApplication.Anotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shahrdari.BussinessLayer;
using Shahrdari.Models.Loging;
using Shahrdari.Models;
using Shahrdari.Enums.Loging;
using Shahrdari.Logging;
using Shahrdari.Factory.Log;
using Shahrdari.Settings;
using Shahrdari.Enums;
using Shahrdari.WebApplication.Classes.Enums;
using Microsoft.AspNet.SignalR.Client;

namespace Shahrdari.WebApplication.Areas.Booth.Controllers
{
    public class DefaultController : BaseController
    {
        [MFBoothRiderAutorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [MFBoothRiderAutorize]
        public ActionResult RequestDetails(int Id)
        {
            B_ServicesCategories bCat = new B_ServicesCategories();
            var categories = bCat.GetServicesCategories(true);
            ViewBag.ElecScore = categories.Where(x => x.Id == 2).Select(x => x.ScorePerUnit).FirstOrDefault();
            ViewBag.MetalScore = categories.Where(x => x.Id == 3).Select(x => x.ScorePerUnit).FirstOrDefault();
            ViewBag.NonMetalScore = categories.Where(x => x.Id == 8).Select(x => x.ScorePerUnit).FirstOrDefault();
            ViewBag.RandomScore = categories.Where(x => x.Id == 9).Select(x => x.ScorePerUnit).FirstOrDefault();
            ViewBag.Request = new B_ServicesRequests().GetFullServicesRequests(Id);
            ViewBag.RequestItems = new B_ServicesRequestItems().GetItems(Id);
            return View();
        }

        [HttpPost]
        public ActionResult GetRequests()
        {
            object result = "";
            try
            {
                result = new B_ServicesRequests().GetServicesRequestsForPersonel(CurrentUser.Id, CurrentUser.PersonelType);
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_BOOTH_RIDER_APPLICATION, E_LogType.ERROR, ex);
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
        public ActionResult DoneRequest(int Id)
        {
            object result = "";
            try
            {
                var bReq = new B_ServicesRequests();
                bReq.CloseRequestByPortal(Id);
                result = true;
                try
                {
                    var req = bReq.GetServicesRequests(Id);
                    var hubConnection = new HubConnection(url: SignalRUrl);
                    var chat = hubConnection.CreateHubProxy(hubName: "Reqeust");

                    hubConnection.Start().Wait();

                    chat.Invoke<bool>("RequestChangedStatus", req, req.UserId);
                }
                catch { }
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_BOOTH_RIDER_APPLICATION, E_LogType.ERROR, ex);
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
        public ActionResult AcceptRequest(int Id)
        {
            object result = "";
            try
            {
                var bReq = new B_ServicesRequests();
                bReq.AsignUserToRequest(CurrentUser.Id, Id);
                result = true;
                try
                {
                    var req = bReq.GetServicesRequests(Id);
                    var hubConnection = new HubConnection(url: SignalRUrl);
                    var chat = hubConnection.CreateHubProxy(hubName: "Reqeust");

                    hubConnection.Start().Wait();

                    chat.Invoke<bool>("RequestChangedStatus", req, req.UserId);
                }
                catch { }
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_BOOTH_RIDER_APPLICATION, E_LogType.ERROR, ex);
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
        public ActionResult EditRequest(int RequestId, List<M_ServicesRequestItems> RequestItems)
        {
            object result = "";
            try
            {
                var bItem = new B_ServicesRequestItems();
                var oldItems = bItem.GetItems(RequestId, E_PublicCategory.SYSTEM_USER_TYPE.NORMAL_USER);
                var userId = new B_ServicesRequests().GetServicesRequests(RequestId).UserId;
                foreach (var li in oldItems)
                    bItem.Delete(li.Id);
                if (RequestItems != null && RequestItems.Count > 0)
                    foreach (var li in RequestItems)
                    {
                        if (li.Value != 0)
                        {
                            var category = new B_ServicesCategories().GetServicesCategories(li.Id);
                            if (category != null)
                                bItem.Add(new M_ServicesRequestItems
                                {
                                    CreateDate = DateTime.Now,
                                    RequestId = RequestId,
                                    UserType = E_PublicCategory.SYSTEM_USER_TYPE.NORMAL_USER,
                                    Value = li.Value,
                                    ImageName = category.ImageName,
                                    ScorePerUnit = category.ScorePerUnit,
                                    Title = category.Title,
                                    Unit = category.Unit,
                                    IsFailed = false,
                                    UserId = userId,
                                    CategoryId = category.Id
                                });
                        }
                    }
                result = true;
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_BOOTH_RIDER_APPLICATION, E_LogType.ERROR, ex);
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
        public ActionResult LoginSubmit(string PhoneNumber, string Password)
        {
            object result = "";
            try
            {
                PhoneNumber = B_PublicFunctions.ReplacePersianNums(PhoneNumber);
                Password = B_PublicFunctions.ReplacePersianNums(Password);
                if (string.IsNullOrEmpty(PhoneNumber))
                    throw F_ExeptionFactory.MakeExeption("نام کاربری را وارد کنید",
                        ((int)E_ErrorCodes.USERNAME_OR_PASSWORD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "UserName", E_LogType.SYSTEM_ERROR);
                if (string.IsNullOrEmpty(Password))
                    throw F_ExeptionFactory.MakeExeption("گذرواژه را وارد کنید",
                        ((int)E_ErrorCodes.USERNAME_OR_PASSWORD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Password", E_LogType.SYSTEM_ERROR);
                var user = new B_Personels().GetPersonelByUserName(PhoneNumber, Password);
                if (user == null)
                    throw F_ExeptionFactory.MakeExeption("نام کاربری یا گذرواژه صحیح نسیت",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "User", E_LogType.SYSTEM_ERROR);

                HttpCookie Coki = new HttpCookie(MFCookies.BOOTH_RIDER_KEY);
                Coki.Value = user.UnicKey.ToString();
                Coki.Expires = DateTime.Now.AddYears(1);
                Response.Cookies.Add(Coki);
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_BOOTH_RIDER_APPLICATION, E_LogType.ERROR, ex);
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

        public ActionResult PasswordChange()
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

        [HttpPost]
        public ActionResult ResetPasswordChangePassword(int Id, string NewPassword, string Code)
        {
            object result = "";
            try
            {
                if (Id == 0)
                    throw F_ExeptionFactory.MakeExeption("کد فعالسازی یافت نشد",
                        ((int)E_ErrorCodes.WRONG_ID) + S_Seprators.ErrorFieldNameSeprator.ToString() + "User", E_LogType.SYSTEM_ERROR);
                Code = Code.ReplacePersianNums();
                if (new B_SmsAuthorise().IsValidCode(Id, int.Parse(Code)))
                {
                    var res = new B_SmsAuthorise().GetSmsDetails(Id);
                    if (res != null)
                    {
                        new B_SmsAuthorise().DeleteVerificationCode(Id);
                    }
                    else
                        throw F_ExeptionFactory.MakeExeption("کد فعالسازی وارد شده صحیح نمیباشد",
                        ((int)E_ErrorCodes.WRONG_CODE) + S_Seprators.ErrorFieldNameSeprator.ToString() + "User", E_LogType.SYSTEM_ERROR);
                }
                else
                    throw F_ExeptionFactory.MakeExeption("کد فعالسازی وارد شده صحیح نمیباشد",
                    ((int)E_ErrorCodes.WRONG_CODE) + S_Seprators.ErrorFieldNameSeprator.ToString() + "User", E_LogType.SYSTEM_ERROR);

                if (string.IsNullOrEmpty(NewPassword))
                    throw F_ExeptionFactory.MakeExeption("گذرواژه وارد شده صحیح نمی باشد",
                        ((int)E_ErrorCodes.USERNAME_OR_PASSWORD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Password", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (Id == 0)
                    throw F_ExeptionFactory.MakeExeption("اطلاعات ورودی معتبر نمیباشد",
                        ((int)E_ErrorCodes.EMPTY_FIELD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "VerificationId", Enums.Loging.E_LogType.SYSTEM_ERROR);
                var smsAuthorize = new B_SmsAuthorise().GetSmsDetails(Id);
                if (smsAuthorize == null)
                    throw F_ExeptionFactory.MakeExeption("اطلاعات ورودی معتبر نمیباشد",
                        ((int)E_ErrorCodes.EMPTY_FIELD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "VerificationId", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (new B_Personels().ChangePassword(smsAuthorize.PhoneNumber, NewPassword))
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
    }
}