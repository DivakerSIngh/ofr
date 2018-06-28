using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;

namespace OneFineRateBLL
{
    public class BL_tblLandActivitiesM
    {
        public static List<CheckBoxList> GetLandActivities(string data)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                string[] objSelectItems;
                if (data != null)
                {
                    objSelectItems = data.Split(',');
                    selectItems = (from am in db.tblLandActivitiesMs.ToList()
                                   join si in objSelectItems.ToList() on am.iLandActivityId equals Convert.ToInt32(si) into result
                                   from r in result.DefaultIfEmpty()
                                   select new CheckBoxList()
                                   {
                                       Text = am.sLandActivity,
                                       Value = am.iLandActivityId,
                                       Selected = r == null ? false : true
                                   }).ToList();
                }
                else
                {
                    selectItems = db.tblLandActivitiesMs.Select(x => new CheckBoxList()
                    {
                        Text = x.sLandActivity,
                        Value = x.iLandActivityId
                    }).ToList();
                }
                return selectItems;
            }
        }
    }
}