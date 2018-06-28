namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblRankManagement")]
    public partial class tblRankManagement
    {
        [Key]
        public int iID { get; set; }

        public int iPropId { get; set; }

        public DateTime dtStartdate { get; set; }

        public DateTime dtEndDate { get; set; }

        public bool? IsSponsored { get; set; }

        public bool? IsRateDisparity { get; set; }

        public int? iNewRank { get; set; }

        public int? iOldRank { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }
    }
}
