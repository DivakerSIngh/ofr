using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;

namespace OneFineRateBLL
{
    public class BL_tblPropertyPolicyMap
    {
        //get single record
        public static etblPropertyPolicyMap GetSingleRecordById(int id)
        {
            etblPropertyPolicyMap eobj = new etblPropertyPolicyMap();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblPropertyPolicyMaps.SingleOrDefault(u => u.iPropId == id);
                if (dbobj != null)
                    eobj = (etblPropertyPolicyMap)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }
        //Update a record
        public static int UpdateRecord(etblPropertyPolicyMap eobj)
        {
            bool Type = false;
            int retval = 0;
            using (OneFineRateEntities dbnew = new OneFineRateEntities())
            {
                try
                {
                    var dbobj = dbnew.tblPropertyPolicyMaps.SingleOrDefault(u => u.iPropId == eobj.iPropId);
                    if (dbobj != null)
                    {
                        Type = true;
                    }

                }
                catch (Exception)
                {
                    throw;
                }
            }
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    if (Type)
                    {
                        OneFineRate.tblPropertyPolicyMap obj = (OneFineRate.tblPropertyPolicyMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyPolicyMap());
                        db.tblPropertyPolicyMaps.Attach(obj);
                        db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        retval = 1;
                    }
                    else
                    {
                        OneFineRate.tblPropertyPolicyMap dbuser = (OneFineRate.tblPropertyPolicyMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyPolicyMap());
                        db.tblPropertyPolicyMaps.Add(dbuser);
                        db.SaveChanges();
                        retval = 1;
                    }

                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return retval;
        }
    }
}