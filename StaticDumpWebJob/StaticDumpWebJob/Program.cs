using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using StaticDumpWebJob.DataManagers;
using System.Configuration;
using log4net.Config;
using FutureSoft.Util;
using System.Data;
using StaticDumpWebJob.DatabaseContext;

namespace StaticDumpWebJob
{

    public class Program
    {
        //Log4net
        //static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static void Main(string[] args)
        {

            string sErrorEmail = "deepaka@futuresoftindia.com,himanshuS@futuresoftindia.com";

            try { sErrorEmail = SqlHelper.ExecuteScalar(ConfigurationManager.ConnectionStrings["OFR_DataContext"].ToString(), CommandType.Text, "SELECT TOP 1 sEmail FROM tblEmailSettingsM WHERE sModule = 'TravelGuruSync'").ToString(); } catch (Exception) { }

            try
            {
                //Log4net configuration
                //BasicConfigurator.Configure();

                string staticDumpUrl = ConfigurationManager.AppSettings["StaticTextDumpUrl"].ToString();
                string userName = ConfigurationManager.AppSettings["UserName"].ToString();
                string password = ConfigurationManager.AppSettings["Password"].ToString();

                //For Testing Purpose
                //System.Threading.Thread.Sleep(1000 * 60 * 3);
                
                new DumpDataManager(sErrorEmail).DownloadAndProcessData(sourceUrl: staticDumpUrl, userName: userName, password: password);
            }
            catch (Exception ex)
            {
                try
                {
                    MailComponent.SendEmail(sErrorEmail, "", "", "TravelGuru Sync Error", ex.ToString(), null, null, false, null, null);
                }
                catch (Exception exMail)
                {
                    LogExceptionDetails(exMail);
                }

                LogExceptionDetails(ex);
            }
        }

        public static void LogExceptionDetails(Exception ex)
        {
            try
            {
                var repository = new Repository<tblErrorLog>();

                repository.Insert(new tblErrorLog
                {
                    dtDate = DateTime.Now,
                    iId = -1,
                    sAction = "TravelGuru Sync Error",
                    sController = "TravelGuru Dump scheduler",
                    sError = ex.Message
                });
            }
            catch (Exception)
            {
                // can not log again, leave it...
            }
        }
    }
}
