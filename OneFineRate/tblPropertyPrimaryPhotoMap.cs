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
    
    public partial class tblPropertyPrimaryPhotoMap
    {
        public long iId { get; set; }
        public long iPhotoId { get; set; }
        public int iPropId { get; set; }
        public Nullable<long> iRoomId { get; set; }
        public string cType { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
    
        public virtual tblPropertyM tblPropertyM { get; set; }
        public virtual tblPropertyPhotoMap tblPropertyPhotoMap { get; set; }
        public virtual tblPropertyRoomMap tblPropertyRoomMap { get; set; }
        public virtual tblUserM tblUserM { get; set; }
    }
}
