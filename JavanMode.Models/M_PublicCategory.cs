using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.Models
{
    /// <summary>
    /// مقادیر ثابت
    /// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("PublicCategory")]
    public class M_PublicCategory
    {
        /// <summary>
        /// شناسه یکتا
        /// </summary>
        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// عنوان
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required]
        public string Title { get; set; }

        /// <summary>
        /// شناسه والد
        /// </summary>
        [System.ComponentModel.DataAnnotations.Required]
        public int ParentId { get; set; }
    }
}
