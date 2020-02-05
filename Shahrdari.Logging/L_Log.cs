using Shahrdari.BussinessLayer;
using Shahrdari.Enums.Loging;
using Shahrdari.Enums.Paths.Log;
using Shahrdari.Factory.Log;
using Shahrdari.Models.Loging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.Logging
{
    /// <summary>
    /// ثبت خطاها
    /// </summary>
    public static class L_Log
    {
        /// <summary>
        /// ارسال لاگ
        /// </summary>
        /// <param name="E">مدل لاگ مربوطه</param>
        public static void SubmitLog(M_SystemLog E)
        {
            try
            {
                switch (E.LogType)
                {
                    case E_LogType.ERROR:
                        {
                            submitError(E);
                        }
                        break;
                    case E_LogType.CUSTOM_LOG:
                        {
                            submitCustom(E);
                        }
                        break;
                }
            }
            catch{ }
        }
        
        /// <summary>
        /// ثبت لاگ مربوط به اررور
        /// </summary>
        /// <param name="e">داده های لاگ</param>
        private static void submitError(M_SystemLog e)
        {
            B_Logs bLog = new B_Logs();
            bLog.Add(new Models.M_Logs {
                ColumnNumber = e.Column,
                FileName = e.FileName,
                LineNumber = e.Line,
                LogType = E_LogType.ERROR,
                Message = e.LogMessage,
                MethodName = e.MethodName,
                SystemName = F_SystemTypeFactory.MakeSystemType(e.SystemType),
                SystemType = e.SystemType,
                Time = DateTime.Now
            });
        }


        private static void submitCustom(M_SystemLog e)
        {
            B_Logs bLog = new B_Logs();
            bLog.Add(new Models.M_Logs
            {
                ColumnNumber = e.Column,
                FileName = e.FileName,
                LineNumber = e.Line,
                LogType = E_LogType.CUSTOM_LOG,
                Message = e.LogMessage,
                MethodName = e.MethodName,
                SystemName = F_SystemTypeFactory.MakeSystemType(e.SystemType),
                SystemType = e.SystemType,
                Time = DateTime.Now
            });
        }
    }
}
