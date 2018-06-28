namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblRatePlanCancelPenaltyTGSpecific_Tmp
    {
        [Key]
        public long iId { get; set; }

        [StringLength(50)]
        public string sSessionId { get; set; }

        [StringLength(12)]
        public string sHotelCode { get; set; }

        [StringLength(12)]
        public string sRatePlanCode { get; set; }

        [StringLength(50)]
        public string sOffSetDropTime { get; set; }

        [StringLength(50)]
        public string sOffSetTimeUnit { get; set; }

        [StringLength(50)]
        public string sOffSetUnitMultiply { get; set; }

        [StringLength(10)]
        public string sNumberOfNight { get; set; }

        [StringLength(10)]
        public string sTaxInclusive { get; set; }

        public DateTime? dtActionDate { get; set; }
    }
}
