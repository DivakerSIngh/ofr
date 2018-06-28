using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OFRChannelMgr.DB_Manager
{
    public class clsUtil
    {
        public bool IsAuthenticatedRateGain(string UserName, string Password)
        {
            if (UserName == ConfigurationManager.AppSettings["RageGainUserName"].ToString() && Password == ConfigurationManager.AppSettings["RageGainPassword"].ToString())
            {
                return true;
            }
            return false;
        }
        public Int16 ConvertOccupancyCode(string Occ)
        {
            Int16 iOcc = 2;
            switch (Occ)
            {
                case "A1":
                    iOcc = 1;
                    break;
                case "A2":
                    iOcc = 2;
                    break;
                case "A3":
                    iOcc = 3;
                    break;
                case "A4":
                    iOcc = 4;
                    break;
                case "A5":
                    iOcc = 5;
                    break;
                default:
                    iOcc = 2;
                    break;
            }
            return iOcc;
        }
        public Int64 UpdateChannelMgrTracker(Int64 iID, int iChannelMgrId, string sReqMsg)
        {
            SqlParameter[] MyParam = new SqlParameter[3];
            MyParam[0] = new SqlParameter("@iID", iID);
            MyParam[1] = new SqlParameter("@iChannelMgrId", iChannelMgrId);
            MyParam[2] = new SqlParameter("@sReqMsg", sReqMsg);
            DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspUpdateChannelMgrTracker", MyParam);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt64(ds.Tables[0].Rows[0][0].ToString());
            }
            return 0;
        }
    }
}