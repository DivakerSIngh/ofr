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
    public class RoomRatePlanController : Controller
    {
        // GET: RoomRatePlan
        [Route("RoomRatePlan")]
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult BindRatePlans(string id)
        {
            int cid = int.Parse(id);
            List<PNames> results = BL_tblPropertyRatePlanMap.GetRatePlanRoomWise(Convert.ToInt32(id), Convert.ToInt32(Session["PropId"].ToString()));
            return Json(new
            {
                suggestions = results
            }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult AddUpdate(eRoomRatePlan_Ph eObj)
        {
            string strReturn = string.Empty;
            object result = null;
            try
            {
                if (ModelState.IsValid)
                {

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

                    strReturn = BL_bulk.SaveInventoryRoomRatePlan_Ph(((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId, Convert.ToInt32(Session["PropId"].ToString()), eObj.Action, DateRoomPlan);
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