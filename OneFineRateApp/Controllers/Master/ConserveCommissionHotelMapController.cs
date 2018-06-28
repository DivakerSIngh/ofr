using OneFineRateAppUtil;
using OneFineRateBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateApp.Controllers.Master
{
    public class ConserveCommissionHotelMapController : BaseController
    {
        // GET: ConserveCommissionHotelMap
        public ActionResult Index()
        {
            ViewData["Id"] = Convert.ToInt32(HttpContext.Request.Params["id"]);
            return View();
        }
        public string SaveConserveCommission(int id, string Hotels)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                if (Hotels != "")
                {
                    string check = BL_ConserveCommission.CheckConserveCommissionHotelMapping(id, Hotels);
                    if (check == "")
                    {
                        int i = BL_ConserveCommission.SaveHotelMapping(id, Hotels, ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId);
                        if (i == 1)
                        {
                            result = new { st = 1, msg = clsUtils.ErrorMsg("Conserve Commission-Hotel mapping", 1) };
                        }
                    }
                    else
                    {
                        result = new { st = 0, msg = check };
                    }
                }
                else
                {
                    result = new { st = 0, msg = clsUtils.ErrorMsg("", 7) };
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
                string check = BL_ConserveCommission.CheckConserveCommissionHotelMapping(id, MapHotels);
                if (check == "")
                {
                    int i = BL_ConserveCommission.SaveFilterHotelMapping(id, MapHotels, UnmapHotels, ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId);
                    if (i == 1)
                    {
                        result = new { st = 1, msg = clsUtils.ErrorMsg("Promo Code-Hotel mapping", 2) };
                    }
                }
                else
                {
                    result = new { st = 0, msg = check };
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