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
    
    public partial class tblPropertyRoomInventory
    {
        public System.DateTime dtInventoryDate { get; set; }
        public int iPropId { get; set; }
        public long iRoomId { get; set; }
        public Nullable<short> iInventory { get; set; }
        public Nullable<short> iSoldInventory { get; set; }
        public Nullable<short> iAvailableInventory { get; set; }
        public bool bStopSell { get; set; }
        public Nullable<short> iCutOff { get; set; }
        public Nullable<short> iBlockedInventory { get; set; }
        public Nullable<System.DateTime> dtInventoryUpdateDate { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
    
        public virtual tblPropertyM tblPropertyM { get; set; }
        public virtual tblPropertyRoomMap tblPropertyRoomMap { get; set; }
        public virtual tblUserM tblUserM { get; set; }
    }
}
