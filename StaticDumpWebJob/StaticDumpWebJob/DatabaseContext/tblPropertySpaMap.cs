namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertySpaMap")]
    public partial class tblPropertySpaMap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPropId { get; set; }

        [Required]
        [StringLength(100)]
        public string sSpaName { get; set; }

        [StringLength(300)]
        public string sSpaDesc { get; set; }

        public bool bHotsprings { get; set; }

        public bool bSauna { get; set; }

        public bool bMudbath { get; set; }

        public bool bSpaTub { get; set; }

        public bool bAdvancedBooking { get; set; }

        public bool bSteamRoom { get; set; }

        public bool b24hours { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }
    }
}
