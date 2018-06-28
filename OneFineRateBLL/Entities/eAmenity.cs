using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eAmenity
    {
        public int iAmenityId { get; set; }
        public string sAmenityName { get; set; }
        public string cStatus { get; set; }
        public DateTime dtActionDate { get; set; }
        public int iActionBy { get; set; }
    }
    public class eAmenityDisp
    {
        public int iAmenityId { get; set; }
        public string sAmenityName { get; set; }
        public string cStatus { get; set; }
        public DateTime dtActionDate { get; set; }
        public string ActionBy { get; set; }
    }

}