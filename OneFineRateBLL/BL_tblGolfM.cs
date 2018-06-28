using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;

namespace OneFineRateBLL
{
    public class BL_tblGolfM
    {
        public static List<CheckBoxList> GetGolf(string data)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                string[] objSelectItems;
                if (data != null)
                {
                    objSelectItems = data.Split(',');
                    selectItems = (from am in db.tblGolfMs.ToList()
                                   join si in objSelectItems.ToList() on am.iGolfId equals Convert.ToInt32(si) into result
                                   from r in result.DefaultIfEmpty()
                                   select new CheckBoxList()
                                   {
                                       Text = am.sGolf,
                                       Value = am.iGolfId,
                                       Selected = r == null ? false : true
                                   }).ToList();
                }
                else
                {
                    selectItems = db.tblGolfMs.Select(x => new CheckBoxList()
                    {
                        Text = x.sGolf,
                        Value = x.iGolfId
                    }).ToList();
                }
                return selectItems;
            }
        }
    }
}