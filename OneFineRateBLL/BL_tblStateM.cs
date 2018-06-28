using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;
using System.Web.Mvc;

namespace OneFineRateBLL
{
    public class BL_tblStateM
    {
        #region Functions

        public static List<SelectListItem> StateList(int? stateID = null, int? countryID = null)
        {
            List<SelectListItem> states = new List<SelectListItem>();

            if (!countryID.HasValue || countryID.Value == 0)
            {
                states.Insert(0, new SelectListItem() { Text = "Select", Value = "0" });

                return states;
            }

            using (OneFineRateEntities dbContext = new OneFineRateEntities())
            {
                states = dbContext.tblStateMs.Where(state => state.iCountryId == countryID)
                    .Select(state => new SelectListItem()
                    {
                        Text = state.sState,
                        Value = state.iStateId.ToString(),
                        Selected = stateID == null ? false : true
                    }).OrderBy(state => state.Text).ToList();

                states.Insert(0, new SelectListItem() { Text = "Select", Value = "0" });
            }

            return states;
        }

        //Add new record
        public static int AddRecord(etblStateM eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblStateM dbstate = (OneFineRate.tblStateM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblStateM());
                    db.tblStateMs.Add(dbstate);
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
                    var obj = db.tblStateMs.SingleOrDefault(u => u.iStateId == id);
                    db.tblStateMs.Attach(obj);
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
        public static int UpdateRecord(etblStateM eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblStateM obj = (OneFineRate.tblStateM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblStateM());
                    db.tblStateMs.Attach(obj);
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

        //Bind Country Dropdown
        public static List<CountryList> CountryList()
        {
            List<CountryList> objCountryList = new List<CountryList>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var objCountry = db.tblCountryMs.AsQueryable();
                    foreach (var item in objCountry)
                    {
                        CountryList objC = new CountryList();
                        objC.iCountryId = item.iCountryId;
                        objC.CountryName = item.sCountry;

                        objCountryList.Add(objC);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return objCountryList;
        }

        //Get Single Record
        public static etblStateM GetSingleRecordById(int id)
        {
            etblStateM eobj = new etblStateM();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblStateMs.SingleOrDefault(u => u.iStateId == id);
                if (dbobj != null)
                    eobj = (etblStateM)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;
        }


        public static etblStateM GetSingleRecordByGST(string gstFirstTwoDigit)
        {
            etblStateM eobj = new etblStateM();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblStateMs.FirstOrDefault(u => u.sStateCode == gstFirstTwoDigit);
                if (dbobj != null)
                    eobj = (etblStateM)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }


        //Get all records
        public static List<etblStateM> GetAllRecordsById(int id)
        {
            List<etblStateM> eobj = new List<etblStateM>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                foreach (OneFineRate.tblStateM item in db.tblStateMs.Where(u => u.iCountryId == id && u.cStatus == "A").ToList())
                {
                    eobj.Add((etblStateM)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblStateM()));
                }
            }
            return eobj;
        }

        //Get list of records
        public static List<etblStateM> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<etblStateM> state = new List<etblStateM>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;

                //for searching
                var objTblState = (from s in db.tblStateMs
                                   join c in db.tblCountryMs on s.iCountryId equals c.iCountryId
                                   select new
                                   {
                                       s.iStateId,
                                       s.iCountryId,
                                       c.sCountry,
                                       s.sState,
                                       cStatus = s.cStatus == "A" ? "Active" : "InActive",
                                       s.dtActionDate,
                                       s.iActionBy
                                   }).Where(u => u.sState.Contains(param.sSearch) || u.sCountry.Contains(param.sSearch)).AsQueryable();

                //get Total Count for show total records
                TotalCount = objTblState.Count();

                //for sorting
                switch (param.iSortingCols)
                {
                    case 0:
                        objTblState = param.sortDirection == "asc" ? objTblState.OrderBy(u => u.sCountry) : objTblState.OrderByDescending(u => u.sCountry);
                        break;
                    case 1:
                        objTblState = param.sortDirection == "asc" ? objTblState.OrderBy(u => u.sState) : objTblState.OrderByDescending(u => u.sState);
                        break;
                    default:
                        objTblState = param.sortDirection == "asc" ? objTblState.OrderBy(u => u.sState) : objTblState.OrderByDescending(u => u.sState);
                        break;
                }


                ////implemented paging
                var data = objTblState.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();


                //convert bll entity object to db entity object
                foreach (var item in data)
                {
                    state.Add((etblStateM)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblStateM()));
                }
                return state;
            }
        }

        #endregion

    }
}