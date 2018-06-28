using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;

namespace OneFineRateBLL
{
    public class BL_tblPropertyDiningMap
    {

        //Get Single Record
        public static etblPropertyDiningMap GetSingleRecord(int id, string Name)
        {
            etblPropertyDiningMap eobj = new etblPropertyDiningMap();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblPropertyDiningMaps.SingleOrDefault(u => u.iPropId == id && u.sRestaurantName == Name);
                if (dbobj != null)
                    eobj = (etblPropertyDiningMap)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }
        //Add new record
        public static int AddRecord(etblPropertyDiningMap eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var checkExistsRestaurantName = db.tblPropertyDiningMaps.Any(
                                            p => p.iPropId == eobj.iPropId
                                                 && p.sRestaurantName == eobj.sRestaurantName);

                    if (checkExistsRestaurantName == false)
                    {
                        OneFineRate.tblPropertyDiningMap dbstate = (OneFineRate.tblPropertyDiningMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyDiningMap());
                        db.tblPropertyDiningMaps.Add(dbstate);
                        db.SaveChanges();
                        retval = 1;
                    }
                    else {
                        retval = 2;
                    }

                    
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retval;
        }

        //Update a record
        public static int UpdateRecord(etblPropertyDiningMap eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblPropertyDiningMap obj = (OneFineRate.tblPropertyDiningMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyDiningMap());
                    db.tblPropertyDiningMaps.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    retval = 1;

                    //var checkExistsRestaurantName = db.tblPropertyDiningMaps.Any(
                    //                      p => p.iPropId == eobj.iPropId
                    //                          && p.iRestaurantID != eobj.iRestaurantID
                    //                           && p.sRestaurantName == eobj.sRestaurantName);

                    //if (checkExistsRestaurantName == false)
                    //{
                       
                    //}
                    //else
                    //{
                    //    retval = 2;
                    //}
                    
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retval;
        }
        //Get list of records
        public static List<etblPropertyDiningMap> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param,int propid ,out int TotalCount)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<etblPropertyDiningMap> user = new List<etblPropertyDiningMap>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;

                //for searching
                var objTblUser = db.tblPropertyDiningMaps.Where(u => u.sRestaurantName.Contains(param.sSearch)&& u.iPropId==propid).AsQueryable();

                //get Total Count for show total records
                TotalCount = objTblUser.Count();

                //for sorting
                switch (param.iSortingCols)
                {
                    case 0:
                        objTblUser = param.sortDirection == "asc" ? objTblUser.OrderBy(u => u.cType) : objTblUser.OrderByDescending(u => u.cType);
                        break;
                    case 1:
                        objTblUser = param.sortDirection == "asc" ? objTblUser.OrderBy(u => u.sRestaurantName) : objTblUser.OrderByDescending(u => u.sRestaurantName);
                        break;
                    case 2:
                        objTblUser = param.sortDirection == "asc" ? objTblUser.OrderBy(u => u.sDescription) : objTblUser.OrderByDescending(u => u.sDescription);
                        break;
                    case 3:
                        objTblUser = param.sortDirection == "asc" ? objTblUser.OrderBy(u => u.b24hours) : objTblUser.OrderByDescending(u => u.b24hours);
                        break;

                }

                ////implemented paging
                List<tblPropertyDiningMap> lstUser = objTblUser.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                //convert bll entity object to db entity object
                foreach (var item in lstUser)
                {
                    user.Add((etblPropertyDiningMap)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblPropertyDiningMap()));
                }
                return user;
            }
        }
    }
}