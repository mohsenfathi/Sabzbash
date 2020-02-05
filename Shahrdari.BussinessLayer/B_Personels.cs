using Shahrdari.Factory.Log;
using Shahrdari.DataAccessLayer;
using Shahrdari.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.BussinessLayer
{
    /// <summary>
    /// بیزینس مربوط به پرسنل
    /// </summary>
    public class B_Personels : B_Base
    {
        /// <summary>
        /// اعتبار سنجی مدل
        /// </summary>
        /// <typeparam name="T">نوع مدل</typeparam>
        /// <param name="Data">داده</param>
        public override void Validate<T>(T Data)
        {
            base.Validate(Data);
            B_PersonelRoles bRole = new B_PersonelRoles();
            if (bRole.GetPersonelRoles(int.Parse(Data.GetType().GetProperty("PersonelRoleId").GetValue(Data).ToString())) == null)
                throw F_ExeptionFactory.MakeExeption("نقش انتخاب شده در سیستم وجود ندارد", Enums.Loging.E_LogType.SYSTEM_ERROR);
            DatabaseContext db = new DatabaseContext();
            int personelId = int.Parse(Data.GetType().GetProperty("Id").GetValue(Data).ToString());
            string personelUserName = Data.GetType().GetProperty("UserName").GetValue(Data).ToString();
            if (db.Personels.Where(c => c.UserName == personelUserName && c.Id != personelId).Count() > 0)
                throw F_ExeptionFactory.MakeExeption("نام کاربری در سیستم وجود دارد", Enums.Loging.E_LogType.SYSTEM_ERROR);

        }

        /// <summary>
        /// دریافت تمامی پرسنل اضافه شده
        /// </summary>
        /// <param name="IsOnlyActive">فقط کاربران فعال</param>
        /// <param name="HaveDeleted">کاربران حذف شده هم شامل شود ؟</param>
        /// <returns>پرسنل</returns>
        public List<M_Personels> GetPersonels(bool IsOnlyActive = false, bool HaveDeleted = false)
        {
            DatabaseContext db = new DatabaseContext();
            if (!IsOnlyActive)
                return db.Personels.Where(c => (c.IsDeleted == false || HaveDeleted)).ToList();
            else
                return db.Personels.Where(c => (c.IsDeleted == false || HaveDeleted) && c.IsActive == true).ToList();
        }

        /// <summary>
        /// دریافت لیست پرسنل با توجه به نقش آنها
        /// </summary>
        /// <param name="RoleId">شناسه نقش</param>
        /// <returns>پرسنل واکشی شده</returns>
        public List<M_Personels> GetPersonels(int RoleId)
        {
            DatabaseContext db = new DatabaseContext();
            return db.Personels.Where(c => c.PersonelRoleId == RoleId).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PersonelId"></param>
        /// <returns></returns>
        public M_Personels GetPersonel(int PersonelId)
        {
            return new DatabaseContext().Personels.Where(x => x.Id == PersonelId).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public M_Personels GetPersonelByUserName(string UserName,string Password)
        {
            DatabaseContext db = new DatabaseContext();
            return db.Personels.Where(c => c.UserName == UserName && c.Password == Password && c.IsActive == true && c.IsDeleted == false).FirstOrDefault();
        }

        /// <summary>
        /// دریافت یک کاربر با توجه به کلید آن
        /// </summary>
        /// <param name="Key">کلید</param>
        /// <returns>کاربر مربوطه</returns>
        public M_Personels GetPersonels(string Key)
        {
            DatabaseContext db = new DatabaseContext();
            return db.Personels.Where(c => c.UnicKey == Key && c.IsActive == true && c.IsDeleted == false).FirstOrDefault();
        }

        /// <summary>
        /// دریافت یک کاربر با توجه به شناسه و کد یکتا
        /// </summary>
        /// <param name="Key">کد یکتا</param>
        /// <param name="Id">شناسه</param>
        /// <returns>کابر واکشی شده</returns>
        public M_Personels GetPersonels(string Key,int Id)
        {
            DatabaseContext db = new DatabaseContext();
            return db.Personels.Where(c => c.UnicKey == Key && c.Id == Id && c.IsActive == true && c.IsDeleted == false).FirstOrDefault();
        }

        /// <summary>
        /// دریافت یک کاربر با توجه به نام کاربری و گذرواژه
        /// </summary>
        /// <param name="User">نام کاربری</param>
        /// <param name="Pass">گذرواژه</param>
        /// <returns>نتیجه عملیات</returns>
        public M_Personels GetPersonels(string User, string Pass)
        {
            DatabaseContext db = new DatabaseContext();
            return db.Personels.Where(c => c.UserName == User && c.Password == Pass && c.IsActive == true && c.IsDeleted == false).FirstOrDefault();
        }

        /// <summary>
        /// دریافت یک کاربر با توجه به شماره تلفن و گذرواژه
        /// </summary>
        /// <param name="PhoneNumber">شماره تلفن</param>
        /// <param name="Pass">گذرواژه</param>
        /// <returns>نتیجه عملیات</returns>
        public M_Personels GetPersonelsByPhone(string PhoneNumber, string Pass)
        {
            DatabaseContext db = new DatabaseContext();
            return db.Personels.Where(c => c.MobileNumber == PhoneNumber && c.Password == Pass && c.IsActive == true && c.IsDeleted == false).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>
        public M_Personels GetPersonelsByPhone(string PhoneNumber)
        {
            DatabaseContext db = new DatabaseContext();
            return db.Personels.Where(c => c.MobileNumber == PhoneNumber && c.IsActive == true && c.IsDeleted == false).FirstOrDefault();
        }

        /// <summary>
        /// دریافت یک کارمند با توجه به شناسه آن
        /// </summary>
        /// <param name="Id">شناسه</param>
        /// <returns>کارمند یافته شده</returns>
        public M_Personels GetPersonelById(int Id)
        {
            DatabaseContext db = new DatabaseContext();
            return db.Personels.Where(c => c.Id == Id).FirstOrDefault();
        }

        /// <summary>
        /// افزودن کارمند جدید
        /// </summary>
        /// <param name="Personel">کارمند مورد نظر</param>
        /// <returns>کارمند اضافه شده</returns>
        public M_Personels Add(M_Personels Personel)
        {
            Validate(Personel);
            if (!Personel.IsActive)
                Personel.DeactiveDate = DateTime.Now;
            DatabaseContext db = new DatabaseContext();
            var max = db.Personels.Max(c => c.ReagentCode);
            if (max < 100000)
                max = 100000;
            Personel.ReagentCode = max + 1;
            db.Personels.Add(Personel);
            db.SaveChanges();
            return Personel;
        }

        /// <summary>
        /// ویرایش پرسنل
        /// </summary>
        /// <param name="Personel">کارمند مورد نظر</param>
        /// <param name="HaveImage">عکس تغییر کرده ؟</param>
        public void Edit(M_Personels Personel)
        {
            Validate(Personel);
            if (!Personel.IsActive)
                Personel.DeactiveDate = DateTime.Now;
            DatabaseContext db = new DatabaseContext();
            db.Personels.Where(c => c.Id == Personel.Id).Load();
            db.Personels.Local[0].BirthDate = Personel.BirthDate;
            db.Personels.Local[0].DeactiveDate = Personel.DeactiveDate;
            db.Personels.Local[0].DeletedDate = Personel.DeletedDate;
            db.Personels.Local[0].FirstName = Personel.FirstName;
            db.Personels.Local[0].ImageName = Personel.ImageName;
            db.Personels.Local[0].IsActive = Personel.IsActive;
            db.Personels.Local[0].IsDeleted = Personel.IsDeleted;
            db.Personels.Local[0].LastName = Personel.LastName;
            db.Personels.Local[0].LastOnline = Personel.LastOnline;
            db.Personels.Local[0].MobileNumber = Personel.MobileNumber;
            db.Personels.Local[0].Password = Personel.Password;
            db.Personels.Local[0].PersonelRoleId = Personel.PersonelRoleId;
            db.Personels.Local[0].UserName = Personel.UserName;
            db.Personels.Local[0].Gender = Personel.Gender;
            db.Personels.Local[0].PersonelType = Personel.PersonelType;
            db.Personels.Local[0].VehicleType = Personel.VehicleType;
            db.Personels.Local[0].VehiclePlaq = Personel.VehiclePlaq;
            db.Personels.Local[0].VehicleDesc = Personel.VehicleDesc;
            db.Personels.Local[0].SumCenterAddress = Personel.SumCenterAddress;
            db.Personels.Local[0].SumCenterTell = Personel.SumCenterTell;
            db.SaveChanges();
        }

        /// <summary>
        /// حذف یک کاربر خاص
        /// </summary>
        /// <param name="Id">شناسه کاربر</param>
        public void Delete(int Id)
        {
            DatabaseContext db = new DatabaseContext();
            db.Personels.Where(c => c.Id == Id).Load();
            db.Personels.Local[0].IsDeleted = true;
            db.Personels.Local[0].DeletedDate = DateTime.Now;
            db.SaveChanges();
        }

        /// <summary>
        /// دریافت پرسنل حذف شده
        /// </summary>
        /// <returns>پرسنل حذف شده</returns>
        public List<M_Personels> GetDeletedPersonel()
        {
            DatabaseContext db = new DatabaseContext();
            return db.Personels.Where(c => c.IsDeleted == true).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>
        public bool ChangeUsername(int Id,string PhoneNumber)
        {
            var db = new DatabaseContext();
            db.Personels.Where(x => x.Id == Id).Load();
            if(db.Personels.Local.Count == 0)
                throw F_ExeptionFactory.MakeExeption("کاربر مورد نظر وجود ندارد", Enums.Loging.E_LogType.SYSTEM_ERROR);
            db.Personels.Local[0].UserName = PhoneNumber;
            db.Personels.Local[0].MobileNumber = PhoneNumber;
            db.Personels.Local[0].IsActive = false;
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        public void VerifyPersonel(int Id)
        {
            var db = new DatabaseContext();
            db.Personels.Where(x => x.Id == Id).Load();
            db.Personels.Local[0].IsVerify = true;
            db.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserName">نام کاربری یا شماره تلفن</param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public bool ChangePassword(string UserName,string Password)
        {
            var db = new DatabaseContext();
            db.Personels.Where(x => x.UserName == UserName || x.MobileNumber == UserName).Load();
            db.Personels.Local[0].Password = Password;
            db.SaveChanges();
            return true;
        }

        public bool ChangeServiceRun(int PersonelId,bool ServiceRun)
        {
            var db = new DatabaseContext();
            db.Personels.Where(x => x.Id == PersonelId).Load();
            db.Personels.Local[0].ServiceRun = ServiceRun;
            db.SaveChanges();
            return true;
        }
    }
}
