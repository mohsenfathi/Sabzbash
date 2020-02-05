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
    /// بیزینس مربوط به آموزش ها
    /// </summary>
    public class B_Learns : B_Base
    {
        /// <summary>
        /// افزودن اموزش جدید به سیستم
        /// </summary>
        /// <param name="Learn">مدل مربوط به اموزش</param>
        /// <returns>مدل اصافه شده</returns>
        public M_Learns Add(M_Learns Learn)
        {
            Validate(Learn);
            var db = new DatabaseContext();
            db.Learns.Add(Learn);
            db.SaveChanges();
            return Learn;
        }

        /// <summary>
        /// ویرایش مقدار آموزش
        /// </summary>
        /// <param name="Learn">مدل مربوط به آموزش</param>
        public void Edit(M_Learns Learn)
        {
            Validate(Learn);
            var db = new DatabaseContext();
            db.Learns.Where(c => c.Id == Learn.Id).Load();
            db.Learns.Local[0].LongDesc = Learn.LongDesc;
            db.Learns.Local[0].ShortDesc = Learn.ShortDesc;
            db.Learns.Local[0].Title = Learn.Title;
            db.Learns.Local[0].Video = Learn.Video;
            db.Learns.Local[0].CoverImage = Learn.CoverImage;
            db.SaveChanges();
        }

        /// <summary>
        /// حذف یک مقدار
        /// </summary>
        /// <param name="Id">شناسه اموزش مورد نظر</param>
        public void Delete(int Id)
        {
            var db = new DatabaseContext();
            db.Learns.Remove(db.Learns.Where(c => c.Id == Id).FirstOrDefault());
            db.SaveChanges();
        }

        /// <summary>
        /// دریافت تمامی اموزش های ثبت شده
        /// </summary>
        /// <returns>اموزش های سیستم</returns>
        public List<M_Learns> GetLearns()
        {
            return new DatabaseContext().Learns.ToList();
        }

        /// <summary>
        /// دریافت یک اموزش خاص
        /// </summary>
        /// <param name="Id">شناسه آموزش</param>
        /// <returns>اموزش واکشی شده</returns>
        public M_Learns GetLearns(int Id)
        {
            return new DatabaseContext().Learns.Where(c=>c.Id == Id).FirstOrDefault();
        }

    }
}
