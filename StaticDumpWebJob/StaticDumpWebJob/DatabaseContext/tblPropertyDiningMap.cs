namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyDiningMap")]
    public partial class tblPropertyDiningMap
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long iRestaurantID { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPropId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string sRestaurantName { get; set; }

        [StringLength(1)]
        public string cType { get; set; }

        [StringLength(50)]
        public string sDescription { get; set; }

        public bool bBreakfast { get; set; }

        public bool bLunch { get; set; }

        public bool bDinner { get; set; }

        [StringLength(2)]
        public string sTimingFromHH { get; set; }

        [StringLength(2)]
        public string sTimingFromMM { get; set; }

        [StringLength(2)]
        public string sTimingToHH { get; set; }

        [StringLength(2)]
        public string sTimingToMM { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public bool bActive { get; set; }
    }
}
