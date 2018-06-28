using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateWeb.Controllers
{
    public class TermAndConditionsController : BaseController
    {
        // GET: TermAndConditions
        [Route("Terms")]
        public ActionResult Index()
        {
            return View();
        }
    }
}