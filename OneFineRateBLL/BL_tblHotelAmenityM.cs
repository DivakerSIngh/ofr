using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using  OneFineRate;
using OneFineRateBLL.Entities;

namespace OneFineRateBLL
{
    public class BL_tblHotelAmenityM
    {
        public static List<etblHotelAmenityM> GetHotelAmenities()
        {
            List<etblHotelAmenityM> eobj = new List<etblHotelAmenityM>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                foreach (OneFineRate.tblHotelAmenityM item in db.tblHotelAmenityMs.ToList())
                {
                    eobj.Add((etblHotelAmenityM)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblHotelAmenityM()));
                }
            }
            return eobj;
        }
    }
}