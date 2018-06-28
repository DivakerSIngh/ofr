using OneFineRate;
using OneFineRateAppUtil;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_MinimumIncome
    {
        public static List<eMinimumIncome> GetHotelList(jQueryDataTableParamModel param, string star, string propIds, string stateIds, int cityId, out int TotalCount)
        {
            var result = new List<eMinimumIncome>();

            SqlParameter[] MyParam = new SqlParameter[9];
            MyParam[0] = new SqlParameter("@StarCategory", star);
            MyParam[1] = new SqlParameter("@PropIds", propIds);
            MyParam[2] = new SqlParameter("@StateIds", stateIds);
            MyParam[3] = new SqlParameter("@iCityId", cityId);
            MyParam[4] = new SqlParameter("@DisplayLength", param.iDisplayLength);
            MyParam[5] = new SqlParameter("@DisplayStart", param.iDisplayStart);
            MyParam[6] = new SqlParameter("@SortDirection", param.sortDirection == "asc" ? "A" : "D");
            MyParam[7] = new SqlParameter("@SortBy", param.iSortingCols);
            MyParam[8] = new SqlParameter("@TotalCount", 0)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            TotalCount = 0;

            DataTable dt = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetMinIncomeList", MyParam).Tables[0];

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var model = new eMinimumIncome();
                    model.iPropId = Convert.ToInt32(dt.Rows[i]["iPropId"]);
                    model.iStarCategoryId = Convert.ToInt32(dt.Rows[i]["iStarCategoryId"]);
                    model.sHotelName = Convert.ToString(dt.Rows[i]["sHotelName"]);
                    model.dMinIncome = Convert.ToDecimal(dt.Rows[i]["dMinIncome"] == DBNull.Value ? 0 : dt.Rows[i]["dMinIncome"]);
                    model.sState = Convert.ToString(dt.Rows[i]["sState"]);
                    model.sCity = Convert.ToString(dt.Rows[i]["sCity"]);
                    model.dtActionDate = Convert.ToString(dt.Rows[i]["dtMinIncomeActionDate"] == DBNull.Value ? "" : dt.Rows[i]["dtMinIncomeActionDate"]);
                    model.sActionBy = Convert.ToString(dt.Rows[i]["ActionBy"]);

                    result.Add(model);
                }

                TotalCount = Convert.ToInt32(MyParam[8].Value);
            }

            return result;
        }


        public static KeyValuePair<int, string> SaveMinimumIncome(string propIds, decimal minimumIncome,int actionBy)
        {
            var result = new KeyValuePair<int, string>();

            SqlParameter[] MyParam = new SqlParameter[4];
            MyParam[0] = new SqlParameter("@sPropIds", propIds);
            MyParam[1] = new SqlParameter("@dMinimumIncome", minimumIncome);
            MyParam[2] = new SqlParameter("@dtMinIncomeActionDate", DateTime.Now);
            MyParam[3] = new SqlParameter("@iActionBy", actionBy);

            var ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "[dbo].[uspSetMinIncome]", MyParam);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                result = new KeyValuePair<int, string>(Convert.ToInt32(ds.Tables[0].Rows[0]["status"].ToString()), ds.Tables[0].Rows[0]["errmsg"].ToString());
            }

            return result;
        }
    }
}