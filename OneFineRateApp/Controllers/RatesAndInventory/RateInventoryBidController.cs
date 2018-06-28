using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateBLL.Entities;
using OneFineRateBLL;
using System.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;
using OneFineRateAppUtil;
using System.IO;
using ClosedXML.Excel;

namespace OneFineRateApp.Controllers.RatesAndInventory
{
    public class RateInventoryBidController : BaseController
    {
        public static string CONNECTION_STRING = string.Empty;
        // GET: RateInventoryBid
        [Route("RateInventoryBid/Index")]
        [Route("RateInventoryBid")]
        public ActionResult Index()
        {
            if (TempData["StartDate"] != null && TempData["StartDate"].ToString().Trim() != "")
                ViewData["StartDate"] = TempData["StartDate"];
            else
                ViewData["StartDate"] = "";

            return View();
        }
        public string BindGrid(string cdate)
        {
            object result = null;
            string strReturn = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                ds = BL_tblPropertyBasicBiddingMap.GetDataforBidRateInventory(Convert.ToInt32(Session["PropId"]), cdate);

                DataTable dtdays = ds.Tables[1];
                DataTable dtvalue = ds.Tables[0];
                DataTable dtrooms = ds.Tables[2];

                string days = getweekdaysJson(dtdays);
                string values = dtvalue.Rows[0]["A"].ToString();
                string rooms = getroomsJson(dtrooms);
                int roomcount = dtrooms.Rows.Count + 11;

                result = new { st = 1, rooms = rooms, days = days, values = values, count = roomcount };
            }
            catch (Exception)
            {
            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }
        #region Fetch Data for bidding popups
        public ActionResult LeadTime(string date, string typ, string pctype)
        {
            PropertyBiddingMapForOverview lst = new PropertyBiddingMapForOverview();
            etblPropertyLeadTimeBiddingMap obj = new etblPropertyLeadTimeBiddingMap();
            lst = BL_tblPropertyLeadTimeBiddingMap.GetRecords(clsUtils.ConvertyyyymmddtoDateTime(date), Convert.ToInt32(Session["PropId"]), Convert.ToInt32(typ),pctype);
            if (lst.Self.Count > 0)
            {
                obj.JSONData = OneFineRateAppUtil.clsUtils.ConvertToJson(lst.Self);
                obj.JSONDataOther = OneFineRateAppUtil.clsUtils.ConvertToJson(lst.Other);
                obj.bCTA = lst.Self[0].bCTA;
                obj.bCTD = lst.Self[0].bCTD;
                obj.bIsClosed = lst.Self[0].bIsClosed;
                obj.dtEffectiveDate = clsUtils.ConvertyyyymmddtoDateTime(date);
                obj.IsPublic = lst.IsPublic;
                if (obj.dtEffectiveDate != null)
                {
                    obj.EffectiveDate = String.Format("{0:dd/MM/yyyy}", obj.dtEffectiveDate);
                }
            }
            else
            {
                obj.dtEffectiveDate = clsUtils.ConvertyyyymmddtoDateTime(date);
                obj.EffectiveDate = String.Format("{0:dd/MM/yyyy}", obj.dtEffectiveDate);
            }


            return PartialView("_PropertyLeadTimeBiddingMap", obj);
        }
        public ActionResult LOS(string date, string typ, string pctype)
        {
            PropertyBiddingMapForOverview lst = new PropertyBiddingMapForOverview();
            etblPropertyLOSBiddingMap obj = new etblPropertyLOSBiddingMap();
            lst = BL_tblPropertyLOSBiddingMap.GetRecords(clsUtils.ConvertyyyymmddtoDateTime(date), Convert.ToInt32(Session["PropId"]), Convert.ToInt32(typ),pctype);
            if (lst.Self.Count > 0)
            {
                obj.JSONData = OneFineRateAppUtil.clsUtils.ConvertToJson(lst.Self);
                obj.JSONDataOther = OneFineRateAppUtil.clsUtils.ConvertToJson(lst.Other);
                obj.bCTA = lst.Self[0].bCTA;
                obj.bCTD = lst.Self[0].bCTD;
                obj.bIsClosed = lst.Self[0].bIsClosed;
                obj.dtEffectiveDate = clsUtils.ConvertyyyymmddtoDateTime(date);
                obj.IsPublic = lst.IsPublic;
                if (obj.dtEffectiveDate != null)
                {
                    obj.EffectiveDate = String.Format("{0:dd/MM/yyyy}", obj.dtEffectiveDate);
                }
            }
            else
            {
                obj.dtEffectiveDate = clsUtils.ConvertyyyymmddtoDateTime(date);
                obj.EffectiveDate = String.Format("{0:dd/MM/yyyy}", obj.dtEffectiveDate);
            }


            return PartialView("_PropertyLOSBiddingMap", obj);
        }
        public ActionResult Rooms(string date, string typ, string pctype)
        {
            PropertyBiddingMapForOverview lst = new PropertyBiddingMapForOverview();
            etblPropertyRoomsBiddingMap obj = new etblPropertyRoomsBiddingMap();
            lst = BL_tblPropertyRoomsBiddingMap.GetRecords(clsUtils.ConvertyyyymmddtoDateTime(date), Convert.ToInt32(Session["PropId"]), Convert.ToInt32(typ),pctype);
            if (lst.Self.Count > 0)
            {
                obj.JSONData = OneFineRateAppUtil.clsUtils.ConvertToJson(lst.Self);
                obj.JSONDataOther = OneFineRateAppUtil.clsUtils.ConvertToJson(lst.Other);
                obj.bCTA = lst.Self[0].bCTA;
                obj.bCTD = lst.Self[0].bCTD;
                obj.bIsClosed = lst.Self[0].bIsClosed;
                obj.dtEffectiveDate = clsUtils.ConvertyyyymmddtoDateTime(date);
                obj.IsPublic = lst.IsPublic;
                if (obj.dtEffectiveDate != null)
                {
                    obj.EffectiveDate = String.Format("{0:dd/MM/yyyy}", obj.dtEffectiveDate);
                }
            }
            else
            {
                obj.dtEffectiveDate = clsUtils.ConvertyyyymmddtoDateTime(date);
                obj.EffectiveDate = String.Format("{0:dd/MM/yyyy}", obj.dtEffectiveDate);
            }


            return PartialView("_PropertyRoomsBiddingMap", obj);
        }
        public ActionResult Week(string date, string type, string pctype)
        {

            etblPropertyWeekendBiddingMapForOverview obj = new etblPropertyWeekendBiddingMapForOverview();
            obj = BL_tblPropertyWeekendBiddingMap.GetSingleRecordById(clsUtils.ConvertyyyymmddtoDateTime(date), Convert.ToInt32(Session["PropId"]), Convert.ToInt32(type), pctype);

            if (obj.Self.bWeekend != null)
            {
                obj.Self.EffectiveDate = String.Format("{0:dd/MM/yyyy}", obj.Self.dtEffectiveDate);
            }
            else
            {
                obj.Self.dtEffectiveDate = clsUtils.ConvertyyyymmddtoDateTime(date);
                obj.Self.EffectiveDate = String.Format("{0:dd/MM/yyyy}", obj.Self.dtEffectiveDate);
            }

            return PartialView("_PropertyWeekendBiddingMap", obj);
        }
        public ActionResult Basic(string date, string typ, string pctype)
        {
            etblPropertyBasicBiddingMapForOverview obj = new etblPropertyBasicBiddingMapForOverview();
            obj = BL_tblPropertyBasicBiddingMap.GetSingleRecordById(clsUtils.ConvertyyyymmddtoDateTime(date), Convert.ToInt32(Session["PropId"]), Convert.ToInt32(typ),pctype);
            //if (obj.dtEffectiveDate != null)
            //{
            //    obj.EffectiveDate = String.Format("{0:dd/MM/yyyy}", obj.dtEffectiveDate);
            //}
            //else
            //{
            obj.Self.dtEffectiveDate = clsUtils.ConvertyyyymmddtoDateTime(date);
            obj.Self.EffectiveDate = String.Format("{0:dd/MM/yyyy}", obj.Self.dtEffectiveDate);
            //}

            return PartialView("_PropertyBasicBiddingMap", obj);
        }
        public ActionResult Updates(string date, string id)
        {
            etblPropertyBidUpgradeM obj = new etblPropertyBidUpgradeM();
            obj = BL_tblPropertyBiddingRateM.GetSingleRecordById(clsUtils.ConvertyyyymmddtoDateTime(date), Convert.ToInt32(Session["PropId"]), Convert.ToInt32(id));
            if (obj.dtEffectiveDate != null)
            {
                ViewData["Date"] = String.Format("{0:dd/MM/yyyy}", obj.dtEffectiveDate);
            }
            else
            {
                obj.dtEffectiveDate = clsUtils.ConvertyyyymmddtoDateTime(date);
                ViewData["Date"] = String.Format("{0:dd/MM/yyyy}", obj.dtEffectiveDate);
            }

            return PartialView("_PropertyBidUpgradeM", obj);
        }
        #endregion
        #region Post Data for different biddings
        [HttpPost]
        public ActionResult UpdateLeadTimeBidding(etblPropertyLeadTimeBiddingMap eObj)
        {
            object result = null;
            try
            {
                if (ModelState.IsValid)
                {
                    if (eObj.dtEffectiveDate.Date < DateTime.Now.AddDays(-7).Date)
                    {
                        result = new { st = 0, msg = "Please provide lead time discounts for this date in Bidding => Bulk Update screen first as this screen can only update discounts." };
                    }
                    else if (eObj.dtEffectiveDate.Date >= DateTime.Now.AddDays(-7).Date && eObj.dtEffectiveDate.Date < DateTime.Now.Date)
                    {
                        result = new { st = 0, msg = "Data for past dates cannot be provided." };
                    }
                    else
                    {
                        bool bValid = true;
                        eObj.iPropId = Convert.ToInt32(Session["PropId"]);
                        eObj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;

                        DataTable BidRange = new DataTable();
                        DataColumn col1 = null;
                        col1 = new DataColumn("dtEffectiveDate", typeof(DateTime));
                        BidRange.Columns.Add(col1);
                        col1 = new DataColumn("iDays", typeof(short));
                        BidRange.Columns.Add(col1);
                        col1 = new DataColumn("dDiscount", typeof(Decimal));
                        BidRange.Columns.Add(col1);
                        col1 = new DataColumn("iAmenityId1", typeof(Int32));
                        BidRange.Columns.Add(col1);
                        col1 = new DataColumn("iApplicabilityId1", typeof(short));
                        BidRange.Columns.Add(col1);
                        col1 = new DataColumn("iAmenityId2", typeof(Int32));
                        BidRange.Columns.Add(col1);
                        col1 = new DataColumn("iApplicabilityId2", typeof(short));
                        BidRange.Columns.Add(col1);

                        if (eObj.SelectedDiscounts != null)
                        {
                            decimal discount = 0;
                            JArray jArray = (JArray)JsonConvert.DeserializeObject(eObj.SelectedDiscounts.Replace("\\", "\""));
                            if (jArray != null)
                            {
                                foreach (var item in jArray)
                                {
                                    if (Convert.ToInt32(item["to"]) == 0)
                                        bValid = false;
                                    for (int i = Convert.ToInt32(item["from"]); i <= Convert.ToInt32(item["to"]); i++)
                                    {
                                        discount = string.IsNullOrEmpty(item["disc"].ToString()) ? Convert.ToDecimal("0") : Convert.ToDecimal(item["disc"]);
                                        DataRow dr = BidRange.NewRow();
                                        dr["dtEffectiveDate"] = eObj.dtEffectiveDate;
                                        dr["iDays"] = i;
                                        dr["dDiscount"] = discount;
                                        dr["iAmenityId1"] = Convert.ToInt32(item["amen1"]);
                                        dr["iApplicabilityId1"] = Convert.ToInt16(item["app1"]);
                                        dr["iAmenityId2"] = Convert.ToInt32(item["amen2"]);
                                        dr["iApplicabilityId2"] = Convert.ToInt16(item["app2"]);
                                        BidRange.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        if (bValid)
                        {
                            int j = BL_tblPropertyLeadTimeBiddingMap.AddUpdateRecord(eObj, BidRange);
                            if (j == 1)
                            {
                                result = new { st = 1, msg = "Updated successfully." };
                            }
                            else
                            {
                                result = new { st = 0, msg = "Kindly try after some time." };
                            }
                        }
                        else
                        {
                            result = new { st = 0, msg = "Please provide lead time discounts for this date in Bidding => Bulk Update screen first as this screen can only update discounts." };
                        }
                    }
                }
                else
                {
                    string errormsg = "";
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            errormsg += error.ErrorMessage;
                            errormsg += "</br>";
                        }
                    }

                    result = new { st = 0, msg = errormsg };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = "Kindly try after some time." };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateLOSBidding(etblPropertyLOSBiddingMap eObj)
        {
            object result = null;
            try
            {
                if (ModelState.IsValid)
                {
                    if (eObj.dtEffectiveDate.Date < DateTime.Now.AddDays(-7).Date)
                    {
                        result = new { st = 0, msg = "Please provide length of stay discounts for this date in Bidding => Bulk Update screen first as this screen can only update discounts." };
                    }
                    else if (eObj.dtEffectiveDate.Date >= DateTime.Now.AddDays(-7).Date && eObj.dtEffectiveDate.Date < DateTime.Now.Date)
                    {
                        result = new { st = 0, msg = "Data for past dates cannot be provided." };
                    }
                    else
                    {
                        bool bValid = true;
                        eObj.iPropId = Convert.ToInt32(Session["PropId"]);
                        eObj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;

                        DataTable BidRange = new DataTable();
                        DataColumn col1 = null;
                        col1 = new DataColumn("dtEffectiveDate", typeof(DateTime));
                        BidRange.Columns.Add(col1);
                        col1 = new DataColumn("iDays", typeof(short));
                        BidRange.Columns.Add(col1);
                        col1 = new DataColumn("dDiscount", typeof(Decimal));
                        BidRange.Columns.Add(col1);
                        col1 = new DataColumn("iAmenityId1", typeof(Int32));
                        BidRange.Columns.Add(col1);
                        col1 = new DataColumn("iApplicabilityId1", typeof(short));
                        BidRange.Columns.Add(col1);
                        col1 = new DataColumn("iAmenityId2", typeof(Int32));
                        BidRange.Columns.Add(col1);
                        col1 = new DataColumn("iApplicabilityId2", typeof(short));
                        BidRange.Columns.Add(col1);

                        if (eObj.SelectedDiscounts != null)
                        {
                            decimal discount = 0;
                            JArray jArray = (JArray)JsonConvert.DeserializeObject(eObj.SelectedDiscounts.Replace("\\", "\""));
                            if (jArray != null)
                            {
                                foreach (var item in jArray)
                                {
                                    if (Convert.ToInt32(item["to"]) == 0)
                                        bValid = false;
                                    for (int i = Convert.ToInt32(item["from"]); i <= Convert.ToInt32(item["to"]); i++)
                                    {
                                        discount = string.IsNullOrEmpty(item["disc"].ToString()) ? Convert.ToDecimal("0") : Convert.ToDecimal(item["disc"]);
                                        DataRow dr = BidRange.NewRow();
                                        dr["dtEffectiveDate"] = eObj.dtEffectiveDate;
                                        dr["iDays"] = i;
                                        dr["dDiscount"] = discount;
                                        dr["iAmenityId1"] = Convert.ToInt32(item["amen1"]);
                                        dr["iApplicabilityId1"] = Convert.ToInt16(item["app1"]);
                                        dr["iAmenityId2"] = Convert.ToInt32(item["amen2"]);
                                        dr["iApplicabilityId2"] = Convert.ToInt16(item["app2"]);
                                        BidRange.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        if (bValid)
                        {
                            int j = BL_tblPropertyLOSBiddingMap.AddUpdateRecord(eObj, BidRange);
                            if (j == 1)
                            {
                                result = new { st = 1, msg = "Updated successfully." };
                            }
                            else
                            {
                                result = new { st = 0, msg = "Kindly try after some time." };
                            }
                        }
                        else
                            result = new { st = 0, msg = "Please provide length of stay discounts for this date in Bidding => Bulk Update screen first as this screen can only update discounts." };
                    }
                }
                else
                {
                    string errormsg = "";
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            errormsg += error.ErrorMessage;
                            errormsg += "</br>";
                        }
                    }

                    result = new { st = 0, msg = errormsg };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = "Kindly try after some time." };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateRoomsBidding(etblPropertyRoomsBiddingMap eObj)
        {
            object result = null;
            try
            {
                if (ModelState.IsValid)
                {
                    if (eObj.dtEffectiveDate.Date < DateTime.Now.AddDays(-7).Date)
                    {
                        result = new { st = 0, msg = "Please provide rooms discounts for this date in Bidding => Bulk Update screen first as this screen can only update discounts." };
                    }
                    else if (eObj.dtEffectiveDate.Date >= DateTime.Now.AddDays(-7).Date && eObj.dtEffectiveDate.Date < DateTime.Now.Date)
                    {
                        result = new { st = 0, msg = "Data for past dates cannot be provided." };
                    }
                    else
                    {
                        bool bValid = true;
                        eObj.iPropId = Convert.ToInt32(Session["PropId"]);
                        eObj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;

                        DataTable BidRange = new DataTable();
                        DataColumn col1 = null;
                        col1 = new DataColumn("dtEffectiveDate", typeof(DateTime));
                        BidRange.Columns.Add(col1);
                        col1 = new DataColumn("iDays", typeof(short));
                        BidRange.Columns.Add(col1);
                        col1 = new DataColumn("dDiscount", typeof(Decimal));
                        BidRange.Columns.Add(col1);
                        col1 = new DataColumn("iAmenityId1", typeof(Int32));
                        BidRange.Columns.Add(col1);
                        col1 = new DataColumn("iApplicabilityId1", typeof(short));
                        BidRange.Columns.Add(col1);
                        col1 = new DataColumn("iAmenityId2", typeof(Int32));
                        BidRange.Columns.Add(col1);
                        col1 = new DataColumn("iApplicabilityId2", typeof(short));
                        BidRange.Columns.Add(col1);

                        if (eObj.SelectedDiscounts != null)
                        {
                            decimal discount = 0;
                            JArray jArray = (JArray)JsonConvert.DeserializeObject(eObj.SelectedDiscounts.Replace("\\", "\""));
                            if (jArray != null)
                            {
                                foreach (var item in jArray)
                                {
                                    if (Convert.ToInt32(item["to"]) == 0)
                                        bValid = false;
                                    for (int i = Convert.ToInt32(item["from"]); i <= Convert.ToInt32(item["to"]); i++)
                                    {
                                        discount = string.IsNullOrEmpty(item["disc"].ToString()) ? Convert.ToDecimal("0") : Convert.ToDecimal(item["disc"]);
                                        DataRow dr = BidRange.NewRow();
                                        dr["dtEffectiveDate"] = eObj.dtEffectiveDate;
                                        dr["iDays"] = i;
                                        dr["dDiscount"] = discount;
                                        dr["iAmenityId1"] = Convert.ToInt32(item["amen1"]);
                                        dr["iApplicabilityId1"] = Convert.ToInt16(item["app1"]);
                                        dr["iAmenityId2"] = Convert.ToInt32(item["amen2"]);
                                        dr["iApplicabilityId2"] = Convert.ToInt16(item["app2"]);
                                        BidRange.Rows.Add(dr);
                                    }
                                }
                            }
                        }
                        if (bValid)
                        {
                            int j = BL_tblPropertyRoomsBiddingMap.AddUpdateRecord(eObj, BidRange);
                            if (j == 1)
                            {
                                result = new { st = 1, msg = "Updated successfully." };
                            }
                            else
                            {
                                result = new { st = 0, msg = "Kindly try after some time." };
                            }
                        }
                        else
                            result = new { st = 0, msg = "Please provide rooms discounts for this date in Bidding => Bulk Update screen first as this screen can only update discounts." };
                    }
                }
                else
                {
                    string errormsg = "";
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            errormsg += error.ErrorMessage;
                            errormsg += "</br>";
                        }
                    }

                    result = new { st = 0, msg = errormsg };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = "Kindly try after some time." };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateWeekEndWeekDayBidding(etblPropertyWeekendBiddingMapForOverview eObj)
        {
            object result = null;
            try
            {
                if (ModelState.IsValid)
                {
                    if (eObj.Self.dtEffectiveDate.Date < DateTime.Now.AddDays(-7).Date)
                    {
                        result = new { st = 0, msg = "Please provide weekend/weekday discounts for this date in Bidding => Bulk Update screen first as this screen can only update discounts." };
                    }
                    else if (eObj.Self.dtEffectiveDate.Date >= DateTime.Now.AddDays(-7).Date && eObj.Self.dtEffectiveDate.Date < DateTime.Now.Date)
                    {
                        result = new { st = 0, msg = "Data for past dates cannot be provided." };
                    }
                    else if (eObj.Self.bWeekend == null)
                    {
                        result = new { st = 0, msg = "Please provide weekend/weekday discounts for this date in Bidding => Bulk Update screen first as this screen can only update discounts." };
                    }
                    else
                    {
                        eObj.Self.iPropId = Convert.ToInt32(Session["PropId"]);
                        eObj.Self.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                        eObj.Self.dtActionDate = DateTime.Now;

                        int j = BL_tblPropertyWeekendBiddingMap.UpdateRecord(eObj);
                        if (j == 1)
                        {
                            result = new { st = 1, msg = "Updated successfully." };
                        }
                        else
                        {
                            result = new { st = 0, msg = "Kindly try after some time." };
                        }
                    }
                }
                else
                {
                    string errormsg = "";
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            errormsg += error.ErrorMessage;
                            errormsg += "</br>";
                        }
                    }

                    result = new { st = 0, msg = errormsg };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = "Kindly try after some time." };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateBasicBidding(etblPropertyBasicBiddingMapForOverview eObj)
        {
            object result = null;
            try
            {
                if (ModelState.IsValid)
                {
                    if (eObj.Self.dtEffectiveDate.Date < DateTime.Now.AddDays(-7).Date)
                    {
                        result = new { st = 0, msg = "Please provide discounts for this date and discount type in Bidding => Bulk Update screen first as this screen can only update discounts." };
                    }
                    else if (eObj.Self.dtEffectiveDate.Date >= DateTime.Now.AddDays(-7).Date && eObj.Self.dtEffectiveDate.Date < DateTime.Now.Date)
                    {
                        result = new { st = 0, msg = "Data for past dates cannot be provided." };
                    }
                    else if (eObj.Self.dBasicDiscount > 100)
                    {
                        result = new { st = 0, msg = "Base discount cannot be more than 100." };
                    }
                    else
                    {
                        eObj.Self.iPropId = Convert.ToInt32(Session["PropId"]);
                        eObj.Self.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                        eObj.Self.dtActionDate = DateTime.Now;

                        int j = BL_tblPropertyBasicBiddingMap.UpdateRecord(eObj);
                        if (j == 1)
                        {
                            result = new { st = 1, msg = "Updated successfully." };
                        }
                        else
                        {
                            result = new { st = 0, msg = "Kindly try after some time." };
                        }
                    }
                }
                else
                {
                    string errormsg = "";
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            errormsg += error.ErrorMessage;
                            errormsg += "</br>";
                        }
                    }

                    result = new { st = 0, msg = errormsg };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = "Kindly try after some time." };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateUpgrade(etblPropertyBidUpgradeM eObj)
        {
            object result = null;
            try
            {
                if (ModelState.IsValid)
                {
                    if (eObj.dtEffectiveDate.Date >= DateTime.Now.AddDays(-7).Date && eObj.dtEffectiveDate.Date < DateTime.Now.Date)
                    {
                        result = new { st = 0, msg = "Data for past dates cannot be provided." };
                    }
                    eObj.iPropId = Convert.ToInt32(Session["PropId"]);
                    eObj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                    eObj.dtActionDate = DateTime.Now;

                    int j = BL_tblPropertyBiddingRateM.UpdateRecord(eObj);
                    if (j == 1)
                    {
                        result = new { st = 1, msg = "Updated successfully." };
                    }
                    else
                    {
                        result = new { st = 0, msg = "Kindly try after some time." };
                    }
                }
                else
                {
                    string errormsg = "";
                    foreach (ModelState modelState in ViewData.ModelState.Values)
                    {
                        foreach (ModelError error in modelState.Errors)
                        {
                            errormsg += error.ErrorMessage;
                            errormsg += "</br>";
                        }
                    }

                    result = new { st = 0, msg = errormsg };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = "Kindly try after some time." };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion
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
            catch (Exception)
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
            catch (Exception)
            {
                result = new { st = 0, msg = "Kindly try after some time." };

            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }
        public static string getweekdaysJson(DataTable dt)
        {
            StringBuilder str = new StringBuilder();
            str.Append("[");
            foreach (DataRow dr in dt.Rows)
            {
                str.Append("{");
                string jsonval = "";
                jsonval = "\"name\" : \"" + dr[0] + "\" ,";
                jsonval += "\"cdate\" : \"" + dr[1] + "\" ,";
                jsonval += "\"myear\" : \"" + dr[2] + "\" ";
                str.Append(jsonval);
                str.Append("},");
            }
            str.Replace(',', ']', str.Length - 1, 1);
            return str.ToString();
        }
        public static string getroomsJson(DataTable dt)
        {
            StringBuilder str = new StringBuilder();
            str.Append("[");
            foreach (DataRow dr in dt.Rows)
            {
                str.Append("{");
                string jsonval = "";
                jsonval = "\"name\" : \"" + dr[1].ToString() + "\" ,";
                jsonval += "\"id\" : \"_" + dr[0].ToString() + "\" ";
                str.Append(jsonval);
                str.Append("},");
            }
            str.Replace(',', ']', str.Length - 1, 1);

            if (str.ToString() == "[")
                str.Append("]");
            return str.ToString();
        }
        public string UpdateCloseOut(string data, string startdate, bool type)
        {
            object result = null;
            string strReturn = string.Empty;
            try
            {
                DateTime StartDate = OneFineRateAppUtil.clsUtils.ConvertmmddyyyytoDateTime(startdate);
                DateTime EndDate = StartDate.AddDays(14);

                DataTable BidDatesWithType = new DataTable();
                DataColumn col = null;
                col = new DataColumn("dtEffectiveDate", typeof(DateTime));
                BidDatesWithType.Columns.Add(col);
                col = new DataColumn("cType", typeof(Int64));
                BidDatesWithType.Columns.Add(col);

                DataTable BidDatesWithTypeForRoom = new DataTable();
                DataColumn col1 = null;
                col1 = new DataColumn("dtEffectiveDate", typeof(DateTime));
                BidDatesWithTypeForRoom.Columns.Add(col1);
                col1 = new DataColumn("cType", typeof(Int64));
                BidDatesWithTypeForRoom.Columns.Add(col1);
                JArray jArray = (JArray)JsonConvert.DeserializeObject(data.Replace("\\", "\""));

                //var res = new List<string>();
                //for (var date = StartDate; date <= EndDate; date = date.AddDays(1))
                //    res.Add(date.ToString());

                //if (jArray != null)
                //{
                //    foreach (var ddate in res)
                //    {
                //        foreach (var item in jArray)
                //        {
                //            DataRow dr = BidDatesWithType.NewRow();
                //            dr["dtEffectiveDate"] = ddate;
                //            dr["cType"] = Convert.ToInt32(item["Type"]);
                //            BidDatesWithType.Rows.Add(dr);

                //        }
                //    }
                //}


                if (jArray != null)
                {
                    for (var date = StartDate; date <= EndDate; date = date.AddDays(1))
                    {
                        foreach (var item in jArray)
                        {
                            if (item["Type"].ToString().Contains("_"))
                            {
                                DataRow dr = BidDatesWithTypeForRoom.NewRow();
                                dr["dtEffectiveDate"] = date.ToString();
                                dr["cType"] = Convert.ToInt32(item["Type"].ToString().Split('_')[1]);
                                BidDatesWithTypeForRoom.Rows.Add(dr);
                            }
                            else
                            {
                                DataRow dr = BidDatesWithType.NewRow();
                                dr["dtEffectiveDate"] = date.ToString();
                                dr["cType"] = Convert.ToInt32(item["Type"]);
                                BidDatesWithType.Rows.Add(dr);
                            }
                        }
                    }
                }


                int j = BL_tblPropertyBasicBiddingMap.UpdateAllBidRecord(Convert.ToInt32(Session["PropId"]), ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId, BidDatesWithType, BidDatesWithTypeForRoom, type);
                if (j == 1)
                {
                    result = new { st = 1, msg = "Closed Out successfully." };
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
        public string GetSheetName(int i)
        {
            string sheetname = "";
            try
            {
                switch (i)
                {
                    case 0:
                        sheetname = "Room";
                        break;
                    case 1:
                        sheetname = "Base + Closeout";
                        break;
                    case 2:
                        sheetname = "Lead Time";
                        break;
                    case 3:
                        sheetname = "LOS";
                        break;
                    case 4:
                        sheetname = "Multiple Rooms";
                        break;
                    case 5:
                        sheetname = "Weekend-Weekday";
                        break;
                    case 6:
                        sheetname = "Upgrade Charges";
                        break;
                    default:
                        sheetname = "";
                        break;
                }
            }
            catch
            {

            }
            return sheetname;
        }
        public ActionResult ExportExcel()
        {
            DataSet ds = BL_bulk.GetBiddingDump(Convert.ToInt32(Session["PropId"]));
            XLWorkbook wb = null;
            try
            {
                using (wb = new XLWorkbook())
                {
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        wb.Worksheets.Add(ds.Tables[i], GetSheetName(i));

                        foreach (IXLWorksheet workSheet in wb.Worksheets)
                        {
                            foreach (IXLTable tab in workSheet.Tables)
                            {
                                workSheet.Table(tab.Name).ShowAutoFilter = false;
                                workSheet.Columns().AdjustToContents();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                Response.Write("<script>alert('Something went wrong !!!')</script>");
            }

            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=Report.xlsx");
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                wb.SaveAs(MyMemoryStream);
                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
                System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            if (TempData["StartDate"] != null && TempData["StartDate"].ToString().Trim() != "")
                ViewData["StartDate"] = TempData["StartDate"];
            else
                ViewData["StartDate"] = "";

            return View("Index");
        }
    }
}