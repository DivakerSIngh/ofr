namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyPolicyMap")]
    public partial class tblPropertyPolicyMap
    {
        [Key]
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

        public bool bChildrenAllowed { get; set; }

        public byte? iComplimentaryStayAge { get; set; }

        public byte? iExtraBedRequiredFrom { get; set; }

        public decimal? dExtraBedCharges { get; set; }

        public bool bAlcoholAllowedOnsite { get; set; }

        public bool bAlcoholServedOnsite { get; set; }

        public bool bVisitorsAllowed { get; set; }

        public bool bPetsAllowed { get; set; }

        public bool bPartiesAllowed { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public virtual tblUserM tblUserM { get; set; }
    }
}
