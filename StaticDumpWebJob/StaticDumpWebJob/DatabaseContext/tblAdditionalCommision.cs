namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblAdditionalCommision")]
    public partial class tblAdditionalCommision
    {
        [Key]
        public int iAdditionalCommisionId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtCommisionStartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtCommisionEndDate { get; set; }

        public decimal? dCommission { get; set; }

        public int? iPropId { get; set; }

        public bool bActive { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public virtual tblUserM tblUserM { get; set; }
    }
}
