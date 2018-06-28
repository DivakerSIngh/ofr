using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;

namespace OneFineRateBLL
{
    public class BL_tblPropertyRoomAmentiesMap
    {
        //get Single record
        public static etblPropertyRoomAmentiesMap GetSingleRecordById(int id)
        {
            etblPropertyRoomAmentiesMap eobj = new etblPropertyRoomAmentiesMap();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblPropertyRoomAmentiesMaps.SingleOrDefault(u => u.iPropId == id);
                if (dbobj != null)
                    eobj = (etblPropertyRoomAmentiesMap)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }
     
        //Update a record
        public static int UpdateRecord(etblPropertyRoomAmentiesMap eobj)
        {
            bool Type = false;
            int retval = 0;
            using (OneFineRateEntities dbnew = new OneFineRateEntities())
            {
                try
                {
                    var dbobj = dbnew.tblPropertyRoomAmentiesMaps.SingleOrDefault(u => u.iPropId == eobj.iPropId);
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
                        OneFineRate.tblPropertyRoomAmentiesMap obj = (OneFineRate.tblPropertyRoomAmentiesMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyRoomAmentiesMap());
                        db.tblPropertyRoomAmentiesMaps.Attach(obj);
                        db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        db.uspUpdateAmenitiesInLookup(eobj.iPropId, 0);
                        retval = 1;
                    }
                    else
                    {
                        OneFineRate.tblPropertyRoomAmentiesMap dbuser = (OneFineRate.tblPropertyRoomAmentiesMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyRoomAmentiesMap());
                        db.tblPropertyRoomAmentiesMaps.Add(dbuser);
                        db.SaveChanges();

                        db.uspUpdateAmenitiesInLookup(eobj.iPropId, 0);
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