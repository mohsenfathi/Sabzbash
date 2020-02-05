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
    [Table("Messages")]
    public class M_Messages
    {
        ///<summary>
        /// شناسه یکتا
        ///</summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// عنوان
        /// </summary>
        [MFRequired(ErrorMessage = "عنوان را وارد کنید")]
        [MFMaxLength(50, ErrorMessage = "عنوان نمیتواند بیش از 50 کاراکتر باشد")]
        public string title { get; set; }

        /// <summary>
        /// پیام
        /// </summary>
        [MFRequired(ErrorMessage = "متن پیام را وارد کنید")]
        [MFMaxLength(200, ErrorMessage = "متن پیام نمیتواند بیش از 200 کاراکتر باشد")]
        public string messages { get; set; }

        /// <summary>
        /// تاریخ ایجاد
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
