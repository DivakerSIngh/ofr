using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;

namespace OneFineRateBLL
{
    public class BL_tblPropertyRecreationMap
    {
        //Get Single Record
        public static etblPropertyRecreationMap GetSingleRecordById(int id)
        {
            etblPropertyRecreationMap eobj = new etblPropertyRecreationMap();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblPropertyRecreationMaps.SingleOrDefault(u => u.iPropId == id);
                if (dbobj != null)
                    eobj = (etblPropertyRecreationMap)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }
        //Update a record
        public static int UpdateRecord(etblPropertyRecreationMap eobj)
        {
            bool Type = false;
            int retval = 0;
            using (OneFineRateEntities dbnew = new OneFineRateEntities())
            {
                try
                {
                    var dbobj = dbnew.tblPropertyRecreationMaps.SingleOrDefault(u => u.iPropId == eobj.iPropId);
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
                        OneFineRate.tblPropertyRecreationMap obj = (OneFineRate.tblPropertyRecreationMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyRecreationMap());
                        db.tblPropertyRecreationMaps.Attach(obj);
                        db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        retval = 1;
                    }
                    else
                    {
                        OneFineRate.tblPropertyRecreationMap dbuser = (OneFineRate.tblPropertyRecreationMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyRecreationMap());
                        db.tblPropertyRecreationMaps.Add(dbuser);
                        db.SaveChanges();
                        retval = 1;
                    }

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