namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblBookingCancellationPolicyAmendMap")]
    public partial class tblBookingCancellationPolicyAmendMap
    {
        [Key]
        public long iID { get; set; }

        public long iAmendId { get; set; }

        public long? iBookingDetailsID { get; set; }

        [Required]
        [StringLength(100)]
        public string sPolicyName { get; set; }

        [Column(TypeName = "date")]
        public DateTime dtDate { get; set; }

        public DateTime? dtActionDate { get; set; }

        public virtual tblBookingAmedTx tblBookingAmedTx { get; set; }
    }
}
