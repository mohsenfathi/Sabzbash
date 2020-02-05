using Shahrdari.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.ViewModels
{
    /// <summary>
    /// ویو مدل مربوط به دسته بندی ها
    /// </summary>
    public class V_ServicesCategories : M_ServicesCategories
    {
        /// <summary>
        /// دسته بند های  زیر مجموعه
        /// </summary>
        public List<V_ServicesCategories> ServicesCategories { get; set; }
    }
}
