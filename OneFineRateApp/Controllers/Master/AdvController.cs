using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateApp.Controllers.Master
{
    [Authorize]
    public class AdvController : BaseController
    {
        // GET: Adv
        [Route("Adv")]
        public ActionResult Index()
        {
            return View();
        }
    }
}