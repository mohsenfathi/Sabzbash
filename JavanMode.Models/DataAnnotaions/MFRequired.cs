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
    public class MFRequired : RequiredAttribute
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
        public MFRequired() : base()
        {
            ErrorCode = E_ErrorCodes.EMPTY_FIELD;
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
