namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblRecentSavingsTx")]
    public partial class tblRecentSavingsTx
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long iBookingId { get; set; }

        [StringLength(100)]
        public string sCity { get; set; }

        public short? iStar { get; set; }

        [StringLength(5)]
        public string sSymbol { get; set; }

        [StringLength(3)]
        public string sCurrencyCode { get; set; }

        public decimal? dActualAmt { get; set; }

        public decimal? dDiscountedAmt { get; set; }

        public DateTime? dtBookingDate { get; set; }
    }
}
