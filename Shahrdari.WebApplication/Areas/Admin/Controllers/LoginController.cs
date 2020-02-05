using Shahrdari.BussinessLayer;
using Shahrdari.WebApplication.Classes.Models;
using Shahrdari.Enums.Loging;
using Shahrdari.Logging;
using Shahrdari.Models;
using Shahrdari.Models.Loging;
using Shahrdari.WebApplication.Classes.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shahrdari.WebApplication.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.CaptchaImage = CreateCaptchaObject();
            return View();
        }
        
        /// <summary>
        /// ساخت مدل تصویر امنیتی
        /// </summary>
        /// <returns>مدل ساخت شده</returns>
        private MFCaptcha CreateCaptchaObject()
        {
            MFCaptcha cp = new MFCaptcha();
            string captchaString = B_PublicFunctions.RandomString(6);
            System.Drawing.Bitmap bmp = B_PublicFunctions.CreateImage(captchaString, Server.MapPath("~/Areas/Admin/Fonts/Captcha.ttf"));
            using (MemoryStream stream = new MemoryStream())
            {
                bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();
                cp.ImageContent = stream.ToArray();
            }
            cp.TextValue = captchaString;
            Session[MFSessions.CAPTCHA_KEY] = cp;
            return cp;
        }
        
        [HttpPost]
        public ActionResult ChangeCaptcha()
        {
            return Content(Convert.ToBase64String(CreateCaptchaObject().ImageContent));
        }

        [HttpPost]
        public ActionResult LoginRequest(string UName, string Pass, string Captcha)
        {
            try
            {
                string res = "Sussess";
                if (Captcha.ToLower() != ((MFCaptcha)Session[MFSessions.CAPTCHA_KEY]).TextValue.ToLower())
                    res = "کد امنیتی صحیح نیست";
                else
                {
                    B_Personels bPersonel = new B_Personels();
                    M_Personels us = bPersonel.GetPersonels(UName, Pass);
                    if (us == null)
                        res = "نام کاربری یا گذرواژه صحیح نیست";
                    else
                    {
                        HttpCookie Coki = new HttpCookie(MFCookies.USER_KEY);
                        Coki.Value = us.UnicKey.ToString();
                        Coki.Expires = DateTime.Now.AddYears(1);
                        Response.Cookies.Add(Coki);
                    }
                }
                return Content(res);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult LogOut()
        {
            HttpCookie Coki = new HttpCookie(MFCookies.USER_KEY);
            Coki.Expires = DateTime.Now.AddMilliseconds(-1);
            Response.Cookies.Add(Coki);
            return RedirectToAction("Index");
        }
    }
}