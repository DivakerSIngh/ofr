using OneFineRateBLL;
using OneFineRateBLL.Entities;
using OneFineRateWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace OneFineRateWeb.Controllers
{
    public class SearchHotelController : BaseController
    {
        // GET: SearchHotel
        public ActionResult Index(PropDetailsForBooking prop)
        {
            // int LocalityId = 8;
            // DateTime dtCheckIn = Convert.ToDateTime("2016-06-20");
            // DateTime dtCheckOut = Convert.ToDateTime("2016-06-21");
            // List<PropDetailsForHotelBooking> data = BL_Locality.GetAllProperty(LocalityId, dtCheckIn, dtCheckOut);
            // PropDetailsForBooking obj = new PropDetailsForBooking();
            // if (data.Count > 0)
            // {
            //     foreach (var item in data)
            //     {
            //         obj.PropDetailsList.Add(item);
            //     }
            // }
            //return View(obj);

            int LocalityId = 2;
            DateTime dtCheckIn = Convert.ToDateTime("2016-06-20");
            DateTime dtCheckOut = Convert.ToDateTime("2016-06-21");

            if (prop.PropDetailsList.Count != 0)
            {
                LocalityId = prop.iLocalityId;
                dtCheckIn = prop.PropDetailsList[0].dtCheckIn == null ? DateTime.Now : Convert.ToDateTime(prop.PropDetailsList[0].dtCheckIn);
                dtCheckOut = prop.PropDetailsList[0].dtCheckOut == null ? DateTime.Now : Convert.ToDateTime(prop.PropDetailsList[0].dtCheckOut);
            }

            List<PropDetailsForHotelBooking> data = BL_Locality.GetAllProperty(LocalityId, dtCheckIn, dtCheckOut);
            PropDetailsForBooking obj = new PropDetailsForBooking();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    obj.PropDetailsList.Add(item);
                }
            }
            else
            {
                obj = prop;
            }

            return View("Index", obj);
        }

        public ActionResult Modify(PropDetailsForBooking prop)
        {
            int LocalityId = prop.iLocalityId;
            DateTime dtCheckIn = prop.PropDetailsList[0].dtCheckIn == null ? DateTime.Now : Convert.ToDateTime(prop.PropDetailsList[0].dtCheckIn);
            DateTime dtCheckOut = prop.PropDetailsList[0].dtCheckOut == null ? DateTime.Now : Convert.ToDateTime(prop.PropDetailsList[0].dtCheckOut);
            List<PropDetailsForHotelBooking> data = BL_Locality.GetAllProperty(LocalityId, dtCheckIn, dtCheckOut);
            PropDetailsForBooking obj = new PropDetailsForBooking();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    obj.PropDetailsList.Add(item);
                }
            }
            else
            {
                obj = prop;
            }

            return View("Index", obj);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Autocomplete(string term)
        {
            var result = new List<KeyValuePair<string, string>>();
            IList<PNames> List = new List<PNames>(BL_Locality.GetAllLocality(Convert.ToInt32(Session["PropId"]), term));
            foreach (var item in List)
            {
                result.Add(new KeyValuePair<string, string>(item.Id.ToString(), item.Name));
            }
            var result3 = result.Where(s => s.Value.ToLower().Contains(term.ToLower())).Select(w => w).ToList();
            return Json(result3, JsonRequestBehavior.AllowGet);
        }
    }
}