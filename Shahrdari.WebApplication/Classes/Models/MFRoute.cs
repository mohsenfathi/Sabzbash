using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shahrdari.WebApplication.Classes.Models
{
    /// <summary>
    /// مدل مربوط به مسر جاری
    /// </summary>
    public class MFRoute
    {
        /// <summary>
        /// نام کنترلر
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// نام اکشن
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// عنوان
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// اولویت
        /// </summary>
        public int Priority { get; set; }
    }
}