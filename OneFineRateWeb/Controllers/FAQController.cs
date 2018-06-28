using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateWeb.Controllers
{
    public class FAQController : BaseController
    {
        // GET: FAQ
        [Route("FAQ")]
        public ActionResult Index()
        {
            return View();
        }
    }
}