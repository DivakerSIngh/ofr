using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateApp.Controllers.Property
{
    public class PropertyTaxesListController : BaseController
    {
         [Route("PropertyTaxesList")]
        // GET: PropertyTaxesList
        public ActionResult Index()
        {
            return View();
        }
    }
}