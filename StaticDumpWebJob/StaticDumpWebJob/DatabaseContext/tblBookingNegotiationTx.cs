namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblBookingNegotiationTx")]
    public partial class tblBookingNegotiationTx
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long iBookingId { get; set; }

        [Key]
        [Column(Order = 1)]
        public DateTime dtNegotiationDate { get; set; }

        public decimal? dTotalNegotiateAmount { get; set; }

        public DateTime? dtActionDate { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }
    }
}
