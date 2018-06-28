using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateWeb.Controllers
{
    public class SitemapController : BaseController
    {
        // GET: Sitemap
        [Route("Sitemap")]
        public ActionResult Index()
        {
            return View();
        }
    }
}