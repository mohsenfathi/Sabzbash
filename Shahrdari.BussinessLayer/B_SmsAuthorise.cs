using Shahrdari.DataAccessLayer;
using Shahrdari.Enums;
using Shahrdari.Factory.Log;
using Shahrdari.Settings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shahrdari.Models;

namespace Shahrdari.BussinessLayer
{
    /// <summary>
    /// بیزینس مربوط به کدهای فعالسازی
    /// </summary>
    public class B_SmsAuthorise : B_Base
    {
        /// <summary>
        /// افزودن کد جدید
        /// </summary>
        /// <param name="PhoneNumber">شماره تلفن</param>
        /// <returns>کد فعالسازی</returns>
        public int AddCode(string PhoneNumber)
        {
            var code = new Random().Next(111111, 999999);
            var db = new DatabaseContext();
            db.SmsAuthorise.Where(x => x.PhoneNumber == PhoneNumber).Load();
            if (db.SmsAuthorise.Local.Count != 0)
                db.SmsAuthorise.Local[0].Code = code;
            else
                db.SmsAuthorise.Add(new M_SmsAuthorise { Code = code, PhoneNumber = PhoneNumber });
            db.SaveChanges();
            return code;
        }

        /// <summary>
        /// بررسی صحت کد فعالسازی
        /// </summary>
        /// <param name="Code">کد فعالسازی</param>
        /// <param name="PhoneNumber">شماره تلفن همراه</param>
        /// <returns>نتیجه تراکنش</returns>
        public void IsValidCode(int Code, string PhoneNumber)
        {
            var db = new DatabaseContext();
            if (db.SmsAuthorise.Where(c => c.PhoneNumber == PhoneNumber && c.Code == Code).Count() == 0)
                    throw F_ExeptionFactory.MakeExeption("کد مورد نظر یافت نشد",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Code", Enums.Loging.E_LogType.SYSTEM_ERROR);
            db.SmsAuthorise.Where(c => c.PhoneNumber == PhoneNumber && c.Code == Code).Load();
            db.SmsAuthorise.Remove(db.SmsAuthorise.Local[0]);
            db.SaveChanges();
        }

        /// <summary>
        /// بررسی صحت کد فعالسازی
        /// </summary>
        /// <param name="Id">شناسه</param>
        /// <param name="Code">کد فعالسازی</param>
        /// <param name="PhoneNumber">شماره تلفن همراه</param>
        /// <returns>نتیجه تراکنش</returns>
        public bool IsValidCode(int Id,int Code)
        {
            var db = new DatabaseContext();
            if (db.SmsAuthorise.Where(c => c.Code == Code && c.Id == Id).Count() == 0)
                throw F_ExeptionFactory.MakeExeption("کد مورد نظر یافت نشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Code", Enums.Loging.E_LogType.SYSTEM_ERROR);
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="VId"></param>
        /// <returns></returns>
        public bool DeleteVerificationCode(int VId)
        {
            var db = new DatabaseContext();
            db.SmsAuthorise.Where(c => c.Id == VId).Load();
            db.SmsAuthorise.Remove(db.SmsAuthorise.Local[0]);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public M_SmsAuthorise GetSmsDetails(int Id)
        {
            return new DatabaseContext().SmsAuthorise.Where(x => x.Id == Id).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>
        public int GetSmsDetailsId(string PhoneNumber)
        {
            return new DatabaseContext().SmsAuthorise.Where(x => x.PhoneNumber == PhoneNumber).Select(x => x.Id).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int ResendVerificationCode(int Id)
        {
            var code = new Random().Next(111111, 999999);
            var db = new DatabaseContext();
            db.SmsAuthorise.Where(x => x.Id == Id).Load();
            db.SmsAuthorise.Local[0].Code = code;
            db.SaveChanges();
            return code;
        }
    }
}
