using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;
using System.Web.Mvc;
using System.Linq.Expressions;
using OneFineRateAppUtil;
using OneFineRateBLL.WebUserEntities;

namespace OneFineRateBLL
{
    public class BL_NegotiationBooking
    {

        public static int AddRecord(PropDetailsM eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    GuestUserDetails objDetail = new GuestUserDetails();
                    objDetail.Title = eobj.sUserTitle;
                    objDetail.FirstName = eobj.sUserFirstName;
                    objDetail.LastName = eobj.sUserLastName;
                    objDetail.Email = eobj.sUserEmail;
                    objDetail.PhoneNumber = eobj.sUserMobileNo;
                    objDetail.sCountryPhoneCode = eobj.sCountryPhoneCode;
                    objDetail.iStateId = eobj.iStateId;

                    OneFineRate.tblWebsiteGuestUserMaster dbuser = (OneFineRate.tblWebsiteGuestUserMaster)OneFineRateAppUtil.clsUtils.ConvertToObject(objDetail, new OneFineRate.tblWebsiteGuestUserMaster());
                    if (eobj.objBooking.iBookingId>0)
                    {

                        var guestId=db.tblBookingTxes.Where(x => x.iBookingId == eobj.objBooking.iBookingId).Select(x => x.iGuestId).SingleOrDefault();

                        dbuser.Id = guestId == null ? 0 : guestId.Value;
                        db.tblWebsiteGuestUserMasters.Attach(dbuser);
                        db.Entry(dbuser).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        db.tblWebsiteGuestUserMasters.Add(dbuser);
                    }
                    db.SaveChanges();
                    retval = Convert.ToInt32(dbuser.Id);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retval;
        }
        //Get Single Record
        public static GuestUserDetails GetSingleRecordById(int id)
        {
            GuestUserDetails eobj = new GuestUserDetails();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblWebsiteGuestUserMasters.SingleOrDefault(u => u.Id == id);
                if (dbobj != null)
                    eobj = (GuestUserDetails)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }

        //Get Single Record
        public static WebsiteUser GetGuestUserDetailById(long customerId)
        {
            WebsiteUser guestUser = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblWebsiteGuestUserMasters.Find(customerId);
                if (dbobj != null)
                {
                    guestUser = new WebsiteUser
                    {
                        Id = dbobj.Id,
                        FirstName = dbobj.FirstName,
                        LastName = dbobj.LastName,
                        PhoneNumber = dbobj.PhoneNumber,
                        sCountryPhoneCode = dbobj.sCountryPhoneCode
                    };
                }
                return guestUser;
            }
        }
    }
}