//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OneFineRate
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblPropertyCancellationPolicyMap
    {
        public tblPropertyCancellationPolicyMap()
        {
            this.tblPropertyPromotionCancellationMaps = new HashSet<tblPropertyPromotionCancellationMap>();
        }
    
        public int iCancellationPolicyId { get; set; }
        public int iPropId { get; set; }
        public string sPolicyName { get; set; }
        public Nullable<System.DateTime> dtValidFrom { get; set; }
        public Nullable<System.DateTime> dtValidTo { get; set; }
        public Nullable<short> dHrsDays { get; set; }
        public string cHrsDays { get; set; }
        public Nullable<bool> bIsNonRefundable { get; set; }
        public Nullable<bool> bIsPercent { get; set; }
        public Nullable<decimal> dValue { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
        public string cStatus { get; set; }
    
        public virtual ICollection<tblPropertyPromotionCancellationMap> tblPropertyPromotionCancellationMaps { get; set; }
    }
}