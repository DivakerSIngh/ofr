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
    
    public partial class tblPropertyRoomsBiddingMap
    {
        public int iPropId { get; set; }
        public System.DateTime dtEffectiveDate { get; set; }
        public short iRooms { get; set; }
        public Nullable<decimal> dLeadDiscount { get; set; }
        public Nullable<int> iAmenityId1 { get; set; }
        public Nullable<short> iApplicabilityId1 { get; set; }
        public Nullable<int> iAmenityId2 { get; set; }
        public Nullable<short> iApplicabilityId2 { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
    
        public virtual tblAmenityM tblAmenityM { get; set; }
        public virtual tblAmenityM tblAmenityM1 { get; set; }
        public virtual tblApplicabilityM tblApplicabilityM { get; set; }
        public virtual tblApplicabilityM tblApplicabilityM1 { get; set; }
        public virtual tblPropertyM tblPropertyM { get; set; }
        public virtual tblUserM tblUserM { get; set; }
    }
}