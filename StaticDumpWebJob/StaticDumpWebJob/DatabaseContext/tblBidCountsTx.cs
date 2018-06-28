namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblBidCountsTx")]
    public partial class tblBidCountsTx
    {
        [Key]
        public long iId { get; set; }

        [StringLength(10)]
        public string sMobile { get; set; }

        public DateTime? dtActionDate { get; set; }
    }
}
