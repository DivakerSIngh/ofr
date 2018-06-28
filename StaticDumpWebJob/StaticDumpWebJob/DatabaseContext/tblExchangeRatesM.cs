namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblExchangeRatesM")]
    public partial class tblExchangeRatesM
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(3)]
        public string sCurrencyCodeFrom { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(3)]
        public string sCurrencyCodeTo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? dRate { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }
    }
}
