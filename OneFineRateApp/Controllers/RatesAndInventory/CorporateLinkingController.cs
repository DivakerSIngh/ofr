using OneFineRateBLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateApp.Controllers.RatesAndInventory
{
    public class CorporateLinkingController : BaseController
    {
        // GET: CorporateLinking
        [Route("CorporateLinking")]
        public ActionResult Index()
        {
            return View();
        }

        public string Save(string Todate, string Fromdate, string basicDisc, string LengthDisc, string LengthAmenity1, string LengthAppl1, string LengthAmenity2,
            string LengthAppl2, string MultipleDisc, string MultipleAmenity1, string MultipleAppl1, string MultipleAmenity2, string MultipleAppl2, string LeadDisc, string LeadAmenity1,
            string LeadAppl1, string LeadAmenity2, string LeadAppl2, string WeekendDisc, string WeekendAmenity1, string WeekendAppl1, string WeekendAmenity2, string WeekendAppl2)
        {
            object result = null;
            string strReturn = string.Empty;
            try
            {

                strReturn = BL_CorporateLinking.Save(((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId, Convert.ToInt32(Session["PropId"]), Todate, Fromdate,
                    basicDisc, LengthDisc, LengthAmenity1, LengthAppl1, LengthAmenity2, LengthAppl2, MultipleDisc, MultipleAmenity1, MultipleAppl1, MultipleAmenity2,
                    MultipleAppl2, LeadDisc, LeadAmenity1, LeadAppl1, LeadAmenity2, LeadAppl2, WeekendDisc, WeekendAmenity1, WeekendAppl1, WeekendAmenity2, WeekendAppl2);

                result = new { st = 1, msg = strReturn };
            }
            catch (Exception)
            {
                result = new { st = 0, msg = "Kindly try after some time." };
            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }

        public ActionResult Discount()
        {
            var propId = Convert.ToInt32(Session["PropId"]);
            var discountModel = BL_CorporateLinking.GetCorporateDiscountRange(propId);

            return PartialView(discountModel);
        }
    }
}