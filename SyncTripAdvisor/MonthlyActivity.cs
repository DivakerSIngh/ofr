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
using FutureSoft.Util;
using System.Data.SqlClient;
namespace SyncTripAdvisor
{
    class MonthlyActivity
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
                    wb.Worksheets.Add(dt, "Report_" + "MonthlyReport" + "_" + counter++);
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
        static void Main()
        {
            try
            {
                string body = "Hello,</br></br></br>Attached herewith the Monthly report (May'17) for review solicitation of OneFineRate.";
                string subject = string.Format("Monthly report ({0}, {1})  for review solicitation of OneFineRate.", string.Format("{0:MMM}", DateTime.Now), string.Format("{0:yy}", DateTime.Now));           
                DataSet dt = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "[uspTripAdvisorMonthlyReport]");
                if (dt.Tables[0] != null && dt.Tables[0].Rows.Count > 0)
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "TripAdvisorMonthlyReport";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    ExportDatasetToExcel(dt);
                    var fileStream = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "TripAdvisorMonthlyReport/MonthlyReport.xlsx", FileMode.Open, FileAccess.Read);
                    var displayName = "TripAdvisorMonthlyReport.xlsx";
                    var attachment = new Attachment(fileStream, displayName);
                    SqlParameter[] MyParam = new SqlParameter[1];
                    MyParam[0] = new SqlParameter("@sModule", "TripAdvisor");
                    DataSet dataset = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "[dbo].[GetEmailByModule]", MyParam);
                    if (dataset.Tables[0]!=null&&dataset.Tables[0].Rows.Count>0)
                    {  
                        MailComponent.SendEmail("kuldeepk@futuresoftindia.com", "", "",subject, body, attachment, null, true);
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
