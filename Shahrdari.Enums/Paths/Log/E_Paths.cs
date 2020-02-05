using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Shahrdari.Enums.Paths.Log
{
    /// <summary>
    /// ادرس های موجود
    /// </summary>
    [Serializable]
    public class E_Paths
    {
        /// <summary>
        /// مسیر مربوط به لاگ ها سیستم
        /// </summary>
        public static string SUBMIT_LOG_PATH { get { return @"C:\Javan Mode Logs"; } }

        /// <summary>
        /// مسیر مربوط به خطاها
        /// </summary>
        public static string SUBMIT_ERROR_PATH { get { return SUBMIT_LOG_PATH + @"\Errors"; } }

        /// <summary>
        ///  مسیر مربوط به هشدارها
        /// </summary>
        public static string SUBMIT_WARNING_PATH { get { return SUBMIT_LOG_PATH + @"\Warnings"; } }

        /// <summary>
        /// خطاهای کاربری
        /// </summary>
        public static string SUBMIT_SYSTEM_ERROR_PATH { get { return SUBMIT_LOG_PATH + @"\SystemError"; } }

        /// <summary>
        /// شروع سرویس
        /// </summary>
        public static string SUBMIT_SERVICE_START_PATH { get { return SUBMIT_LOG_PATH + @"\ServiceStart"; } }

        /// <summary>
        /// پایان سرویس
        /// </summary>
        public static string SUBMIT_SERVICE_END_PATH { get { return SUBMIT_LOG_PATH + @"\ServiceEnd"; } }

        /// <summary>
        /// یک لاگ دلخواه
        /// </summary>
        public static string SUBMIT_CUSTOM_LOG_PATH { get { return SUBMIT_LOG_PATH + @"\CustomLog"; } }
    }
}
