using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateWeb.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return PartialView("Error");
        }
        public ActionResult Notfound()
        {
            return PartialView();
        }
        public ActionResult Forbidden()
        {
            return PartialView();
        }
        public ActionResult InternalServerError()
        {
            return PartialView();
        }
        public ActionResult Unauthorized()
        {
            return PartialView();
        }

    }
}