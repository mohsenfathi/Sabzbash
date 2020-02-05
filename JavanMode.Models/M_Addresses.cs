using Shahrdari.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.Models
{
    /// <summary>
    /// مدل مربوط به آدرس ها
    /// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("Addresses")]
    public class M_Addresses
    {
        /// <summary>
        /// شناسه جدول
        /// </summary>
        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// شناسه کاربر
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// موقعیت جغرافیایی lat
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "مختصات جغرافیا را وارد کنید")]
        [System.ComponentModel.DataAnnotations.MaxLength(30, ErrorMessage = "حداکثر طول برای lat 30 کاراکتر است")]
        public string Lat { get; set; }

        /// <summary>
        /// موقعیت جغرافیایی lat
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "مختصات جغرافیا را وارد کنید")]
        [System.ComponentModel.DataAnnotations.MaxLength(30, ErrorMessage = "حداکثر طول برای Lng 30 کاراکتر است")]
        public string Lng { get; set; }

        /// <summary>
        /// عنوان
        /// </summary>
        public E_PublicCategory.INSTITUTE_TYPE Type { get; set; }

        /// <summary>
        /// آدرس
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "آدرس را وارد کنید")]
        [System.ComponentModel.DataAnnotations.MaxLength(200, ErrorMessage = "حداکثر طول برای آدرس 200 کاراکتر است")]
        public string Address { get; set; }

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// پلاک
        /// </summary>
        public string Plaque { get; set; }

        /// <summary>
        /// واحد
        /// </summary>
        public int? Unit { get; set; }

        /// <summary>
        /// آدرس منتخب
        /// </summary>
        public bool? IsFavorite { get; set; }
    }
}
