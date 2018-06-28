using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace OneFineRateApp.Controllers.RatesAndInventory
{
    public class BulkUpdateController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s">Start date</param>
        /// <param name="e">End date</param>
        /// <returns></returns>
        [Route("BulkUpdate")]
        public ActionResult Index(string s, string e)
        {
            ViewBag.StartDate = s;
            ViewBag.EndDate = e;
            return View();
        }

        public string GetRoomAndRatePlan(string OnlyBase)
        {
            object result = null;
            string strReturn = string.Empty;

            try
            {
                strReturn = BL_bulk.GetRoomAndRatePlanForBulk(Convert.ToInt32(Session["PropId"]), Convert.ToBoolean(OnlyBase));
                result = new { st = 1, msg = strReturn };
            }
            catch (Exception ex)
            {
                result = new { st = 0, msg = "Kindly try after some time." };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }

        public class edata
        {
            public string dates { get; set; } 
            public string roomid { get; set; }
            public string plan { get; set; }
            public string Inv { get; set; }
            public string StopSell { get; set; }
            public string CutOff { get; set; }
            public string CloseOut { get; set; }
            public string Min { get; set; }
            public string Max { get; set; }
            public string CTA { get; set; }
            public string CTD { get; set; }
            public string single { get; set; }
            public string doble { get; set; }
            public string triple { get; set; }
            public string quad { get; set; }
            public string quin { get; set; }
        }
      
        public string SaveInventory(edata obj)
        {

            //string dates = "", roomid = "", plan = "", Inv = "", StopSell = "", CutOff = "", CloseOut = "", Min = "", Max = "", CTA = "", CTD = "", single = "", doble = "", triple = "", quad = "", quin = "";
            object result = null;
            string strReturn = string.Empty;

            try
            {
                strReturn = BL_bulk.SaveInventory(((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId, Convert.ToInt32(Session["PropId"]), string.IsNullOrEmpty(obj.dates) ? "" : obj.dates, string.IsNullOrEmpty(obj.roomid) ? "" : obj.roomid, string.IsNullOrEmpty(obj.plan) ? "" : obj.plan, string.IsNullOrEmpty(obj.Inv) ? "" : obj.Inv, string.IsNullOrEmpty(obj.StopSell) ? "" : obj.StopSell, string.IsNullOrEmpty(obj.CutOff) ? "" : obj.CutOff, string.IsNullOrEmpty(obj.CloseOut) ? "" : obj.CloseOut, string.IsNullOrEmpty(obj.Min) ? "" : obj.Min, string.IsNullOrEmpty(obj.Max) ? "" : obj.Max, string.IsNullOrEmpty(obj.CTA) ? "" : obj.CTA, string.IsNullOrEmpty(obj.CTD) ? "" : obj.CTD, string.IsNullOrEmpty(obj.single) ? "" : obj.single, string.IsNullOrEmpty(obj.doble) ? "" : obj.doble, string.IsNullOrEmpty(obj.triple) ? "" : obj.triple, string.IsNullOrEmpty(obj.quad) ? "" : obj.quad, string.IsNullOrEmpty(obj.quin) ? "" : obj.quin);
                if (strReturn == "a"){
                    result = new { st = 1, msg = strReturn };
                }
                else
                {
                    result = new { st = 2, msg = strReturn };
                }
                
            }
            catch (Exception ex)
            {
                OneFineRateAppUtil.clsUtils.SendErrorMail(ex);
                result = new { st = 0, msg = "Kindly try after some time." };
            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;            
        }

        public ActionResult GoToOverview(string StartDate)
        {
            TempData["StartDate"] = StartDate;

            return RedirectToAction("index", "RateInventory");

        }
    }
}