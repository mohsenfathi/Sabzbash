using Shahrdari.Enums.Loging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shahrdari.Factory.Log
{
    /// <summary>
    /// ساختن اکسپشن
    /// </summary>
    public static class F_ExeptionFactory
    {
        /// <summary>
        /// ساخت اکپشن
        /// </summary>
        /// <param name="msg">متن پیغام</param>
        /// <param name="logType">نوع لاگ</param>
        /// <returns>اکسپشن ساخته شده</returns>
        public static Exception MakeExeption(string msg, E_LogType logType)
        {
            Exception ex = new Exception(msg);
            ex.Source = logType.ToString();
            return ex;
        }

        /// <summary>
        /// ساخت اکپشن
        /// </summary>
        /// <param name="msg">متن پیغام</param>
        /// <param name="code">کد خطا</param>
        /// <param name="logType">نوع لاگ</param>
        /// <returns>اکسپشن ساخته شده</returns>
        public static Exception MakeExeption(string msg, string code, E_LogType logType)
        {
            Exception ex = new Exception(msg);
            ex.HelpLink = code;
            ex.Source = logType.ToString();
            return ex;
        }
    }
}
