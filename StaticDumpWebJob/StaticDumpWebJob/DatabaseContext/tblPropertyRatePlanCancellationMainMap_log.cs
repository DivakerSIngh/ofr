namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblPropertyRatePlanCancellationMainMap_log
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPropId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iRPId { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "date")]
        public DateTime dtCancellationValidFrom { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "date")]
        public DateTime dtCancellationValidTo { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string sCancellationPolicyId { get; set; }

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
        [Column(Order = 5)]
        [StringLength(1)]
        public string ch_action { get; set; }
    }
}
