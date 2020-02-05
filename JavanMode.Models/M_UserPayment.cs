using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shahrdari.Enums;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.Models
{
    [Table("UserPayment")]
    public class M_UserPayment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; set; }
        public long Point { get; set; }
        public int AccountId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public E_PublicCategory.PAYMENT_STATUS Status { get; set; }
        public string Message { get; set; }
        public E_PublicCategory.PAYMENT_TYPE Type { get; set; }
        public bool IsDeleted { get; set; }
        public E_PublicCategory.SYSTEM_USER_TYPE UserType { get; set; }
    }
}
