using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateBLL.Entities;
using OneFineRate;

namespace OneFineRateBLL
{
    
    public class BL_tblLanguageM
    {
        public static List<CheckBoxList> GetLanguages(string data)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                string[] objSelectItems;
                if (data != null)
                {
                    objSelectItems = data.Split(',');
                    selectItems = (from am in db.tblLanguageMs.ToList()
                                   join si in objSelectItems.ToList() on am.iLanguageId equals Convert.ToInt32(si) into result
                                   from r in result.DefaultIfEmpty()
                                   select new CheckBoxList()
                                   {
                                       Text = am.sLanguage,
                                       Value = am.iLanguageId,
                                       Selected = r == null ? false : true
                                   }).ToList();
                }
                else
                {
                    selectItems = db.tblLanguageMs.Select(x => new CheckBoxList()
                    {
                        Text = x.sLanguage,
                        Value = x.iLanguageId
                    }).ToList();
                }



                return selectItems;
            }
        }

    }
}