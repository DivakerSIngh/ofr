using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;

namespace OneFineRateBLL
{
    public class BL_tblAccomodationM
    {
        //Get all records
        public static List<etblAccomodationM> GetAllRecords()
        {

            List<etblAccomodationM> eobj = new List<etblAccomodationM>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                foreach (OneFineRate.tblAccomodationM item in db.tblAccomodationMs.ToList())
                {
                    eobj.Add((etblAccomodationM)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblAccomodationM()));
                }
            }
            return eobj;
        }
    }
}