namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyTripAdvisorM")]
    public partial class tblPropertyTripAdvisorM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPropId { get; set; }

        public int? iTripAdvisorLOCID { get; set; }

        [StringLength(500)]
        public string sRatingImageURL { get; set; }

        public decimal? iRating { get; set; }

        [StringLength(500)]
        public string sRankingString { get; set; }

        public int? iReviewsCount { get; set; }

        [StringLength(500)]
        public string sWebURL { get; set; }
    }
}
