using Shahrdari.Models;
using Shahrdari.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Shahrdari.BussinessLayer
{
    /// <summary>
    /// توابع عمومی
    /// </summary>
    public static class B_PublicFunctions
    {

        /// <summary>
        /// ساخت تصویر کپچا
        /// </summary>
        /// <param name="ImageText">متن مورد نظر</param>
        /// <returns>تصویر ساخته شده</returns>
        public static Bitmap CreateImage(string ImageText, string ftnUrl)
        {
            PrivateFontCollection foo = new PrivateFontCollection();
            foo.AddFontFile(ftnUrl);
            Random rnd = new Random();
            Font myCustomFont = new Font((FontFamily)foo.Families[0], 30f, FontStyle.Underline, GraphicsUnit.Pixel);
            Bitmap bmpImage = new Bitmap(180, 62);
            Graphics MyGraphics = Graphics.FromImage(bmpImage);
            MyGraphics = Graphics.FromImage(bmpImage);
            MyGraphics.Clear(Color.Transparent);
            MyGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            MyGraphics.DrawString(ImageText, myCustomFont, new SolidBrush(Color.FromArgb(rnd.Next(0, 125), rnd.Next(0, 125), rnd.Next(0, 125))), 5, 10);
            for (int i = 0; i <= 10; i++)
            {
                Pen pen = new Pen(Color.FromArgb(rnd.Next(0, 125), rnd.Next(0, 125), rnd.Next(0, 125)));
                MyGraphics.DrawLine(pen, new Point(rnd.Next(i * 12, 180), rnd.Next(i * 5, 62)), new Point(rnd.Next(1, 62), rnd.Next(1, 62)));
            }
            MyGraphics.Flush();
            return (bmpImage);
        }

        /// <summary>
        /// ساخت متن تصادفی
        /// </summary>
        /// <param name="length">طول متن</param>
        /// <returns>رشته ساخته شده</returns>
        public static string RandomString(int length)
        {
            const string chars = "0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// تعیین معتبر بودن کد ملی
        /// </summary>
        /// <param name="NationalCode">کد ملی وارد شده</param>
        /// <returns>
        /// در صورتی که کد ملی صحیح باشد خروجی <c>true</c> و در صورتی که کد ملی اشتباه باشد خروجی <c>false</c> خواهد بود
        /// </returns>
        public static Boolean IsValidNationalCode(this String NationalCode)
        {
            //در صورتی که کد ملی وارد شده تهی باشد

            if (String.IsNullOrEmpty(NationalCode))
                return false;


            //در صورتی که کد ملی وارد شده طولش کمتر از 10 رقم باشد
            if (NationalCode.Length != 10)
                return false;

            //در صورتی که کد ملی ده رقم عددی نباشد
            var regex = new Regex(@"\d{10}");
            if (!regex.IsMatch(NationalCode))
                return false;

            //در صورتی که رقم‌های کد ملی وارد شده یکسان باشد
            var allDigitEqual = new[] { "0000000000", "1111111111", "2222222222", "3333333333", "4444444444", "5555555555", "6666666666", "7777777777", "8888888888", "9999999999" };
            if (allDigitEqual.Contains(NationalCode)) return false;


            //عملیات شرح داده شده در بالا
            var chArray = NationalCode.ToCharArray();
            var num0 = Convert.ToInt32(chArray[0].ToString()) * 10;
            var num2 = Convert.ToInt32(chArray[1].ToString()) * 9;
            var num3 = Convert.ToInt32(chArray[2].ToString()) * 8;
            var num4 = Convert.ToInt32(chArray[3].ToString()) * 7;
            var num5 = Convert.ToInt32(chArray[4].ToString()) * 6;
            var num6 = Convert.ToInt32(chArray[5].ToString()) * 5;
            var num7 = Convert.ToInt32(chArray[6].ToString()) * 4;
            var num8 = Convert.ToInt32(chArray[7].ToString()) * 3;
            var num9 = Convert.ToInt32(chArray[8].ToString()) * 2;
            var a = Convert.ToInt32(chArray[9].ToString());

            var b = (((((((num0 + num2) + num3) + num4) + num5) + num6) + num7) + num8) + num9;
            var c = b % 11;

            return (((c < 2) && (a == c)) || ((c >= 2) && ((11 - c) == a)));
        }

        /// <summary>
        /// تعیین معتبر بودن شناسه ملی
        /// </summary>
        /// <param name="NationalId">کد ملی وارد شده</param>
        /// <returns>
        /// در صورتی که شناسه ملی صحیح باشد خروجی <c>true</c> و در صورتی که کد ملی اشتباه باشد خروجی <c>false</c> خواهد بود
        /// </returns>
        public static Boolean IsValidNationalId(this String NationalId)
        {
            if (string.IsNullOrEmpty(NationalId))
                return false;

            if (NationalId.Length != 11)
                return false;

            return true;
        }

        /// <summary>
        /// تبدیل تاریخ شمسی به میلادی
        /// </summary>
        /// <param name="Date">تاریخ شمسی</param>
        /// <returns>تاریخ تبدیل شده</returns>
        public static DateTime ConverShamsiToMiladi(this string Date)
        {
            try
            {
                string[] s = Date.Split('/');
                System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                DateTime dt1 = pc.ToDateTime(int.Parse(s[0]), int.Parse(s[1]), int.Parse(s[2]), 0, 0, 0, 0);
                return dt1;
            }
            catch
            {
                return new DateTime();
            }
        }

        /// <summary>
        /// چک کردن اینکه آیا تاریخ وارد شده صحیح است یا خیر
        /// </summary>
        /// <param name="date">تاریخ مورد نظر</param>
        /// <param name="IsPast">به تاریخ بعد از تاریخ جاری حساس باشد</param>
        /// <returns>
        /// در صورتی که تاریخ صحیح باشد خروجی <c>true</c> و در صورتی که تاریخ تولد اشتباه باشد خروجی <c>false</c> خواهد بود
        /// </returns>
        public static Boolean IsValidDate(this String Date, bool IsPast = true)
        {
            if (Date.Length >= 8 && Date.Length <= 10)
            {
                string[] s = Date.Split('/');
                if (s.Length != 3)
                    return false;
                if (!s[1].IsNumber())
                    return false;
                if (!s[2].IsNumber())
                    return false;
                if (!s[0].IsNumber())
                    return false;
                if (((int.Parse(s[1]) <= 6 && int.Parse(s[1]) >= 1) && (int.Parse(s[2]) <= 31 && int.Parse(s[2]) >= 1))
                    || ((int.Parse(s[1]) <= 12 && int.Parse(s[1]) >= 7) && (int.Parse(s[2]) <= 30 && int.Parse(s[2]) >= 1)))
                {
                    System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                    DateTime dt1 = pc.ToDateTime(int.Parse(s[0]), int.Parse(s[1]), int.Parse(s[2]), 0, 0, 0, 0);
                    DateTime dt2 = DateTime.Today;
                    if (((dt2 - dt1).TotalDays / 365) < 0 && IsPast == true)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// چک کردن اینکه آیا تاریخ میلادی وارد شده صحیح است یا خیر
        /// </summary>
        /// <param name="date">تاریخ مورد نظر</param>
        /// <param name="IsPast">به تاریخ بعد از تاریخ جاری حساس باشد</param>
        /// <returns>
        /// در صورتی که تاریخ صحیح باشد خروجی <c>true</c> و در صورتی که تاریخ تولد اشتباه باشد خروجی <c>false</c> خواهد بود
        /// </returns>
        public static Boolean IsValidMiladiDate(this String Date, bool IsPast = false)
        {
            DateTime resultDateTime;
            bool result = DateTime.TryParse(Date, out resultDateTime);
            if (IsPast == true)
                if (resultDateTime > DateTime.Now)
                    result = false;
            return result;
        }

        /// <summary>
        /// چک کردن صحیح بودن شماره تلفن
        /// </summary>
        /// <param name="PhoneNumber">شماره تلفن</param>
        /// <param name="IsMobile">تلفن همراه است یا خیر</param>
        /// <returns></returns>
        public static bool IsValidPhone(this String PhoneNumber, bool IsMobile = false)
        {
            if (string.IsNullOrEmpty(PhoneNumber))
                return false;

            Regex reg = new Regex("^[0-9]*$");
            if (!reg.IsMatch(PhoneNumber))
                return false;

            if (PhoneNumber[0] != '0')
                return false;

            if (IsMobile == true && PhoneNumber.Length < 11)
                return false;

            return true;
        }

        /// <summary>
        /// تبدیل یک لیست به ایکس ام ال
        /// </summary>
        /// <typeparam name="T">نوع لیست مورد نظر</typeparam>
        /// <param name="pList">لیست مورد نظر</param>
        /// <returns>نتیجه عملیات</returns>
        public static string ListToXML<T>(IEnumerable<T> pList)
        {
            string Result = "";

            PropertyInfo[] pinf = typeof(T).GetProperties();

            foreach (var item in pList)
            {
                Result += "<" + typeof(T).Name + ">";
                foreach (PropertyInfo prop in pinf)
                {
                    Result += "<" + prop.Name + ">" + prop.GetValue(item, null) + "</" + prop.Name + ">";
                }
                Result += "</" + typeof(T).Name + ">";
            }

            return Result;
        }

        /// <summary>
        /// تبدیل یک مدل ایکس.ام.ال به مدل کلاس
        /// </summary>
        /// <typeparam name="T">نوع مدل مورد نظر</typeparam>
        /// <param name="XmlText">ایکس.ام.ال</param>
        /// <returns></returns>
        public static List<T> ReadFromXml<T>(string XmlText)
        {
            List<T> list = new List<T>();
            List<T> listRes = new List<T>();
            PropertyInfo[] pinf = typeof(T).GetProperties();
            string Node = typeof(T).Name;
            List<string> Result = new List<string>();
            XmlDataDocument xmldoc = new XmlDataDocument();
            xmldoc.LoadXml(XmlText);
            XmlNodeList xmlnode = xmldoc.GetElementsByTagName(Node);

            foreach (XmlNode xn in xmlnode)
            {
                if (xn.InnerXml.IndexOf("<" + Node + ">") == -1)
                    Result.Add("<" + Node + ">" + xn.InnerXml + "</" + Node + ">");
            }
            foreach (string str in Result)
            {
                var instance = Activator.CreateInstance(typeof(T));
                foreach (PropertyInfo prop in pinf)
                {
                    XmlDataDocument xmldoc1 = new XmlDataDocument();
                    xmldoc1.LoadXml(str);
                    XmlNodeList xmlnode1 = xmldoc1.GetElementsByTagName(prop.Name);
                    var s = "";
                    foreach (XmlNode xn2 in xmlnode1)
                    {
                        s = xn2.InnerText;
                    }
                    try
                    {
                        if (prop.PropertyType.IsEnum == true)
                        {
                            Type enumType = prop.PropertyType;
                            if (enumType == null)
                                continue;
                            string valName = Enum.GetName(enumType, int.Parse(s));
                            object enumValue = Enum.Parse(enumType, valName);
                            prop.SetValue(instance, enumValue, null);
                        }
                        else
                        {
                            prop.SetValue(instance, Convert.ChangeType(s, prop.PropertyType), null);
                        }
                    }
                    catch
                    {
                        try
                        {
                            prop.SetValue(instance, Convert.ChangeType(s, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType), null);
                        }
                        catch { }
                    }
                }
                list.Add((T)instance);
                listRes.Add((T)instance);
            }
            return list;
        }

        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// </summary>
        /// <param name="Date">تاریخ مورد نظر</param>
        /// <param name="IsMMDD">ایا خروجی به صورت 1395/02/09 باشد ؟ , پیشفرض خیر</param>
        /// <returns>تاریخ شمسی</returns>
        public static string ConvertMiladiToShamsi(this DateTime Date, bool IsMMDD = false)
        {
            try
            {
                PersianCalendar pc = new PersianCalendar();
                int year = pc.GetYear(Date);
                int month = pc.GetMonth(Date);
                int day = pc.GetDayOfMonth(Date);
                if (IsMMDD == true)
                    return year + "/" + month.ToString().PadLeft(2, '0') + "/" + day.ToString().PadLeft(2, '0');
                return year + "/" + month + "/" + day;
            }
            catch
            {
                return "تاریخ نامعتبر";
            }
        }

        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// </summary>
        /// <param name="Date">تاریخ مورد نظر</param>
        /// <param name="Pattern">قالب تاریخ خروجی با در برداشتن <code>M(Month) , D(Day) , Y(Year)</code> مثلا <code>D/M/Y</code></param>
        /// <param name="IsMMDD">ایا خروجی به صورت 1395/02/09 باشد ؟ , پیشفرض خیر</param>
        /// <param name="haveInvalidDate">درصورت معتبر نیودن تاریخ , تاریخ نا معتبر بازگردانده شود ؟</param>
        /// <returns>تاریخ شمسی</returns>
        public static string ConvertMiladiToShamsi(this DateTime Date, string Pattern, bool IsMMDD = false, bool haveInvalidDate = true)
        {
            try
            {
                System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                int year = pc.GetYear(Date);
                int month = pc.GetMonth(Date);
                int day = pc.GetDayOfMonth(Date);
                if (IsMMDD == true)
                    return Pattern.Replace("Y", year.ToString())
                        .Replace("M", month.ToString().PadLeft(2, '0'))
                        .Replace("D", day.ToString().PadLeft(2, '0'));
                return Pattern.Replace("Y", year.ToString()).Replace("M", month.ToString()).Replace("D", day.ToString());
            }
            catch
            {
                if (haveInvalidDate)
                    return "تاریخ نامعتبر";
                else
                    return "";
            }
        }

        /// <summary>
        /// مپ کردن دو شی
        /// </summary>
        /// <typeparam name="TIn">نوع ابجکت ورودی</typeparam>
        /// <typeparam name="TOut">نوع ابجکت خروجی</typeparam>
        /// <param name="pList">لیست حاوی داده ورودی</param>
        /// <returns>نتیجه عملیات</returns>
        public static List<TOut> GenericMaper<TIn, TOut>(IEnumerable<TIn> pList)
        {
            List<TOut> Result = new List<TOut>();
            if (pList == null)
                return Result;
            PropertyInfo[] inProp = typeof(TIn).GetProperties();

            foreach (var item in pList)
            {
                if (item == null)
                    continue;
                var instance = Activator.CreateInstance<TOut>();
                foreach (PropertyInfo li in inProp)
                {
                    var value = li.GetValue(item, null);
                    try
                    {
                        if (li.PropertyType.IsEnum == true)
                        {
                            Type enumType = li.PropertyType;
                            if (enumType == null)
                                continue;
                            string valName = Enum.GetName(enumType, int.Parse(value.ToString()));
                            object enumValue = Enum.Parse(enumType, valName);
                            li.SetValue(instance, enumValue, null);
                        }
                        else
                        {
                            li.SetValue(instance, Convert.ChangeType(value, li.PropertyType), null);
                        }
                    }
                    catch
                    {
                        try
                        {
                            li.SetValue(instance, Convert.ChangeType(value, Nullable.GetUnderlyingType(li.PropertyType) ?? li.PropertyType), null);
                        }
                        catch { }
                    }
                }
                Result.Add((TOut)instance);
            }
            return Result;
        }

        /// <summary>
        /// چک کردن عدد بودن یا نبودن یک رشته
        /// </summary>
        /// <param name="Input">رشته مورد نظر</param>
        /// <returns>در صورت عدد بودن <c>بلی</c> و در غیر این صورت <c>خیر</c> باز خواهد گشت</returns>
        public static bool IsNumber(this string Input)
        {
            decimal converNumber = 0;
            return decimal.TryParse(Input, out converNumber);
        }

        /// <summary>
        /// چک کردن بولین بودن یا نبودن یک رشته
        /// </summary>
        /// <param name="Input">رشته مورد نظر</param>
        /// <returns>در صورت بولین بودن <c>بلی</c> و در غیر این صورت <c>خیر</c> باز خواهد گشت</returns>
        public static bool IsBoolean(this string Input)
        {
            bool converBool = false;
            return bool.TryParse(Input, out converBool);
        }

        /// <summary>
        /// اعتبارسنجی تاریخ
        /// </summary>
        /// <param name="Date">تاریخ مورد نظر</param>
        /// <returns>نتیجه عملیات</returns>
        public static bool IsValidDate(this DateTime Date)
        {
            return Date == new DateTime() ? false : true;
        }

        /// <summary>
        /// جاگزین کردن اعداد انگلیسی با فارسی
        /// </summary>
        /// <param name="Value">رشته مورد نظر</param>
        /// <returns>نتیجه عملیات</returns>
        public static string ReplacePersianNums(this string Value)
        {
            if (Value == null)
                return "";
            string[] pn = { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };
            string[] en = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            for (int i = 0; i < en.Length; i++)
                Value = Value.Replace( pn[i],en[i]);
            return Value;
        }

        /// <summary>
        /// تغییر اندازه عکس
        /// </summary>
        /// <param name="Image">تصویر مورد نظر</param>
        /// <param name="Width">عرض</param>
        /// <param name="Height">طول</param>
        /// <returns>تصویر ریسایز شده</returns>
        public static Image ResizeImage(this Image Image, int Width, int Height)
        {
            var destRect = new Rectangle(0, 0, Width, Height);
            var destImage = new Bitmap(Width, Height);

            destImage.SetResolution(Image.HorizontalResolution, Image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(Image, destRect, 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        /// <summary>
        /// دریافت نام روز از تاریخ
        /// </summary>
        /// <param name="Date">تاریخ مورد نظر</param>
        /// <returns>نام روز مانند دو شنبه</returns>
        public static string GetDayName(this DateTime Date)
        {
            PersianCalendar pc = new PersianCalendar();
            string dayName = "";
            switch (pc.GetDayOfWeek(DateTime.Now))
            {
                case DayOfWeek.Friday:
                    dayName = "جمعه";
                    break;
                case DayOfWeek.Monday:
                    dayName = "دوشنبه";
                    break;
                case DayOfWeek.Saturday:
                    dayName = "شنبه";
                    break;
                case DayOfWeek.Sunday:
                    dayName = "یکشنبه";
                    break;
                case DayOfWeek.Thursday:
                    dayName = "پنج شنبه";
                    break;
                case DayOfWeek.Tuesday:
                    dayName = "سه شنبه";
                    break;
                case DayOfWeek.Wednesday:
                    dayName = "چهار شنبه";
                    break;
            }
            return dayName;
        }

        /// <summary>
        /// دریافت نام ماه
        /// </summary>
        /// <param name="Date">تاریخ مورد نظر</param>
        /// <returns>نام ماه مانن فروردین</returns>
        public static string GetMonthName(this DateTime Date)
        {
            PersianCalendar pc = new PersianCalendar();
            string monthName = "";
            switch (pc.GetMonth(DateTime.Now))
            {
                case 1:
                    monthName = "فروردین";
                    break;
                case 2:
                    monthName = "اردیبهشت";
                    break;
                case 3:
                    monthName = "خرداد";
                    break;
                case 4:
                    monthName = "تیر";
                    break;
                case 5:
                    monthName = "مرداد";
                    break;
                case 6:
                    monthName = "شهریور";
                    break;
                case 7:
                    monthName = "مهر";
                    break;
                case 8:
                    monthName = "آبان";
                    break;
                case 9:
                    monthName = "آذر";
                    break;
                case 10:
                    monthName = "دی";
                    break;
                case 11:
                    monthName = "بهمن";
                    break;
                case 12:
                    monthName = "اسفند";
                    break;
            }
            return monthName;
        }

        /// <summary>
        /// اعتبار سنجی مدل
        /// </summary>
        /// <typeparam name="T">نوع مدل ارسال شده</typeparam>
        /// <param name="Data">مدل مربوطه</param>
        /// <returns>نتیجه عملیات</returns>
        public static M_ValidationResult ValidateModel<T>(this T Data)
        {
            var results = new List<ValidationResult>();
            var result = Validator.TryValidateObject(Data, new ValidationContext(Data), results, true);
            var messages = results.Select(c => c.ErrorMessage + (c.ErrorMessage.Contains(S_Seprators.ErrorMessageSeprator) ?
            S_Seprators.ErrorFieldNameSeprator.ToString() + c.MemberNames.FirstOrDefault() : "")).ToList();
            return new M_ValidationResult { IsValid = result, Errors = messages };
        }

        public static string ConvertToPesianDateName(this DateTime Date, bool haveYear = false)
        {
            var res = "";
            PersianCalendar pc = new PersianCalendar();
            switch ((int)pc.GetDayOfWeek(Date))
            {
                case (0):
                    res += "یکشنبه";
                    break;
                case (1):
                    res += "دوشنبه";
                    break;
                case (2):
                    res += "سه شنبه";
                    break;
                case (3):
                    res += "چهار شنبه";
                    break;
                case (4):
                    res += "پنج شنبه";
                    break;
                case (5):
                    res += "جمعه";
                    break;
                case (6):
                    res += "شنبه";
                    break;
            }
            res += " " + pc.GetDayOfMonth(Date) + " ";
            switch (pc.GetMonth(Date))
            {
                case (1):
                    res += "فروردین";
                    break;
                case (2):
                    res += "اردیبهشت";
                    break;
                case (3):
                    res += "خرداد";
                    break;
                case (4):
                    res += "تیر";
                    break;
                case (5):
                    res += "مرداد";
                    break;
                case (6):
                    res += "شهریور";
                    break;
                case (7):
                    res += "مهر";
                    break;
                case (8):
                    res += "آبان";
                    break;
                case (9):
                    res += "آذر";
                    break;
                case (10):
                    res += "دی";
                    break;
                case (11):
                    res += "بهمن";
                    break;
                case (12):
                    res += "اسفند";
                    break;
            }
            return haveYear ? res + " " + pc.GetYear(Date) : res;
        }
    }
}
