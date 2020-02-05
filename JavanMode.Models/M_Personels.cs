using Shahrdari.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.Models
{
    /// <summary>
    /// پرسنل
    /// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("Personels")]
    public class M_Personels
    {
        /// <summary>
        /// سازنده پیشفرض
        /// </summary>
        public M_Personels()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            chars = new string(Enumerable.Repeat(chars, 45)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            string key = Guid.NewGuid().ToString();
            key = key.Replace("-", "");
            key = key.Insert(5, chars.Substring(0, 10));
            key = key.Insert(10, chars.Substring(11, 10));
            key = key.Insert(7, chars.Substring(21, 10));
            key = key.Insert(3, chars.Substring(31, 10));
            if (key.Length > 55)
                key = key.Substring(0, 55);
            UnicKey = key;
            ImageName = "Default.jpg";
            RegisterDate = LastOnline = DateTime.Now;
            IsDeleted = false;
        }

        /// <summary>
        /// شناسه یکتا
        /// </summary>
        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// نام
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "نام را وارد کنید")]
        [System.ComponentModel.DataAnnotations.MaxLength(50, ErrorMessage = "حداکثر طول برای نام 50 کاراکتر است")]
        public string FirstName { get; set; }

        /// <summary>
        /// نام خانوادگی
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "نام خانوادگی را وارد کنید")]
        [System.ComponentModel.DataAnnotations.MaxLength(50, ErrorMessage = "حداکثر طول برای نام خانوادگی 50 کاراکتر است")]
        public string LastName { get; set; }

        /// <summary>
        /// نام کاربری
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "نام کاربری را وارد کنید")]
        [System.ComponentModel.DataAnnotations.MaxLength(50, ErrorMessage = "حداکثر طول برای نام کاربری 50 کاراکتر است")]
        public string UserName { get; set; }

        /// <summary>
        /// گذرواژه
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "گذرواژه را وارد کنید")]
        [System.ComponentModel.DataAnnotations.MaxLength(50, ErrorMessage = "حداکثر طول برای گذرواژه 50 کاراکتر است")]
        public string Password { get; set; }

        /// <summary>
        /// جنسیت
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "جنسیت را وارد کنید")]
        public E_PublicCategory.GENDER Gender { get; set; }

        /// <summary>
        /// نام تصویر کاربر
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required]
        public string ImageName { get; set; }

        /// <summary>
        /// تاریخ تولد
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "تاریخ تولد را وارد کنید")]
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// شماره تلفن همراه
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "شماره تلفن همراه را وارد کنید")]
        [System.ComponentModel.DataAnnotations.MaxLength(11, ErrorMessage = "حداکثر طول برای شماره تلفن همراه 11 کاراکتر است")]
        [System.ComponentModel.DataAnnotations.MinLength(11, ErrorMessage = "حداقل طول برای شماره تلفن همراه 11 کاراکتر است ، مثال صحیح : 09121234567")]
        public string MobileNumber { get; set; }

        /// <summary>
        /// تاریخ ثبت نام
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required]
        public DateTime RegisterDate { get; set; }

        /// <summary>
        /// تاریخ آخرین آنلاین
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required]
        public DateTime LastOnline { get; set; }

        /// <summary>
        /// وضعیت فعال بودن کاربر
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "وضعیت فعال بودن کاربر را انتخاب کنید")]
        public bool IsActive { get; set; }

        /// <summary>
        /// وضعیت حذف شدگیه کاربر
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "وضعیت حذف شدگیه کاربر را انتخاب کنید")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// تاریخ غیر فعالسازی کاربر
        /// </summary>
        public DateTime? DeactiveDate { get; set; }

        /// <summary>
        /// تاریخ حذف
        /// </summary>
        public DateTime? DeletedDate { get; set; }

        /// <summary>
        /// نقش کاربر
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "نقش کاربر را وارد کنید")]
        public int PersonelRoleId { get; set; }

        /// <summary>
        /// کد یکتا کاربر
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required]
        public string UnicKey { get; set; }

        /// <summary>
        /// نوع کاربر
        /// </summary>
        public E_PublicCategory.PERSONEL_TYPE PersonelType { get; set; }
        
        /// <summary>
        /// نوع وسیله نقلیه
        /// </summary>
        public E_PublicCategory.VEHICLE_TYPE? VehicleType { get; set; }

        /// <summary>
        /// پلاک وسیله نقلیه
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(8, ErrorMessage = "حداکثر طول برای پلاک وسیله نقلیه 8 کاراکتر است")]
        public string VehiclePlaq { get; set; }

        /// <summary>
        /// توضیحات وسیله نقلیه
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(100, ErrorMessage = "حداکثر طول برای توضیحات وسیله نقلیه 100 کاراکتر است")]
        public string VehicleDesc { get; set; }

        /// <summary>
        /// ادرس مرکز تجمیع
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(100, ErrorMessage = "حداکثر طول برای آدرس مرکز تجمیع 100 کاراکتر است")]
        public string SumCenterAddress { get; set; }

        /// <summary>
        /// تلفن مرکز تجمیع
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(11, ErrorMessage = "حداکثر طول برای تلفن مرکز تجمیع 11 کاراکتر است")]
        public string SumCenterTell { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsVerify { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ServiceRun { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ReagentCode { get; set; }
    }
}
