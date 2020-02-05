using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.Models
{
    /// <summary>
    /// مدل مربوط به اعتبار سنجی فرم ها
    /// </summary>
    public class M_ValidationResult
    {
        /// <summary>
        /// لیست خطاهای داده شده
        /// </summary>
        public List<string> Errors { get; set; }
        
        /// <summary>
        /// فرم داده های قابل قبول دارد ؟
        /// </summary>
        public bool IsValid { get; set; }
    }
}
