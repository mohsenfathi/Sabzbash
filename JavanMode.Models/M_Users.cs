using Shahrdari.Enums;
using Shahrdari.Models.DataAnnotaions;
using Shahrdari.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.Models
{
    /// <summary>
    /// کاربران سیستم
    /// </summary>
    [Table("Users")]
    public class M_Users
    {
        public M_Users()
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
            IsActive = true;
            IsDeleted = false;
        }

        ///<summary>
        /// شناسه یکتا
        ///</summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Id (Primary key)

        ///<summary>
        /// نام
        ///</summary>
        [MFRequired(ErrorMessage = "نام را وارد کنید")]
        [MFMaxLength(50, ErrorMessage = "نام نمیتواند بیش از 50 کاراکتر باشد")]
        public string FirstName { get; set; } // FirstName (length: 50)

        ///<summary>
        /// نام خانوادگی
        ///</summary>
        [MFRequired(ErrorMessage = "نام خانوادگی را وارد کنید")]
        [MFMaxLength(50, ErrorMessage = "نام خانوادگی نمیتواند بیش از 50 کاراکتر باشد")]
        public string LastName { get; set; } // LastName (length: 50)

        ///<summary>
        /// جنسیت
        ///</summary>
        public E_PublicCategory.GENDER? Gender { get; set; } // Gender

        ///<summary>
        /// نام تصویر کاربر
        ///</summary>
        public string ImageName { get; set; } // ImageName (length: 40)

        ///<summary>
        /// تاریخ تولد
        ///</summary>
        public DateTime? BirthDate { get; set; } // BirthDate

        ///<summary>
        /// شماره تلفن همراه
        ///</summary>
        [MFRequired(ErrorMessage = "شماره تلفن همراه را وارد کنید")]
        [MFMaxLength(11, ErrorMessage = "شماره تلفن همراه نمیتواند بیش از 11 کاراکتر باشد")]
        [MFMinLength(11, ErrorMessage = "شماره تلفن همراه نمیتواند کمتر از از 11 کاراکتر باشد")]
        public string MobileNumber { get; set; } // MobileNumber (length: 11)

        /// <summary>
        /// پست الکترونیکی
        /// </summary>
        [MFMaxLength(50, ErrorMessage = "پست الکترونیکی نمیتواند بیش از 50 کاراکتر باشد")]
        public string Email { get; set; }

        ///<summary>
        /// تاریخ ثبت نام
        ///</summary>
        [Required(ErrorMessage = "تاریخ ثبت نام را وارد کنید")]
        public System.DateTime RegisterDate { get; set; } // RegisterDate

        ///<summary>
        /// تاریخ آخرین آنلاین
        ///</summary>
        [Required(ErrorMessage = "تاریخ آخرین آنلاین را وارد کنید")]
        public System.DateTime LastOnline { get; set; } // LastOnline

        ///<summary>
        /// وضعیت فعال بودن کاربر
        ///</summary>
        public bool IsActive { get; set; } // IsActive

        ///<summary>
        /// وضعیت حذف شدگیه کاربر
        ///</summary>
        public bool IsDeleted { get; set; } // IsDeleted

        ///<summary>
        /// تاریخ غیر فعالسازی کاربر
        ///</summary>
        public System.DateTime? DeactiveDate { get; set; } // DeactiveDate

        ///<summary>
        /// تاریخ حذف
        ///</summary>
        public System.DateTime? DeletedDate { get; set; } // DeletedDate

        /// <summary>
        /// کد یکتا کاربر
        /// </summary>
        [Required]
        public string UnicKey { get; set; }

        /// <summary>
        /// نوع کاربر
        /// </summary>
        public E_PublicCategory.USER_TYPE UserType { get; set; }

        /// <summary>
        /// نام موسسه
        /// </summary>
        [MFMaxLength(50, ErrorMessage = "نام موسسه نمیتواند بیش از 50 کاراکتر باشد")]
        public string InstituteName { get; set; }

        /// <summary>
        /// نوع موسسه
        /// </summary>
        public E_PublicCategory.INSTITUTE_TYPE InstituteType { get; set; }

        /// <summary>
        /// کد پستی
        /// </summary>
        [MFMaxLength(10, ErrorMessage = "کد پستی نمیتواند بیش از 10 کاراکتر باشد")]
        public string PostalCode { get; set; }

        /// <summary>
        /// کد پستی
        /// </summary>
        [MFMaxLength(11, ErrorMessage = "تلفن ثابت نمیتواند بیش از 11 کاراکتر باشد")]
        public string Tell { get; set; }

        /// <summary>
        /// شناسه کاربر دعوت کننده
        /// </summary>
        public int? InviteUserId { get; set; }

        /// <summary>
        /// گذرواژه
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// کد معرف
        /// </summary>
        public int ReagentCode { get; set; }

        /// <summary>
        /// شناسه معرف
        /// </summary>
        public int? ReagentUserId { get; set; }
    }
}
