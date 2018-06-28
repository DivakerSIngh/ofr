using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System.Globalization;

namespace OneFineRateApp.Controllers.RatesAndInventory.NegotiationNotification
{
    [Authorize]
    public class NegotiationNotificationController : BaseController
    {
        // GET: NegotiationNotification
        [Route("NegotiationNotification")]
        public ActionResult Index()
        {
            etblPropertyRatePlanMap obj = new etblPropertyRatePlanMap();
            TempData["PropID"] = Convert.ToInt32(Session["PropId"].ToString());
            return View(obj);
        }

        public string Save(int Id, string rectype, decimal NegotiationVal)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                int i = BL_NegotiationNotification.Update(Id, rectype, NegotiationVal, Convert.ToInt32(Session["PropId"].ToString()));
                if (i == 1)
                {
                    result = new { st = 1, msg = "Updated Successfully ." };
                }
                else
                {
                    result = new { st = 0, msg = "Kindly try after some time." };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = "Kindly try after some time." };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }
        public string CopyAll(int Id, string rectype, decimal NegotiationVal)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                int i = BL_NegotiationNotification.Update(0, rectype, NegotiationVal, Convert.ToInt32(Session["PropId"].ToString()));
                if (i == 1)
                {
                      result = new { st = 1, msg = "Updated Successfully ." };
                 }
                else
                {
                    result = new { st = 0, msg = "Kindly try after some time." };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = "Kindly try after some time." };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }


    }
}