using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateApp.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index(string message)
        {
            return View("Error");
        }
        public ActionResult HttpError404(string message)
        {
            return View("_404");
        }
        public ActionResult HttpError500(string message)
        {
            return View("Error");
        }
    }
}