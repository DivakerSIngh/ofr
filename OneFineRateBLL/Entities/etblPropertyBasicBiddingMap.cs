using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblPropertyBasicBiddingMap
    {
        public int iPropId { get; set; }
        public System.DateTime dtEffectiveDate { get; set; }
        public Nullable<bool> bIsClosed { get; set; }
        public Nullable<bool> bCTA { get; set; }
        public Nullable<bool> bCTD { get; set; }
        public Nullable<decimal> dBasicDiscount { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
        public Nullable<bool> bIsClosedroom { get; set; }
        public Nullable<bool> bCTAroom { get; set; }
        public Nullable<bool> bCTDroom { get; set; }
        public Nullable<bool> bIsClosedlead { get; set; }
        public Nullable<bool> bCTAlead { get; set; }
        public Nullable<bool> bCTDlead { get; set; }
        public Nullable<bool> bIsClosedlos { get; set; }
        public Nullable<bool> bCTAlos { get; set; }
        public Nullable<bool> bCTDlos { get; set; }
        public string EffectiveDate { get; set; }
        public Nullable<bool> bIsClosedweek { get; set; }
        public Nullable<bool> bCTAweek { get; set; }
        public Nullable<bool> bCTDweek { get; set; }    
    }

    public class etblPropertyBasicBiddingMapForOverview
    {
        public etblPropertyBasicBiddingMap Self { get; set; }
        public etblPropertyBasicBiddingMap Other { get; set; }
        public bool IsPublic { get; set; }
    }
}