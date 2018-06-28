namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyRatePlanMap")]
    public partial class tblPropertyRatePlanMap
    {
        public tblPropertyRatePlanMap()
        {
            tblChangeHistoryRates = new HashSet<tblChangeHistoryRate>();
            tblPropertyBiddingRateMs = new HashSet<tblPropertyBiddingRateM>();
            tblPropertyPromotionMaps = new HashSet<tblPropertyPromotionMap>();
            tblPropertyRatePlanRoomTypeMaps = new HashSet<tblPropertyRatePlanRoomTypeMap>();
            tblPropertyRoomRatePlanInventoryMaps = new HashSet<tblPropertyRoomRatePlanInventoryMap>();
        }

        [Key]
        public int iRPId { get; set; }

        public int? iPropId { get; set; }

        [StringLength(50)]
        public string sRatePlan { get; set; }

        [StringLength(1)]
        public string cRatePlanType { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtValidFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtValidTo { get; set; }

        public bool? bLinkToExistingRatePlan { get; set; }

        public int? iLinkRatePlanId { get; set; }

        public bool? bIsPlus { get; set; }

        public bool? bIsPercent { get; set; }

        public decimal? dValue { get; set; }

        [StringLength(50)]
        public string sAmenityId { get; set; }

        [StringLength(1000)]
        public string sAmenity { get; set; }

        public short? iMinLengthStay { get; set; }

        public short? iMaxLengthStay { get; set; }

        public short? dHrsDays { get; set; }

        [StringLength(1)]
        public string cHrsDays { get; set; }

        public bool? bIsBefore { get; set; }

        [StringLength(100)]
        public string sCancellationPolicy { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtCancellationValidFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtCancellationValidTo { get; set; }

        public bool? bIsSecretDeal { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        [StringLength(200)]
        public string sRoomId { get; set; }

        public decimal? dNegotiationPer { get; set; }

        public bool? bParentActive { get; set; }

        public virtual ICollection<tblChangeHistoryRate> tblChangeHistoryRates { get; set; }

        public virtual ICollection<tblPropertyBiddingRateM> tblPropertyBiddingRateMs { get; set; }

        public virtual ICollection<tblPropertyPromotionMap> tblPropertyPromotionMaps { get; set; }

        public virtual tblUserM tblUserM { get; set; }

        public virtual ICollection<tblPropertyRatePlanRoomTypeMap> tblPropertyRatePlanRoomTypeMaps { get; set; }

        public virtual ICollection<tblPropertyRoomRatePlanInventoryMap> tblPropertyRoomRatePlanInventoryMaps { get; set; }
    }
}
