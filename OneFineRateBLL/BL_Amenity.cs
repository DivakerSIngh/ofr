using OneFineRate;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_Amenity
    {
        #region functions
        //Add new record
        public static int AddRecord(eAmenity eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    int dbobj = (from s in db.tblAmenityMs.Where(u => u.sAmenityName == eobj.sAmenityName.Trim())
                                 select new
                                 {
                                     s.iAmenityId,
                                 }).Count();
                    if (dbobj > 0)
                    {
                        return retval = 2;
                    }

                    OneFineRate.tblAmenityM dbuser = (OneFineRate.tblAmenityM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblAmenityM());
                    db.tblAmenityMs.Add(dbuser);
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
                    var obj = db.tblAmenityMs.SingleOrDefault(u => u.iAmenityId == id);
                    db.tblAmenityMs.Attach(obj);
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
        public static int UpdateRecord(eAmenity eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    int dbobj = (from s in db.tblAmenityMs.Where(u => u.sAmenityName == eobj.sAmenityName.Trim() && u.iAmenityId!=eobj.iAmenityId)
                                 select new
                                 {
                                     s.iAmenityId,
                                 }).Count();
                    if (dbobj > 0)
                    {
                        return retval = 2;
                    }

                    OneFineRate.tblAmenityM obj = (OneFineRate.tblAmenityM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblAmenityM());
                    db.tblAmenityMs.Attach(obj);
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
        public static eAmenity GetSingleRecordById(int id)
        {
            eAmenity eobj = new eAmenity();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblAmenityMs.SingleOrDefault(u => u.iAmenityId == id);
                if (dbobj != null)
                    eobj = (eAmenity)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }
        //Get all records
        public static List<eAmenity> GetAllAmenities()
        {

            List<eAmenity> eobj = new List<eAmenity>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                foreach (OneFineRate.tblAmenityM item in db.tblAmenityMs.Where(u => u.cStatus == "A").ToList())
                {
                    eobj.Add((eAmenity)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eAmenity()));
                }
            }
            return eobj;
        }

        public static List<eApplicability> GetAllApplicabilities()
        {

            List<eApplicability> eobj = new List<eApplicability>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                foreach (OneFineRate.tblApplicabilityM item in db.tblApplicabilityMs.OrderBy(u => u.iSortOrder).ToList())
                {
                    eobj.Add((eApplicability)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eApplicability()));
                }
            }
            return eobj;
        }

        //Get list of records
        public static List<eAmenityDisp> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<eAmenityDisp> Amenitylst = new List<eAmenityDisp>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;
                var objTblAmenity = (from s in db.tblAmenityMs
                                     join c in db.tblUserMs on s.iActionBy equals c.iUserId into Tax
                                     from T in Tax.DefaultIfEmpty()

                                     select new
                                     {
                                         s.iAmenityId,
                                         s.sAmenityName,
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
                        objTblAmenity = param.sortDirection == "asc" ? objTblAmenity.OrderBy(u => u.sAmenityName) : objTblAmenity.OrderByDescending(u => u.sAmenityName);
                        break;
                    case 1:
                        objTblAmenity = param.sortDirection == "asc" ? objTblAmenity.OrderBy(u => u.cStatus) : objTblAmenity.OrderByDescending(u => u.cStatus);
                        break;

                }

                ////implemented paging
                var lstUser = objTblAmenity.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                //convert bll entity object to db entity object
                foreach (var item in lstUser)
                {
                    Amenitylst.Add((eAmenityDisp)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eAmenityDisp()));
                }
                return Amenitylst;
                // SqlParameter[] MyParam = new SqlParameter[5];
                // MyParam[0] = new SqlParameter("@Amenity", param.sSearch + "%");
                // MyParam[1] = new SqlParameter("@DisplayLength", param.iDisplayLength);
                // MyParam[2] = new SqlParameter("@DisplayStart", param.iDisplayStart);
                // MyParam[3] = new SqlParameter("@SortDirection", param.sortDirection == "asc" ? "A" : "D");
                // MyParam[4] = new SqlParameter("@TotalCount", 0);
                // MyParam[4].Direction = System.Data.ParameterDirection.Output;
                // //var objTblUser = db.Database.SqlQuery<eAmenityDisp>("uspGetAmenitiesByName @Amenity, @DisplayLength, @DisplayStart, @SortDirection", new SqlParameter("@Amenity", param.sSearch + "%"),new SqlParameter("@DisplayLength", param.iDisplayLength),new SqlParameter("@DisplayStart", param.iDisplayStart),new SqlParameter("@SortDirection", param.sortDirection == "asc" ? "A" : "D"));
                // user = db.Database.SqlQuery<eAmenityDisp>("uspGetAmenitiesByName @Amenity, @DisplayLength, @DisplayStart, @SortDirection, @TotalCount out", MyParam).ToList();

                // //var objTblUser = db.tblAmenityMs.Include("tblExtranetUserM").Where(u => u.sChainName.Contains(param.sSearch)).AsQueryable();
                // //for searching
                // //var objTblUser = db.tblAmenityMs.Where(u => u.sAmenityName.Contains(param.sSearch)).AsQueryable();


                // //get Total Count for show total records
                // TotalCount = Convert.ToInt16(MyParam[4].Value); //user.Count();

                // //for sorting
                // //switch (param.iSortingCols)
                // //{
                // //    case 0:
                // //        objTblUser = param.sortDirection == "asc" ? objTblUser.OrderBy(u => u.sAmenityName) : objTblUser.OrderByDescending(u => u.sAmenityName);
                // //        break;

                // //}

                // ////implemented paging
                //// List<tblAmenityM> lstUser = objTblUser.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                // //convert bll entity object to db entity object
                // //foreach (var item in objTblUser)
                // //{
                // //    user.Add((eAmenityDisp)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eAmenityDisp()));
                // //}
                // return user;
            }
        }


        #endregion
    }
}