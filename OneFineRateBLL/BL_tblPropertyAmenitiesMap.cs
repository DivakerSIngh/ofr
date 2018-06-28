using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;

namespace OneFineRateBLL
{
    public class BL_tblPropertyAmenitiesMap
    {
        //Get Single Record
        public static etblPropertyAmenitiesMap GetSingleRecordById(int id)
        {
            etblPropertyAmenitiesMap eobj = new etblPropertyAmenitiesMap();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblPropertyAmenitiesMaps.SingleOrDefault(u => u.iPropId == id);
                if (dbobj != null)
                    eobj = (etblPropertyAmenitiesMap)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }
     
        //Update a record
        public static int UpdateRecord(etblPropertyAmenitiesMap eobj)
        {
            bool Type = false;
            int retval = 0;
            using (OneFineRateEntities dbnew = new OneFineRateEntities())
            {
                try
                {
                    var dbobj = dbnew.tblPropertyAmenitiesMaps.SingleOrDefault(u => u.iPropId == eobj.iPropId);
                    if(dbobj!=null)
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

                    //remove old mapings
                    db.tblPropertyParkingMaps.RemoveRange(db.tblPropertyParkingMaps.Where(x => x.iPropId == eobj.iPropId));

                    //Add mapings
                    if (eobj.HotelParkingsMapList != null)
                    {
                        db.tblPropertyParkingMaps.AddRange(eobj.HotelParkingsMapList.Select(x => new tblPropertyParkingMap()
                        {
                            iPropId = x.iPropId,
                            sCarName = x.sCarName,
                            cAirportRail = x.cAirportRail,
                            bIsFree=x.bIsFree,
                            dOnewayCharge=x.dOnewayCharge,
                            dTwowayCharge=x.dTwowayCharge,
                            dtActionDate = x.dtActionDate,
                            iActionBy = x.iActionBy
                        }).ToList());
                    }
                    if (Type)
                    {
                        OneFineRate.tblPropertyAmenitiesMap obj = (OneFineRate.tblPropertyAmenitiesMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyAmenitiesMap());
                        db.tblPropertyAmenitiesMaps.Attach(obj);
                        db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        retval = 1;
                    }
                    else
                    {
                        OneFineRate.tblPropertyAmenitiesMap dbuser = (OneFineRate.tblPropertyAmenitiesMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyAmenitiesMap());
                        db.tblPropertyAmenitiesMaps.Add(dbuser);
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
        public static List<etblPropertyParkingMap> GetPropertyParkingList(int id)
        {
            List<etblPropertyParkingMap> eobj = new List<etblPropertyParkingMap>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                foreach (OneFineRate.tblPropertyParkingMap item in db.tblPropertyParkingMaps.Where(u => u.iPropId == id).AsQueryable().ToList())
                {
                    eobj.Add((etblPropertyParkingMap)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblPropertyParkingMap()));
                }
            }
            return eobj;
        }
    }
}