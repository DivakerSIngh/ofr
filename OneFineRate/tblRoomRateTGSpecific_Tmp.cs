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
    
    public partial class tblRoomRateTGSpecific_Tmp
    {
        public long iId { get; set; }
        public string sSessionId { get; set; }
        public string sHotelCode { get; set; }
        public string sRatePlanCode { get; set; }
        public string sRoomTypeCode { get; set; }
        public Nullable<System.DateTime> dEffectiveDate { get; set; }
        public string tEffectiveTime { get; set; }
        public Nullable<decimal> dAmountBeforeTax { get; set; }
        public Nullable<decimal> dTaxAmount { get; set; }
        public string sAdditionalGuestAmountRPH { get; set; }
        public string sAdditionalGuestAmountAgeQualificationCode { get; set; }
        public Nullable<decimal> dAdditionalGuestAmountTax { get; set; }
        public string sBookable { get; set; }
        public string iBaseChildOccupancy { get; set; }
        public string iBaseAdultOccupancy { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
    }
}
