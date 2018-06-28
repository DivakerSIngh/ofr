using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateWeb.Controllers
{
    public class AboutUsController : BaseController
    {
        // GET: AboutUs
        [Route("AboutUs")]
        public ActionResult Index()
        {
            return View();
        }
    }
}