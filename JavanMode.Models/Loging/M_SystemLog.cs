using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shahrdari.Enums.Loging;
using System.Diagnostics;
using System.Reflection;

namespace Shahrdari.Models.Loging
{
    /// <summary>
    /// مدل مربوط به خطاها
    /// </summary>
    public class M_SystemLog : Exception
    {
        /// <summary>
        /// متد سازنده برای لاگ های رزرو شده مثل شروع یک عملیات
        /// </summary>
        /// <param name="SysTp">نوع سیستم</param>
        /// <param name="LogTp">نوع لاگ</param>
        public M_SystemLog(E_SystemType SysTp, E_LogType LogTp)
        {
            SystemType = SysTp;
            LogType = LogTp;
        }

        /// <summary>
        /// متد سازنده برای لاگ های خطا
        /// </summary>
        /// <param name="SysTp">نوع سیستم</param>
        /// <param name="LogTp">نوع خطا</param>
        /// <param name="Ex">مدل خطا</param>
        public M_SystemLog(E_SystemType SysTp, E_LogType LogTp, Exception Ex)
        {
            SystemType = SysTp;
            LogType = LogTp;
            LogMessage = Ex.Message.Contains("An error occurred while updating the entries. See the inner exception for details") ? Ex.InnerException.InnerException.Message : Ex.Message;
            StackTrace st = new StackTrace(Ex, true);
            StackFrame frame = st.GetFrame(0);
            MethodBase site = Ex.TargetSite;
            FileName = frame.GetFileName();
            MethodName = frame.GetMethod().Name;
            Line = frame.GetFileLineNumber();
            Column = frame.GetFileColumnNumber();
        }

        /// <summary>
        /// متد سازنده برای لاگ های دلخواه
        /// </summary>
        /// <param name="SysTp">نوع سیستم</param>
        /// <param name="LogTp">نوع لاگ</param>
        /// <param name="Message">متن لاگ</param>
        public M_SystemLog(E_SystemType SysTp, E_LogType LogTp, string Message)
        {
            SystemType = SysTp;
            LogType = LogTp;
            LogMessage = Message;
        }

        /// <summary>
        /// نوع خطا
        /// </summary>
        public E_LogType LogType { get; set; }

        /// <summary>
        /// متن خطای مورد نظر
        /// </summary>
        public string LogMessage { get; set; }

        /// <summary>
        /// سیستمی که درخواست لاگ کرده
        /// </summary>
        public E_SystemType SystemType { get; set; }

        /// <summary>
        /// نام فایل
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// نام متد
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// شماره خط
        /// </summary>
        public int Line { get; set; }

        /// <summary>
        /// شماره ستون
        /// </summary>
        public int Column { get; set; }
    }
}
