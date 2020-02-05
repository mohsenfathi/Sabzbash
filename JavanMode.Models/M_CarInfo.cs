using Shahrdari.Enums;
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
    /// <summary>
    /// مشخصات خودرو
    /// </summary>
    [Table("CarInfo")]
    public class M_CarInfo
    {
        ///<summary>
        /// شناسه یکتا
        ///</summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// مدل خودرو
        /// </summary>
        [MFRequired(ErrorMessage = "نام ماشین را وارد کنید")]
        [MFMaxLength(50, ErrorMessage = "نام ماشین نمیتواند بیش از 50 کاراکتر باشد")]
        public string Name { get; set; }
        
        /// <summary>
        /// مشخصه اولیه پلاک
        /// </summary>
        public int TagFirst { get; set; }

        /// <summary>
        /// مشخصه میانی پلاک
        /// </summary>
        public string TagMiddle { get; set; }

        /// <summary>
        /// مشخصه ثانویه پلاک
        /// </summary>
        public int TagLast { get; set; }

        /// <summary>
        /// تقسیم کشوری پلاک
        /// </summary>
        public int TagNational { get; set; }

        /// <summary>
        /// رنگ پلاک
        /// </summary>
        public E_PublicCategory.TAG_COLOR TagColor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PersonelId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public E_PublicCategory.CAR_TYPE Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Capacity { get; set; }

    }
}
