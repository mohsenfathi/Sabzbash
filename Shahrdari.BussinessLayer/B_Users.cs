using Shahrdari.DataAccessLayer;
using Shahrdari.Enums;
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
    /// بیزینس مربوط به کاربر
    /// </summary>
    public class B_Users : B_Base
    {
        public override void Validate<T>(T Data)
        {
            base.Validate(Data);
            var db = new DatabaseContext();
            var mobileNumber = typeof(T).GetProperty("MobileNumber").GetValue(Data).ToString();
            var id = int.Parse(typeof(T).GetProperty("Id").GetValue(Data).ToString());
            if (db.Users.Where(c => c.Id != id && c.MobileNumber == mobileNumber && c.IsDeleted == false).Count() > 0)
                throw F_ExeptionFactory.MakeExeption($"شماره تلفن {mobileNumber} قبلا در سیستم ثبت شده است",
                    ((int)E_ErrorCodes.HAVE_USER_MOBILE_NUMBER) + S_Seprators.ErrorFieldNameSeprator.ToString() + "MobileNumber", Enums.Loging.E_LogType.SYSTEM_ERROR);
        }

        /// <summary>
        /// دریافت تمامی کاربران ثبت شده در بانک اطلاعاتی
        /// </summary>
        /// <returns>تمامی کاربران</returns>
        public List<M_Users> GetUsers()
        {
            return new DatabaseContext().Users.Where(c => c.IsDeleted == false).OrderByDescending(c => c.Id).ToList();
        }

        /// <summary>
        /// دریافت یک کاربر با شناسه ئ کد یکتا
        /// </summary>
        /// <param name="Id">شناسه کاربر</param>
        /// <param name="UnicCode">کد یکتا</param>
        /// <returns>کاربر واکشی شده</returns>
        public M_Users GetUsers(int Id, string UnicCode)
        {
            // && c.IsActive == true
            return new DatabaseContext().Users.Where(c => c.Id == Id && c.UnicKey == UnicCode && c.IsDeleted == false).FirstOrDefault();
        }

        /// <summary>
        /// دریافت تمامی کاربران ثبت شده در بانک اطلاعاتی  با توجه به نام فامیلی و تفن همراه با صفحه بندی
        /// </summary>
        /// <param name="PageNumber">شماره صفحه</param>
        /// <param name="Family">فامیلی</param>
        /// <param name="Mobile">تلفن همراه</param>
        /// <param name="Name">نام</param>
        /// <returns>تمامی کاربران</returns>
        public List<V_Users> GetUsers(int PageNumber, string Name, string Family, string Mobile)
        {
            var users =  new DatabaseContext().Users.Where(c => c.IsDeleted == false
            && c.FirstName.Contains(Name) && c.LastName.Contains(Family) && Mobile.Contains(Mobile)).OrderByDescending(c => c.Id)
                .Skip((PageNumber - 1) * PageCapacity).Take(PageCapacity).ToList();
            return B_PublicFunctions.GenericMaper<M_Users, V_Users>(users);
        }
        
        /// <summary>
        /// دریافت تمامی کاربران ثبت شده در بانک اطلاعاتی با توجه به نام فامیلی و تفن همراه
        /// </summary>
        /// <param name="Family">فامیلی</param>
        /// <param name="Mobile">تلفن همراه</param>
        /// <param name="Name">نام</param>
        /// <returns>تمامی کاربران</returns>
        public List<M_Users> GetUsers(string Name, string Family, string Mobile)
        {
            return new DatabaseContext().Users.Where(c => c.IsDeleted == false && c.FirstName.Contains(Name) && c.LastName.Contains(Family) && Mobile.Contains(Mobile)).ToList();
        }

        /// <summary>
        /// درافت کاربران با توجه به لیستی از شناسه ها
        /// </summary>
        /// <param name="Ids">شناسه های مورد نظر</param>
        /// <returns>نتیجه تراکنش</returns>
        public List<M_Users> GetUsers(List<int> Ids)
        {
            return new DatabaseContext().Users.Where(c => Ids.Contains(c.Id)).ToList();
        }

        /// <summary>
        /// دریافت یک کاربر با توجه به شناسه آن
        /// </summary>
        /// <param name="Id">شناسه کاربر</param>
        /// <returns>کاربر واکشی شده</returns>
        public M_Users GetUsers(int Id)
        {
            return new DatabaseContext().Users.Where(c => c.Id == Id).FirstOrDefault();
        }

        /// <summary>
        /// افزودن کاربر جدید
        /// </summary>
        /// <param name="User">کاربر مد نظر</param>
        /// <returns>نتیجه عملیات</returns>
        public M_Users Add(M_Users User)
        {
            Validate(User);
            User.RegisterDate = User.LastOnline = DateTime.Now;
            User.IsDeleted = false;
            User.IsActive = false;
            User.DeletedDate = User.DeactiveDate = null;
            var db = new DatabaseContext();
            var max = db.Users.Max(c => c.ReagentCode);
            if (max < 100000)
                max = 100000;
            User.ReagentCode = max + 1;
            db.Users.Add(User);
            db.SaveChanges();

            /*Middleware.MiddlewareClient cl = new Middleware.MiddlewareClient();
            Middleware.M_KhadamatiUser convertedUser = new Middleware.M_KhadamatiUser();
            convertedUser.BirthDate = User.BirthDate;
            convertedUser.DeactiveDate = User.DeactiveDate;
            convertedUser.DeletedDate = User.DeletedDate;
            convertedUser.Email = User.Email;
            convertedUser.FirstName = User.FirstName;
            convertedUser.Gender = User.Gender == null ? (int?)null : Convert.ToInt32(User.Gender);
            convertedUser.Id = User.Id;
            convertedUser.ImageName = User.ImageName;
            convertedUser.IsActive = User.IsActive;
            convertedUser.IsDeleted = User.IsDeleted;
            convertedUser.LastName = User.LastName;
            convertedUser.LastOnline = User.LastOnline;
            convertedUser.MobileNumber = User.MobileNumber;
            convertedUser.RegisterDate = User.RegisterDate;
            convertedUser.UnicKey = User.UnicKey;
            cl.CreateSepidarUser(convertedUser);*/

            return User;
        }

        /// <summary>
        /// ویرایش یک کاربر
        /// </summary>
        /// <param name="User">کاربر مورد نظر</param>
        /// <param name="CheckUnicCode">کد یکتا چک شود ؟</param>
        public void Edit(M_Users User, bool CheckUnicCode = false)
        {
            if (!CheckUnicCode)
                Validate(User);
            DatabaseContext db = new DatabaseContext();
            if (!CheckUnicCode)
                db.Users.Where(c => c.Id == User.Id && c.IsDeleted == false).Load();
            else
                db.Users.Where(c => c.Id == User.Id && c.UnicKey == User.UnicKey && c.IsDeleted == false).Load();
            if (db.Users.Local.Count <= 0)
                throw F_ExeptionFactory.MakeExeption("کاربری برای ویرایش یافت نشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "User", Enums.Loging.E_LogType.SYSTEM_ERROR);
            if (CheckUnicCode)
            {
                User.LastOnline = db.Users.Local[0].LastOnline;
                User.RegisterDate = db.Users.Local[0].RegisterDate;
                Validate(User);
            }
            if (!CheckUnicCode)
            {
                db.Users.Local[0].IsActive = User.IsActive;
                db.Users.Local[0].DeactiveDate = User.DeactiveDate;
            }
            db.Users.Local[0].BirthDate = User.BirthDate;
            db.Users.Local[0].FirstName = User.FirstName;
            db.Users.Local[0].Gender = User.Gender;
            db.Users.Local[0].ImageName = User.ImageName;
            db.Users.Local[0].LastName = User.LastName;
            db.Users.Local[0].MobileNumber = User.MobileNumber;
            db.Users.Local[0].Email = User.Email;
            db.Users.Local[0].UserType = User.UserType;
            db.Users.Local[0].InstituteType = User.InstituteType;
            db.Users.Local[0].InstituteName = User.InstituteName;
            db.Users.Local[0].PostalCode = User.PostalCode;
            db.Users.Local[0].Tell = User.Tell;
            db.SaveChanges();
        }

        public void EditForSignup(M_Users User)
        {
            DatabaseContext db = new DatabaseContext();
            db.Users.Where(c => c.Id == User.Id && c.UnicKey == User.UnicKey).Load();
            db.Users.Local[0].BirthDate = User.BirthDate;
            db.Users.Local[0].FirstName = User.FirstName;
            db.Users.Local[0].Gender = User.Gender;
            db.Users.Local[0].ImageName = User.ImageName;
            db.Users.Local[0].LastName = User.LastName;
            db.Users.Local[0].MobileNumber = User.MobileNumber;
            db.Users.Local[0].Email = User.Email;
            db.Users.Local[0].UserType = User.UserType;
            db.Users.Local[0].InstituteType = User.InstituteType;
            db.Users.Local[0].InstituteName = User.InstituteName;
            db.Users.Local[0].PostalCode = User.PostalCode;
            db.Users.Local[0].Tell = User.Tell;
            db.Users.Local[0].IsDeleted = false;
            db.Users.Local[0].IsActive = false;
            db.SaveChanges();
        }

        /// <summary>
        /// حذف یک کاربر
        /// </summary>
        /// <param name="Id">شناسه کاربر</param>
        public void Delete(int Id)
        {
            DatabaseContext db = new DatabaseContext();
            db.Users.Where(c => c.Id == Id && c.IsDeleted == false).Load();
            if (db.Users.Local.Count <= 0)
                throw F_ExeptionFactory.MakeExeption("کاربری برای حذف یافت نشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "User", Enums.Loging.E_LogType.SYSTEM_ERROR);
            db.Users.Local[0].IsDeleted = true;
            db.Users.Local[0].DeletedDate = DateTime.Now;
            db.SaveChanges();
        }

        /// <summary>
        /// حذف یک کاربر با چک کردن بونید کد
        /// </summary>
        /// <param name="Id">شناسه کاربر</param>
        /// <param name="UnicCode">کد یکتا</param>
        public void Delete(int Id, string UnicCode)
        {
            DatabaseContext db = new DatabaseContext();
            db.Users.Where(c => c.Id == Id && c.UnicKey == UnicCode && c.IsDeleted == false && c.IsActive == true).Load();
            if (db.Users.Local.Count <= 0)
                throw F_ExeptionFactory.MakeExeption("کاربری برای حذف یافت نشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "User", Enums.Loging.E_LogType.SYSTEM_ERROR);
            db.Users.Local[0].IsDeleted = true;
            db.Users.Local[0].DeletedDate = DateTime.Now;
            db.SaveChanges();
        }

        /// <summary>
        /// دریافت یک کاربر با توجه به شماره تلفن
        /// </summary>
        /// <param name="PhoneNumber">شماره تلفن</param>
        /// <returns>کاربر واکشی شده</returns>
        public M_Users GetUsers(string PhoneNumber)
        {
            var user = new DatabaseContext().Users.Where(c => c.MobileNumber == PhoneNumber).FirstOrDefault();
            return user;
        }

        /// <summary>
        /// دریافت کاربر با توجه به کلید یکتای آن
        /// </summary>
        /// <param name="UnicKey">کلید یکتا</param>
        /// <returns>کاربر واکشی شده</returns>
        public M_Users GetUsersByToken(string UnicKey)
        {
            var user = new DatabaseContext().Users.Where(c => c.UnicKey == UnicKey).FirstOrDefault();
            return user;
        }

        /// <summary>
        /// دریافت کاربران حذف شده
        /// </summary>
        /// <returns>تمامی کاربران حذف شده</returns>
        public List<M_Users> GetDeletedUsers()
        {
            return new DatabaseContext().Users.Where(c => c.IsDeleted == true).ToList();
        }

        /// <summary>
        /// بازگردانی کابر حذف شده
        /// </summary>
        /// <param name="Id">شناسه کاربر</param>
        public void RevertUser(int Id)
        {
            var db = new DatabaseContext();
            db.Users.Where(c => c.Id == Id).Load();
            if (db.Users.Local.Count <= 0)
                throw F_ExeptionFactory.MakeExeption("کاربری برای بازگردانی یافت نشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "User", Enums.Loging.E_LogType.SYSTEM_ERROR);
            db.Users.Local[0].IsDeleted = false;
            db.SaveChanges();
        }

        /// <summary>
        /// دریافت کاربر با توجه کد فعالسازی
        /// </summary>
        /// <param name="ReagentCode">کد فعالسازی</param>
        /// <returns>کاربر واکشی شده</returns>
        public M_Users GetUserByReagentCode(int ReagentCode)
        {
            return new DatabaseContext().Users.Where(c => c.ReagentCode == ReagentCode).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PhoneNumber"></param>
        public void ActiveUser(string PhoneNumber)
        {
            DatabaseContext db = new DatabaseContext();
            db.Users.Where(x => x.MobileNumber == PhoneNumber).Load();
            db.Users.Local[0].IsActive = true;
            db.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="NewPassword"></param>
        /// <returns></returns>
        public bool ChangePassword(string UserName,string NewPassword)
        {
            var db = new DatabaseContext();
            db.Users.Where(x => x.MobileNumber == UserName).Load();
            db.Users.Local[0].Password = NewPassword;
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool ChangeOnlineDate(int UserId)
        {
            var db = new DatabaseContext();
            db.Users.Where(x => x.Id == UserId).Load();
            if (db.Users.Local.Count != 0)
                db.Users.Local[0].LastOnline = DateTime.Now;
            db.SaveChanges();
            return true;
        }
    }
}
