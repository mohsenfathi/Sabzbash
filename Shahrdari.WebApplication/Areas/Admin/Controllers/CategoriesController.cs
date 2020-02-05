using Shahrdari.BussinessLayer;
using Shahrdari.Enums.Loging;
using Shahrdari.Logging;
using Shahrdari.Models.Loging;
using Shahrdari.WebApplication.Anotations;
using Shahrdari.Models;
using Shahrdari.WebApplication.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;

namespace Shahrdari.WebApplication.Areas.Admin.Controllers
{
    public class CategoriesController : BaseController
    {
        #region Categories

        [MFAutorize("لیست دسته بندی ها", "دسته بندی")]
        [HttpGet]
        public ActionResult Index()
        {
            var bCategory = new B_ServicesCategories();
            return View(bCategory.GetServicesCategories(false));
        }

        [MFAutorize("دزیافت دسته بندی", "دسته بندی")]
        [HttpPost]
        public ActionResult GetCategory(int ParentId)
        {
            object result = "";
            try
            {
                result = new B_ServicesCategories().GetServicesCategories(ParentId, false);
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

        [MFAutorize("افزودن دسته بندی", "دسته بندی")]
        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.Route = new List<MFRoute>
            {
                new MFRoute{ ActionName = "Index", ControllerName="Categories", Title = "لیست دسته بندی ها", Priority = 1 }
            };
            return View();
        }

        [MFAutorize("افزودن دسته بندی", "دسته بندی")]
        [HttpPost]
        public ActionResult Add(M_ServicesCategories ServicesCategories)
        {
            object result = "";
            try
            {
                var bServicesCategories = new B_ServicesCategories();
                bServicesCategories.Add(ServicesCategories);
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

        [MFAutorize("ویرایش و حذف دسته بندی", "دسته بندی")]
        [HttpGet]
        public ActionResult Modify(int Id)
        {
            ViewBag.Route = new List<MFRoute>
            {
                new MFRoute{ ActionName = "Index", ControllerName="Categories", Title = "لیست دسته بندی ها", Priority = 1 }
            };
            var bServicesCategories = new B_ServicesCategories();
            var servicesCategories = bServicesCategories.GetServicesCategories(Id);
            if (servicesCategories == null)
                return RedirectToAction("Index");
            return View(servicesCategories);
        }

        [MFAutorize("ویرایش دسته بندی", "دسته بندی")]
        [HttpPost]
        public ActionResult Modify(M_ServicesCategories ServicesCategories)
        {
            object result = "";
            try
            {
                B_ServicesCategories bServicesCategories = new B_ServicesCategories();
                var oldServicesCategories = bServicesCategories.GetServicesCategories(ServicesCategories.Id);
                bServicesCategories.Edit(ServicesCategories);
                if (oldServicesCategories.ImageName.ToLower() != "default.png" && oldServicesCategories.ImageName.ToLower() != ServicesCategories.ImageName)
                {
                    try
                    {
                        System.IO.File.Delete(Server.MapPath("~/Areas/Admin/Images/Categories/xxxhdpi") + "/" + oldServicesCategories.ImageName);
                        System.IO.File.Delete(Server.MapPath("~/Areas/Admin/Images/Categories/xxhdpi") + "/" + oldServicesCategories.ImageName);
                        System.IO.File.Delete(Server.MapPath("~/Areas/Admin/Images/Categories/xhdpi") + "/" + oldServicesCategories.ImageName);
                        System.IO.File.Delete(Server.MapPath("~/Areas/Admin/Images/Categories/hdpi") + "/" + oldServicesCategories.ImageName);
                        System.IO.File.Delete(Server.MapPath("~/Areas/Admin/Images/Categories/mdpi") + "/" + oldServicesCategories.ImageName);
                    }
                    catch { }
                }
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

        [MFAutorize("حذف دسته بندی", "دسته بندی")]
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            object result = "";
            try
            {
                B_ServicesCategories bServicesCategories = new B_ServicesCategories();
                var oldServicesCategories = bServicesCategories.GetServicesCategories(Id);
                bServicesCategories.Delete(Id);
                try
                {
                    System.IO.File.Delete(Server.MapPath("~/Images/Categories/xxxhdpi") + "/" + oldServicesCategories.ImageName);
                    System.IO.File.Delete(Server.MapPath("~/Images/Categories/xxhdpi") + "/" + oldServicesCategories.ImageName);
                    System.IO.File.Delete(Server.MapPath("~/Images/Categories/xhdpi") + "/" + oldServicesCategories.ImageName);
                    System.IO.File.Delete(Server.MapPath("~/Images/Categories/hdpi") + "/" + oldServicesCategories.ImageName);
                    System.IO.File.Delete(Server.MapPath("~/Images/Categories/mdpi") + "/" + oldServicesCategories.ImageName);
                }
                catch { }
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

        [MFAutorize("آپلود تصویر", "دسته بندی")]
        [HttpPost]
        public ActionResult UploadImage()
        {
            object result = "";
            try
            {
                string fileName = Guid.NewGuid().ToString().Replace("-", "") + ".png";
                HttpPostedFileBase file = Request.Files[0];
                file.SaveAs(Server.MapPath("~/Areas/Admin/Images/Categories/xxxhdpi") + "/" + fileName);
                Image img = Image.FromStream(file.InputStream, true, true);
                img.ResizeImage(144, 144).Save(Server.MapPath("~/Areas/Admin/Images/Categories/xxhdpi") + "/" + fileName);
                img.ResizeImage(96, 96).Save(Server.MapPath("~/Areas/Admin/Images/Categories/xhdpi") + "/" + fileName);
                img.ResizeImage(72, 72).Save(Server.MapPath("~/Areas/Admin/Images/Categories/hdpi") + "/" + fileName);
                img.ResizeImage(48, 48).Save(Server.MapPath("~/Areas/Admin/Images/Categories/mdpi") + "/" + fileName);
                file.InputStream.Close();
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

        #endregion Categories

        /*
        #region Tariffs

        [MFAutorize("لیست تعرفه ها", "تعرفه ها")]
        [HttpGet]
        public ActionResult Tariffs()
        {
            ViewBag.Categories = new B_ServicesCategories().GetServicesCategories(false);
            return View();
        }

        [MFAutorize("افزودن تعرفه", "تعرفه ها")]
        [HttpGet]
        public ActionResult AddTariff()
        {
            ViewBag.Route = new List<MFRoute>
            {
                new MFRoute{ ActionName = "AddTariff", ControllerName="Categories", Title = "لیست تعرفه ها", Priority = 1 }
            };
            B_ServicesCategories bCategories = new B_ServicesCategories();
            ViewBag.Categories = bCategories.GetServicesCategories(false);
            return View();
        }

        [MFAutorize("افزودن تعرفه", "تعرفه ها")]
        [HttpPost]
        public ActionResult AddTariff(M_Tariffs Tariff, List<int> CategoryIds)
        {
            object result = "";
            try
            {
                var bTariffs = new B_Tariffs();
                bTariffs.Add(Tariff, CategoryIds);
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

        [MFAutorize("ویرایش و حذف تعرفه", "تعرفه ها")]
        [HttpGet]
        public ActionResult ModityTariff(int Id)
        {
            ViewBag.Route = new List<MFRoute>
            {
                new MFRoute{ ActionName = "AddTariff", ControllerName="Categories", Title = "لیست تعرفه ها", Priority = 1 }
            };
            var tariffe = new B_Tariffs().GetTariffs(Id);
            if (tariffe == null)
                return RedirectToAction("Tariffs");
            B_ServicesCategories bCategories = new B_ServicesCategories();
            ViewBag.Categories = bCategories.GetServicesCategories(false);
            return View(tariffe);
        }

        [MFAutorize("ویرایش تعرفه", "تعرفه ها")]
        [HttpPost]
        public ActionResult ModityTariff(M_Tariffs Tariff, List<int> CategoryIds)
        {
            object result = "";
            try
            {
                var bTariffs = new B_Tariffs();
                result = bTariffs.Edit(Tariff, CategoryIds);
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

        [MFAutorize("حذف تعرفه", "تعرفه ها")]
        [HttpPost]
        public ActionResult DeleteTariff(int id)
        {
            object result = "";
            try
            {
                new B_Tariffs().Delete(id);
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

        [MFAutorize("لیست تعرفه ها", "تعرفه ها")]
        [HttpPost]
        public ActionResult GetTariffs(string Title, int ServicCategoryId)
        {
            object result = "";
            try
            {
                result = new B_Tariffs().GetTariffs(Title, ServicCategoryId);
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

        #endregion Tariffs*/
    }
}