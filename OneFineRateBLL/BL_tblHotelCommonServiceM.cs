using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;
using System.Web.Mvc;

namespace OneFineRateBLL
{
    public class BL_tblHotelCommonServiceM
    {
        public static List<CheckBoxList> GetPropertyCommonServices(string data)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                string[] objSelectItems;
                if (data != null)
                {
                    objSelectItems = data.Split(',');
                    selectItems = (from am in db.tblHotelCommonServiceMs.ToList()
                                   join si in objSelectItems.ToList() on am.iHotelCommonServiceId equals Convert.ToInt32(si) into result
                                   from r in result.DefaultIfEmpty()
                                   select new CheckBoxList()
                                   {
                                       Text = am.sCommonService,
                                       Value = am.iHotelCommonServiceId,
                                       Selected = r == null ? false : true
                                   }).ToList();
                }
                else
                {
                    selectItems = db.tblHotelCommonServiceMs.Select(x => new CheckBoxList()
                    {
                        Text = x.sCommonService,
                        Value = x.iHotelCommonServiceId
                    }).ToList();
                }
                return selectItems;
            }
        }
    }
}