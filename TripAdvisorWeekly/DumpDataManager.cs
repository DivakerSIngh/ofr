using ClosedXML.Excel;
using FutureSoft.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace TripAdvisorWeekly
{
   public class DumpDataManager
    {
        #region private variables

        string sErrorEmails = string.Empty;
       
       
        StringBuilder overllStatusBuilder = new StringBuilder();

        #endregion

       public void SendMail(string body)
       {
           Task.Run(() => { MailComponent.SendEmail(this.sErrorEmails, "", "", "TravelGuru Sync Status", body, null, null, false, null, null); });
       }
        /// <summary>
        /// Download data and wait for completion.
        /// </summary>
        /// <param name="sourceUrl"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public void DownloadAndProcessData(string sourceUrl, string userName, string password)
        {
            using (WebClient webClientDownloader = new WebClient())
            {
                try
                {

                    // webClientDownloader_DownloadDataCompleted(null, null); // Used for testing to read data from Local
                    AutoResetEvent waiter = new AutoResetEvent(false);
                    webClientDownloader.Credentials = new NetworkCredential(userName, password);
                    // webClientDownloader.DownloadProgressChanged += webClientDownloader_DownloadProgressChanged;
                    webClientDownloader.DownloadDataCompleted += new DownloadDataCompletedEventHandler(webClientDownloader_DownloadDataCompleted);
                    webClientDownloader.DownloadDataAsync(new Uri(sourceUrl), waiter);
                    waiter.WaitOne();
                }
                catch (WebException ex)
                {
                    throw ex;
                }
            }
        }
        /// <summary>
        /// This method will called once the file download process has been completed !
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void webClientDownloader_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {

            #region Read data from Stream and assign to private static variables

            try
            {
              
                if (e == null && e.Result == null)
                {
                   // SendMail("Downloaded data from travelGuru failed ! , Data null \n");
                    return;
                }

                //SendMail(string.Format("Download completed at {0} \n\n", DateTime.Now));
                overllStatusBuilder.Append(string.Format("Download completed at {0} \n\n", DateTime.Now));

                using (Stream data = new MemoryStream(e.Result))
                {
                    try
                    {
                        using (ZipArchive archive = new ZipArchive(data, ZipArchiveMode.Read))
                        {
                            foreach (var entry in archive.Entries)
                            {
                                Property xmlFeed;
                                using (var stream = entry.Open())
                                {
                                    using (var reader = new StreamReader(stream, Encoding.Default, true))
                                    {
                                        XmlSerializer serializer = new XmlSerializer(typeof(Property));
                                        xmlFeed = (Property)serializer.Deserialize(reader);
                                    }
                                    RunStoreProcedure(xmlFeed);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        
                        throw;
                    }
                }
            }
            //catch (DbEntityValidationException entityValidationEx)
            //{
            //    var exceptionStringBuilder = new StringBuilder();
            //    foreach (var eve in entityValidationEx.EntityValidationErrors)
            //    {
            //        exceptionStringBuilder.Append(string.Format("\n\nEntity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //            eve.Entry.Entity.GetType().Name, eve.Entry.State));
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            exceptionStringBuilder.Append(string.Format("- Property: \"{0}\", Error: \"{1}\"",
            //                ve.PropertyName, ve.ErrorMessage));
            //        }
            //    }

            //  SendMail(exceptionStringBuilder.ToString());

            //    throw entityValidationEx;
            //}
            catch (Exception ex)
            {
                throw ex;
            }

            #endregion Read data from Stream and assign to private static variables
        
        }
        public void RunStoreProcedure(Property xmlFeed)
        {
            try
            {

                // Update IsJointlyCollected in tblTripAdvisorReviews uspUpdatebIsJointlyCollected
                DataTable TripAdvisorReviewbIsJointlyCollected = new DataTable();
                DataColumn col3 = null;
                col3 = new DataColumn("iTripAdvisorLOCID", typeof(Int32));
                TripAdvisorReviewbIsJointlyCollected.Columns.Add(col3);
                col3 = new DataColumn("ReviewId", typeof(Int32));
                TripAdvisorReviewbIsJointlyCollected.Columns.Add(col3);
                col3 = new DataColumn("status", typeof(String));
                TripAdvisorReviewbIsJointlyCollected.Columns.Add(col3);
                DataRow dr = TripAdvisorReviewbIsJointlyCollected.NewRow();
                dr["iTripAdvisorLOCID"] = Convert.ToInt32(xmlFeed.Id);
                dr["ReviewId"] = Convert.ToInt32(xmlFeed.Reviews.Review.Id);
                dr["status"] = xmlFeed.Reviews.Review.ModerationStatus;
                SqlParameter[] MyParam = new SqlParameter[1];
                MyParam[0] = new SqlParameter("@TripAdvisorReviewbIsJointlyCollected", TripAdvisorReviewbIsJointlyCollected);
                MyParam[0].TypeName = "[dbo].[TripAdvisorReviewbIsJointlyCollected]";
                SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspUpdatebIsJointlyCollected", MyParam);

            }
            catch (Exception ex)
            {
                SendMail(ex.ToString());
            }
        }
        public static void ExportDataTableToExcel(DataTable ds,string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (var wb = new XLWorkbook())
            {
                var counter = 1;

                wb.Properties.Title = "Post-Trip Email Report.";
                wb.Properties.Company = "OneFineRate";

                //foreach (DataTable dt in ds.Tables)
                //{
                 wb.Worksheets.Add(ds, "Report_" + "MonthlyReport" + "_" + counter++);
                //}

                foreach (IXLWorksheet workSheet in wb.Worksheets)
                {
                    foreach (IXLTable tab in workSheet.Tables)
                    {
                        workSheet.Table(tab.Name).ShowAutoFilter = false;
                        workSheet.Columns().AdjustToContents();
                    }
                }

                wb.SaveAs(AppDomain.CurrentDomain.BaseDirectory + "PostTipEmailsData/Post-TripEmails.xlsx");
            }
        }
       
    }
}

