using Shahrdari.Enums;
using Shahrdari.Settings;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shahrdari.Models.DataAnnotaions
{
    public class MFMinLength : MinLengthAttribute
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
        public MFMinLength(int lenght) : base(lenght)
        {
            ErrorCode = E_ErrorCodes.MIN_LENGHT;
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