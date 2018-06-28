namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblPropertySpaMap_log
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPropId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string sSpaName { get; set; }

        [StringLength(300)]
        public string sSpaDesc { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool bHotsprings { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool bSauna { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool bMudbath { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool bSpaTub { get; set; }

        [Key]
        [Column(Order = 6)]
        public bool bAdvancedBooking { get; set; }

        [Key]
        [Column(Order = 7)]
        public bool bSteamRoom { get; set; }

        [Key]
        [Column(Order = 8)]
        public bool b24hours { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        [StringLength(150)]
        public string vc_changed_by { get; set; }

        [StringLength(100)]
        public string vc_changed_ip { get; set; }

        [StringLength(1000)]
        public string vc_programname { get; set; }

        public DateTime? dt_changed_date { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(1)]
        public string ch_action { get; set; }
    }
}
