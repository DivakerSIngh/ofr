namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyPromotionCancellationMap")]
    public partial class tblPropertyPromotionCancellationMap
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iCancellationPolicyId { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "date")]
        public DateTime dtDate { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public virtual tblPropertyCancellationPolicyMap tblPropertyCancellationPolicyMap { get; set; }

        public virtual tblPropertyPromotionMap tblPropertyPromotionMap { get; set; }
    }
}
