using OneFineRate;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_UserFeedbackTripAdvisor
    {
        #region functions
        //Add new record
        public static int AddRecord(etblUserFeedbackTripAdvisor eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {

                    OneFineRate.tblUserFeedbackTripAdvisor dbuser = (OneFineRate.tblUserFeedbackTripAdvisor)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblUserFeedbackTripAdvisor());
                    db.tblUserFeedbackTripAdvisors.Add(dbuser);
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
        //Delete a record
        public static int DeleteRecord(int bookingId)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var obj = db.tblUserFeedbackTripAdvisors.SingleOrDefault(u => u.iBookingId == bookingId);
                    db.tblUserFeedbackTripAdvisors.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
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
        //Update a record
        public static int UpdateRecord(etblUserFeedbackTripAdvisor eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblUserFeedbackTripAdvisor obj = (OneFineRate.tblUserFeedbackTripAdvisor)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblUserFeedbackTripAdvisor());
                    db.tblUserFeedbackTripAdvisors.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
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

        //Get Single Record
        public static etblUserFeedbackTripAdvisor GetSingleRecordById(int bookingId)
        {
            etblUserFeedbackTripAdvisor eobj = new etblUserFeedbackTripAdvisor();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblUserFeedbackTripAdvisors.SingleOrDefault(u => u.iBookingId == bookingId);
                if (dbobj != null)
                    eobj = (etblUserFeedbackTripAdvisor)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;
        }


        #endregion
    }
}