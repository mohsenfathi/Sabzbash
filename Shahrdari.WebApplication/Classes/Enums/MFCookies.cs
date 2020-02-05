using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shahrdari.WebApplication.Classes.Enums
{
    /// <summary>
    /// کلید های کوکی
    /// </summary>
    public static class MFCookies
    {
        /// <summary>
        /// کلید کاربران
        /// </summary>
        public static string USER_KEY { get { return "UserUnicKeyForApplication"; } }

        /// <summary>
        /// کلید کاربران نهایی
        /// </summary>
        public static string END_USER_KEY { get { return "EndUserUnicKeyForApplication"; } }

        /// <summary>
        /// کلید مربوط به قسمت راننده و غرفه
        /// </summary>
        public static string BOOTH_RIDER_KEY { get { return "BoothRiderKeyForApplication"; } }
    }
}