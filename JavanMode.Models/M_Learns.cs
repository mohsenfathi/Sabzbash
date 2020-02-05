using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.Models
{
    /// <summary>
    /// آموزش
    /// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("Learns")]
    public class M_Learns
    {
        /// <summary>
        /// شناسه جدول
        /// </summary>
        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// عنوان
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "عنوان را وارد کنید")]
        [System.ComponentModel.DataAnnotations.MaxLength(100, ErrorMessage = "حداکثر طول برای عنوان 100 کاراکتر است")]
        public string Title { get; set; }

        /// <summary>
        /// توضیحات کوتاه
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "توضیحات کوتاه را وارد کنید")]
        [System.ComponentModel.DataAnnotations.MaxLength(500, ErrorMessage = "حداکثر طول برای توضیحات کوتاه 500 کاراکتر است")]
        public string ShortDesc { get; set; }

        /// <summary>
        /// توضیحات کامل
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "توضیحات کامل را وارد کنید")]
        public string LongDesc { get; set; }

        /// <summary>
        /// ویدئو
        /// </summary>
        [System.ComponentModel.DataAnnotations.MaxLength(200, ErrorMessage = "نام ویدئو طولانی است")]
        public string Video { get; set; }

        /// <summary>
        /// مجموع رتبه
        /// </summary>
        public int? RateSum { get; set; }

        /// <summary>
        /// تعداد رتبه
        /// </summary>
        public int? RateCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CoverImage { get; set; }
    }
}
