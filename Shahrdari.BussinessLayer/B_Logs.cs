using Shahrdari.DataAccessLayer;
using Shahrdari.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.BussinessLayer
{
    /// <summary>
    /// بیزینس لاگ های سیستم
    /// </summary>
    public class B_Logs
    {
        /// <summary>
        /// افزودن لاگ
        /// </summary>
        /// <param name="Log">کدل لاگ</param>
        /// <returns>لاگ اضافه شده</returns>
        public M_Logs Add(M_Logs Log)
        {
            if (string.IsNullOrEmpty(Log.FileName))
                Log.FileName = "بدون مقدار";
            if (string.IsNullOrEmpty(Log.Message))
                Log.Message = "بدون مقدار";
            if (string.IsNullOrEmpty(Log.MethodName))
                Log.MethodName = "بدون مقدار";
            if (string.IsNullOrEmpty(Log.SystemName))
                Log.SystemName = "بدون مقدار";
            DatabaseContext db = new DatabaseContext();
            db.Logs.Add(Log);
            db.SaveChanges();
            return Log;
        }
    }
}
