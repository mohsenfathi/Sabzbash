using Shahrdari.Enums.Loging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.Models
{
    /// <summary>
    /// لاگ های سیستم
    /// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("Logs")]
    public class M_Logs
    {
        public M_Logs()
        {
            Time = DateTime.Now;
            FileName = "None";
            MethodName = "None";
            LineNumber = 0;
            ColumnNumber = 0;
            Seen = false;
        }

        /// <summary>
        /// شناسه یکتا
        /// </summary>
        [System.ComponentModel.DataAnnotations.Key]
        public int Id { get; set; }

        /// <summary>
        /// نوع لاگ
        /// </summary>
        public E_LogType LogType { get; set; }

        /// <summary>
        /// متن لاگ
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// تاریخ ایجاد لاگ
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// نام فایل
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// نام متد
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// شماره سطر
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        /// شماره ستون
        /// </summary>
        public int ColumnNumber { get; set; }

        /// <summary>
        /// نام سیستم
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// مشاهده شده
        /// </summary>
        public bool Seen { get; set; }

        /// <summary>
        /// نوع سیستم
        /// </summary>
        public E_SystemType SystemType { get; set; }
    }
}
