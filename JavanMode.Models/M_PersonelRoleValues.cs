using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.Models
{
    /// <summary>
    /// مقادیر نقش های کاربر
    /// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("PersonelRoleValues")]
    public class M_PersonelRoleValues
    {
        /// <summary>
        /// شناسه یکتا
        /// </summary>
        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// عنوان اکشن و کنترلر
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "نام دسترسی معتبر نیست")]
        public string AccessName { get; set; }

        /// <summary>
        /// شناسه نقش
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "نقش مشخص نیست")]
        public int PersonelRoleId { get; set; }
    }
}
