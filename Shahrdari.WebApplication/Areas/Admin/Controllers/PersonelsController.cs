using Shahrdari.BussinessLayer;
using Shahrdari.Enums;
using Shahrdari.Enums.Loging;
using Shahrdari.Logging;
using Shahrdari.Models;
using Shahrdari.Models.Loging;
using Shahrdari.WebApplication.Anotations;
using Shahrdari.WebApplication.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shahrdari.WebApplication.Areas.Admin.Controllers
{
    public class PersonelsController : BaseController
    {
        #region کارکنان

        [MFAutorize("لیست کارکنان", "کارکنان")]
        [HttpGet]
        public ActionResult Index()
        {
            B_Personels bPersonel = new B_Personels();
            ViewBag.Roles = new B_PersonelRoles().GetPersonelRoles();
            return View(bPersonel.GetPersonels().Where(c => c.Id != CurrentUser.Id).ToList());
        }

        [MFAutorize("افزودن کارمند", "کارکنان")]
        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.Route = new List<MFRoute>
            {
                new MFRoute{ ActionName = "Index", ControllerName="Personels", Title = "لیست کارکنان", Priority = 1 }
            };
            ViewBag.Roles = new B_PersonelRoles().GetPersonelRoles();
            ViewBag.VehicleType = new B_PublicCategory().GetPublicCategory(E_PublicCategory.PUBLIC_CATEGORY_PARENT.CAR_TYPE);
            ViewBag.PlaqColor = new B_PublicCategory().GetPublicCategory(E_PublicCategory.PUBLIC_CATEGORY_PARENT.TAG_COLOR);
            return View();
        }

        [MFAutorize("افزودن کارمند", "کارکنان")]
        [HttpPost]
        public ActionResult Add(M_Personels Personel, M_CarInfo CareInfo, M_BoothInfo BoothInfo)
        {
            object result = "";
            try
            {
                B_Personels bPersonel = new B_Personels();
                var personel = bPersonel.Add(Personel);

                if (Personel.PersonelType == E_PublicCategory.PERSONEL_TYPE.DRIVER)
                {
                    CareInfo.PersonelId = personel.Id;
                    new B_CarInfo().Add(CareInfo);
                }
                else if (Personel.PersonelType == E_PublicCategory.PERSONEL_TYPE.INTEGRATION_CENTER || Personel.PersonelType == E_PublicCategory.PERSONEL_TYPE.SUM_CENER)
                {
                    BoothInfo.PersonelId = personel.Id;
                    new B_Booth().Add(BoothInfo);
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

        [MFAutorize("ویرایش و حذف کارمند", "کارکنان")]
        [HttpGet]
        public ActionResult Modify(int Id)
        {
            ViewBag.Route = new List<MFRoute>
            {
                new MFRoute{ ActionName = "Index", ControllerName="Personels", Title = "لیست کارکنان", Priority = 1 }
            };
            B_Personels bRole = new B_Personels();
            var personel = bRole.GetPersonelById(Id);
            if (personel == null || personel.Id == CurrentUser.Id)
                return RedirectToAction("Index");
            B_PersonelRoles bRoleVal = new B_PersonelRoles();
            ViewBag.Roles = bRoleVal.GetPersonelRoles();
            ViewBag.VehicleType = new B_PublicCategory().GetPublicCategory(E_PublicCategory.PUBLIC_CATEGORY_PARENT.CAR_TYPE);
            ViewBag.PlaqColor = new B_PublicCategory().GetPublicCategory(E_PublicCategory.PUBLIC_CATEGORY_PARENT.TAG_COLOR);

            ViewBag.Booth = new B_Booth().GetBoothByPersonelId(personel.Id);
            ViewBag.Car = new B_CarInfo().GetCarInfoByPersonelId(personel.Id);

            return View(personel);
        }

        [MFAutorize("ویرایش و حذف کارمند", "کارکنان")]
        [HttpPost]
        public ActionResult Modify(M_Personels Personel, M_CarInfo CareInfo, M_BoothInfo BoothInfo)
        {
            object result = "";
            try
            {
                B_Personels bPersonel = new B_Personels();
                var oldPersonel = bPersonel.GetPersonelById(Personel.Id);
                if (oldPersonel.ImageName.ToLower() != "default.jpg" && oldPersonel.ImageName.ToLower() != Personel.ImageName)
                    System.IO.File.Delete(Server.MapPath("~/Areas/Admin/Images/Profile") + "/" + oldPersonel.ImageName);
                bPersonel.Edit(Personel);
                if (CareInfo != null)
                {
                    if (CareInfo.Id == 0)
                    {
                        CareInfo.PersonelId = Personel.Id;
                        new B_CarInfo().Add(CareInfo);
                    }
                    else
                        new B_CarInfo().Edit(CareInfo);
                }
                if (BoothInfo != null)
                {
                    if (BoothInfo.Id == 0)
                    {
                        BoothInfo.PersonelId = Personel.Id;
                        new B_Booth().Add(BoothInfo);
                    }
                    else
                        new B_Booth().Edit(BoothInfo);
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

        [MFAutorize("آپلود تصویر", "کارکنان")]
        [HttpPost]
        public ActionResult UploadProfileImage()
        {
            object result = "";
            try
            {
                string fileName = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                HttpPostedFileBase file = Request.Files[0];
                int fileSize = file.ContentLength;
                System.IO.Stream fileContent = file.InputStream;
                file.SaveAs(Server.MapPath("~/Areas/Admin/Images/Profile") + "/" + fileName);
                file.SaveAs(Server.MapPath("~/Areas/Admin/Images/Personels") + "/" + fileName);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                Response.Cache.SetNoStore();
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

        [MFAutorize("حذف کارمند", "کارکنان")]
        [HttpPost]
        public ActionResult Delete(int Id)
        {
            object result = "";
            try
            {
                B_Personels bPersonel = new B_Personels();
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

        [MFAutorize("کارکنان حذف شده", "کارکنان")]
        [HttpGet]
        public ActionResult Deleted()
        {
            ViewBag.Route = new List<MFRoute>
            {
                new MFRoute{ ActionName = "Index", ControllerName="Personels", Title = "لیست کارکنان", Priority = 1 }
            };
            B_Personels bPersonel = new B_Personels();
            return View(bPersonel.GetDeletedPersonel());
        }

        [MFAutorize("کارکنان حذف شده", "کارکنان")]
        [HttpPost]
        public ActionResult RevertUser(int Id)
        {
            object result = "";
            try
            {
                B_Personels bPersonel = new B_Personels();
                var personel = bPersonel.GetPersonelById(Id);
                personel.IsDeleted = false;
                bPersonel.Edit(personel);
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


        [MFAutorize("دریافت سفرهای رفته رانندگان", "کارکنان")]
        [HttpPost]
        public ActionResult GetServiceMapPointes(int Id)
        {
            object result = "";
            try
            {
                B_ServicesRequests bPersonel = new B_ServicesRequests();
                var points = bPersonel.GetServiceMapPointes(Id);
                var objs = new List<object>();
                foreach (var li in points)
                {
                    objs.Add(new {
                        popupContent = "<p></p>",
                        center = new { lat = li.Split(',')[0].Trim(), lng = li.Split(',')[1].Trim() },
                        iconOpts =  new { iconUrl = "/Areas/Admin/Images/MapMarkerNew.png", iconRetinaUrl = "/Areas/Admin/Images/MapMarkerNew.png", iconSize = new int[] { 30, 38 } }
                    });
                }
                result = objs;
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

        #endregion کارکنان

        #region نقش های کارکنان

        [MFAutorize("لیست نقش ها", "نقش های کارکنان")]
        [HttpGet]
        public ActionResult Roles()
        {
            B_PersonelRoles bRoles = new B_PersonelRoles();
            return View(bRoles.GetPersonelRoles());
        }

        [MFAutorize("افزودن نقش", "نقش های کارکنان")]
        [HttpGet]
        public ActionResult AddRole()
        {
            ViewBag.Route = new List<MFRoute>
            {
                new MFRoute{ ActionName = "Roles", ControllerName="Personels", Title = "لیست نقش ها", Priority = 1 }
            };
            ViewBag.Actions = getActions();
            return View();
        }

        [MFAutorize("افزودن نقش", "نقش های کارکنان")]
        [HttpPost]
        public ActionResult AddRole(M_PersonelRoles Role, List<string> Permissions)
        {
            object result = "";
            try
            {
                B_PersonelRoles bPersonelRole = new B_PersonelRoles();
                B_PersonelRoleValues bPersonelRoleValue = new B_PersonelRoleValues();
                Role = bPersonelRole.Add(Role);
                if (!Role.HasFullControl)
                    foreach (var li in Permissions)
                    {
                        bPersonelRoleValue.Add(new M_PersonelRoleValues
                        {
                            AccessName = li,
                            PersonelRoleId = Role.Id
                        });
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

        [MFAutorize("ویرایش و حذف نقش", "نقش های کارکنان")]
        [HttpGet]
        public ActionResult ModifyRole(int Id)
        {
            ViewBag.Route = new List<MFRoute>
            {
                new MFRoute{ ActionName = "Roles", ControllerName="Personels", Title = "لیست نقش ها", Priority = 1 }
            };
            B_PersonelRoles bRole = new B_PersonelRoles();
            var role = bRole.GetPersonelRoles(Id);
            if (role == null)
                return RedirectToAction("Roles");
            B_PersonelRoleValues bRoleVal = new B_PersonelRoleValues();
            ViewBag.RoleValue = bRoleVal.GetPersonelRoleValues(Id);
            ViewBag.Actions = getActions();
            return View(role);
        }

        [MFAutorize("ویرایش و حذف نقش", "نقش های کارکنان")]
        [HttpPost]
        public ActionResult ModifyRole(M_PersonelRoles Role, List<string> Permissions)
        {
            object result = "";
            try
            {
                B_PersonelRoles bPersonelRole = new B_PersonelRoles();
                bPersonelRole.Edit(Role);
                B_PersonelRoleValues bPersonelRoleValue = new B_PersonelRoleValues();
                bPersonelRoleValue.Delete(Role.Id);
                if (!Role.HasFullControl)
                    foreach (var li in Permissions)
                    {
                        bPersonelRoleValue.Add(new M_PersonelRoleValues
                        {
                            AccessName = li,
                            PersonelRoleId = Role.Id
                        });
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

        [MFAutorize("حذف نقش", "نقش های کارکنان")]
        [HttpPost]
        public ActionResult DeleteRole(int Id)
        {
            object result = "";
            try
            {
                B_PersonelRoles bPersonelRole = new B_PersonelRoles();
                bPersonelRole.Delete(Id);
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

        #endregion نقش های کارکنان

        [MFAutorize("آپلود تصویر", "کارکنان")]
        [HttpPost]
        public ActionResult UploadImage()
        {
            object result = "";
            try
            {
                var isCar = bool.Parse(Request.Headers["IsCar"]);
                string fileName = Guid.NewGuid().ToString().Replace("-", "") + ".jpg";
                HttpPostedFileBase file = Request.Files[0];
                int fileSize = file.ContentLength;
                System.IO.Stream fileContent = file.InputStream;
                file.SaveAs(Server.MapPath("~/Areas/Admin/Images/" + (isCar ? "CarInfo" : "BoothInfo")) + "/" + fileName);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
                Response.Cache.SetNoStore();
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