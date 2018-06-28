using OneFineRate;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateBLL
{
    public class BL_Country
    {
        #region functions
        //Add new record
        public static int AddRecord(eCountries eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblCountryM dbuser = (OneFineRate.tblCountryM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblCountryM());
                    db.tblCountryMs.Add(dbuser);
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
                    var obj = db.tblCountryMs.SingleOrDefault(u => u.iCountryId == id);
                    db.tblCountryMs.Attach(obj);
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
        public static int UpdateRecord(eCountries eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblCountryM obj = (OneFineRate.tblCountryM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblCountryM());
                    db.tblCountryMs.Attach(obj);
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
        public static eCountries GetSingleRecordById(int id)
        {
            eCountries eobj = new eCountries();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblCountryMs.SingleOrDefault(u => u.iCountryId == id);
                if (dbobj != null)
                    eobj = (eCountries)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }

        //Get Single Record

        public static List<eStates> GetStatesByCountryId(int countryId)
        {
            var stateList = new List<eStates>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblStateMs.Where(u => u.iCountryId == countryId && u.cStatus == "A").ToList();
                if (dbobj != null)
                {
                    foreach (var item in dbobj)
                    {
                        stateList.Add(new eStates { iStateId = item.iStateId, sState = item.sState });
                    }
                }
            }
            return stateList;
        }

        public static List<eStates> GetStatesByStateIdForDropdown(int stateId)
        {
            var stateList = new List<eStates>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblStateMs.Where(u => u.iStateId == stateId).ToList();
                if (dbobj != null)
                {
                    foreach (var item in dbobj)
                    {
                        stateList.Add(new eStates { iStateId = item.iStateId, sState = item.sState });
                    }
                }
            }
            return stateList;
        }

        //Get Single Record
        public static List<eCities> GetCitiesByStateId(int stateId)
        {
            var stateList = new List<eCities>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblCityMs.Where(u => u.iStateId == stateId).ToList();
                if (dbobj != null)
                {
                    foreach (var item in dbobj)
                    {
                        stateList.Add(new eCities { iCityId = item.iCityId, sCity = item.sCity });
                    }
                }
            }
            return stateList;
        }

        //Get all records

        public static List<eCountries> GetAllRecords()
        {
            List<eCountries> eobj = new List<eCountries>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                foreach (OneFineRate.tblCountryM item in db.tblCountryMs.Where(u => u.cStatus == "A").ToList())
                {
                    eobj.Add((eCountries)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eCountries()));
                }
            }
            return eobj;
        }

        //Get list of records
        public static List<eCountries> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<eCountries> user = new List<eCountries>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;

                //for searching
                var objTblUser = db.tblCountryMs.Where(u => u.sCountry.Contains(param.sSearch)).AsQueryable();


                //get Total Count for show total records
                TotalCount = objTblUser.Count();

                //for sorting
                switch (param.iSortingCols)
                {
                    case 0:
                        objTblUser = param.sortDirection == "asc" ? objTblUser.OrderBy(u => u.sCountry) : objTblUser.OrderByDescending(u => u.sCountry);
                        break;

                }


                ////implemented paging
                List<tblCountryM> lstUser = objTblUser.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                //convert bll entity object to db entity object
                foreach (var item in lstUser)
                {
                    user.Add((eCountries)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eCountries()));
                }
                return user;
            }
        }

        //Get all records

        public static List<eCountryCodePhone> GetAllCountryPhoneCodes()
        {
            List<eCountryCodePhone> eobj = new List<eCountryCodePhone>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var countryPhoneCodeList = db.tblCountryCodePhoneMs.ToList();

                foreach (var item in countryPhoneCodeList)
                {
                    eobj.Add((eCountryCodePhone)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eCountryCodePhone()));
                }
            }
            return eobj;
        }


        #endregion
    }
}