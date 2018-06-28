using OneFineRate;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_AdditionalCommission
    {
        #region Functions

        //Add new record
        public static int AddUpdateRecord(etblAdditionalCommission eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    SqlParameter[] parameters =
                   {
                            new SqlParameter("@iAdditionalCommisionId",eobj.iAdditionalCommisionId), 
                            new SqlParameter("@dtCommisionStartDate",eobj.dtCommisionStartDate), 
                            new SqlParameter("@dtCommisionEndDate",eobj.dtCommisionEndDate),
                            new SqlParameter("@bActive",eobj.bActive),
                            new SqlParameter("@dCommission",eobj.dCommission),
                            new SqlParameter("@iPropId",eobj.iPropId),
                            new SqlParameter("@iActionBy",eobj.iActionBy)
                   };

                    retval = db.Database.SqlQuery<int>("uspSetAdditionalCommision @iAdditionalCommisionId, @dtCommisionStartDate,@dtCommisionEndDate, @bActive, @dCommission, @iPropId, @iActionBy", parameters).SingleOrDefault();

                    //OneFineRate.tblAdditionalCommision dbstate = (OneFineRate.tblAdditionalCommision)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblAdditionalCommision());
                    //db.tblAdditionalCommisions.Add(dbstate);
                    //db.SaveChanges();
                    //retval = 1;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retval;
        }

        //Delete a record
        public static int DeleteRecord(int iCommissionId)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var obj = db.tblAdditionalCommisions.SingleOrDefault(u => u.iAdditionalCommisionId == iCommissionId);
                    db.tblAdditionalCommisions.Attach(obj);
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
        //public static int UpdateRecord(etblAdditionalCommission eobj)
        //{
        //    int retval = 0;
        //    using (OneFineRateEntities db = new OneFineRateEntities())
        //    {
        //        try
        //        {
        //            //More Login here...

        //            OneFineRate.tblAdditionalCommision obj = (OneFineRate.tblAdditionalCommision)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblAdditionalCommision());
        //            db.tblAdditionalCommisions.Attach(obj);
        //            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        //            db.SaveChanges();
        //            retval = 1;
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //    }
        //    return retval;
        //}


        //Update a record
        //Delete a record
        public static int UpdateCommissionStatus(int iCommissionId, bool status)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var obj = db.tblAdditionalCommisions.SingleOrDefault(u => u.iAdditionalCommisionId == iCommissionId);
                    obj.bActive = status;
                    db.tblAdditionalCommisions.Attach(obj);
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
        public static etblAdditionalCommission GetSingleRecordById(int iCommissionId)
        {
            etblAdditionalCommission eobj = new etblAdditionalCommission();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblAdditionalCommisions.SingleOrDefault(u => u.iAdditionalCommisionId == iCommissionId);
                if (dbobj != null)
                {
                    eobj.iAdditionalCommisionId = dbobj.iAdditionalCommisionId;
                    eobj.bActive = dbobj.bActive;
                    eobj.dCommission = dbobj.dCommission;
                    eobj.dtActionDate = dbobj.dtActionDate;
                    eobj.dtCommisionEndDate = dbobj.dtCommisionEndDate.Value.ToString("dd/MM/yyyy");
                    eobj.dtCommisionStartDate = dbobj.dtCommisionStartDate.Value.ToString("dd/MM/yyyy");
                    eobj.iActionBy = dbobj.iActionBy;
                    eobj.iPropId = dbobj.iPropId;
                }

            }
            return eobj;

        }

        public static decimal GetPropertyCommission(int propertyId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                return db.tblBankDetailMs.Where(x => x.iPropId == propertyId).Select(x => x.dCommission).SingleOrDefault().GetValueOrDefault();
            }
        }

        //Get all records
        public static List<etblAdditionalCommission> GetAllRecordsByPropertyId(int iPropId)
        {
            List<etblAdditionalCommission> eobj = new List<etblAdditionalCommission>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                foreach (OneFineRate.tblAdditionalCommision item in db.tblAdditionalCommisions.Where(u => u.iPropId == iPropId).ToList())
                {
                    eobj.Add((etblAdditionalCommission)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblAdditionalCommission()));
                }
            }
            return eobj;
        }

        // Added for Addional commission detail to show in grid.
        //Get list of records
        public static List<etblAdditionalCommission> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, int propid, out int TotalCount)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<etblAdditionalCommission> commissionList = new List<etblAdditionalCommission>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;

                var objTblUser = (from t1 in db.tblAdditionalCommisions
                                  join t2 in db.tblPropertyMs on t1.iPropId equals t2.iPropId
                                  join u in db.tblUserMs on t1.iActionBy equals u.iUserId
                                  where t1.iPropId == propid
                                  select new
                                  {
                                      t1.iPropId,
                                      t1.iAdditionalCommisionId,
                                      t1.dtCommisionStartDate,
                                      t1.dtCommisionEndDate,
                                      t1.bActive,
                                      t1.dCommission,
                                      t1.dtActionDate,
                                      t2.sHotelName,
                                      iActionByName = u.sFirstName + " " + u.sLastName,
                                  }).AsQueryable();

                //get Total Count for show total records
                TotalCount = objTblUser.Count();

                //for sorting
                switch (param.iSortingCols)
                {
                    case 0:
                        objTblUser = param.sortDirection == "asc" ? objTblUser.OrderBy(u => u.dtCommisionStartDate) : objTblUser.OrderByDescending(u => u.dtCommisionStartDate);
                        break;
                    case 1:
                        objTblUser = param.sortDirection == "asc" ? objTblUser.OrderBy(u => u.dtCommisionEndDate) : objTblUser.OrderByDescending(u => u.dtCommisionEndDate);
                        break;
                    case 2:
                        objTblUser = param.sortDirection == "asc" ? objTblUser.OrderBy(u => u.dCommission) : objTblUser.OrderByDescending(u => u.dCommission);
                        break;
                    case 3:
                        objTblUser = param.sortDirection == "asc" ? objTblUser.OrderBy(u => u.bActive) : objTblUser.OrderByDescending(u => u.bActive);
                        break;
                    case 4:
                        objTblUser = param.sortDirection == "asc" ? objTblUser.OrderBy(u => u.dtActionDate) : objTblUser.OrderByDescending(u => u.dtActionDate);
                        break;
                    default:
                        break;

                }

                ////implemented paging
                var data = objTblUser.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                //convert bll entity object to db entity object
                foreach (var item in data)
                {
                    commissionList.Add(new etblAdditionalCommission()
                    {

                        dtCommisionEndDate = item.dtCommisionEndDate.Value.ToString("dd/MM/yyy"),
                        dtCommisionStartDate = item.dtCommisionStartDate.Value.ToString("dd/MM/yyy"),
                        iPropId = item.iPropId,
                        iAdditionalCommisionId = item.iAdditionalCommisionId,
                        iActionByName = item.iActionByName,
                        bActive = item.bActive,
                        dtActionDate = item.dtActionDate,
                        dCommission = item.dCommission,
                    });
                }
                return commissionList;
            }
        }

        #endregion
    }
}