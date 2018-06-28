using OneFineRate;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_LoyalityAmenityMap
    {
        #region functions
        //Add new record
        public static int AddLoyalityAmenityMap(eLoyalityAmenityMap eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblLoyalityAmenityMap dbuser = (OneFineRate.tblLoyalityAmenityMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblLoyalityAmenityMap());
                    db.tblLoyalityAmenityMaps.Add(dbuser);
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
        public static int DeleteData(int id)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var obj = db.tblLoyalityAmenityMaps.SingleOrDefault(u => u.iLoyalityAmenityId == id);
                    db.tblLoyalityAmenityMaps.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
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
        public static int UpdateLoyalityPoints(eLoyalityPoints eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblLoyalityPointsM obj = (OneFineRate.tblLoyalityPointsM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblLoyalityPointsM());
                    db.tblLoyalityPointsMs.Attach(obj);
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

        //Get Single Record
        public static eLoyalityPoints GetSingleRecordById()
        {
            eLoyalityPoints eobj = new eLoyalityPoints();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblLoyalityPointsMs.SingleOrDefault();
                if (dbobj != null)
                    eobj = (eLoyalityPoints)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }

        //Get Single Record
        public static eLoyalityPointsCustomerData GetRecordById(int iId)
        {
            eLoyalityPointsCustomerData eobj = new eLoyalityPointsCustomerData();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = (from t in db.tblCustomerPointsMaps
                             select new eLoyalityPointsCustomerData
                             {
                                 iId = t.iId,
                                 iCustomerId = t.iCustomerId,
                                 iTotalPoints = t.iTotalPoints,
                                 iUsedPoints = t.iUsedPoints,
                                 iRemPoints = t.iRemPoints,
                                 dtCreatedOn = t.dtCreatedOn,
                                 dtExpiryOriginal = t.dtExpiryOriginal,
                                 dtExpiry = t.dtExpiry,
                                 cType = t.cType,
                                 cStatus = t.cStatus,

                             }).Where(u => u.iId == iId).ToList();

                if (dbobj != null)
                    eobj = (eLoyalityPointsCustomerData)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj[0], eobj);
            }
            return eobj;

        }

        //Update a record
        public static int UpdateLoyalityPointsExpiryDate(eLoyalityPointsCustomerData eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblCustomerPointsMap obj = (OneFineRate.tblCustomerPointsMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblCustomerPointsMap());
                    db.tblCustomerPointsMaps.Attach(obj);
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

        //Get all records
        public static List<eLoyalityPoints> GetAllAmenities()
        {

            List<eLoyalityPoints> eobj = new List<eLoyalityPoints>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                foreach (OneFineRate.tblLoyalityPointsM item in db.tblLoyalityPointsMs.Where(u => u.cStatus == "A").ToList())
                {
                    eobj.Add((eLoyalityPoints)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eLoyalityPoints()));
                }
            }
            return eobj;
        }

        //Get list of records
        public static List<eLoyalityAmenityDisp> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<eLoyalityAmenityDisp> Amenitylst = new List<eLoyalityAmenityDisp>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;
                var objTblAmenity = (from s in db.tblLoyalityAmenityMaps
                                     join a in db.tblAmenityMs on s.iAmenityId equals a.iAmenityId
                                     join c in db.tblUserMs on s.iActionBy equals c.iUserId into Tax
                                     from T in Tax.DefaultIfEmpty()

                                     select new
                                     {
                                         s.iLoyalityAmenityId,
                                         s.iPoints,
                                         a.sAmenityName,
                                         s.dtActionDate,
                                         cStatus = s.cStatus == "A" ? "Live" : "Disable",
                                         ActionBy = T.sFirstName + " " + T.sLastName
                                     }).Where(u => u.sAmenityName.Contains(param.sSearch)).AsQueryable();

                //get Total Count for show total records
                TotalCount = objTblAmenity.Count();

                //for sorting
                switch (param.iSortingCols)
                {
                    case 0:
                        objTblAmenity = param.sortDirection == "asc" ? objTblAmenity.OrderBy(u => u.iPoints) : objTblAmenity.OrderByDescending(u => u.iPoints);
                        break;
                    case 1:
                        objTblAmenity = param.sortDirection == "asc" ? objTblAmenity.OrderBy(u => u.sAmenityName) : objTblAmenity.OrderByDescending(u => u.sAmenityName);
                        break;
                }

                ////implemented paging
                var lstUser = objTblAmenity.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                //convert bll entity object to db entity object
                foreach (var item in lstUser)
                {
                    Amenitylst.Add((eLoyalityAmenityDisp)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eLoyalityAmenityDisp()));
                }
                return Amenitylst;
            }
        }

        //Get list of records
        public static List<eLoyalityPointsCustomerData> getRecordForSearchCustomerData(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<eLoyalityPointsCustomerData> Customerlst = new List<eLoyalityPointsCustomerData>();
                List<eLoyalityPointsCustomerData> objResult = new List<eLoyalityPointsCustomerData>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;

                objResult = db.Database.SqlQuery<eLoyalityPointsCustomerData>("uspGetAllLoyalityPointsCustomerData").ToList();
                var result = objResult.Where(a => a.DisplayName.ToLower().Contains(param.sSearch.ToLower())
                                               || a.Email.ToLower().Contains(param.sSearch.ToLower())
                                               || a.PhoneNumber.ToLower().Contains(param.sSearch.ToLower()));

                //get Total Count for show total records
                TotalCount = result.Count();

                //for sorting
                switch (param.iSortingCols)
                {
                    case 2:
                        result = param.sortDirection == "asc" ? result.OrderBy(u => u.dtCreatedOn) : result.OrderByDescending(u => u.dtCreatedOn);
                        break;
                   
                    case 6:
                        result = param.sortDirection == "asc" ? result.OrderBy(u => u.iTotalPoints) : result.OrderByDescending(u => u.iTotalPoints);
                        break;
                    case 7:
                        result = param.sortDirection == "asc" ? result.OrderBy(u => u.iRemPoints) : result.OrderByDescending(u => u.iRemPoints);
                        break;
                    case 8:
                        result = param.sortDirection == "asc" ? result.OrderBy(u => u.dtExpiry) : result.OrderByDescending(u => u.dtExpiry);
                        break;

                }

                ////implemented paging
                var lstUser = result.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                //convert bll entity object to db entity object
                foreach (var item in lstUser)
                {
                    Customerlst.Add((eLoyalityPointsCustomerData)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eLoyalityPointsCustomerData()));
                }
                return Customerlst;
            }
        }


        #endregion
    }
}