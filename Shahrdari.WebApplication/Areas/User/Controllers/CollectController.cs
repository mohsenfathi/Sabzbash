using Shahrdari.BussinessLayer;
using Shahrdari.Enums;
using Shahrdari.WebApplication.Anotations;
using System.Web.Mvc;
using Shahrdari.Models.Area_User;
namespace Shahrdari.WebApplication.Areas.User.Controllers
{
    public class CollectController : BaseController
    {
        // GET: User/Collect
        [MFUsersAutorize]
        public ActionResult Index()
        {
            ViewBag.FullName = CurrentUser.FirstName + " " + CurrentUser.LastName;
            ViewBag.Score = "امتیاز شما " + new B_ServicesRequestItems().GetSumUserPoint(CurrentUser.Id, E_PublicCategory.SYSTEM_USER_TYPE.NORMAL_USER);
            return View();
        }
        [MFUsersAutorize]
        public ActionResult PayInCash()
        {
            return View();
        }
        [MFUsersAutorize]
        public ActionResult CimCharge()
        {
            return View();
        }
    }
}