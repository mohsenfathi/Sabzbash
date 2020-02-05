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
    public class LearnController : BaseController
    {
        [MFAutorize("لیست آموزش ها", "آموزش ها")]
        [HttpGet]
        public ActionResult Index()
        {
            return View(new B_Learns().GetLearns());
        }

        [MFAutorize("افزودن آموزش جدید", "آموزش ها")]
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [MFAutorize("افزودن آموزش جدید", "آموزش ها")]
        [HttpPost]
        public ActionResult Add(M_Learns Learn)
        {
            object result = "";
            try
            {
                B_Learns bPersonel = new B_Learns();
                bPersonel.Add(Learn);
                result = "Success";
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
        
        [MFAutorize("ویرایش آموزش", "آموزش ها")]
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            return View(new B_Learns().GetLearns(Id));
        }

        [MFAutorize("ویرایش آموزش", "آموزش ها")]
        [HttpPost]
        public ActionResult Edit(M_Learns Learn)
        {
            object result = "";
            try
            {
                B_Learns bPersonel = new B_Learns();
                bPersonel.Edit(Learn);
                result = "Success";
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

        [MFAutorize("حذف آموزش", "آموزش ها")]
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            object result = "";
            try
            {
                B_Learns bPersonel = new B_Learns();
                bPersonel.Delete(Id);
                result = "Success";
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
        
        [MFAutorize("آپلود فایل", "آموزش ها")]
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
                file.SaveAs(Server.MapPath("~/Areas/Admin/AttachedFiles") + "/" + fileName);
                result = $"{Request.Url.Scheme}{System.Uri.SchemeDelimiter}{Request.Url.Authority}/Areas/Admin/AttachedFiles/" + fileName;
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

        [MFAutorize("آپلود فایل", "آموزش ها")]
        [HttpPost]
        public ActionResult UploadCoverFile()
        {
            object result = "";
            try
            {
                string fileName = Guid.NewGuid().ToString().Replace("-", "") + "_" + Request.Files[0].FileName;
                HttpPostedFileBase file = Request.Files[0];
                int fileSize = file.ContentLength;
                System.IO.Stream fileContent = file.InputStream;
                file.SaveAs(Server.MapPath("~/Areas/Admin/Images/Learns") + "/" + fileName);
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