using FutureSoft.Util;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateApp.Controllers.RatesAndInventory
{
    public class BulkBidController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t">i.e type of public or corporate</param>
        /// <param name="s">start date</param>
        /// <param name="e">end date</param>
        /// <returns></returns>
        [Route("BulkBid")]
        public ActionResult Index(string t, string s, string e)
        {
            if (string.IsNullOrWhiteSpace(t))
            {
                t = "b";
            }
            ViewBag.Type = t;
            ViewBag.StartDate = s;
            ViewBag.EndDate = e;
            return View();
        }

        public string GetAmenitiesList()
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                List<eAmenity> AM = new List<eAmenity>();
                AM = BL_Amenity.GetAllAmenities();

                strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(AM);
                result = new { st = 1, msg = strReturn };
            }
            catch (Exception ex)
            {
                result = new { st = 0, msg = "Kindly try after some time." };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }

        public string GetApplicabilitiesList()
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                List<eApplicability> AM = new List<eApplicability>();
                AM = BL_Amenity.GetAllApplicabilities();

                strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(AM);
                result = new { st = 1, msg = strReturn };
            }
            catch (Exception ex)
            {
                result = new { st = 0, msg = "Kindly try after some time." };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }

        public class eSaveLead
        {
            public string dates { get; set; }
            public string CloseOut { get; set; }
            public string CTA { get; set; }
            public string CTD { get; set; }
            public string Lead { get; set; }
            public string typ { get; set; }
        }

        public string SaveLead(eSaveLead eSaveLead)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                strReturn = BL_BulkBid.SaveLead(((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId, Convert.ToInt32(Session["PropId"]), string.IsNullOrEmpty(eSaveLead.dates) ? "" : eSaveLead.dates, string.IsNullOrEmpty(eSaveLead.CloseOut) ? "" : eSaveLead.CloseOut, string.IsNullOrEmpty(eSaveLead.CTA) ? "" : eSaveLead.CTA, string.IsNullOrEmpty(eSaveLead.CTD) ? "" : eSaveLead.CTD, string.IsNullOrEmpty(eSaveLead.Lead) ? "" : eSaveLead.Lead, eSaveLead.typ);
                result = new { st = 1, msg = strReturn };
            }
            catch (Exception ex)
            {
                result = new { st = 0, msg = "Kindly try after some time." };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }

        public class eSaveLOS
        {
            public string dates { get; set; }
            public string CloseOut { get; set; }
            public string CTA { get; set; }
            public string CTD { get; set; }
            public string LOS { get; set; }
            public string typ { get; set; }
        }

        public string SaveLOS(eSaveLOS eSaveLOS)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                strReturn = BL_BulkBid.SaveLOS(((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId, Convert.ToInt32(Session["PropId"]), string.IsNullOrEmpty(eSaveLOS.dates) ? "" : eSaveLOS.dates, string.IsNullOrEmpty(eSaveLOS.CloseOut) ? "" : eSaveLOS.CloseOut, string.IsNullOrEmpty(eSaveLOS.CTA) ? "" : eSaveLOS.CTA, string.IsNullOrEmpty(eSaveLOS.CTD) ? "" : eSaveLOS.CTD, string.IsNullOrEmpty(eSaveLOS.LOS) ? "" : eSaveLOS.LOS, eSaveLOS.typ);
                result = new { st = 1, msg = strReturn };
            }
            catch (Exception ex)
            {
                OneFineRateAppUtil.clsUtils.SendErrorMail(ex);
                result = new { st = 0, msg = "Kindly try after some time." };
            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }

        public class eSaveDays
        {
            public string dates { get; set; }
            public string CloseOut { get; set; }
            public string CTA { get; set; }
            public string CTD { get; set; }
            public string Days { get; set; }
            public string typ { get; set; }
        }

        public string SaveDays(eSaveDays eSaveDays)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                strReturn = BL_BulkBid.SaveWeekend(((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId, Convert.ToInt32(Session["PropId"]), string.IsNullOrEmpty(eSaveDays.dates) ? "" : eSaveDays.dates, string.IsNullOrEmpty(eSaveDays.CloseOut) ? "" : eSaveDays.CloseOut, string.IsNullOrEmpty(eSaveDays.CTA) ? "" : eSaveDays.CTA, string.IsNullOrEmpty(eSaveDays.CTD) ? "" : eSaveDays.CTD, string.IsNullOrEmpty(eSaveDays.Days) ? "" : eSaveDays.Days, eSaveDays.typ);
                result = new { st = 1, msg = strReturn };
            }
            catch (Exception ex)
            {
                result = new { st = 0, msg = "Kindly try after some time." };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }

        public class eSaveRooms
        {
            public string dates { get; set; }
            public string CloseOut { get; set; }
            public string CTA { get; set; }
            public string CTD { get; set; }
            public string Rooms { get; set; }
            public string typ { get; set; }
        }

        public string SaveRooms(eSaveRooms eSaveRooms)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                strReturn = BL_BulkBid.SaveRoom(((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId, Convert.ToInt32(Session["PropId"]), string.IsNullOrEmpty(eSaveRooms.dates) ? "" : eSaveRooms.dates, string.IsNullOrEmpty(eSaveRooms.CloseOut) ? "" : eSaveRooms.CloseOut, string.IsNullOrEmpty(eSaveRooms.CTA) ? "" : eSaveRooms.CTA, string.IsNullOrEmpty(eSaveRooms.CTD) ? "" : eSaveRooms.CTD, string.IsNullOrEmpty(eSaveRooms.Rooms) ? "" : eSaveRooms.Rooms, eSaveRooms.typ);
                result = new { st = 1, msg = strReturn };
            }
            catch (Exception ex)
            {
                result = new { st = 0, msg = "Kindly try after some time." };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }

        public class eSaveBasic
        {
            public string dates { get; set; }
            public string CloseOut { get; set; }
            public string CTA { get; set; }
            public string CTD { get; set; }
            public string Basic { get; set; }
            public string typ { get; set; }
        }

        public string SaveBasic(eSaveBasic eSaveBasic)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                strReturn = BL_BulkBid.SaveBasic(((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId, Convert.ToInt32(Session["PropId"]), string.IsNullOrEmpty(eSaveBasic.dates) ? "" : eSaveBasic.dates, string.IsNullOrEmpty(eSaveBasic.CloseOut) ? "" : eSaveBasic.CloseOut, string.IsNullOrEmpty(eSaveBasic.CTA) ? "" : eSaveBasic.CTA, string.IsNullOrEmpty(eSaveBasic.CTD) ? "" : eSaveBasic.CTD, string.IsNullOrEmpty(eSaveBasic.Basic) ? "" : eSaveBasic.Basic, eSaveBasic.typ);
                result = new { st = 1, msg = strReturn };
            }
            catch (Exception ex)
            {
                result = new { st = 0, msg = "Kindly try after some time." };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }

        public class eSaveForAll
        {
            public string dates { get; set; }
            public string CloseOut { get; set; }
            public string CTA { get; set; }
            public string CTD { get; set; }
            public string typ { get; set; }
        }

        public string SaveForAll(eSaveForAll eSaveForAll)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                strReturn = BL_BulkBid.SaveCTACTDClosedForAllDisc(((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId, Convert.ToInt32(Session["PropId"]), string.IsNullOrEmpty(eSaveForAll.dates) ? "" : eSaveForAll.dates, string.IsNullOrEmpty(eSaveForAll.CloseOut) ? "" : eSaveForAll.CloseOut, string.IsNullOrEmpty(eSaveForAll.CTA) ? "" : eSaveForAll.CTA, string.IsNullOrEmpty(eSaveForAll.CTD) ? "" : eSaveForAll.CTD, eSaveForAll.typ);
                result = new { st = 1, msg = strReturn };
            }
            catch (Exception ex)
            {
                result = new { st = 0, msg = "Kindly try after some time." };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }

        public class eCheckExistingBidDates
        {
            public string dates { get; set; }
        }
        public string CheckExistingBidDates(eCheckExistingBidDates eCheckExistingBidDates)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                strReturn = BL_BulkBid.CheckExistingBidDates(Convert.ToInt32(Session["PropId"]), eCheckExistingBidDates.dates);

                if (strReturn != "a")
                    result = new { st = 1, msg = strReturn };
                else
                    result = new { st = 2, msg = strReturn };

            }
            catch (Exception ex)
            {
                result = new { st = 0, msg = "Kindly try after some time." };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }

        public ActionResult GoToOverview(string StartDate)
        {
            TempData["StartDate"] = StartDate;

            return RedirectToAction("index", "RateInventoryBid");

        }

    }
}