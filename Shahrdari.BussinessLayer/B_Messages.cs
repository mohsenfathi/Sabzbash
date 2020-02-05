using Shahrdari.DataAccessLayer;
using Shahrdari.Enums;
using Shahrdari.Factory.Log;
using Shahrdari.Models;
using Shahrdari.Settings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.BussinessLayer
{
    /// <summary>
    /// پیام ها
    /// </summary>
    public class B_Messages : B_Base
    {
        /// <summary>
        /// دریافت همه پیام ها با توجه به شناسه کاربر
        /// </summary>
        /// <param name="UserId">شناسه کاربر</param>
        /// <returns>نتیجه تراکنش</returns>
        public List<M_Messages> GetMessgas()
        {
            return new DatabaseContext().Messages.ToList();
        }

        /// <summary>
        /// دریافت یک پیغام خواص
        /// </summary>
        /// <param name="Id">شناسه پیغام</param>
        /// <returns>نتیجه تراکنش</returns>
        public M_Messages GetMessgas(int Id)
        {
            return new DatabaseContext().Messages.Where(c=>c.Id == Id).FirstOrDefault();
        }

        /// <summary>
        /// افزودن پیام جدید
        /// </summary>
        /// <param name="Message">مدل مربوط به پیام</param>
        /// <returns>مقدار اضافه شده به پایگاه داده</returns>
        public M_Messages Add(M_Messages Message)
        {
            Validate(Message);
            var db = new DatabaseContext();
            Message.CreateDate = DateTime.Now;
            Message = db.Messages.Add(Message);
            db.SaveChanges();
            return Message;
        }

        /// <summary>
        /// ویرایش پیغام
        /// </summary>
        /// <param name="Message">پیغام مربوطه </param>
        /// <returns>نتیجه تراکنش</returns>
        public M_Messages Edit(M_Messages Message)
        {
            Validate(Message);
            var db = new DatabaseContext();
            db.Messages.Where(c=>c.Id == Message.Id).Load();
            if(db.Messages.Local.Count == 0)
                throw F_ExeptionFactory.MakeExeption("پیغامی برای ویرایش یافت نشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Messages", Enums.Loging.E_LogType.SYSTEM_ERROR);
            db.Messages.Local[0].messages = Message.messages;
            db.Messages.Local[0].title = Message.title;
            db.SaveChanges();
            return Message;
        }

        /// <summary>
        /// حذف پیام خواص
        /// </summary>
        /// <param name="Id">شناسه پیام</param>
        /// <returns>نتیجه عملیات</returns>
        public bool Delete(int Id)
        {
            var db = new DatabaseContext();
            db.Messages.Where(c => c.Id == Id).Load();
            if (db.Messages.Local.Count == 0)
                throw F_ExeptionFactory.MakeExeption("پیغامی برای حذف یافت نشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Messages", Enums.Loging.E_LogType.SYSTEM_ERROR);
            db.Messages.Remove(db.Messages.Local[0]);
            db.SaveChanges();
            return true;
        }
    }
}
