using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;

namespace OneFineRateBLL
{
    public class BL_tblOfferReviewTrackerTx
    {
        //Add new record
        public static int AddRecord(etblOfferReviewTrackerTx eobj)
        {

            //eobj contins the hotel id, current date time and ipaddress 
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    //Convert the eobj into entity object of tblOfferReviewTrackerTx and save the data 
                    OneFineRate.tblOfferReviewTrackerTx dbuser = (OneFineRate.tblOfferReviewTrackerTx)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblOfferReviewTrackerTx());
                    db.tblOfferReviewTrackerTxes.Add(dbuser);
                    db.SaveChanges();
                    retval = 1;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retval;
        }
    }
}