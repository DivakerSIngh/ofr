using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;
using System.Web.Mvc;
namespace OneFineRateBLL
{
    public class BL_tblHotelLeisureM
    {
        public static List<CheckBoxList> GetPropertyLeisure(string data)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                string[] objSelectItems;
                if (data != null)
                {
                    objSelectItems = data.Split(',');
                    selectItems = (from am in db.tblHotelLeisureMs.ToList()
                                   join si in objSelectItems.ToList() on am.iHotelLeisureId equals Convert.ToInt32(si) into result
                                   from r in result.DefaultIfEmpty()
                                   select new CheckBoxList()
                                   {
                                       Text = am.sLeisure,
                                       Value = am.iHotelLeisureId,
                                       Selected = r == null ? false : true
                                   }).ToList();
                }
                else
                {
                    selectItems = db.tblHotelLeisureMs.Select(x => new CheckBoxList()
                    {
                        Text = x.sLeisure,
                        Value = x.iHotelLeisureId
                    }).ToList();
                }
                return selectItems;
            }
        }
    }
}