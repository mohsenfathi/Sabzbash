using Microsoft.AspNet.SignalR.Client;
using Shahrdari.BussinessLayer;
using Shahrdari.Enums;
using Shahrdari.Enums.Loging;
using Shahrdari.Factory.Log;
using Shahrdari.Logging;
using Shahrdari.Models;
using Shahrdari.Models.Loging;
using Shahrdari.Settings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Shahrdari.WebApplication.WebApis
{
    public class PersonelsController : BaseController
    {
        [HttpPost]
        public IHttpActionResult GetPersonelByPhone([FromBody]RequestModelLogin RquestDatas)
        {
            try
            {
                B_Personels bUser = new B_Personels();
                var user = bUser.GetPersonelsByPhone(RquestDatas.PhoneNumber, RquestDatas.Password);
                if (user == null)
                    throw F_ExeptionFactory.MakeExeption("کاربری یافت نشد",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Personels", E_LogType.SYSTEM_ERROR);

                return Json(PrepareSuccessResult(new {
                    BirthDateStr = user.BirthDate.ConvertMiladiToShamsi(),
                    BirthDate = TimeSpan.FromTicks(user.BirthDate.Ticks).TotalMilliseconds,
                    FirstName = user.FirstName,
                    Gender = user.Gender,
                    ImageName = user.ImageName,
                    LastName = user.LastName,
                    MobileNumber = user.MobileNumber,
                    RegisterDateStr = user.RegisterDate.ConvertMiladiToShamsi(),
                    RegisterDate = TimeSpan.FromTicks(user.RegisterDate.Ticks).TotalMilliseconds,
                    UnicKey = user.UnicKey,
                    UserName = user.UserName,
                    Id = user.Id,
                    Rate = new B_ServicesRequests().GetServiceRateByPersonelId(user.Id)
                }));
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

        [HttpPatch]
        public IHttpActionResult EditPersonel([FromBody]RequestModelEditPersonel RquestDatas)
        {
            try
            {
                B_Personels bPersonel = new B_Personels();
                var personel = bPersonel.GetPersonels(RquestDatas.UnicKey, RquestDatas.Id);
                if(personel == null)
                    throw F_ExeptionFactory.MakeExeption("پرسنل مورد نظر در سیستم وجود ندارد",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Id", Enums.Loging.E_LogType.SYSTEM_ERROR);
                personel.BirthDate = RquestDatas.BirthDate;
                personel.FirstName = RquestDatas.FirstName;
                personel.Gender = RquestDatas.Gender;
                personel.LastName = RquestDatas.LastName;
                personel.MobileNumber = RquestDatas.MobileNumber;
                personel.Password = RquestDatas.Password;
                personel.UserName = RquestDatas.UserName;
                bPersonel.Edit(personel);
                return Json(PrepareSuccessResult(true));
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

        #region Khalaj

        [HttpPost]
        public IHttpActionResult Login([FromBody]LoginModel Data)
        {
            try
            {
                if (string.IsNullOrEmpty(Data.UserName))
                    throw F_ExeptionFactory.MakeExeption("نام کاربری را وارد کنید",
                        ((int)E_ErrorCodes.USERNAME_OR_PASSWORD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "UserName", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (string.IsNullOrEmpty(Data.Password))
                    throw F_ExeptionFactory.MakeExeption("گذرواژه را وارد کنید",
                        ((int)E_ErrorCodes.USERNAME_OR_PASSWORD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Password", Enums.Loging.E_LogType.SYSTEM_ERROR);
                var personel = new B_Personels().GetPersonelByUserName(Data.UserName, Data.Password);
                if (personel == null)
                    throw F_ExeptionFactory.MakeExeption("چنین کاربری یافت نشد",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Personel", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (personel.IsDeleted == true)
                    throw F_ExeptionFactory.MakeExeption("کاربر مورد نظر فعال نمیباشد",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Personel", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (personel.Password == Data.Password)
                    return Json(PrepareSuccessResult(getPersonel(personel.Id)));
                else
                    throw F_ExeptionFactory.MakeExeption("نام کاربری یا گذرواژه صحیح نمیباشد",
                        ((int)E_ErrorCodes.USERNAME_OR_PASSWORD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "UserNameOrPassword", Enums.Loging.E_LogType.SYSTEM_ERROR);
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

        [HttpPost]
        public IHttpActionResult ChangeUserName([FromBody]ChangeUserNameModel Data)
        {
            try
            {
                int id = validatePersonel();
                B_Personels bPersonels = new B_Personels();
                if (string.IsNullOrEmpty(Data.PhoneNumber))
                    throw F_ExeptionFactory.MakeExeption("شماره تلفن را وارد کنید",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PhoneNumber", E_LogType.SYSTEM_ERROR);
                var personel = bPersonels.GetPersonelById(id);
                if(personel.IsVerify)
                    throw F_ExeptionFactory.MakeExeption("کاربر گرامی ! امکان تغییر نام کاربری وجود ندارد",
                        ((int)E_ErrorCodes.USERNAME_OR_PASSWORD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "UserName", E_LogType.SYSTEM_ERROR);
                bPersonels.ChangeUsername(id, Data.PhoneNumber);
                var code = new B_SmsAuthorise().AddCode(Data.PhoneNumber);
                new ApplicationHelper.Sms().SendSms(Data.PhoneNumber,code.ToString());
                return Json(PrepareSuccessResult(new B_SmsAuthorise().GetSmsDetailsId(Data.PhoneNumber)));
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

        [HttpPost]
        public IHttpActionResult VerifyUserName([FromBody]VerifyUserNameModel Data)
        {
            try
            {
                int id = validatePersonel();
                if(Data.Id == 0)
                    throw F_ExeptionFactory.MakeExeption("اطلاعات ورودی ناقص است",
                        ((int)E_ErrorCodes.WRONG_ID) + S_Seprators.ErrorFieldNameSeprator.ToString() + "VID", E_LogType.SYSTEM_ERROR);
                if(string.IsNullOrEmpty(Data.Code))
                    throw F_ExeptionFactory.MakeExeption("کد وارد شده صحیح نیست",
                        ((int)E_ErrorCodes.WRONG_CODE) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Code", E_LogType.SYSTEM_ERROR);
                var smsAuthorize = new B_SmsAuthorise().GetSmsDetails(Data.Id);
                if(smsAuthorize == null)
                    throw F_ExeptionFactory.MakeExeption("شناسه وارد شده صحیح نیست",
                        ((int)E_ErrorCodes.WRONG_ID) + S_Seprators.ErrorFieldNameSeprator.ToString() + "VID", E_LogType.SYSTEM_ERROR);
                if(smsAuthorize.Code != int.Parse(Data.Code))
                    throw F_ExeptionFactory.MakeExeption("کد فعال سازی وارد شده صحیح نمیباشد",
                        ((int)E_ErrorCodes.WRONG_CODE) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Code", E_LogType.SYSTEM_ERROR);
                new B_Personels().VerifyPersonel(id);
                return Json(PrepareSuccessResult(getPersonel(id)));
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

        [HttpPost]
        public IHttpActionResult GetRequestForPersonel(bool NotFinish)
        {
            try
            {
                int id = validatePersonel();
                return Json(PrepareSuccessResult(getRequestDetail(id,NotFinish)));
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

        [HttpPost]
        public IHttpActionResult ChangeStatus([FromBody]ChangeStatusModel Data)
        {
            try
            {
                int id = validatePersonel();
                E_PublicCategory.REQUEST_STATUS status = (E_PublicCategory.REQUEST_STATUS)Enum.Parse(typeof(E_PublicCategory.REQUEST_STATUS), Data.Status, true);
                var request = new B_ServicesRequests().GetServicesRequests(Data.RequestId);
                if (request != null)
                {
                    new B_ServicesRequests().ChangeStatus(Data.RequestId, request.UserId, status);
                    var cController = new ServicesRequestsController();
                    try
                    {
                        var hubConnection = new HubConnection(url: SignalRUrl);
                        var chat = hubConnection.CreateHubProxy(hubName: "Reqeust");

                        hubConnection.Start().Wait();

                        chat.Invoke<bool>("RequestChangedStatus", cController.getRequestDetail(request.Id,request.UserId), request.UserId);
                    }
                    catch { }
                }
                return Json(PrepareSuccessResult(getRequestDetail(request.Id)));
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

        [HttpPost]
        public IHttpActionResult CancelRequest([FromBody]CancelRequestModel Data)
        {
            try
            {
                int id = validatePersonel();
                return Json(PrepareSuccessResult(new B_ServicesRequests().CancelRequestPersonel(Data.ServiceRequestId, Data.DeleteMessageId, Data.DeleteMessage)));
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

        [HttpPost]
        public IHttpActionResult FinishRequest([FromBody]FinishRequestModel Data)
        {
            try
            {
                int id = validatePersonel();
                if(Data.RequestId == 0)
                    throw F_ExeptionFactory.MakeExeption("اطلاعات ورودی ناقص است",
                        ((int)E_ErrorCodes.WRONG_ID) + S_Seprators.ErrorFieldNameSeprator.ToString() + "RequestId", E_LogType.SYSTEM_ERROR);
                var request = new B_ServicesRequests().GetServicesRequests(Data.RequestId);
                if(Data.Items == null && Data.Items.Count == 0)
                    throw F_ExeptionFactory.MakeExeption("اطلاعات ورودی ناقص است",
                        ((int)E_ErrorCodes.WRONG_ID) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Items", E_LogType.SYSTEM_ERROR);
                var bItem = new B_ServicesRequestItems();
                foreach (var li in Data.Items)
                {
                    var category = new B_ServicesCategories().GetServicesCategories(li.Id);
                    if (category != null)
                        bItem.Add(new M_ServicesRequestItems
                        {
                            CreateDate = DateTime.Now,
                            RequestId = Data.RequestId,
                            UserType = E_PublicCategory.SYSTEM_USER_TYPE.PERSONEL,
                            Value = li.Value,
                            ImageName = category.ImageName,
                            ScorePerUnit = category.ScorePerUnit,
                            Title = category.Title,
                            Unit = category.Unit,
                            IsFailed = true,
                            UserId = id,
                        });
                }
                new B_ServicesRequests().FinishRequestPersonel(Data.RequestId);
                new B_ServicesRequests().ChangeStatus(Data.RequestId, request.UserId, E_PublicCategory.REQUEST_STATUS.WAIT_FOR_END_REQUEST_ACCEPT);
                var cController = new ServicesRequestsController();
                //try
                //{
                //    var hubConnection = new HubConnection(url: SignalRUrl);
                //    var chat = hubConnection.CreateHubProxy(hubName: "Reqeust");

                //    hubConnection.Start().Wait();

                //    chat.Invoke<bool>("RequestChangedStatus", cController.getRequestDetail(Data.RequestId, request.UserId), request.UserId);
                //}
                //catch { }
                try
                {
                    var hubConnection = new HubConnection(url: SignalRUrl);
                    var chat = hubConnection.CreateHubProxy(hubName: "Reqeust");

                    hubConnection.Start().Wait();

                    chat.Invoke<bool>("FinishRequestPersonel", cController.getRequestDetail(Data.RequestId, request.UserId,E_PublicCategory.SYSTEM_USER_TYPE.PERSONEL), request.UserId);
                }
                catch { }
                return Json(PrepareSuccessResult(getRequestDetail(Data.RequestId)));
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

        [HttpPost]
        public IHttpActionResult ResetPasswordSendVerificationCode([FromBody]ResetPasswordSendVerificationCodeModel Data)
        {
            try
            {
                if (string.IsNullOrEmpty(Data.PhoneNumber))
                    throw F_ExeptionFactory.MakeExeption("شماره تلفن وارد شده صحیح نیست",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PhoneNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                var personel = new B_Personels().GetPersonelsByPhone(Data.PhoneNumber);
                if (personel == null)
                    throw F_ExeptionFactory.MakeExeption("شماره تلفن وارد شده در سیستم ثبت نشده است",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PhoneNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (personel.IsDeleted == true)
                    throw F_ExeptionFactory.MakeExeption("چنین کاربری یافت نشد",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "User", Enums.Loging.E_LogType.SYSTEM_ERROR);
                int code = new B_SmsAuthorise().AddCode(Data.PhoneNumber);
                if (code != 0)
                {
                    new ApplicationHelper.Sms().SendSms(Data.PhoneNumber, code.ToString());
                    return Json(PrepareSuccessResult(new B_SmsAuthorise().GetSmsDetailsId(Data.PhoneNumber)));
                }
                else
                    throw F_ExeptionFactory.MakeExeption("متاسفانه کد فعال سازی ارسال نشد. مجددا تلاش کنید",
                         ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "VerificationCode", E_LogType.SYSTEM_ERROR);
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

        [HttpPost]
        public IHttpActionResult ResetPasswordConfirmVerificationCode([FromBody]ResetPasswordConfirmVerificationCodeModel Data)
        {
            try
            {
                if (string.IsNullOrEmpty(Data.NewPassword))
                    throw F_ExeptionFactory.MakeExeption("گذرواژه وارد شده صحیح نمی باشد",
                        ((int)E_ErrorCodes.USERNAME_OR_PASSWORD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Password", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (Data.VerificationId == 0)
                    throw F_ExeptionFactory.MakeExeption("اطلاعات ورودی معتبر نمیباشد",
                        ((int)E_ErrorCodes.EMPTY_FIELD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "VerificationId", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (Data.VerificationCode == 0)
                    throw F_ExeptionFactory.MakeExeption("اطلاعات ورودی معتبر نمیباشد",
                        ((int)E_ErrorCodes.EMPTY_FIELD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "VerificationCode", Enums.Loging.E_LogType.SYSTEM_ERROR);
                var smsAuthorize = new B_SmsAuthorise().GetSmsDetails(Data.VerificationId);
                if (smsAuthorize == null)
                    throw F_ExeptionFactory.MakeExeption("اطلاعات ورودی معتبر نمیباشد",
                        ((int)E_ErrorCodes.EMPTY_FIELD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "VerificationId", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (smsAuthorize.Code != Data.VerificationCode)
                    throw F_ExeptionFactory.MakeExeption("کد اعتبار سنجی صحیح نمیباشد",
                        ((int)E_ErrorCodes.WRONG_CODE) + S_Seprators.ErrorFieldNameSeprator.ToString() + "VerificationCode", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (new B_Personels().ChangePassword(smsAuthorize.PhoneNumber, Data.NewPassword))
                    return Json(PrepareSuccessResult(getPersonel(new B_Personels().GetPersonelsByPhone(smsAuthorize.PhoneNumber).Id)));
                else
                    throw F_ExeptionFactory.MakeExeption("خطا در تغییر گذرواژه",
                        ((int)E_ErrorCodes.USERNAME_OR_PASSWORD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Password", Enums.Loging.E_LogType.SYSTEM_ERROR);

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

        [HttpPost]
        public IHttpActionResult ChangeServiceRun(bool ServiceRun)
        {
            try
            {
                int id = validatePersonel();
                new B_Personels().ChangeServiceRun(id, ServiceRun);
                return Json(PrepareSuccessResult(getPersonel(id)));
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

        [HttpGet]
        public IHttpActionResult GetSetting()
        {
            try
            {
                int id = validatePersonel();
                return Json(PrepareSuccessResult(new
                {
                    RateItems = new List<object> {
                        new { Rate = 1, Items = new List<object> {
                            new {Id = (int)E_PublicCategory.FEEDBACK.DISRESPECTFUL_BEHAVIOR, Title = "رفتار نا محترمانه" },
                            new {Id = (int)E_PublicCategory.FEEDBACK.GET_TO_YOUR_PLACE_LATE, Title = "دیر رسیدن به محل شما" }
                        } },
                        new { Rate = 2, Items = new List<object> {
                            new {Id = (int)E_PublicCategory.FEEDBACK.DISRESPECTFUL_BEHAVIOR, Title = "رفتار نا محترمانه" },
                            new {Id = (int)E_PublicCategory.FEEDBACK.GET_TO_YOUR_PLACE_LATE, Title = "دیر رسیدن به محل شما" }
                        } },
                        new { Rate = 3, Items = new List<object> {
                            new {Id = (int)E_PublicCategory.FEEDBACK.DISRESPECTFUL_BEHAVIOR, Title = "رفتار نا محترمانه" },
                            new {Id = (int)E_PublicCategory.FEEDBACK.GET_TO_YOUR_PLACE_LATE, Title = "دیر رسیدن به محل شما" },
                            new {Id = (int)E_PublicCategory.FEEDBACK.ARRIVE_ON_TIME, Title = "به موقع رسیدن" },
                            new {Id = (int)E_PublicCategory.FEEDBACK.RESPECTFUL_BEHAVIOR, Title = "رفتار محترمانه" },
                            new {Id = (int)E_PublicCategory.FEEDBACK.SPEED_OF_SERVICE, Title = "سرعت در سرویس دهی" }
                        } },
                        new { Rate = 4, Items = new List<object> {
                            new {Id = (int)E_PublicCategory.FEEDBACK.ARRIVE_ON_TIME, Title = "به موقع رسیدن" },
                            new {Id = (int)E_PublicCategory.FEEDBACK.RESPECTFUL_BEHAVIOR, Title = "رفتار محترمانه" },
                            new {Id = (int)E_PublicCategory.FEEDBACK.SPEED_OF_SERVICE, Title = "سرعت در سرویس دهی" }
                        } },
                        new { Rate = 5, Items = new List<object> {
                            new {Id = (int)E_PublicCategory.FEEDBACK.ARRIVE_ON_TIME, Title = "به موقع رسیدن" },
                            new {Id = (int)E_PublicCategory.FEEDBACK.RESPECTFUL_BEHAVIOR, Title = "رفتار محترمانه" },
                            new {Id = (int)E_PublicCategory.FEEDBACK.SPEED_OF_SERVICE, Title = "سرعت در سرویس دهی" }
                        } },
                    },
                    AboutUs = "سبز باش سامانه هوشمند آموزش وفرهنگ سازی تفکیک زباله از مبدااست که با هدف حمایت از محیط ‌زیست و بهبود چرخه مدیریت پسماند، به عنوان پلي بین علاقمندان محیط زیست، دوست داران طبیعت و زیست بانان در مسير بهبود مدیریت شهری است. سبز باش هزینه های تفکیک زباله را به طرز چشم‌گیری کاهش می دهد و تاثیر به سزايي در حفظ و بقای محیط زیست و پيش بيني اینده ای سبز برای ما و فرزندانمان دارد.",
                    CancelRequestItem = new List<object> {
                            new {Id = (int)E_PublicCategory.FEEDBACK.DISRESPECTFUL_BEHAVIOR, Title = "رفتار نا محترمانه" },
                            new {Id = (int)E_PublicCategory.FEEDBACK.GET_TO_YOUR_PLACE_LATE, Title = "دیر رسیدن به محل شما" },
                            new {Id = (int)E_PublicCategory.FEEDBACK.PERSONAL_REASONS, Title = "دلایل شخصی" }
                        },
                    IsForceUpdate = bool.Parse(ConfigurationManager.AppSettings["IsForceUpdate"]),
                    AndroidVersion = int.Parse(ConfigurationManager.AppSettings["AndroidVersion"]),
                    ApkLink = ConfigurationManager.AppSettings["ApkLink"],
                    InviteFriend = new {
                        Message = ConfigurationManager.AppSettings["InviteFriendMessage"].Replace("#", ConfigurationManager.AppSettings["InviteFriendPoint"]),
                        ShareMessage = ConfigurationManager.AppSettings["ShareMessage"].Replace("#", new B_Personels().GetPersonelById(id).ReagentCode.ToString()),
                        Code = new B_Personels().GetPersonelById(id).ReagentCode
                    }
                }));
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

        [HttpPost]
        public IHttpActionResult GetPersonel()
        {
            try
            {
                int id = validatePersonel();
                return Json(PrepareSuccessResult(getPersonel(id)));
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

        [HttpPost]
        public IHttpActionResult AcceptRequest([FromBody]AcceptRequestModel Data)
        {
            try
            {
                int id = validatePersonel();
                B_ServicesRequests bRequest = new B_ServicesRequests();
                var request = bRequest.GetServicesRequests(Data.RequestId);
                if(request != null)
                {
                    if(request.PersonelId != null)
                        throw F_ExeptionFactory.MakeExeption("درخواست توسط راننده دیگری پذیرفته شده است",
                            ((int)E_ErrorCodes.ACCEPTED_BY_ANOTHER_DRIVER) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PersonelId", Enums.Loging.E_LogType.SYSTEM_ERROR);
                    bRequest.AsignUserToRequest(id, Data.RequestId);
                    new B_ServicesRequests().ChangeStatus(Data.RequestId, request.UserId, E_PublicCategory.REQUEST_STATUS.APPROVED_AND_REFERENCED);
                    var cController = new ServicesRequestsController();
                    try
                    {
                        var hubConnection = new HubConnection(url: SignalRUrl);
                        var chat = hubConnection.CreateHubProxy(hubName: "Reqeust");

                        hubConnection.Start().Wait();

                        chat.Invoke<bool>("AcceptRequestByCar", cController.getRequestDetail(request.Id, request.UserId), request.UserId);
                    }
                    catch { }
                    return Json(PrepareSuccessResult(getRequestDetail(Data.RequestId)));
                }
                else
                    throw F_ExeptionFactory.MakeExeption("خطا در اطلاعات ارسالی",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "request", Enums.Loging.E_LogType.SYSTEM_ERROR);
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

        [HttpGet]
        public IHttpActionResult GetRequestOnWork()
        {
            try
            {
                int id = validatePersonel();
                B_ServicesRequests bRequest = new B_ServicesRequests();
                var request = bRequest.GetServiceRequestOnWork(id);
                List<object> obj = new List<object>();
                if (request != null && request.Count > 0)
                    foreach (var li in request)
                        obj.Add(getRequestDetail(li.Id));
                return Json(PrepareSuccessResult(obj));
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

        #endregion Khalaj

        #region private funcs

        private object getPersonel(int id)
        {
            var personel =  new B_Personels().GetPersonelById(id);
            if (personel != null)
            {
                object car = null;
                object booth = null;
                if (personel.PersonelType == E_PublicCategory.PERSONEL_TYPE.SUM_CENER)
                    booth = getBooth(personel);
                else if (personel.PersonelType == E_PublicCategory.PERSONEL_TYPE.DRIVER)
                    car = getCar(personel);
                return new
                {
                    BirthDate = personel.BirthDate,
                    DeactiveDate = personel.DeactiveDate,
                    DeletedDate = personel.DeletedDate,
                    FirstName = personel.FirstName,
                    Gender = ((E_PublicCategory.GENDER)personel.Gender).ToString(),
                    Id = personel.Id,
                    ImageName = personel.ImageName,
                    IsActive = personel.IsActive,
                    IsDeleted = personel.IsDeleted,
                    LastName = personel.LastName,
                    LastOnline = personel.LastOnline,
                    MobileNumber = personel.MobileNumber,
                    PersonelRoleId = personel.PersonelRoleId,
                    PersonelType = ((E_PublicCategory.PERSONEL_TYPE)personel.PersonelType).ToString(),
                    RegisterDate = personel.RegisterDate,
                    UnicKey = personel.UnicKey,
                    UserName = personel.UserName,
                    Car = car,
                    Booth = booth,
                    ServiceRun = personel.ServiceRun
                };
            }
            else
                return null;
        }

        private object getBooth(M_Personels personel)
        {
            if (personel != null)
            {
                var boothInfo = new B_Booth().GetBoothByPersonelId(personel.Id);
                if (boothInfo != null)
                    return new
                    {
                        BoothInfo = new
                        {
                            Address = boothInfo.Address,
                            Description = boothInfo.Description,
                            Id = boothInfo.Id,
                            ImageUrl = GetImageAddress(boothInfo.ImageName, new E_FTPRoutes(BaseUrl).BOOTH_INFO),
                            Lat = boothInfo.Lat,
                            Lng = boothInfo.Lng,
                            Name = boothInfo.Name
                        }
                    };
                else
                    return new { Personel = personel };
            }
            else
                return null;
        }

        private object getCar(M_Personels personel)
        {
            if (personel != null)
            {
                var carInfo = new B_CarInfo().GetCarInfo(personel.Id);
                var loc = new B_Location().GetLocationByPersonelId(personel.Id);
                if (carInfo != null)
                    return (new
                    {
                        CarInfo = new
                        {
                            Name = carInfo.Name,
                            Color = carInfo.Color,
                            Type = ((E_PublicCategory.CAR_TYPE)carInfo.Type).ToString(),
                            TagNational = carInfo.TagNational,
                            TagMiddle = carInfo.TagMiddle,
                            TagLast = carInfo.TagLast,
                            TagFirst = carInfo.TagFirst,
                            TagColor = ((E_PublicCategory.TAG_COLOR)carInfo.TagColor).ToString(),
                            ImageUrl = GetImageAddress(carInfo.Image, new E_FTPRoutes(BaseUrl).CAR_INFO),
                            Id = carInfo.Id,
                            Capacity = carInfo.Capacity
                        },
                        Location = loc != null ? new
                        {
                            Bearing = loc.Bearing,
                            Date = loc.Date,
                            Lat = loc.Lat,
                            Lng = loc.Lng
                        } : null
                    });
            }
            else
                return null;
            return null;
        }

        private object getCar(int personelId)
        {
            var personel = new B_Personels().GetPersonelById(personelId);
            if (personel != null)
            {
                var carInfo = new B_CarInfo().GetCarInfo(personel.Id);
                var loc = new B_Location().GetLocationByPersonelId(personel.Id);
                if (carInfo != null)
                    return (new
                    {
                        CarInfo = new
                        {
                            Name = carInfo.Name,
                            Color = carInfo.Color,
                            Type = ((E_PublicCategory.CAR_TYPE)carInfo.Type).ToString(),
                            TagNational = carInfo.TagNational,
                            TagMiddle = carInfo.TagMiddle,
                            TagLast = carInfo.TagLast,
                            TagFirst = carInfo.TagFirst,
                            TagColor = ((E_PublicCategory.TAG_COLOR)carInfo.TagColor).ToString(),
                            ImageUrl = GetImageAddress(carInfo.Image, new E_FTPRoutes(BaseUrl).CAR_INFO),
                            Id = carInfo.Id,
                            Capacity = carInfo.Capacity
                        },
                        Location = loc != null ? new
                        {
                            Bearing = loc.Bearing,
                            Date = loc.Date,
                            Lat = loc.Lat,
                            Lng = loc.Lng
                        } : null
                    });
            }
            else
                return null;
            return null;
        }

        private object getRequestDetail(int personelId,bool NotFinish)
        {
            List<object> obj = new List<object>();
            var requests = new B_ServicesRequests().GetServicesRequestsForPersonel(personelId,NotFinish);
            if(requests != null && requests.Count > 0)
                foreach(var li in requests)
                {
                    var item = new B_ServicesRequestItems().GetItems(li.Id);
                    var items = new List<object>();
                    foreach (var lii in item)
                    {
                        items.Add(new
                        {
                            Id = lii.Id,
                            Value = lii.Value,
                            Title = lii.Title,
                            Unit = lii.Unit,
                            PointPerUnit = lii.ScorePerUnit,
                            ParentId = new B_ServicesCategories().GetParentId(lii.Title),
                            CategoryId = lii.CategoryId
                        });
                    }
                    var date = new B_Pouriya_Date().GetDate(li.Pouriya_DateId.HasValue ? (int)li.Pouriya_DateId : 0);
                    obj.Add(new
                    {
                        Address = new AddressModel { Address = li.Address, Lng = li.GeographicalCoordinates.Split(',')[1], Lat = li.GeographicalCoordinates.Split(',')[0], Plaque = li.PlaqueNumber, Unit = li.UnitNumber },
                        DayId = li.Pouriya_DateId,
                        Description = li.Description,
                        TimeId = li.Pouriya_TimeId,
                        Type = li.Pouriya_Type,
                        Items = items,
                        Status = ((E_PublicCategory.REQUEST_STATUS)li.Status).ToString(),
                        FinishDate = li.FinishDate,
                        Id = li.Id,
                        CreationTime = li.CreateDate,
                        Price = 0,
                        TimeStr = new B_PublicCategory().GetCategoryTitle(li.Pouriya_TimeId.HasValue ? (int)li.Pouriya_TimeId : 0),
                        DayStr = date != null ? date.Date.ConvertToPesianDateName() : string.Empty,
                        UserInfoModel = getUser(li.UserId)
                    });
                }
            return obj;
        }
        
        public object getRequestDetail(int RequestId,E_PublicCategory.SYSTEM_USER_TYPE UserType = E_PublicCategory.SYSTEM_USER_TYPE.NORMAL_USER)
        {
            object obj = new object();
            var requests = new B_ServicesRequests().GetServicesRequests(RequestId);
            if (requests != null)
            {
                    var item = new B_ServicesRequestItems().GetItems(requests.Id).Where(x=>x.UserType == UserType).ToList();
                    var items = new List<object>();
                    long price = 0;
                    foreach (var lii in item)
                    {
                        if(lii.ScorePerUnitDriver.HasValue)
                            price += (long)lii.ScorePerUnitDriver * (lii.Value.HasValue ? (long)(lii.Value) : 0);
                        items.Add(new
                        {
                            Id = lii.Id,
                            Value = lii.Value,
                            Title = lii.Title,
                            Unit = lii.Unit,
                            PointPerUnit = lii.ScorePerUnit,
                            ParentId = new B_ServicesCategories().GetParentId(lii.Title),
                            CategoryId = lii.CategoryId
                        });
                    }
                    var date = new B_Pouriya_Date().GetDate(requests.Pouriya_DateId.HasValue ? (int)requests.Pouriya_DateId : 0);
                    obj = new
                    {
                        Address = new AddressModel { Address = requests.Address, Lng = requests.GeographicalCoordinates.Split(',')[1], Lat = requests.GeographicalCoordinates.Split(',')[0], Plaque = requests.PlaqueNumber, Unit = requests.UnitNumber },
                        DayId = requests.Pouriya_DateId,
                        Description = requests.Description,
                        TimeId = requests.Pouriya_TimeId,
                        Type = requests.Pouriya_Type,
                        Items = items,
                        Status = ((E_PublicCategory.REQUEST_STATUS)requests.Status).ToString(),
                        FinishDate = requests.FinishDate,
                        Id = requests.Id,
                        CreationTime = requests.CreateDate,
                        TotalPoint = price,
                        TimeStr = new B_PublicCategory().GetCategoryTitle(requests.Pouriya_TimeId.HasValue ? (int)requests.Pouriya_TimeId : 0),
                        DayStr = date != null ? date.Date.ConvertToPesianDateName() : string.Empty,
                        UserInfoModel = getUser(requests.UserId)
                    };
                }
            return obj;
        }

        private object getUser(int userId)
        {
            B_Users bUser = new B_Users();

            var user = bUser.GetUsers(userId);
            if (user == null)
                throw F_ExeptionFactory.MakeExeption("کاربری یافت نشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "User", Enums.Loging.E_LogType.SYSTEM_ERROR);

            var services = new B_ServicesRequests().GetServicesRequests(userId, new List<int>());

            List<M_Addresses> mAddress = new B_Addresses().GetAddresses(userId);
            List<object> objAddress = new List<object>();
            if (mAddress != null && mAddress.Count > 0)
                foreach (var li in mAddress)
                {
                    objAddress.Add(new
                    {
                        Lat = li.Lat,
                        Lng = li.Lng,
                        Address = li.Address,
                        Type = ((E_PublicCategory.INSTITUTE_TYPE)li.Type).ToString(),
                        Title = li.Title,
                        Plaque = li.Plaque,
                        Unit = li.Unit
                    });
                }
            var point = new B_ServicesRequestItems().GetSumUserPoint(userId, E_PublicCategory.SYSTEM_USER_TYPE.NORMAL_USER);
            var type = "";
            if (user.InstituteType == E_PublicCategory.INSTITUTE_TYPE.HOME)
                type = "HOME";
            else if (user.InstituteType == E_PublicCategory.INSTITUTE_TYPE.FACTORY)
                type = "FACTOR";
            else if (user.InstituteType == E_PublicCategory.INSTITUTE_TYPE.COMMERCIAL)
                type = "BUSSUNESS";
            else if (user.InstituteType == E_PublicCategory.INSTITUTE_TYPE.OFFICIAL)
                type = "OFFICE";
            return new
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Id,
                Phonenumber = user.MobileNumber,
                Email = user.Email,
                Point = point,
                Creadit = point * 100,
                ImageUrl = GetImageAddress(user.ImageName, new E_FTPRoutes(BaseUrl).USERS),
                UserType = type,
                ReagentCode = user.ReagentCode
            };
        }

        #endregion

        #region RquestModels
        public class RequestModelLogin
        {
            public string PhoneNumber { get; set; }
            public string Password { get; set; }
        }

        public class RequestModelEditPersonel
        {
            public int Id { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string UserName { get; set; }

            public string Password { get; set; }

            public E_PublicCategory.GENDER Gender { get; set; }
            
            public DateTime BirthDate { get; set; }

            public string MobileNumber { get; set; }
            
            public string UnicKey { get; set; }
        }

        public class LoginModel
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        public class ChangeUserNameModel
        {
            public string PhoneNumber { get; set; }
        }

        public class VerifyUserNameModel
        {
            public int Id { get; set; }
            public string Code { get; set; }
        }

        public class AddressModel
        {
            public string Lat { get; set; }
            public string Lng { get; set; }
            public string Address { get; set; }
            public string Unit { get; set; }
            public string Plaque { get; set; }
        }

        public class CancelRequestModel
        {
            public int ServiceRequestId { get; set; }
            public int DeleteMessageId { get; set; }
            public string DeleteMessage { get; set; }
        }

        public class FinishRequestModel
        {
            public int RequestId { get; set; }
            public List<Items> Items { get; set; }
        }

        public class ChangeStatusModel
        {
            public int RequestId { get; set; }
            public string Status { get; set; }
        }

        public class Items
        {
            public int Id { get; set; }
            public int Value { get; set; }
        }

        public class ResetPasswordSendVerificationCodeModel
        {
            public string PhoneNumber { get; set; }
        }

        public class ResetPasswordConfirmVerificationCodeModel
        {
            public int VerificationId { get; set; }
            public string NewPassword { get; set; }
            public int VerificationCode { get; set; }
        }

        public class AcceptRequestModel
        {
            public int RequestId { get; set; }
        }
        #endregion RquestModels
    }
}