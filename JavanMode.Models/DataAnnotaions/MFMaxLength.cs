using Shahrdari.Enums;
using Shahrdari.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.Models.DataAnnotaions
{
    public class MFMaxLength : MaxLengthAttribute
    {
        /// <summary>
        /// کد خطا
        /// </summary>
        public E_ErrorCodes ErrorCode { get; set; }

        private string _orgErrorMessage = "";

        /// <summary>
        /// سازنده پیشفرض
        /// </summary>
        /// <param name="errorCode">کد خطا</param>
        public MFMaxLength(int lenght) : base(lenght)
        {
            ErrorCode = E_ErrorCodes.MAX_LENGTH;
        }

        public override bool IsValid(object value)
        {
            if (string.IsNullOrEmpty(_orgErrorMessage))
                _orgErrorMessage = ErrorMessage;
            ErrorMessage = _orgErrorMessage + S_Seprators.ErrorMessageSeprator.ToString() + ((int)ErrorCode);
            return base.IsValid(value);
        }
    }
}
