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
    
    public partial class tblCountryM
    {
        public tblCountryM()
        {
            this.tblCityMs = new HashSet<tblCityM>();
            this.tblLocalityMs = new HashSet<tblLocalityM>();
            this.tblStateMs = new HashSet<tblStateM>();
        }
    
        public int iCountryId { get; set; }
        public string sCountry { get; set; }
        public string sCountryCode { get; set; }
        public string cStatus { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
        public string sCountryCurrencyCode { get; set; }
    
        public virtual ICollection<tblCityM> tblCityMs { get; set; }
        public virtual tblUserM tblUserM { get; set; }
        public virtual ICollection<tblLocalityM> tblLocalityMs { get; set; }
        public virtual ICollection<tblStateM> tblStateMs { get; set; }
    }
}
