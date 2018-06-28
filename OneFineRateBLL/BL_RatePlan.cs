using OneFineRate;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace OneFineRateBLL
{
    public class BL_RatePlan
    {
        #region functions
        //Add new record
        public static int AddRecord(eRatePlan eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {

                    int dbobj = (from s in db.tblRatePlanMs.Where(u => u.sRatePlan == eobj.sRatePlan.Trim())
                                 select new
                                 {
                                     s.iRatePlanId,
                                 }).Count();
                    if (dbobj > 0)
                    {
                        return retval = 2;
                    }

                    OneFineRate.tblRatePlanM dbuser = (OneFineRate.tblRatePlanM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblRatePlanM());
                    db.tblRatePlanMs.Add(dbuser);
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
        public static int DeleteRecord(int id)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var obj = db.tblRatePlanMs.SingleOrDefault(u => u.iRatePlanId == id);
                    db.tblRatePlanMs.Attach(obj);
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
        public static int UpdateRecord(eRatePlan eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    int dbobj = (from s in db.tblRatePlanMs.Where(u => u.sRatePlan == eobj.sRatePlan.Trim() && u.iRatePlanId != eobj.iRatePlanId)
                                 select new
                                 {
                                     s.iRatePlanId,
                                 }).Count();
                    if (dbobj > 0)
                    {
                        return retval = 2;
                    }

                    OneFineRate.tblRatePlanM obj = (OneFineRate.tblRatePlanM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblRatePlanM());
                    db.tblRatePlanMs.Attach(obj);
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
        public static eRatePlan GetSingleRecordById(int id)
        {
            eRatePlan eobj = new eRatePlan();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblRatePlanMs.SingleOrDefault(u => u.iRatePlanId == id);
                if (dbobj != null)
                    eobj = (eRatePlan)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }
        //Get all records
        public static List<eRatePlan> GetAllRatePlans()
        {

            List<eRatePlan> eobj = new List<eRatePlan>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                foreach (OneFineRate.tblRatePlanM item in db.tblRatePlanMs.Where(u => u.cStatus == "A").ToList())
                {
                    eobj.Add((eRatePlan)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eRatePlan()));
                }
            }
            return eobj;
        }

        //Get list of records
        public static List<eRatePlanDisp> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<eRatePlanDisp> RatePlanLst = new List<eRatePlanDisp>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;
                var objTblRatePlan = (from s in db.tblRatePlanMs
                                     join c in db.tblUserMs on s.iActionBy equals c.iUserId into Tax
                                     from T in Tax.DefaultIfEmpty()

                                     select new
                                     {
                                         s.iRatePlanId,
                                         s.sRatePlan,
                                         s.dtActionDate,
                                         cStatus = s.cStatus == "A" ? "Live" : "Disable",
                                         ActionBy = T.sFirstName + " " + T.sLastName
                                     }).Where(u => u.sRatePlan.Contains(param.sSearch)).AsQueryable();


                //get Total Count for show total records
                TotalCount = objTblRatePlan.Count();

                //for sorting
                switch (param.iSortingCols)
                {
                    case 0:
                        objTblRatePlan = param.sortDirection == "asc" ? objTblRatePlan.OrderBy(u => u.sRatePlan) : objTblRatePlan.OrderByDescending(u => u.sRatePlan);
                        break;
                    case 1:
                        objTblRatePlan = param.sortDirection == "asc" ? objTblRatePlan.OrderBy(u => u.cStatus) : objTblRatePlan.OrderByDescending(u => u.cStatus);
                        break;

                }

                ////implemented paging
                var lstUser = objTblRatePlan.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                //convert bll entity object to db entity object
                foreach (var item in lstUser)
                {
                    RatePlanLst.Add((eRatePlanDisp)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eRatePlanDisp()));
                }
                return RatePlanLst;
                //param.sSearch = param.sSearch == null ? "" : param.sSearch;

                //SqlParameter[] MyParam = new SqlParameter[5];
                //MyParam[0] = new SqlParameter("@RatePlan", param.sSearch + "%");
                //MyParam[1] = new SqlParameter("@DisplayLength", param.iDisplayLength);
                //MyParam[2] = new SqlParameter("@DisplayStart", param.iDisplayStart);
                //MyParam[3] = new SqlParameter("@SortDirection", param.sortDirection == "asc" ? "A" : "D");
                //MyParam[4] = new SqlParameter("@TotalCount", 0);
                //MyParam[4].Direction = System.Data.ParameterDirection.Output;
                ////System.Data.Objects.ObjectParameter output = new System.Data.Objects.ObjectParameter("CustomerCount", typeof(int));
                ////var objTblUser = db.Database.SqlQuery<eRatePlanDisp>("uspGetAmenitiesByName @RatePlan, @DisplayLength, @DisplayStart, @SortDirection", new SqlParameter("@RatePlan", param.sSearch + "%"),new SqlParameter("@DisplayLength", param.iDisplayLength),new SqlParameter("@DisplayStart", param.iDisplayStart),new SqlParameter("@SortDirection", param.sortDirection == "asc" ? "A" : "D"));
                //user = db.Database.SqlQuery<eRatePlanDisp>("uspGetRatePlanByName @RatePlan, @DisplayLength, @DisplayStart, @SortDirection, @TotalCount out", MyParam).ToList();

                //TotalCount = Convert.ToInt16(MyParam[4].Value);

                //return user;
            }
        }


        #endregion
    }
}