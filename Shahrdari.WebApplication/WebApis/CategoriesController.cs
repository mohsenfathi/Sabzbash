using Shahrdari.BussinessLayer;
using Shahrdari.Enums;
using Shahrdari.Enums.Loging;
using Shahrdari.Logging;
using Shahrdari.Models;
using Shahrdari.Models.Loging;
using Shahrdari.Settings;
using Shahrdari.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Shahrdari.WebApplication.WebApis
{
    public class CategoriesController : BaseController
    {
        [HttpGet]
        public IHttpActionResult GetAllCategories()
        {
            try
            {
                B_ServicesCategories bCategory = new B_ServicesCategories();
                var category = B_PublicFunctions.GenericMaper<M_ServicesCategories, V_ServicesCategories>(bCategory.GetServicesCategories(false));
                category = prepareCategoryViewModel(0, category);
                return Json(PrepareSuccessResult(category));
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                if (ex.Source != E_LogType.SYSTEM_ERROR.ToString())
                    return Json(PrepareFaildResult("خطایی در سرور وجود دارد", (int)E_ErrorCodes.SERVER_ERROR + S_Seprators.ErrorFieldNameSeprator.ToString() + ""));
                return Json(PrepareFaildResult(exx.LogMessage, ex.HelpLink));
            }
        }

        [HttpGet]
        public IHttpActionResult GetCategory(int? Id)
        {
            try
            {
                List<object> obj = new List<object>();
                var categories = new B_ServicesCategories().GetServicesCategories(Id.HasValue ? Id.Value : 0, true);
                if (categories != null && categories.Count > 0)
                    foreach (var li in categories)
                        obj.Add(new {
                            CreateDate = li.CreateDate,
                            DeactiveDate = li.DeactiveDate,
                            Description = li.Description,
                            HaveNew = li.HaveNew,
                            Id = li.Id,
                            ImageUrl = GetImageAddress(li.ImageName, new E_FTPRoutes(BaseUrl).CATEGORIES),
                            IsActive = li.IsActive,
                            Lineages = li.Lineages,
                            ParentId = li.ParentId,
                            Route = li.Route,
                            ScorePerUnit = li.ScorePerUnit,
                            Title = li.Title,
                            Unit = li.Unit,
                            CategoryId = li.Id
                        });
                return Json(PrepareSuccessResult(obj));
            }
            catch (Exception ex)
            {
                M_SystemLog exx = new M_SystemLog(E_SystemType.SHAHRDARI_WEB_APPLICATION, E_LogType.ERROR, ex);
                if (ex.Source == E_LogType.SYSTEM_ERROR.ToString())
                    exx.LogType = E_LogType.SYSTEM_ERROR;
                L_Log.SubmitLog(exx);
                if (ex.Source != E_LogType.SYSTEM_ERROR.ToString())
                    return Json(PrepareFaildResult("خطایی در سرور وجود دارد", (int)E_ErrorCodes.SERVER_ERROR + S_Seprators.ErrorFieldNameSeprator.ToString() + ""));
                return Json(PrepareFaildResult(exx.LogMessage, ex.HelpLink));
            }
        }

        private List<V_ServicesCategories> prepareCategoryViewModel(int parentId, List<V_ServicesCategories> categories)
        {
            var result = new List<V_ServicesCategories>();
            foreach (var li in categories.Where(c => c.ParentId == parentId).ToList())
            {
                li.ServicesCategories = prepareCategoryViewModel(li.Id, categories);
                result.Add(li);
            }
            return result;
        }
    }
}