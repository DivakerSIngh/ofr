namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblPropertyPolicyMap_log
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPropId { get; set; }

        [StringLength(100)]
        public string sCreditCardId { get; set; }

        public bool? b24HrsCheckIn { get; set; }

        [StringLength(2)]
        public string sCheckInHH { get; set; }

        [StringLength(2)]
        public string sCheckInMM { get; set; }

        public bool? bEarlyCheckInChargeable { get; set; }

        public bool? b24HrsCheckout { get; set; }

        [StringLength(2)]
        public string sCheckoutHH { get; set; }

        [StringLength(2)]
        public string sCheckoutMM { get; set; }

        public bool? bEarlyCheckoutChargeable { get; set; }

        public byte? iMinCheckInAge { get; set; }

        [Key]
        [Column(Order = 1)]
        public bool bChildrenAllowed { get; set; }

        public byte? iComplimentaryStayAge { get; set; }

        public byte? iExtraBedRequiredFrom { get; set; }

        public decimal? dExtraBedCharges { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool bAlcoholAllowedOnsite { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool bAlcoholServedOnsite { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool bVisitorsAllowed { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool bPetsAllowed { get; set; }

        [Key]
        [Column(Order = 6)]
        public bool bPartiesAllowed { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        [StringLength(150)]
        public string vc_changed_by { get; set; }

        [StringLength(100)]
        public string vc_changed_ip { get; set; }

        [StringLength(1000)]
        public string vc_programname { get; set; }

        public DateTime? dt_changed_date { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(1)]
        public string ch_action { get; set; }
    }
}
