using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;

namespace OneFineRateBLL
{
    public class BL_tblOnsiteRecreationFacilitiesM
    {
        public static List<CheckBoxList> GetOnsiteRecreationFacilities(string data)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                string[] objSelectItems;
                if (data != null)
                {
                    objSelectItems = data.Split(',');
                    selectItems = (from am in db.tblOnsiteRecreationFacilitiesMs.ToList()
                                   join si in objSelectItems.ToList() on am.iRecreationFacilityId equals Convert.ToInt32(si) into result
                                   from r in result.DefaultIfEmpty()
                                   select new CheckBoxList()
                                   {
                                       Text = am.sRecreationFacility,
                                       Value = am.iRecreationFacilityId,
                                       Selected = r == null ? false : true
                                   }).ToList();
                }
                else
                {
                    selectItems = db.tblOnsiteRecreationFacilitiesMs.Select(x => new CheckBoxList()
                    {
                        Text = x.sRecreationFacility,
                        Value = x.iRecreationFacilityId
                    }).ToList();
                }
                return selectItems;
            }
        }
    }
}