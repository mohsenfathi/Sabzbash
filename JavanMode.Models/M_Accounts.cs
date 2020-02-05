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
    [Table("Accounts")]
    public class M_Accounts
    {
        /// <summary>
        /// شناسه جدول
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        public bool IsDelete { get; set; }

        [MFRequired(ErrorMessage = "شماره شبا را وارد کنید")]
        [MFMaxLength(50, ErrorMessage = "شماره شبا نمیتواند بیش از 50 کاراکتر باشد")]
        public string ShabaNumber { get; set; }

        [MFRequired(ErrorMessage = "نام دارنده حساب را وارد کنید")]
        [MFMaxLength(100, ErrorMessage = "نام دارنده حساب نمیتواند بیش از 100 کاراکتر باشد")]
        public string AccountHolderName { get; set; }

        [MFRequired(ErrorMessage = "نام بانک را وارد کنید")]
        [MFMaxLength(100, ErrorMessage = "نام بانک نمیتواند بیش از 100 کاراکتر باشد")]
        public string BankName { get; set; }
    }
}
