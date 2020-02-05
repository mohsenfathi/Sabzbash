using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shahrdari.Enums.Loging;

namespace Shahrdari.Factory.Log
{
    /// <summary>
    /// کارخانه ساخت نوع سیستم
    /// </summary>
    public static class F_SystemTypeFactory
    {
        /// <summary>
        /// ساخت سیستم
        /// </summary>
        /// <param name="SysType">نوع سیستم</param>
        /// <returns>نام سیستم ساخته شده</returns>
        public static string MakeSystemType(E_SystemType SysType)
        {
            if (SysType == E_SystemType.SHAHRDARI_WEB_APPLICATION)
                return "Shahrdari Web Application";
            return "";
        }
    }
}
