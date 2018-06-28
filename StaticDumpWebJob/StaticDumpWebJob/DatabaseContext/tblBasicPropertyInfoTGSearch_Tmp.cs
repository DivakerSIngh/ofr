namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblBasicPropertyInfoTGSearch_Tmp
    {
        [Key]
        public long iId { get; set; }

        [StringLength(50)]
        public string sSessionId { get; set; }

        [StringLength(12)]
        public string sHotelCode { get; set; }

        [StringLength(10)]
        public string sCurrencyCode { get; set; }

        [StringLength(10)]
        public string sStopCell { get; set; }

        [StringLength(50)]
        public string sLowestRatePlanCode { get; set; }

        public int? iRank { get; set; }

        [StringLength(50)]
        public string sHotelType { get; set; }

        [StringLength(10)]
        public string iReviewRating { get; set; }

        [StringLength(10)]
        public string iReviewCount { get; set; }

        [StringLength(500)]
        public string sOverviewUrl { get; set; }

        public DateTime? dtActionDate { get; set; }
    }
}
