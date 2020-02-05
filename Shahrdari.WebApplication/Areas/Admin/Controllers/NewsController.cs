using Shahrdari.BussinessLayer;
using Shahrdari.Enums.Loging;
using Shahrdari.Logging;
using Shahrdari.Models;
using Shahrdari.Models.Loging;
using Shahrdari.WebApplication.Anotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shahrdari.WebApplication.Areas.Admin.Controllers
{
    public class NewsController : BaseController
    {
        [MFAutorize("لیست اخبار در سیستم", "اخبار")]
        [HttpGet]
        public ActionResult Index()
        {
            return View(new B_News().GetNews().OrderByDescending(c => c.Id).ToList());
        }

        [MFAutorize("افزود و ویرایش اخبار", "اخبار")]
        [HttpGet]
        public ActionResult Modify(int Id)
        {
            ViewBag.News = new B_News().GetNews(Id);
            return View();
        }

        [MFAutorize("افزود و ویرایش اخبار", "اخبار")]
        [HttpPost]
        public ActionResult Modify(M_News News)
        {
            object result = "";
            try
            {
                if (News.Id == 0)
                    new B_News().Add(News);
                else
                    new B_News().Edit(News);
                result = true;
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                result = "Error";
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                {
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                    result = exx;
                }
                L_Log.SubmitLog(exx);
            }
            return Json(result);
        }

        [MFAutorize("حذف خبر", "اخبار")]
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            object result = "";
            try
            {
                new B_News().Delete(Id);
                result = true;
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                result = "Error";
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                {
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                    result = exx;
                }
                L_Log.SubmitLog(exx);
            }
            return Json(result);
        }

        [MFAutorize("آپلود فایل", "اخبار")]
        [HttpPost]
        public ActionResult UploadFile()
        {
            object result = "";
            try
            {
                string fileName = Guid.NewGuid().ToString().Replace("-", "") + "_" + Request.Files[0].FileName;
                HttpPostedFileBase file = Request.Files[0];
                int fileSize = file.ContentLength;
                System.IO.Stream fileContent = file.InputStream;
                file.SaveAs(Server.MapPath("~/Areas/Admin/Images/News") + "/" + fileName);
                result = fileName;
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                result = "Error";
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                {
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                    result = exx;
                }
                L_Log.SubmitLog(exx);
            }
            return Json(result);
        }
    }
}