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
    
    public partial class tblPropertyParkingMap
    {
        public int iPropId { get; set; }
        public string sCarName { get; set; }
        public string cAirportRail { get; set; }
        public Nullable<bool> bIsFree { get; set; }
        public Nullable<decimal> dOnewayCharge { get; set; }
        public Nullable<decimal> dTwowayCharge { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
    
        public virtual tblPropertyM tblPropertyM { get; set; }
        public virtual tblUserM tblUserM { get; set; }
    }
}
