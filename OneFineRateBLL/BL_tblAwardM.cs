using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;
using System.Web.Mvc;

namespace OneFineRateBLL
{
    public class BL_tblAwardM
    {
        public static List<CheckBoxList> GetAwards(string data)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                string[] objSelectItems;

                if (data != null)
                {
                    objSelectItems = data.Split(',');
                    selectItems = (from am in db.tblAwardMs.ToList()
                                   join si in objSelectItems.ToList() on am.iAwardId equals Convert.ToInt32(si) into result
                                   from r in result.DefaultIfEmpty()
                                   select new CheckBoxList()
                                   {
                                       Text = am.sAward,
                                       Value = am.iAwardId,
                                       Selected = r == null ? false : true
                                   }).ToList();
                }
                else
                {
                    selectItems = db.tblAwardMs.Select(x => new CheckBoxList()
                    {
                        Text = x.sAward,
                        Value = x.iAwardId
                    }).ToList();
                }
                return selectItems;
            }
        }
    }
}