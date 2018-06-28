using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;
using System.Web.Mvc;
using System.Linq.Expressions;
using OneFineRateAppUtil;
using System.Data.SqlClient;
using System.Data;

namespace OneFineRateBLL
{
    public class BL_NegotiationNotification
    {

        public static List<etblNegotiationNotification> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount, int propId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<etblNegotiationNotification> RatePlanLst = new List<etblNegotiationNotification>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;

                SqlParameter[] MyParam = new SqlParameter[1];
                MyParam[0] = new SqlParameter("@propId", propId);
                var objTblRatePlan = db.Database.SqlQuery<etblNegotiationNotification>("uspNegotiationNotificationList @propId", MyParam).ToList();

                var result = objTblRatePlan.Where(a => a.sRatePlan.ToLower().Contains(param.sSearch.ToLower()) || a.sType.ToLower().Contains(param.sSearch.ToLower()));
                //get Total Count for show total records 
                TotalCount = result.Count();

                //for sorting
                switch (param.iSortingCols)
                {
                    case 0:
                        result = param.sortDirection == "asc" ? result.OrderBy(u => u.RecId) : result.OrderByDescending(u => u.RecId);
                        break;
                    case 1:
                        result = param.sortDirection == "asc" ? result.OrderBy(u => u.sRatePlan) : result.OrderByDescending(u => u.sRatePlan);
                        break;
                    case 2:
                        result = param.sortDirection == "asc" ? result.OrderBy(u => u.sValidity) : result.OrderByDescending(u => u.sValidity);
                        break;
                }

                ////implemented paging
                var lstUser = result.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                //convert bll entity object to db entity object
                foreach (var item in lstUser)
                {
                    RatePlanLst.Add((etblNegotiationNotification)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblNegotiationNotification()));
                }
                return RatePlanLst;
            }
        }

        public static int Update(int Id, string rectype, decimal NegotiationVal, int PropId)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    SqlParameter[] MyParam = new SqlParameter[4];
                    MyParam[0] = new SqlParameter("@RecId", Id);
                    MyParam[1] = new SqlParameter("@rectype", rectype);
                    MyParam[2] = new SqlParameter("@NegotiationVal", NegotiationVal);
                    MyParam[3] = new SqlParameter("@PropId", PropId);
                    
                    db.Database.ExecuteSqlCommand("UpdateNegotiationTriggerDiscount @RecId,@rectype,@NegotiationVal,@PropId", MyParam);
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