namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblTripAdvisorReview
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iTripAdvisorLOCID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iReviewId { get; set; }

        [StringLength(200)]
        public string sTitle { get; set; }

        public string sText { get; set; }

        [StringLength(200)]
        public string sReviewer { get; set; }

        public decimal? iRating { get; set; }

        [StringLength(500)]
        public string sRatingImageURL { get; set; }

        [StringLength(500)]
        public string sReviewURL { get; set; }

        public DateTime? dtReviewDate { get; set; }

        public DateTime? dtTravelDate { get; set; }

        [StringLength(50)]
        public string sTripType { get; set; }

        [StringLength(200)]
        public string sUserLocation { get; set; }
    }
}
