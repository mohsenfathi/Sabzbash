using Shahrdari.DataAccessLayer;
using Shahrdari.Enums;
using Shahrdari.Enums.Loging;
using Shahrdari.Factory.Log;
using Shahrdari.Models;
using Shahrdari.Settings;
using Shahrdari.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.BussinessLayer
{
    /// <summary>
    /// بیزینس مربوط به درخواست ها
    /// </summary>
    public class B_ServicesRequests : B_Base
    {
        /// <summary>
        /// اعتبار سنجی مدل
        /// </summary>
        /// <typeparam name="T">نوع مدل</typeparam>
        /// <param name="Data">داده</param>
        public override void Validate<T>(T Data)
        {
            base.Validate(Data);

            if (new B_Users().GetUsers(int.Parse(Data.GetType().GetProperty("UserId").GetValue(Data).ToString())) == null)
                throw F_ExeptionFactory.MakeExeption("کاربر مورد نظر در سیستم وجود ندارد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "UserId", Enums.Loging.E_LogType.SYSTEM_ERROR);

            //if (((int?)Data.GetType().GetProperty("PersonelId").GetValue(Data)).HasValue)
            //    if (new B_Users().GetUsers(int.Parse(Data.GetType().GetProperty("PersonelId").GetValue(Data).ToString())) == null)
            //        throw F_ExeptionFactory.MakeExeption("پرسنل مورد نظر در سیستم وجود ندارد",
            //            ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PersonelId", Enums.Loging.E_LogType.SYSTEM_ERROR);
        }

        /// <summary>
        /// افزودن مقدار جدید
        /// </summary>
        /// <param name="ServicesRequest">داده مورد نظر</param>
        /// <returns>نتیجه عملیات</returns>
        public M_ServicesRequests Add(M_ServicesRequests ServicesRequest)
        {
            Validate(ServicesRequest);
            DatabaseContext db = new DatabaseContext();
            ServicesRequest.CreateDate = DateTime.Now;
            if (ServicesRequest.ImmediatelyRequest)
                ServicesRequest.ReferralDate = DateTime.Now;
            db.ServicesRequests.Add(ServicesRequest);
            db.SaveChanges();
            return ServicesRequest;
        }

        /// <summary>
        /// دریافت لیستی از درخواست ها برای کاربر خواص
        /// </summary>
        /// <param name="UserId">شناسه کاربر</param>
        /// <param name="NotId">شناسه ها برای چشم پوشی</param>
        /// <returns>درخواست مورد نظر</returns>
        public List<M_ServicesRequests> GetServicesRequests(int UserId, List<int> NotId)
        {
            if (NotId == null)
                NotId = new List<int>();
            return new DatabaseContext().ServicesRequests.Where(c => c.UserId == UserId && !NotId.Contains(c.Id) && c.IsDeleted == false).ToList();
        }

        /// <summary>
        /// دریافت لیستی از درخواست ها با توجه به وضعیت آن و و صفحه بندی برای کاربر خاص
        /// </summary>
        /// <param name="UserId">شناسه کاربر</param>
        /// <param name="Status">وضعیت درخواست</param>
        /// <param name="PageNumber">شماره صفحه</param>
        /// <param name="PageCapasity">وضعیت صفحه</param>
        /// <returns>لیستی از کاربران واکشی شده</returns>
        public Tuple<int, List<M_ServicesRequests>> GetServicesRequests(int UserId, E_PublicCategory.REQUEST_STATUS? Status, int PageNumber, int PageCapasity)
        {
            int count = new DatabaseContext().ServicesRequests.Where(c => c.UserId == UserId && c.IsDeleted == false && (!Status.HasValue || c.Status == Status)).Count();
            return new Tuple<int, List<M_ServicesRequests>>(count, new DatabaseContext().ServicesRequests.Where(c => c.UserId == UserId && c.IsDeleted == false
              && (!Status.HasValue || c.Status == Status)).OrderBy(c => c.Id).Skip((PageNumber - 1) * PageCapasity).Take(PageCapasity).ToList());
        }

        /// <summary>
        /// دریافت لیستی از درخواست ها با توجه به وضعیت آن و و صفحه بندی برای پرسنل خاص
        /// </summary>
        /// <param name="PersonelId">شناسه پرسنل</param>
        /// <param name="Status">وضعیت درخواست</param>
        /// <param name="PageNumber">شماره صفحه</param>
        /// <param name="PageCapasity">وضعیت صفحه</param>
        /// <returns>لیستی از کاربران واکشی شده</returns>
        public Tuple<int, List<M_ServicesRequests>> GetServicesRequestsForPersonel(int PersonelId, E_PublicCategory.REQUEST_STATUS? Status, int PageNumber, int PageCapasity)
        {
            int count = new DatabaseContext().ServicesRequests.Where(c => c.PersonelId == PersonelId && c.IsDeleted == false && (!Status.HasValue || c.Status == Status)).Count();
            return new Tuple<int, List<M_ServicesRequests>>(count, new DatabaseContext().ServicesRequests.Where(c => c.PersonelId == PersonelId && c.IsDeleted == false
              && (!Status.HasValue || c.Status == Status)).OrderBy(c => c.Id).Skip((PageNumber - 1) * PageCapasity).Take(PageCapasity).ToList());
        }

        /// <summary>
        /// ویرایش یک درخواست
        /// </summary>
        /// <param name="ServiceRequest">درخواست مورد نظر</param>
        /// <param name="IsService">درخواست ای.پی.آی هستش ؟</param>
        public void Edit(M_ServicesRequests ServiceRequest, bool IsService)
        {
            Validate(ServiceRequest);
            DatabaseContext db = new DatabaseContext();
            db.ServicesRequests.Where(c => c.Id == ServiceRequest.Id && c.IsDeleted == false).Load();
            if (db.ServicesRequests.Local.Count == 0)
                throw F_ExeptionFactory.MakeExeption("درخواستی برای ویرایش پیدا نشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Id", Enums.Loging.E_LogType.SYSTEM_ERROR);
            if (IsService && db.ServicesRequests.Local.Where(c => c.UserId == ServiceRequest.UserId).Count() == 0)
                throw F_ExeptionFactory.MakeExeption("درخواست مورد نظر برای کاربر جاری نمیباشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Id", Enums.Loging.E_LogType.SYSTEM_ERROR);
            db.ServicesRequests.Local[0].Address = ServiceRequest.Address;
            db.ServicesRequests.Local[0].Description = ServiceRequest.Description;
            db.ServicesRequests.Local[0].GeographicalCoordinates = ServiceRequest.GeographicalCoordinates;
            db.ServicesRequests.Local[0].ImmediatelyRequest = ServiceRequest.ImmediatelyRequest;
            db.ServicesRequests.Local[0].PlaqueNumber = ServiceRequest.PlaqueNumber;
            db.ServicesRequests.Local[0].ReferralDate = ServiceRequest.ReferralDate;
            db.ServicesRequests.Local[0].UnitNumber = ServiceRequest.UnitNumber;
            db.ServicesRequests.Local[0].UpdateDate = DateTime.Now;
            db.ServicesRequests.Local[0].Pouriya_DateId = ServiceRequest.Pouriya_DateId;
            db.ServicesRequests.Local[0].Pouriya_TimeId = ServiceRequest.Pouriya_TimeId;
            db.ServicesRequests.Local[0].Pouriya_Type = ServiceRequest.Pouriya_Type;
            if (!IsService)
            {
                db.ServicesRequests.Local[0].Status = ServiceRequest.Status;
                db.ServicesRequests.Local[0].PersonelId = ServiceRequest.PersonelId;
                if (ServiceRequest.Status == E_PublicCategory.REQUEST_STATUS.APPROVED_AND_REFERENCED)
                    db.ServicesRequests.Local[0].ToDoDate = DateTime.Now;
            }
            db.SaveChanges();
        }

        /// <summary>
        /// حذف بک درخواست
        /// </summary>
        /// <param name="ServiceRequestId">شناسه درخوسات</param>
        /// <param name="UserId">شناسه کاربر</param>
        /// <param name="IsService">درخواست ای.پی.آی هستش ؟</param>
        public void Delete(int ServiceRequestId, int UserId, bool IsService, string Desc = null)
        {
            DatabaseContext db = new DatabaseContext();
            db.ServicesRequests.Where(c => c.Id == ServiceRequestId && c.IsDeleted == false).Load();
            if (db.ServicesRequests.Local.Count == 0)
                throw F_ExeptionFactory.MakeExeption("درخواستی برای حذف پیدا نشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Id", Enums.Loging.E_LogType.SYSTEM_ERROR);
            if (IsService && db.ServicesRequests.Local.Where(c => c.UserId == UserId).Count() == 0)
                throw F_ExeptionFactory.MakeExeption("درخواست مورد نظر برای کاربر جاری نمیباشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Id", Enums.Loging.E_LogType.SYSTEM_ERROR);
            db.ServicesRequests.Local[0].IsDeleted = true;
            db.ServicesRequests.Local[0].DeletedDate = DateTime.Now;
            db.ServicesRequests.Local[0].DeleteDescription = Desc;
            db.SaveChanges();
        }

        /// <summary>
        /// بستن یک درخواست
        /// </summary>
        /// <param name="Comment">کامنت پایان کار</param>
        /// <param name="Rate">رتبه</param>
        /// <param name="RequestId">شناسه درخواست</param>
        /// <param name="UserId">شناسه کاربر</param>
        public void CloseRequest(string Comment, int Rate, int RequestId, int UserId)
        {
            DatabaseContext db = new DatabaseContext();
            db.ServicesRequests.Where(c => c.Id == RequestId && c.IsDeleted == false).Load();
            if (db.ServicesRequests.Local.Count == 0)
                throw F_ExeptionFactory.MakeExeption("درخواستی برای ویرایش پیدا نشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Id", Enums.Loging.E_LogType.SYSTEM_ERROR);
            if (db.ServicesRequests.Local.Where(c => c.UserId == UserId).Count() == 0)
                throw F_ExeptionFactory.MakeExeption("درخواست مورد نظر برای کاربر جاری نمیباشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Id", Enums.Loging.E_LogType.SYSTEM_ERROR);
            db.ServicesRequests.Local[0].Rate = Rate;
            db.ServicesRequests.Local[0].FinishComment = Comment;
            db.ServicesRequests.Local[0].FinishDate = DateTime.Now;
            db.ServicesRequests.Local[0].Status = E_PublicCategory.REQUEST_STATUS.CLOSED;
            db.SaveChanges();
            new B_ServicesRequestItems().CommitScoreItems(RequestId, E_PublicCategory.SYSTEM_USER_TYPE.NORMAL_USER);
        }

        public void CancelRequestUser(string Comment, int RequestId, int UserId,int MessageDeleteId)
        {
            DatabaseContext db = new DatabaseContext();
            db.ServicesRequests.Where(c => c.Id == RequestId && c.IsDeleted == false).Load();
            if (db.ServicesRequests.Local.Count == 0)
                throw F_ExeptionFactory.MakeExeption("درخواستی برای ویرایش پیدا نشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Id", Enums.Loging.E_LogType.SYSTEM_ERROR);
            if (db.ServicesRequests.Local.Where(c => c.UserId == UserId).Count() == 0)
                throw F_ExeptionFactory.MakeExeption("درخواست مورد نظر برای کاربر جاری نمیباشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Id", Enums.Loging.E_LogType.SYSTEM_ERROR);
            db.ServicesRequests.Local[0].FinishComment = Comment;
            db.ServicesRequests.Local[0].FinishDate = DateTime.Now;
            db.ServicesRequests.Local[0].Status = E_PublicCategory.REQUEST_STATUS.CANCEL;
            db.SaveChanges();
            new B_ServicesRequestItems().DennyScoreItems(RequestId, E_PublicCategory.SYSTEM_USER_TYPE.NORMAL_USER);
        }

        /// <summary>
        /// دریافت تعداد درخواست با توجه به وضعیت
        /// </summary>
        /// <param name="Status">وضعیت درخواست</param>
        /// <returns>تعداد درخواست ها</returns>
        public int GetServicesRequestsCount(E_PublicCategory.REQUEST_STATUS Status)
        {
            return new DatabaseContext().ServicesRequests.Where(c => c.Status == Status && c.IsDeleted == false).Count();
        }

        /// <summary>
        /// دریافت درخواست های جدید با کاربر و دسته بندی به صورت کامل
        /// </summary>
        /// <returns>لیستی از درخواست های جدید</returns>
        public List<V_ServicesRequests> GetFullNewServicesRequests(int PersonelId = 0)
        {
            var db = new DatabaseContext();
            return db.ServicesRequests.Where(c => c.Status == E_PublicCategory.REQUEST_STATUS.NEW_REQUEST && c.IsDeleted == false
                    && (PersonelId == 0 || c.PersonelId == PersonelId))
                .Join(db.Users, c => c.UserId, c => c.Id, (r, u) => new
                {
                    ServicesRequests = r,
                    User = u
                }).Join(db.PublicCategory, c => (int)c.ServicesRequests.Status, c => c.Id, (r, p) => new V_ServicesRequests
                {
                    Address = r.ServicesRequests.Address,
                    CreateDate = r.ServicesRequests.CreateDate,
                    DeletedDate = r.ServicesRequests.DeletedDate,
                    Description = r.ServicesRequests.Description,
                    FinishComment = r.ServicesRequests.FinishComment,
                    FinishDate = r.ServicesRequests.FinishDate,
                    GeographicalCoordinates = r.ServicesRequests.GeographicalCoordinates,
                    Id = r.ServicesRequests.Id,
                    ImmediatelyRequest = r.ServicesRequests.ImmediatelyRequest,
                    IsDeleted = r.ServicesRequests.IsDeleted,
                    PersonelId = r.ServicesRequests.PersonelId,
                    PlaqueNumber = r.ServicesRequests.PlaqueNumber,
                    Rate = r.ServicesRequests.Rate,
                    ReferralDate = r.ServicesRequests.ReferralDate,
                    Status = r.ServicesRequests.Status,
                    UnitNumber = r.ServicesRequests.UnitNumber,
                    UserId = r.ServicesRequests.UserId,
                    User = r.User,
                    RequetsStatus = p.Title
                }).ToList();
        }

        /// <summary>
        /// دریافت یک دسته بندی خاص با تمامی جزئیات
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public V_ServicesRequests GetFullServicesRequests(int Id)
        {
            var db = new DatabaseContext();
            return db.ServicesRequests.Where(c => c.Id == Id && c.IsDeleted == false)
                .Join(db.Users, c => c.UserId, c => c.Id, (r, u) => new
                {
                    ServicesRequests = r,
                    User = u
                }).Join(db.PublicCategory, c => (int)c.ServicesRequests.Status, c => c.Id, (r, p) => new V_ServicesRequests
                {
                    Address = r.ServicesRequests.Address,
                    CreateDate = r.ServicesRequests.CreateDate,
                    DeletedDate = r.ServicesRequests.DeletedDate,
                    Description = r.ServicesRequests.Description,
                    FinishComment = r.ServicesRequests.FinishComment,
                    FinishDate = r.ServicesRequests.FinishDate,
                    GeographicalCoordinates = r.ServicesRequests.GeographicalCoordinates,
                    Id = r.ServicesRequests.Id,
                    ImmediatelyRequest = r.ServicesRequests.ImmediatelyRequest,
                    IsDeleted = r.ServicesRequests.IsDeleted,
                    PersonelId = r.ServicesRequests.PersonelId,
                    PlaqueNumber = r.ServicesRequests.PlaqueNumber,
                    Rate = r.ServicesRequests.Rate,
                    ReferralDate = r.ServicesRequests.ReferralDate,
                    Status = r.ServicesRequests.Status,
                    UnitNumber = r.ServicesRequests.UnitNumber,
                    UserId = r.ServicesRequests.UserId,
                    ToDoDate = r.ServicesRequests.ToDoDate,
                    UpdateDate = r.ServicesRequests.UpdateDate,
                    User = r.User,
                    RequetsStatus = p.Title,
                    Pouriya_Type = r.ServicesRequests.Pouriya_Type,
                    Pouriya_TimeId = r.ServicesRequests.Pouriya_TimeId,
                    Pouriya_DateId = r.ServicesRequests.Pouriya_DateId
                }).FirstOrDefault();
        }

        /// <summary>
        /// دریافت یک درخواست با توجه به شناسه درخواست
        /// </summary>
        /// <param name="Id">شناسه درخواست</param>
        /// <returns>درخواست مورئ نظر</returns>
        public M_ServicesRequests GetServicesRequests(int Id)
        {
            return new DatabaseContext().ServicesRequests.Where(c => c.Id == Id).FirstOrDefault();
        }

        /// <summary>
        /// دریافت یک درخواست با توجه به شناسه درخواست
        /// </summary>
        /// <param name="Id">شناسه درخواست</param>
        /// <param name="UserId">شناسه کاربر</param>
        /// <returns>درخواست مورئ نظر</returns>
        public M_ServicesRequests GetServicesRequests(int Id, int UserId)
        {
            var result = new DatabaseContext().ServicesRequests.Where(c => c.Id == Id && c.UserId == UserId).FirstOrDefault();
            if (result == null)
                throw F_ExeptionFactory.MakeExeption("درخواست مورد نظر برای این کاربر پیدا نشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "UserId", Enums.Loging.E_LogType.SYSTEM_ERROR);
            return result;
        }

        /// <summary>
        /// دریافت یک درخواست با توجه به شناسه درخواست برای پرسنل
        /// </summary>
        /// <param name="Id">شناسه درخواست</param>
        /// <param name="PersonelId">شناسه پرسنل</param>
        /// <returns>درخواست مورئ نظر</returns>
        public M_ServicesRequests GetServicesRequestsForPersonel(int Id, int PersonelId)
        {
            var result = new DatabaseContext().ServicesRequests.Where(c => c.Id == Id && c.PersonelId == PersonelId).FirstOrDefault();
            if (result == null)
                throw F_ExeptionFactory.MakeExeption("درخواست مورد نظر برای این کاربر پیدا نشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PersonelId", Enums.Loging.E_LogType.SYSTEM_ERROR);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PersonelId"></param>
        /// <returns></returns>
        public List<M_ServicesRequests> GetServicesRequestsForPersonel(int PersonelId,bool NotFinish)
        {
            var result = new List<M_ServicesRequests>();
            if (NotFinish)
                result = new DatabaseContext().ServicesRequests.Where(c =>c.PersonelId == PersonelId  && (c.Status == E_PublicCategory.REQUEST_STATUS.CLOSED || c.Status == E_PublicCategory.REQUEST_STATUS.REQUEST_FINISHED_FROM_USER || c.Status == E_PublicCategory.REQUEST_STATUS.WAIT_FOR_END_REQUEST_ACCEPT)).ToList();
            else
                result = new DatabaseContext().ServicesRequests.Where(c => c.PersonelId == PersonelId && c.CreateDate.Year + "/" + c.CreateDate.Month + "/" + c.CreateDate.Day ==DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day && c.Status == E_PublicCategory.REQUEST_STATUS.APPROVED_AND_REFERENCED).ToList();
            if (result == null)
                throw F_ExeptionFactory.MakeExeption("درخواست مورد نظر برای این کاربر پیدا نشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "PersonelId", Enums.Loging.E_LogType.SYSTEM_ERROR);
            return result;
        }

        /// <summary>
        /// دریافت درخواست ها با توجه به فیلتر های خاص
        /// </summary>
        /// <param name="StatusId">شناسه وضعیت</param>
        /// <param name="FirstName">نام</param>
        /// <param name="LastName">نام خانوادگی</param>
        /// <param name="FromCreateDate">از تاریخ</param>
        /// <param name="ToCreateDate">تا تاریخ</param>
        /// <param name="PageNumber">شماره صفحه</param>
        /// <returns>لیست درخواست های واکشی شده</returns>
        public List<V_ServicesRequests> GetFullServicesRequests(int StatusId, string FirstName, string LastName, DateTime? FromCreateDate, DateTime? ToCreateDate, int PageNumber, int PersonelId = 0)
        {
            var db = new DatabaseContext();
            return db.ServicesRequests.Where(c => c.IsDeleted == false
            && (StatusId == 0 || (int)c.Status == StatusId)
            && (!FromCreateDate.HasValue || c.CreateDate >= FromCreateDate.Value)
            && (PersonelId == 0 || c.PersonelId == PersonelId)
            && (!ToCreateDate.HasValue || c.CreateDate <= ToCreateDate.Value))
                .Join(db.Users.Where(c => c.FirstName.Contains(FirstName) && c.LastName.Contains(LastName)), c => c.UserId, c => c.Id, (r, u) => new
                {
                    ServicesRequests = r,
                    User = u
                }).Join(db.PublicCategory, c => (int)c.ServicesRequests.Status, c => c.Id, (r, p) => new V_ServicesRequests
                {
                    Address = r.ServicesRequests.Address,
                    CreateDate = r.ServicesRequests.CreateDate,
                    DeletedDate = r.ServicesRequests.DeletedDate,
                    Description = r.ServicesRequests.Description,
                    FinishComment = r.ServicesRequests.FinishComment,
                    FinishDate = r.ServicesRequests.FinishDate,
                    GeographicalCoordinates = r.ServicesRequests.GeographicalCoordinates,
                    Id = r.ServicesRequests.Id,
                    ImmediatelyRequest = r.ServicesRequests.ImmediatelyRequest,
                    IsDeleted = r.ServicesRequests.IsDeleted,
                    PersonelId = r.ServicesRequests.PersonelId,
                    PlaqueNumber = r.ServicesRequests.PlaqueNumber,
                    Rate = r.ServicesRequests.Rate,
                    ReferralDate = r.ServicesRequests.ReferralDate,
                    Status = r.ServicesRequests.Status,
                    UnitNumber = r.ServicesRequests.UnitNumber,
                    UserId = r.ServicesRequests.UserId,
                    User = r.User,
                    RequetsStatus = p.Title,
                    ToDoDate = r.ServicesRequests.ToDoDate,
                }).OrderByDescending(c => c.Status == E_PublicCategory.REQUEST_STATUS.NEW_REQUEST ? 999999999999 : c.Id)
                .ThenBy(c=>c.CreateDate)
                .Skip((PageNumber - 1) * PageCapacity)
                .Take(PageCapacity)
                .ToList();
        }

        /// <summary>
        /// دریافت لیستی از درخواست های با توجه به وضعیت آنها
        /// </summary>
        /// <param name="Status">وضعیت ها</param>
        /// <returns>درخواست های واکشی شده</returns>
        public List<V_ServicesRequests> GetFullServicesRequests(E_PublicCategory.REQUEST_STATUS[] Status)
        {
            var db = new DatabaseContext();
            return db.ServicesRequests.Where(c => c.IsDeleted == false && (Status).Contains(c.Status))
                .Join(db.Users, c => c.UserId, c => c.Id, (r, u) => new
                {
                    ServicesRequests = r,
                    User = u
                }).Join(db.PublicCategory, c => (int)c.ServicesRequests.Status, c => c.Id, (r, p) => new V_ServicesRequests
                {
                    Address = r.ServicesRequests.Address,
                    CreateDate = r.ServicesRequests.CreateDate,
                    DeletedDate = r.ServicesRequests.DeletedDate,
                    Description = r.ServicesRequests.Description,
                    FinishComment = r.ServicesRequests.FinishComment,
                    FinishDate = r.ServicesRequests.FinishDate,
                    GeographicalCoordinates = r.ServicesRequests.GeographicalCoordinates,
                    Id = r.ServicesRequests.Id,
                    ImmediatelyRequest = r.ServicesRequests.ImmediatelyRequest,
                    IsDeleted = r.ServicesRequests.IsDeleted,
                    PersonelId = r.ServicesRequests.PersonelId,
                    PlaqueNumber = r.ServicesRequests.PlaqueNumber,
                    Rate = r.ServicesRequests.Rate,
                    ReferralDate = r.ServicesRequests.ReferralDate,
                    Status = r.ServicesRequests.Status,
                    UnitNumber = r.ServicesRequests.UnitNumber,
                    UserId = r.ServicesRequests.UserId,
                    User = r.User,
                    RequetsStatus = p.Title
                }).ToList();
        }

        /// <summary>
        /// دریافت میانگین رتبه سرویس به وسیله شناسه پرسنل
        /// </summary>
        /// <param name="PersonelId">شناسه پرسنل</param>
        /// <returns>میانگین رتبه</returns>
        public double? GetServiceRateByPersonelId(int PersonelId)
        {
            var db = new DatabaseContext();
            return db.ServicesRequests.Where(c => c.PersonelId == PersonelId).Average(c => c.Rate);
        }

        /// <summary>
        /// ارجا دادن درخواست
        /// </summary>
        /// <param name="RequestId">شناسه درخواست</param>
        /// <param name="PersonelId">شناسه کاربر</param>
        /// <param name="ReferDate">تاریخ ارجا</param>
        public void ReferRequest(int RequestId, int PersonelId, DateTime ReferDate)
        {
            if (new B_Personels().GetPersonelById(PersonelId) == null)
                throw F_ExeptionFactory.MakeExeption("پرسنل انتخاب شده صحیح نیست", E_LogType.SYSTEM_ERROR);
            var db = new DatabaseContext();
            db.ServicesRequests.Where(c => c.Id == RequestId).Load();
            if (db.ServicesRequests.Local.Count <= 0)
                throw F_ExeptionFactory.MakeExeption("درخواست مورد نظر یافت نشد", E_LogType.SYSTEM_ERROR);
            var fromDate = ReferDate.AddMinutes(-90);
            var toDate = ReferDate.AddMinutes(90);
            var inTimeRequestCount = db.ServicesRequests.Where(c => (c.ReferralDate >= fromDate && c.ReferralDate <= toDate) && c.PersonelId == PersonelId && c.Id != RequestId).Count();
            if (inTimeRequestCount > 0)
                throw F_ExeptionFactory.MakeExeption("پرسنل انتخاب شده در این زمان کار برای انجام دارد , سرویس کار باید حداقل تا یک ساعت و سی دقیقه قبل و بعد از زمان مشخص شده زمان آزاد داشته باشد", E_LogType.SYSTEM_ERROR);
            db.ServicesRequests.Local[0].PersonelId = PersonelId;
            db.ServicesRequests.Local[0].ReferralDate = ReferDate;
            db.ServicesRequests.Local[0].ToDoDate = DateTime.Now;
            db.ServicesRequests.Local[0].Status = E_PublicCategory.REQUEST_STATUS.APPROVED_AND_REFERENCED;
            db.SaveChanges();
        }

        /// <summary>
        /// عوض کردن وضعیت درخواست
        /// </summary>
        /// <param name="RequestId">شناسه درخواست</param>
        /// <param name="UserId">شناسه کاربر</param>
        /// <param name="Status"وضعیت</param>
        public void ChangeStatus(int RequestId, int UserId, E_PublicCategory.REQUEST_STATUS Status)
        {
            DatabaseContext db = new DatabaseContext();
            db.ServicesRequests.Where(c => c.Id == RequestId && c.IsDeleted == false).Load();
            if (db.ServicesRequests.Local.Count == 0)
                throw F_ExeptionFactory.MakeExeption("درخواستی برای ویرایش پیدا نشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Id", Enums.Loging.E_LogType.SYSTEM_ERROR);
            if (db.ServicesRequests.Local.Where(c => c.UserId == UserId).Count() == 0)
                throw F_ExeptionFactory.MakeExeption("درخواست مورد نظر برای کاربر جاری نمیباشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Id", Enums.Loging.E_LogType.SYSTEM_ERROR);
            if (Status == E_PublicCategory.REQUEST_STATUS.CLOSED)
                throw F_ExeptionFactory.MakeExeption("شما نمیتوانید درخواست را ببندید",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Id", Enums.Loging.E_LogType.SYSTEM_ERROR);
            db.ServicesRequests.Local[0].Status = Status;
            db.SaveChanges();
        }

        /// <summary>
        /// عوض کردن وضعیت درخواست
        /// </summary>
        /// <param name="RequestId">شناسه درخواست</param>
        /// <param name="UserId">شناسه کاربر</param>
        /// <param name="Status">وضعیت</param>
        /// <param name="Comment">کامنت پایان کار</param>
        /// <param name="Rate">رتبه</param>
        public void ChangeStatus(int RequestId, int UserId, E_PublicCategory.REQUEST_STATUS Status, string Comment, int Rate)
        {
            DatabaseContext db = new DatabaseContext();
            db.ServicesRequests.Where(c => c.Id == RequestId && c.IsDeleted == false).Load();
            if (db.ServicesRequests.Local.Count == 0)
                throw F_ExeptionFactory.MakeExeption("درخواستی برای ویرایش پیدا نشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Id", Enums.Loging.E_LogType.SYSTEM_ERROR);
            if (db.ServicesRequests.Local.Where(c => c.UserId == UserId).Count() == 0)
                throw F_ExeptionFactory.MakeExeption("درخواست مورد نظر برای کاربر جاری نمیباشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Id", Enums.Loging.E_LogType.SYSTEM_ERROR);
            if (Status == E_PublicCategory.REQUEST_STATUS.CLOSED)
                throw F_ExeptionFactory.MakeExeption("شما نمیتوانید درخواست را ببندید",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Id", Enums.Loging.E_LogType.SYSTEM_ERROR);
            db.ServicesRequests.Local[0].Status = Status;
            db.ServicesRequests.Local[0].Rate = Rate;
            db.ServicesRequests.Local[0].FinishComment = Comment;
            db.ServicesRequests.Local[0].FinishDate = DateTime.Now;
            db.SaveChanges();
        }

        public List<V_ServicesRequests> GetFullLast30Request()
        {

            var db = new DatabaseContext();
            return db.ServicesRequests.Where(c => c.IsDeleted == false)
                .Join(db.Users, c => c.UserId, c => c.Id, (r, u) => new
                {
                    ServicesRequests = r,
                    User = u
                }).Join(db.PublicCategory, c => (int)c.ServicesRequests.Status, c => c.Id, (r, p) => new V_ServicesRequests
                {
                    Address = r.ServicesRequests.Address,
                    CreateDate = r.ServicesRequests.CreateDate,
                    DeletedDate = r.ServicesRequests.DeletedDate,
                    Description = r.ServicesRequests.Description,
                    FinishComment = r.ServicesRequests.FinishComment,
                    FinishDate = r.ServicesRequests.FinishDate,
                    GeographicalCoordinates = r.ServicesRequests.GeographicalCoordinates,
                    Id = r.ServicesRequests.Id,
                    ImmediatelyRequest = r.ServicesRequests.ImmediatelyRequest,
                    IsDeleted = r.ServicesRequests.IsDeleted,
                    PersonelId = r.ServicesRequests.PersonelId,
                    PlaqueNumber = r.ServicesRequests.PlaqueNumber,
                    Rate = r.ServicesRequests.Rate,
                    ReferralDate = r.ServicesRequests.ReferralDate,
                    Status = r.ServicesRequests.Status,
                    UnitNumber = r.ServicesRequests.UnitNumber,
                    UserId = r.ServicesRequests.UserId,
                    User = r.User,
                    RequetsStatus = p.Title
                })
                .OrderByDescending(c => c.Id)
                .Take(PageCapacity)
                .ToList();
        }

        public List<Tuple<int, E_PublicCategory.REQUEST_STATUS>> GetForChartType()
        {
            var db = new DatabaseContext();
            var res = db.ServicesRequests
                .Select(c => new { Status = c.Status, c.Id })
                .GroupBy(c => c.Status)
                .Select(c => new { Count = c.Count(), Type = c.Key })
                .ToList();
            return res.Select(c => new Tuple<int, E_PublicCategory.REQUEST_STATUS>(c.Count, c.Type)).ToList();
        }

        public bool AsignUserToRequest(int PersonalId, int RequestId)
        {
            var db = new DatabaseContext();
            db.ServicesRequests.Where(x => x.Id == RequestId).Load();
            if (db.ServicesRequests.Local.Count == 0)
                throw F_ExeptionFactory.MakeExeption("خطای ارسال اطلاعات",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "RequestId", Enums.Loging.E_LogType.SYSTEM_ERROR);
            db.ServicesRequests.Local[0].PersonelId = PersonalId;
            db.ServicesRequests.Local[0].Status = E_PublicCategory.REQUEST_STATUS.APPROVED_AND_REFERENCED;
            db.ServicesRequests.Local[0].ToDoDate = DateTime.Now;
            db.SaveChanges();
            return true;
        }

        public DateTime? GetLastRequestDateByUserId(int UserId)
        {
            var request = new DatabaseContext().ServicesRequests.Where(c => c.UserId == UserId).OrderByDescending(c => c.Id).FirstOrDefault();
            return request == null ? null : (DateTime?)request.CreateDate;
        }

        public void CloseRequestByPortal(int RequestId)
        {
            DatabaseContext db = new DatabaseContext();
            db.ServicesRequests.Where(c => c.Id == RequestId && c.IsDeleted == false).Load();
            if (db.ServicesRequests.Local.Count == 0)
                throw F_ExeptionFactory.MakeExeption("درخواستی برای ویرایش پیدا نشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Id", Enums.Loging.E_LogType.SYSTEM_ERROR);
            db.ServicesRequests.Local[0].FinishComment = "بسته شده توسط پرتال";
            db.ServicesRequests.Local[0].FinishDate = DateTime.Now;
            db.ServicesRequests.Local[0].Status = E_PublicCategory.REQUEST_STATUS.CLOSED;
            db.SaveChanges();
            new B_ServicesRequestItems().CommitScoreItems(RequestId, E_PublicCategory.SYSTEM_USER_TYPE.NORMAL_USER);
        }

        public bool CancelRequestPersonel(int Id,int DeleteMessageId,string DeleteMessage)
        {
            var db = new DatabaseContext();
            db.ServicesRequests.Where(x => x.Id == Id).Load();
            db.ServicesRequests.Local[0].IsDeleted = true;
            db.ServicesRequests.Local[0].DeletedDate = DateTime.Now;
            db.ServicesRequests.Local[0].DeleteDescription = DeleteMessage;
            db.ServicesRequests.Local[0].DeleteMessageId = DeleteMessageId;
            db.ServicesRequests.Local[0].Status = E_PublicCategory.REQUEST_STATUS.CANCEL;
            db.ServicesRequests.Local[0].IsDeletedUser = false;
            db.SaveChanges();
            return true;
        }

        public bool FinishRequestPersonel(int Id)
        {
            var db = new DatabaseContext();
            db.ServicesRequests.Where(x => x.Id == Id).Load();
            db.ServicesRequests.Local[0].FinishDate = DateTime.Now;
            db.SaveChanges();
            return true;
        }

        public List<M_ServicesRequests> GetServiceRequestOnWork(int PersonelId)
        {
            var db = new DatabaseContext();
            return db.ServicesRequests.Where(x => x.PersonelId == PersonelId && (x.Status == E_PublicCategory.REQUEST_STATUS.EXPERT_READY_TO_GO || x.Status == E_PublicCategory.REQUEST_STATUS.EXPERT_ON_PLACE)).ToList();
        }

        public bool FinishRequestUser(int Id,string FinishComment,int Rate,List<int> FinishMessageId)
        {
            var messageIds = "";
            if (FinishMessageId != null && FinishMessageId.Count > 0)
                foreach (var li in FinishMessageId)
                    messageIds += li + ",";
            var db = new DatabaseContext();
            db.ServicesRequests.Where(x => x.Id == Id).Load();
            db.ServicesRequests.Local[0].Status = E_PublicCategory.REQUEST_STATUS.CLOSED;
            db.ServicesRequests.Local[0].Rate = Rate;
            db.ServicesRequests.Local[0].FinishComment = FinishComment;
            db.ServicesRequests.Local[0].FinishMessageId = !string.IsNullOrEmpty(messageIds) ? messageIds.Remove(messageIds.Length - 1) : string.Empty; 
            db.ServicesRequests.Local[0].ShowRatingDialog = true;
            db.SaveChanges();
            return true;
        }

        public List<string> GetServiceMapPointes(int PersonelId)
        {
            var db = new DatabaseContext();
            return db.ServicesRequests
                .Where(c => c.PersonelId == PersonelId)
                .Select(c => c.GeographicalCoordinates)
                .ToList();
        }

        public M_ServicesRequests GetUserLastRequest(int UserId,string RequestType)
        {
            return new DatabaseContext().ServicesRequests.Where(c => c.UserId == UserId && c.IsDeleted == false && (c.IsDeletedUser == false || c.IsDeletedUser == null) && c.Status != E_PublicCategory.REQUEST_STATUS.CANCEL && c.Status != E_PublicCategory.REQUEST_STATUS.CLOSED && c.Status != E_PublicCategory.REQUEST_STATUS.REQUEST_FINISHED_FROM_USER && c.Pouriya_Type == RequestType).OrderByDescending(x=>x.CreateDate).FirstOrDefault();
        }

        public M_ServicesRequests GetUserLastRequest(int UserId,E_PublicCategory.REQUEST_STATUS Status)
        {
            return new DatabaseContext().ServicesRequests.Where(c => c.UserId == UserId && c.IsDeleted == false && (c.IsDeletedUser == false || c.IsDeletedUser == null) && c.Status == Status).OrderByDescending(x => x.CreateDate).FirstOrDefault();
        }
        
        /// <summary>
        /// دریافت لیستی از درخواست ها با توجه به شناسه پرسنل
        /// </summary>
        /// <param name="PersonelId">شناسه پرسنل</param>
        /// <returns>نتیجه تراکنش</returns>
        public List<V_ServicesRequests> GetServicesRequestsForPersonel(int PersonelId, E_PublicCategory.PERSONEL_TYPE Type)
        {
            var db = new DatabaseContext();
            return db.ServicesRequests
                .Where(c => c.PersonelId == PersonelId || (Type == E_PublicCategory.PERSONEL_TYPE.DRIVER && c.PersonelId == null))
                .Where(x=>x.Status != E_PublicCategory.REQUEST_STATUS.CANCEL)
                .Join(db.Users, c => c.UserId, c => c.Id, (r, u) => new
                {
                    ServicesRequests = r,
                    User = u
                }).Join(db.PublicCategory, c => (int)c.ServicesRequests.Status, c => c.Id, (r, p) => new V_ServicesRequests
                {
                    Address = r.ServicesRequests.Address,
                    CreateDate = r.ServicesRequests.CreateDate,
                    DeletedDate = r.ServicesRequests.DeletedDate,
                    Description = r.ServicesRequests.Description,
                    FinishComment = r.ServicesRequests.FinishComment,
                    FinishDate = r.ServicesRequests.FinishDate,
                    GeographicalCoordinates = r.ServicesRequests.GeographicalCoordinates,
                    Id = r.ServicesRequests.Id,
                    ImmediatelyRequest = r.ServicesRequests.ImmediatelyRequest,
                    IsDeleted = r.ServicesRequests.IsDeleted,
                    PersonelId = r.ServicesRequests.PersonelId,
                    PlaqueNumber = r.ServicesRequests.PlaqueNumber,
                    Rate = r.ServicesRequests.Rate,
                    ReferralDate = r.ServicesRequests.ReferralDate,
                    Status = r.ServicesRequests.Status,
                    UnitNumber = r.ServicesRequests.UnitNumber,
                    UserId = r.ServicesRequests.UserId,
                    User = r.User,
                    RequetsStatus = p.Title
                })
                .OrderByDescending(c => c.CreateDate)
                .ToList();
        }
    }
}
