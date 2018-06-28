namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblBasicPropertyInfoTGSpecific_Tmp
    {
        [Key]
        public long iId { get; set; }

        [StringLength(50)]
        public string sSessionId { get; set; }

        [StringLength(12)]
        public string sHotelCode { get; set; }

        [StringLength(200)]
        public string sHotelName { get; set; }

        [StringLength(5)]
        public string sCurrencyCode { get; set; }

        [StringLength(20)]
        public string sLatitude { get; set; }

        [StringLength(20)]
        public string sLongitude { get; set; }

        [StringLength(300)]
        public string sAddress { get; set; }

        [StringLength(50)]
        public string sCity { get; set; }

        [StringLength(10)]
        public string sPostalCode { get; set; }

        [StringLength(50)]
        public string sStateName { get; set; }

        [StringLength(50)]
        public string sCountryName { get; set; }

        [StringLength(10)]
        public string sAwardRating { get; set; }

        [StringLength(10)]
        public string sStopCell { get; set; }

        [StringLength(50)]
        public string sLowestRatePlanCode { get; set; }

        [StringLength(50)]
        public string sFlexibleCheckIn { get; set; }

        [StringLength(50)]
        public string sStateSeoId { get; set; }

        [StringLength(50)]
        public string sCitySeoId { get; set; }

        public int? iNoOfRooms { get; set; }

        public int? iNoOfFloors { get; set; }

        [StringLength(50)]
        public string sHotelType { get; set; }

        public string sDescription { get; set; }

        [StringLength(10)]
        public string sCheckInTime { get; set; }

        [StringLength(10)]
        public string sCheckOutTime { get; set; }

        [StringLength(50)]
        public string sAreaSeoId { get; set; }

        [StringLength(50)]
        public string sArea { get; set; }

        [StringLength(500)]
        public string sAmenityDescription { get; set; }

        [StringLength(200)]
        public string sReviewImageUrl { get; set; }

        [StringLength(200)]
        public string sThumbnailUrl { get; set; }

        [StringLength(200)]
        public string sThumbnailImageName { get; set; }

        [StringLength(200)]
        public string sImageUrl { get; set; }

        [StringLength(300)]
        public string sOverviewUrl { get; set; }

        public DateTime? dtActionDate { get; set; }
    }
}
