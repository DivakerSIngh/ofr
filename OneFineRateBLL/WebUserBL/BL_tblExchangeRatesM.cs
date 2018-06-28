using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;

namespace OneFineRateBLL.WebUserBL
{
    public class BL_tblExchangeRatesM
    {
        public static etblExchangeRatesM GetSingleRecordById(string CurrencyCode)
        {
            etblExchangeRatesM eobj = new etblExchangeRatesM();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblExchangeRatesMs.SingleOrDefault(u => u.sCurrencyCodeFrom == CurrencyCode && u.sCurrencyCodeTo == "INR");
                if (dbobj != null)
                    eobj = (etblExchangeRatesM)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }
    }
}