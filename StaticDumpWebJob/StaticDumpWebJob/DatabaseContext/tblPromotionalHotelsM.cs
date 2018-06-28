namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPromotionalHotelsM")]
    public partial class tblPromotionalHotelsM
    {
        [Key]
        public long iId { get; set; }

        public int iPropId { get; set; }

        [Required]
        [StringLength(2)]
        public string sPosition { get; set; }

        [StringLength(200)]
        public string sImageUrl { get; set; }

        [StringLength(1000)]
        public string sDescription { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }
    }
}
