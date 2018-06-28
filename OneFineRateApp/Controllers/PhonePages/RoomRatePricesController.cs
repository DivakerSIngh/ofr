using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using OneFineRateAppUtil;
using System.Data;

namespace OneFineRateApp.Controllers.PhonePages
{
    public class RoomRatePricesController : Controller
    {
        // GET: RoomRatePrices
        [Route("RoomRatePrices")]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddUpdate(eRoomRatePrice_Ph eObj)
        {
            string strReturn = string.Empty;
            object result = null;
            try
            {
                if (ModelState.IsValid)
                {
                    if((eObj.SingleRate==null||eObj.SingleRate==0)&&(eObj.DoubleRate == null || eObj.DoubleRate == 0) &&(eObj.TripleRate == null || eObj.TripleRate == 0) && (eObj.QuadrupleRate == null || eObj.QuadrupleRate == 0) && (eObj.QuintrupleRate == null || eObj.QuintrupleRate == 0))
                    {
                        result = new { st = 0, msg = "Please enter at least one Rate." };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    eObj.dtFrom = clsUtils.ConvertddmmyyyytoDateTime(eObj.FromDate);
                    eObj.dtTo = clsUtils.ConvertddmmyyyytoDateTime(eObj.ToDate);

                    var res = new List<string>();
                    for (var date = eObj.dtFrom; date <= eObj.dtTo; date = date.AddDays(1))
                        res.Add(date.ToString());


                    DataTable DateRoomPlan = new DataTable();
                    DataColumn col1 = null;
                    col1 = new DataColumn("dtInventoryDate", typeof(DateTime));
                    DateRoomPlan.Columns.Add(col1);
                    col1 = new DataColumn("iRoomId", typeof(Int64));
                    DateRoomPlan.Columns.Add(col1);
                    col1 = new DataColumn("iRPId", typeof(Int32));
                    DateRoomPlan.Columns.Add(col1);


                    foreach (var ddate in res)
                    {
                        DataRow drDateRoom = DateRoomPlan.NewRow();
                        drDateRoom["dtInventoryDate"] = ddate;
                        drDateRoom["iRoomId"] = eObj.RoomId;
                        drDateRoom["iRPId"] = eObj.PlanId;
                        DateRoomPlan.Rows.Add(drDateRoom);
                    }

                    Decimal single = eObj.SingleRate.ToString() == "" ? Convert.ToDecimal("0") : Convert.ToDecimal(eObj.SingleRate);
                    Decimal doble = eObj.DoubleRate.ToString() == "" ? Convert.ToDecimal("0") : Convert.ToDecimal(eObj.DoubleRate);
                    Decimal triple = eObj.TripleRate.ToString() == "" ? Convert.ToDecimal("0") : Convert.ToDecimal(eObj.TripleRate);
                    Decimal quad = eObj.QuadrupleRate.ToString() == "" ? Convert.ToDecimal("0") : Convert.ToDecimal(eObj.QuadrupleRate);
                    Decimal quin = eObj.QuintrupleRate.ToString() == "" ? Convert.ToDecimal("0") : Convert.ToDecimal(eObj.QuintrupleRate);

                    strReturn = BL_bulk.SaveInventoryRoomRatePlanPrices_Ph(((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId, Convert.ToInt32(Session["PropId"].ToString()), DateRoomPlan, single, doble, triple, quad, quin);
                    if (strReturn == "a")
                    {
                        result = new { st = 1, msg = strReturn };
                    }
                    else
                    {
                        result = new { st = 2, msg = strReturn };
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
                result = new { st = 0, msg = "Kindly try after some time" };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}