namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyPromotionMap")]
    public partial class tblPropertyPromotionMap
    {
        public tblPropertyPromotionMap()
        {
            tblPropertyPromoRateMaps = new HashSet<tblPropertyPromoRateMap>();
            tblPropertyPromotionCancellationMainMaps = new HashSet<tblPropertyPromotionCancellationMainMap>();
            tblPropertyPromotionCancellationMaps = new HashSet<tblPropertyPromotionCancellationMap>();
            tblPropertyPromotionRoomTypeMaps = new HashSet<tblPropertyPromotionRoomTypeMap>();
        }

        [Key]
        public int iID { get; set; }

        public int iPropId { get; set; }

        public int iPromoId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtBookingDateFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtBookingDateTo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtStayDateFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtStayDateTo { get; set; }

        public int? iRPId { get; set; }

        public bool? bIsPlus { get; set; }

        public bool? bIsPercent { get; set; }

        public decimal? dValue { get; set; }

        [Required]
        [StringLength(50)]
        public string sRoomTypeId { get; set; }

        [StringLength(50)]
        public string sAmenityId { get; set; }

        [StringLength(1000)]
        public string sAmenity { get; set; }

        [StringLength(100)]
        public string sCancellationPolicy { get; set; }

        public bool? bIsSecretDeal { get; set; }

        public short? iHrsDays { get; set; }

        public short? iMinLengthStay { get; set; }

        public short? iMaxLengthStay { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }

        public decimal? dNegotiationPer { get; set; }

        public bool? bParentActive { get; set; }

        public virtual tblPromoM tblPromoM { get; set; }

        public virtual ICollection<tblPropertyPromoRateMap> tblPropertyPromoRateMaps { get; set; }

        public virtual ICollection<tblPropertyPromotionCancellationMainMap> tblPropertyPromotionCancellationMainMaps { get; set; }

        public virtual ICollection<tblPropertyPromotionCancellationMap> tblPropertyPromotionCancellationMaps { get; set; }

        public virtual tblPropertyRatePlanMap tblPropertyRatePlanMap { get; set; }

        public virtual tblUserM tblUserM { get; set; }

        public virtual ICollection<tblPropertyPromotionRoomTypeMap> tblPropertyPromotionRoomTypeMaps { get; set; }
    }
}
