using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;
using System.Web.Mvc;

namespace OneFineRateBLL
{
    public class BL_tblHotelConvenienceM
    {
        public static List<CheckBoxList> GetPropertyConveniences(string data)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                string[] objSelectItems;
                if (data != null)
                {
                    objSelectItems = data.Split(',');
                    selectItems = (from am in db.tblHotelConvenienceMs.ToList()
                                   join si in objSelectItems.ToList() on am.iHotelConvenienceId equals Convert.ToInt32(si) into result
                                   from r in result.DefaultIfEmpty()
                                   select new CheckBoxList()
                                   {
                                       Text = am.sConvenience,
                                       Value = am.iHotelConvenienceId,
                                       Selected = r == null ? false : true
                                   }).ToList();
                }
                else
                {
                    selectItems = db.tblHotelConvenienceMs.Select(x => new CheckBoxList()
                    {
                        Text = x.sConvenience,
                        Value = x.iHotelConvenienceId
                    }).ToList();
                }
                return selectItems;
            }
        }
    }
}