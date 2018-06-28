using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eRatePlan
    {

        public short iRatePlanId { get; set; }
        public string sRatePlan { get; set; }
        public string cStatus { get; set; }
        public DateTime dtActionDate { get; set; }
        public int iActionBy { get; set; }

    }
    public class eRatePlanDisp
    {
        public short iRatePlanId { get; set; }
        public string sRatePlan { get; set; }
        public string cStatus { get; set; }
        public DateTime dtActionDate { get; set; }
        public string ActionBy { get; set; }
    }
}