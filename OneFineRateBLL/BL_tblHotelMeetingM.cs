using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;
using System.Web.Mvc;
namespace OneFineRateBLL
{
    public class BL_tblHotelMeetingM
    {
        public static List<CheckBoxList> GetPropertyMeetings(string data)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                string[] objSelectItems;
                if (data != null)
                {
                    objSelectItems = data.Split(',');
                    selectItems = (from am in db.tblHotelMeetingMs.ToList()
                                   join si in objSelectItems.ToList() on am.iHotelMeetingId equals Convert.ToInt32(si) into result
                                   from r in result.DefaultIfEmpty()
                                   select new CheckBoxList()
                                   {
                                       Text = am.sMeeting,
                                       Value = am.iHotelMeetingId,
                                       Selected = r == null ? false : true
                                   }).ToList();
                }
                else
                {
                    selectItems = db.tblHotelMeetingMs.Select(x => new CheckBoxList()
                    {
                        Text = x.sMeeting,
                        Value = x.iHotelMeetingId
                    }).ToList();
                }
                return selectItems;
            }
        }
    }
}