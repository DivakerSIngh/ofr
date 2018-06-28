using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;

namespace OneFineRateBLL
{
    public class BL_tblRoomOutdoorViewM
    {
        public static List<CheckBoxList> GetRoomOutdoorViews(string data)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                string[] objSelectItems;
                if (data != null)
                {
                    objSelectItems = data.Split(',');
                    selectItems = (from am in db.tblRoomOutdoorViewMs.ToList()
                                   join si in objSelectItems.ToList() on am.iRoomOutdoorViewId equals Convert.ToInt32(si) into result
                                   from r in result.DefaultIfEmpty()
                                   select new CheckBoxList()
                                   {
                                       Text = am.sRoomOutdoorView,
                                       Value = am.iRoomOutdoorViewId,
                                       Selected = r == null ? false : true
                                   }).ToList();
                }
                else
                {
                    selectItems = db.tblRoomOutdoorViewMs.Select(x => new CheckBoxList()
                    {
                        Text = x.sRoomOutdoorView,
                        Value = x.iRoomOutdoorViewId
                    }).ToList();
                }
                return selectItems;
            }
        }
    }
}