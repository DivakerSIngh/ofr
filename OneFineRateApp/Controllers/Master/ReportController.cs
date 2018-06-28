using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using OneFineRateAppUtil;
using System.Data;
using System.IO;
using ClosedXML.Excel;

namespace OneFineRateApp.Controllers.Master
{
    public class ReportController : BaseController
    {
        // GET: Report
        [Route("Report")]
        public ActionResult Index()
        {
            return View(new eReport());
        }

        public ActionResult ExportExcel(string table, string fromdate, string todate)
        {
            eReport objReport = new eReport();

            try
            {
                var ds = OneFineRateBLL.BL_Report.GetLogReport(table, fromdate, todate);

                using (var wb = new XLWorkbook())
                {
                    var counter = 1;

                    wb.Properties.Title = "Report_" + table;
                    wb.Properties.Company = "OneFineRate";

                    foreach (DataTable dt in ds.Tables)
                    {
                        wb.Worksheets.Add(dt, "Report_" + table + "_" + counter++);
                    }

                    foreach (IXLWorksheet workSheet in wb.Worksheets)
                    {
                        foreach (IXLTable tab in workSheet.Tables)
                        {
                            workSheet.Table(tab.Name).ShowAutoFilter = false;
                            workSheet.Columns().AdjustToContents();
                        }
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
                }

            }
            catch (Exception)
            {
                Response.Write("<script>alert('Something went wrong !!!')</script>");
            }

            return View("Index", objReport);
        }
    }
}