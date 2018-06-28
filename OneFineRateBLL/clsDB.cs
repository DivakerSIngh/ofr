using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{

    public class clsDB
    {
        SqlConnection objConn = default(SqlConnection);
        public DataSet ListBidInventory(int propid, string date)
        {

            try
            {
                DataSet ds = new DataSet();
                objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
                objConn.Open();

                System.Data.SqlClient.SqlParameter[] MyParams = new System.Data.SqlClient.SqlParameter[2];
                MyParams[0] = new System.Data.SqlClient.SqlParameter("@PropId", propid);
                MyParams[1] = new System.Data.SqlClient.SqlParameter("@dtStart", date);

                ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspGetRatePlanOverViewBid", MyParams);

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objConn.Dispose();
            }
        }
        public DataSet ListReminders(int propid, string date)
        {

            try
            {
                DataSet ds = new DataSet();
                objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
                objConn.Open();

                System.Data.SqlClient.SqlParameter[] MyParams = new System.Data.SqlClient.SqlParameter[2];
                MyParams[0] = new System.Data.SqlClient.SqlParameter("@PropId", propid);
                MyParams[1] = new System.Data.SqlClient.SqlParameter("@dtStart", date);

                ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspGetRatePlanOverView", MyParams);

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objConn.Dispose();
            }
        }
        public DataSet ListPropertyDashboard(int propid)
        {

            try
            {
                DataSet ds = new DataSet();
                objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
                objConn.Open();

                System.Data.SqlClient.SqlParameter[] MyParams = new System.Data.SqlClient.SqlParameter[1];
                MyParams[0] = new System.Data.SqlClient.SqlParameter("@PropId", propid);

                ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspGetBestPossibleRatesForDashBoard", MyParams);

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objConn.Dispose();
            }
        }

        public DataSet ListSimulatorData(int propid, string BookingDate, string ArrivalDate, string LOS, string Rooms, string Occupancy, int Type)
        {
            try
            {
                DataSet ds = new DataSet();
                objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
                objConn.Open();

                System.Data.SqlClient.SqlParameter[] MyParams = new System.Data.SqlClient.SqlParameter[6];
                MyParams[0] = new System.Data.SqlClient.SqlParameter("@PropId", propid);
                MyParams[1] = new System.Data.SqlClient.SqlParameter("@dtArrival", ArrivalDate);
                MyParams[2] = new System.Data.SqlClient.SqlParameter("@dtBooking", BookingDate);
                MyParams[3] = new System.Data.SqlClient.SqlParameter("@LOS", LOS);
                MyParams[4] = new System.Data.SqlClient.SqlParameter("@Rooms", Rooms);
                MyParams[5] = new System.Data.SqlClient.SqlParameter("@Occupancy", Occupancy);
                var procName = Type == 1 ? "uspGetBidSimulateData" : Type == 2 ? "uspGetBidSimulateDataCorp" : "";
                ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, procName, MyParams);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objConn.Dispose();
            }
        }

        public DataSet ListSimulatorDataV2(int propid, string BookingDate, string ArrivalDate, string LOS, string Rooms, string Occupancy, int Type)
        {
            try
            {
                DataSet ds = new DataSet();
                using (var objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString()))
                {
                    objConn.Open();

                    System.Data.SqlClient.SqlParameter[] MyParams = new System.Data.SqlClient.SqlParameter[7];
                    MyParams[0] = new System.Data.SqlClient.SqlParameter("@PropId", propid);
                    MyParams[1] = new System.Data.SqlClient.SqlParameter("@dtArrival", ArrivalDate);
                    MyParams[2] = new System.Data.SqlClient.SqlParameter("@dtBooking", BookingDate);
                    MyParams[3] = new System.Data.SqlClient.SqlParameter("@LOS", LOS);
                    MyParams[4] = new System.Data.SqlClient.SqlParameter("@Rooms", Rooms);
                    MyParams[5] = new System.Data.SqlClient.SqlParameter("@Occupancy", Occupancy);
                    MyParams[6] = new System.Data.SqlClient.SqlParameter("@type", Type);
                    ds = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspGetBidSimulateDataCorpPublic ", MyParams);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objConn.Dispose();
            }
        }


        public DataTable GetBidLoadRateList(int propId)
        {
            try
            {
                DataTable dt = new DataTable();
                objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
                objConn.Open();

                System.Data.SqlClient.SqlParameter[] MyParams = new System.Data.SqlClient.SqlParameter[1];
                MyParams[0] = new System.Data.SqlClient.SqlParameter("@PropId", propId);

                dt = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspGetBidLoadRateList", MyParams).Tables[0];

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objConn.Dispose();
            }
        }

        public DataTable GetBidLoadRateUpgradeList(int propId, string ValidFrom, string ValidTo)
        {
            try
            {
                DataTable dt = new DataTable();
                objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
                objConn.Open();

                System.Data.SqlClient.SqlParameter[] MyParams = new System.Data.SqlClient.SqlParameter[3];
                MyParams[0] = new System.Data.SqlClient.SqlParameter("@PropId", propId);
                MyParams[1] = new System.Data.SqlClient.SqlParameter("@dtValidFrom", ValidFrom);
                MyParams[2] = new System.Data.SqlClient.SqlParameter("@dtValidTo", ValidTo);

                dt = SqlHelper.ExecuteDataset(objConn, CommandType.StoredProcedure, "uspGetBidLoadRateUpgradeList", MyParams).Tables[0];

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objConn.Dispose();
            }
        }

    }
}