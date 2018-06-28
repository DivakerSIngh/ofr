using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateBLL.Entities;
using OneFineRateBLL;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;
using OneFineRateAppUtil;
using System.Text;

namespace OneFineRateApp.Controllers.RatesAndInventory
{
    public class LoadBiddingRateController : BaseController
    {
        // GET: LoadBiddingRate
        [Route("LoadBiddingRate")]
        public ActionResult Index()
        {
            etblPropertyBiddingRateM obj = new etblPropertyBiddingRateM();
            ViewBag.rooms = BL_tblPropertyRoomMap.GetAllPropertyRoomNamesCurrency(Convert.ToInt32(Session["PropId"]));
            return View(obj);
        }

        public JsonResult GetUpgradeRooms(string Room, string rateplan)
        {
            if (rateplan == "")
                rateplan = "0";
            if (Room == "")
                Room = "0";
            List<PropertyRooms> results = BL_tblPropertyRatePlanMap.GetUpgradeRooms(Convert.ToInt32(Room), Convert.ToInt32(rateplan), Convert.ToInt32(Session["PropId"]));
            return Json(new
            {
                suggestions = results
            }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult BindRatePlans(string id)
        {
            int cid = int.Parse(id);
            List<PNames> results = BL_tblPropertyRatePlanMap.GetRatePlanRoomWise(cid, Convert.ToInt32(Session["PropId"]));
            return Json(new
            {
                suggestions = results
            }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult AddUpdate(etblPropertyBiddingRateM eObj)
        {
            object result = null;
            try
            {
                if (ModelState.IsValid)
                {

                    eObj.iPropId = Convert.ToInt32(Session["PropId"]);
                    eObj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;

                    DateTime datefrom = clsUtils.ConvertddmmyyyytoDateTime(eObj.datefrom);
                    DateTime dateto = clsUtils.ConvertddmmyyyytoDateTime(eObj.dateto);

                    int RPID = (int)eObj.iRPId;
                    int RoomId = (int)eObj.iRoomId;

                    string ValidCheck = BL_tblPropertyBiddingRateM.CheckRatePlanValidity(RPID, datefrom, dateto);

                    if (ValidCheck != "a")
                    {
                        result = new { st = 0, msg = ValidCheck };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    string ValidCheck2 = BL_tblPropertyBiddingRateM.CheckRoomRatePlanPrice( RoomId, RPID, datefrom, dateto);

                    if (ValidCheck2 != "a")
                    {
                        result = new { st = 0, msg = ValidCheck2 };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                    var res = new List<string>();
                    for (var date = datefrom; date <= dateto; date = date.AddDays(1))
                        res.Add(date.ToString());

                    DataTable PropertyBiddingRateM = new DataTable();
                    DataColumn col1 = null;
                    col1 = new DataColumn("dtEffectiveDate", typeof(DateTime));
                    PropertyBiddingRateM.Columns.Add(col1);
                    col1 = new DataColumn("iRoomId", typeof(Int64));
                    PropertyBiddingRateM.Columns.Add(col1);
                    col1 = new DataColumn("iRPId", typeof(Int32));
                    PropertyBiddingRateM.Columns.Add(col1);

                    DataTable PropertyBidUpgradeM = new DataTable();
                    DataColumn col = null;
                    col = new DataColumn("dtEffectiveDate", typeof(DateTime));
                    PropertyBidUpgradeM.Columns.Add(col);
                    col = new DataColumn("iRoomId", typeof(Int64));
                    PropertyBidUpgradeM.Columns.Add(col);
                    col = new DataColumn("dUpgradeCharge", typeof(Decimal));
                    PropertyBidUpgradeM.Columns.Add(col);
                    col = new DataColumn("sUpgradeType", typeof(string));
                    PropertyBidUpgradeM.Columns.Add(col);                    

                    foreach (var item in res)
                    {
                        DataRow dr = PropertyBiddingRateM.NewRow();
                        dr["dtEffectiveDate"] = item;
                        dr["iRoomId"] = eObj.iRoomId;
                        dr["iRPId"] = eObj.iRPId;
                        PropertyBiddingRateM.Rows.Add(dr);
                    }

                    if (eObj.selectedRoomPrices != null)
                    {
                        JArray jArray = (JArray)JsonConvert.DeserializeObject(eObj.selectedRoomPrices.Replace("\\", "\""));
                        if (jArray != null)
                        {
                            foreach (var ddate in res)
                            {
                                foreach (var item in jArray)
                                {
                                    if (item["value"].ToString().Trim() != "")
                                    {
                                        try
                                        {
                                            if (Convert.ToDecimal(item["value"]) > 0)
                                            {
                                                try
                                                {
                                                    int a = Convert.ToInt32(item["value"]);
                                                }
                                                catch (Exception)
                                                {
                                                    result = new { st = 0, msg = "Upgrade amount should be a valid integer greater than 1." };
                                                    return Json(result, JsonRequestBehavior.AllowGet);
                                                }
                                                DataRow dr = PropertyBidUpgradeM.NewRow();
                                                dr["dtEffectiveDate"] = ddate;
                                                dr["iRoomId"] = Convert.ToInt64(item["roomid"]);
                                                dr["dUpgradeCharge"] = Convert.ToDecimal(Convert.ToInt32(item["value"]));
                                                dr["sUpgradeType"] = "WIS";
                                                PropertyBidUpgradeM.Rows.Add(dr);
                                            }
                                            else
                                            {
                                                DataRow dr = PropertyBidUpgradeM.NewRow();
                                                dr["dtEffectiveDate"] = ddate;
                                                dr["iRoomId"] = Convert.ToInt64(item["roomid"]);
                                                dr["dUpgradeCharge"] = DBNull.Value;
                                                dr["sUpgradeType"] = "BTB";
                                                PropertyBidUpgradeM.Rows.Add(dr);
                                            }
                                        }
                                        catch (Exception E)
                                        {
                                            result = new { st = 0, msg = "Upgrade amount should be a valid integer greater than 1." };
                                            return Json(result, JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                }
                            }

                        }
                    }
                    int j = BL_tblPropertyBiddingRateM.AddUpdateRecord(eObj, PropertyBiddingRateM, PropertyBidUpgradeM);
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

        public string GetBidLoadRateList()
        {
            string strReturn = string.Empty;
            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();

            try
            {
                strReturn = BL_tblPropertyBiddingRateM.GetBidLoadRateList(Convert.ToInt32(Session["PropId"]));
                return strReturn;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string GetBidLoadRateUpgradeList(string ValidFrom, string ValidTo)
        {
            string strReturn = string.Empty;
            string outputJson = string.Empty;
            StringBuilder sb = new StringBuilder();

            try
            {
                strReturn = BL_tblPropertyBiddingRateM.GetBidLoadRateUpgradeList(Convert.ToInt32(Session["PropId"]), ValidFrom, ValidTo);
                return strReturn;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult GoToOverview() //string StartDate
        {
            //TempData["StartDate"] = StartDate;

            return RedirectToAction("index", "BulkBid");

        }
    }
}