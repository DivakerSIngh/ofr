namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblOfferReviewTrackerTx")]
    public partial class tblOfferReviewTrackerTx
    {
        [Key]
        public long iId { get; set; }

        public int? iPropId { get; set; }

        [StringLength(15)]
        public string sIPAddress { get; set; }

        public DateTime? dtActionDate { get; set; }
    }
}
