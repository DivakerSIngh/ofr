using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateWeb.Controllers
{
    public class CareersController : BaseController
    {
        // GET: Careers
        [Route("Careers")]
        public ActionResult Index()
        {
            return View();
        }
    }
}