using Shahrdari.Factory.Log;
using Shahrdari.Models;
using Shahrdari.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.BussinessLayer
{
    /// <summary>
    /// بیزینس پایه
    /// </summary>
    public class B_Base
    {
        /// <summary>
        /// ظرفیت صفحات
        /// </summary>
        protected int PageCapacity { get { return 30; } }

        /// <summary>
        /// اعتبار سنجی مدل
        /// </summary>
        /// <typeparam name="T">نوع مدل</typeparam>
        /// <param name="Data">داده</param>
        public virtual void Validate<T>(T Data)
        {
            M_ValidationResult ValidationResult = Data.ValidateModel();
            if (!ValidationResult.IsValid)
            {
                var errorContent = ValidationResult.Errors.First().Split(S_Seprators.ErrorMessageSeprator).ToList();
                if (errorContent.Count == 1)
                    throw F_ExeptionFactory.MakeExeption(errorContent[0], Enums.Loging.E_LogType.SYSTEM_ERROR);
                throw F_ExeptionFactory.MakeExeption(errorContent[0], errorContent[1] , Enums.Loging.E_LogType.SYSTEM_ERROR);
            }
        }
    }
}
