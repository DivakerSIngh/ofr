using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblStateM
    {
        public int iStateId { get; set; }
        public int iCountryId { get; set; }
        public string sCountry { get; set; }
        public string sState { get; set; }
        public string cStatus { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
    }
    public class CountryList
    {
        public int iCountryId { get; set; }
        public string CountryName { get; set; }
    }
}