using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.Models
{
    /// <summary>
    /// نقش های کاربران
    /// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("PersonelRoles")]
    public class M_PersonelRoles
    {
        public M_PersonelRoles()
        {
            HasFullControl = false;
        }

        /// <summary>
        /// شناسه یکتا
        /// </summary>
        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// عنوان
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "عنوان نقش را وارد کنید")]
        public string Title { get; set; }

        /// <summary>
        /// دسترسی فول دارد ؟
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required]
        public bool HasFullControl { get; set; }
    }
}
