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
    
    public partial class tblBookedDayWiseTaxAmountDetails_Mod_Log
    {
        public long iID { get; set; }
        public int iBookingID { get; set; }
        public int iBookingDetailsID { get; set; }
        public string iRoomId { get; set; }
        public System.DateTime dtStayDay { get; set; }
        public Nullable<int> iPropId { get; set; }
        public Nullable<decimal> dAmount { get; set; }
        public Nullable<decimal> dTaxPerc_Old { get; set; }
        public Nullable<decimal> dTaxVal_Old { get; set; }
        public Nullable<decimal> dTax_Old { get; set; }
        public Nullable<decimal> dTaxPerc_New { get; set; }
        public Nullable<decimal> dTaxVal_New { get; set; }
        public Nullable<decimal> dTax_New { get; set; }
        public Nullable<bool> bIsEmailSent { get; set; }
        public Nullable<System.DateTime> dtEmailSentOn { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
    }
}
