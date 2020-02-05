using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Shahrdari.Enums.Loging
{
    /// <summary>
    /// نوع ارور های پیش آمده
    /// </summary>
    [Serializable]
    public enum E_LogType
    {
        /// <summary>
        /// تذکر
        /// </summary>
        WARNNING = 1,

        /// <summary>
        /// خطا یا باگ در برنامه
        /// </summary>
        ERROR = 2,

        /// <summary>
        /// خطاهای کاربر
        /// </summary>
        SYSTEM_ERROR = 3,

        /// <summary>
        /// عملیات موفق
        /// </summary>
        SUCCESS = 4,

        /// <summary>
        /// اجرا شدن برنامه
        /// </summary>
        APPLICATION_START = 5,

        /// <summary>
        /// بستن برنامه
        /// </summary>
        APPLICATION_END = 6,

        /// <summary>
        /// اجرا شدن یک تابع
        /// </summary>
        FUNCTION_START = 7,

        /// <summary>
        /// تمام شدن یک تابع
        /// </summary>
        FUNCTION_END = 8,

        /// <summary>
        /// اجرا شدن سرویس
        /// </summary>
        SERVICE_START = 9,

        /// <summary>
        /// بستن سرویس
        /// </summary>
        SERVICE_END = 10,

        /// <summary>
        /// یک لاگ با متن دلخواه
        /// </summary>
        CUSTOM_LOG = 11,
    }
}
