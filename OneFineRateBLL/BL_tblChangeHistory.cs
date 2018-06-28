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

namespace OneFineRateBLL
{
    public class BL_tblChangeHistory
    {
        public static List<RoomNames> GetRoomTypeForDD(int PropId)
        {
            List<RoomNames> data = new List<RoomNames>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    data = (from t3 in db.tblPropertyRoomMaps
                            select new RoomNames
                            {
                                propId = t3.iPropId,
                                Name = t3.sRoomName,
                                Id = t3.iRoomId
                            }).Distinct().Where(u => u.propId == PropId).ToList();
                }
                catch
                {
                    throw;
                }
            }
            return data;
        }

        public static List<RoomNames> GetPromotionTypeForDD()
        {
            List<RoomNames> data = new List<RoomNames>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    data = (from t3 in db.tblPromoMs
                            select new RoomNames
                            {
                                propId = t3.iPromoId,
                                Name = t3.sPromoName,
                                Id = t3.iPromoId
                            }).Distinct().ToList();
                }
                catch
                {
                    throw;
                }
            }
            return data;
        }

        public static List<MenuPageName> GetPageNameForDD(int PropId)
        {
            List<MenuPageName> data = new List<MenuPageName>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    SqlParameter[] MyParam = new SqlParameter[1];
                    MyParam[0] = new SqlParameter("@PropId", PropId);
                    var objUnion = db.Database.SqlQuery<MenuPageName>("uspGetPropertInfoDDForCH @PropId", MyParam).ToList();
                    data = objUnion.ToList();
                }
                catch
                {
                    throw;
                }
            }
            return data;
        }

        public static List<RoomNames> GetRatePlanByRoomID(long RoomType, int PropID)
        {
            List<RoomNames> data = new List<RoomNames>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var data1 = (from t1 in db.tblPropertyRatePlanRoomTypeMaps
                                 join t2 in db.tblPropertyRatePlanMaps on t1.iRPId equals t2.iRPId
                                 select new
                                 {
                                     propId = (int)t2.iPropId,
                                     Name = t2.sRatePlan, // + "  From  " + ReturnDateinDDMMYYYY() + "  To  " + ReturnDateinDDMMYYYY((DateTime)t2.dtValidTo),
                                     From = (DateTime)t2.dtValidFrom,
                                     To = (DateTime)t2.dtValidTo,
                                     Id = t2.iRPId,
                                     RoomTypeId = t1.iRoomId
                                 }).Where(u => u.RoomTypeId == RoomType && u.propId == PropID).Distinct().ToList();

                    data.AddRange(data1.Select(x => new RoomNames
                    {
                        propId = x.propId,
                        Name = x.Name + "  From  " + x.From.ToString("dd/MM/yyyy") + "  To  " + x.To.ToString("dd/MM/yyyy"),
                        Id = x.Id,
                        RoomTypeId = x.RoomTypeId
                    }));
                }
                catch
                {
                    throw;
                }
            }
            return data;
        }

        public static List<etblChangeHistory> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount, int PropId, DateTime dtFrom, DateTime dtTo, int roomType, int ratePlan, DateTime dtEffectiveFrom, DateTime dtEffectiveTo)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<etblChangeHistory> user = new List<etblChangeHistory>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;
                List<etblChangeHistory> changeLst = new List<etblChangeHistory>();
                List<etblChangeHistory> objResult = new List<etblChangeHistory>();

                SqlParameter[] MyParam = new SqlParameter[7];

                MyParam[0] = new SqlParameter("@dtFrom", dtFrom);
                MyParam[1] = new SqlParameter("@dtTo", dtTo);
                MyParam[2] = new SqlParameter("@roomType", roomType);
                MyParam[3] = new SqlParameter("@ratePlan", ratePlan);
                MyParam[4] = new SqlParameter("@dtEffectiveFrom", dtEffectiveFrom);
                MyParam[5] = new SqlParameter("@dtEffectiveTo", dtEffectiveTo);
                MyParam[6] = new SqlParameter("@PropId", PropId);
                objResult = db.Database.SqlQuery<etblChangeHistory>("uspChangeHistory_H @dtFrom,@dtTo,@roomType,@ratePlan,@dtEffectiveFrom,@dtEffectiveTo,@PropId ", MyParam).ToList();

                var result = objResult.Where(a => a.sItem.ToLower().Contains(param.sSearch.ToLower())).ToList();
                //get Total Count for show total records 
                TotalCount = result.Count();

                ////implemented paging
                var lstUser = result.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                foreach (var item in lstUser)
                {
                    changeLst.Add((etblChangeHistory)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblChangeHistory()));
                }
                return changeLst;
            }

        }


        public static List<etblChangeHistory> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount, int PropId, DateTime dtFrom, DateTime dtTo, string Id)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<etblChangeHistory> user = new List<etblChangeHistory>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;
                List<etblChangeHistory> changeLst = new List<etblChangeHistory>();
                List<etblChangeHistory> objResult = new List<etblChangeHistory>();

                SqlParameter[] MyParam = new SqlParameter[4];

                MyParam[0] = new SqlParameter("@dtFrom", dtFrom);
                MyParam[1] = new SqlParameter("@dtTo", dtTo);
                MyParam[2] = new SqlParameter("@Id", Id);
                MyParam[3] = new SqlParameter("@PropId", PropId);
                objResult = db.Database.SqlQuery<etblChangeHistory>("uspChangeHistoryPropertyInformation_H @dtFrom,@dtTo,@Id,@PropId ", MyParam).ToList();

                var result = objResult.Where(a => a.sItem.ToLower().Contains(param.sSearch.ToLower())).ToList();
                //get Total Count for show total records 
                TotalCount = result.Count();

                ////implemented paging
                var lstUser = result.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                foreach (var item in lstUser)
                {
                    changeLst.Add((etblChangeHistory)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblChangeHistory()));
                }
                return changeLst;
            }
        }


        public static List<etblChangeHistory> getRecordForSearchPromotions(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount, DateTime dtFrom, DateTime dtTo, string Id, int PropId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<etblChangeHistory> user = new List<etblChangeHistory>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;
                List<etblChangeHistory> changeLst = new List<etblChangeHistory>();
                List<etblChangeHistory> objResult = new List<etblChangeHistory>();

                SqlParameter[] MyParam = new SqlParameter[4];

                MyParam[0] = new SqlParameter("@dtFrom", dtFrom);
                MyParam[1] = new SqlParameter("@dtTo", dtTo);
                MyParam[2] = new SqlParameter("@Id", Id);
                MyParam[3] = new SqlParameter("@PropId", PropId);
                objResult = db.Database.SqlQuery<etblChangeHistory>("uspChangeHistoryPropertyPromotionMap_H @dtFrom,@dtTo,@Id,@PropId ", MyParam).ToList();

                var result = objResult.Where(a => a.sItem.ToLower().Contains(param.sSearch.ToLower())).ToList();
                //get Total Count for show total records 
                TotalCount = result.Count();

                ////implemented paging
                var lstUser = result.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                foreach (var item in lstUser)
                {
                    changeLst.Add((etblChangeHistory)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblChangeHistory()));
                }
                return changeLst;
            }
        }

        public static List<etblChangeHistory> getRecordForSearchRatePlan(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount, DateTime dtFrom, DateTime dtTo, int PropId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<etblChangeHistory> user = new List<etblChangeHistory>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;
                List<etblChangeHistory> changeLst = new List<etblChangeHistory>();
                List<etblChangeHistory> objResult = new List<etblChangeHistory>();

                SqlParameter[] MyParam = new SqlParameter[3];

                MyParam[0] = new SqlParameter("@dtFrom", dtFrom);
                MyParam[1] = new SqlParameter("@dtTo", dtTo);
                MyParam[2] = new SqlParameter("@PropId", PropId);
                objResult = db.Database.SqlQuery<etblChangeHistory>("uspChangeHistoryPropertyRatePlanMap_H @dtFrom,@dtTo,@PropId ", MyParam).ToList();

                var result = objResult.Where(a => a.sItem.ToLower().Contains(param.sSearch.ToLower())).ToList();
                //get Total Count for show total records 
                TotalCount = result.Count();

                ////implemented paging
                var lstUser = result.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                foreach (var item in lstUser)
                {
                    changeLst.Add((etblChangeHistory)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblChangeHistory()));
                }
                return changeLst;
            }
        }

        public static DataTable getRecordForSearchExcel(int PropId, DateTime dtFrom, DateTime dtTo, int roomType, int ratePlan, DateTime dtEffectiveFrom, DateTime dtEffectiveTo)
        {
            SqlParameter[] MyParam = new SqlParameter[7];
            MyParam[0] = new SqlParameter("@dtFrom", dtFrom);
            MyParam[1] = new SqlParameter("@dtTo", dtTo);
            MyParam[2] = new SqlParameter("@roomType", roomType);
            MyParam[3] = new SqlParameter("@ratePlan", ratePlan);
            MyParam[4] = new SqlParameter("@dtEffectiveFrom", dtEffectiveFrom);
            MyParam[5] = new SqlParameter("@dtEffectiveTo", dtEffectiveTo);
            MyParam[6] = new SqlParameter("@PropId", PropId);
            DataTable dt = new DataTable();
            dt = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"], CommandType.StoredProcedure, "uspChangeHistory_H", MyParam).Tables[0];
            return dt;
            //get Total Count for show total records 
    }

    public static DataTable getRecordForSearchPromotionsExcel(DateTime dtFrom, DateTime dtTo, string Id, int PropId)
    {
        SqlParameter[] MyParam = new SqlParameter[4];
        MyParam[0] = new SqlParameter("@dtFrom", dtFrom);
        MyParam[1] = new SqlParameter("@dtTo", dtTo);
        MyParam[2] = new SqlParameter("@Id", Id);
        MyParam[3] = new SqlParameter("@PropId", PropId);
        DataTable dt = new DataTable();
        dt = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"], CommandType.StoredProcedure, "uspChangeHistoryPropertyPromotionMap_H", MyParam).Tables[0];
        return dt;
    }

    public static DataTable getRecordForSearchRatePlanExcel(DateTime dtFrom, DateTime dtTo, int PropId)
    {
        SqlParameter[] MyParam = new SqlParameter[3];
        MyParam[0] = new SqlParameter("@dtFrom", dtFrom);
        MyParam[1] = new SqlParameter("@dtTo", dtTo);
        MyParam[2] = new SqlParameter("@PropId", PropId);
        DataTable dt = new DataTable();
        dt = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"], CommandType.StoredProcedure, "uspChangeHistoryPropertyRatePlanMap_H", MyParam).Tables[0];
        return dt;
    }
    public static DataTable getRecordForSearchExcel(int PropId, DateTime dtFrom, DateTime dtTo, string Id)
    {
        SqlParameter[] MyParam = new SqlParameter[4];
        MyParam[0] = new SqlParameter("@dtFrom", dtFrom);
        MyParam[1] = new SqlParameter("@dtTo", dtTo);
        MyParam[2] = new SqlParameter("@Id", Id);
        MyParam[3] = new SqlParameter("@PropId", PropId);
        DataTable dt = new DataTable();
        dt = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"], CommandType.StoredProcedure, "uspChangeHistoryPropertyInformation_H", MyParam).Tables[0];
        return dt;
    }


}
}