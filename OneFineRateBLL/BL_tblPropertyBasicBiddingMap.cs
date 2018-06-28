using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace OneFineRateBLL
{
    public class BL_tblPropertyBasicBiddingMap
    {
        public static DataSet ListSimulatorDataV2(int propid, string BookingDate, string ArrivalDate, string LOS, string Rooms, string Occupancy, string Type)
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
        }


        //Update a record
        public static int UpdateAllBidRecord(int propid, int ActionBy, DataTable BidDatesWithType, DataTable BidDatesWithTypeForRoom, bool type)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    SqlParameter[] MyParam = new SqlParameter[5];
                    MyParam[0] = new SqlParameter("@BidDatesWithType", BidDatesWithType);
                    MyParam[0].TypeName = "[dbo].[BidDatesWithType]";
                    MyParam[1] = new SqlParameter("@BidDatesWithTypeForRoom", BidDatesWithTypeForRoom);
                    MyParam[1].TypeName = "[dbo].[BidDatesWithType]";
                    MyParam[2] = new SqlParameter("@iActionBy", ActionBy);
                    MyParam[3] = new SqlParameter("@iPropId", propid);
                    MyParam[4] = new SqlParameter("@ClosedOut", type);


                    db.Database.ExecuteSqlCommand("uspUpdateAllRateBid @BidDatesWithType, @BidDatesWithTypeForRoom, @iActionBy, @iPropId, @ClosedOut ", MyParam);
                    retval = 1;

                }
                catch (Exception)
                {
                    retval = 0;
                    throw;
                }
            }
            return retval;
        }
        public static DataSet GetDataforBidRateInventory(int propid, string cdate)
        {
            clsDB obj = new clsDB();
            DataSet ds = new DataSet();
            ds = obj.ListBidInventory(propid, cdate);
            return ds;
        }
        //get single record
        public static etblPropertyBasicBiddingMapForOverview GetSingleRecordById(DateTime Date, int PropId, int typ,string corporateOrPublic)
        {
            etblPropertyBasicBiddingMapForOverview OBJ = new etblPropertyBasicBiddingMapForOverview();
            etblPropertyBasicBiddingMap eobjPublic = new etblPropertyBasicBiddingMap();
            etblPropertyBasicBiddingMap eobjCorp = new etblPropertyBasicBiddingMap();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                // Getting Data for Public rates
                {
                    var dbobj = db.tblPropertyBasicBiddingMaps.SingleOrDefault(u => u.iPropId == PropId && u.dtEffectiveDate == Date);
                    if (dbobj != null)
                        eobjPublic = (etblPropertyBasicBiddingMap)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobjPublic);
                }

                // Getting Data for Corp rates
                {
                    var dbobj = db.tblPropertyBasicBiddingMapCorps.SingleOrDefault(u => u.iPropId == PropId && u.dtEffectiveDate == Date);
                    if (dbobj != null)
                        eobjCorp = (etblPropertyBasicBiddingMap)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobjCorp);
                }
            }

            if(string.IsNullOrEmpty(corporateOrPublic))
            {
                if (typ % 2 == 1) // Public discounts as main. Corporate as other.
                {
                    OBJ.Self = eobjPublic;
                    OBJ.Other = eobjCorp;
                    OBJ.IsPublic = true;
                }
                else
                {
                    OBJ.Self = eobjCorp;
                    OBJ.Other = eobjPublic;
                    OBJ.IsPublic = false;
                }
            }

            else if (corporateOrPublic == null && typ == 9)
            {
                OBJ.Self = eobjPublic;
                OBJ.Other = eobjCorp;
                OBJ.IsPublic = true;
            }
            else
            {
                if (corporateOrPublic == "p")
                {
                    OBJ.Self = eobjPublic;
                    OBJ.Other = eobjCorp;
                    OBJ.IsPublic = true;
                }
                else
                {
                    OBJ.Self = eobjCorp;
                    OBJ.Other = eobjPublic;
                    OBJ.IsPublic = false;
                }
            }
           
            return OBJ;

        }
        //Update a record
        public static int UpdateRecord(etblPropertyBasicBiddingMapForOverview eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    DataTable BidRange = new DataTable();
                    DataColumn col = null;
                    col = new DataColumn("dtEffectiveDate", typeof(DateTime));
                    BidRange.Columns.Add(col);

                    DataRow drBidRange = BidRange.NewRow();
                    drBidRange["dtEffectiveDate"] = eobj.Self.dtEffectiveDate;
                    BidRange.Rows.Add(drBidRange);

                    SqlParameter[] MyParam = new SqlParameter[8];
                    MyParam[0] = new SqlParameter("@BidDates", BidRange);
                    MyParam[0].TypeName = "[dbo].[BidDates]";
                    MyParam[1] = new SqlParameter("@iPropId", eobj.Self.iPropId);
                    MyParam[2] = new SqlParameter("@bCloseOut", eobj.Self.bIsClosed);
                    MyParam[3] = new SqlParameter("@bCTA", eobj.Self.bCTA);
                    MyParam[4] = new SqlParameter("@bCTD", eobj.Self.bCTD);
                    MyParam[5] = new SqlParameter("@iActionBy", eobj.Self.iActionBy);
                    MyParam[6] = new SqlParameter("@dDiscount", eobj.Self.dBasicDiscount == null ? (object)DBNull.Value : eobj.Self.dBasicDiscount);
                    MyParam[7] = new SqlParameter("@typ", eobj.IsPublic ? 1 : 0);
                    db.Database.ExecuteSqlCommand("uspSaveBasicDisc @BidDates, @iPropId, @iActionBy, @bCloseOut, @bCTA, @bCTD, @dDiscount, @typ", MyParam);
                    retval = 1;
                }
                catch (Exception) { }
            }
            return retval;
        }
        public static DataSet GetDataforBidSimulator(int propid, string BookingDate, string ArrivalDate, string LOS, string Rooms, string Occupancy, int Type)
        {
            clsDB obj = new clsDB();
            DataSet ds = new DataSet();
            ds = obj.ListSimulatorData(propid, BookingDate, ArrivalDate, LOS, Rooms, Occupancy, Type);
            return ds;
        }

        public static DataTable GetBidLoadRateList(int propId)
        {
            clsDB obj = new clsDB();
            DataTable dt = new DataTable();
            dt = obj.GetBidLoadRateList(propId);
            return dt;
        }
        public static DataTable GetBidLoadRateUpgradeList(int propId, string ValidFrom, string ValidTo)
        {
            clsDB obj = new clsDB();
            DataTable dt = new DataTable();
            dt = obj.GetBidLoadRateUpgradeList(propId, ValidFrom, ValidTo);
            return dt;
        }
    }
}