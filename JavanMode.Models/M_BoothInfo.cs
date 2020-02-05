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
    /// غرفه های بازیافت
    /// </summary>
    [Table("BoothInfo")]
    public class M_BoothInfo
    {
        ///<summary>
        /// شناسه یکتا
        ///</summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// نام غرفه بازیافت
        /// </summary>
        [MFMaxLength(250, ErrorMessage = "نام خانوادگی نمیتواند بیش از 250 کاراکتر باشد")]
        public string Name { get; set; }

        /// <summary>
        /// آدرس
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// لت
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// لانگ
        /// </summary>
        public double Lng { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int PersonelId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ImageName { get; set; }

        public int? Capacity { get; set; }
    }
}
