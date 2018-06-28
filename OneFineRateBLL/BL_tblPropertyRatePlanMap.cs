using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;
using System.Web.Mvc;
using System.Linq.Expressions;
using OneFineRateAppUtil;
using System.Globalization;
using System.Transactions;

namespace OneFineRateBLL
{
    public class BL_tblPropertyRatePlanMap
    {

        //get all rate plan name room wise
        public static List<PNames> GetRatePlanRoomWise(int roomid, int propid)
        {
            List<PNames> objMapping = new List<PNames>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {

                var objMapping1 = (from t1 in db.tblPropertyRatePlanMaps
                                   join t2 in db.tblPropertyRatePlanRoomTypeMaps on t1.iRPId equals t2.iRPId
                                   where t1.iPropId == propid && t2.iRoomId == roomid && t1.cStatus == "A"
                                   select new
                                   {
                                       Id = t1.iRPId.ToString(),
                                       Name = t1.sRatePlan.Trim(),
                                       date = t1.dtValidTo
                                   }).AsQueryable().ToList();

                objMapping.AddRange(objMapping1.Where(a => Convert.ToDateTime(a.date) > DateTime.Now).Select(a => new PNames()
                {
                    Name = a.Name,
                    Id = a.Id.ToString()
                }));

                return objMapping;
            }
        }

        //get all rate plan name room wise
        public static List<PropertyRooms> GetUpgradeRooms(int roomid, int RPID, int propid)
        {
            List<PropertyRooms> objMapping = new List<PropertyRooms>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var MappedPlans = db.uspGetRoomUpgrade(roomid, RPID, propid);
                var MappedPlansList = MappedPlans.ToList();

                objMapping.AddRange(MappedPlansList.Select(a => new PropertyRooms()
                {
                    Name = a.sRoomName,
                    Roomid = a.iRoomId,
                    Currency = a.sCurrencyCode
                }));

                return objMapping;
            }
        }
        public static List<PNames> GetAllRatePlans(int propid)
        {
            List<PNames> data = new List<PNames>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    data = (from t1 in db.tblPropertyRatePlanRoomTypeMaps
                            join t2 in db.tblPropertyRoomMaps on t1.iRoomId equals t2.iRoomId
                            join t3 in db.tblPropertyRatePlanMaps on t1.iRPId equals t3.iRPId
                            where t2.iPropId == propid && t3.iPropId == propid
                            select new PNames
                            {
                                Name = t2.sRoomName + " - " + t3.sRatePlan,
                                Id = t1.iRoomId + "|" + t1.iRPId

                            }).ToList();
                }
                catch
                {
                    throw;
                }
            }
            return data;
        }

        public static List<RPNames> GetRatePlansForDD(int iRPId, int PropId)
        {
            List<RPNames> data = new List<RPNames>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                //try
                //{
                //    data = (from t3 in db.tblPropertyRatePlanMaps
                //            select new RPNames
                //            {
                //                PropId = t3.iPropId,
                //                Name = t3.sRatePlan,
                //                Id = t3.iRPId
                //            }).Where(u => u.PropId == PropId && u.Id != iRPId).Distinct().ToList();
                //}
                //catch
                //{
                //    throw;
                //}

                var AllRatePlans = db.uspGetRatePlanForMapping(PropId, iRPId);
                var AllRatePlansList = AllRatePlans.ToList();

                data.AddRange(AllRatePlansList.Select(menu => new RPNames
                {
                    Id = menu.iRPId,
                    Name = menu.sRatePlan,
                    PropId = menu.iPropId
                }));

                //foreach (var RatePlan in AllRatePlansList)
                //{
                //    RPNames RPN = new RPNames();
                //    RPN.Id = RatePlan.iRPId;
                //    RPN.Name = RatePlan.sRatePlan;
                //    RPN.PropId = RatePlan.iPropId;
                //    data.Add(RPN);
                //}
            }
            return data;
        }

        public static List<RPNames> GetRatePlansForTypehead(string term)
        {
            List<RPNames> data = new List<RPNames>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    data = (from t in db.tblRatePlanMs.Where(t => t.cStatus == "A")
                            select new RPNames
                            {
                                Name = t.sRatePlan,
                                Id = t.iRatePlanId
                            }).Distinct().ToList();
                }
                catch
                {
                    throw;
                }
            }
            return data;
        }

        public static etblPropertyRatePlanMap GetAllRatePlansByID(int a)
        {
            etblPropertyRatePlanMap promorec = new etblPropertyRatePlanMap();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var rec = (from m in db.tblPropertyRatePlanMaps
                           join c in db.tblUserMs on m.iActionBy equals c.iUserId
                           select new
                           {
                               m.iRPId,
                               m.iPropId,
                               m.sRatePlan,
                               m.cRatePlanType,
                               m.dtValidFrom,
                               m.dtValidTo,
                               m.bLinkToExistingRatePlan,
                               m.iLinkRatePlanId,
                               m.bIsPlus,
                               m.bIsPercent,
                               m.dValue,
                               m.sAmenityId,
                               m.sAmenity,
                               m.iMinLengthStay,
                               m.iMaxLengthStay,
                               m.dHrsDays,
                               m.cHrsDays,
                               m.bIsBefore,
                               m.sCancellationPolicy,
                               m.bIsSecretDeal,
                               m.cStatus,
                               m.dtActionDate,
                               m.iActionBy,
                               m.dtCancellationValidFrom,
                               m.dtCancellationValidTo,
                               UserName = c.sFirstName + " " + c.sLastName
                           }
                                   ).Where(u => u.iRPId == a).AsQueryable();
                var data = rec.ToList();
                etblPropertyRatePlanMapMain objMain = new etblPropertyRatePlanMapMain();
                objMain = (etblPropertyRatePlanMapMain)OneFineRateAppUtil.clsUtils.ConvertToObject(data[0], objMain);

                promorec.iRPId = objMain.iRPId;
                promorec.iPropId = objMain.iPropId;
                promorec.sRatePlan = objMain.sRatePlan;
                promorec.cRatePlanType = objMain.cRatePlanType;
                promorec.dtValidFrom = objMain.dtValidFrom.ToString("dd/MM/yyyy");
                promorec.dtValidTo = objMain.dtValidTo.ToString("dd/MM/yyyy");
                promorec.bLinkToExistingRatePlan = objMain.bLinkToExistingRatePlan;
                promorec.iLinkRatePlanId = objMain.iLinkRatePlanId;
                promorec.bIsPlus = objMain.bIsPlus;
                promorec.bIsPercent = objMain.bIsPercent;
                promorec.dValue = objMain.dValue;
                promorec.sAmenityId = objMain.sAmenityId;
                promorec.sAmenity = objMain.sAmenity;
                promorec.iMinLengthStay = objMain.iMinLengthStay;
                promorec.iMaxLengthStay = objMain.iMaxLengthStay;
                promorec.dHrsDays = objMain.dHrsDays;
                promorec.cHrsDays = objMain.cHrsDays;
                promorec.bIsBefore = objMain.bIsBefore;
                promorec.iIsBefore = objMain.bIsBefore ? 1 : 0;
                promorec.sCancellationPolicy = objMain.sCancellationPolicy;
                promorec.bIsSecretDeal = objMain.bIsSecretDeal;
                promorec.cStatus = objMain.cStatus;
                promorec.dtActionDate = objMain.dtActionDate;
                promorec.iActionBy = objMain.iActionBy;
                //promorec.dtCancellationValidFrom = DateTime.Now.ToString("dd/MM/yyyy");
                //promorec.dtCancellationValidTo = DateTime.Now.ToString("dd/MM/yyyy");
                promorec.dtCancellationValidFrom = null;
                promorec.dtCancellationValidTo = null;
                promorec.UserName = objMain.UserName;



                var CancellationData = (from m in db.tblPropertyRatePlanCancellationMainMaps
                                        select new
                                        {
                                            m.iRPId,
                                            m.dtCancellationValidFrom,
                                            m.dtCancellationValidTo,
                                            m.sCancellationPolicyId,
                                        }).Where(u => u.iRPId == a).AsQueryable();

                var data1 = CancellationData.ToList();
                List<CancellationPolicyGrid> objPolicy = new List<CancellationPolicyGrid>();
                for (int i = 0; i < data1.Count; i++)
                {
                    objPolicy.Add(new CancellationPolicyGrid()
                    {
                        CancellationPolicyId = data1[i].sCancellationPolicyId.Split(',').ToList(),
                        CancellationValidFrom = data1[i].dtCancellationValidFrom.ToString("dd/MM/yyyy"),
                        CancellationValidTo = data1[i].dtCancellationValidTo.ToString("dd/MM/yyyy"),
                        sPolicyId = data1[i].sCancellationPolicyId
                    });
                    var sName = objPolicy[i].sPolicyId.Split(',').ToList();
                    foreach (var item in sName)
                    {
                        int CanId = Convert.ToInt16(item);
                        var getName = (from m in db.tblPropertyCancellationPolicyMaps select new { m.iCancellationPolicyId, m.sPolicyName, m.cStatus }).Where(u => u.iCancellationPolicyId == CanId).AsQueryable().ToList();
                        if (getName.Count != 0)
                            if (objPolicy[i].sPolicyName == null)
                            {
                                objPolicy[i].sPolicyName = getName[0].sPolicyName + (getName[0].cStatus == "A" ? "" : "(Disabled)");
                            }
                            else
                            {
                                objPolicy[i].sPolicyName = objPolicy[i].sPolicyName + "," + getName[0].sPolicyName + (getName[0].cStatus == "A" ? "" : "(Disabled)");
                            }

                    }
                    if (objPolicy[i].sPolicyName != null)
                        objPolicy[i].CancellationPolicyName = objPolicy[i].sPolicyName.Split(',').ToList();

                }
                promorec.CancellationPolicyGrid = objPolicy;


                //promorec = (etblPropertyRatePlanMap)OneFineRateAppUtil.clsUtils.ConvertToObject(data[0], promorec);
                return promorec;
            }
        }

        //Add new rate plan
        public static int AddRecord(etblPropertyRatePlanMap eobj, List<CancellationPolicyGrid> jArray)
        {
            int retval = 0;
            using (var transationscope = new TransactionScope())
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
                        List<string> sList = eobj.sCancellationPolicy.Split(',').ToList();
                        List<string> sList1 = sList.Distinct().ToList();
                        eobj.sCancellationPolicy = string.Join(",", sList1);

                        etblPropertyRatePlanMapMain eobjMain = new etblPropertyRatePlanMapMain();
                        eobjMain.iRPId = eobj.iRPId;
                        eobjMain.iPropId = eobj.iPropId;
                        eobjMain.sRatePlan = eobj.sRatePlan;
                        eobjMain.cRatePlanType = eobj.cRatePlanType;
                        eobjMain.dtValidFrom = Convert.ToDateTime(clsUtils.ConvertddmmyyyytoDateTime(eobj.dtValidFrom));
                        eobjMain.dtValidTo = Convert.ToDateTime(clsUtils.ConvertddmmyyyytoDateTime(eobj.dtValidTo));
                        eobjMain.bLinkToExistingRatePlan = eobj.bLinkToExistingRatePlan;
                        eobjMain.iLinkRatePlanId = eobj.iLinkRatePlanId;
                        eobjMain.bIsPlus = eobj.bIsPlus;
                        eobjMain.bIsPercent = eobj.bIsPercent;
                        eobjMain.dValue = eobj.dValue;
                        eobjMain.sAmenityId = eobj.sAmenityId;
                        eobjMain.sAmenity = eobj.sAmenity;
                        eobjMain.iMinLengthStay = eobj.iMinLengthStay;
                        eobjMain.iMaxLengthStay = eobj.iMaxLengthStay;
                        eobjMain.dHrsDays = eobj.dHrsDays;
                        eobjMain.cHrsDays = eobj.cHrsDays;
                        eobjMain.bIsBefore = eobj.bIsBefore;
                        eobjMain.sCancellationPolicy = eobj.sCancellationPolicy;
                        eobjMain.dtCancellationValidFrom = Convert.ToDateTime(null);
                        eobjMain.dtCancellationValidTo = Convert.ToDateTime(null);
                        eobjMain.bIsSecretDeal = eobj.bIsSecretDeal;
                        eobjMain.cStatus = eobj.cStatus;
                        eobjMain.dtActionDate = eobj.dtActionDate;
                        eobjMain.iActionBy = eobj.iActionBy;
                        eobjMain.sRoomId = eobj.sRoomTypeId;


                        #region Check Cancellation Policy Dates
                        int CancellationPolicyDateCount = 0;
                        int SelectedValidityDateCount = 0;

                        DateTime SelectedValidityCurrentDate = eobjMain.dtValidFrom;
                        while (SelectedValidityCurrentDate <= eobjMain.dtValidTo)
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

                        var checkExistsRatePlanInValidity = db.tblPropertyRatePlanMaps.Any(
                                               p => p.iPropId == eobjMain.iPropId
                                                    && p.sRatePlan == eobjMain.sRatePlan
                                                    && (((p.dtValidFrom <= eobjMain.dtValidFrom && p.dtValidTo >= eobjMain.dtValidFrom) || (p.dtValidFrom <= eobjMain.dtValidTo && p.dtValidTo >= eobjMain.dtValidTo))
                                                        || ((eobjMain.dtValidFrom <= p.dtValidFrom && eobjMain.dtValidTo >= p.dtValidFrom) || (eobjMain.dtValidFrom <= p.dtValidTo && eobjMain.dtValidTo >= p.dtValidTo)))
                                                   );

                        if (checkExistsRatePlanInValidity == true)
                        {
                            return retval = 4;
                        }

                        bool IsValidate = true;
                        if (eobj.bLinkToExistingRatePlan == true)
                        {
                            var rec = (from m in db.tblPropertyRatePlanMaps
                                       select new
                                       {
                                           m.iRPId,
                                           m.dtValidFrom,
                                           m.dtValidTo,
                                       }).Where(u => u.iRPId == eobj.iLinkRatePlanId).AsQueryable().ToList();

                            if (rec[0].dtValidFrom <= clsUtils.ConvertddmmyyyytoDateTime(eobj.dtValidFrom) && rec[0].dtValidTo >= clsUtils.ConvertddmmyyyytoDateTime(eobj.dtValidTo))
                            {
                                IsValidate = true;
                            }
                            else
                            { IsValidate = false; retval = 3; }
                        }


                        bool chkExists = db.tblPropertyRatePlanMaps.Any(
                                                p => p.sRatePlan == eobjMain.sRatePlan
                                                    && p.dtValidFrom == eobjMain.dtValidFrom
                                                    && p.dtValidTo == eobjMain.dtValidTo
                                                    && p.cStatus == eobj.cStatus 
                                                    && p.iPropId == eobj.iPropId);

                        if (chkExists != true)
                        {
                            if (IsValidate == true || eobj.bLinkToExistingRatePlan == false)
                            {
                                int iPropPromoID = 0;

                                if (eobj.iLinkRatePlanId > 0)
                                    using (OneFineRateEntities db1 = new OneFineRateEntities())
                                    {
                                        var RP = db1.tblPropertyRatePlanMaps.Where(a => a.iRPId == eobj.iLinkRatePlanId).ToList();

                                        if (RP[0].cStatus == "I" || RP[0].bParentActive == false)
                                            eobjMain.bParentActive = false;
                                        else
                                            eobjMain.bParentActive = true;
                                    }
                                else
                                    eobjMain.bParentActive = true;
                                OneFineRate.tblPropertyRatePlanMap dbuser = (OneFineRate.tblPropertyRatePlanMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobjMain, new OneFineRate.tblPropertyRatePlanMap());
                                db.tblPropertyRatePlanMaps.Add(dbuser);
                                db.SaveChanges();
                                var vRoom = eobj.sRoomTypeId.Split(',').ToList();
                                iPropPromoID = dbuser.iRPId;
                                if (vRoom != null)
                                {
                                    db.tblPropertyRatePlanRoomTypeMaps.AddRange(vRoom.Select(x => new tblPropertyRatePlanRoomTypeMap()
                                    {
                                        iRPId = iPropPromoID,
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

                                    db.tblPropertyRatePlanCancellationMainMaps.AddRange(objjArray.Select(x => new tblPropertyRatePlanCancellationMainMap()
                                    {
                                        iPropId = (int)eobj.iPropId,
                                        iRPId = iPropPromoID,
                                        dtCancellationValidFrom = Convert.ToDateTime(x.CancellationValidFrom),
                                        dtCancellationValidTo = Convert.ToDateTime(x.CancellationValidTo),
                                        sCancellationPolicyId = x.sPolicyId,
                                        dtActionDate = DateTime.Now,
                                        iActionBy = eobj.iActionBy
                                    }).ToList());

                                    List<tblPropertyRatePlanCancellationMap> LPRPC = new List<tblPropertyRatePlanCancellationMap>();
                                    for (int i = 0; i < objjArray.Count; i++)
                                    {
                                        string[] IDs = objjArray[i].sPolicyId.Split(',');

                                        for (int j = 0; j < IDs.Length; j++)
                                        {
                                            DateTime CurrentDate = objjArray[i].CancellationValidFrom;
                                            while (CurrentDate <= objjArray[i].CancellationValidTo)
                                            {
                                                tblPropertyRatePlanCancellationMap PRPC = new tblPropertyRatePlanCancellationMap();
                                                PRPC.dtDate = CurrentDate;
                                                PRPC.iCancellationPolicyId = Convert.ToInt32(IDs[j]);
                                                PRPC.iPropId = (int)eobj.iPropId;
                                                PRPC.iRPId = iPropPromoID;
                                                PRPC.iActionBy = eobj.iActionBy;
                                                PRPC.dtActionDate = DateTime.Now;

                                                LPRPC.Add(PRPC);
                                                CurrentDate = CurrentDate.AddDays(1);
                                            }
                                        }
                                    }
                                    db.tblPropertyRatePlanCancellationMaps.AddRange(LPRPC);
                                }
                                db.SaveChanges();
                                retval = 1;
                            }
                            else
                            { retval = 3; }

                        }
                        else
                        { retval = 4; }
                        transationscope.Complete();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        transationscope.Dispose();
                    }
                }
            }
            return retval;
        }

        //Update new rate plan
        public static int UpdateRecord(etblPropertyRatePlanMap eobj, List<CancellationPolicyGrid> jArray)
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
                    List<string> OldRooms = new List<string>();
                    List<string> sList = eobj.sCancellationPolicy.Split(',').ToList();
                    List<string> sList1 = sList.Distinct().ToList();
                    eobj.sCancellationPolicy = string.Join(",", sList1);

                    etblPropertyRatePlanMapMain eobjMain = new etblPropertyRatePlanMapMain();
                    eobjMain.iRPId = eobj.iRPId;
                    eobjMain.iPropId = eobj.iPropId;
                    eobjMain.sRatePlan = eobj.sRatePlan;
                    eobjMain.cRatePlanType = eobj.cRatePlanType;
                    eobjMain.dtValidFrom = Convert.ToDateTime(clsUtils.ConvertddmmyyyytoDateTime(eobj.dtValidFrom));
                    eobjMain.dtValidTo = Convert.ToDateTime(clsUtils.ConvertddmmyyyytoDateTime(eobj.dtValidTo));
                    eobjMain.bLinkToExistingRatePlan = eobj.bLinkToExistingRatePlan;
                    eobjMain.iLinkRatePlanId = eobj.iLinkRatePlanId;
                    eobjMain.bIsPlus = eobj.bIsPlus;
                    eobjMain.bIsPercent = eobj.bIsPercent;
                    eobjMain.dValue = eobj.dValue;
                    eobjMain.sAmenityId = eobj.sAmenityId;
                    eobjMain.sAmenity = eobj.sAmenity;
                    eobjMain.iMinLengthStay = eobj.iMinLengthStay;
                    eobjMain.iMaxLengthStay = eobj.iMaxLengthStay;
                    eobjMain.dHrsDays = eobj.dHrsDays;
                    eobjMain.cHrsDays = eobj.cHrsDays;
                    eobjMain.bIsBefore = eobj.bIsBefore;
                    eobjMain.sCancellationPolicy = eobj.sCancellationPolicy;
                    eobjMain.dtCancellationValidFrom = Convert.ToDateTime(null);
                    eobjMain.dtCancellationValidTo = Convert.ToDateTime(null);
                    eobjMain.bIsSecretDeal = eobj.bIsSecretDeal;
                    eobjMain.cStatus = eobj.cStatus;
                    eobjMain.dtActionDate = eobj.dtActionDate;
                    eobjMain.iActionBy = eobj.iActionBy;
                    eobjMain.sRoomId = eobj.sRoomTypeId;

                    #region Check Cancellation Policy Dates
                    int CancellationPolicyDateCount = 0;
                    int SelectedValidityDateCount = 0;

                    DateTime SelectedValidityCurrentDate = eobjMain.dtValidFrom;
                    while (SelectedValidityCurrentDate <= eobjMain.dtValidTo)
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



                    var checkExistsRatePlanInValidity = db.tblPropertyRatePlanMaps.Any(
                                          p => p.iPropId == eobjMain.iPropId
                                               && p.sRatePlan == eobjMain.sRatePlan
                                               && p.iRPId != eobjMain.iRPId
                                               && (((p.dtValidFrom <= eobjMain.dtValidFrom && p.dtValidTo >= eobjMain.dtValidFrom) || (p.dtValidFrom <= eobjMain.dtValidTo && p.dtValidTo >= eobjMain.dtValidTo))
                                                   || ((eobjMain.dtValidFrom <= p.dtValidFrom && eobjMain.dtValidTo >= p.dtValidFrom) || (eobjMain.dtValidFrom <= p.dtValidTo && eobjMain.dtValidTo >= p.dtValidTo)))
                                              );

                    if (checkExistsRatePlanInValidity == true)
                    {
                        return retval = 4;
                    }


                    bool IsValidate = true;
                    #region Date Validation

                    var cValidDates = (from t in db.tblPropertyRatePlanMaps
                                       select new ForValidation()
                                       {
                                           RPId = t.iRPId,
                                           propid = t.iPropId,
                                           ValidFromOld = t.dtValidFrom,
                                           ValidToOld = t.dtValidTo
                                       }).Where(u => u.RPId == eobj.iRPId && u.propid == eobj.iPropId).AsQueryable().ToList();

                    if (cValidDates[0].ValidFromOld > clsUtils.ConvertddmmyyyytoDateTime(eobj.dtValidFrom) || cValidDates[0].ValidToOld > clsUtils.ConvertddmmyyyytoDateTime(eobj.dtValidTo))
                    {
                        IsValidate = false; retval = 5; return retval;
                    }

                    #endregion


                    #region ParentChildValidation

                    //var cValid = (from p in db.tblPropertyRatePlanRoomTypeMaps
                    //              join t in db.tblPropertyRatePlanMaps on p.iRPId equals t.iRPId
                    //              join s in db.tblPropertyRoomRatePlanInventoryMaps on t.iPropId equals s.iPropId
                    //              join s1 in db.tblPropertyRoomRatePlanInventoryMaps on p.iRoomId equals s1.iRoomId
                    //              select new ForValidation()
                    //              {
                    //                  roomId = p.iRoomId,
                    //                  RPId = t.iLinkRatePlanId,
                    //                  propid = t.iPropId
                    //              }).Where(u => u.RPId == eobj.iRPId && u.propid == eobj.iPropId).AsQueryable().ToList();




                    #endregion



                    if (eobjMain.bLinkToExistingRatePlan == true)
                    {
                        var rec = (from m in db.tblPropertyRatePlanMaps
                                   select new
                                   {
                                       m.iRPId,
                                       m.dtValidFrom,
                                       m.dtValidTo,
                                   }).Where(u => u.iRPId == eobjMain.iLinkRatePlanId).AsQueryable().ToList();
                        if (rec.Count > 0)
                        {
                            if (rec[0].dtValidFrom <= clsUtils.ConvertddmmyyyytoDateTime(eobj.dtValidFrom) && rec[0].dtValidTo >= clsUtils.ConvertddmmyyyytoDateTime(eobj.dtValidTo))
                            {
                                IsValidate = true;
                            }
                            else
                            { IsValidate = false; retval = 3; }
                        }


                        else
                        { IsValidate = false; retval = 3; }
                    }

                    if (IsValidate == true || eobj.bLinkToExistingRatePlan == false)
                    {

                        var vRoom = eobj.sRoomTypeId.Split(',').ToList();
                        if (vRoom != null)
                        {
                            IEnumerable<tblPropertyRatePlanRoomTypeMap> list = db.tblPropertyRatePlanRoomTypeMaps.Where(u => u.iRPId == eobj.iRPId).ToList();
                            db.tblPropertyRatePlanRoomTypeMaps.RemoveRange(list);

                            db.tblPropertyRatePlanRoomTypeMaps.AddRange(vRoom.Select(x => new tblPropertyRatePlanRoomTypeMap()
                            {
                                iRPId = eobj.iRPId,
                                iRoomId = Convert.ToInt16(x),//x.iRoomTypeId,
                                dtActionDate = DateTime.Now,
                                iActionBy = eobj.iActionBy
                            }).ToList());

                            foreach (var item in list)
                            {
                                if (!vRoom.Contains(item.iRoomId.ToString()))
                                    OldRooms.Add(item.iRoomId.ToString());
                            }
                        }

                        var recChekCancellationData = (from m in db.tblPropertyRatePlanCancellationMainMaps
                                                       select new
                                                       {
                                                           m.iRPId,
                                                           m.dtCancellationValidFrom,
                                                           m.dtCancellationValidTo,
                                                       }).Where(u => u.iRPId == eobj.iRPId).AsQueryable().ToList();

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

                            IEnumerable<tblPropertyRatePlanCancellationMainMap> list = db.tblPropertyRatePlanCancellationMainMaps.Where(u => u.iRPId == eobj.iRPId).ToList();
                            db.tblPropertyRatePlanCancellationMainMaps.RemoveRange(list);
                            //db.SaveChanges();

                            db.tblPropertyRatePlanCancellationMainMaps.AddRange(objjArray.Select(x => new tblPropertyRatePlanCancellationMainMap()
                            {
                                iPropId = (int)eobj.iPropId,
                                iRPId = eobj.iRPId,
                                dtCancellationValidFrom = Convert.ToDateTime(x.CancellationValidFrom),
                                dtCancellationValidTo = Convert.ToDateTime(x.CancellationValidTo),
                                sCancellationPolicyId = x.sPolicyId,
                                dtActionDate = DateTime.Now,
                                iActionBy = eobj.iActionBy
                            }).ToList());

                            IEnumerable<tblPropertyRatePlanCancellationMap> list1 = db.tblPropertyRatePlanCancellationMaps.Where(u => u.iRPId == eobj.iRPId).ToList();
                            db.tblPropertyRatePlanCancellationMaps.RemoveRange(list1);
                            List<tblPropertyRatePlanCancellationMap> LPRPC = new List<tblPropertyRatePlanCancellationMap>();
                            for (int i = 0; i < objjArray.Count; i++)
                            {
                                string[] IDs = objjArray[i].sPolicyId.Split(',');

                                for (int j = 0; j < IDs.Length; j++)
                                {
                                    DateTime CurrentDate = objjArray[i].CancellationValidFrom;
                                    while (CurrentDate <= objjArray[i].CancellationValidTo)
                                    {
                                        tblPropertyRatePlanCancellationMap PRPC = new tblPropertyRatePlanCancellationMap();
                                        PRPC.dtDate = CurrentDate;
                                        PRPC.iCancellationPolicyId = Convert.ToInt32(IDs[j]);
                                        PRPC.iPropId = (int)eobj.iPropId;
                                        PRPC.iRPId = eobj.iRPId;
                                        PRPC.iActionBy = eobj.iActionBy;
                                        PRPC.dtActionDate = DateTime.Now;

                                        LPRPC.Add(PRPC);
                                        CurrentDate = CurrentDate.AddDays(1);
                                    }
                                }
                            }

                            db.tblPropertyRatePlanCancellationMaps.AddRange(LPRPC);
                        }
                        if (eobj.iLinkRatePlanId > 0)
                            using (OneFineRateEntities db1 = new OneFineRateEntities())
                            {
                                var RP = db1.tblPropertyRatePlanMaps.Where(a => a.iRPId == eobj.iLinkRatePlanId).ToList();

                                if (RP[0].cStatus == "I" || RP[0].bParentActive == false)
                                    eobjMain.bParentActive = false;
                                else
                                    eobjMain.bParentActive = true;
                            }
                        else
                            eobjMain.bParentActive = true;
                        OneFineRate.tblPropertyRatePlanMap obj = (OneFineRate.tblPropertyRatePlanMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobjMain, new OneFineRate.tblPropertyRatePlanMap());
                        db.tblPropertyRatePlanMaps.Attach(obj);
                        db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        if (OldRooms.Count > 0)
                            db.uspSetActiveDeactiveByParent(eobj.iRPId);

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

        //Delete a record
        public static int DeleteRecord(etblPropertyRatePlanMap eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    etblPropertyRatePlanMapMain eobjMain = new etblPropertyRatePlanMapMain();
                    eobjMain.iRPId = eobj.iRPId;
                    eobjMain.iPropId = eobj.iPropId;
                    eobjMain.sRatePlan = eobj.sRatePlan;
                    eobjMain.cRatePlanType = eobj.cRatePlanType;
                    eobjMain.dtValidFrom = Convert.ToDateTime(clsUtils.ConvertddmmyyyytoDateTime(eobj.dtValidFrom));
                    eobjMain.dtValidTo = Convert.ToDateTime(clsUtils.ConvertddmmyyyytoDateTime(eobj.dtValidTo));
                    eobjMain.bLinkToExistingRatePlan = eobj.bLinkToExistingRatePlan;
                    eobjMain.iLinkRatePlanId = eobj.iLinkRatePlanId;
                    eobjMain.bIsPlus = eobj.bIsPlus;
                    eobjMain.bIsPercent = eobj.bIsPercent;
                    eobjMain.dValue = eobj.dValue;
                    eobjMain.sAmenityId = eobj.sAmenityId;
                    eobjMain.sAmenity = eobj.sAmenity;
                    eobjMain.iMinLengthStay = eobj.iMinLengthStay;
                    eobjMain.iMaxLengthStay = eobj.iMaxLengthStay;
                    eobjMain.dHrsDays = eobj.dHrsDays;
                    eobjMain.cHrsDays = eobj.cHrsDays;
                    eobjMain.bIsBefore = eobj.bIsBefore;
                    eobjMain.sCancellationPolicy = eobj.sCancellationPolicy;
                    eobjMain.dtCancellationValidFrom = Convert.ToDateTime(null);
                    eobjMain.dtCancellationValidTo = Convert.ToDateTime(null);
                    eobjMain.bIsSecretDeal = eobj.bIsSecretDeal;
                    eobjMain.cStatus = eobj.cStatus;
                    eobjMain.dtActionDate = eobj.dtActionDate;
                    eobjMain.iActionBy = eobj.iActionBy;

                    if (eobj.iLinkRatePlanId > 0)
                        using (OneFineRateEntities db1 = new OneFineRateEntities())
                        {
                            var RP = db1.tblPropertyRatePlanMaps.Where(a => a.iRPId == eobj.iLinkRatePlanId).ToList();

                            if (RP[0].cStatus == "I" || RP[0].bParentActive == false)
                                eobjMain.bParentActive = false;
                            else
                                eobjMain.bParentActive = true;
                        }
                    else
                        eobjMain.bParentActive = true;

                    OneFineRate.tblPropertyRatePlanMap obj = (OneFineRate.tblPropertyRatePlanMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobjMain, new OneFineRate.tblPropertyRatePlanMap());
                    db.tblPropertyRatePlanMaps.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    db.uspSetActiveDeactiveByParent(eobj.iRPId);
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


        public static List<CheckBoxList> GetRoomTypeCheckBox(int PropId, int val)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                selectItems = db.tblPropertyRoomMaps.Select(x => new CheckBoxList()
                {
                    Text = x.sRoomName,
                    Value = x.iRoomId,
                    propid = x.iPropId,
                    activeStatus = x.bActive
                }).Where(u => u.activeStatus == true && u.propid == PropId).OrderBy(U => U.Text).ToList();
                return selectItems;
            }
        }

        public static List<CheckBoxList> GetRoomTypeCheckBox(int? RPId, int PropId, int? iLinkRatePlanId, bool CanShowCheckedRooms)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                if (RPId > 0 && RPId != null)
                {
                    #region ParentChildValidation

                    //var cValid = (from p in db.tblPropertyRatePlanRoomTypeMaps
                    //              join t in db.tblPropertyRatePlanMaps on p.iRPId equals t.iRPId
                    //              join s in db.tblPropertyRoomRatePlanInventoryMaps on t.iPropId equals s.iPropId
                    //              join s1 in db.tblPropertyRoomRatePlanInventoryMaps on p.iRoomId equals s1.iRoomId
                    //              select new ForValidation()
                    //              {
                    //                  roomId = p.iRoomId,
                    //                  RPId = t.iLinkRatePlanId,
                    //                  propid = t.iPropId
                    //              }).Distinct().Where(u => u.RPId == RPId && u.propid == PropId).AsQueryable().ToList();




                    //List<long> list1 = new List<long>();
                    //foreach (var item in cValid.ToList())
                    //{
                    //    list1.Add(Convert.ToInt32(item.roomId));
                    //}

                    #endregion

                    if (iLinkRatePlanId == null)
                    {
                        # region Parent
                        var selectItems1 = (from x in db.tblPropertyRoomMaps
                                            join s in db.tblPropertyRatePlanRoomTypeMaps on x.iRoomId equals s.iRoomId
                                            select new CheckBoxList()
                                            {
                                                id = s.iRPId,
                                                Text = x.sRoomName,
                                                Value = x.iRoomId,
                                                Selected = (s != null && CanShowCheckedRooms) ? true : false

                                            }).Where(u => u.id == RPId).OrderBy(u => u.Text);

                        selectItems = selectItems1.AsQueryable().ToList();
                        List<int> list = new List<int>();
                        foreach (var item in selectItems)
                        {
                            list.Add(Convert.ToInt32(item.Value));
                        }
                        string data = string.Join(",", list.Select(n => n.ToString()).ToArray());
                        string[] objSelectItems;
                        if (data == null || data == "")
                            data = "0";

                        objSelectItems = data.Split(',');
                        selectItems = (from x in db.tblPropertyRoomMaps.ToList()
                                       join si in objSelectItems.ToList() on x.iRoomId equals Convert.ToInt32(si) into result
                                       from r in result.DefaultIfEmpty()
                                       select new CheckBoxList()
                                       {
                                           Text = x.sRoomName,
                                           Value = x.iRoomId,
                                           propid = x.iPropId,
                                           activeStatus = x.bActive,
                                           Selected = (r != null && CanShowCheckedRooms) ? true : false
                                       }).Where(u => u.activeStatus == true && u.propid == PropId).OrderBy(u => u.Text).ToList();

                        #endregion
                    }
                    else
                    {
                        #region Child
                        selectItems = (from x in db.tblPropertyRoomMaps
                                       join s in db.tblPropertyRatePlanRoomTypeMaps on x.iRoomId equals s.iRoomId
                                       select new CheckBoxList()
                                       {
                                           id = s.iRPId,
                                           Text = x.sRoomName,
                                           Value = x.iRoomId,
                                           Selected = (s != null && CanShowCheckedRooms) ? true : false

                                           //}).Where(u => u.id == iLinkRatePlanId).AsQueryable().ToList();
                                       }).Where(u => u.id == RPId).AsQueryable().OrderBy(u => u.Text).ToList();

                        List<int> list = new List<int>();
                        foreach (var item in selectItems)
                        {
                            list.Add(Convert.ToInt32(item.Value));
                        }
                        string data = string.Join(",", list.Select(n => n.ToString()).ToArray());
                        string[] objSelectItems;

                        if (data != null && data != "")
                        {
                            objSelectItems = data.Split(',');
                            //selectItems = (from x in db.tblPropertyRoomMaps.ToList()
                            //               //join s1 in list on x.iRoomId equals s1
                            //               join si in objSelectItems.ToList() on x.iRoomId equals Convert.ToInt32(si)
                            //               select new CheckBoxList()
                            //               {
                            //                   Text = x.sRoomName,
                            //                   Value = x.iRoomId,
                            //                   propid = x.iPropId,
                            //                   activeStatus = x.bActive
                            //                   ,Selected = si == null ? false : true
                            //               }).Where(u => u.activeStatus == true && u.propid == PropId).ToList();

                            selectItems = (from x in db.tblPropertyRoomMaps.ToList()
                                           join s in db.tblPropertyRatePlanRoomTypeMaps on x.iRoomId equals s.iRoomId
                                           join si in objSelectItems.ToList() on x.iRoomId equals Convert.ToInt32(si) into result
                                           from r in result.DefaultIfEmpty()
                                           select new CheckBoxList()
                                           {
                                               Text = x.sRoomName,
                                               Value = x.iRoomId,
                                               propid = x.iPropId,
                                               activeStatus = x.bActive,
                                               id = s.iRPId,
                                               Selected = (r != null && CanShowCheckedRooms) ? true : false
                                           }).Where(u => u.activeStatus == true && u.propid == PropId && u.id == iLinkRatePlanId).OrderBy(u => u.Text).ToList();

                        }
                        #endregion
                    }






                }
                else
                {
                    selectItems = db.tblPropertyRoomMaps.Select(x => new CheckBoxList()
                    {
                        Text = x.sRoomName,
                        Value = x.iRoomId,
                        propid = x.iPropId,
                        activeStatus = x.bActive,
                    }).Where(u => u.activeStatus == true && u.propid == PropId).OrderBy(u => u.Text).ToList();
                }

                return selectItems;
            }
        }

        public static List<string> GetParentRatePlanValidity(int? RPId, int PropId)
        {
            List<string> selectedValidity = new List<string>();

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var rec = (from m in db.tblPropertyRatePlanMaps
                           select new
                           {
                               m.iRPId,
                               m.iPropId,
                               m.dtValidFrom,
                               m.dtValidTo,
                           }).Where(u => u.iRPId == RPId && u.iPropId == PropId).AsQueryable().ToList();

                selectedValidity.Add(Convert.ToDateTime(rec[0].dtValidFrom).ToString("dd/MM/yyyy"));
                selectedValidity.Add(Convert.ToDateTime(rec[0].dtValidTo).ToString("dd/MM/yyyy"));
            }
            return selectedValidity;

        }


        public static List<CheckBoxList> GetAmentiesCheckBox()
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                selectItems = db.tblAmenityMs.Select(x => new CheckBoxList()
                {
                    Text = x.sAmenityName,
                    Value = x.iAmenityId,
                    activeStatus = x.cStatus == "A" ? true : false
                }).Where(u => u.activeStatus == true).ToList();
                return selectItems;
            }
        }

        public static List<CheckBoxList> GetAmentiesCheckBox(string data)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                string[] objSelectItems;
                if (data != null)
                {
                    objSelectItems = data.Split(',');
                    selectItems = (from x in db.tblAmenityMs.Where(x => x.cStatus == "A").ToList()
                                   join si in objSelectItems.ToList() on x.iAmenityId equals Convert.ToInt32(si) into result
                                   from r in result.DefaultIfEmpty()
                                   select new CheckBoxList()
                                   {
                                       Text = x.sAmenityName,
                                       Value = x.iAmenityId,
                                       Selected = r == null ? false : true
                                   }).ToList();
                }
                else
                {
                    selectItems = db.tblAmenityMs.Where(x => x.cStatus == "A").Select(x => new CheckBoxList()
                    {
                        Text = x.sAmenityName,
                        Value = x.iAmenityId
                    }).ToList();
                }
                return selectItems;
            }
        }

        public static List<CheckBoxList> GetCancellationPolicy(int PropId)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                selectItems = db.tblPropertyCancellationPolicyMaps.Select(x => new CheckBoxList()
                {
                    Text = x.sPolicyName,
                    Value = x.iCancellationPolicyId,
                    propid = x.iPropId,
                    activeStatus = x.cStatus == "A" ? true : false
                }).Where(u => u.activeStatus == true && u.propid == PropId).ToList();
                return selectItems;
            }
        }

        public static List<CheckBoxList> GetCancellationPolicy(int PropId, string data)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                string[] objSelectItems;
                if (data != null)
                {
                    objSelectItems = data.Split(',');
                    selectItems = (from x in db.tblPropertyCancellationPolicyMaps.ToList()
                                   join si in objSelectItems.ToList() on x.iCancellationPolicyId equals Convert.ToInt32(si) into result
                                   from r in result.DefaultIfEmpty()
                                   select new CheckBoxList()
                                   {
                                       Text = x.sPolicyName,
                                       Value = x.iCancellationPolicyId,
                                       propid = x.iPropId,
                                       activeStatus = x.cStatus == "A" ? true : false,
                                       //Selected = r == null ? false : true
                                       Selected = false
                                   }).Where(u => u.activeStatus == true && u.propid == PropId).ToList();
                }
                else
                {
                    selectItems = db.tblPropertyCancellationPolicyMaps.Select(x => new CheckBoxList()
                    {
                        Text = x.sPolicyName,
                        Value = x.iCancellationPolicyId,
                        propid = x.iPropId,
                        activeStatus = x.cStatus == "A" ? true : false
                    }).Where(u => u.activeStatus == true && u.propid == PropId).ToList();
                }
                return selectItems;
            }
        }

        public static List<etblPropertyRatePlanMap> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount, int propId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<etblPropertyRatePlanMap> RatePlanLst = new List<etblPropertyRatePlanMap>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;
                var objTblRatePlan = db.Database.SqlQuery<etblPropertyRatePlanMap>("uspPropertyRatePlanList").ToList();

                var result = objTblRatePlan.Where(a => a.iPropId == propId && (a.sRatePlan.ToLower().Contains(param.sSearch.ToLower()) || a.sType.ToLower().Contains(param.sSearch.ToLower())));
                //get Total Count for show total records 
                TotalCount = result.Count();

                //for sorting
                switch (param.iSortingCols)
                {
                    case 0:
                        result = param.sortDirection == "asc" ? result.OrderBy(u => u.iRPId) : result.OrderByDescending(u => u.iRPId);
                        break;
                    case 1:
                        result = param.sortDirection == "asc" ? result.OrderBy(u => u.sRatePlan) : result.OrderByDescending(u => u.sRatePlan);
                        break;
                    case 2:
                        result = param.sortDirection == "asc" ? result.OrderBy(u => u.sValidity) : result.OrderByDescending(u => u.sValidity);
                        break;
                }

                ////implemented paging
                var lstUser = result.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                //convert bll entity object to db entity object
                foreach (var item in lstUser)
                {
                    RatePlanLst.Add((etblPropertyRatePlanMap)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblPropertyRatePlanMap()));
                }
                return RatePlanLst;
            }
        }


    }

}