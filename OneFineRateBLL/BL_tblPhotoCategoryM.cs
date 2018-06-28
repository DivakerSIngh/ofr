using OneFineRate;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_tblPhotoCategoryM
    {
        //Get all records
        public static List<etblPhotoCategoryM> GetAllRecords()
        {
            List<etblPhotoCategoryM> eobj = new List<etblPhotoCategoryM>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                foreach (OneFineRate.tblPhotoCategoryM item in db.tblPhotoCategoryMs.ToList())
                {
                    eobj.Add((etblPhotoCategoryM)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblPhotoCategoryM()));
                }
            }
            return eobj;
        }

        //Get all records
        public static List<etblPhotoSubCategoryM> GetSubcategoriesFromCategoryId(long categoryId, long PropId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                SqlParameter[] parameterValue = new SqlParameter[2];
                parameterValue[0] = new SqlParameter("@iPhotoCatId", categoryId);
                parameterValue[1] = new SqlParameter("@iPropId", PropId);
               // var parameterValue = new SqlParameter("@iPhotoCatId", categoryId);

                return db.Database.SqlQuery<etblPhotoSubCategoryM>("uspGetPhotoSubCategoryByCatId @iPhotoCatId, @iPropId", parameterValue).ToList<etblPhotoSubCategoryM>();
                //user = db.Database.SqlQuery<eRatePlanDisp>("uspGetRatePlanByName @RatePlan, @DisplayLength, @DisplayStart, @SortDirection, @TotalCount out", MyParam).ToList();

            }
        }
    }
}