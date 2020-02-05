using Shahrdari.BussinessLayer;
using Shahrdari.WebApplication.Anotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shahrdari.WebApplication.Areas.Admin.Controllers
{
    public class SuggestionsController : BaseController
    {
        [MFAutorize("لیست انتقادات و پیشنهادات", "انتقادات و پیشنهادات")]
        [HttpGet]
        public ActionResult Index()
        {
            var model = new B_ListenEar().GetAllListenEar();
            new B_ListenEar().UpdateRead();
            return View(model);
        }
    }
}