using Shahrdari.BussinessLayer;
using Shahrdari.Enums;
using Shahrdari.Enums.Loging;
using Shahrdari.Logging;
using Shahrdari.Models;
using Shahrdari.Models.Area_User;
using Shahrdari.Models.Loging;
using Shahrdari.ViewModels;
using Shahrdari.WebApplication.Anotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shahrdari.WebApplication.Areas.User.Controllers
{
    public class EducationController : BaseController
    {
        // GET: User/Education
        [MFUsersAutorize]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetEducations()
        {
            object result = "";
            try
            {
                result = new B_Learns().GetLearns();
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_USER_WEB_APPLICATION, E_LogType.ERROR, ex);
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

        [HttpPost]
        public ActionResult Search(string SearchField)
        {
            object result = "";
            try
            {
                if (string.IsNullOrEmpty(SearchField))
                    result = new B_Learns().GetLearns().OrderByDescending(x=>x.Id);
                else
                    result = new B_Learns().GetLearns().Where(x=>x.Title.Contains(SearchField)).ToList().OrderByDescending(x => x.Id);
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_USER_WEB_APPLICATION, E_LogType.ERROR, ex);
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