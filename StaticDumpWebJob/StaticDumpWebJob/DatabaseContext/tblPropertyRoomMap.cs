namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyRoomMap")]
    public partial class tblPropertyRoomMap
    {
        public tblPropertyRoomMap()
        {
            tblChangeHistoryPIRooms = new HashSet<tblChangeHistoryPIRoom>();
            tblChangeHistoryRates = new HashSet<tblChangeHistoryRate>();
            tblChangeHistoryRooms = new HashSet<tblChangeHistoryRoom>();
            tblPropertyBiddingRateMs = new HashSet<tblPropertyBiddingRateM>();
            tblPropertyPrimaryPhotoMaps = new HashSet<tblPropertyPrimaryPhotoMap>();
            tblPropertyPromoRateMaps = new HashSet<tblPropertyPromoRateMap>();
            tblPropertyPromotionRoomTypeMaps = new HashSet<tblPropertyPromotionRoomTypeMap>();
            tblPropertyRoomInventories = new HashSet<tblPropertyRoomInventory>();
            tblPropertyRoomRatePlanInventoryMaps = new HashSet<tblPropertyRoomRatePlanInventoryMap>();
            tblPropertyRoomTypeRoomAmentiesMaps = new HashSet<tblPropertyRoomTypeRoomAmentiesMap>();
            tblPropertyTaxMaps = new HashSet<tblPropertyTaxMap>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long iRoomId { get; set; }

        public int iPropId { get; set; }

        public short iRoomTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string sRoomName { get; set; }

        [StringLength(50)]
        public string sRoomCode { get; set; }

        public byte? iMaxOccupancy { get; set; }

        public byte? iMaxChildren { get; set; }

        public byte? iNoOfExtraBeds { get; set; }

        public byte? iNoOfBedrooms { get; set; }

        public byte? iNoOfBathrooms { get; set; }

        public decimal? dSizeSqft { get; set; }

        public decimal? dSizeMtr { get; set; }

        [StringLength(10)]
        public string sSizeType { get; set; }

        public short? iTotalRooms { get; set; }

        public short? iTwinBeds { get; set; }

        public short? iDoubleBeds { get; set; }

        [StringLength(50)]
        public string sRoomAccessibilityIds { get; set; }

        [StringLength(50)]
        public string sBuildingCharacteristicsIds { get; set; }

        [StringLength(50)]
        public string sRoomOutdoorViewIds { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public bool bActive { get; set; }

        public byte? iSeq { get; set; }

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
