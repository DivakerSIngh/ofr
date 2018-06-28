namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyTopAttrationTG")]
    public partial class tblPropertyTopAttrationTG
    {
        [Key]
        public long iPropertyTopAttractionId { get; set; }

        [StringLength(12)]
        public string iVendorId { get; set; }

        [StringLength(500)]
        public string sNameOfAttraction { get; set; }

        public decimal? iDistanceInKM { get; set; }

        public DateTime? dtActionDate { get; set; }
    }
}
