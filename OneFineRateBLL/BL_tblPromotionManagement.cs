
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;
using System.Globalization;
using System.Data.SqlClient;


namespace OneFineRateBLL
{
    public class BL_tblPromotionManagement
    {
        public static List<CheckBoxList> GetRoomTypeCheckBox(int PropId)
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
                }).Where(u => u.propid == PropId && u.activeStatus == true).OrderBy(u => u.Text).ToList();
                return selectItems;
            }
        }

        public static List<CheckBoxList> GetRoomTypeCheckBox(int PropId, int iId, int? RPId)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                //selectItems = db.tblPropertyRoomMaps.Select(x => new CheckBoxList()
                //{
                //    Text = x.sRoomName,
                //    Value = x.iRoomId,
                //    propid = x.iPropId,
                //    activeStatus = x.bActive
                //}).Where(u => u.propid == PropId && u.activeStatus == true).ToList();

                var selectedRooms = (from x in db.tblPropertyPromotionMaps
                                     join s in db.tblPropertyPromotionRoomTypeMaps on x.iID equals s.iPropPromoID
                                     select new CheckBoxList()
                                     {
                                         id = (int)s.iRoomId,
                                         iRPid = x.iID
                                     }).Where(u => u.iRPid == iId).ToList();


                selectItems = (from x in db.tblPropertyRoomMaps.ToList()
                               join s in db.tblPropertyRatePlanRoomTypeMaps on x.iRoomId equals s.iRoomId
                               join si in selectedRooms.ToList() on x.iRoomId equals Convert.ToInt32(si.id) into result
                               from r in result.DefaultIfEmpty()
                               select new CheckBoxList()
                               {
                                   propid = x.iPropId,
                                   iRPid = s.iRPId,
                                   activeStatus = x.bActive,
                                   Text = x.sRoomName,
                                   Value = Convert.ToInt32(x.iRoomId),
                                   Selected = r == null ? false : true
                               }).Distinct().Where(u => u.propid == PropId && u.iRPid == RPId && u.activeStatus == true).OrderBy(u => u.Text).ToList();

                return selectItems;
            }
        }

        public static List<CheckBoxList> GetRoomTypeCheckBox(string data)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                string[] objSelectItems;
                if (data != null)
                {
                    objSelectItems = data.Split(',');
                    selectItems = (from x in db.tblPropertyRoomMaps.ToList()
                                   join si in objSelectItems.ToList() on x.iRoomId equals Convert.ToInt32(si) into result
                                   from r in result.DefaultIfEmpty()
                                   select new CheckBoxList()
                                   {
                                       Text = x.sRoomName,
                                       Value = Convert.ToInt32(x.iRoomId),
                                       Selected = r == null ? false : true
                                   }).ToList();
                }
                else
                {
                    selectItems = db.tblPropertyRoomMaps.Select(x => new CheckBoxList()
                    {
                        Text = x.sRoomName,
                        Value = Convert.ToInt32(x.iRoomId)
                    }).ToList();
                }
                return selectItems;
            }
        }

        public static List<CheckBoxList> GetRoomTypeCheckBox(int PropId, int iId)
        {
            List<CheckBoxList> selectItems = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var selectedRooms = (from x in db.tblPropertyPromotionMaps
                                     join s in db.tblPropertyPromotionRoomTypeMaps on x.iID equals s.iPropPromoID
                                     select new CheckBoxList()
                                     {
                                         id = (int)s.iRoomId,
                                         iRPid = x.iID
                                     }).Where(u => u.iRPid == iId).ToList();


                selectItems = (from x in db.tblPropertyRoomMaps.ToList()
                               join si in selectedRooms.ToList() on x.iRoomId equals Convert.ToInt32(si.id) into result
                               from r in result.DefaultIfEmpty()
                               select new CheckBoxList()
                               {
                                   propid = x.iPropId,
                                   activeStatus = x.bActive,
                                   Text = x.sRoomName,
                                   Value = Convert.ToInt32(x.iRoomId),
                                   Selected = r == null ? false : true
                               }).Distinct().Where(u => u.propid == PropId && u.activeStatus == true).ToList();

                return selectItems;
            }
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

        public static List<CheckBoxList> GetCancellationPolicy(int propId)
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
                }).Where(u => u.activeStatus == true && u.propid == propId).ToList();
                return selectItems;
            }
        }

        public static List<CheckBoxList> GetCancellationPolicy(int propId, string data)
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
                                       propid = x.iPropId,
                                       Text = x.sPolicyName,
                                       Value = x.iCancellationPolicyId,
                                       activeStatus = x.cStatus == "A" ? true : false,
                                       Selected = r == null ? false : true
                                   }).Where(u => u.propid == propId && u.activeStatus == true).ToList();
                }
                else
                {
                    selectItems = db.tblPropertyCancellationPolicyMaps.Select(x => new CheckBoxList()
                    {
                        propid = x.iPropId,
                        Text = x.sPolicyName,
                        activeStatus = x.cStatus == "A" ? true : false,
                        Value = x.iCancellationPolicyId
                    }).Where(u => u.propid == propId && u.activeStatus == true).ToList();
                }
                return selectItems;
            }
        }

        public static List<etblPropertyRatePlanMap> GetAllRecords(int propId)
        {
            List<etblPropertyRatePlanMap> eobj = new List<etblPropertyRatePlanMap>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var obj = (from x in db.tblPropertyRatePlanMaps.ToList()
                           select new etblPropertyRatePlanMap()
                           {
                               iPropId = x.iPropId,
                               sRatePlan = x.sRatePlan,
                               iRPId = x.iRPId,
                               cStatus = x.cStatus
                           }).Where(u => u.iPropId == propId && u.cStatus == "A").OrderBy(u => u.sRatePlan).ToList();

                foreach (var item in obj)
                {
                    eobj.Add((etblPropertyRatePlanMap)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblPropertyRatePlanMap()));

                }

            }
            return eobj;
        }
        public static int GetPromoID(string PromoName)
        {
            int promoID = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                foreach (OneFineRate.tblPromoM item in db.tblPromoMs.ToList())
                {
                    if (item.sPromoName == PromoName)
                    {
                        promoID = item.iPromoId;
                    }
                }
            }
            return promoID;
        }


        public static List<etblPropertyPromotionMap> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount, int promoid, int propid)
        {
            List<etblPropertyPromotionMap> promorec = new List<etblPropertyPromotionMap>();
            List<etblPropertyPromotionMap> objResult = new List<etblPropertyPromotionMap>();
            param.sSearch = param.sSearch == null ? "" : param.sSearch;
            using (OneFineRateEntities _context = new OneFineRateEntities())
            {
                SqlParameter[] MyParam = new SqlParameter[2];
                MyParam[0] = new SqlParameter("@promoid", promoid);
                MyParam[1] = new SqlParameter("@propid", propid);
                objResult = _context.Database.SqlQuery<etblPropertyPromotionMap>("uspPromotionsListByID @promoid , @propid", MyParam).ToList();

                var result = objResult.Where(a => a.sRoomTypeId.ToLower().Contains(param.sSearch.ToLower()));




                //for sorting 
                switch (param.iSortingCols)
                {
                    case 1:
                        result = param.sortDirection == "asc" ? result.OrderBy(u => u.dtBookingDateFrom) : result.OrderByDescending(u => u.dtBookingDateFrom);
                        break;
                    case 2:
                        result = param.sortDirection == "asc" ? result.OrderBy(u => u.dtBookingDateTo) : result.OrderByDescending(u => u.dtBookingDateTo);
                        break;
                    case 3:
                        result = param.sortDirection == "asc" ? result.OrderBy(u => u.dtStayDateFrom) : result.OrderByDescending(u => u.dtStayDateFrom);
                        break;
                    case 4:
                        result = param.sortDirection == "asc" ? result.OrderBy(u => u.dtStayDateTo) : result.OrderByDescending(u => u.dtStayDateTo);
                        break;
                }

                //get Total Count for show total records
                var result1 = result.ToList();
                TotalCount = result1.Count;

                ////implemented paging
                var lstUser = result.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                foreach (var item in lstUser)
                {
                    promorec.Add((etblPropertyPromotionMap)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblPropertyPromotionMap()));
                }



                return promorec;

            }
        }

        public static List<etblPropertyPromotionMap> getPropertyPromoDataByID(int a)
        {
            List<etblPropertyPromotionMap> promorec = new List<etblPropertyPromotionMap>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var rec = (from m in db.tblPropertyPromotionMaps
                           join n in db.tblPropertyRatePlanMaps on m.iRPId equals n.iRPId into result
                           from r in result.DefaultIfEmpty()
                           join c in db.tblUserMs on m.iActionBy equals c.iUserId
                           select new
                           {
                               m.iID,
                               m.dtBookingDateFrom,
                               m.dtBookingDateTo,
                               m.dtStayDateFrom,
                               m.dtStayDateTo,
                               m.iRPId,
                               m.sRoomTypeId,
                               m.dValue,
                               m.iActionBy,
                               m.iPromoId,
                               m.iPropId,
                               m.bIsPlus,
                               m.bIsPercent,
                               m.sAmenityId,
                               m.sAmenity,
                               m.sCancellationPolicy,
                               m.bIsSecretDeal,
                               m.iHrsDays,
                               m.iMinLengthStay,
                               m.iMaxLengthStay,
                               m.dtActionDate,
                               m.cStatus,
                               r.dtValidFrom,
                               r.dtValidTo,
                               UserName = c.sFirstName + " " + c.sLastName,
                               m.dNegotiationPer,
                           }
                                   ).Where(u => u.iID == a).AsQueryable();

                var data = rec.ToList();

                etblPropertyPromotionMap objMain = new etblPropertyPromotionMap();
                objMain = (etblPropertyPromotionMap)OneFineRateAppUtil.clsUtils.ConvertToObject(data[0], objMain);
                promorec.Add(new etblPropertyPromotionMap()
                {
                    iID = objMain.iID,
                    dtBookingDateFrom = objMain.dtBookingDateFrom,
                    dtBookingDateTo = objMain.dtBookingDateTo,
                    dtStayDateFrom = objMain.dtStayDateFrom,
                    dtStayDateTo = objMain.dtStayDateTo,
                    iRPId = objMain.iRPId,
                    sRoomTypeId = objMain.sRoomTypeId,
                    dValue = objMain.dValue,
                    iActionBy = objMain.iActionBy,
                    iPromoId = objMain.iPromoId,
                    iPropId = objMain.iPropId,
                    bIsPlus = objMain.bIsPlus,
                    bIsPercent = objMain.bIsPercent,
                    sAmenityId = objMain.sAmenityId,
                    sAmenity = objMain.sAmenity,
                    sCancellationPolicy = objMain.sCancellationPolicy,
                    bIsSecretDeal = objMain.bIsSecretDeal,
                    iHrsDays = objMain.iHrsDays,
                    iMinLengthStay = objMain.iMinLengthStay,
                    iMaxLengthStay = objMain.iMaxLengthStay,
                    dtActionDate = objMain.dtActionDate,
                    cStatus = objMain.cStatus,
                    UserName = objMain.UserName,
                    dtRPValidFrom = objMain.dtValidFrom.ToString("dd/MM/yyyy"),
                    dtRPValidTo = objMain.dtValidTo.ToString("dd/MM/yyyy"),
                    dNegotiationPer = objMain.dNegotiationPer
                });

                var CancellationData = (from m in db.tblPropertyPromotionCancellationMainMaps
                                        select new
                                        {
                                            m.iID,
                                            m.dtCancellationValidFrom,
                                            m.dtCancellationValidTo,
                                            m.sCancellationPolicyId,
                                        }).Where(u => u.iID == a).AsQueryable();

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
                        if (objPolicy[i].sPolicyName == null)
                        {
                            objPolicy[i].sPolicyName = getName[0].sPolicyName + (getName[0].cStatus == "A" ? "" : "(Disabled)");
                        }
                        else
                        {
                            objPolicy[i].sPolicyName = objPolicy[i].sPolicyName + "," + getName[0].sPolicyName + (getName[0].cStatus == "A" ? "" : "(Disabled)");
                        }

                    }
                    objPolicy[i].CancellationPolicyName = objPolicy[i].sPolicyName.Split(',').ToList();

                }
                promorec[0].CancellationPolicyGrid = objPolicy;

                //foreach (var item in data)
                //{
                //    promorec.Add((etblPropertyPromotionMap)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblPropertyPromotionMap()));
                //}

                return promorec;

            }
        }
    }
}