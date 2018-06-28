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
    public class InventoryPlanController : Controller
    {
        // GET: InventoryPlan
        [Route("InventoryPlan")]
        public ActionResult Index()
        {

            return View();
        }
        [HttpPost]
        public ActionResult AddUpdate(eInventoryPlan_Ph eObj)
        {
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

                    List<PNames> RoomIds = new List<PNames>();
                    DataTable DateRoom = new DataTable();
                    DataColumn col = null;
                    col = new DataColumn("dtInventoryDate", typeof(DateTime));
                    DateRoom.Columns.Add(col);
                    col = new DataColumn("iRoomId", typeof(Int64));
                    DateRoom.Columns.Add(col);
                    int PropId=0;
                    if (Session["PropId"] != null)
                    {
                        PropId = Convert.ToInt32(Session["PropId"]);
                    }
                    if (eObj.RoomType == true)
                    {
                        RoomIds = BL_tblPropertyRoomMap.GetAllPropertyRoomNames(PropId);
                        foreach (var ddate in res)
                        {
                            foreach (var item in RoomIds)
                            {
                                DataRow drDateRoom = DateRoom.NewRow();
                                drDateRoom["dtInventoryDate"] = ddate;
                                drDateRoom["iRoomId"] = item.Id;
                                DateRoom.Rows.Add(drDateRoom);
                            }
                        }
                    }
                    else
                    {
                        foreach (var ddate in res)
                        {
                            DataRow drDateRoom = DateRoom.NewRow();
                            drDateRoom["dtInventoryDate"] = ddate;
                            drDateRoom["iRoomId"] = eObj.RoomId;
                            DateRoom.Rows.Add(drDateRoom);
                        }
                    }

                    int i = BL_bulk.SaveInventoryPlan_Ph(((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId, Convert.ToInt32(Session["PropId"].ToString()), eObj.Action, DateRoom);
                    if (i == 1)
                    {
                        result = new { st = 1, msg = "Updated successfully" };
                    }
                    else
                    {
                        result = new { st = 0, msg = "Kindly try after some time" };
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