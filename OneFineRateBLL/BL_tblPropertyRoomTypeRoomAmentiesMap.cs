using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;

namespace OneFineRateBLL
{
    public class BL_tblPropertyRoomTypeRoomAmentiesMap
    {
        public static etblPropertyRoomTypeRoomAmentiesMap GetSingleRecordById(int id)
        {
            etblPropertyRoomTypeRoomAmentiesMap eobj = new etblPropertyRoomTypeRoomAmentiesMap();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblPropertyRoomTypeRoomAmentiesMaps.SingleOrDefault(u => u.iRoomId == id);
                if (dbobj != null)
                    eobj = (etblPropertyRoomTypeRoomAmentiesMap)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }
        //Update a record
        public static int UpdateRecord(etblPropertyRoomTypeRoomAmentiesMap eobj)
        {
            bool Type = false;
            int retval = 0;
            using (OneFineRateEntities dbnew = new OneFineRateEntities())
            {
                try
                {
                    var dbobj = dbnew.tblPropertyRoomTypeRoomAmentiesMaps.SingleOrDefault(u => u.iPropId == eobj.iPropId && u.iRoomId==eobj.iRoomId);
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
                        OneFineRate.tblPropertyRoomTypeRoomAmentiesMap obj = (OneFineRate.tblPropertyRoomTypeRoomAmentiesMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyRoomTypeRoomAmentiesMap());
                        db.tblPropertyRoomTypeRoomAmentiesMaps.Attach(obj);
                        db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        db.uspUpdateAmenitiesInLookup(eobj.iPropId, eobj.iRoomId);
                        retval = 1;
                    }
                    else
                    {
                        OneFineRate.tblPropertyRoomTypeRoomAmentiesMap dbuser = (OneFineRate.tblPropertyRoomTypeRoomAmentiesMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyRoomTypeRoomAmentiesMap());
                        db.tblPropertyRoomTypeRoomAmentiesMaps.Add(dbuser);
                        db.SaveChanges();

                        db.uspUpdateAmenitiesInLookup(eobj.iPropId, eobj.iRoomId);
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