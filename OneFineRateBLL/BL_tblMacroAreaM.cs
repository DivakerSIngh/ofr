using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRate;
using OneFineRateBLL.Entities;
using System.Web.Mvc;

namespace OneFineRateBLL
{
    public class BL_tblMacroAreaM
    {
        #region Functions
        //Get Areas List
        public static List<SelectListItem> AreasList(int? areaid = null, int? cityid = null)
        {
            List<SelectListItem> areaList = new List<SelectListItem>();

            if (!cityid.HasValue || cityid.Value == 0)
            {
                areaList.Insert(0, new SelectListItem() { Text = "Select", Value = "0" });

                return areaList;
            }

            using (OneFineRateEntities dbContext = new OneFineRateEntities())
            {
                areaList = dbContext.tblMacroAreaMs.Where(s => s.iCityId == cityid).Select(s => new SelectListItem()
                {
                    Text = s.sArea.ToString(),
                    Value = s.iAreaId.ToString(),
                    Selected = areaid == null ? false : true
                }).OrderBy(s => s.Text).ToList();

                areaList.Insert(0, new SelectListItem() { Text = "Select", Value = "0" });
            }

            return areaList;
        }
        //Get All localities
        public static List<eMacroAreaM> MacroAreaList()
        {
            List<eMacroAreaM> data = new List<eMacroAreaM>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                data = (from u in db.tblMacroAreaMs.ToList()
                        select new eMacroAreaM
                        {
                            sArea = u.sArea,
                            iAreaId = u.iAreaId
                        }).AsQueryable().ToList();
            }
            return data;
        }
        //Add new record
        public static int AddRecord(eMacroAreaM eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    int dbobj = (from s in db.tblMacroAreaMs.Where(u => u.iCountryId == eobj.iCountryId && u.iStateId == eobj.iStateId && u.iCityId == eobj.iCityId && u.sArea == eobj.sArea.Trim())
                                 select new
                                 {
                                     s.iAreaId,
                                 }).Count();
                    if (dbobj > 0)
                    {
                        return retval = 2;
                    }


                    OneFineRate.tblMacroAreaM dbstate = (OneFineRate.tblMacroAreaM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblMacroAreaM());
                    db.tblMacroAreaMs.Add(dbstate);
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
                    var obj = db.tblMacroAreaMs.SingleOrDefault(u => u.iAreaId == id);
                    db.tblMacroAreaMs.Attach(obj);
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
        public static int UpdateRecord(eMacroAreaM eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    int dbobj = (from s in db.tblMacroAreaMs.Where(u => u.iCountryId == eobj.iCountryId && u.iStateId == eobj.iStateId && u.iCityId == eobj.iCityId && u.sArea == eobj.sArea.Trim() && u.iAreaId != eobj.iAreaId)
                                 select new
                                 {
                                     s.iAreaId,
                                 }).Count();
                    if (dbobj > 0)
                    {
                        return retval = 2;
                    }

                    OneFineRate.tblMacroAreaM obj = (OneFineRate.tblMacroAreaM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblMacroAreaM());
                    db.tblMacroAreaMs.Attach(obj);
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
                    objCountryList.AddRange(objCountry.Select(o => new CountryList
                    {
                        iCountryId = o.iCountryId,
                        CountryName = o.sCountry
                    }));

                    //foreach (var item in objCountry)
                    //{
                    //    CountryList objC = new CountryList();
                    //    objC.iCountryId = item.iCountryId;
                    //    objC.CountryName = item.sCountry;

                    //    objCountryList.Add(objC);
                    //}
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return objCountryList;
        }


        //Get Single Record
        public static eMacroAreaM GetSingleRecordById(int id)
        {
            eMacroAreaM eobj = new eMacroAreaM();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblMacroAreaMs.SingleOrDefault(u => u.iAreaId == id);
                if (dbobj != null)
                    eobj = (eMacroAreaM)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }

        public static List<SelectListItem> StateList(int? stateID = null, int? countryID = null)
        {
            List<SelectListItem> States = new List<SelectListItem>();
            using (OneFineRateEntities dbContext = new OneFineRateEntities())
            {
                States = dbContext.tblStateMs.AsEnumerable().Where(state => countryID == null || state.iCountryId == countryID).Select(state => new SelectListItem()
                {
                    Text = state.sState.ToString(),
                    Value = state.iStateId.ToString(),
                    Selected = stateID == null ? false : true
                }).OrderBy(state => state.Text).ToList();

                States.Insert(0, new SelectListItem() { Text = "Select", Value = "0" });
            }

            return States;
        }

        public static List<SelectListItem> CityList(int? cityid = null, int? stateID = null, int? countryID = null)
        {
            List<SelectListItem> cities = new List<SelectListItem>();
            using (OneFineRateEntities dbContext = new OneFineRateEntities())
            {
                cities = dbContext.tblCityMs.AsEnumerable().Where(city => (countryID == null || city.iCountryId == countryID) && (city.iStateId == stateID || stateID == null)).Select(city => new SelectListItem()
                {
                    Text = city.sCity.ToString(),
                    Value = city.iCityId.ToString(),
                    Selected = cityid == null ? false : true
                }).OrderBy(state => state.Text).ToList();

                cities.Insert(0, new SelectListItem() { Text = "Select", Value = "0" });
            }

            return cities;
        }

        //Get all records
        public static List<eMacroAreaM> GetAllRecordsById(int id)
        {
            List<eMacroAreaM> eobj = new List<eMacroAreaM>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                foreach (OneFineRate.tblMacroAreaM item in db.tblMacroAreaMs.Where(u => u.iCityId == id).AsQueryable().ToList())
                {
                    eobj.Add((eMacroAreaM)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eMacroAreaM()));
                }
            }
            return eobj;
        }

        //Get list of records
        public static List<eMacroAreaMList> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<eMacroAreaMList> macroarea = new List<eMacroAreaMList>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;

                //for searching

               // var objTblMacroArea1 = db.Database.SqlQuery<eMacroAreaMList>("uspGetMacroAreas");
                var objTblMacroArea = (from m in db.tblMacroAreaMs
                                       join c in db.tblCountryMs on m.iCountryId equals c.iCountryId
                                       join s in db.tblStateMs on m.iStateId equals s.iStateId
                                       join ct in db.tblCityMs on m.iCityId equals ct.iCityId
                                       join u in db.tblUserMs on m.iActionBy equals u.iUserId into lt                                       
                                       from T in lt.DefaultIfEmpty()
                                       join p in db.tblPropertyMs on m.iAreaId equals p.iAreaId into MP
                                       from Pr in MP.DefaultIfEmpty()
                                       select new
                                       {
                                           m.iAreaId,
                                           m.iCountryId,
                                           c.sCountry,
                                           m.iStateId,
                                           s.sState,
                                           m.iCityId,
                                           ct.sCity,
                                           m.sArea,
                                           cStatus = m.cStatus == "A" ? "Live" : "Disabled",
                                           m.dtActionDate,
                                           iActionBy = T.sFirstName + " " + T.sLastName,
                                           ipropId = MP.Count()
                                       }
                                   ).Distinct().Where(u => u.sCountry.Contains(param.sSearch) || u.sState.Contains(param.sSearch)
                                   || u.sCity.Contains(param.sSearch) || u.sArea.Contains(param.sSearch)).AsQueryable();

                //var objTblMacroArea = objTblMacroArea1.Where(u => u.sCountry.Contains(param.sSearch) || u.sState.Contains(param.sSearch)
                //                           || u.sCity.Contains(param.sSearch) || u.sArea.Contains(param.sSearch)).AsQueryable();

                //get Total Count for show total records
                TotalCount = objTblMacroArea.Count();

                //for sorting
                switch (param.iSortingCols)
                {
                    case 0:
                        objTblMacroArea = param.sortDirection == "asc" ? objTblMacroArea.OrderBy(u => u.sCountry) : objTblMacroArea.OrderByDescending(u => u.sCountry);
                        break;
                    case 1:
                        objTblMacroArea = param.sortDirection == "asc" ? objTblMacroArea.OrderBy(u => u.sState) : objTblMacroArea.OrderByDescending(u => u.sState);
                        break;
                    case 2:
                        objTblMacroArea = param.sortDirection == "asc" ? objTblMacroArea.OrderBy(u => u.sCity) : objTblMacroArea.OrderByDescending(u => u.sCity);
                        break;
                    case 3:
                        objTblMacroArea = param.sortDirection == "asc" ? objTblMacroArea.OrderBy(u => u.sArea) : objTblMacroArea.OrderByDescending(u => u.sArea);
                        break;
                    case 4:
                        objTblMacroArea = param.sortDirection == "asc" ? objTblMacroArea.OrderBy(u => u.cStatus) : objTblMacroArea.OrderByDescending(u => u.cStatus);
                        break;
                }


                ////implemented paging
                var data = objTblMacroArea.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

                //macroarea.AddRange(data.Select(item => (eMacroAreaMList)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eMacroAreaMList())));


                //convert bll entity object to db entity object
                foreach (var item in data)
                {
                   //item.cStatus =  item.cStatus == "A" ? "Live" : "Disabled";

                    macroarea.Add((eMacroAreaMList)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new eMacroAreaMList()));
                }
                
                return macroarea;
            }
        }

        #endregion
    }
}