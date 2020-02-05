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
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Device.Location;
using System.Reflection;
using System.Web;
using Microsoft.AspNet.SignalR.Client;

namespace Shahrdari.WebApplication.WebApis
{
    public class UsersController : BaseController
    {
        [HttpPost]
        public IHttpActionResult AddUser([FromBody]M_Users User)
        {
            try
            {
                B_Users bUser = new B_Users();
                User = bUser.Add(User);
                return Json(PrepareSuccessResult(User));
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
        public IHttpActionResult EditUser([FromBody]M_Users User)
        {
            try
            {
                validateUser();
                B_Users bUser = new B_Users();
                User.Id = int.Parse(Request.Headers.GetValues("UserId").FirstOrDefault());
                User.UnicKey = Request.Headers.GetValues("UnicKey").FirstOrDefault();
                bUser.Edit(User, true);
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

        [HttpPost]
        public IHttpActionResult DeleteUser()
        {
            try
            {
                validateUser();
                B_Users bUser = new B_Users();
                bUser.Delete(int.Parse(Request.Headers.GetValues("UserId").FirstOrDefault()), Request.Headers.GetValues("UnicKey").FirstOrDefault());
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

        [HttpPost]
        public IHttpActionResult SendVerifyCode([FromBody]RequestModelGetUserByPhone RquestDatas)
        {
            try
            {
                return Json(PrepareSuccessResult(sendVerifyCode(RquestDatas.PhoneNumber)));
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
        public IHttpActionResult GetUserByPhone([FromBody]RequestModelGetUserByPhone RquestDatas)
        {
            try
            {
                B_Users bUser = new B_Users();
                var user = bUser.GetUsers(RquestDatas.PhoneNumber);
                if (user == null)
                    throw F_ExeptionFactory.MakeExeption("کاربری یافت نشد",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "User", E_LogType.SYSTEM_ERROR);
                new B_SmsAuthorise().IsValidCode(RquestDatas.Code, RquestDatas.PhoneNumber);
                return Json(PrepareSuccessResult(user));
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
        public IHttpActionResult GetInviteMessage()
        {
            try
            {
                validateUser();
                int id = int.Parse(Request.Headers.GetValues("UserId").FirstOrDefault());
                var user = new B_Users().GetUsers(id);
                var res = System.Configuration.ConfigurationManager.AppSettings["MessageText"]
                    .Replace("#Name#", user.FirstName + " " + user.LastName)
                    .Replace("#Id#", id.ToString());
                return Json(PrepareSuccessResult(res));
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
        public IHttpActionResult GetUser()
        {
            try
            {
                B_Users bUser = new B_Users();

                int id = int.Parse(Request.Headers.GetValues("UserId").FirstOrDefault());
                string unicKey = Request.Headers.GetValues("UnicKey").FirstOrDefault();

                var user = bUser.GetUsers(id, unicKey);
                if (user == null)
                    throw F_ExeptionFactory.MakeExeption("کاربری یافت نشد",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "User", Enums.Loging.E_LogType.SYSTEM_ERROR);

                var services = new B_ServicesRequests().GetServicesRequests(id, new List<int>());

                return Json(PrepareSuccessResult(new
                {
                    BirthDate = !user.BirthDate.HasValue ? "" : user.BirthDate.Value.ConvertMiladiToShamsi(),
                    BirthDateStr = !user.BirthDate.HasValue ? 0 : TimeSpan.FromTicks(user.BirthDate.Value.Ticks).TotalMilliseconds,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    Gender = user.Gender,
                    ImageName = user.ImageName,
                    LastName = user.LastName,
                    MobileNumber = user.MobileNumber,
                    ServiceFinishedCount = services.Where(c => c.Status == E_PublicCategory.REQUEST_STATUS.CLOSED).Count(),
                    ServiceOpenCount = services.Where(c => c.Status != E_PublicCategory.REQUEST_STATUS.CLOSED).Count()
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
        public IHttpActionResult AddAddress([FromBody]M_Addresses Address)
        {
            try
            {
                validateUser();
                Address.UserId = int.Parse(Request.Headers.GetValues("UserId").FirstOrDefault());
                Address.Type = E_PublicCategory.INSTITUTE_TYPE.FAVORITE;
                Address = new B_Addresses().Add(Address);
                return Json(PrepareSuccessResult(getUser(Address.UserId)));
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
        public IHttpActionResult EditAddress([FromBody]M_Addresses Address)
        {
            try
            {
                validateUser();
                Address.UserId = int.Parse(Request.Headers.GetValues("UserId").FirstOrDefault());
                new B_Addresses().Edit(Address);
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

        [HttpPost]
        public IHttpActionResult DeleteAddress(int AddressId)
        {
            try
            {
                validateUser();
                new B_Addresses().Delete(AddressId);
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

        [HttpGet]
        public IHttpActionResult GetAddress()
        {
            try
            {
                validateUser();
                var res = new B_Addresses().GetAddresses(int.Parse(Request.Headers.GetValues("UserId").FirstOrDefault()));
                return Json(PrepareSuccessResult(res));
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

        #region Khalaj Changes

        [HttpPost]
        public IHttpActionResult VerifyByPass([FromBody]RequestModelVerifyByPass Data)
        {
            try
            {
                B_Users bUser = new B_Users();
                if (Data.Type == "Password")
                {
                    var user = bUser.GetUsers(Data.PhoneNumber);
                    if (user == null)
                        throw F_ExeptionFactory.MakeExeption("کاربری یافت نشد",
                            ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "User", E_LogType.SYSTEM_ERROR);
                    if (user.Password != Data.Data)
                        throw F_ExeptionFactory.MakeExeption("نام کاربری یا گذرواژه صحیح نمیباشد",
                            ((int)E_ErrorCodes.USERNAME_OR_PASSWORD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "User", E_LogType.SYSTEM_ERROR);
                    else
                    {
                        var code = new B_SmsAuthorise().AddCode(Data.PhoneNumber);
                        new ApplicationHelper.Sms().SendSms(Data.PhoneNumber, code.ToString());
                        return Json(PrepareSuccessResult(code));
                    }
                }
                else
                    throw F_ExeptionFactory.MakeExeption("نوع ورودی صحیح نمیباشد",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "User", E_LogType.SYSTEM_ERROR);
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
        public IHttpActionResult VerifyPhoneNumber(int VId, [FromBody]RequestModelVerifyPhoneNumber Data)
        {
            try
            {
                if (VId == 0)
                    throw F_ExeptionFactory.MakeExeption("شناسه وارد شده صحیح نمیباشد",
                        ((int)E_ErrorCodes.WRONG_ID) + S_Seprators.ErrorFieldNameSeprator.ToString() + "User", E_LogType.SYSTEM_ERROR);
                if (new B_SmsAuthorise().IsValidCode(VId, Data.Code))
                {
                    var res = new B_SmsAuthorise().GetSmsDetails(VId);
                    if (res != null)
                    {
                        new B_Users().ActiveUser(res.PhoneNumber);
                        new B_SmsAuthorise().DeleteVerificationCode(VId);
                        return Json(PrepareSuccessResult(getUser(Data.UserId)));
                    }
                    else
                        return Json(PrepareSuccessResult("کد وارد شده صحیح نمیباشد"));
                }
                else
                    throw F_ExeptionFactory.MakeExeption("کد فعالسازی وارد شده صحیح نمیباشد",
                    ((int)E_ErrorCodes.WRONG_CODE) + S_Seprators.ErrorFieldNameSeprator.ToString() + "User", E_LogType.SYSTEM_ERROR);
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
        public IHttpActionResult ResendSms(int VId)
        {
            try
            {
                if (VId == 0)
                    throw F_ExeptionFactory.MakeExeption("شناسه وارد شده صحیح نمیباشد",
                        ((int)E_ErrorCodes.WRONG_ID) + S_Seprators.ErrorFieldNameSeprator.ToString() + "User", E_LogType.SYSTEM_ERROR);
                var code = new B_SmsAuthorise().ResendVerificationCode(VId);
                var smsAuthorize = new B_SmsAuthorise().GetSmsDetails(VId);
                if(code != 0)
                {
                    new ApplicationHelper.Sms().SendSms(smsAuthorize.PhoneNumber,code.ToString());
                    return Json(PrepareSuccessResult("کد فعال سازی با موفقیت ارسال شد"));
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

        [HttpPatch]
        public IHttpActionResult Users([FromBody]RequestModelUser Data)
        {
            try
            {
                int UId = validateUser();
                M_Addresses mAddress = new M_Addresses();
                B_Users bUser = new B_Users();
                if (UId == 0)
                {
                    var tmpUser = bUser.GetUsers(Data.PhoneNumber);
                    if (tmpUser != null && tmpUser.Id != 0)
                    {
                        if (tmpUser.IsActive)
                            throw F_ExeptionFactory.MakeExeption($"شماره تلفن {tmpUser.MobileNumber} قبلا در سیستم ثب شده",
                                ((int)E_ErrorCodes.HAVE_USER_MOBILE_NUMBER) + S_Seprators.ErrorFieldNameSeprator.ToString() + "MobileNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                        else
                        {
                            M_Users mUser = new M_Users();
                            mUser = mapUsers(Data, tmpUser.Id);
                            mUser.UnicKey = tmpUser.UnicKey;
                            bUser.EditForSignup(mUser);
                            var user = bUser.GetUsers(tmpUser.Id);
                            if (Data.Address != null && Data.Address.Count > 0)
                            {
                                new B_Addresses().Delete(tmpUser.Id, true);
                                foreach (var li in Data.Address)
                                {
                                    mAddress.Address = li.DetailAddress;
                                    mAddress.Lat = li.Lat;
                                    mAddress.Lng = li.Lng;
                                    mAddress.Type = li.Type;
                                    mAddress.UserId = UId;
                                }
                                new B_Addresses().Add(mAddress);
                            }
                            return Json(PrepareSuccessResult(new
                            {
                                activationId = sendVerifyCode(tmpUser.MobileNumber) == true ?
                                new B_SmsAuthorise().GetSmsDetailsId(tmpUser.MobileNumber) : 0,
                                userId = mUser.Id
                            }));
                        }
                    }
                    else
                    {
                        M_Users mUser = new M_Users();
                        mUser = mapUsers(Data, UId);
                        var res = bUser.Add(mUser);
                        if (res != null)
                        {
                            if (Data.Address != null && Data.Address.Count > 0)
                            {
                                foreach (var li in Data.Address)
                                {
                                    mAddress.Address = li.DetailAddress;
                                    mAddress.Lat = li.Lat;
                                    mAddress.Lng = li.Lng;
                                    mAddress.Type = li.Type;
                                    mAddress.UserId = res.Id;
                                }
                                new B_Addresses().Add(mAddress);
                            }
                            return Json(PrepareSuccessResult(new
                            {
                                activationId = sendVerifyCode(res.MobileNumber) == true ?
                                    new B_SmsAuthorise().GetSmsDetailsId(res.MobileNumber) : 0,
                                userId = res.Id
                            }));
                        }
                        else
                            return Json(PrepareSuccessResult(null));
                    }
                }
                else
                {
                    var user = bUser.GetUsers(UId);
                    var mUser = mapUsers(Data, UId);
                    mUser.UnicKey = user.UnicKey;
                    bUser.Edit(mUser);
                    //new B_Addresses().Delete(UId, true);
                    //if (Data.Address != null && Data.Address.Count > 0)
                    //{
                    //    foreach (var li in Data.Address)
                    //    {
                    //        mAddress.Address = li.DetailAddress;
                    //        mAddress.Lat = li.Lat;
                    //        mAddress.Lng = li.Lng;
                    //        mAddress.Type = li.Type;
                    //        mAddress.UserId = UId;
                    //        mAddress.Plaque = li.Plaque;
                    //        mAddress.Unit = li.Unit;
                    //    }
                    //    new B_Addresses().Add(mAddress);
                    //}
                    return Json(PrepareSuccessResult(
                        getUser(UId)
                    ));
                }
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
        public IHttpActionResult GetUser(int UId)
        {
            try
            {
                return Json(PrepareSuccessResult(getUser(UId)));
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
        public IHttpActionResult GetCarOrBooth([FromBody]GetCarOrBoothModel Data)
        {
            try
            {
                if (Data.Type == "BOOTH")
                {
                    List<object> obj = new List<object>();
                    var booth = new B_Booth().GetBoothes(Data.Lat,Data.Lng);
                    if (booth != null && booth.Count > 0)
                        foreach (var li in booth)
                        {
                            var Personel = new B_Personels().GetPersonel(li.PersonelId);
                            var point = new B_ServicesRequestItems().GetSumUserPoint(li.PersonelId,E_PublicCategory.SYSTEM_USER_TYPE.PERSONEL);
                            obj.Add(new
                            {
                                Name = li.Name,
                                Address = li.Address,
                                Discription = li.Description,
                                Lat = li.Lat,
                                Lng = li.Lng,
                                ImageName = li.ImageName,
                                IsActive = li.IsActive,
                                IsDelete = li.IsDelete,
                                FirstName = Personel.FirstName,
                                LastName = Personel.LastName,
                                Id = li.Id,
                                Phonenumber = Personel.MobileNumber,
                                Point = point,
                                Creadit = point * 100,
                                ImageUrl = GetImageAddress(Personel.ImageName, new E_FTPRoutes(BaseUrl).PERSONELS),
                                UserType = Personel.PersonelType
                            });
                        }
                    return Json(PrepareSuccessResult(obj));
                }
                else if (Data.Type == "CAR")
                {
                    List<object> obj = new List<object>();
                    var nearDrivers = new B_Location().GetNearDrivers(Data.Lat, Data.Lng);
                    if (nearDrivers != null && nearDrivers.Count > 0)
                        foreach(var li in nearDrivers)
                        {
                            var CarInfo = new B_CarInfo().GetCarInfo(li.PersonelId);
                            var personel = new B_Personels().GetPersonel(li.PersonelId);
                            if (CarInfo != null && personel != null)
                                obj.Add(new
                                {
                                    location = new
                                    {
                                        Bearing = li.Bearing,
                                        Date = li.Date,
                                        Lat = li.Lat,
                                        Lng = li.Lng
                                    },
                                    TagColor = CarInfo.TagColor,
                                    TagFirst = CarInfo.TagFirst,
                                    TagLast = CarInfo.TagLast,
                                    TagMiddle = CarInfo.TagMiddle,
                                    TagNational = CarInfo.TagNational,
                                    Name = CarInfo.Name,
                                    Image = CarInfo.Image,
                                    Color = CarInfo.Color,
                                    Capacity = CarInfo.Capacity,
                                    Type = CarInfo.Type,
                                    DriverFirstName = personel.FirstName,
                                    DriverLastName = personel.LastName,
                                    DriverPhoneNumber = personel.MobileNumber,
                                    DriverImage = personel.ImageName
                                });
                            else
                                continue;
                        }
                    else
                        return Json(PrepareSuccessResult(null));
                    return Json(PrepareSuccessResult(obj));
                }
                return Json(PrepareSuccessResult(null));
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
        public IHttpActionResult Login([FromBody]RequestModelLogin Data)
        {
            try
            {
                if(string.IsNullOrEmpty(Data.UserName))
                    throw F_ExeptionFactory.MakeExeption("نام کاربری را وارد کنید",
                        ((int)E_ErrorCodes.USERNAME_OR_PASSWORD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "UserName", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (string.IsNullOrEmpty(Data.Password))
                    throw F_ExeptionFactory.MakeExeption("گذرواژه را وارد کنید",
                        ((int)E_ErrorCodes.USERNAME_OR_PASSWORD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Password", Enums.Loging.E_LogType.SYSTEM_ERROR);
                var user = new B_Users().GetUsers(Data.UserName);
                if(user == null)
                    throw F_ExeptionFactory.MakeExeption("چنین کاربری یافت نشد",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "User", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if(user.IsDeleted == true)
                    throw F_ExeptionFactory.MakeExeption("چنین کاربری یافت نشد",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "User", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (user.Password == Data.Password)
                    return Json(PrepareSuccessResult(getUser(user.Id)));
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
        public IHttpActionResult UserPayment([FromBody]RequestModelUserPayment Data)
        {
            try
            {
                if (Data.UserId == 0)
                    throw F_ExeptionFactory.MakeExeption("کاربر مورد نظر یافت نشد",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "User", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (Data.Point == 0)
                    throw F_ExeptionFactory.MakeExeption("مبلغ را وارد کنید",
                        ((int)E_ErrorCodes.EMPTY_AMOUNT) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Amount", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (Data.Type == E_PublicCategory.PAYMENT_TYPE.CASH_WITHDRAWAL && Data.AccountId == 0)
                    throw F_ExeptionFactory.MakeExeption("شماره حساب مورد نظر یافت نشد",
                        ((int)E_ErrorCodes.ACCOUNT_ID) + S_Seprators.ErrorFieldNameSeprator.ToString() + "AccountId", Enums.Loging.E_LogType.SYSTEM_ERROR);
                var point = new B_ServicesRequestItems().GetSumUserPoint(Data.UserId, E_PublicCategory.SYSTEM_USER_TYPE.NORMAL_USER);
                if (point < Data.Point)
                    throw F_ExeptionFactory.MakeExeption("امتیاز وارد شده از میزان اعتبار شما بیشتر است",
                        ((int)E_ErrorCodes.EMPTY_AMOUNT) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Amount", Enums.Loging.E_LogType.SYSTEM_ERROR);
                var res = new B_UserPayment().AddUserPayment(new M_UserPayment {
                    AccountId = Data.AccountId,
                    Point = Data.Point,
                    CreateDate = DateTime.Now,
                    Message = string.Empty,
                    ModifyDate = DateTime.Now,
                    Status = E_PublicCategory.PAYMENT_STATUS.NEW,
                    UserId = Data.UserId,
                    Type = Data.Type,
                    IsDeleted = false,
                    UserType = E_PublicCategory.SYSTEM_USER_TYPE.NORMAL_USER
                });
                if (res)
                    return Json(PrepareSuccessResult(true));
                else
                    throw F_ExeptionFactory.MakeExeption("متاسفاته امتیاز شما ثبت نشد",
                        ((int)E_ErrorCodes.POINT) + S_Seprators.ErrorFieldNameSeprator.ToString() + "AccountId", Enums.Loging.E_LogType.SYSTEM_ERROR);
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
        public IHttpActionResult ChangePassword([FromBody]ChangePasswordModel Data)
        {
            try
            {
                int userId = validateUser();
                if (string.IsNullOrEmpty(Data.OldPassword))
                    throw F_ExeptionFactory.MakeExeption("گذرواژه را وارد کنید",
                        ((int)E_ErrorCodes.USERNAME_OR_PASSWORD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "OldPassword", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (string.IsNullOrEmpty(Data.NewPassword))
                    throw F_ExeptionFactory.MakeExeption("گذرواژه جدید را وارد کنید",
                        ((int)E_ErrorCodes.USERNAME_OR_PASSWORD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "NewPassword", Enums.Loging.E_LogType.SYSTEM_ERROR);
                var user = new B_Users().GetUsers(userId);
                if(user.Password != Data.OldPassword)
                    throw F_ExeptionFactory.MakeExeption("گذرواژه وارد شده صحیح نیست",
                        ((int)E_ErrorCodes.USERNAME_OR_PASSWORD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "OldPassword", Enums.Loging.E_LogType.SYSTEM_ERROR);
                new B_Users().ChangePassword(user.MobileNumber, Data.NewPassword);
                return Json(PrepareSuccessResult(getUser(user.Id)));
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
        public IHttpActionResult AddAcount([FromBody]M_Accounts Data)
        {
            try
            {
                validateUser();
                var userId = int.Parse(Request.Headers.GetValues("UserId").FirstOrDefault());
                Data.IsDelete = false;
                Data.UserId = userId;
                new B_Accounts().AddAccount(Data);
                return Json(PrepareSuccessResult(new B_Accounts().GetAccounts(userId, true)));
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
        public IHttpActionResult GetAccounts()
        {
            try
            {
                validateUser();
                var userId = int.Parse(Request.Headers.GetValues("UserId").FirstOrDefault());
                return Json(PrepareSuccessResult(new B_Accounts().GetAccounts(userId, true)));
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
        public IHttpActionResult EditAccount([FromBody]M_Accounts Data)
        {
            try
            {
                validateUser();
                new B_Accounts().EditAccount(Data);
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

        [HttpPost]
        public IHttpActionResult DeleteAccount(int Id)
        {
            try
            {
                validateUser();
                var userId = int.Parse(Request.Headers.GetValues("UserId").FirstOrDefault());
                return Json(PrepareSuccessResult(new B_Accounts().DeleteAccount(Id, userId)));
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
                if(string.IsNullOrEmpty(Data.PhoneNumber))
                    throw F_ExeptionFactory.MakeExeption("شماره تلفن وارد شده صحیح نیست",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PhoneNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                var user = new B_Users().GetUsers(Data.PhoneNumber);
                if (user == null)
                    throw F_ExeptionFactory.MakeExeption("شماره تلفن وارد شده در سیستم ثبت نشده است",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PhoneNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (user.IsDeleted == true)
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
                if(smsAuthorize == null)
                    throw F_ExeptionFactory.MakeExeption("اطلاعات ورودی معتبر نمیباشد",
                        ((int)E_ErrorCodes.EMPTY_FIELD) + S_Seprators.ErrorFieldNameSeprator.ToString() + "VerificationId", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if(smsAuthorize.Code != Data.VerificationCode)
                    throw F_ExeptionFactory.MakeExeption("کد اعتبار سنجی صحیح نمیباشد",
                        ((int)E_ErrorCodes.WRONG_CODE) + S_Seprators.ErrorFieldNameSeprator.ToString() + "VerificationCode", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (new B_Users().ChangePassword(smsAuthorize.PhoneNumber, Data.NewPassword))
                    return Json(PrepareSuccessResult(getUser(new B_Users().GetUsers(smsAuthorize.PhoneNumber).Id)));
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
        public IHttpActionResult RequestFinishFromUser([FromBody]RequestFinishFromUserModel Data)
        {
            try
            {
                var id = validateUser();
                B_ServicesRequests bRq = new B_ServicesRequests();
                bRq.FinishRequestUser(Data.RequestId, Data.Comment, Data.Rate, Data.MessageId);
                var request = bRq.GetServicesRequests(Data.RequestId);
                var cController = new PersonelsController();
                try
                {
                    var hubConnection = new HubConnection(url: SignalRUrl);
                    var chat = hubConnection.CreateHubProxy(hubName: "Reqeust");

                    hubConnection.Start().Wait();

                    chat.Invoke<bool>("RequestChangedStatus", cController.getRequestDetail(Data.RequestId), id);
                }
                catch { }
                var rController = new ServicesRequestsController();
                return Json(PrepareSuccessResult(rController.getRequestDetail(Data.RequestId, id)));
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

        public class RequestModelVerifyByPass
        {
            public string PhoneNumber { get; set; }
            public string Type { get; set; }
            public string Data { get; set; }
        }
        public class RequestFinishFromUserModel
        {
            public int RequestId { get; set; }
            public string Comment { get; set; }
            public int Rate { get; set; }
            public List<int> MessageId { get; set; }
        }
        public class RequestModelVerifyPhoneNumber
        {
            public int Code { get; set; }
            public int UserId { get; set; }
        }
        public class RequestModelUser
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Type { get; set; }
            public string PhoneNumber { get; set; }
            public int CodeIntroduser { get; set; }
            public string Password { get; set; }
            public List<address> Address { get; set; }
            public string Email { get; set; }
        }
        public class address
        {
            public string Lat { get; set; }
            public string Lng { get; set; }
            public string DetailAddress { get; set; }
            public E_PublicCategory.INSTITUTE_TYPE Type { get; set; }
            public string Title { get; set; }
            public int Unit { get; set; }
            public string Plaque { get; set; }
        }
        public class GetCarOrBoothModel
        {
            public double Lat { get; set; }
            public double Lng { get; set; }
            public int Bring { get; set; }
            public string Type { get; set; }
        }
        public class RequestModelUserPayment
        {
            public int UserId { get; set; }
            public long Point { get; set; }
            public int AccountId { get; set; }
            public E_PublicCategory.PAYMENT_TYPE Type { get; set; }
        }
        public class RequestModelLogin
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }
        public class ChangePasswordModel
        {
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
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
        #endregion Khalaj Changes

        #region Private Functions

        private bool sendVerifyCode(string phoneNumber)
        {
            B_Users bUser = new B_Users();
            var user = bUser.GetUsers(phoneNumber);
            if (user == null)
                throw F_ExeptionFactory.MakeExeption("کاربری یافت نشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "User", Enums.Loging.E_LogType.SYSTEM_ERROR);
            var code = new B_SmsAuthorise().AddCode(phoneNumber);
            var sender = "10004346";
            var receptor = phoneNumber;
            var message = "کد فعالسازی شما در سبز باش " + code.ToString();
            var api = new Kavenegar.KavenegarApi("71625976436F417872583968335938637870516D4F3131304B7634746D796743");
            api.Send(sender, receptor, message);
            return true;
        }

        private M_Users mapUsers(RequestModelUser data, int uId)
        {
            M_Users mUser = new M_Users();
            mUser.FirstName = data.FirstName;
            mUser.LastName = data.LastName;
            if (data.Type == "HOME")
                mUser.InstituteType = E_PublicCategory.INSTITUTE_TYPE.HOME;
            else if (data.Type == "OFFICE")
                mUser.InstituteType = E_PublicCategory.INSTITUTE_TYPE.OFFICIAL;
            else if (data.Type == "BUSSUNESS")
                mUser.InstituteType = E_PublicCategory.INSTITUTE_TYPE.COMMERCIAL;
            else if (data.Type == "FACTOR")
                mUser.InstituteType = E_PublicCategory.INSTITUTE_TYPE.FACTORY;
            mUser.MobileNumber = data.PhoneNumber;
            mUser.RegisterDate = DateTime.Now;
            mUser.LastOnline = DateTime.Now;
            mUser.IsActive = false;
            mUser.IsDeleted = false;
            mUser.UnicKey = Guid.NewGuid().ToString();
            mUser.UserType = E_PublicCategory.USER_TYPE.HOME_STORE;
            mUser.Password = data.Password;
            mUser.Id = uId;
            mUser.Email = data.Email;
            return mUser;
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
            var point = new B_ServicesRequestItems().GetSumUserPoint(userId,E_PublicCategory.SYSTEM_USER_TYPE.NORMAL_USER);
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
                Address = objAddress,
                UniqKey = user.UnicKey,
                ImageUrl = GetImageAddress(user.ImageName,new E_FTPRoutes(BaseUrl).USERS),
                UserType = type,
                ReagentCode = user.ReagentCode
            };
        }

        #endregion Private Functions
        
        #region RquestModels
        public class RequestModelDeleteUser
        {
            public int Id { get; set; }
            public string UnicCode { get; set; }
        }
        public class RequestModelGetUserByPhone
        {
            public string PhoneNumber { get; set; }
            public int Code { get; set; }
        }
        #endregion RquestModels
    }
}
