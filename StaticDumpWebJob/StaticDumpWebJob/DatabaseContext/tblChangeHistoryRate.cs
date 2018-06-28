namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblChangeHistoryRate
    {
        [Key]
        public long iID { get; set; }

        public int? iPropId { get; set; }

        public int? iRPId { get; set; }

        public long? iRoomId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtEffectiveDate { get; set; }

        [StringLength(100)]
        public string sItem { get; set; }

        [StringLength(500)]
        public string sOld { get; set; }

        [StringLength(500)]
        public string sNew { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public virtual tblPropertyRatePlanMap tblPropertyRatePlanMap { get; set; }

        public virtual tblPropertyRoomMap tblPropertyRoomMap { get; set; }
    }
}
