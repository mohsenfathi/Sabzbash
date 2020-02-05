using Shahrdari.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.ViewModels
{
    /// <summary>
    /// ویو مدل کاربران
    /// </summary>
    public class V_Users : M_Users
    {
        /// <summary>
        /// مشاهده شده توسط کاربر جاری
        /// </summary>
        public bool AdminSee { get; set; }

        /// <summary>
        /// تاریخ اخرین درخواست
        /// </summary>
        public DateTime? LastRequest { get; set; }

        public string ConnectionId { get; set; }
    }
}
