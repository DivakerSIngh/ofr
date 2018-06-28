using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblCityM
    {
        public int iCityId { get; set; }
        public int iCountryId { get; set; }
        public int iStateId { get; set; }
        public string sCity { get; set; }
        public string cStatus { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
    }
}