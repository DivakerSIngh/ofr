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
using System.Text;
using OneFineRateAppUtil;
using System.IO;
using ClosedXML.Excel;


namespace OneFineRateApp.Controllers.RatesAndInventory
{
    public class RateInventoryController : BaseController
    {
        public static string CONNECTION_STRING = string.Empty;
        // GET: RateInventory
        [Route("RateInventory")]
        public ActionResult Index()
        {
            if (TempData["StartDate"] != null && TempData["StartDate"].ToString().Trim() != "")
                ViewData["StartDate"] = TempData["StartDate"];
            else
                ViewData["StartDate"] = "";

            return View();
        }
        public ActionResult RoomInventory(string roomid, string date)
        {
            etblPropertyRoomInventory obj = new etblPropertyRoomInventory();
            obj = BL_tblPropertyRoomInventory.GetSingleRecordById(Convert.ToInt32(roomid), clsUtils.ConvertyyyymmddtoDateTime(date));
            if (obj.iPropId != 0)
            {
                if (obj.dtInventoryDate != null)
                {
                    obj.InventoryDate = String.Format("{0:dd/MM/yyyy}", obj.dtInventoryDate);
                }
                if (obj.bStopSell == true)
                {
                    obj.sStopSell = "Yes";
                }
                else
                {
                    obj.sStopSell = "No";
                }
            }
            else
            {
                obj.dtInventoryDate = clsUtils.ConvertyyyymmddtoDateTime(date);
                obj.InventoryDate = String.Format("{0:dd/MM/yyyy}", obj.dtInventoryDate);
                obj.iRoomId = Convert.ToInt64(roomid);
            }



            return PartialView("_RoomInventory", obj);
        }
        public ActionResult RoomInventoryRatePlan(string roomid, string date, string planId)
        {
            etblPropertyRoomRatePlanInventoryMap obj = new etblPropertyRoomRatePlanInventoryMap();
            obj = BL_tblPropertyRoomRatePlanInventoryMap.GetSingleRecordById(Convert.ToInt32(roomid), Convert.ToInt32(planId), clsUtils.ConvertyyyymmddtoDateTime(date));
            if (obj.iPropId != 0)
            {
                if (obj.bCloseOut == true)
                {
                    obj.sCloseOut = "On";
                }
                else
                {
                    obj.sCloseOut = "Off";
                }
                if (obj.bCTA == true)
                {
                    obj.sCTA = "On";
                }
                else
                {
                    obj.sCTA = "Off";
                }
                if (obj.bCTD == true)
                {
                    obj.sCTD = "On";
                }
                else
                {
                    obj.sCTD = "Off";
                }
            }
            obj.iRoomId = Convert.ToInt64(roomid);
            obj.iRPId = Convert.ToInt32(planId);
            obj.dtInventoryDate = clsUtils.ConvertyyyymmddtoDateTime(date);
            return PartialView("_RoomInventoryRatePlan", obj);
        }
        [HttpPost]
        public ActionResult AddUpdate(etblPropertyRoomInventory eObj)
        {
            object result = null;
            try
            {
                if (ModelState.IsValid)
                {
                    eObj.iPropId = Convert.ToInt32(Session["PropId"]);
                    eObj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                    eObj.dtActionDate = DateTime.Now;
                    string s = BL_bulk.SaveInventory(((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId, Convert.ToInt32(Session["PropId"]), eObj.dtInventoryDate.ToString("MM/dd/yyyy").Replace("-", "/"), eObj.iRoomId.ToString(), "", eObj.iAvailableInventory.ToString(), eObj.bStopSell ? "On" : "Off", eObj.iCutOff.ToString(), "", "", "", "", "", "", "", "", "", "");
                    if (s == "a")
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
                    result = new { st = 0, msg = "Kindly try after some time." };
                }
            }
            catch (Exception)
            {
                result = new { st = 0, msg = "Kindly try after some time." };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddUpdateRatePlan(etblPropertyRoomRatePlanInventoryMap eObj)
        {
            object result = null; 
            try
            {
                if (ModelState.IsValid)
                {
                    eObj.iPropId = Convert.ToInt32(Session["PropId"]);
                    eObj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                    eObj.dtActionDate = DateTime.Now;

                    if (eObj.SelectedOccupancies != null)
                    {
                        JArray jArray = (JArray)JsonConvert.DeserializeObject(eObj.SelectedOccupancies.Replace("\\", "\""));
                        if (jArray != null)
                        {
                            List<etblPropertyParkingMap> lstPropertyParkingMap = new List<etblPropertyParkingMap>();
                            foreach (var item in jArray)
                            {
                                if (Convert.ToInt32(item["type"]) == 1)
                                {
                                    eObj.dSingleRate = Convert.ToDecimal(item["value"]);
                                }
                                if (Convert.ToString(item["type"]) == "2")
                                {
                                    eObj.dDoubleRate = Convert.ToDecimal(item["value"]);
                                }
                                if (Convert.ToString(item["type"]) == "3")
                                {
                                    eObj.dTripleRate = Convert.ToDecimal(item["value"]);
                                }
                                if (Convert.ToString(item["type"]) == "4")
                                {
                                    eObj.dQuadrupleRate = Convert.ToDecimal(item["value"]);
                                }
                                if (Convert.ToString(item["type"]) == "5")
                                {
                                    eObj.dQuintrupleRate = Convert.ToDecimal(item["value"]);
                                }

                            }

                        }
                    }
                    string strReturn = BL_bulk.SaveInventory(((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId, Convert.ToInt32(Session["PropId"]), eObj.dtInventoryDate.ToString("MM/dd/yyyy").Replace("-", "/"), eObj.iRoomId.ToString(), eObj.iRoomId.ToString() + "-" + eObj.iRPId.ToString(), "", "", "", eObj.bCloseOut ? "On" : "Off", eObj.iMinLengthStay.ToString(), eObj.iMaxLengthStay.ToString(), eObj.bCTA ? "On" : "Off", eObj.bCTD ? "On" : "Off", eObj.dSingleRate.ToString(), eObj.dDoubleRate.ToString(), eObj.dTripleRate.ToString(), eObj.dQuadrupleRate.ToString(), eObj.dQuintrupleRate.ToString());
                    
                    if (strReturn == "a")
                    {
                        result = new { st = 1, msg = "Updated successfully." };
                    }
                    else if (strReturn.StartsWith("["))
                    {
                        result = new { st = 0, msg = "Rate Plan is not valid for this date." };
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
        public static string getroomsJson(DataTable dt)
        {
            StringBuilder str = new StringBuilder();
            str.Append("[");
            foreach (DataRow dr in dt.Rows)
            {
                str.Append("{");
                string jsonval = "";
                jsonval = "\"name\" : \"" + dr[1] + "\" ,";
                jsonval += "\"type\" : \"" + dr[2] + "\" ,";
                jsonval += "\"ilinked\" : \"" + dr[3] + "\" ,";
                jsonval += "\"irid\" : \"" + dr[4] + "\" ,";
                jsonval += "\"irpid\" : \"" + dr[5] + "\" ";
                str.Append(jsonval);
                str.Append("},");
            }
            str.Replace(',', ']', str.Length - 1, 1);
            return str.ToString();
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

        public string BindGrid(string cdate)
        {
            object result = null;
            string strReturn = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                ds = BL_tblPropertyRoomInventory.GetDataforPropertyRateInventory(Convert.ToInt32(Session["PropId"]), cdate);

                DataTable dtrooms = ds.Tables[0];
                DataTable dtdays = ds.Tables[2];
                DataTable dtvalue = ds.Tables[1];

                if (dtrooms.Rows.Count > 1)
                {
                    string rooms = getroomsJson(dtrooms);
                    string days = getweekdaysJson(dtdays);
                    string values = dtvalue.Rows[0]["A"].ToString();
                    int roomcount = dtrooms.Rows.Count;

                    result = new { st = 1, rooms = rooms, days = days, values = values, count = roomcount };
                }
                else
                {
                    result = new { st = 0, msg = "There are no rooms and rate plans available for this property." };

                }
            }
            catch (Exception)
            {
            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }
        public string UpdateCloseOut(string data, string startdate,bool type)
        {
            object result = null;
            string strReturn = string.Empty;
            try
            {
                DateTime StartDate = OneFineRateAppUtil.clsUtils.ConvertmmddyyyytoDateTime(startdate);
                DateTime EndDate = StartDate.AddDays(14);

                DataTable PropertyRoomInventory = new DataTable();
                DataColumn col = null;
                col = new DataColumn("dtInventoryDate", typeof(DateTime));
                PropertyRoomInventory.Columns.Add(col);
                col = new DataColumn("iRoomId", typeof(Int64));
                PropertyRoomInventory.Columns.Add(col);

                DataTable PropertyRoomRatePlanInventoryMap = new DataTable();
                DataColumn col1 = null;
                col1 = new DataColumn("dtInventoryDate", typeof(DateTime));
                PropertyRoomRatePlanInventoryMap.Columns.Add(col1);
                col1 = new DataColumn("iRoomId", typeof(Int64));
                PropertyRoomRatePlanInventoryMap.Columns.Add(col1);
                col1 = new DataColumn("iRPId", typeof(Int64));
                PropertyRoomRatePlanInventoryMap.Columns.Add(col1);


                var res = new List<string>();
                for (var date = StartDate; date <= EndDate; date = date.AddDays(1))
                    res.Add(date.ToString());

                JArray jArray = (JArray)JsonConvert.DeserializeObject(data.Replace("\\", "\""));
                if (jArray != null)
                {
                    foreach (var ddate in res)
                    {
                        foreach (var item in jArray)
                        {
                            if (item["Type"].ToString() == "R")
                            {
                                DataRow dr = PropertyRoomInventory.NewRow();
                                dr["dtInventoryDate"] = ddate;
                                dr["iRoomId"] = Convert.ToInt64(item["rid"]);
                                PropertyRoomInventory.Rows.Add(dr);

                            }
                            if (item["Type"].ToString() == "P")
                            {
                                DataRow dr = PropertyRoomRatePlanInventoryMap.NewRow();
                                dr["dtInventoryDate"] = ddate;
                                dr["iRoomId"] = Convert.ToInt64(item["rid"]);
                                dr["iRPId"] = Convert.ToInt32(item["rpid"]);
                                PropertyRoomRatePlanInventoryMap.Rows.Add(dr);
                            }
                        }
                    }
                }
                int j = BL_tblPropertyRoomInventory.AddUpdateRecord(Convert.ToInt32(Session["PropId"]), ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId, PropertyRoomInventory, PropertyRoomRatePlanInventoryMap,type);
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
                        sheetname = "Basic + Closeout";
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

    }
}