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
    
    public partial class tblBookingTrakerTx
    {
        public long iTrakerID { get; set; }
        public long iBookingId { get; set; }
        public string BookingStatus { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
    }
}
