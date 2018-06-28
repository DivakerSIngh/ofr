using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblPropertyDiningMap
    {
        public long iRestaurantID { get; set; }
        public int iPropId { get; set; }
        public string sRestaurantName { get; set; }
        public string cType { get; set; }
        public string sDescription { get; set; }
        public bool bBreakfast { get; set; }
        public bool bLunch { get; set; }
        public bool bDinner { get; set; }
        public string sTimingFromHH { get; set; }
        public string sTimingFromMM { get; set; }
        public string sTimingToHH { get; set; }
        public string sTimingToMM { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
        public bool bActive { get; set; }
        public string Mode { get; set; }
        public string sTimingFromHHOld { get; set; }
        public string sTimingToHHOld { get; set; }
        public Nullable<bool> b24hours { get; set; }
    }
}