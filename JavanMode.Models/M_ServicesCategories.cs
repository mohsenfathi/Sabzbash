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
    /// دسته بندی ها
    /// </summary>
    [Table("ServicesCategories")]
    public class M_ServicesCategories
    {
        /// <summary>
        /// سازنده پیشفرض
        /// </summary>
        public M_ServicesCategories()
        {
            CreateDate = DateTime.Now;
            IsActive = HaveNew = true;
            ParentId = 0;
            ImageName = "Default.png";
        }

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
        /// وضعیت فعال بودن
        ///</summary>
        [Required]
        public bool IsActive { get; set; } // IsActive

        ///<summary>
        /// تاریخ غیر فعال شدن
        ///</summary>
        public System.DateTime? DeactiveDate { get; set; } // DeactiveDate

        ///<summary>
        /// شناسه پدر
        ///</summary>
        [Required]
        public int ParentId { get; set; } // ParentId

        ///<summary>
        /// ساحتار درختی
        ///</summary>
        public string Lineages { get; set; } // Lineages (length: 100)

        ///<summary>
        /// نام تصویر
        ///</summary>
        [Required]
        public string ImageName { get; set; } // ImageName (length: 40)

        ///<summary>
        /// توضیحات
        ///</summary>
        [MaxLength(250, ErrorMessage = "توضیحات نمیتواند بیش از 500 کاراکتر باشد")]
        public string Description { get; set; } // Description (length: 500)

        /// <summary>
        /// برچسب جدید داشته باشد ؟
        /// </summary>
        [Required]
        public bool HaveNew { get; set; }

        /// <summary>
        /// مسیر
        /// </summary>
        public string Route { get; set; }

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
        /// امتیاز برای هر واحد کاربر
        /// </summary>
        public long? ScorePerUnitDriver { get; set; }
    }
}
