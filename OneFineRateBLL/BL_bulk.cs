using OneFineRate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using OneFineRateAppUtil;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace OneFineRateBLL.Entities
{
    public class BL_bulk
    {
        public static string GetRoomAndRatePlanForBulk(int iPropId, Boolean OnlyBase)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var MappedPlans = db.uspGetRoomAndRatePlanForBulk(iPropId, OnlyBase);
                    var MappedPlansList = MappedPlans.ToList();
                    return MappedPlansList[0].ToString();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        


        public static string SaveInventory(int iUserId, int PropId, string dates, string roomid, string plans, string Inv, string StopSell, string CutOff, string CloseOut,
            string Min, string Max, string CTA, string CTD, string single, string doble, string triple, string quad, string quin)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {                
                SqlParameter[] MyParam = new SqlParameter[17];
                try
                {
                    string[] Alldates = dates.Split('$');
                    string[] Allroomid = roomid.Split(',');
                    string[] Allplan = plans.Split(',');

                    DataTable DateRoom = new DataTable();
                    DataColumn col = null;
                    col = new DataColumn("dtInventoryDate", typeof(DateTime));
                    DateRoom.Columns.Add(col);
                    col = new DataColumn("iRoomId", typeof(Int64));
                    DateRoom.Columns.Add(col);
                    foreach (string date in Alldates)
                    {   
                        try
                        {
                            foreach (string room in Allroomid)
                            {
                                DataRow drDateRoom = DateRoom.NewRow();
                                drDateRoom["dtInventoryDate"] = OneFineRateAppUtil.clsUtils.ConvertmmddyyyytoDateTime(date);
                                drDateRoom["iRoomId"] = room;
                                DateRoom.Rows.Add(drDateRoom);
                            }
                        }
                        catch (Exception) { }
                    }

                    DataTable DateRoomPlan = new DataTable();
                    DataColumn col1 = null;
                    col1 = new DataColumn("dtInventoryDate", typeof(DateTime));
                    DateRoomPlan.Columns.Add(col1);
                    col1 = new DataColumn("iRoomId", typeof(Int64));
                    DateRoomPlan.Columns.Add(col1);
                    col1 = new DataColumn("iRPId", typeof(Int32));
                    DateRoomPlan.Columns.Add(col1);
                    foreach (string date in Alldates)
                    {
                        foreach (string plan in Allplan)
                        {
                            try
                            {
                                DataRow drDateRoom = DateRoomPlan.NewRow();
                                drDateRoom["dtInventoryDate"] = OneFineRateAppUtil.clsUtils.ConvertmmddyyyytoDateTime(date);
                                string[] splt = plan.Split('-');
                                drDateRoom["iRoomId"] = splt[0];
                                drDateRoom["iRPId"] = splt[1];
                                DateRoomPlan.Rows.Add(drDateRoom);
                            }
                            catch (Exception) { }
                        }
                    }

                    List<InvalidRatePlans> IRP = new List<InvalidRatePlans>();
                    MyParam[0] = new SqlParameter("@DateRoom", DateRoom);
                    MyParam[0].TypeName = "[dbo].[DateRoom]";
                    MyParam[1] = new SqlParameter("@DateRoomPlan", DateRoomPlan);
                    MyParam[1].TypeName = "[dbo].[DateRoomPlan]";
                    MyParam[2] = new SqlParameter("@iPropId", PropId);
                    MyParam[3] = new SqlParameter("@iInventory", Inv == "" ? (object)DBNull.Value : Inv);
                    MyParam[4] = new SqlParameter("@bStopSell", StopSell == "" ? (object)DBNull.Value : (bool?)(StopSell == "On" ? true : false));
                    MyParam[5] = new SqlParameter("@iCutOff", CutOff == "" ? (object)DBNull.Value : CutOff);
                    MyParam[6] = new SqlParameter("@iActionBy", iUserId);
                    MyParam[7] = new SqlParameter("@bCloseOut", CloseOut == "" ? (object)DBNull.Value : (bool?)(CloseOut == "On" ? true : false));
                    MyParam[8] = new SqlParameter("@iMinLengthStay", Min == "" ? (object)DBNull.Value : Min);
                    MyParam[9] = new SqlParameter("@iMaxLengthStay", Max == "" ? (object)DBNull.Value : Max);
                    MyParam[10] = new SqlParameter("@bCTA", CTA == "" ? (object)DBNull.Value : (CTA == "On" ? "1" : "0"));
                    MyParam[11] = new SqlParameter("@bCTD", CTD == "" ? (object)DBNull.Value : (bool?)(CTD == "On" ? true : false));
                    MyParam[12] = new SqlParameter("@dSingleRate", single == "" ? (object)DBNull.Value : single);
                    MyParam[13] = new SqlParameter("@dDoubleRate", doble == "" ? (object)DBNull.Value : doble);
                    MyParam[14] = new SqlParameter("@dTripleRate", triple == "" ? (object)DBNull.Value : triple);
                    MyParam[15] = new SqlParameter("@dQuadrupleRate", quad == "" ? (object)DBNull.Value : quad);
                    MyParam[16] = new SqlParameter("@dQuintrupleRate", quin == "" ? (object)DBNull.Value : quin);
                    //db.Database.ExecuteSqlCommand("uspSaveInventoryDetails @DateRoom, @DateRoomPlan, @iPropId, @iInventory, @bStopSell, @iCutOff, @iActionBy, @bCloseOut, @iMinLengthStay, @iMaxLengthStay, @bCTA, @bCTD, @dSingleRate, @dDoubleRate, @dTripleRate, @dQuadrupleRate, @dQuintrupleRate", MyParam);
                    IRP = db.Database.SqlQuery<InvalidRatePlans>("uspSaveInventoryDetails @DateRoom, @DateRoomPlan, @iPropId, @iInventory, @bStopSell, @iCutOff, @iActionBy, @bCloseOut, @iMinLengthStay, @iMaxLengthStay, @bCTA, @bCTD, @dSingleRate, @dDoubleRate, @dTripleRate, @dQuadrupleRate, @dQuintrupleRate", MyParam).ToList();
                    if (IRP.Count > 0)
                    {
                        //var serializer = new JavaScriptSerializer();
                        var js = JsonConvert.SerializeObject(IRP);
                        return js.ToString();
                    }
                    else
                    {
                        return "a";
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public static int SaveInventoryPlan_Ph(int iUserId, int PropId, bool StopSell, DataTable DateRoom)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                SqlParameter[] MyParam = new SqlParameter[4];
                try
                {
                    MyParam[0] = new SqlParameter("@DateRoom", DateRoom);
                    MyParam[0].TypeName = "[dbo].[DateRoom]";
                    MyParam[1] = new SqlParameter("@iPropId", PropId);
                    MyParam[2] = new SqlParameter("@bStopSell", StopSell);
                    MyParam[3] = new SqlParameter("@iActionBy", iUserId);
                    db.Database.SqlQuery<InvalidRatePlans>("uspSaveInventoryDetails_Ph @DateRoom, @iPropId, @bStopSell, @iActionBy", MyParam).ToList();
                    return 1;
                }
                catch (Exception)
                {
                    return 0;
                    throw;
                }
            }
        }
        public static string SaveInventoryRoomRatePlan_Ph(int iUserId, int PropId, bool StopSell, DataTable DateRoomPlan)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                SqlParameter[] MyParam = new SqlParameter[4];
                try
                {
                    List<InvalidRatePlans> IRP = new List<InvalidRatePlans>();
                    MyParam[0] = new SqlParameter("@DateRoomPlan", DateRoomPlan);
                    MyParam[0].TypeName = "[dbo].[DateRoomPlan]";
                    MyParam[1] = new SqlParameter("@iPropId", PropId);
                    MyParam[2] = new SqlParameter("@bStopSell", StopSell);
                    MyParam[3] = new SqlParameter("@iActionBy", iUserId);
                    IRP = db.Database.SqlQuery<InvalidRatePlans>("uspSaveRoomPlanInventory_Ph @DateRoomPlan, @iPropId, @bStopSell, @iActionBy", MyParam).ToList();
                    if (IRP.Count > 0)
                    {
                        //var serializer = new JavaScriptSerializer();
                        var js = JsonConvert.SerializeObject(IRP);
                        return js.ToString();
                    }
                    else
                    {
                        return "a";
                    }
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }
        }
        public static string SaveInventoryRoomRatePlanPrices_Ph(int iUserId, int PropId,  DataTable DateRoomPlan,decimal single,decimal doble,decimal triple,decimal quad, decimal quin)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                SqlParameter[] MyParam = new SqlParameter[8];
                try
                {
                    List<InvalidRatePlans> IRP = new List<InvalidRatePlans>();
                    MyParam[0] = new SqlParameter("@DateRoomPlan", DateRoomPlan);
                    MyParam[0].TypeName = "[dbo].[DateRoomPlan]";
                    MyParam[1] = new SqlParameter("@iPropId", PropId);
                    MyParam[2] = new SqlParameter("@iActionBy", iUserId);
                    MyParam[3] = new SqlParameter("@dSingleRate", single == 0 ? (object)DBNull.Value : single);
                    MyParam[4] = new SqlParameter("@dDoubleRate", doble == 0 ? (object)DBNull.Value : doble);
                    MyParam[5] = new SqlParameter("@dTripleRate", triple == 0 ? (object)DBNull.Value : triple);
                    MyParam[6] = new SqlParameter("@dQuadrupleRate", quad == 0 ? (object)DBNull.Value : quad);
                    MyParam[7] = new SqlParameter("@dQuintrupleRate", quin == 0 ? (object)DBNull.Value : quin);
                    IRP = db.Database.SqlQuery<InvalidRatePlans>("uspSaveInventoryRoomRatePlanPrices @DateRoomPlan, @iPropId, @iActionBy, @dSingleRate, @dDoubleRate, @dTripleRate, @dQuadrupleRate, @dQuintrupleRate", MyParam).ToList();
                    if (IRP.Count > 0)
                    {
                        //var serializer = new JavaScriptSerializer();
                        var js = JsonConvert.SerializeObject(IRP);
                        return js.ToString();
                    }
                    else
                    {
                        return "a";
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        // For download excel sheet
        public static DataSet GetBiddingDump(int PropId)
        {
            DataSet ds;
            try
            {
                SqlParameter[] MyParam = new System.Data.SqlClient.SqlParameter[1];
                MyParam[0] = new System.Data.SqlClient.SqlParameter("@iPropId", PropId);
                ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetBiddingDumpByProp", MyParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public static string SaveBarChanges(eBarChanges_mob obj,int userId,int propId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                SqlParameter[] MyParam = new SqlParameter[4];
                try
                {
                    List<InvalidRatePlans> IRP = new List<InvalidRatePlans>();
                    MyParam[0] = new SqlParameter("@StartDate", obj.dtFrom);
                    MyParam[1] = new SqlParameter("@EndDate", obj.ToDate);
                    MyParam[2] = new SqlParameter("@iRoomId", obj.RoomId);
                    MyParam[3] = new SqlParameter("@iRPId", obj.PlanId);
                    MyParam[4] = new SqlParameter("@iPropId", propId);
                    MyParam[5] = new SqlParameter("@iActionBy",userId);
                    MyParam[6] = new SqlParameter("@dSingleRate",obj.dSingleRate);
                    MyParam[7] = new SqlParameter("@dDoubleRate", obj.dDoubleRate);
                    MyParam[8] = new SqlParameter("@dTripleRate", obj.dTripleRate);
                    MyParam[9] = new SqlParameter("@dQuadrupleRate", obj.dQuadrupleRate);
                    MyParam[10] = new SqlParameter("@dQuintrupleRate", obj.dQuintrupleRate);
                    DataSet ds = SqlHelper.ExecuteDataset(db.Database.Connection.ConnectionString, CommandType.StoredProcedure, "uspSaveRatesForRoomRatePlans_Ph", MyParam);
                    if (IRP.Count > 0)
                    {
                        //var serializer = new JavaScriptSerializer();
                        var js = JsonConvert.SerializeObject(IRP);
                        return js.ToString();
                    }
                    else
                    {
                        return "a";
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }

    public class InvalidRatePlans
    {
        public string sRatePlan { get; set; }
        public string dtValidFrom { get; set; }
        public string dtValidTo { get; set; }
    }
}