using OneFineRateAppUtil;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateApp.Controllers
{
    public class BarChangesController : Controller
    {
        // GET: BarChanges
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(eBarChanges_mob model)
        {
            model.dtFrom = clsUtils.ConvertddmmyyyytoDateTime(model.FromDate);
            model.dtTo = clsUtils.ConvertddmmyyyytoDateTime(model.ToDate);
            throw new NotImplementedException();
        }
    }
}