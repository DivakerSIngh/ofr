using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;
using System.Data.Entity.Infrastructure;
using OneFineRateAppUtil;
using System.Transactions;

namespace OneFineRateBLL
{
    public class BL_tblPropertyPromotionMap
    {
        //Add new record
        public static int AddRecord(etblPropertyPromotionMap eobj, List<CancellationPolicyGrid> jArray)
        {
            int retval = 0;
            bool chkExists = false;
            using (var transactionScope = new TransactionScope())
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    try
                    {
                        if (jArray.Count > 0)
                        {
                            for (int i = 0; i < jArray.Count; i++)
                            {
                                if (eobj.sCancellationPolicy == null)
                                {
                                    eobj.sCancellationPolicy = jArray[i].sPolicyId;
                                }
                                else
                                {
                                    eobj.sCancellationPolicy = eobj.sCancellationPolicy + "," + jArray[i].sPolicyId;
                                }

                            }
                        }

                        if (eobj.iPromoId != Convert.ToInt32(Promotions.OFRFreebies))
                        {
                            #region Check Cancellation Policy Dates
                            int CancellationPolicyDateCount = 0;
                            int SelectedValidityDateCount = 0;

                            DateTime SelectedValidityCurrentDate = eobj.dtStayDateFrom;
                            while (SelectedValidityCurrentDate <= eobj.dtStayDateTo)
                            {
                                SelectedValidityDateCount++;
                                SelectedValidityCurrentDate = SelectedValidityCurrentDate.AddDays(1);
                            }

                            for (int i = 0; i < jArray.Count; i++)
                            {
                                DateTime CancellationPolicyCurrentDate = Convert.ToDateTime(clsUtils.ConvertddmmyyyytoDateTime(jArray[i].CancellationValidFrom));
                                while (CancellationPolicyCurrentDate <= Convert.ToDateTime(clsUtils.ConvertddmmyyyytoDateTime(jArray[i].CancellationValidTo)))
                                {
                                    CancellationPolicyDateCount++;
                                    CancellationPolicyCurrentDate = CancellationPolicyCurrentDate.AddDays(1);
                                }
                            }

                            if (SelectedValidityDateCount != CancellationPolicyDateCount)
                            {
                                return retval = 6;
                            }

                            #endregion

                            chkExists = db.tblPropertyPromotionMaps.Any(
                                             p => p.dtBookingDateFrom == eobj.dtBookingDateFrom
                                                 && p.dtBookingDateTo == eobj.dtBookingDateTo
                                                 && p.dtStayDateFrom == eobj.dtStayDateFrom
                                                 && p.dtStayDateTo == eobj.dtStayDateTo
                                                 && p.iRPId == eobj.iRPId
                                                 && p.bIsPercent == eobj.bIsPercent
                                                 && p.bIsPlus == eobj.bIsPlus
                                                 && p.dValue == eobj.dValue
                                                 && p.sAmenityId == eobj.sAmenityId
                                                 && p.sAmenity == eobj.sAmenity
                                                 && p.sCancellationPolicy == eobj.sCancellationPolicy
                                                 && p.sRoomTypeId == eobj.sRoomTypeId
                                                 && p.bIsSecretDeal == eobj.bIsSecretDeal
                                                 && p.cStatus == eobj.cStatus
                                             );
                        }


                        if (chkExists != true)
                        {
                            int iPropPromoID = 0;
                            if (eobj.iPromoId == Convert.ToInt32(Promotions.OFRFreebies))
                            {
                                eobj.iRPId = null;
                            }
                            else
                            {
                                var RP = db.tblPropertyRatePlanMaps.Where(a => a.iRPId == eobj.iRPId).ToList();

                                if (RP[0].cStatus == "I" || RP[0].bParentActive == false)
                                    eobj.bParentActive = false;
                                else
                                    eobj.bParentActive = true;
                            }

                            OneFineRate.tblPropertyPromotionMap dbuser = (OneFineRate.tblPropertyPromotionMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyPromotionMap());
                            db.tblPropertyPromotionMaps.Add(dbuser);
                            db.SaveChanges();
                            var vRoom = eobj.sRoomTypeId.Split(',').ToList();
                            iPropPromoID = dbuser.iID;
                            if (vRoom != null)
                            {
                                db.tblPropertyPromotionRoomTypeMaps.AddRange(vRoom.Select(x => new tblPropertyPromotionRoomTypeMap()
                                {
                                    iPropPromoID = iPropPromoID,
                                    iRoomId = Convert.ToInt16(x),//x.iRoomTypeId,
                                    dtActionDate = DateTime.Now,
                                    iActionBy = eobj.iActionBy
                                }).ToList());
                            }

                            if (jArray.Count > 0)
                            {
                                List<CancellationPolicyGridMain> objjArray = new List<CancellationPolicyGridMain>();

                                for (int i = 0; i < jArray.Count; i++)
                                {
                                    objjArray.Add(new CancellationPolicyGridMain()
                                    {
                                        CancellationValidFrom = Convert.ToDateTime(clsUtils.ConvertddmmyyyytoDateTime(jArray[i].CancellationValidFrom)),
                                        CancellationValidTo = Convert.ToDateTime(clsUtils.ConvertddmmyyyytoDateTime(jArray[i].CancellationValidTo)),
                                        sPolicyId = jArray[i].sPolicyId
                                    });
                                }

                                db.tblPropertyPromotionCancellationMainMaps.AddRange(objjArray.Select(x => new tblPropertyPromotionCancellationMainMap()
                                {
                                    iID = iPropPromoID,
                                    dtCancellationValidFrom = Convert.ToDateTime(x.CancellationValidFrom),
                                    dtCancellationValidTo = Convert.ToDateTime(x.CancellationValidTo),
                                    sCancellationPolicyId = x.sPolicyId,
                                    dtActionDate = DateTime.Now,
                                    iActionBy = eobj.iActionBy
                                }).ToList());

                                List<tblPropertyPromotionCancellationMap> LPRPC = new List<tblPropertyPromotionCancellationMap>();
                                for (int i = 0; i < objjArray.Count; i++)
                                {
                                    string[] IDs = objjArray[i].sPolicyId.Split(',');

                                    for (int j = 0; j < IDs.Length; j++)
                                    {
                                        DateTime CurrentDate = objjArray[i].CancellationValidFrom;
                                        while (CurrentDate <= objjArray[i].CancellationValidTo)
                                        {
                                            tblPropertyPromotionCancellationMap PRPC = new tblPropertyPromotionCancellationMap();
                                            PRPC.dtDate = CurrentDate;
                                            PRPC.iCancellationPolicyId = Convert.ToInt32(IDs[j]);
                                            PRPC.iID = iPropPromoID;
                                            PRPC.iActionBy = eobj.iActionBy;
                                            PRPC.dtActionDate = DateTime.Now;

                                            LPRPC.Add(PRPC);
                                            CurrentDate = CurrentDate.AddDays(1);
                                        }
                                    }
                                }

                                db.tblPropertyPromotionCancellationMaps.AddRange(LPRPC);
                            }
                            db.SaveChanges();
                            retval = 1;
                        }
                        else
                        { retval = 2; }
                        transactionScope.Complete();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        transactionScope.Dispose();
                    }
                }
            }
            return retval;
        }

        //Update a record
        public static int UpdateRecord(etblPropertyPromotionMap eobj, List<CancellationPolicyGrid> jArray)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    if (jArray.Count > 0)
                    {
                        for (int i = 0; i < jArray.Count; i++)
                        {
                            if (eobj.sCancellationPolicy == null)
                            {
                                eobj.sCancellationPolicy = jArray[i].sPolicyId;
                            }
                            else
                            {
                                eobj.sCancellationPolicy = eobj.sCancellationPolicy + "," + jArray[i].sPolicyId;
                            }

                        }
                    }



                    if (eobj.iPromoId != Convert.ToInt32(Promotions.OFRFreebies))
                    {
                        #region Check Cancellation Policy Dates
                        int CancellationPolicyDateCount = 0;
                        int SelectedValidityDateCount = 0;

                        DateTime SelectedValidityCurrentDate = eobj.dtStayDateFrom;
                        while (SelectedValidityCurrentDate <= eobj.dtStayDateTo)
                        {
                            SelectedValidityDateCount++;
                            SelectedValidityCurrentDate = SelectedValidityCurrentDate.AddDays(1);
                        }

                        for (int i = 0; i < jArray.Count; i++)
                        {
                            DateTime CancellationPolicyCurrentDate = Convert.ToDateTime(clsUtils.ConvertddmmyyyytoDateTime(jArray[i].CancellationValidFrom));
                            while (CancellationPolicyCurrentDate <= Convert.ToDateTime(clsUtils.ConvertddmmyyyytoDateTime(jArray[i].CancellationValidTo)))
                            {
                                CancellationPolicyDateCount++;
                                CancellationPolicyCurrentDate = CancellationPolicyCurrentDate.AddDays(1);
                            }
                        }

                        if (SelectedValidityDateCount != CancellationPolicyDateCount)
                        {
                            return retval = 6;
                        }

                        #endregion
                    }



                    int iPropPromoID = 0;
                    if (eobj.iPromoId == Convert.ToInt32(Promotions.OFRFreebies))
                    {
                        eobj.iRPId = null;
                    }

                    var vRoom = eobj.sRoomTypeId.Split(',').ToList();
                    iPropPromoID = eobj.iID;
                    if (vRoom != null)
                    {
                        IEnumerable<tblPropertyPromotionRoomTypeMap> list = db.tblPropertyPromotionRoomTypeMaps.Where(u => u.iPropPromoID == iPropPromoID).ToList();
                        db.tblPropertyPromotionRoomTypeMaps.RemoveRange(list);
                        //db.SaveChanges();

                        db.tblPropertyPromotionRoomTypeMaps.AddRange(vRoom.Select(x => new tblPropertyPromotionRoomTypeMap()
                        {
                            iPropPromoID = iPropPromoID,
                            iRoomId = Convert.ToInt16(x),//x.iRoomTypeId,
                            dtActionDate = DateTime.Now,
                            iActionBy = eobj.iActionBy
                        }).ToList());
                        //  db.SaveChanges();
                    }

                    var recChekCancellationData = (from m in db.tblPropertyPromotionCancellationMainMaps
                                                   select new
                                                   {
                                                       m.iID,
                                                       m.dtCancellationValidFrom,
                                                       m.dtCancellationValidTo,
                                                   }).Where(u => u.iID == iPropPromoID).AsQueryable().ToList();

                    if (jArray.Count > 0 || recChekCancellationData.Count > 0)
                    {
                        List<CancellationPolicyGridMain> objjArray = new List<CancellationPolicyGridMain>();

                        for (int i = 0; i < jArray.Count; i++)
                        {
                            objjArray.Add(new CancellationPolicyGridMain()
                            {
                                CancellationValidFrom = Convert.ToDateTime(clsUtils.ConvertddmmyyyytoDateTime(jArray[i].CancellationValidFrom)),
                                CancellationValidTo = Convert.ToDateTime(clsUtils.ConvertddmmyyyytoDateTime(jArray[i].CancellationValidTo)),
                                sPolicyId = jArray[i].sPolicyId
                            });
                        }

                        IEnumerable<tblPropertyPromotionCancellationMainMap> list = db.tblPropertyPromotionCancellationMainMaps.Where(u => u.iID == iPropPromoID).ToList();
                        db.tblPropertyPromotionCancellationMainMaps.RemoveRange(list);
                        //db.SaveChanges();

                        db.tblPropertyPromotionCancellationMainMaps.AddRange(objjArray.Select(x => new tblPropertyPromotionCancellationMainMap()
                        {
                            iID = iPropPromoID,
                            dtCancellationValidFrom = Convert.ToDateTime(x.CancellationValidFrom),
                            dtCancellationValidTo = Convert.ToDateTime(x.CancellationValidTo),
                            sCancellationPolicyId = x.sPolicyId,
                            dtActionDate = DateTime.Now,
                            iActionBy = eobj.iActionBy
                        }).ToList());

                        IEnumerable<tblPropertyPromotionCancellationMap> list1 = db.tblPropertyPromotionCancellationMaps.Where(u => u.iID == iPropPromoID).ToList();
                        db.tblPropertyPromotionCancellationMaps.RemoveRange(list1);
                        List<tblPropertyPromotionCancellationMap> LPRPC = new List<tblPropertyPromotionCancellationMap>();
                        for (int i = 0; i < objjArray.Count; i++)
                        {
                            string[] IDs = objjArray[i].sPolicyId.Split(',');

                            for (int j = 0; j < IDs.Length; j++)
                            {
                                DateTime CurrentDate = objjArray[i].CancellationValidFrom;
                                while (CurrentDate <= objjArray[i].CancellationValidTo)
                                {
                                    tblPropertyPromotionCancellationMap PRPC = new tblPropertyPromotionCancellationMap();
                                    PRPC.dtDate = CurrentDate;
                                    PRPC.iCancellationPolicyId = Convert.ToInt32(IDs[j]);
                                    PRPC.iID = iPropPromoID;
                                    PRPC.iActionBy = eobj.iActionBy;
                                    PRPC.dtActionDate = DateTime.Now;

                                    LPRPC.Add(PRPC);
                                    CurrentDate = CurrentDate.AddDays(1);
                                }
                            }
                        }

                        db.tblPropertyPromotionCancellationMaps.AddRange(LPRPC);
                    }

                    using (OneFineRateEntities db1 = new OneFineRateEntities())
                    {
                        var PP = db1.tblPropertyPromotionMaps.Where(a => a.iID == eobj.iID).ToList();
                        if (eobj.iPromoId != Convert.ToInt32(Promotions.OFRFreebies))
                        {
                            eobj.bParentActive = (bool)PP[0].bParentActive;
                        }
                        eobj.iPromoId = PP[0].iPromoId;
                    }

                    OneFineRate.tblPropertyPromotionMap obj = (OneFineRate.tblPropertyPromotionMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyPromotionMap());
                    db.tblPropertyPromotionMaps.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;

                    //db.SaveChanges();
                    //db.SaveChanges();
                    //db.uspSetActiveDeactiveByParent(eobj.iRPId);
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
        public static int DeleteRecord(etblPropertyPromotionMap eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    using (OneFineRateEntities db1 = new OneFineRateEntities())
                    {
                        var PP = db1.tblPropertyPromotionMaps.Where(a => a.iID == eobj.iID).ToList();
                        if (eobj.iPromoId != Convert.ToInt32(Promotions.OFRFreebies))
                        {
                            eobj.bParentActive = (bool)PP[0].bParentActive;
                        }
                        eobj.iPromoId = PP[0].iPromoId;
                    }

                    OneFineRate.tblPropertyPromotionMap obj = (OneFineRate.tblPropertyPromotionMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyPromotionMap());
                    db.tblPropertyPromotionMaps.Attach(obj);
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

        public static List<CheckBoxList> GetRoomTypeCheckBox(int? RPId, int PropId)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                if (RPId > 0 || RPId != null)
                {
                    selectItems = (from x in db.tblPropertyRoomMaps.ToList()
                                   join s in db.tblPropertyRatePlanRoomTypeMaps on x.iRoomId equals s.iRoomId
                                   select new CheckBoxList()
                                   {
                                       iRPid = s.iRPId,
                                       Text = x.sRoomName,
                                       Value = x.iRoomId,
                                       propid = x.iPropId,
                                       activeStatus = x.bActive
                                   }).Where(u => u.activeStatus == true && u.iRPid == RPId && u.propid == PropId).ToList();

                }
                else
                {
                    selectItems = db.tblPropertyRoomMaps.Select(x => new CheckBoxList()
                    {
                        Text = x.sRoomName,
                        Value = x.iRoomId,
                        propid = x.iPropId,
                        activeStatus = x.bActive,
                    }).Where(u => u.activeStatus == true && u.propid == PropId).ToList();
                }

                return selectItems;
            }
        }

        public static List<etblPropertyPromotionMap> GetRatePlanValidity(int? RPId, int PropId)
        {
            List<etblPropertyPromotionMap> selectItems = new List<etblPropertyPromotionMap>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                if (RPId > 0 || RPId != null)
                {
                    selectItems = (from x in db.tblPropertyRatePlanMaps
                                   select new etblPropertyPromotionMap()
                                   {
                                       iRPId = x.iRPId,
                                       iPropId = (int)x.iPropId,
                                       dtRPValidFrom = x.dtValidFrom.ToString(),
                                       dtRPValidTo = x.dtValidTo.ToString(),
                                       cStatus = x.cStatus
                                   }).Where(u => u.cStatus == "A" && u.iRPId == RPId && u.iPropId == PropId).AsEnumerable().ToList();

                }

                if (selectItems.Count > 0)
                {
                    var val1 = selectItems[0].dtRPValidFrom.Split('-').ToArray();
                    selectItems[0].dtRPValidFrom = val1[2] + "/" + val1[1] + "/" + val1[0];

                    var val2 = selectItems[0].dtRPValidTo.Split('-').ToArray();
                    selectItems[0].dtRPValidTo = val2[2] + "/" + val2[1] + "/" + val2[0];

                    return selectItems;
                }
                else { return null; }


            }
        }
    }
}