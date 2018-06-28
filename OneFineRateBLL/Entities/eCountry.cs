using OneFineRate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eCountries 
    {

        public int iCountryId { get; set; }
        public string sCountry { get; set; }
        public string cStatus { get; set; }
        public DateTime dtActionDate { get; set; }
        public int iActionBy { get; set; }
    }

    public class eStates
    {
        public int iStateId { get; set; }
        public string sState { get; set; }
    }

    public class eCities
    {
        public int iCityId { get; set; }
        public string sCity { get; set; }
    }
}