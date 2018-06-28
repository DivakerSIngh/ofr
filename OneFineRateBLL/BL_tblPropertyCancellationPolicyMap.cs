using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;

namespace OneFineRateBLL
{
    public class BL_tblPropertyCancellationPolicyMap
    {
        //Get Single Record
        public static etblPropertyCancellationPolicyMap GetSingleRecordById(int id)
        {
            etblPropertyCancellationPolicyMap eobj = new etblPropertyCancellationPolicyMap();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblPropertyCancellationPolicyMaps.SingleOrDefault(u => u.iCancellationPolicyId == id);
                if (dbobj != null)
                    eobj = (etblPropertyCancellationPolicyMap)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }

        public static int CheckStatus(int PropId, int RatePlanId, int RoomTypeId, DateTime stayfrom, DateTime stayto)
        {
            int result = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                result = db.tblPropertyTaxMaps.Where(u => u.iPropId == PropId && u.iRPId == RatePlanId && u.iRoomId == RoomTypeId && ((stayfrom > u.dtStayTo && stayto > u.dtStayTo) || (stayto < u.dtStayFrom && stayfrom < u.dtStayFrom))).AsQueryable().Count();

            }
            return result;

        }

        public static bool CheckIfPolicyNameExists(string policyName, int propId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    return db.tblPropertyCancellationPolicyMaps.Any(x => x.sPolicyName.Equals(policyName, StringComparison.InvariantCultureIgnoreCase) && x.iPropId.Equals(propId));
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        public static bool CheckIfPolicyNameExistsOnUpdate(string policyName, int iCancellationPolicyId, int iPropId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    return db.tblPropertyCancellationPolicyMaps
                        .Where(x => x.iPropId == iPropId)
                        .Except(db.tblPropertyCancellationPolicyMaps.Where(x => x.iCancellationPolicyId == iCancellationPolicyId && x.iPropId == iPropId))
                        .Any(x => x.sPolicyName.Equals(policyName, StringComparison.InvariantCultureIgnoreCase));
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }



        //Add new record
        public static int AddRecord(etblPropertyCancellationPolicyMap eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblPropertyCancellationPolicyMap dbuser = (OneFineRate.tblPropertyCancellationPolicyMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyCancellationPolicyMap());
                    db.tblPropertyCancellationPolicyMaps.Add(dbuser);
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
        //Update a record
        public static int UpdateRecord(etblPropertyCancellationPolicyMap eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblPropertyCancellationPolicyMap obj = (OneFineRate.tblPropertyCancellationPolicyMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyCancellationPolicyMap());
                    db.tblPropertyCancellationPolicyMaps.Attach(obj);
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
        //Get list of records
        public static List<etblPropertyCancellationPolicyMap> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, int propid, out int TotalCount)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                IQueryable<etblPropertyCancellationPolicyMap> objTblUser;
                List<etblPropertyCancellationPolicyMap> user = new List<etblPropertyCancellationPolicyMap>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;
                //if (datefrom.ToString() == "01-01-0001 00:00:00" || dateto.ToString() == "01-01-0001 00:00:00")
                //{
                objTblUser = (from t1 in db.tblPropertyCancellationPolicyMaps
                              join t2 in db.tblUserMs on t1.iActionBy equals t2.iUserId
                              where t1.iPropId == propid
                              select new etblPropertyCancellationPolicyMap
                              {
                                  sPolicyName = t1.sPolicyName,
                                  iPropId = t1.iPropId,
                                  iCancellationPolicyId = t1.iCancellationPolicyId,
                                  dtValidFrom = t1.dtValidFrom,
                                  dtValidTo = t1.dtValidTo,
                                  bIsNonRefundable = t1.bIsNonRefundable,
                                  cStatus = t1.cStatus == "A" ? "Live" : "Disable",
                                  dtActionDate = t1.dtActionDate,
                                  ActionBy = t2.sFirstName + " " + t2.sLastName

                              }).Where(u => u.sPolicyName.Contains(param.sSearch)).AsQueryable();

                //}
                //else
                //{
                //    objTblUser = (from t1 in db.tblPropertyCancellationPolicyMaps
                //                  join t2 in db.tblUserMs on t1.iActionBy equals t2.iUserId
                //                  select new etblPropertyCancellationPolicyMap
                //                  {
                //                      sPolicyName = t1.sPolicyName,
                //                      iPropId = t1.iPropId,
                //                      iCancellationPolicyId = t1.iCancellationPolicyId,
                //                      dtValidFrom = t1.dtValidFrom,
                //                      dtValidTo = t1.dtValidTo,
                //                      bIsNonRefundable = t1.bIsNonRefundable,
                //                      cStatus = t1.cStatus == "A" ? "Live" : "Disable",
                //                      dtActionDate = t1.dtActionDate,
                //                      ActionBy = t2.sFirstName + " " + t2.sLastName

                //                  }).Where(u => u.sPolicyName.Contains(param.sSearch) && (u.dtValidFrom >= datefrom && u.dtValidTo <= dateto)).AsQueryable();

                //}

                //for searching
                // var objTblUser = db.tblPropertyMs.Where(u => u.sHotelName.Contains(param.sSearch)).AsQueryable();

                //get Total Count for show total records
                TotalCount = objTblUser.Count();

                //for sorting
                switch (param.iSortingCols)
                {
                    case 0:
                        objTblUser = param.sortDirection == "asc" ? objTblUser.OrderBy(u => u.iCancellationPolicyId) : objTblUser.OrderByDescending(u => u.iCancellationPolicyId);
                        break;
                    case 1:
                        objTblUser = param.sortDirection == "asc" ? objTblUser.OrderBy(u => u.sPolicyName) : objTblUser.OrderByDescending(u => u.sPolicyName);
                        break;
                }
                ////implemented paging
                var lstUser = objTblUser.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                //convert bll entity object to db entity object
                foreach (var item in lstUser)
                {
                    user.Add((etblPropertyCancellationPolicyMap)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblPropertyCancellationPolicyMap()));
                }
                return user;
            }
        }

        public static string CheckPolicyMapping(int id)
        {
            string eobj = "";
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = (from t1 in db.tblPropertyCancellationPolicyMaps
                             join t2 in db.tblPropertyPromotionCancellationMaps on t1.iCancellationPolicyId equals t2.iCancellationPolicyId
                             where t1.iCancellationPolicyId == id
                             join t3 in db.tblPropertyPromotionMaps on t2.iID equals t3.iID
                             where t3.cStatus == "A"
                             select new
                             {
                                 t2.iID
                             }).Distinct().AsQueryable().ToList();
                if (dbobj != null)
                    if (dbobj.Count > 1)
                    {
                        eobj = "This cancellation policy cannot be disabled because it is mapped with promotions ( ID : ";
                        for (int i = 0; i < dbobj.Count; i++)
                        {

                            if (i == 0)
                            {
                                eobj += dbobj[i].iID.ToString();
                            }
                            else if (i == dbobj.Count - 1)
                            {
                                eobj += " and " + dbobj[i].iID.ToString();
                            }
                            else
                            {
                                eobj += ", " + dbobj[i].iID.ToString();
                            }

                        }
                        eobj += " )";
                    }
                    else if (dbobj.Count == 1)
                    {
                        eobj = "This cancellation policy cannot be disabled because it is mapped with promotion ( ID : " + dbobj[0].iID.ToString() + " )";
                    }

                var dbobj2 = (from t1 in db.tblPropertyCancellationPolicyMaps
                              join t2 in db.tblPropertyRatePlanCancellationMaps on t1.iCancellationPolicyId equals t2.iCancellationPolicyId
                              where t1.iCancellationPolicyId == id
                              join t3 in db.tblPropertyRatePlanMaps on t2.iRPId equals t3.iRPId
                              where t3.cStatus == "A"
                              select new
                              {
                                  t2.iRPId
                              }).Distinct().AsQueryable().ToList();
                if (dbobj2 != null)
                    if (dbobj2.Count > 1)
                    {
                        if (eobj == "")
                            eobj = "This cancellation policy cannot be disabled because it is mapped with rate plans ( ID : ";
                        else
                            eobj += " and rate plans ( ID : ";
                        for (int i = 0; i < dbobj2.Count; i++)
                        {

                            if (i == 0)
                            {
                                eobj += dbobj2[i].iRPId.ToString();
                            }
                            else if (i == dbobj2.Count - 1)
                            {
                                eobj += " and " + dbobj2[i].iRPId.ToString();
                            }
                            else
                            {
                                eobj += ", " + dbobj2[i].iRPId.ToString();
                            }

                        }
                        eobj += " )";
                    }
                    else if (dbobj2.Count == 1)
                    {
                        if (eobj == "")
                            eobj = "This cancellation policy cannot be disabled because it is mapped with rate plan ( ID : " + dbobj2[0].iRPId.ToString() + " )";
                        else
                            eobj += " and rate plan ( ID : " + dbobj2[0].iRPId.ToString() + ")";

                    }
            }
            return eobj;

        }
    }
}