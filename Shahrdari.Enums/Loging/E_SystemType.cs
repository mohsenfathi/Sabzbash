using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shahrdari.Enums.Loging
{
    /// <summary>
    /// نام سیستم ها
    /// </summary>
    [Serializable]
    public enum E_SystemType
    {
        /// <summary>
        /// وبسرویس جوان مد
        /// </summary>
        SHAHRDARI_WEB_APPLICATION = 2,

        /// <summary>
        /// کاربر پرتال شهرداری
        /// </summary>
        SHAHRDARI_USER_WEB_APPLICATION = 3,

        /// <summary>
        /// هاب سیگنار آر
        /// </summary>
        SIGNALR_HUB = 4,

        /// <summary>
        /// پرسنل نسخه وب شهرداری
        /// </summary>
        SHAHRDARI_BOOTH_RIDER_APPLICATION = 5,
    }
}
