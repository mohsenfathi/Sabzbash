using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.Enums
{
    /// <summary>
    /// کدهای خطا
    /// </summary>
    public enum E_ErrorCodes
    {
        /// <summary>
        /// خطایی در سرور وجود دارد
        /// </summary>
        SERVER_ERROR = -1,

        /// <summary>
        /// عملیات موفق
        /// </summary>
        SUCCESS = 0,
        
        /// <summary>
        /// فیدل خالی است
        /// </summary>
        EMPTY_FIELD = 1,
        
        /// <summary>
        /// متن وارد شده برای فیلد طولانی است
        /// </summary>
        MAX_LENGTH = 3,

        /// <summary>
        /// متن وارد شده کمتر از حد مجاز است
        /// </summary>
        MIN_LENGHT = 4,

        /// <summary>
        /// شماره تلفن همراه قبلا در سیستم ثب شده
        /// </summary>
        HAVE_USER_MOBILE_NUMBER = 5,

        /// <summary>
        /// عدم دریافت اطلاعات
        /// </summary>
        NOT_FOUND = 6,

        /// <summary>
        /// فاکتور پرداخت شده قابل ویرایش نمیباشد
        /// </summary>
        PAID_FACTOR_CANT_EDIT = 7,

        /// <summary>
        /// این کد تخفیف قبلا استفاده شده
        /// </summary>
        USED_DISCOUNT_CODE = 8,

        /// <summary>
        /// حداقل جمع کل فاکتور برای استفاده از کد تخفیف کافی نیست
        /// </summary>
        HAVE_NOT_MINIMUM_AMOUNT = 9,

        /// <summary>
        /// کد تخفیف قابل استفاده برای نوع درخواست جاری نیست
        /// </summary>
        NOT_MATCH_DISCOUNT_CODE_AND_SERVICE_CATEGORY = 10,

        /// <summary>
        /// حداقل تعداد درخواست های برای استفاده از این کد تخفیف کافی نیست
        /// </summary>
        INADEQUATE_NUMBER_OF_REQUESTS = 11,

        /// <summary>
        /// نام کاربری یا گذرواژه
        /// </summary>
        USERNAME_OR_PASSWORD = 12,

        /// <summary>
        /// شناسه غلط
        /// </summary>
        WRONG_ID = 13,

        /// <summary>
        /// غلط کد 
        /// </summary>
        WRONG_CODE = 14,

        /// <summary>
        /// مبلغ خالی
        /// </summary>
        EMPTY_AMOUNT = 15,

        /// <summary>
        /// شناسه حساب
        /// </summary>
        ACCOUNT_ID = 16,

        /// <summary>
        /// امتیاز
        /// </summary>
        POINT = 17,

        /// <summary>
        /// قایل
        /// </summary>
        FILE = 18,

        /// <summary>
        /// درخواست پذیرفته شده توسط راننده دیگر
        /// </summary>
        ACCEPTED_BY_ANOTHER_DRIVER = 19
    }
}
