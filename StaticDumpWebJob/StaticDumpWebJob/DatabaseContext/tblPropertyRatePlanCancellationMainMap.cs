namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyRatePlanCancellationMainMap")]
    public partial class tblPropertyRatePlanCancellationMainMap
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

        [Required]
        [StringLength(50)]
        public string sCancellationPolicyId { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }
    }
}
