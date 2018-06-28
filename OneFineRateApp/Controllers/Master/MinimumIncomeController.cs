using ClosedXML.Excel;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateApp.Controllers.Master
{
    public class MinimumIncomeController : BaseController
    {
        [Route("MinimumIncome")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveMinimumIncome(string selectedPropertyIds, decimal minimumIncome)
        {
            try
            {
                if (!string.IsNullOrEmpty(selectedPropertyIds))
                {
                    var actionById = ((BL_Login.UserDetails)Session["UserDetails"]).iUserId;

                    var result = BL_MinimumIncome.SaveMinimumIncome(selectedPropertyIds, minimumIncome, actionById);

                    return Json(new { status = result.Key == 1, message = result.Value }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = false, message = "Please select at least one property to set minimum income." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = "An error occurred while updating the record!" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult GetHotelSearchData(string hotelSearchTxt)
        {
            var hotelList = BL_tblPropertyM.GetAllPropertyNameList(hotelSearchTxt);

            return Json(new { hotelList }, JsonRequestBehavior.AllowGet);
        }


        public void ExportToExcel(string propertyIds)
        {
            try
            {
                if (string.IsNullOrEmpty(propertyIds))
                {
                    throw new Exception("Property Ids can not be null or empty when exporting to excel.");
                }

                SqlParameter[] MyParam = new SqlParameter[1];
                MyParam[0] = new SqlParameter("@sPropIds", propertyIds);

                DataTable dt = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(),
                    CommandType.StoredProcedure, "uspGetMinIncomeListToExport", MyParam).Tables[0];

                using (var wb = new XLWorkbook())
                {
                    wb.Properties.Title = "MinimumIncomeReport";
                    wb.Properties.Company = "OneFineRate";
                    wb.Worksheets.Add(dt, "MinimumIncomeReport1");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=MinimumIncomeReport.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                        System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Something went wrong !!! Details : " + ex.Message + " ')</script>");
            }
        }
    }
}