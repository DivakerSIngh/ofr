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
    
    public partial class tblPropertyDiningMap
    {
        public long iRestaurantID { get; set; }
        public int iPropId { get; set; }
        public string sRestaurantName { get; set; }
        public string cType { get; set; }
        public string sDescription { get; set; }
        public bool bBreakfast { get; set; }
        public bool bLunch { get; set; }
        public bool bDinner { get; set; }
        public Nullable<bool> b24hours { get; set; }
        public string sTimingFromHH { get; set; }
        public string sTimingFromMM { get; set; }
        public string sTimingToHH { get; set; }
        public string sTimingToMM { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
        public bool bActive { get; set; }
    }
}
