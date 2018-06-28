namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblChangeHistoryPolicy")]
    public partial class tblChangeHistoryPolicy
    {
        [Key]
        public long iID { get; set; }

        public int? iPropId { get; set; }

        [StringLength(100)]
        public string sItem { get; set; }

        [StringLength(500)]
        public string sOld { get; set; }

        [StringLength(500)]
        public string sNew { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }
    }
}
