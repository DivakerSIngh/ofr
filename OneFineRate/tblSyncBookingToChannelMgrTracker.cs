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
    
    public partial class tblSyncBookingToChannelMgrTracker
    {
        public long iBookingId { get; set; }
        public Nullable<System.DateTime> dtSyncToChannelMgr { get; set; }
        public string cSyncStatus { get; set; }
        public string sXMLString { get; set; }
        public string sError { get; set; }
    
        public virtual tblBookingTx tblBookingTx { get; set; }
    }
}