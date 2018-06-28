using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblPropertyWeekendBiddingMap
    {
        public int iPropId { get; set; }
        public System.DateTime dtEffectiveDate { get; set; }
        public Nullable<bool> bIsClosedweek { get; set; }
        public Nullable<bool> bCTAweek { get; set; }
        public Nullable<bool> bCTDweek { get; set; }
        public Nullable<bool> bWeekend { get; set; }
        public Nullable<decimal> dWeekendDiscount { get; set; }
        public Nullable<int> iAmenityId1 { get; set; }
        public Nullable<short> iApplicabilityId1 { get; set; }
        public Nullable<int> iAmenityId2 { get; set; }
        public Nullable<short> iApplicabilityId2 { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
        public string EffectiveDate { get; set; }
    }

    public class etblPropertyWeekendBiddingMapForOverview
    {
        public etblPropertyWeekendBiddingMap Self { get; set; }
        public etblPropertyWeekendBiddingMap Other { get; set; }
        public bool IsPublic { get; set; }
    }
}