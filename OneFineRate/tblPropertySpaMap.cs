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
    
    public partial class tblPropertySpaMap
    {
        public int iPropId { get; set; }
        public string sSpaName { get; set; }
        public string sSpaDesc { get; set; }
        public bool bHotsprings { get; set; }
        public bool bSauna { get; set; }
        public bool bMudbath { get; set; }
        public bool bSpaTub { get; set; }
        public bool bAdvancedBooking { get; set; }
        public bool bSteamRoom { get; set; }
        public bool b24hours { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
    }
}