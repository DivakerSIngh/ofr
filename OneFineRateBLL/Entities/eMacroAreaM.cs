using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace OneFineRateBLL.Entities
{
    public class eMacroAreaM
    {
        public int iAreaId { get; set; }
        public int iCountryId { get; set; }
        public string sCountry { get; set; }
        public int iStateId { get; set; }
        public string sState { get; set; }
        public int iCityId { get; set; }
        public string sCity { get; set; }
        public string sArea { get; set; }
        public string cStatus { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public int iActionBy { get; set; }
        public List<SelectListItem> StateList { get; set; }
        public List<SelectListItem> CityList { get; set; }

    }

    public class eMacroAreaMList
    {
        public int iAreaId { get; set; }
        public int iCountryId { get; set; }
        public string sCountry { get; set; }
        public int iStateId { get; set; }
        public string sState { get; set; }
        public int iCityId { get; set; }
        public string sCity { get; set; }
        public string sArea { get; set; }
        public string cStatus { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public string iActionBy { get; set; }
        public List<SelectListItem> StateList { get; set; }
        public List<SelectListItem> CityList { get; set; }
         public int ipropId { get; set; }
        

    }
}