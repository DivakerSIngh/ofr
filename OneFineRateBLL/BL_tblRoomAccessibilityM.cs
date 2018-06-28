using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;

namespace OneFineRateBLL
{
    public class BL_tblRoomAccessibilityM
    {
        public static List<CheckBoxList> GetRoomAccessibility(string data)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                string[] objSelectItems;
                if (data != null)
                {
                    objSelectItems = data.Split(',');
                    selectItems = (from am in db.tblRoomAccessibilityMs.ToList()
                                   join si in objSelectItems.ToList() on am.iRoomAccessibilityId equals Convert.ToInt32(si) into result
                                   from r in result.DefaultIfEmpty()
                                   select new CheckBoxList()
                                   {
                                       Text = am.sRoomAccessibility,
                                       Value = am.iRoomAccessibilityId,
                                       Selected = r == null ? false : true
                                   }).ToList();
                }
                else
                {
                    selectItems = db.tblRoomAccessibilityMs.Select(x => new CheckBoxList()
                    {
                        Text = x.sRoomAccessibility,
                        Value = x.iRoomAccessibilityId
                    }).ToList();
                }
                return selectItems;
            }
        }
    }
}