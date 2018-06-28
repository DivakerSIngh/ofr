namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblPropertyDiningMap_log
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long iRestaurantID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPropId { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string sRestaurantName { get; set; }

        [StringLength(1)]
        public string cType { get; set; }

        [StringLength(50)]
        public string sDescription { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool bBreakfast { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool bLunch { get; set; }

        [Key]
        [Column(Order = 5)]
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

        [Key]
        [Column(Order = 6)]
        public bool bActive { get; set; }

        [StringLength(150)]
        public string vc_changed_by { get; set; }

        [StringLength(100)]
        public string vc_changed_ip { get; set; }

        [StringLength(1000)]
        public string vc_programname { get; set; }

        public DateTime? dt_changed_date { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(1)]
        public string ch_action { get; set; }
    }
}
