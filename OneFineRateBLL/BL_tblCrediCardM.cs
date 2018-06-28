using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;

namespace OneFineRateBLL
{
    public class BL_tblCrediCardM
    {
        public static List<CheckBoxList> GetCrediCards(string data)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                string[] objSelectItems;
                if (data != null)
                {
                    objSelectItems = data.Split(',');
                    selectItems = (from am in db.tblCrediCardMs.ToList()
                                   join si in objSelectItems.ToList() on am.iCreditCardId equals Convert.ToInt32(si) into result
                                   from r in result.DefaultIfEmpty()
                                   select new CheckBoxList()
                                   {
                                       Text = am.sCreditCard,
                                       Value = am.iCreditCardId,
                                       Selected = r == null ? false : true
                                   }).ToList();
                }
                else
                {
                    selectItems = db.tblCrediCardMs.Select(x => new CheckBoxList()
                    {
                        Text = x.sCreditCard,
                        Value = x.iCreditCardId
                    }).ToList();
                }
                return selectItems;
            }
        }
    }
}