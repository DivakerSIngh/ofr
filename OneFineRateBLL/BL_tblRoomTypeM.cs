using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;

namespace OneFineRateBLL
{
    public class BL_tblRoomTypeM
    {
        //Get all records
        public static List<etblRoomTypeM> GetAllRecords()
        {
            List<etblRoomTypeM> eobj = new List<etblRoomTypeM>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                foreach (OneFineRate.tblRoomTypeM item in db.tblRoomTypeMs.ToList())
                {
                    eobj.Add((etblRoomTypeM)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblRoomTypeM()));
                }
            }
            return eobj;
        }
    }
}