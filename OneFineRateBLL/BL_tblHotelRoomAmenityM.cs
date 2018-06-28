using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;

namespace OneFineRateBLL
{
    public class BL_tblHotelRoomAmenityM
    {
        public static List<etblHotelRoomAmenityM> GetHotelRoomAmenityRadio()
        {
            List<etblHotelRoomAmenityM> eobj = new List<etblHotelRoomAmenityM>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                foreach (OneFineRate.tblHotelRoomAmenityM item in db.tblHotelRoomAmenityMs.Where(u => u.cRadioCheckBox == "R").AsQueryable().ToList())
                {
                    eobj.Add((etblHotelRoomAmenityM)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblHotelRoomAmenityM()));
                }
            }
            return eobj;
        }
        public static List<CheckBoxList> GetHotelRoomAmenityCheckBox(string data, string newdata = null)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                string[] objSelectItems=null;
                if (data != null)
                {
                    objSelectItems = data.Split(',');
                    if (newdata != null)
                    {
                        objSelectItems = objSelectItems.Union(newdata.Split(',')).ToArray();
                    }
                   
                    selectItems = (from am in db.tblHotelRoomAmenityMs.Where(u => u.cRadioCheckBox == "C").ToList()
                                   join si in objSelectItems.ToList() on am.iHotelRoomAmenityId equals Convert.ToInt32(si) into result
                                   from r in result.DefaultIfEmpty()
                                   select new CheckBoxList()
                                   {
                                       Text = am.sRoomAmenity,
                                       Value = am.iHotelRoomAmenityId,
                                       Selected = r == null ? false : true
                                   }).ToList();
                }
                else if (newdata != null)
                {
                    objSelectItems = newdata.Split(',');
                    selectItems = (from am in db.tblHotelRoomAmenityMs.Where(u => u.cRadioCheckBox == "C").ToList()
                                   join si in objSelectItems.ToList() on am.iHotelRoomAmenityId equals Convert.ToInt32(si) into result
                                   from r in result.DefaultIfEmpty()
                                   select new CheckBoxList()
                                   {
                                       Text = am.sRoomAmenity,
                                       Value = am.iHotelRoomAmenityId,
                                       Selected = r == null ? false : true
                                   }).ToList();
                }
                else
                {
                    selectItems = db.tblHotelRoomAmenityMs.Where(u => u.cRadioCheckBox == "C").Select(x => new CheckBoxList()
                    {
                        Text = x.sRoomAmenity,
                        Value = x.iHotelRoomAmenityId
                    }).ToList();
                }
                return selectItems;
            }
        }
    }
}