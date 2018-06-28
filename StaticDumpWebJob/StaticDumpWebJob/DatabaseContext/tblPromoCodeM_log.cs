namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblPromoCodeM_log
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPromoCodeId { get; set; }

        [StringLength(100)]
        public string sPromoCode { get; set; }

        [StringLength(100)]
        public string sPromoTitle { get; set; }

        [StringLength(200)]
        public string sPromoDescription { get; set; }

        [StringLength(1)]
        public string cPercentageValue { get; set; }

        public decimal? dValue { get; set; }

        public int? iAmenityId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtBookingFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtBookingTo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtStayFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtStayTo { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        [StringLength(5000)]
        public string sTermCondition { get; set; }

        [StringLength(150)]
        public string vc_changed_by { get; set; }

        [StringLength(100)]
        public string vc_changed_ip { get; set; }

        [StringLength(1000)]
        public string vc_programname { get; set; }

        public DateTime? dt_changed_date { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(1)]
        public string ch_action { get; set; }
    }
}
