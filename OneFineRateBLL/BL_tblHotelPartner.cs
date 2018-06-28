using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;
using System.Web.Mvc;
using System.Linq.Expressions;
using OneFineRateAppUtil;

namespace OneFineRateBLL
{
    public class BL_tblHotelPartner
    {

        public static int AddRecord(etblHotelPartnerM eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var checkExists = db.tblHotelPartnerMs.Any(
                                          p => p.sHotelName == eobj.sHotelName
                                               && p.sEmail == eobj.sEmail);

                    if (checkExists == false)
                    {
                        OneFineRate.tblHotelPartnerM dbuser = (OneFineRate.tblHotelPartnerM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblHotelPartnerM());
                        db.tblHotelPartnerMs.Add(dbuser);
                        db.SaveChanges();
                        retval = 1;
                    }
                    else
                    {
                        retval = 2;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retval;
        }


        public static string GetPartnerEmailId()
        {
            string Email = "";
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var objresult = (from s in db.tblEmailSettingsMs
                                     select new
                                         {
                                             s.sEmail,
                                             s.sModule
                                         }).Where(m => m.sModule == "Partner").AsQueryable().ToList();

                    Email = objresult[0].sEmail;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return Email;
        }


    }
}