using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;

namespace OneFineRateBLL
{
    public class BL_tblStarCategoryM
    {
        //Get all records
        public static List<etblStarCategoryM> GetAllRecords()
        {

            List<etblStarCategoryM> eobj = new List<etblStarCategoryM>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                foreach (OneFineRate.tblStarCategoryM item in db.tblStarCategoryMs.ToList())
                {
                    eobj.Add((etblStarCategoryM)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblStarCategoryM()));
                }
            }
            return eobj;
        }
    }
}