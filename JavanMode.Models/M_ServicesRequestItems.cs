using Shahrdari.Enums;
using Shahrdari.Models.DataAnnotaions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.Models
{
    /// <summary>
    /// مدل مربوط به درخواست ها
    /// </summary>
    [Table("ServicesRequestItems")]
    public class M_ServicesRequestItems
    {
        ///<summary>
        /// شناسه یکتا
        ///</summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Id (Primary key)

        ///<summary>
        /// عنوان
        ///</summary>
        [Required(ErrorMessage = "عنوان را وارد کنید")]
        [MaxLength(250, ErrorMessage = "عنوان نمیتواند بیش از 250 کاراکتر باشد")]
        public string Title { get; set; } // Title (length: 250)

        ///<summary>
        /// تاریخ ایجاد
        ///</summary>
        [Required]
        public System.DateTime CreateDate { get; set; } // CreateDate

        ///<summary>
        /// نام تصویر
        ///</summary>
        [Required]
        public string ImageName { get; set; } // ImageName (length: 40)

        /// <summary>
        /// واحد اندازه گیری
        /// </summary>
        [MaxLength(100, ErrorMessage = "واحد اندازه گیری نمیتواند بیش از 100 کاراکتر باشد")]
        [Required(ErrorMessage = "فیلد واحد اندازه گیری اجباری میباشد")]
        public string Unit { get; set; }

        /// <summary>
        /// امتیاز به ازای هر یک واحد
        /// </summary>
        [Required]
        public long ScorePerUnit { get; set; }

        /// <summary>
        /// شناسه درخواست
        /// </summary>
        public int RequestId { get; set; }

        /// <summary>
        /// اضافه کننده کاربر است ؟
        /// </summary>
        public E_PublicCategory.SYSTEM_USER_TYPE UserType { get; set; }

        /// <summary>
        /// مقدار
        /// </summary>
        public double? Value { get; set; }

        /// <summary>
        /// شناسه کاربر
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// لغو شده
        /// </summary>
        public bool IsFailed { get; set; }

        /// <summary>
        /// امتیاز برای هر واحد کاربر
        /// </summary>
        public long? ScorePerUnitDriver { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? CategoryId { get; set; }
    }
}
