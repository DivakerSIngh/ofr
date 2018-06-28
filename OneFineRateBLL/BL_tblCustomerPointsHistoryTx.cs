using OneFineRate;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_tblCustomerPointsHistoryTx
    {

        public static List<etblCustomerPointsHistoryTx> GetAllRecord(long customerId,string startDate, string enddate)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var idParam = new SqlParameter[3];
                    idParam[0] = new SqlParameter("@dtStart", startDate);
                    idParam[1] = new SqlParameter("@dtEnd", enddate);
                    idParam[2] = new SqlParameter("@iCustomerId", customerId);

                    return db.Database.SqlQuery<etblCustomerPointsHistoryTx>("uspRewardPoints @dtStart,@dtEnd, @iCustomerId", idParam).ToList();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}