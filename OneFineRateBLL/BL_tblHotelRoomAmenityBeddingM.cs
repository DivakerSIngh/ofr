using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;

namespace OneFineRateBLL
{
    public class BL_tblHotelRoomAmenityBeddingM
    {
        public static List<CheckBoxList> GetHotelRoomAmenityBedding(string data, string newdata = null)
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

                    selectItems = (from am in db.tblHotelRoomAmenityBeddingMs.ToList()
                                   join si in objSelectItems.ToList() on am.iHotelRoomAmenityBeddingId equals Convert.ToInt32(si) into result
                                   from r in result.DefaultIfEmpty()
                                   select new CheckBoxList()
                                   {
                                       Text = am.sRoomAmenityBedding,
                                       Value = am.iHotelRoomAmenityBeddingId,
                                       Selected = r == null ? false : true
                                   }).ToList();
                }
                else if (newdata != null)
                {
                    objSelectItems = newdata.Split(',');
                    selectItems = (from am in db.tblHotelRoomAmenityBeddingMs.ToList()
                                   join si in objSelectItems.ToList() on am.iHotelRoomAmenityBeddingId equals Convert.ToInt32(si) into result
                                   from r in result.DefaultIfEmpty()
                                   select new CheckBoxList()
                                   {
                                       Text = am.sRoomAmenityBedding,
                                       Value = am.iHotelRoomAmenityBeddingId,
                                       Selected = r == null ? false : true
                                   }).ToList();
                }
                else
                {
                    selectItems = db.tblHotelRoomAmenityBeddingMs.Select(x => new CheckBoxList()
                    {
                        Text = x.sRoomAmenityBedding,
                        Value = x.iHotelRoomAmenityBeddingId
                    }).ToList();
                }
                return selectItems;
            }
        }
    }
}