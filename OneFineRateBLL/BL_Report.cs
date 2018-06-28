using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Data.SqlClient;
using OneFineRateAppUtil;
using System.Configuration;
using System.Data;
using FutureSoft.Util;

namespace OneFineRateBLL
{
    public class BL_Report
    {
        public static DataSet GetLogReport(string table, string fromdate, string todate)
        {
            SqlParameter[] MyParam = new SqlParameter[3];
            MyParam[0] = new SqlParameter("@fromdate", fromdate);
            MyParam[1] = new SqlParameter("@todate", todate);
            MyParam[2] = new SqlParameter("@table", table);
            DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetMasterLog", MyParam);
            return ds;
        }

        /// <summary>
        /// This function will send error report to the respective person with complete error logs as provide in parameter.
        /// </summary>
        /// <param name="errorDescription">Complete logs of error that need to send.</param>
        /// <returns>Status of the process as int : 1 means success and -1 means fail</returns>
        public int SendErrorReport(string errorDescription)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var errorSpecificEmails = db.tblEmailSettingsMs.Where(x => x.sModule == EmailSetting.WebSiteError.ToString()).FirstOrDefault();

                if (errorSpecificEmails != null)
                {
                    try
                    {
                        MailComponent.SendEmail(errorSpecificEmails.sEmail, "", "", "Error", errorDescription, null, null, false, null, null);
                        return 1;
                    }
                    catch (Exception)
                    {
                    }
                }
                return -1;
            }
        }


        public enum EmailSetting
        {
            Bidding,
            ChannelMgrError,
            Negotiation,
            Partner,
            Refund,
            RevenueManager,
            TravelGuruConfirmation,
            TravelGuruSync,
            WebSiteError
        }
    }
}