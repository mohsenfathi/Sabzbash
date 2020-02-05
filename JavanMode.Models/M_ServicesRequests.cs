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
    [Table("ServicesRequests")]
    public class M_ServicesRequests
    {
        /// <summary>
        /// سازنده پیشفرض
        /// </summary>
        public M_ServicesRequests()
        {
            CreateDate = DateTime.Now;
            ImmediatelyRequest = false;
            ReferralDate = DateTime.Now;
            IsDeleted = false;
            Status = E_PublicCategory.REQUEST_STATUS.NEW_REQUEST;
            ShowRatingDialog = false;
        }

        ///<summary>
        /// شناسه یکتا جدول
        ///</summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Id (Primary key)

        ///<summary>
        /// شناسه کاربر
        ///</summary>
        public int UserId { get; set; } // UserId

        /// <summary>
        /// شناسه پرسنل
        /// </summary>
        public int? PersonelId { get; set; } // PersonelId

        ///<summary>
        /// تاریخ ایجاد
        ///</summary>
        public System.DateTime CreateDate { get; set; } // CreateDate

        ///<summary>
        /// درخواست آنی
        ///</summary>
        public bool ImmediatelyRequest { get; set; } // ImmediatelyRequest

        ///<summary>
        /// تاریخ و زمان ارجاء
        ///</summary>
        public System.DateTime ReferralDate { get; set; } // ReferralDate

        ///<summary>
        /// توضیحات
        ///</summary>
        public string Description { get; set; } // Description (length: 500)

        ///<summary>
        /// مختصات جغرافیایی
        ///</summary>
        [MFRequired(ErrorMessage = "مختصات جغرافیایی را وارد کنید")]
        [MFMaxLength(50, ErrorMessage = "مختصات جغرافیایی نمیتواند بیش از 50 کاراکتر عددی و نقطه باشد")]
        public string GeographicalCoordinates { get; set; } // GeographicalCoordinates (length: 23)

        ///<summary>
        /// وضعیت درخواست
        ///</summary>
        public E_PublicCategory.REQUEST_STATUS Status { get; set; } // Status

        ///<summary>
        /// شماره پلاک
        ///</summary>
        public string PlaqueNumber { get; set; } // PlaqueNumber (length: 4)

        ///<summary>
        /// شماره واحد
        ///</summary>
        public string UnitNumber { get ; set; } // UnitNumber (length: 4)

        ///<summary>
        /// آدرس
        ///</summary>
        public string Address { get; set; } // Address (length: 255)

        ///<summary>
        /// وضعیت حذف شدگیه درخواست
        ///</summary>
        public bool IsDeleted { get; set; } // IsDeleted

        ///<summary>
        /// تاریخ حذف
        ///</summary>
        public System.DateTime? DeletedDate { get; set; } // DeletedDate

        ///<summary>
        /// تاریخ پایان
        ///</summary>
        public System.DateTime? FinishDate { get; set; } // FinishDate

        ///<summary>
        /// امتیاز
        ///</summary>
        public int? Rate { get; set; } // Rate

        ///<summary>
        /// نظر انجام کار
        ///</summary>
        [MFMaxLength(255, ErrorMessage = "نظر انجام کار نمیتواند بیش از 255 کاراکتر باشد")]
        public string FinishComment { get; set; } // FinishComment (length: 255)

        /// <summary>
        /// تاریخ ویرایش
        /// </summary>
        public DateTime? UpdateDate { get; set; }
        
        /// <summary>
        /// تاریخ ارجا برای انجام کار
        /// </summary>
        public DateTime? ToDoDate { get; set; }

        /// <summary>
        /// دلیل حذف درخواست
        /// </summary>
        public string DeleteDescription { get; set; }

        public int? Pouriya_TimeId { get; set; }
        public int? Pouriya_DateId { get; set; }
        public string Pouriya_Type { get; set; }
        public int? DeleteMessageId { get; set; }
        public bool? IsDeletedUser { get; set; }

        public bool? ShowRatingDialog { get; set; }

        public string FinishMessageId { get; set; }
    }
}
