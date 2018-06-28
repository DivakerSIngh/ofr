using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;

namespace OneFineRateBLL
{
    public class BL_tblVideoUrlM
    {
        //get url name
        public static etblVideoUrlM GetParameters()
        {
            etblVideoUrlM eobj = new etblVideoUrlM();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblVideoUrlMs.SingleOrDefault();
                if (dbobj != null)
                    eobj = ((etblVideoUrlM)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, new etblVideoUrlM()));
            }
            return eobj;

        }
    }
}