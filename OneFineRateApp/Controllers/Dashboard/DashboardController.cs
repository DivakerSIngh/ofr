using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateBLL.Entities;
using OneFineRateBLL;
using System.Text;

namespace OneFineRateApp.Controllers.Dashboard
{
    public class DashboardController : BaseController
    {
        // GET: Dashboard
        [Authorize]
        public ActionResult Dashboard()
        {
            Session["MenuType"] = "2";
            return View();
        }

        [HttpGet]
        public JsonResult BindGrid()
        {
            object result = null;
            string strReturn = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                ds = BL_tblPropertyM.GetDataforPropertyDashboard(Convert.ToInt32(Session["PropId"].ToString()));

                DataTable dtrooms = ds.Tables[1];
                DataTable dtdays = ds.Tables[0];
                DataTable dtvalue = ds.Tables[2];

                string rooms = dtrooms.Rows[0]["A"].ToString();
                string days = getweekdaysJson(dtdays);
                string values = dtvalue.Rows[0]["A"].ToString();
                result = new { st = 1, rooms = rooms, days = days, values = values };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        //public string BindGrid()
        //{
        //    object result = null;
        //    string strReturn = string.Empty;
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        ds = BL_tblPropertyM.GetDataforPropertyDashboard(Convert.ToInt32(Session["PropId"].ToString()));

        //        DataTable dtrooms = ds.Tables[1];
        //        DataTable dtdays = ds.Tables[0];
        //        DataTable dtvalue = ds.Tables[2];

        //        string rooms = dtrooms.Rows[0]["A"].ToString();
        //        string days = getweekdaysJson(dtdays);
        //        string values = dtvalue.Rows[0]["A"].ToString();
        //        result = new { st = 1, rooms = rooms, days = days, values = values };

        //    }
        //    catch (Exception)
        //    {
        //    }
        //    strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
        //    return strReturn;
        //}
        public string FetchNegCount()
        {
            object result = null;
            string strReturn = string.Empty;
            try
            {
                int CNT = BL_tblPropertyM.GetPendingChangesCount(Convert.ToInt32(Session["PropId"].ToString()));
                result = new { st = 1, CNT = CNT};
            }
            catch (Exception)
            {
            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }


        [HttpGet]
        public JsonResult GetHotelRank()
        {
            try
            {
                int iPropId = Convert.ToInt32(Session["PropId"].ToString());
                var data = BL_tblPropertyM.GetHotelRank(iPropId);
                return Json(new { data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { data = new HotelRank() }, JsonRequestBehavior.AllowGet);
            }
        }
        public static string getweekdaysJson(DataTable dt)
        {
            StringBuilder str = new StringBuilder();
            str.Append("[");
            foreach (DataRow dr in dt.Rows)
            {
                str.Append("{");
                string jsonval = "";
                jsonval = "\"name\" : \"" + dr[0] + "\"";
                str.Append(jsonval);
                str.Append("},");
            }
            str.Replace(',', ']', str.Length - 1, 1);
            return str.ToString();
        }

        public PartialViewResult GetNotifications()
        {
            eDashBoardNotifications obj = new eDashBoardNotifications();
            obj= BL_Dashboard.GetDashBoardNotifications(Convert.ToInt32(Session["PropId"].ToString()));

            return PartialView("pNotifications", obj);
        }


    }
}