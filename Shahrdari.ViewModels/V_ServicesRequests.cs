using Shahrdari.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.ViewModels
{
    /// <summary>
    /// ویو مدل مربوط به درخواست
    /// </summary>
    public class V_ServicesRequests : M_ServicesRequests
    {
        /// <summary>
        /// کاربر درخواست دهنده
        /// </summary>
        public M_Users User { get; set; }

        /// <summary>
        /// پرسنلی که باید کارو انجام شده
        /// </summary>
        public M_Personels ResponsiblePersonel { get; set; }

        /// <summary>
        /// وضعیت درخواست
        /// </summary>
        public string RequetsStatus { get; set; }
    }
}
