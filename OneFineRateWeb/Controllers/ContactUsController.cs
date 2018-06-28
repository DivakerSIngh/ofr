using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateWeb.Controllers
{
    public class ContactUsController : Controller
    {
        // GET: ContactUs
        [Route("Contactus")]
        public ActionResult Index()
        {
            return View();
        }
    }
}