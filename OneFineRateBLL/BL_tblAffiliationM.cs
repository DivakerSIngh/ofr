using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRate;
using OneFineRateBLL.Entities;

namespace OneFineRateBLL
{
    public class BL_tblAffiliationM
    {
        public static List<CheckBoxList> GetAffilations(string data)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                string[] objSelectItems;

                if (data != null)
                {
                    objSelectItems = data.Split(',');
                    selectItems = (from am in db.tblAffiliationMs.ToList()
                                   join si in objSelectItems.ToList() on am.iAffiliationId equals Convert.ToInt32(si) into result
                                   from r in result.DefaultIfEmpty()
                                   select new CheckBoxList()
                                   {
                                       Text = am.sAffiliation,
                                       Value = am.iAffiliationId,
                                       Selected = r == null ? false : true
                                   }).ToList();
                }
                else
                {
                    selectItems = db.tblAffiliationMs.Select(x => new CheckBoxList()
                    {
                        Text = x.sAffiliation,
                        Value = x.iAffiliationId
                    }).ToList();
                }
                return selectItems;
            }
        }
    }
}