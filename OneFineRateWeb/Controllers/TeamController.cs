using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateWeb.Controllers
{
    public class TeamController : BaseController
    {
        // GET: Team
        [Route("Team")]
        public ActionResult Index()
        {
            return View();
        }
    }
}