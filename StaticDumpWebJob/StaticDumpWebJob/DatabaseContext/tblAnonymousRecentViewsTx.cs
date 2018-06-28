namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblAnonymousRecentViewsTx")]
    public partial class tblAnonymousRecentViewsTx
    {
        [Key]
        public long iId { get; set; }

        public int? iPropId { get; set; }

        public DateTime? dtActionDate { get; set; }
    }
}
