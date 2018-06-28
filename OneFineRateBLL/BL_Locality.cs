using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;
using System.Web.Mvc;
using System.Data.SqlClient;

namespace OneFineRateBLL
{
    public class BL_Locality
    {
        #region Functions
        //Get All localities
        //public static List<etblLocality> LocalityList(int id)
        //{
        //    List<etblLocality> data = new List<etblLocality>();
        //    using (OneFineRateEntities db = new OneFineRateEntities())
        //    {
        //        data = (from u in db.tblLocalityMs.Where(u => u.iAreaId == id)
        //                             select new etblLocality
        //                             {
        //                                 sLocality = u.sLocality,
        //                                 iLocalityId = u.iLocalityId
        //                             }).AsQueryable().ToList();
        //    }
        //    return data;
        //}
        //Get Areas List
        public static List<SelectListItem> LocalityList(int? localityid = null, int? areaid = null)
        {
            List<SelectListItem> localityList = new List<SelectListItem>();

            if (!areaid.HasValue || areaid.Value == 0)
            {
                localityList.Insert(0, new SelectListItem() { Text = "Select", Value = "0" });

                return localityList;
            }

            using (OneFineRateEntities dbContext = new OneFineRateEntities())
            {
                localityList = dbContext.tblLocalityMs.Where(s => s.iAreaId == areaid).Select(s => new SelectListItem()
                {
                    Text = s.sLocality,
                    Value = s.iLocalityId.ToString(),
                    Selected = localityid == null ? false : true
                }).OrderBy(s => s.Text).ToList();

                localityList.Insert(0, new SelectListItem() { Text = "Select", Value = "0" });
            }

            return localityList;
        }

        //Get all localities
        public static List<etblLocality> LocalityList(int id)
        {
            List<etblLocality> eobj = new List<etblLocality>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                foreach (OneFineRate.tblLocalityM item in db.tblLocalityMs.Where(u => u.iAreaId == id && u.cStatus == "A").ToList())
                {
                    eobj.Add((etblLocality)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblLocality()));
                }
            }
            return eobj;
        }
        //Add new record
        public static List<PNames> MacroLocalitiesAjaxCall(string txt ,int city)
        {
            //Dictionary<string, string> data = new Dictionary<string, string>();
            List<PNames> data = new List<PNames>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var areaQuery = (from u in db.tblMacroAreaMs.ToList()
                                     select new PNames
                                     {
                                         Name = u.sArea,
                                         Id = u.iAreaId + "|A",
                                         iCityId = u.iCityId
                                     }).Where(u => u.iCityId == city && u.Name.ToLower().Contains(txt.ToLower())).ToList();
                    var localityQuery = (from u in db.tblLocalityMs.ToList()
                                         select new PNames
                                         {
                                             Name = u.sLocality,
                                             Id = u.iLocalityId + "|L",
                                             iCityId = Convert.ToInt32(u.iCityId)
                                         }).Where(u => u.iCityId == city && u.Name.ToLower().Contains(txt.ToLower())).ToList();

                    data.AddRange(areaQuery.Union(localityQuery));
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return data;
        }
      
        public static List<PNames> MacroLocalities()
        {
            //Dictionary<string, string> data = new Dictionary<string, string>();
            List<PNames> data = new List<PNames>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var areaQuery = (from u in db.tblMacroAreaMs.ToList()
                                     select new PNames
                                              {
                                                  Name = u.sArea,
                                                  Id = u.iAreaId + "|A"
                                              }).ToList();
                    var localityQuery = (from u in db.tblLocalityMs.ToList()
                                         select new PNames
                                         {
                                             Name = u.sLocality,
                                             Id = u.iLocalityId + "|L"
                                         }).ToList();

                    data.AddRange(areaQuery.Union(localityQuery));
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return data;
        }
        public static int AddRecord(etblLocality eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    int dbobj = (from s in db.tblLocalityMs.Where(u => u.iCountryId == eobj.iCountryId && u.iStateId == eobj.iStateId && u.iCityId == eobj.iCityId && u.iAreaId == eobj.iAreaId && u.sLocality == eobj.sLocality.Trim())
                                 select new
                                 {
                                     s.iLocalityId,
                                 }).Count();
                    if (dbobj > 0)
                    {
                        return retval = 2;
                    }

                    OneFineRate.tblLocalityM dbstate = (OneFineRate.tblLocalityM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblLocalityM());
                    db.tblLocalityMs.Add(dbstate);
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
                    var obj = db.tblLocalityMs.SingleOrDefault(u => u.iLocalityId == id);
                    db.tblLocalityMs.Attach(obj);
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
        public static int UpdateRecord(etblLocality eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    int dbobj = (from s in db.tblLocalityMs.Where(u => u.iCountryId == eobj.iCountryId && u.iStateId == eobj.iStateId && u.iCityId == eobj.iCityId && u.iAreaId == eobj.iAreaId && u.sLocality == eobj.sLocality.Trim() && u.iLocalityId != eobj.iLocalityId)
                                 select new
                                 {
                                     s.iLocalityId,
                                 }).Count();
                    if (dbobj > 0)
                    {
                        return retval = 2;
                    }

                    OneFineRate.tblLocalityM obj = (OneFineRate.tblLocalityM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblLocalityM());
                    db.tblLocalityMs.Attach(obj);
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
                    objCountryList.AddRange(objCountry.Select(item => new CountryList()
                    {
                        iCountryId = item.iCountryId,
                        CountryName = item.sCountry
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
        public static etblLocality GetSingleRecordById(int id)
        {
            etblLocality eobj = new etblLocality();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblLocalityMs.SingleOrDefault(u => u.iLocalityId == id);
                if (dbobj != null)
                    eobj = (etblLocality)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }

        //Get all records
        public static List<etblLocality> GetAllRecordsById(int id)
        {
            List<etblLocality> eobj = new List<etblLocality>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                foreach (OneFineRate.tblLocalityM item in db.tblLocalityMs.Where(u => u.iAreaId == id).AsQueryable().ToList())
                {
                    eobj.Add((etblLocality)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblLocality()));
                }
            }
            return eobj;
        }

        //Get list of records
        public static List<etblLocalityList> getRecordForSearch(OneFineRateAppUtil.jQueryDataTableParamModel param, out int TotalCount)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                List<etblLocalityList> locality = new List<etblLocalityList>();
                param.sSearch = param.sSearch == null ? "" : param.sSearch;

                //for searching
                var objTblMacroArea = (from l in db.tblLocalityMs
                                       join m in db.tblMacroAreaMs on l.iAreaId equals m.iAreaId
                                       join c in db.tblCountryMs on l.iCountryId equals c.iCountryId
                                       join s in db.tblStateMs on l.iStateId equals s.iStateId
                                       join ct in db.tblCityMs on l.iCityId equals ct.iCityId
                                       join u in db.tblUserMs on l.iActionBy equals u.iUserId into lt
                                       from T in lt.DefaultIfEmpty()
                                       join c1 in db.tblPropertyMs on l.iLocalityId equals c1.iLocalityId into S2
                                       from a in S2.DefaultIfEmpty()
                                       select new
                                       {
                                           l.iLocalityId,
                                           l.iCountryId,
                                           c.sCountry,
                                           l.iStateId,
                                           s.sState,
                                           l.iCityId,
                                           ct.sCity,
                                           l.iAreaId,
                                           m.sArea,
                                           l.sLocality,
                                           cStatus = l.cStatus == "A" ? "Live" : "Disable",
                                           l.dtActionDate,
                                           iActionBy = T.sFirstName + " " + T.sLastName,
                                           iPropId = S2.Count()
                                       }
                                   ).Distinct().Where(u => u.sCountry.Contains(param.sSearch) || u.sState.Contains(param.sSearch)
                                   || u.sCity.Contains(param.sSearch) || u.sArea.Contains(param.sSearch) || u.sLocality.Contains(param.sSearch)).AsQueryable();

            
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
                        objTblMacroArea = param.sortDirection == "asc" ? objTblMacroArea.OrderBy(u => u.sLocality) : objTblMacroArea.OrderByDescending(u => u.sLocality);
                        break;
                    case 5:
                        objTblMacroArea = param.sortDirection == "asc" ? objTblMacroArea.OrderBy(u => u.cStatus) : objTblMacroArea.OrderByDescending(u => u.cStatus);
                        break;
                }


                ////implemented paging
                var data = objTblMacroArea.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();


                //convert bll entity object to db entity object
                foreach (var item in data)
                {
                    locality.Add((etblLocalityList)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblLocalityList()));
                }
                return locality;
            }
        }

        public static List<PNames> GetAllLocality(int propId, string characters)
        {
            List<PNames> objMapping = new List<PNames>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {

                objMapping = (from t1 in db.tblPropertyMs
                              join t2 in db.tblLocalityMs on t1.iLocalityId equals t2.iLocalityId
                              join t3 in db.tblMacroAreaMs on t1.iAreaId equals t3.iAreaId
                              join t4 in db.tblCityMs on t1.iCityId equals t4.iCityId
                              join t5 in db.tblStateMs on t1.iStateId equals t5.iStateId
                              select new PNames()
                              {
                                  Id = t2.iLocalityId.ToString(),
                                  Name = t2.sLocality.Trim() + " , " + t3.sArea.Trim() + " , " + t4.sCity.Trim() + " , " + t5.sState.Trim()
                              }).Distinct().Where(u => u.Name.Contains(characters)).AsQueryable().ToList();
                return objMapping;
            }
        }

        public static List<PropDetailsForHotelBooking> GetAllProperty(int LocalityId, DateTime dtCheckIn, DateTime dtCheckOut)
        {
            List<PropDetailsForHotelBooking> objMapping = new List<PropDetailsForHotelBooking>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                SqlParameter[] MyParam = new SqlParameter[3];
                MyParam[0] = new SqlParameter("@LocalityId", LocalityId);
                MyParam[1] = new SqlParameter("@dtCheckIn", dtCheckIn);
                MyParam[2] = new SqlParameter("@dtCheckOut", dtCheckOut);
                objMapping = db.Database.SqlQuery<PropDetailsForHotelBooking>("uspGetAllPropertyForBooking @LocalityId,@dtCheckIn,@dtCheckOut", MyParam).ToList();
                // objMapping = objMapping.Where(u => u.iLocalityId == LocalityId && u.propStatus == "A").AsQueryable().ToList();
                return objMapping;
            }
        }

        #endregion
    }
}