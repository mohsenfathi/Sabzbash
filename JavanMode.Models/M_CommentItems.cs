using Shahrdari.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.Models
{
    /// <summary>
    /// 
    /// </summary>
    [Table("CommentItems")]
    public class M_CommentItems
    {
        /// <summary>
        /// شناسه یکتا
        /// </summary>
        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// شناسه بازخورد
        /// </summary>
        public E_PublicCategory.FEEDBACK CommentItemId { get; set; }

        /// <summary>
        /// شناسه درخواست
        /// </summary>
        public int RequestId { get; set; }
    }
}
