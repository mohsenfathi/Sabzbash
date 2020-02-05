
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
    [Table("News")]
    public class M_News
    {
        public M_News()
        {
            CreationDate = DateTime.Now;
            Image = "Default.jpg";
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MFRequired(ErrorMessage = "عنوان را وارد کنید")]
        [MFMaxLength(100, ErrorMessage = "عنوان نمیتواند بیش از 100 کاراکتر باشد")]
        public string Title { get; set; }

        [MFRequired(ErrorMessage = "چکیده خبر را وارد کنید")]
        [MFMaxLength(300, ErrorMessage = "چکیده خبر نمیتواند بیش از 300 کاراکتر باشد")]
        public string Summery { get; set; }

        [MFRequired(ErrorMessage = "خبر را وارد کنید")]
        public string Description { get; set; }

        public string Image { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
