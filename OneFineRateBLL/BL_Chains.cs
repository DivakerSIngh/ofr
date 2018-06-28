using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;

namespace OneFineRateBLL.Entities
{
    public class BL_Chains
    {

        #region functions
        //Add new record
        public static int AddRecord(eChains eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    int dbobj = (from s in db.tblChainMs.Where(u => u.sChainName==eobj.sChainName.Trim())
                                 select new
                                 {
                                     s.sChainName,
                                 }).Count();
                    if (dbobj > 0)
                    {
                        return retval = 2;
                    }

                    OneFineRate.tblChainM dbuser = (OneFineRate.tblChainM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblChainM());
                    db.tblChainMs.Add(dbuser);
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
                    var obj = db.tblChainMs.SingleOrDefault(u => u.iChainID == id);
                    db.tblChainMs.Attach(obj);
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
        public static int UpdateRecord(eChains eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    int dbobj = (from s in db.tblChainMs.Where(u => u.sChainName == eobj.sChainName.Trim() && u.iChainID != eobj.iChainID)
                                 select new
                                 {
                                     s.iChainID,
                                 }).Count();
                    if (dbobj > 0)
                    {
                        return retval = 2;
                    }

                    OneFineRate.tblChainM obj = (OneFineRate.tblChainM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblChainM());
                    db.tblChainMs.Attach(obj);
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
        public static eChains GetSingleRecordById(int id)
        {
            eChains eobj = new eChains();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblChainMs.SingleOrDefault(u => u.iChainID == id);
                if (dbobj != null)
                    eobj = (eChains)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }
        //Get all records
        public static List<eChains> GetAllChains()
        {
            List<eChains> eobj = new List<eChains>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                foreach (OneFineRate.tblChainM item in db.tblChainMs.Where(u => u.cStatus == "A").ToList())
                {
                    eobj.Add((eChains)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eChains()));
                }
            }
            return eobj;
        }

        //Get list of records
        public static List<eChains> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<eChains> user = new List<eChains>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;

                
                //var objTblUser = db.tblChainMs.Include("tblExtranetUserM").Where(u => u.sChainName.Contains(param.sSearch)).AsQueryable();
                //for searching
                var objTblUser = (from C in db.tblChainMs
                                  join U in db.tblUserMs on C.iActionBy equals U.iUserId
                                  select new
                                  {
                                      C.iChainID,
                                      C.sChainName,
                                      C.dtActionDate,
                                      cStatus = C.cStatus == "A" ? "Live" : "Disable",
                                      ActionBy = U.sFirstName + " " + U.sLastName
                                  }).Where(u => u.sChainName.Contains(param.sSearch)).AsQueryable();


                //get Total Count for show total records
                TotalCount = objTblUser.Count();

                //for sorting
                switch (param.iSortingCols)
                {
                    case 0:
                        objTblUser = param.sortDirection == "asc" ? objTblUser.OrderBy(u => u.sChainName) : objTblUser.OrderByDescending(u => u.sChainName);
                        break;
                    case 1:
                        objTblUser = param.sortDirection == "asc" ? objTblUser.OrderBy(u => u.cStatus) : objTblUser.OrderByDescending(u => u.cStatus);
                        break;
                }

                ////implemented paging
                var lstUser = objTblUser.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                user.AddRange(lstUser.Select(item => (eChains)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eChains())));
                
                return user;
            }
        }


        #endregion
    }
}