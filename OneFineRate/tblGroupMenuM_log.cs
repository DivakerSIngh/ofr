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
    
    public partial class tblGroupMenuM_log
    {
        public int iGroupId { get; set; }
        public int iMenuId { get; set; }
        public Nullable<System.DateTime> dtCreationDate { get; set; }
        public Nullable<int> iCreatedBy { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
        public string cStatus { get; set; }
        public string vc_changed_by { get; set; }
        public string vc_changed_ip { get; set; }
        public string vc_programname { get; set; }
        public Nullable<System.DateTime> dt_changed_date { get; set; }
        public string ch_action { get; set; }
    }
}
