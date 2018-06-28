using System;
using System.Collections.Generic;
using System.Linq;
using OneFineRate;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_UnfinishedTransactions
    {

        public static etblUnfinishedTransactionsToBePaid GetUnfinishedBalanceToBePaidData(long? BookingId)
        {
            etblUnfinishedTransactionsToBePaid objMapping = new etblUnfinishedTransactionsToBePaid();
            List<etblHotelFacilities> objresultHotelFacilities = new List<etblHotelFacilities>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {

                #region General Details
                SqlParameter[] MyParam = new SqlParameter[1];
                MyParam[0] = new SqlParameter("@BookingId", BookingId);
                var objresult = db.Database.SqlQuery<etblUnfinishedTransactionsToBePaid>("uspGetUnfinishedBalanceToBePaidData @BookingId", MyParam).ToList();
                objMapping = (etblUnfinishedTransactionsToBePaid)OneFineRateAppUtil.clsUtils.ConvertToObject(objresult[0], objMapping);

                int propId = objMapping.iPropId;
                SqlParameter[] MyParamHotelFacilities = new SqlParameter[1];
                MyParamHotelFacilities[0] = new SqlParameter("@PropId", propId);
                objresultHotelFacilities = db.Database.SqlQuery<etblHotelFacilities>("uspGetOfferReviewHotelAmenties @PropId", MyParamHotelFacilities).ToList();
                objMapping.lstetblHotelFacilities = objresultHotelFacilities;

                #endregion
                return objMapping;
            }
        }

        public static etblUnfinishedHotelNotSelected GetUnfinishedHotelNotSelected(long? BookingId)
        {
            etblUnfinishedHotelNotSelected objMapping = new etblUnfinishedHotelNotSelected();
            List<etblHotelFacilities> objresultHotelFacilities = new List<etblHotelFacilities>();
            List<etblSelectedHotels> objSelectedHotels = new List<etblSelectedHotels>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {

                #region General Details
                SqlParameter[] MyParam = new SqlParameter[1];
                MyParam[0] = new SqlParameter("@BookingId", BookingId);
                objSelectedHotels = db.Database.SqlQuery<etblSelectedHotels>("uspGetUnfinishedHotelNotSelected @BookingId", MyParam).ToList();

                for (int i = 0; i < objSelectedHotels.Count; i++)
                {
                    int propId = objSelectedHotels[i].iPropId;
                    SqlParameter[] MyParamHotelFacilities = new SqlParameter[1];
                    MyParamHotelFacilities[0] = new SqlParameter("@PropId", propId);
                    objresultHotelFacilities = db.Database.SqlQuery<etblHotelFacilities>("uspGetOfferReviewHotelAmenties @PropId", MyParamHotelFacilities).ToList();
                    objSelectedHotels[i].lstetblHotelFacilities = objresultHotelFacilities;
                }
                objMapping.lstetblSelectedHotels = objSelectedHotels;
                #endregion
                return objMapping;
            }
        }

        public static List<etblUnfinishedTransactions> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount, string sSearchType, string sName, string sEmailID, string iTelephone, Int64 iBookingID, string sTransDate)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<etblUnfinishedTransactions> user = new List<etblUnfinishedTransactions>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;
                List<etblUnfinishedTransactions> changeLst = new List<etblUnfinishedTransactions>();
                List<etblUnfinishedTransactions> objResult = new List<etblUnfinishedTransactions>();


                SqlParameter[] MyParam = new SqlParameter[6];


                MyParam[0] = new SqlParameter("@sSearchType", sSearchType);
                MyParam[1] = new SqlParameter("@sName", sName + "%");
                MyParam[2] = new SqlParameter("@sEmailID", sEmailID);
                MyParam[3] = new SqlParameter("@iTelephone", iTelephone);
                MyParam[4] = new SqlParameter("@iBookingID", iBookingID);
                //MyParam[5] = new SqlParameter("@sTransDate", (sTransDate.Trim() == string.Empty ? null : OneFineRateAppUtil.clsUtils.ConvertddmmyyyytoDateTime(sTransDate)));
                MyParam[5] = new SqlParameter("@sTransDate", "");
                if (sTransDate.Trim() == string.Empty)
                {
                    MyParam[5].Value = "";
                }
                else
                {
                    MyParam[5].Value = OneFineRateAppUtil.clsUtils.ConvertddmmyyyytoDateTime(sTransDate);
                }

                try
                {

                    objResult = db.Database.SqlQuery<etblUnfinishedTransactions>("uspGetUnFinishedTransactionData @sSearchType ,@sName ,@sEmailID ,@iTelephone ,@iBookingID ,@sTransDate", MyParam).ToList();

                }
                catch (System.Exception)
                {
                    
                    throw;
                }
                var result = objResult.Where(a => a.sName.Contains(param.sSearch.ToLower()) || a.sEmailID.Contains(param.sSearch.ToUpper())).ToList();
                //get Total Count for show total records 
                TotalCount = result.Count();

                ////implemented paging
                var lstUser = result.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                foreach (var item in lstUser)
                {
                    changeLst.Add((etblUnfinishedTransactions)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblUnfinishedTransactions()));
                }
                return changeLst;
            }

        }

        public static int UpdateBookingStatus(long? BookingId)
        {
            SqlConnection objConn = default(SqlConnection);
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                string BookingStatus = "C";
                objConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());
                objConn.Open();

                System.Data.SqlClient.SqlParameter[] MyParams = new System.Data.SqlClient.SqlParameter[2];
                MyParams[0] = new System.Data.SqlClient.SqlParameter("@BookingId", BookingId);
                MyParams[1] = new System.Data.SqlClient.SqlParameter("@BookingStatus", BookingStatus);

                int i = SqlHelper.ExecuteNonQuery(objConn, CommandType.StoredProcedure, "uspUpdateBookingStatus", MyParams);
                return i;
            }
        }

        public static etblPropertyDetail GetPropertyData(long? iBookingID)
        {
            etblPropertyDetail objMapping = new etblPropertyDetail();
            List<etblHotelFacilities> objresultHotelFacilities = new List<etblHotelFacilities>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {

                #region General Details
                SqlParameter[] MyParam = new SqlParameter[1];
                MyParam[0] = new SqlParameter("@iBookingID", iBookingID);
                var result = db.Database.SqlQuery<etblSelectedHotels>("uspGetPropertyData @iBookingID", MyParam).ToList();
                objMapping = (etblPropertyDetail)OneFineRateAppUtil.clsUtils.ConvertToObject(result[0], objMapping);


                int propId = objMapping.iPropId;
                SqlParameter[] MyParamHotelFacilities = new SqlParameter[1];
                MyParamHotelFacilities[0] = new SqlParameter("@PropId", propId);
                objresultHotelFacilities = db.Database.SqlQuery<etblHotelFacilities>("uspGetOfferReviewHotelAmenties @PropId", MyParamHotelFacilities).ToList();
                objMapping.lstetblHotelFacilities = objresultHotelFacilities;

                #endregion
                return objMapping;
            }
        }
    }
}