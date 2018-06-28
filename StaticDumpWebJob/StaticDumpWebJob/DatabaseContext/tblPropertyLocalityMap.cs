namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyLocalityMap")]
    public partial class tblPropertyLocalityMap
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPropId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iAreaLocalityId { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(1)]
        public string cAreaLocality { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }
    }
}
