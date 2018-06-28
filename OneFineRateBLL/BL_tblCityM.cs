using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OneFineRateBLL.Entities;
using OneFineRate;
using System.Web.Mvc;
using System.Runtime.Caching;

namespace OneFineRateBLL
{
    public class BL_tblCityM
    {
        //Get Cities List
        public static List<SelectListItem> CityList(int? cityid = null, int? stateID = null, int? countryID = null)
        {
            using (OneFineRateEntities dbContext = new OneFineRateEntities())
            {
                var cities = new List<SelectListItem>();

                if (!countryID.HasValue || !stateID.HasValue || countryID.Value == 0 || stateID.Value == 0)
                {
                    cities.Insert(0, new SelectListItem() { Text = "Select", Value = "0" });

                    return cities;
                }

                cities = dbContext.tblCityMs.Where(x => x.iCountryId == countryID && x.iStateId == stateID).OrderBy(x=>x.sCity)
                    .Select(y => new SelectListItem()
                    {
                        Text = y.sCity,
                        Value = y.iCityId.ToString(),
                        Selected = cityid == null ? false : true
                    }).ToList();

                cities.Insert(0, new SelectListItem() { Text = "Select", Value = "0" });

                return cities;
            }
        }
        //Get all records
        public static List<etblCityM> GetAllRecordsById(int id)
        {
            List<etblCityM> eobj = new List<etblCityM>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                foreach (OneFineRate.tblCityM item in db.tblCityMs.Where(u => u.iStateId == id && u.cStatus == "A").ToList())
                {
                    eobj.Add((etblCityM)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblCityM()));
                }
            }
            return eobj;
        }
    }



    public class InMemoryCache
    {
        public T GetOrSet<T>(string cacheKey, Func<T> getItemCallback) where T : class
        {
            T item = MemoryCache.Default.Get(cacheKey) as T;
            if (item == null)
            {
                item = getItemCallback();
                MemoryCache.Default.Add(cacheKey, item, DateTime.Now.AddDays(10));
            }
            return item;
        }
    }
}