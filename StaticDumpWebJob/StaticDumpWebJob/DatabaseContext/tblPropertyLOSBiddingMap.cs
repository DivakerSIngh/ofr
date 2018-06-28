namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyLOSBiddingMap")]
    public partial class tblPropertyLOSBiddingMap
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPropId { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime dtEffectiveDate { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short iLOSDays { get; set; }

        public decimal? dLeadDiscount { get; set; }

        public int? iAmenityId1 { get; set; }

        public short? iApplicabilityId1 { get; set; }

        public int? iAmenityId2 { get; set; }

        public short? iApplicabilityId2 { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public virtual tblAmenityM tblAmenityM { get; set; }

        public virtual tblAmenityM tblAmenityM1 { get; set; }

        public virtual tblApplicabilityM tblApplicabilityM { get; set; }

        public virtual tblApplicabilityM tblApplicabilityM1 { get; set; }

        public virtual tblUserM tblUserM { get; set; }
    }
}
