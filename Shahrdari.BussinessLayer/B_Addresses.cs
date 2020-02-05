using Shahrdari.DataAccessLayer;
using Shahrdari.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Shahrdari.Factory.Log;
using Shahrdari.Enums;
using Shahrdari.Settings;

namespace Shahrdari.BussinessLayer
{
    /// <summary>
    /// بیزینس مربوط به آدرس ها
    /// </summary>
    public class B_Addresses : B_Base
    {
        /// <summary>
        /// افزودن آدرس جدید
        /// </summary>
        /// <param name="Address">مندل مربوط به ادرس</param>
        /// <returns>ادرس اضافه شده</returns>
        public M_Addresses Add(M_Addresses Address)
        {
            Validate(Address);
            var db = new DatabaseContext();
            db.Addresses.Add(Address);
            db.SaveChanges();
            return Address;
        }

        /// <summary>
        /// ویرایش آدرس
        /// </summary>
        /// <param name="Address">مدل مربوط آدرس</param>
        public void Edit(M_Addresses Address)
        {
            Validate(Address);
            var db = new DatabaseContext();
            db.Addresses.Where(c => c.Id == Address.Id).Load();
            if (db.Addresses.Local.Count <= 0)
                throw F_ExeptionFactory.MakeExeption("آدرسی برای ویرایش یافت نشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Address", Enums.Loging.E_LogType.SYSTEM_ERROR);
            db.Addresses.Local[0].Address = Address.Address;
            //db.Addresses.Local[0].Lat = Address.Lat;
            //db.Addresses.Local[0].Lng = Address.Lng;
            db.Addresses.Local[0].Type = Address.Type;
            db.Addresses.Local[0].Title = Address.Title;
            db.Addresses.Local[0].Plaque = Address.Plaque;
            db.Addresses.Local[0].Unit = Address.Unit;
            db.SaveChanges();
        }

        /// <summary>
        /// حذف آدرس
        /// </summary>
        /// <param name="Id">شناسه آدرس</param>
        public void Delete(int Id)
        {
            var db = new DatabaseContext();
            db.Addresses.Where(c => c.Id == Id).Load();
            if (db.Addresses.Local.Count <= 0)
                throw F_ExeptionFactory.MakeExeption("آدرسی برای حذف یافت نشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Address", Enums.Loging.E_LogType.SYSTEM_ERROR);
            db.Addresses.Remove(db.Addresses.Local[0]);
            db.SaveChanges();
        }

        /// <summary>
        /// حذف آدرس
        /// </summary>
        /// <param name="UserId">شناسه کاربر</param>
        /// <param name="IsUser">فلگ</param>
        public void Delete(int UserId,bool IsUser = true)
        {
            var db = new DatabaseContext();
            db.Addresses.Where(c => c.UserId == UserId).Load();
            if (db.Addresses.Local.Count <= 0)
                throw F_ExeptionFactory.MakeExeption("آدرسی برای حذف یافت نشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Address", Enums.Loging.E_LogType.SYSTEM_ERROR);
            db.Addresses.RemoveRange(db.Addresses.Local);
            db.SaveChanges();
        }

        /// <summary>
        /// دریافت یک آدرس با توجه به شناسه آن
        /// </summary>
        /// <param name="Id">شناسه آدرس</param>
        /// <returns>نتیجه تراکنش</returns>
        public M_Addresses GetAddress(int Id)
        {
            return new DatabaseContext().Addresses.Where(c => c.Id == Id).FirstOrDefault();
        }

        /// <summary>
        /// دریافت لیست آدرس با توجه به شناسه کاربر
        /// </summary>
        /// <param name="UserId">شناسه کاربر</param>
        /// <returns>نتیجه تراکنش</returns>
        public List<M_Addresses> GetAddresses(int UserId,bool IsFavorite=false)
        {
            return new DatabaseContext().Addresses.Where(c => c.UserId == UserId && c.IsFavorite == IsFavorite).ToList();
        }
    }
}
