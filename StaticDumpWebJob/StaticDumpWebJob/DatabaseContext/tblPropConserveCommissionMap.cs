namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropConserveCommissionMap")]
    public partial class tblPropConserveCommissionMap
    {
        [Key]
        public int iCCPropId { get; set; }

        public int? iCCId { get; set; }

        public int? iPropId { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }
    }
}
