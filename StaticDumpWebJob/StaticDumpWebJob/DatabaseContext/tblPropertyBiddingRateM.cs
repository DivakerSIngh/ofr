namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyBiddingRateM")]
    public partial class tblPropertyBiddingRateM
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPropId { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime dtEffectiveDate { get; set; }

        public long? iRoomId { get; set; }

        public int? iRPId { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public virtual tblPropertyRatePlanMap tblPropertyRatePlanMap { get; set; }

        public virtual tblPropertyRoomMap tblPropertyRoomMap { get; set; }
    }
}
