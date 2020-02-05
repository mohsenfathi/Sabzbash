using Shahrdari.BussinessLayer;
using Shahrdari.Enums;
using Shahrdari.Enums.Loging;
using Shahrdari.Factory.Log;
using Shahrdari.Logging;
using Shahrdari.Models;
using Shahrdari.Models.Loging;
using Shahrdari.Settings;
using Shahrdari.ViewModels;
using Shahrdari.WebApplication.Anotations;
using Shahrdari.WebApplication.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shahrdari.WebApplication.Areas.Admin.Controllers
{
    public class ServicesRequestsController : BaseController
    {
        [MFAutorize("لیست درخواست ها", "درخواست ها")]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.RequestType = new B_PublicCategory().GetPublicCategory(Enums.E_PublicCategory.PUBLIC_CATEGORY_PARENT.REQUEST_STATUS);
            return View();
        }

        [MFAutorize("دریافت لیست درخواست ها", "درخواست ها")]
        [HttpPost]
        public ActionResult GetRequests(int StatusId, string FirstName, string LastName, DateTime? FromCreateDate, DateTime? ToCreateDate, int PageNumber)
        {
            object result = "";
            try
            {
                result = new B_ServicesRequests().GetFullServicesRequests(StatusId, FirstName, LastName, FromCreateDate, ToCreateDate, PageNumber, CurrentUser.PersonelType == E_PublicCategory.PERSONEL_TYPE.SUM_CENER ? CurrentUser.Id : 0);
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

        [MFAutorize("درخوات های جدید", "درخواست ها")]
        [HttpGet]
        public ActionResult NewRequests()
        {
            ViewBag.Route = new List<MFRoute>
            {
                new MFRoute{ ActionName = "Index", ControllerName="ServicesRequests", Title = "لیست درخواست ها", Priority = 1 }
            };
            var requsts = new B_ServicesRequests().GetFullNewServicesRequests(CurrentUser.PersonelType == E_PublicCategory.PERSONEL_TYPE.SUM_CENER ? CurrentUser.Id : 0);
            return View(requsts.OrderBy(c => c.Id).ToList());
        }

        [MFAutorize("مشاهده جزئیات درخواست", "درخواست ها")]
        [HttpGet]
        public ActionResult ShowDetails(int Id, bool FromNew)
        {
            var route = new List<MFRoute>
            {
                new MFRoute{ ActionName = "Index", ControllerName="ServicesRequests", Title = "لیست درخواست ها", Priority = 1 }
            };
            if (FromNew)
                route.Add(new MFRoute { ActionName = "NewRequests", ControllerName = "ServicesRequests", Title = "درخوات های جدید", Priority = 2 });
            ViewBag.Route = route;
            var requst = new B_ServicesRequests().GetFullServicesRequests(Id);
            if (requst == null)
                return RedirectToAction("Index");
            if (requst.PersonelId.HasValue)
                requst.ResponsiblePersonel = new B_Personels().GetPersonelById(requst.PersonelId.Value);
            ViewBag.UserList = new B_ServicesRequestItems().GetItems(requst.Id, E_PublicCategory.SYSTEM_USER_TYPE.NORMAL_USER);
            ViewBag.PersonelList = new B_ServicesRequestItems().GetItems(requst.Id, E_PublicCategory.SYSTEM_USER_TYPE.PERSONEL);
            
            B_Personels bPersonel = new B_Personels();
            if (requst.ResponsiblePersonel != null)
                ViewBag.DriverList = bPersonel.GetPersonels().Where(c => c.PersonelType == (requst.Pouriya_Type == "BOOTH" ? E_PublicCategory.PERSONEL_TYPE.SUM_CENER : E_PublicCategory.PERSONEL_TYPE.DRIVER) && c.Id != requst.ResponsiblePersonel.Id).ToList();
            else
                ViewBag.DriverList = bPersonel.GetPersonels().Where(c => c.PersonelType == (requst.Pouriya_Type == "BOOTH" ? E_PublicCategory.PERSONEL_TYPE.SUM_CENER : E_PublicCategory.PERSONEL_TYPE.DRIVER)).ToList();

            return View(requst);
        }

        [MFAutorize("ویرایش و حذف درخواست", "درخواست ها")]
        [HttpGet]
        public ActionResult Modify(int Id)
        {
            ViewBag.Route = new List<MFRoute>
            {
                new MFRoute{ ActionName = "Index", ControllerName="ServicesRequests", Title = "لیست درخواست ها", Priority = 1 }
            };

            var request = new B_ServicesRequests().GetServicesRequests(Id);
            if (request == null)
                return RedirectToAction("Index");
            if(request.Status == E_PublicCategory.REQUEST_STATUS.CLOSED)
                return RedirectToAction("Index");

            ViewBag.ServiceCategoryList = new B_ServicesCategories().GetServicesCategories(false);
            ViewBag.ServiceRequestItems = new B_ServicesRequestItems().GetItems(Id);
            
            var date1 = DateTime.Now;
            var bDate = new B_Pouriya_Date();
            List<object> obj = new List<object>();
            for (int i = 0; i < 7; i++)
            {
                if (i != 0)
                    date1 = date1.AddDays(1);
                var date = bDate.GetDate(date1);
                if (date == null)
                    date = bDate.Add(date1);
                obj.Add(new
                {
                    Title = date1.ConvertToPesianDateName(),
                    Id = date.Id,
                    Hours = new List<object> {
                        new { Hour = "8 تا 11",  Id = (int)E_PublicCategory.SERVICE_TIME.Time_8_11, IsActive = isPast(date1.AddDays(i),E_PublicCategory.SERVICE_TIME.Time_8_11) },
                        new { Hour = "11 تا 14", Id = (int)E_PublicCategory.SERVICE_TIME.Time_11_14, IsActive = isPast(date1.AddDays(i),E_PublicCategory.SERVICE_TIME.Time_11_14) },
                        new { Hour = "14 تا 17", Id = (int)E_PublicCategory.SERVICE_TIME.Time_14_17, IsActive = isPast(date1.AddDays(i),E_PublicCategory.SERVICE_TIME.Time_14_17) },
                        new { Hour = "17 تا 20", Id = (int)E_PublicCategory.SERVICE_TIME.Time_17_20, IsActive = isPast(date1.AddDays(i),E_PublicCategory.SERVICE_TIME.Time_17_20) },
                        new { Hour = "20 تا 22", Id = (int)E_PublicCategory.SERVICE_TIME.Time_20_22, IsActive =  isPast(date1.AddDays(i),E_PublicCategory.SERVICE_TIME.Time_20_22) }
                    }
                });
            }
            ViewBag.Times = Json(obj);

            return View(request);
        }
        
        [MFAutorize("ویرایش درخواست", "درخواست ها")]
        [HttpPost]
        public ActionResult Modify(M_ServicesRequests ServicesRequests, List<M_ServicesRequestItems> RequestList)
        {
            object result = "";
            try
            {
                var request = new B_ServicesRequests().GetServicesRequests(ServicesRequests.Id);
                if (request != null)
                {
                    request.Address = ServicesRequests.Address;
                    request.Description = ServicesRequests.Description;
                    request.ImmediatelyRequest = ServicesRequests.ImmediatelyRequest;
                    request.PlaqueNumber = ServicesRequests.PlaqueNumber;
                    request.UnitNumber = ServicesRequests.UnitNumber;
                    request.UpdateDate = DateTime.Now;
                    new B_ServicesRequests().Edit(request, false);
                }
                var bItems = new B_ServicesRequestItems();
                var oldItems = bItems.GetItems(ServicesRequests.Id, E_PublicCategory.SYSTEM_USER_TYPE.NORMAL_USER);
                foreach (var li in oldItems)
                    bItems.Delete(li.Id);
                foreach (var li in RequestList)
                {
                    if(!li.CategoryId.HasValue)
                        throw F_ExeptionFactory.MakeExeption("مقادیر دسته بندی های انتخاب شده صحیح نیست لطفا مراتب را سریعتر به تیم فنی پروژه گزارش دهید", Enums.Loging.E_LogType.SYSTEM_ERROR);
                    var category = new B_ServicesCategories().GetServicesCategories(li.CategoryId.Value);
                    li.CategoryId = category.Id;
                    li.CreateDate = category.CreateDate;
                    li.ImageName = category.ImageName;
                    li.IsFailed = false;
                    li.RequestId = ServicesRequests.Id;
                    li.ScorePerUnit = category.ScorePerUnit;
                    li.ScorePerUnitDriver = category.ScorePerUnitDriver;
                    li.Title = category.Title;
                    li.Unit = category.Unit;
                    li.UserId = request.UserId;
                    li.UserType = E_PublicCategory.SYSTEM_USER_TYPE.NORMAL_USER;
                    li.Id = 0;
                    bItems.Add(li);
                }

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

        [MFAutorize("افزودن درخواست", "درخواست ها")]
        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.Route = new List<MFRoute>
            {
                new MFRoute{ ActionName = "Index", ControllerName="ServicesRequests", Title = "لیست درخواست ها", Priority = 1 }
            };
            
            var date1 = DateTime.Now;
            var bDate = new B_Pouriya_Date();
            List<object> obj = new List<object>();
            for (int i = 0; i < 7; i++)
            {
                if (i != 0)
                    date1 = date1.AddDays(1);
                var date = bDate.GetDate(date1);
                if (date == null)
                    date = bDate.Add(date1);
                obj.Add(new
                {
                    Title = date1.ConvertToPesianDateName(),
                    Id = date.Id,
                    Hours = new List<object> {
                        new { Hour = "8 تا 11",  Id = (int)E_PublicCategory.SERVICE_TIME.Time_8_11, IsActive = isPast(date1.AddDays(i),E_PublicCategory.SERVICE_TIME.Time_8_11) },
                        new { Hour = "11 تا 14", Id = (int)E_PublicCategory.SERVICE_TIME.Time_11_14, IsActive = isPast(date1.AddDays(i),E_PublicCategory.SERVICE_TIME.Time_11_14) },
                        new { Hour = "14 تا 17", Id = (int)E_PublicCategory.SERVICE_TIME.Time_14_17, IsActive = isPast(date1.AddDays(i),E_PublicCategory.SERVICE_TIME.Time_14_17) },
                        new { Hour = "17 تا 20", Id = (int)E_PublicCategory.SERVICE_TIME.Time_17_20, IsActive = isPast(date1.AddDays(i),E_PublicCategory.SERVICE_TIME.Time_17_20) },
                        new { Hour = "20 تا 22", Id = (int)E_PublicCategory.SERVICE_TIME.Time_20_22, IsActive =  isPast(date1.AddDays(i),E_PublicCategory.SERVICE_TIME.Time_20_22) }
                    }
                });
            }

            ViewBag.Times = Json(obj);

            return View();
        }

        [MFAutorize("افزودن درخواست", "درخواست ها")]
        [HttpPost]
        public ActionResult Add(M_ServicesRequests ServicesRequests)
        {
            object result = "";
            try
            {
                ServicesRequests.ReferralDate = DateTime.Now;
                ServicesRequests.Status = E_PublicCategory.REQUEST_STATUS.NEW_REQUEST;
                new B_ServicesRequests().Add(ServicesRequests);
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

        [MFAutorize("حذف درخواست", "درخواست ها")]
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            object result = "";
            try
            {
                new B_ServicesRequests().Delete(Id, Id, false);
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

        [MFAutorize("دریافت مقادیری از کاربران", "درخواست ها")]
        [HttpPost]
        public ActionResult GetUser(string Name, string Family, string Mobile)
        {
            object result = "";
            try
            {
                var users = new B_Users().GetUsers(Name, Family, Mobile);
                result = users;
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

        [MFAutorize("دریافت مقادیری از کاربران", "درخواست ها")]
        [HttpPost]
        public ActionResult ReferRequest(int Id, int DriverId)
        {
            object result = "";
            try
            {
                var bReq = new B_ServicesRequests();
                bReq.AsignUserToRequest(DriverId, Id);
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

        [MFAutorize("اتمام درخواست", "درخواست ها")]
        [HttpPost]
        public ActionResult DoneRequest(int Id)
        {
            object result = "";
            try
            {
                var bReq = new B_ServicesRequests();
                bReq.CloseRequestByPortal(Id);
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
    }
}