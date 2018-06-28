namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyCancellationPolicyMap")]
    public partial class tblPropertyCancellationPolicyMap
    {
        public tblPropertyCancellationPolicyMap()
        {
            tblPropertyPromotionCancellationMaps = new HashSet<tblPropertyPromotionCancellationMap>();
        }

        [Key]
        public int iCancellationPolicyId { get; set; }

        public int iPropId { get; set; }

        [Required]
        [StringLength(100)]
        public string sPolicyName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtValidFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtValidTo { get; set; }

        public short? dHrsDays { get; set; }

        [StringLength(10)]
        public string cHrsDays { get; set; }

        public bool? bIsNonRefundable { get; set; }

        public bool? bIsPercent { get; set; }

        public decimal? dValue { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        [Required]
        [StringLength(1)]
        public string cStatus { get; set; }

        public virtual ICollection<tblPropertyPromotionCancellationMap> tblPropertyPromotionCancellationMaps { get; set; }
    }
}
