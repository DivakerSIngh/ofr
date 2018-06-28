namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyBasicBiddingMap")]
    public partial class tblPropertyBasicBiddingMap
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPropId { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime dtEffectiveDate { get; set; }

        public bool? bIsClosed { get; set; }

        public bool? bCTA { get; set; }

        public bool? bCTD { get; set; }

        public decimal? dBasicDiscount { get; set; }

        public bool? bIsClosedroom { get; set; }

        public bool? bCTAroom { get; set; }

        public bool? bCTDroom { get; set; }

        public bool? bIsClosedlead { get; set; }

        public bool? bCTAlead { get; set; }

        public bool? bCTDlead { get; set; }

        public bool? bIsClosedlos { get; set; }

        public bool? bCTAlos { get; set; }

        public bool? bCTDlos { get; set; }

        public bool? bIsClosedweek { get; set; }

        public bool? bCTAweek { get; set; }

        public bool? bCTDweek { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public virtual tblUserM tblUserM { get; set; }
    }
}
