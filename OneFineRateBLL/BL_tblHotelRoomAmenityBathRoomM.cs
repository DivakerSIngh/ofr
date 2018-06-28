using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;

namespace OneFineRateBLL
{
    public class BL_tblHotelRoomAmenityBathRoomM
    {
        public static List<CheckBoxList> GetHotelRoomAmenityBathRoom(string data, string newdata = null)
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
                   
                    selectItems = (from am in db.tblHotelRoomAmenityBathRoomMs.ToList()
                                   join si in objSelectItems.ToList() on am.iHotelRoomAmenityBathRoomId equals Convert.ToInt32(si) into result
                                   from r in result.DefaultIfEmpty()
                                   select new CheckBoxList()
                                   {
                                       Text = am.sRoomAmenityBathRoom,
                                       Value = am.iHotelRoomAmenityBathRoomId,
                                       Selected = r == null ? false : true
                                   }).ToList();
                }
                else if (newdata != null)
                {
                    objSelectItems = newdata.Split(',');
                    selectItems = (from am in db.tblHotelRoomAmenityBathRoomMs.ToList()
                                   join si in objSelectItems.ToList() on am.iHotelRoomAmenityBathRoomId equals Convert.ToInt32(si) into result
                                   from r in result.DefaultIfEmpty()
                                   select new CheckBoxList()
                                   {
                                       Text = am.sRoomAmenityBathRoom,
                                       Value = am.iHotelRoomAmenityBathRoomId,
                                       Selected = r == null ? false : true
                                   }).ToList();
                }
                else
                {
                    selectItems = db.tblHotelRoomAmenityBathRoomMs.Select(x => new CheckBoxList()
                    {
                        Text = x.sRoomAmenityBathRoom,
                        Value = x.iHotelRoomAmenityBathRoomId
                    }).ToList();
                }
                return selectItems;
            }
        }
    }
}