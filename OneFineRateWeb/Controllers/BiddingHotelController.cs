using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateBLL.Entities;
using OneFineRateBLL;

namespace OneFineRateWeb.Controllers
{
    public class BiddingHotelController : Controller
    {
        // GET: BiddingHotel
        public ActionResult Index()
        {
            PropDetailsM objlist = new PropDetailsM();
            //objlist = BL_PropDetails.GetPropertyDetails(1);
            return View(objlist);
        }
    }
}