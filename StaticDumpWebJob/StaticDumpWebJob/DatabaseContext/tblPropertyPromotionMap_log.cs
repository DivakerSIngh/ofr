namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblPropertyPromotionMap_log
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPropId { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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

        [Key]
        [Column(Order = 3)]
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

        [StringLength(150)]
        public string vc_changed_by { get; set; }

        [StringLength(100)]
        public string vc_changed_ip { get; set; }

        [StringLength(1000)]
        public string vc_programname { get; set; }

        public DateTime? dt_changed_date { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(1)]
        public string ch_action { get; set; }

        public decimal? dNegotiationPer { get; set; }
    }
}
