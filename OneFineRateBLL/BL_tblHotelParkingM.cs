using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;

namespace OneFineRateBLL
{
    public class BL_tblHotelParkingM
    {
        public static List<etblHotelParkingM> GetHotelParkingsRadio()
        {
            List<etblHotelParkingM> eobj = new List<etblHotelParkingM>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                foreach (OneFineRate.tblHotelParkingM item in db.tblHotelParkingMs.Where(u=>u.cRadioCheckBox=="R").AsQueryable().ToList())
                {
                    eobj.Add((etblHotelParkingM)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblHotelParkingM()));
                }
            }
            return eobj;
        }
        public static List<CheckBoxList> GetHotelParkingsCheckBox(string data)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                string[] objSelectItems;
                if (data != null)
                {
                    objSelectItems = data.Split(',');
                    selectItems = (from am in db.tblHotelParkingMs.Where(u=>u.cRadioCheckBox=="C").ToList()
                                   join si in objSelectItems.ToList() on am.iHotelParkingId equals Convert.ToInt32(si) into result
                                   from r in result.DefaultIfEmpty()
                                   select new CheckBoxList()
                                   {
                                       Text = am.sParking,
                                       Value = am.iHotelParkingId,
                                       Selected = r == null ? false : true
                                   }).ToList();
                }
                else
                {
                    selectItems = db.tblHotelParkingMs.Where(u=>u.cRadioCheckBox=="C").Select(x => new CheckBoxList()
                    {
                        Text = x.sParking,
                        Value = x.iHotelParkingId
                    }).ToList();
                }
                return selectItems;
            }
        }
    }
}