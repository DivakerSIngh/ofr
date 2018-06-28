using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblPropertyLeadTimeBiddingMap
    {
        public int iPropId { get; set; }
        public System.DateTime dtEffectiveDate { get; set; }
        public short iLeadDays { get; set; }
        public Nullable<decimal> dLeadDiscount { get; set; }
        public Nullable<int> iAmenityId1 { get; set; }
        public Nullable<short> iApplicabilityId1 { get; set; }
        public Nullable<int> iAmenityId2 { get; set; }
        public Nullable<short> iApplicabilityId2 { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
        public Nullable<bool> bIsClosed { get; set; }
        public Nullable<bool> bCTA { get; set; }
        public Nullable<bool> bCTD { get; set; }

        public short ifrom { get; set; }
        public short iTo { get; set; }
        public string JSONData { get; set; }
        public string JSONDataOther { get; set; }
        public string SelectedDiscounts { get; set; }
        public string EffectiveDate { get; set; }
        public bool IsPublic { get; set; }
    }
    public class PropertyBiddingMap
    {
      
        public Nullable<decimal> dLeadDiscount { get; set; }
        public Nullable<int> iAmenityId1 { get; set; }
        public Nullable<short> iApplicabilityId1 { get; set; }
        public Nullable<int> iAmenityId2 { get; set; }
        public Nullable<short> iApplicabilityId2 { get; set; }
        public Nullable<bool> bIsClosed { get; set; }
        public Nullable<bool> bCTA { get; set; }
        public Nullable<bool> bCTD { get; set; }
        public short ifrom { get; set; }
        public short iTo { get; set; }
    }

    public class PropertyBiddingMapForOverview
    {
        public List<PropertyBiddingMap> Self { get; set; }
        public List<PropertyBiddingMap> Other { get; set; }
        public bool IsPublic { get; set; }
    }
}