using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.Enums
{
    /// <summary>
    /// مقادیر مربوط به لوکاپ ها
    /// </summary>
    public static class E_PublicCategory
    {
        /// <summary>
        /// پدرهای لوکاپ ها
        /// </summary>
        public enum PUBLIC_CATEGORY_PARENT
        {
            /// <summary>
            /// جنسیت
            /// </summary>
            GENDER = 1,

            /// <summary>
            /// وضعیت درخواست
            /// </summary>
            REQUEST_STATUS = 1005,

            /// <summary>
            /// نوع کاربر سیستم
            /// </summary>
            SYSTEM_USER_TYPE = 1018,

            /// <summary>
            /// نوع پرسنل
            /// </summary>
            PERSONEL_TYPE = 2020,

            /// <summary>
            /// نوع وسیله نقلیه
            /// </summary>
            VEHICLE_TYPE = 2024,

            /// <summary>
            /// نوع کاربر
            /// </summary>
            USER_TYPE = 2030,

            /// <summary>
            /// نوع موسسه
            /// </summary>
            INSTITUTE_TYPE = 2034,

            /// <summary>
            /// ساعت سرویس دهی
            /// </summary>
            SERVICE_TIME = 2039,

            /// <summary>
            /// بازخورد
            /// </summary>
            FEEDBACK = 2045,

            /// <summary>
            /// نوع پرداخت
            /// </summary>
            PAYMENT_TYPE = 2061,

            /// <summary>
            /// وضعیت پرداخت
            /// </summary>
            PAYMENT_STATUS = 2056,

            /// <summary>
            /// رنگ پلاک
            /// </summary>
            TAG_COLOR = 2070,

            /// <summary>
            /// نوع خودرو
            /// </summary>
            CAR_TYPE = 2074,
        }

        /// <summary>
        /// جنسیت
        /// </summary>
        public enum GENDER
        {

            /// <summary>
            /// آقا
            /// </summary>
            MAN = 2,

            /// <summary>
            /// خانم
            /// </summary>
            WOMAN = 3,
        }
        
        /// <summary>
        /// نوع کاربر سیستم
        /// </summary>
        public enum SYSTEM_USER_TYPE
        {
            /// <summary>
            /// پرسنل
            /// </summary>
            PERSONEL = 1019,

            /// <summary>
            /// کاربر معمولی
            /// </summary>
            NORMAL_USER = 1020
        }

        /// <summary>
        /// نوع پرسنل
        /// </summary>
        public enum PERSONEL_TYPE
        {
            /// <summary>
            /// فقط کاربر پرتال
            /// </summary>
            ONLY_PORTAL_USER = 2021,

            /// <summary>
            /// راننده
            /// </summary>
            DRIVER = 2022,

            /// <summary>
            /// مرکز تجمیع
            /// </summary>
            INTEGRATION_CENTER = 2023,

            /// <summary>
            /// غرفه
            /// </summary>
            SUM_CENER = 2069
        }

        /// <summary>
        /// نوع وسیله نقلیه
        /// </summary>
        public enum VEHICLE_TYPE
        {
            /// <summary>
            /// موتو و سه چرخ
            /// </summary>
            MOTOR = 2025,

            /// <summary>
            /// کامیونت
            /// </summary>
            TRUCK = 2026,

            /// <summary>
            /// وانت
            /// </summary>
            PICKUP = 2027,

            /// <summary>
            /// غرفه های ثابط
            /// </summary>
            STABLE_BOOTHS = 2028,
        }
        
        /// <summary>
        /// نوع کاربر
        /// </summary>
        public enum USER_TYPE
        {
            /// <summary>
            /// خانه / مغازه
            /// </summary>
            HOME_STORE = 2031 ,
            
            /// <summary>
            /// مجتمع مسکونی , اداری یا تجاری / کاخانه / اداره
            /// </summary>
            RESIDENTIAL_OFFICE_OR_COMMERCIAL = 2032
        }

        /// <summary>
        /// نوع موسسه
        /// </summary>
        public enum INSTITUTE_TYPE
        {
            /// <summary>
            /// اداری
            /// </summary>
            OFFICIAL = 2035,

            /// <summary>
            /// تجاری
            /// </summary>
            COMMERCIAL = 2036,

            /// <summary>
            /// کارخانه
            /// </summary>
            FACTORY = 2037,

            /// <summary>
            /// خانه
            /// </summary>
            HOME = 2038,

            /// <summary>
            /// آدرس منتخب
            /// </summary>
            FAVORITE = 2068,
        }

        /// <summary>
        /// وضعیت درخواست
        /// </summary>
        public enum REQUEST_STATUS
        {
            /// <summary>
            /// درخواست جدید
            /// </summary>
            NEW_REQUEST = 1006,

            /// <summary>
            /// تایید و ارجا داده شده برای انجام
            /// </summary>
            APPROVED_AND_REFERENCED = 1007,

            /// <summary>
            /// متخصص آماده حرکت به محل
            /// </summary>
            EXPERT_READY_TO_GO = 2019,

            /// <summary>
            /// در اتظار دریافت
            /// </summary>
            WAIT_FOR_GET = 2078,

            /// <summary>
            /// در حال انجام کار
            /// </summary>
            EXPERT_ON_PLACE = 2018,

            /// <summary>
            /// اتمام درخواست از طرف پرسنل
            /// </summary>
            WAIT_FOR_END_REQUEST_ACCEPT = 2079,

            /// <summary>
            /// تایید پایان درخواست کاربر
            /// </summary>
            REQUEST_FINISHED_FROM_USER = 2080,

            /// <summary>
            /// بسته شده
            /// </summary>
            CLOSED = 1008,

            /// <summary>
            /// لغو شده
            /// </summary>
            CANCEL = 2081
        }

        /// <summary>
        /// ساعت سرویس دهی
        /// </summary>
        public enum SERVICE_TIME
        {
            /// <summary>
            /// 8 تا 11
            /// </summary>
            Time_8_11 = 2040,

            /// <summary>
            /// 11 تا 14
            /// </summary>
            Time_11_14 = 2041,

            /// <summary>
            /// 14 تا 17
            /// </summary>
            Time_14_17 = 2042,

            /// <summary>
            /// 17 تا 20
            /// </summary>
            Time_17_20 = 2043,

            /// <summary>
            /// 20 تا 22
            /// </summary>
            Time_20_22 = 2044
        }

        /// <summary>
        /// بازخورد
        /// </summary>
        public enum FEEDBACK
        {
            /// <summary>
            /// رفتار نا محترمانه
            /// </summary>
            DISRESPECTFUL_BEHAVIOR = 2046,

            /// <summary>
            /// دیر رسیذن به محل شما
            /// </summary>
            GET_TO_YOUR_PLACE_LATE = 2048,

            /// <summary>
            /// دلایل شخصی
            /// </summary>
            PERSONAL_REASONS = 2054,

            /// <summary>
            /// رفتار محترمانه 
            /// </summary>
            RESPECTFUL_BEHAVIOR = 2050,

            /// <summary>
            /// به موقع رسیدن
            /// </summary>
            ARRIVE_ON_TIME = 2051,

            /// <summary>
            /// سرعت در سرویس دهی
            /// </summary>
            SPEED_​​OF_SERVICE = 2052
        }

        /// <summary>
        /// نوع پرداخت
        /// </summary>
        public enum PAYMENT_TYPE
        {
            /// <summary>
            /// نیکوکاری
            /// </summary>
            CHARITY = 2062,

            /// <summary>
            /// کمک به محیط زیست
            /// </summary>
            HELPING_ENVIROMENT = 2063,

            /// <summary>
            /// کپن تخفیف
            /// </summary>
            DISCOUNT_COUPONS = 2064,

            /// <summary>
            /// برداشت نقدی
            /// </summary>
            CASH_WITHDRAWAL = 2065,

            /// <summary>
            /// شارژ سیم کارت
            /// </summary>
            SIMCARD_CHARGE = 2066,

            /// <summary>
            /// محصولات
            /// </summary>
            PRODUCTS = 2067
        }

        /// <summary>
        /// وضعیت پرداخت
        /// </summary>
        public enum PAYMENT_STATUS
        {
            /// <summary>
            /// لغو شده
            /// </summary>
            FAILED = 2057,

            /// <summary>
            /// انجام شده
            /// </summary>
            SUCCESS = 2058,

            /// <summary>
            /// در حال بررسی
            /// </summary>
            PENDING = 2059,

            /// <summary>
            /// درخواست جدید
            /// </summary>
            NEW = 2060
        }

        /// <summary>
        /// رنگ پلاک
        /// </summary>
        public enum TAG_COLOR
        {
            /// <summary>
            /// سفید
            /// </summary>
            WHITE = 2071,

            /// <summary>
            /// زرد
            /// </summary>
            YELLOW = 2072,

            /// <summary>
            /// قرمز
            /// </summary>
            RED = 2073,
        }

        public enum CAR_TYPE
        {
            /// <summary>
            /// سه چرخ
            /// </summary>
            THREE_WHEELS = 2075,

            /// <summary>
            /// کامیونت
            /// </summary>
            TRUCK = 2076,

            /// <summary>
            /// وانت
            /// </summary>
            PICKUP = 2077
        }
    }
}
