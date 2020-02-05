using Shahrdari.BussinessLayer;
using Shahrdari.Enums.Loging;
using Shahrdari.Logging;
using Shahrdari.Models;
using Shahrdari.Models.Loging;
using Shahrdari.WebApplication.Anotations;
using System;
using System.Web.Mvc;

namespace Shahrdari.WebApplication.Areas.Admin.Controllers
{
    public class MessageController : BaseController
    {
        [MFAutorize("لیست پیامها", "پیامها")]
        [HttpGet]
        public ActionResult Index()
        {
            var bMessage = new B_Messages();
            return View(bMessage.GetMessgas());
        }

        [MFAutorize("افزودن پیام جدید", "پیامها")]
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [MFAutorize("افزودن پیام جدید", "پیامها")]
        [HttpPost]
        public ActionResult Add(string messages, string title)
        {
            object result = "";
            try
            {
                B_Messages bMessage = new B_Messages();
                bMessage.Add(new M_Messages { CreateDate = DateTime.Now, messages = messages, title = title });
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

        [MFAutorize("ویرایش پیام", "پیامها")]
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var bMessage = new B_Messages();
            return View(bMessage.GetMessgas(Id));
        }

        [MFAutorize("ویرایش پیام", "پیامها")]
        [HttpPost]
        public ActionResult Edit(M_Messages Message)
        {
            object result = "";
            try
            {
                B_Messages bMessage = new B_Messages();
                bMessage.Edit(Message);
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

        [MFAutorize("حذف پیام", "پیامها")]
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            object result = "";
            try
            {
                B_Messages bMessage = new B_Messages();
                bMessage.Delete(Id);
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
    }
}