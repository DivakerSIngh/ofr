namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblConserveCommissionM")]
    public partial class tblConserveCommissionM
    {
        [Key]
        public int iCCId { get; set; }

        public decimal? dCommission { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtTo { get; set; }

        public bool? bDisplayRateComm { get; set; }

        public bool? bCOComm { get; set; }

        public decimal? dCounterTrigger { get; set; }

        public bool? bBidComm { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }
    }
}
