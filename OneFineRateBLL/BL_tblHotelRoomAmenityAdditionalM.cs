using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;

namespace OneFineRateBLL
{
    public class BL_tblHotelRoomAmenityAdditionalM
    {
        public static List<CheckBoxList> GetHotelRoomAmenityAdditional(string data, string newdata = null)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                string[] objSelectItems = null;
                if (data != null)
                {
                    objSelectItems = data.Split(',');
                    if (newdata != null)
                    {
                        objSelectItems = objSelectItems.Union(newdata.Split(',')).ToArray();
                    }
                   
                    selectItems = (from am in db.tblHotelRoomAmenityAdditionalMs.ToList()
                                   join si in objSelectItems.ToList() on am.iHotelRoomAmenityAdditionalId equals Convert.ToInt32(si) into result
                                   from r in result.DefaultIfEmpty()
                                   select new CheckBoxList()
                                   {
                                       Text = am.sRoomAmenityAdditional,
                                       Value = am.iHotelRoomAmenityAdditionalId,
                                       Selected = r == null ? false : true
                                   }).ToList();
                }
                else if (newdata != null)
                {
                    objSelectItems = newdata.Split(',');
                    selectItems = (from am in db.tblHotelRoomAmenityAdditionalMs.ToList()
                                   join si in objSelectItems.ToList() on am.iHotelRoomAmenityAdditionalId equals Convert.ToInt32(si) into result
                                   from r in result.DefaultIfEmpty()
                                   select new CheckBoxList()
                                   {
                                       Text = am.sRoomAmenityAdditional,
                                       Value = am.iHotelRoomAmenityAdditionalId,
                                       Selected = r == null ? false : true
                                   }).ToList();
                }
                else
                {
                    selectItems = db.tblHotelRoomAmenityAdditionalMs.Select(x => new CheckBoxList()
                    {
                        Text = x.sRoomAmenityAdditional,
                        Value = x.iHotelRoomAmenityAdditionalId
                    }).ToList();
                }
                return selectItems;
            }
        }
    }
}