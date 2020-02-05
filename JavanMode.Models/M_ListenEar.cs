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
    [Table("ListenEar")]
    public class M_ListenEar
    {
        ///<summary>
        /// شناسه یکتا
        ///</summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Id (Primary key)

        /// <summary>
        /// شناسه کاربر
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        [MFRequired(ErrorMessage = "توضیحات را وارد کنید")]
        public string Description { get; set; }

        /// <summary>
        /// خوانده شده
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// تاریخ ایجاد
        /// </summary>
        public DateTime CreationDate { get; set; }
    }
}
