namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblUserM_log
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iUserId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string sUserName { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string sPassword { get; set; }

        [StringLength(50)]
        public string sFirstName { get; set; }

        [StringLength(50)]
        public string sLastName { get; set; }

        [StringLength(200)]
        public string sEmail { get; set; }

        [StringLength(15)]
        public string sContact { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime dtCreatedOn { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }

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
        [Column(Order = 4)]
        [StringLength(1)]
        public string ch_action { get; set; }
    }
}
