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
    
    public partial class tblPropertyRoomMap
    {
        public tblPropertyRoomMap()
        {
            this.tblChangeHistoryPIRooms = new HashSet<tblChangeHistoryPIRoom>();
            this.tblChangeHistoryRates = new HashSet<tblChangeHistoryRate>();
            this.tblChangeHistoryRooms = new HashSet<tblChangeHistoryRoom>();
            this.tblPropertyBiddingRateMs = new HashSet<tblPropertyBiddingRateM>();
            this.tblPropertyPrimaryPhotoMaps = new HashSet<tblPropertyPrimaryPhotoMap>();
            this.tblPropertyPromoRateMaps = new HashSet<tblPropertyPromoRateMap>();
            this.tblPropertyPromotionRoomTypeMaps = new HashSet<tblPropertyPromotionRoomTypeMap>();
            this.tblPropertyRoomInventories = new HashSet<tblPropertyRoomInventory>();
            this.tblPropertyRoomRatePlanInventoryMaps = new HashSet<tblPropertyRoomRatePlanInventoryMap>();
            this.tblPropertyRoomTypeRoomAmentiesMaps = new HashSet<tblPropertyRoomTypeRoomAmentiesMap>();
            this.tblPropertyTaxMaps = new HashSet<tblPropertyTaxMap>();
        }
    
        public long iRoomId { get; set; }
        public int iPropId { get; set; }
        public short iRoomTypeId { get; set; }
        public string sRoomName { get; set; }
        public string sRoomCode { get; set; }
        public Nullable<byte> iMaxOccupancy { get; set; }
        public Nullable<byte> iMaxChildren { get; set; }
        public Nullable<byte> iNoOfExtraBeds { get; set; }
        public Nullable<byte> iNoOfBedrooms { get; set; }
        public Nullable<byte> iNoOfBathrooms { get; set; }
        public Nullable<decimal> dSizeSqft { get; set; }
        public Nullable<decimal> dSizeMtr { get; set; }
        public string sSizeType { get; set; }
        public Nullable<short> iTotalRooms { get; set; }
        public Nullable<short> iTwinBeds { get; set; }
        public Nullable<short> iDoubleBeds { get; set; }
        public string sRoomAccessibilityIds { get; set; }
        public string sBuildingCharacteristicsIds { get; set; }
        public string sRoomOutdoorViewIds { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
        public bool bActive { get; set; }
        public Nullable<byte> iSeq { get; set; }
    
        public virtual ICollection<tblChangeHistoryPIRoom> tblChangeHistoryPIRooms { get; set; }
        public virtual ICollection<tblChangeHistoryRate> tblChangeHistoryRates { get; set; }
        public virtual ICollection<tblChangeHistoryRoom> tblChangeHistoryRooms { get; set; }
        public virtual ICollection<tblPropertyBiddingRateM> tblPropertyBiddingRateMs { get; set; }
        public virtual ICollection<tblPropertyPrimaryPhotoMap> tblPropertyPrimaryPhotoMaps { get; set; }
        public virtual ICollection<tblPropertyPromoRateMap> tblPropertyPromoRateMaps { get; set; }
        public virtual ICollection<tblPropertyPromotionRoomTypeMap> tblPropertyPromotionRoomTypeMaps { get; set; }
        public virtual ICollection<tblPropertyRoomInventory> tblPropertyRoomInventories { get; set; }
        public virtual tblUserM tblUserM { get; set; }
        public virtual ICollection<tblPropertyRoomRatePlanInventoryMap> tblPropertyRoomRatePlanInventoryMaps { get; set; }
        public virtual ICollection<tblPropertyRoomTypeRoomAmentiesMap> tblPropertyRoomTypeRoomAmentiesMaps { get; set; }
        public virtual ICollection<tblPropertyTaxMap> tblPropertyTaxMaps { get; set; }
    }
}