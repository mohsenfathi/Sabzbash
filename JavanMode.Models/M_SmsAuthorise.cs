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
    /// مدل مربوط به کدهای فعالسازی
    /// </summary>
    [Table("SmsAuthorise")]
    public class M_SmsAuthorise
    {
        /// <summary>
        /// شناسه جدول
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// شماره تلفن
        /// </summary>
        public String PhoneNumber { get; set; }

        /// <summary>
        /// کد فعالسازی
        /// </summary>
        public int Code { get; set; }
    }
}
