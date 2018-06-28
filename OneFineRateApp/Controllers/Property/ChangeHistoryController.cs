using ClosedXML.Excel;
using OneFineRateAppUtil;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateApp.Controllers.Property
{
    public class ChangeHistoryController : BaseController
    {
        // GET: ChangeHistory
        public ActionResult Index()
        {
            int iUserID = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
            etblChangeHistory obj = new etblChangeHistory
            {
                PropertyList = new List<SelectListItem> { new SelectListItem { Text = "All Hotels", Value = "0" } }
            };
            obj.PropertyList.AddRange(BL_tblPropertyM.GetUserPropertyList(iUserID));
            return View(obj);
        }
        public ActionResult GetRoomType(int PropId)
        {
            var data = BL_tblChangeHistory.GetRoomTypeForDD(PropId);
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        public ActionResult FillRatePlan(int RoomType, int PropId)
        {
            var rateplan = BL_tblChangeHistory.GetRatePlanByRoomID(RoomType, PropId);
            return Json(rateplan, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPageName(int PropId)
        {
            var data = BL_tblChangeHistory.GetPageNameForDD(PropId);
            return Json(data,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetHistoryChangeDataExcel(int? PropId, string chkradio, string dtfrom, string dtto, string dteffectivefrom, string dteffectiveto, int? id, int? roomType, int? ratePlan)
        {
            DataTable dt = new DataTable();
            DateTime dtFrom = dtfrom == "" ? DateTime.Now : DateTime.ParseExact(dtfrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime dtTo = dtto == "" ? DateTime.Now : DateTime.ParseExact(dtto, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            if (chkradio == "R")
            {
                DateTime dtEffectiveFrom = dteffectivefrom == "" ? DateTime.Now : DateTime.ParseExact(dteffectivefrom, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                DateTime dtEffectiveTo = dteffectiveto == "" ? DateTime.Now : DateTime.ParseExact(dteffectiveto, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                if (PropId != 0)
                {
                    dt = BL_tblChangeHistory.getRecordForSearchExcel(PropId.Value, dtFrom, dtTo, roomType.Value, ratePlan.Value, dtEffectiveFrom, dtEffectiveTo);
                }
                else
                {
                    dt = BL_tblChangeHistory.getRecordForSearchExcel(PropId.Value, dtFrom, dtTo, 0, 0, dtEffectiveFrom, dtEffectiveTo);
                }
            }
            if (chkradio == "I" || chkradio == "B")
            {
                string Id = id.ToString();
                dt = BL_tblChangeHistory.getRecordForSearchExcel(PropId.Value, dtFrom, dtTo, Id);
            }
            if (chkradio == "Promotion")
            {
                string Id = id.ToString();
                dt = BL_tblChangeHistory.getRecordForSearchPromotionsExcel(dtFrom, dtTo, Id, PropId.Value);
            }

            if (chkradio == "RatePlan")
            {
                dt = BL_tblChangeHistory.getRecordForSearchRatePlanExcel(dtFrom, dtTo, PropId.Value);
            }
            DownloadExcel(dt);
            int iUserID = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
            etblChangeHistory obj = new etblChangeHistory
            {
                PropertyList = new List<SelectListItem> { new SelectListItem { Text = "All Hotels", Value = "0" } }
            };
            obj.PropertyList.AddRange(BL_tblPropertyM.GetUserPropertyList(iUserID));
            return View("Index",obj);

        }

        [NonAction]
        public void DownloadExcel(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                XLWorkbook wb = null;
                try
                {
                    using (wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt, "History");

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
                catch (Exception)
                {
                    Response.Write("<script>alert('Something went wrong !!!')</script>");
                }

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=History.xlsx");
                using (System.IO.MemoryStream MyMemoryStream = new System.IO.MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                    System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
            }

        }
    }
}