using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using ClosedXML.Excel;
using System.Net.Mail;

using System.Data.SqlClient;
using FutureSoft.Util;
namespace TripAdvisorMonthlyReport
{
    class Program
    {
        private static void ExportDatasetToExcel(DataSet ds)
        {
            using (var wb = new XLWorkbook())
            {
                var counter = 1;

                wb.Properties.Title = "Monthly Activity Report.";
                wb.Properties.Company = "OneFineRate";

                foreach (DataTable dt in ds.Tables)
                {
                    if (counter == 1) //data to be exported is in first table
                    {
                        wb.Worksheets.Add(dt, "Report_" + "MonthlyReport" + "_" + counter++);
                    }
                }

                foreach (IXLWorksheet workSheet in wb.Worksheets)
                {
                    foreach (IXLTable tab in workSheet.Tables)
                    {
                        workSheet.Table(tab.Name).ShowAutoFilter = false;
                        workSheet.Columns().AdjustToContents();
                    }
                }

                wb.SaveAs(AppDomain.CurrentDomain.BaseDirectory + "TripAdvisorMonthlyReport/MonthlyReport.xlsx");
            }
        }
        static void Main(string[] args)
        {
            try
            {
                string body = "", subject = "";
                // Gets Data from tblBookingTripAdvisorReviewTx
                DataSet dt = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "[uspTripAdvisorMonthlyReport]");
                if (dt != null && dt.Tables.Count > 0 && dt.Tables[0] != null && dt.Tables[0].Rows.Count > 0)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "TripAdvisorMonthlyReport";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }


                    body = "Hello,<br/><br/>Attached herewith the Monthly report (" + dt.Tables[1].Rows[0]["MonthYear"].ToString() + ") for review solicitation of OneFineRate.<br/><br/>Thank you,<br/>OneFineRate Team";
                    //subject = string.Format("Monthly report ({0}, {1})  for review solicitation of OneFineRate.", string.Format("{0:MMM}", DateTime.Now), string.Format("{0:yy}", DateTime.Now));
                    subject = "Monthly report (" + dt.Tables[1].Rows[0]["MonthYear"].ToString() + ") for review solicitation of OneFineRate.";
                    ExportDatasetToExcel(dt);
                    var fileStream = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "TripAdvisorMonthlyReport/MonthlyReport.xlsx", FileMode.Open, FileAccess.Read);
                    var displayName = "TripAdvisorMonthlyReport.xlsx";
                    var attachment = new Attachment(fileStream, displayName);
                    SqlParameter[] MyParam = new SqlParameter[1];
                    MyParam[0] = new SqlParameter("@sModule", "TripAdvisor");
                    DataSet dataset = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "[GetEmailByModule]", MyParam);
                    if (dataset.Tables[0] != null && dataset.Tables[0].Rows.Count > 0)
                    {
                        MailComponent.SendEmail(dataset.Tables[0].Rows[0]["Email"].ToString(), "", "", subject, body, attachment, null, true, fileStream, displayName);
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
