using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;
using System.Web.Mvc;

namespace OneFineRateBLL
{
    public class BL_tblAccessbilityM
    {
        public static List<CheckBoxList> GetAccessbilities(string data)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                string[] objSelectItems;
                if (data != null)
                {
                    objSelectItems = data.Split(',');
                    selectItems = (from am in db.tblAccessbilityMs.ToList()
                                   join si in objSelectItems.ToList() on am.iAccessibilityId equals Convert.ToInt32(si) into result
                                   from r in result.DefaultIfEmpty()
                                   select new CheckBoxList()
                                   {
                                       Text = am.sAccessibility,
                                       Value = am.iAccessibilityId,
                                       Selected = r == null ? false : true
                                   }).ToList();
                }
                else
                {
                    selectItems = db.tblAccessbilityMs.Select(x => new CheckBoxList()
                    {
                        Text = x.sAccessibility,
                        Value = x.iAccessibilityId
                    }).ToList();
                }
                return selectItems;
            }
        }
    }
}