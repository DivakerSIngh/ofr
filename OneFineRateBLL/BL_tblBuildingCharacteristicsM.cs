using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;

namespace OneFineRateBLL
{
    public class BL_tblBuildingCharacteristicsM
    {
        public static List<CheckBoxList> GetBuildingCharacteristics(string data)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                string[] objSelectItems;
                if (data != null)
                {
                    objSelectItems = data.Split(',');
                    selectItems = (from am in db.tblBuildingCharacteristicsMs.ToList()
                                   join si in objSelectItems.ToList() on am.iBuildingCharacteristicsId equals Convert.ToInt32(si) into result
                                   from r in result.DefaultIfEmpty()
                                   select new CheckBoxList()
                                   {
                                       Text = am.sBuildingCharacteristics,
                                       Value = am.iBuildingCharacteristicsId,
                                       Selected = r == null ? false : true
                                   }).ToList();
                }
                else
                {
                    selectItems = db.tblBuildingCharacteristicsMs.Select(x => new CheckBoxList()
                    {
                        Text = x.sBuildingCharacteristics,
                        Value = x.iBuildingCharacteristicsId
                    }).ToList();
                }
                return selectItems;
            }
        }
    }
}