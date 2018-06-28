using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;

namespace OneFineRateBLL
{
    public class BL_tblCarouselM
    {
        //Get all records
        public static List<etblCarouselM> GetAllRecords()
        {
            List<etblCarouselM> eobj = new List<etblCarouselM>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                foreach (OneFineRate.tblCarouselM item in db.tblCarouselMs.OrderBy(u=> u.iSeq).ToList())
                {
                    eobj.Add((etblCarouselM)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblCarouselM()));
                }
            }
            return eobj;
        }
    }
}