using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;

namespace OneFineRateBLL
{
    public class BL_tblPropertyRoomRatePlanInventoryMap
    {
        //Get Single Record
        public static etblPropertyRoomRatePlanInventoryMap GetSingleRecordById(int roomid, int rateplanid, DateTime inventorydate)
        {
            etblPropertyRoomRatePlanInventoryMap eobj = new etblPropertyRoomRatePlanInventoryMap();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = (from t2 in db.tblPropertyRatePlanMaps
                             join t1 in db.tblPropertyRoomRatePlanInventoryMaps on t2.iRPId equals t1.iRPId into S1
                             from a in S1.DefaultIfEmpty()
                             join t3 in db.tblPropertyRoomMaps on a.iRoomId equals t3.iRoomId into S2
                             from b in S2.DefaultIfEmpty()
                             select new
                             {
                                 a.dtInventoryDate,
                                 a.iPropId,
                                 a.iRoomId,
                                 a.iRPId,
                                 a.bCloseOut,
                                 iMinLengthStay = a.iMinLengthStay == null ? t2.iMinLengthStay : a.iMinLengthStay,
                                 iMaxLengthStay = a.iMaxLengthStay == null ? t2.iMaxLengthStay : a.iMaxLengthStay,
                                 a.bCTA,
                                 a.bCTD,
                                 a.dSingleRate,
                                 a.dDoubleRate,
                                 a.dTripleRate,
                                 a.dQuadrupleRate,
                                 a.dQuintrupleRate,
                                 a.dtActionDate,
                                 iActionBy = t2.bLinkToExistingRatePlan == true ? 1 : 0,
                                 t2.sRatePlan,
                                 b.sRoomName
                             }).Where(u => u.dtInventoryDate == inventorydate && u.iRoomId == roomid && u.iRPId == rateplanid).AsQueryable().SingleOrDefault();

                if (dbobj == null)
                {
                    var NewdbObj = (from t2 in db.tblPropertyRatePlanMaps
                                    join t3 in db.tblPropertyRoomMaps on t2.iPropId equals t3.iPropId into S2
                                    from b in S2.DefaultIfEmpty()
                                    select new
                                    {
                                        iActionBy = t2.bLinkToExistingRatePlan == true ? 1 : 0,
                                        t2.sRatePlan,
                                        b.sRoomName,
                                        b.iRoomId,
                                        t2.iRPId
                                    }).Where(u => u.iRoomId == roomid && u.iRPId == rateplanid).AsQueryable().SingleOrDefault();
                    eobj = (etblPropertyRoomRatePlanInventoryMap)OneFineRateAppUtil.clsUtils.ConvertToObject(NewdbObj, eobj);
                }
                else
                {
                    eobj = (etblPropertyRoomRatePlanInventoryMap)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
                }
            }
            return eobj;

        }

        //Update a record
        public static int UpdateRecord(etblPropertyRoomRatePlanInventoryMap eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    //remove old mapings

                    db.tblPropertyRoomRatePlanInventoryMaps.RemoveRange(db.tblPropertyRoomRatePlanInventoryMaps.Where(x => x.iRoomId == eobj.iRoomId && x.dtInventoryDate == eobj.dtInventoryDate && x.iRPId == eobj.iRPId));

                    OneFineRate.tblPropertyRoomRatePlanInventoryMap obj = (OneFineRate.tblPropertyRoomRatePlanInventoryMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyRoomRatePlanInventoryMap());
                    db.tblPropertyRoomRatePlanInventoryMaps.Add(obj);

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