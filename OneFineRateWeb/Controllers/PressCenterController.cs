using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateWeb.Controllers
{
    public class PressCenterController : BaseController
    {
        // GET: PressCenter
        [Route("PressCenter")]
        public ActionResult Index()
        {
            return View();
        }
    }
}