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
    
    public partial class tblChannelMgrBookingTx
    {
        public long iId { get; set; }
        public Nullable<long> iBookingId { get; set; }
        public Nullable<int> iPropId { get; set; }
        public Nullable<System.DateTime> dtOFRSend { get; set; }
        public Nullable<System.DateTime> dtOFRReceive { get; set; }
        public string cStatus { get; set; }
        public string sCMDescription { get; set; }
        public string sCMStatus { get; set; }
        public string sErrorType { get; set; }
        public string sErrorCode { get; set; }
    
        public virtual tblBookingTx tblBookingTx { get; set; }
        public virtual tblChannelMgrBookingTracker tblChannelMgrBookingTracker { get; set; }
        public virtual tblPropertyM tblPropertyM { get; set; }
    }
}
