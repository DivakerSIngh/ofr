using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System.Data.Entity.Validation;
using System.Globalization;
using OneFineRateAppUtil;
namespace OneFineRateApp.Controllers.Master
{
    [Authorize]
    public class UserHotelMapController : BaseController
    {
       // [Route("UserHotelMap")]
        public ActionResult Index()
        {
            ViewData["Id"] = Convert.ToInt32(HttpContext.Request.Params["id"]);
            ViewData["fname"] = Convert.ToString(HttpContext.Request.Params["fname"]);
            ViewData["lname"] = Convert.ToString(HttpContext.Request.Params["lname"]);

            return View("UserHotelMap");
        }

        public string SaveHotels(int id, string Hotels)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {

                int i = BL_UserPropertyMap.SaveHotelMapping(id, Hotels, ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId);
                if (i == 1)
                {
                    result = new { st = 1, msg = clsUtils.ErrorMsg("User-Hotel mapping", 1) };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = clsUtils.ErrorMsg("", 3) };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }

        public string SaveFilterHotels(int id, string MapHotels, string UnmapHotels)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {

                int i = BL_UserPropertyMap.SaveFilterHotelMapping(id, MapHotels, UnmapHotels,((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId);
                if (i == 1)
                {
                    result = new { st = 1, msg = clsUtils.ErrorMsg("User-Hotel mapping", 2) };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = clsUtils.ErrorMsg("", 3) };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }
    }
}