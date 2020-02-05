using Microsoft.AspNet.SignalR.Client;
using Shahrdari.BussinessLayer;
using Shahrdari.Enums;
using Shahrdari.Enums.Loging;
using Shahrdari.Factory.Log;
using Shahrdari.Logging;
using Shahrdari.Models;
using Shahrdari.Models.Area_User;
using Shahrdari.Models.Loging;
using Shahrdari.Settings;
using Shahrdari.WebApplication.Anotations;
using Shahrdari.WebApplication.Classes.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shahrdari.WebApplication.Areas.User.Controllers
{
    public class HomeController : BaseController
    {
        [MFUsersAutorize]
        public ActionResult Index()
        {
            var user = CurrentUser;
            ViewBag.FullName = CurrentUser.FirstName + " " + CurrentUser.LastName;
            ViewBag.Score = "امتیاز شما " + new B_ServicesRequestItems().GetSumUserPoint(CurrentUser.Id, E_PublicCategory.SYSTEM_USER_TYPE.NORMAL_USER);
            return View();
        }

        [HttpPost]
        public ActionResult GetLastRequestUser()
        {
            object result = "";
            try
            {
                var temp = new B_ServicesRequests().GetUserLastRequest(CurrentUser.Id, E_PublicCategory.REQUEST_STATUS.CLOSED);
                if (temp != null && (bool)temp.ShowRatingDialog == false && temp.PersonelId != null)
                    result = new { Request = temp, Point = new B_ServicesRequestItems().GetItemsPoint(temp.Id), Personel = new B_Personels().GetPersonelById((int)temp.PersonelId) };
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
        public ActionResult StationMode()
        {
            ViewBag.Date = getDate();
            B_ServicesCategories bCat = new B_ServicesCategories();
            var categories = bCat.GetServicesCategories(true);
            ViewBag.ElecScore = categories.Where(x => x.Id == 2).Select(x => x.ScorePerUnit).FirstOrDefault();
            ViewBag.MetalScore = categories.Where(x => x.Id == 3).Select(x => x.ScorePerUnit).FirstOrDefault();
            ViewBag.NonMetalScore = categories.Where(x => x.Id == 8).Select(x => x.ScorePerUnit).FirstOrDefault();
            ViewBag.RandomScore = categories.Where(x => x.Id == 9).Select(x => x.ScorePerUnit).FirstOrDefault();
            ViewBag.UserId = CurrentUser.Id;
            return View();
        }

        [MFUsersAutorize]
        [HttpPost]
        public ActionResult GetNearStations(double Lat, double Lng)
        {
            object result = "";
            try
            {
                var booth = new B_Booth().GetBoothes(Lat, Lng).ToList();
                var markers = new List<object>();
                var NearStations = new List<object>();
                foreach (var li in booth)
                {
                    markers.Add(new
                    {
                        StationId = li.Id,
                        Title = li.Name,
                        popupContent = getMapPupWindow(li),
                        center = new { lat = li.Lat, lng = li.Lng },
                        iconOpts = new { iconUrl = "/Areas/User/Images/CapLogo.png", iconSize = new int[] { 30, 30 } }
                    });


                }
                var res = new Helper.ApiCall(@"https://api.cedarmaps.com/v1/geocode/cedarmaps.streets/" + Lat + "," + Lng + ".json?access_token=7775fb1e47034c4ae978aae1b3ac3f6f0b4ec6b3", "").CallApiGet();
                result = new { markers = markers, Address = res };
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
        public ActionResult SearchOnMap(double Lat, double Lng, string SearchText)
        {
            object result = "";
            try
            {
                if (string.IsNullOrEmpty(SearchText))
                    throw F_ExeptionFactory.MakeExeption("مقدار فیلد جستجو خالی است",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PhoneNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                var res = new Helper.ApiCall(@"https://api.cedarmaps.com/v1/geocode/cedarmaps.streets/" + SearchText + "?location=" + Lat + "," + Lng + "&distance=50&access_token=7775fb1e47034c4ae978aae1b3ac3f6f0b4ec6b3", "").CallApiGet();
                result = res;
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
        public ActionResult GetNearCars(double Lat, double Lng)
        {
            object result = "";
            try
            {
                var cars = new B_Location().GetNearDrivers(Lat, Lng).ToList();
                var markers = new List<object>();
                foreach (var li in cars)
                {
                    markers.Add(new
                    {
                        center = new { lat = li.Lat, lng = li.Lng },
                        iconOpts = new { iconUrl = "/Areas/User/Images/CarIcon.png", iconSize = new int[] { 30, 30 } }
                    });
                }
                var res = new Helper.ApiCall(@"https://api.cedarmaps.com/v1/geocode/cedarmaps.streets/" + Lat + "," + Lng + ".json?access_token=7775fb1e47034c4ae978aae1b3ac3f6f0b4ec6b3", "").CallApiGet();
                result = new { markers = markers, Address = res };
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
        public ActionResult GetFavoriteAddress(int Id)
        {
            object result = "";
            try
            {
                result = new B_Addresses().GetAddress(Id);
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
        public ActionResult EditFavoriteAddress(M_Addresses Address)
        {
            object result = "";
            try
            {
                B_Addresses bAddress = new B_Addresses();
                bAddress.Edit(Address);
                result = bAddress.GetAddresses(CurrentUser.Id, true);
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
        public ActionResult RideMode()
        {
            //var Cars = new B_Location().GetDriversLocation();
            var markers = "";
            //foreach (var li in booth)
            //{
            //    markers += (markers == "" ? "" : ",") + "{ 'StationId' : '" + li.Id + "' , 'popupContent': '" + getMapPupWindow(li) + "', 'center': { 'lat': " + li.Lat + " ,'lng': " + li.Lng + " }, 'iconOpts': { 'iconUrl': '/Areas/User/Images/CapLogo.png', 'iconSize': [30, 30] } }";
            //}
            //ViewBag.Stations = markers;
            ViewBag.Date = getDate();
            B_ServicesCategories bCat = new B_ServicesCategories();
            var categories = bCat.GetServicesCategories(true);
            ViewBag.ElecScore = categories.Where(x => x.Id == 2).Select(x => x.ScorePerUnit).FirstOrDefault();
            ViewBag.MetalScore = categories.Where(x => x.Id == 3).Select(x => x.ScorePerUnit).FirstOrDefault();
            ViewBag.NonMetalScore = categories.Where(x => x.Id == 8).Select(x => x.ScorePerUnit).FirstOrDefault();
            ViewBag.RandomScore = categories.Where(x => x.Id == 9).Select(x => x.ScorePerUnit).FirstOrDefault();
            ViewBag.UserId = CurrentUser.Id;
            ViewBag.Addresses = new B_Addresses().GetAddresses(CurrentUser.Id, true);
            return View();
        }

        [MFUsersAutorize]
        public ActionResult RideModeProceed()
        {
            var request = new B_ServicesRequests().GetUserLastRequest(CurrentUser.Id, "CAR");
            if (request != null)
            {
                if (request.Status == E_PublicCategory.REQUEST_STATUS.CLOSED)
                    return RedirectToAction("RideMode");
                var driver = new B_Personels().GetPersonelById(request.PersonelId.HasValue ? (int)request.PersonelId : 0);
                if (driver != null)
                {
                    var car = new B_CarInfo().GetCarInfo(driver.Id);
                    ViewBag.DriverName = driver.FirstName + " " + driver.LastName;
                    ViewBag.DriverCode = "کد راننده : " + driver.Id;
                    ViewBag.DriverImage = new E_FTPRoutes(BaseUrl).PERSONELS + driver.ImageName;
                    ViewBag.Car = car.Name + " - " + car.Color;
                    ViewBag.TagFirst = car.TagFirst;
                    ViewBag.TagMiddle = car.TagMiddle;
                    ViewBag.TagLast = car.TagLast;
                    ViewBag.NationalTag = car.TagNational;
                    ViewBag.PhoneNumber = driver.MobileNumber;
                }
                else
                {
                    ViewBag.DriverName = " --- ";
                    ViewBag.DriverImage = new E_FTPRoutes(BaseUrl).PERSONELS + "Default.jpg";
                    ViewBag.DriverCode = "کد راننده : ---";
                    ViewBag.Car = " --- ";
                    ViewBag.LicensePlaceNumber = "------";
                    ViewBag.NationalTag = "--";
                    ViewBag.PhoneNumber = "";
                }
                ViewBag.Status = getRequestStatus(request.Status);
                ViewBag.LatUser = request.GeographicalCoordinates.Split(',')[0];
                ViewBag.LngUser = request.GeographicalCoordinates.Split(',')[1];
                ViewBag.RequestId = request.Id;
                ViewBag.Disrespectful = (int)E_PublicCategory.FEEDBACK.DISRESPECTFUL_BEHAVIOR;
                ViewBag.PersonalReasons = (int)E_PublicCategory.FEEDBACK.PERSONAL_REASONS;
                ViewBag.GetToYourPlaceLate = (int)E_PublicCategory.FEEDBACK.GET_TO_YOUR_PLACE_LATE;
                ViewBag.Request = request;
                ViewBag.RequestItems = new B_ServicesRequestItems().GetItems(request.Id, E_PublicCategory.SYSTEM_USER_TYPE.NORMAL_USER);
            }
            else
                return RedirectToAction("RideMode");
            return View();
        }

        [HttpPost]
        public ActionResult CancelRequest(int RequestId, int DeleteMessageId, string MessageText)
        {
            object result = "";
            try
            {
                new B_ServicesRequests().CancelRequestUser(MessageText, RequestId, CurrentUser.Id, DeleteMessageId);
                var request = new B_ServicesRequests().GetServicesRequests(RequestId);
                if (request.PersonelId.HasValue)
                    try
                    {
                        var hubConnection = new HubConnection(url: SignalRUrl);
                        var chat = hubConnection.CreateHubProxy(hubName: "Reqeust");

                        hubConnection.Start().Wait();
                        chat.Invoke<bool>("RequestChangedStatusPersonel", request, request.PersonelId);
                    }
                    catch { }
                else
                {
                    try
                    {
                        var hubConnection = new HubConnection(url: SignalRUrl);
                        var chat = hubConnection.CreateHubProxy(hubName: "Reqeust");

                        hubConnection.Start().Wait();
                        chat.Invoke<bool>("NewRequestDriver", request);
                    }
                    catch { }
                }
                
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

        private string getRequestStatus(E_PublicCategory.REQUEST_STATUS status)
        {
            switch (status)
            {
                case E_PublicCategory.REQUEST_STATUS.APPROVED_AND_REFERENCED:
                    return "در حال یافتن نزدیکترین خودرو";
                case E_PublicCategory.REQUEST_STATUS.EXPERT_ON_PLACE:
                    return "ماشین سبز در محل میباشد";
                case E_PublicCategory.REQUEST_STATUS.EXPERT_READY_TO_GO:
                    return "خودرو سبز در حال آمدن به سمت شماست";
                case E_PublicCategory.REQUEST_STATUS.NEW_REQUEST:
                    return "در حال بررسی";
                case E_PublicCategory.REQUEST_STATUS.WAIT_FOR_END_REQUEST_ACCEPT:
                    return "اتمام درخواست";
                case E_PublicCategory.REQUEST_STATUS.WAIT_FOR_GET:
                    return "مرکز # منتظر دریافت پسماند شماست!";
                default:
                    return "";
            }
        }

        [MFUsersAutorize]
        public ActionResult StationModeProceed()
        {
            var request = new B_ServicesRequests().GetUserLastRequest(CurrentUser.Id, "BOOTH");
            if (request != null)
            {
                if (request.Status == E_PublicCategory.REQUEST_STATUS.CLOSED)
                    return RedirectToAction("StationMode");
                var personel = new B_Personels().GetPersonelById(request.PersonelId.HasValue ? (int)request.PersonelId : 0);
                var booth = new B_Booth().GetBoothByPersonelId(request.PersonelId.HasValue ? (int)request.PersonelId : 0);
                if (booth != null)
                {
                    ViewBag.PersonelName = personel.FirstName + " " + personel.LastName;
                    ViewBag.PersonelImage = new E_FTPRoutes(BaseUrl).PERSONELS + personel.ImageName;
                    ViewBag.StationName = booth.Name;
                    ViewBag.PhoneNumber = personel.MobileNumber;
                    ViewBag.BoothAddress = booth.Address;
                    ViewBag.StationCode = "کد مرکز : " + booth.Id;
                }
                else
                {
                    ViewBag.PersonelName = "---";
                    ViewBag.PersonelImage = new E_FTPRoutes(BaseUrl).PERSONELS + personel.ImageName;
                    ViewBag.StationName = "---";
                    ViewBag.PhoneNumber = "";
                    ViewBag.StationCode = "کد مرکز : ---";
                }
                ViewBag.Status = getRequestStatus(request.Status).Replace("#", booth.Id.ToString());
                ViewBag.Lat = request.GeographicalCoordinates.Split(',')[0];
                ViewBag.Lng = request.GeographicalCoordinates.Split(',')[1];
                ViewBag.RequestId = request.Id;
                ViewBag.Disrespectful = (int)E_PublicCategory.FEEDBACK.DISRESPECTFUL_BEHAVIOR;
                ViewBag.PersonalReasons = (int)E_PublicCategory.FEEDBACK.PERSONAL_REASONS;
                ViewBag.Request = request;
                ViewBag.RequestItems = new B_ServicesRequestItems().GetItems(request.Id, E_PublicCategory.SYSTEM_USER_TYPE.NORMAL_USER);
            }
            else
                return RedirectToAction("StationMode");
            return View();
        }

        [HttpPost]
        public ActionResult AddFavoriteAddress(M_Addresses Address)
        {
            object result = "";
            try
            {
                if (string.IsNullOrEmpty(Address.Title))
                    throw F_ExeptionFactory.MakeExeption("عنوان وارد شده صحیح نیست",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PhoneNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (string.IsNullOrEmpty(Address.Address))
                    throw F_ExeptionFactory.MakeExeption("آدرس وارد شده صحیح نیست",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PhoneNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (Address.Unit == 0 || Address.Unit == null)
                    throw F_ExeptionFactory.MakeExeption("شماره واحد وارد شده صحیح نیست",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PhoneNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (Address.Plaque == "0" || string.IsNullOrEmpty(Address.Plaque))
                    throw F_ExeptionFactory.MakeExeption("شماره پلاک وارد شده صحیح نیست",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PhoneNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                B_Addresses bAddress = new B_Addresses();
                Address.UserId = CurrentUser.Id;
                Address.IsFavorite = true;
                bAddress.Add(Address);
                result = bAddress.GetAddresses(CurrentUser.Id, true);
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
        public ActionResult AddRequestStation(RequestModelAddRequest Data)
        {
            object result = "";
            try
            {
                if (!isValidUserForInsertRequest("BOOTH"))
                    throw F_ExeptionFactory.MakeExeption("کاربر گرامی تا اتمام درخواست قبلی،امکان ثبت درخواست جدید میسر نمیباشد",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PhoneNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                //if(string.IsNullOrEmpty(Data.Address))
                //    throw F_ExeptionFactory.MakeExeption("آدرس وارد شده صحیح نیست",
                //        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PhoneNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (Data.BoothId == 0)
                    throw F_ExeptionFactory.MakeExeption("برای ثبت درخواست ابتدا غرفه را انتخاب کنید",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PhoneNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (Data.DayId == 0)
                    throw F_ExeptionFactory.MakeExeption("روز انتخاب شده صحیح نیست",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PhoneNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (Data.TimeId == 0)
                    throw F_ExeptionFactory.MakeExeption("ساعت انتخاب شده صحیح نیست",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PhoneNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                //if (string.IsNullOrEmpty(Data.Description))
                //    throw F_ExeptionFactory.MakeExeption("توضیحات وارد شده صحیح نیست",
                //        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PhoneNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (Data.RequestItems.Count == 0)
                    throw F_ExeptionFactory.MakeExeption("پسماندهای خود را به دسته بندی اضافه کنید",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PhoneNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                var bServicesRequests = new B_ServicesRequests();
                var request = new M_ServicesRequests
                {
                    Address = "",
                    Description = Data.Description,
                    GeographicalCoordinates = Data.Lat + "," + Data.Lng,
                    PlaqueNumber = "",
                    Pouriya_DateId = Data.DayId,
                    Pouriya_TimeId = Data.TimeId,
                    Pouriya_Type = "BOOTH",
                    Status = E_PublicCategory.REQUEST_STATUS.WAIT_FOR_GET,
                    UnitNumber = "",
                    UserId = CurrentUser.Id,
                    PersonelId = new B_Booth().GetBoothes(Data.BoothId).PersonelId,
                    CreateDate = DateTime.Now,
                    ToDoDate = DateTime.Now
                };
                request = bServicesRequests.Add(request);
                var bItem = new B_ServicesRequestItems();
                if (Data.RequestItems != null && Data.RequestItems.Count > 0)
                    foreach (var li in Data.RequestItems)
                    {
                        if (li.Value != 0)
                        {
                            var category = new B_ServicesCategories().GetServicesCategories(li.Id);
                            if (category != null)
                                bItem.Add(new M_ServicesRequestItems
                                {
                                    CreateDate = DateTime.Now,
                                    RequestId = request.Id,
                                    UserType = E_PublicCategory.SYSTEM_USER_TYPE.NORMAL_USER,
                                    Value = li.Value,
                                    ImageName = category.ImageName,
                                    ScorePerUnit = category.ScorePerUnit,
                                    Title = category.Title,
                                    Unit = category.Unit,
                                    IsFailed = false,
                                    UserId = CurrentUser.Id,
                                    CategoryId = category.Id
                                });
                        }
                    }
                try
                {
                    var hubConnection = new HubConnection(url: SignalRUrl);
                    var chat = hubConnection.CreateHubProxy(hubName: "Reqeust");

                    hubConnection.Start().Wait();

                    chat.Invoke<bool>("NewRequestStation", request, request.PersonelId);
                }
                catch { }
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
        public ActionResult AddRequestCar(RequestModelAddRequest Data)
        {
            object result = "";
            try
            {
                if (!isValidUserForInsertRequest("CAR"))
                    throw F_ExeptionFactory.MakeExeption("کاربر گرامی تا اتمام درخواست قبلی،امکان ثبت درخواست جدید میسر نمیباشد",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PhoneNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (string.IsNullOrEmpty(Data.Address))
                    throw F_ExeptionFactory.MakeExeption("آدرس وارد شده صحیح نیست",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PhoneNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (Data.DayId == 0)
                    throw F_ExeptionFactory.MakeExeption("روز انتخاب شده صحیح نیست",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PhoneNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (Data.TimeId == 0)
                    throw F_ExeptionFactory.MakeExeption("ساعت انتخاب شده صحیح نیست",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PhoneNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                //if (string.IsNullOrEmpty(Data.Description))
                //    throw F_ExeptionFactory.MakeExeption("توضیحات وارد شده صحیح نیست",
                //        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PhoneNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (Data.UnitNumber == 0)
                    throw F_ExeptionFactory.MakeExeption("شماره واحد وارد شده صحیح نیست",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PhoneNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                if (Data.PlaqueNumber == 0)
                    throw F_ExeptionFactory.MakeExeption("شماره پلاک وارد شده صحیح نیست",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PhoneNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                bool rqItems = true;
                if (Data.RequestItems.Count > 0)
                    foreach (var li in Data.RequestItems)
                        if (li.Value > 0)
                            rqItems = false;
                if (rqItems)
                    throw F_ExeptionFactory.MakeExeption("پسماندهای خود را به دسته بندی اضافه کنید",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PhoneNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
                var bServicesRequests = new B_ServicesRequests();
                var request = new M_ServicesRequests
                {
                    Address = Data.Address,
                    Description = Data.Description,
                    GeographicalCoordinates = Data.Lat + "," + Data.Lng,
                    PlaqueNumber = Data.PlaqueNumber.ToString(),
                    Pouriya_DateId = 0,
                    Pouriya_TimeId = 0,
                    Pouriya_Type = "CAR",
                    Status = E_PublicCategory.REQUEST_STATUS.NEW_REQUEST,
                    UnitNumber = Data.UnitNumber.ToString(),
                    UserId = CurrentUser.Id,
                    PersonelId = null,
                    CreateDate = DateTime.Now,
                };
                request = bServicesRequests.Add(request);
                var bItem = new B_ServicesRequestItems();
                if (Data.RequestItems != null && Data.RequestItems.Count > 0)
                    foreach (var li in Data.RequestItems)
                    {
                        if (li.Value != 0)
                        {
                            var category = new B_ServicesCategories().GetServicesCategories(li.Id);
                            if (category != null)
                                bItem.Add(new M_ServicesRequestItems
                                {
                                    CreateDate = DateTime.Now,
                                    RequestId = request.Id,
                                    UserType = E_PublicCategory.SYSTEM_USER_TYPE.NORMAL_USER,
                                    Value = li.Value,
                                    ImageName = category.ImageName,
                                    ScorePerUnit = category.ScorePerUnit,
                                    Title = category.Title,
                                    Unit = category.Unit,
                                    IsFailed = false,
                                    UserId = CurrentUser.Id,
                                    CategoryId = category.Id
                                });
                        }
                    }

                try
                {
                    var hubConnection = new HubConnection(url: SignalRUrl);
                    var chat = hubConnection.CreateHubProxy(hubName: "Reqeust");

                    hubConnection.Start().Wait();

                    chat.Invoke<bool>("NewRequestDriver", request);
                }
                catch { }
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
        public ActionResult DeleteFavoriteAddress(int Id)
        {
            object result = "";
            try
            {
                B_Addresses bAddress = new B_Addresses();
                if (Id == 0)
                    throw F_ExeptionFactory.MakeExeption("شما مجاز به حذف آدرس منتخب نیستید",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "FavoriteAddress", Enums.Loging.E_LogType.SYSTEM_ERROR);
                bAddress.Delete(Id);
                result = bAddress.GetAddresses(CurrentUser.Id, true);
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
        public ActionResult GetRequestHistory()
        {
            object result = "";
            try
            {
                var obj = new List<object>();
                var requests = new B_ServicesRequests().GetServicesRequests(CurrentUser.Id, null).Where(x => x.Status == E_PublicCategory.REQUEST_STATUS.CLOSED).ToList().OrderByDescending(x => x.CreateDate);
                if (requests.Count() > 0 && requests != null)
                    foreach (var li in requests)
                        obj.Add(new
                        {
                            Date = li.CreateDate.ConvertMiladiToShamsi(),
                            Number = li.Id,
                            Point = new B_ServicesRequestItems().GetItemsPoint(li.Id)
                        });
                result = obj;
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
        public ActionResult GetLastRequest(string Type)
        {
            object result = "";
            try
            {
                var temp = new B_ServicesRequests().GetUserLastRequest(CurrentUser.Id, Type);
                if (temp != null)
                    result = temp;
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
        public ActionResult FinishRequestFromUser(int RequestId, int Rate, List<int> MessageId, string Comment)
        {
            object result = "";
            try
            {
                result = new B_ServicesRequests().FinishRequestUser(RequestId, Comment, Rate, MessageId);
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

        private bool isValidUserForInsertRequest(string Type)
        {
            B_ServicesRequests bRequest = new B_ServicesRequests();
            var requests = bRequest.GetServicesRequests(CurrentUser.Id, null);
            var temp = requests.Where(x => x.Status != E_PublicCategory.REQUEST_STATUS.CLOSED && x.Status != E_PublicCategory.REQUEST_STATUS.CANCEL && x.Pouriya_Type == Type).ToList();
            if (temp.Count != 0)
                return false;
            else
                return true;
        }

        [HttpPost]
        public ActionResult GetTime(int DateId)
        {
            object result = "";
            try
            {
                var date = new B_Pouriya_Date().GetDate(DateId);
                result = new List<object> {
                         new { Hour = "8 تا 11",  Id = (int)E_PublicCategory.SERVICE_TIME.Time_8_11, IsActive = isPast(date.Date,E_PublicCategory.SERVICE_TIME.Time_8_11) },
                         new { Hour = "11 تا 14", Id = (int)E_PublicCategory.SERVICE_TIME.Time_11_14, IsActive = isPast(date.Date,E_PublicCategory.SERVICE_TIME.Time_11_14) },
                         new { Hour = "14 تا 17", Id = (int)E_PublicCategory.SERVICE_TIME.Time_14_17, IsActive = isPast(date.Date,E_PublicCategory.SERVICE_TIME.Time_14_17) },
                         new { Hour = "17 تا 20", Id = (int)E_PublicCategory.SERVICE_TIME.Time_17_20, IsActive = isPast(date.Date,E_PublicCategory.SERVICE_TIME.Time_17_20) },
                         new { Hour = "20 تا 22", Id = (int)E_PublicCategory.SERVICE_TIME.Time_20_22, IsActive =  isPast(date.Date,E_PublicCategory.SERVICE_TIME.Time_20_22) }
                    };
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

        private string getMapPupWindow(M_BoothInfo Stations)
        {
            var personel = new B_Personels().GetPersonelById(Stations.PersonelId);
            return $"<table class=\"tbList\" StationId=\"{Stations.Id}\">" +
                        $"<tr style=\"background:#FFF;text-align: center;\">" +
                            $"<td>{Stations.Name}</td>" +
                        $"</tr>" +
                        $"<tr style=\"background:#FFF;text-align: center;\">" +
                            $"<td>{personel.FirstName + " " + personel.LastName}</td>" +
                        $"</tr>" +
                        $"<tr style=\"background:#FFF;text-align: center;\">" +
                            $"<td>ظرفیت : {Stations.Capacity}</td>" +
                        $"</tr>" +
                        $"<tr style=\"background:#FFF;text-align: center;\">" +
                            $"<td><input type=\"button\" class=\"btn btnGreen\" onclick=\"SelectStation({Stations.Id});\" value=\"انتخاب\"/></td>" +
                        $"</tr>" +
                   $"</table>";
        }

        private List<M_ComboBox> getDate()
        {
            var date1 = DateTime.Now;
            var bDate = new B_Pouriya_Date();
            List<M_ComboBox> obj = new List<M_ComboBox>();
            for (int i = 0; i < 7; i++)
            {
                if (i != 0)
                    date1 = date1.AddDays(1);
                var date = bDate.GetDate(date1);
                if (date == null)
                    date = bDate.Add(date1);
                obj.Add(new M_ComboBox
                {
                    Title = date1.ConvertToPesianDateName(),
                    Value = date.Id,
                });
            }
            return obj;
        }

        private bool isPast(DateTime date, E_PublicCategory.SERVICE_TIME time)
        {
            DateTime dt = new DateTime();
            if (time == E_PublicCategory.SERVICE_TIME.Time_8_11)
                dt = date.AddHours(11);
            if (time == E_PublicCategory.SERVICE_TIME.Time_11_14)
                dt = date.AddHours(14);
            if (time == E_PublicCategory.SERVICE_TIME.Time_14_17)
                dt = date.AddHours(17);
            if (time == E_PublicCategory.SERVICE_TIME.Time_17_20)
                dt = date.AddHours(20);
            if (time == E_PublicCategory.SERVICE_TIME.Time_20_22)
                dt = date.AddHours(22);

            int result = DateTime.Compare(dt, DateTime.Now);

            if (result < 0)
                return false;
            else if (result == 0)
                return false;
            else
                return true;
        }

        [HttpPost]
        public ActionResult Exit()
        {
            object result = "";
            try
            {
                HttpCookie Coki = new HttpCookie(MFCookies.END_USER_KEY);
                Coki.Expires = DateTime.Now.AddMilliseconds(-1);
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

        public class RequestModelAddRequest
        {
            public string Address { get; set; }
            public string Description { get; set; }
            public int PlaqueNumber { get; set; }
            public int UnitNumber { get; set; }
            public int DayId { get; set; }
            public int TimeId { get; set; }
            public int UserId { get; set; }
            public int BoothId { get; set; }
            public double Lat { get; set; }
            public double Lng { get; set; }
            public List<RequestItems> RequestItems { get; set; }
        }

        public class RequestItems
        {
            public int Id { get; set; }
            public int Value { get; set; }
        }
    }
}