using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Text;
namespace OneFineRateBLL
{
    public class BL_tblPropertyBiddingRateM
    {
        public static string CheckRatePlanValidity(int RPID, DateTime datefrom, DateTime dateto)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var RatePlan = (from s in db.tblPropertyRatePlanMaps
                                select new
                                {
                                    s.iRPId,
                                    s.dtValidFrom,
                                    s.dtValidTo
                                }).Distinct().Where(u => u.iRPId.Equals(RPID) && (u.dtValidFrom > datefrom || u.dtValidFrom > dateto || u.dtValidTo < datefrom || u.dtValidTo < dateto)).AsQueryable();

                var RP = RatePlan.ToList();
                if (RP.Count > 0)
                {
                    DateTime FROM = (DateTime)RP[0].dtValidFrom;
                    DateTime TO = (DateTime)RP[0].dtValidTo;
                    return "Rate plan is valid between " + FROM.ToString("dd/MM/yyyy") + " and " + TO.ToString("dd/MM/yyyy");
                }
                return "a";
            }
        }

        public static string CheckRoomRatePlanPrice(int RoomId, int RPID, DateTime datefrom, DateTime dateto)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var RatePlan = (from p in db.tblPropertyRoomRatePlanInventoryMaps.Where(u => u.iRoomId == RoomId && u.iRPId == RPID && (u.dtInventoryDate == datefrom || u.dtInventoryDate == dateto))
                                select new
                                {
                                    p.dSingleRate,
                                    p.dDoubleRate,
                                    p.dTripleRate
                                }).Distinct().Where(u => u.dSingleRate != null || u.dDoubleRate != null || u.dTripleRate != null).AsQueryable();



                var RP = RatePlan.ToList();
                if (RP.Count == 0)
                {
                    return "Please update price for this rate plan & room for chosen bidding dates";
                }
                return "a";
            }
        }

        //Update a record
        public static int AddUpdateRecord(etblPropertyBiddingRateM eobj, DataTable PropertyBiddingRateM, DataTable PropertyBidUpgradeM)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    SqlParameter[] MyParam = new SqlParameter[4];
                    MyParam[0] = new SqlParameter("@PropertyBiddingRateM", PropertyBiddingRateM);
                    MyParam[0].TypeName = "[dbo].[PropertyBiddingRateM]";
                    MyParam[1] = new SqlParameter("@PropertyBidUpgradeM", PropertyBidUpgradeM);
                    MyParam[1].TypeName = "[dbo].[PropertyBidUpgradeM]";
                    MyParam[2] = new SqlParameter("@iActionBy", eobj.iActionBy);
                    MyParam[3] = new SqlParameter("@iPropId", eobj.iPropId);


                    db.Database.ExecuteSqlCommand("uspSaveBiddingRates @PropertyBiddingRateM, @PropertyBidUpgradeM, @iActionBy, @iPropId ", MyParam);
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

        public static string GetBidLoadRateList(int iPropId)
        {

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {

                    DataTable dt = new DataTable();
                    dt = BL_tblPropertyBasicBiddingMap.GetBidLoadRateList(iPropId);

                    StringBuilder str = new StringBuilder();
                    str.Append("[");
                    foreach (DataRow dr in dt.Rows)
                    {
                        str.Append("{");
                        string jsonval = "";
                        jsonval = "\"sRoomName\" : \"" + dr["sRoomName"].ToString() + "\" ,";
                        jsonval += "\"sRatePlan\" : \"" + dr["sRatePlan"].ToString() + "\" ,";
                        jsonval += "\"ValidFrom\" : \"" + dr["ValidFrom"].ToString() + "\" ,";
                        jsonval += "\"ValidTo\" : \"" + dr["ValidTo"].ToString() + "\" ,";
                        jsonval += "\"UpgradeAvailable\" : \"" + dr["UpgradeAvailable"].ToString() + "\" ";
                        str.Append(jsonval);
                        str.Append("},");

                    }
                    str.Replace(',', ']', str.Length - 1, 1);

                    if (str.ToString() == "[")
                        str.Append("]");
                    return str.ToString();

                    //var BidLoadRate = db.uspGetBidLoadRateList(iPropId);
                    //var BidLoadRateList = BidLoadRate.ToList();
                    //if (BidLoadRateList.Count > 0)
                    //{
                    //    List<uspGetBidLoadRateList_Result> result = new List<uspGetBidLoadRateList_Result>();
                    //    result.AddRange(BidLoadRateList.Select(usr => (uspGetBidLoadRateList_Result)OneFineRateAppUtil.clsUtils.ConvertToObject(usr, new uspGetBidLoadRateList_Result())));
                    //    return JsonConvert.SerializeObject(result);
                    //}
                    //return null;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public static string GetBidLoadRateUpgradeList(int iPropId, string ValidFrom, string ValidTo)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {

                    DataTable dt = new DataTable();
                    dt = BL_tblPropertyBasicBiddingMap.GetBidLoadRateUpgradeList(iPropId, ValidFrom, ValidTo);

                    StringBuilder str = new StringBuilder();
                    str.Append("[");
                    foreach (DataRow dr in dt.Rows)
                    {
                        str.Append("{");
                        string jsonval = "";
                        jsonval = "\"sRoomName\" : \"" + dr["sRoomName"].ToString() + "\" ,";
                        jsonval += "\"dUpgradeCharge\" : \"" + dr["dUpgradeCharge"].ToString() + "\" ,";
                        jsonval += "\"ValidFrom\" : \"" + dr["ValidFrom"].ToString() + "\" ,";
                        jsonval += "\"ValidTo\" : \"" + dr["ValidTo"].ToString() + "\" ,";
                        jsonval += "\"sUpgradeType\" : \"" + dr["sUpgradeType"].ToString() + "\" ";
                        str.Append(jsonval);
                        str.Append("},");

                    }
                    str.Replace(',', ']', str.Length - 1, 1);

                    if (str.ToString() == "[")
                        str.Append("]");
                    return str.ToString();

                    //var BidLoadRateUpgrade = db.uspGetBidLoadRateUpgradeList(iPropId, ValidFrom, ValidTo);
                    //var BidLoadRateUpgradeList = BidLoadRateUpgrade.ToList();
                    //if (BidLoadRateUpgradeList.Count > 0)
                    //{
                    //    List<uspGetBidLoadRateUpgradeList_Result> result = new List<uspGetBidLoadRateUpgradeList_Result>();
                    //    result.AddRange(BidLoadRateUpgradeList.Select(usr => (uspGetBidLoadRateUpgradeList_Result)OneFineRateAppUtil.clsUtils.ConvertToObject(usr, new uspGetBidLoadRateUpgradeList_Result())));
                    //    return JsonConvert.SerializeObject(result);
                    //}
                    //return null;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        //get single record
        public static etblPropertyBidUpgradeM GetSingleRecordById(DateTime Date, int PropId, int id)
        {
            etblPropertyBidUpgradeM eobj = new etblPropertyBidUpgradeM();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblPropertyBidUpgradeMs.SingleOrDefault(u => u.iPropId == PropId && u.dtEffectiveDate == Date && u.iRoomId == id);
                if (dbobj != null)
                    eobj = (etblPropertyBidUpgradeM)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;
        }

        //Update a record
        public static int UpdateRecord(etblPropertyBidUpgradeM eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblPropertyBidUpgradeM obj = (OneFineRate.tblPropertyBidUpgradeM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyBidUpgradeM());
                    db.tblPropertyBidUpgradeMs.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    retval = 1;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retval;
        }
    }
}