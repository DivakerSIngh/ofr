namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblPropertyMTG_Tmp
    {
        [Key]
        public int iPropId { get; set; }

        [StringLength(300)]
        public string sHotelName { get; set; }

        [StringLength(12)]
        public string iChainId { get; set; }

        public short? iStarCategoryId { get; set; }

        public short? iCurrencyId { get; set; }

        public int? iAccomodationId { get; set; }

        public short? iYearBuilt { get; set; }

        public short? iLastRenovation { get; set; }

        public short? iRooms { get; set; }

        public short? iFloors { get; set; }

        public short? iTower { get; set; }

        [StringLength(100)]
        public string sLocality { get; set; }

        [StringLength(100)]
        public string sArea { get; set; }

        [Required]
        [StringLength(100)]
        public string sCountry { get; set; }

        [Required]
        [StringLength(100)]
        public string sState { get; set; }

        [Required]
        [StringLength(100)]
        public string sCity { get; set; }

        [StringLength(1000)]
        public string sAddress { get; set; }

        public int? iPinCode { get; set; }

        [StringLength(250)]
        public string sWebSite { get; set; }

        public decimal? dLatitude { get; set; }

        public decimal? dLongitude { get; set; }

        [StringLength(8000)]
        public string sDescription { get; set; }

        [StringLength(200)]
        public string sDistanceFromAirportRailwayStation { get; set; }

        [StringLength(200)]
        public string sNearestTransport { get; set; }

        [StringLength(200)]
        public string sAreaRecommended { get; set; }

        public string sTopAttractions { get; set; }

        [StringLength(50)]
        public string sGeneralManagerName { get; set; }

        [StringLength(50)]
        public string sRevenueManagerName { get; set; }

        [StringLength(50)]
        public string sFinanceContactName { get; set; }

        [StringLength(50)]
        public string sPrimaryContactName { get; set; }

        [StringLength(15)]
        public string sGeneralManagerContact { get; set; }

        [StringLength(15)]
        public string sRevenueManagerContact { get; set; }

        [StringLength(15)]
        public string sFinanceContactContact { get; set; }

        [StringLength(15)]
        public string sPrimaryContactContact { get; set; }

        [StringLength(250)]
        public string sGeneralManagerEmail { get; set; }

        [StringLength(250)]
        public string sRevenueManagerEmail { get; set; }

        [StringLength(250)]
        public string sFinanceContactEmail { get; set; }

        [StringLength(250)]
        public string sPrimaryContactEmail { get; set; }

        [StringLength(50)]
        public string sAccessbilityIds { get; set; }

        [StringLength(50)]
        public string sAwardIds { get; set; }

        [StringLength(50)]
        public string sAffiliationIds { get; set; }

        [StringLength(50)]
        public string sLanguageIds { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        [StringLength(100)]
        public string sAreaRecommended1 { get; set; }

        [StringLength(100)]
        public string sAreaRecommended2 { get; set; }

        [StringLength(100)]
        public string sAreaRecommended3 { get; set; }

        [StringLength(100)]
        public string sNearestTransport1 { get; set; }

        public decimal? dNearestTransport1 { get; set; }

        [StringLength(100)]
        public string sNearestTransport2 { get; set; }

        public decimal? dNearestTransport2 { get; set; }

        [StringLength(100)]
        public string sNearestTransport3 { get; set; }

        public decimal? dNearestTransport3 { get; set; }

        [StringLength(100)]
        public string sTopAttraction1 { get; set; }

        [StringLength(100)]
        public string sTopAttraction2 { get; set; }

        [StringLength(100)]
        public string sTopAttraction3 { get; set; }

        [StringLength(100)]
        public string sDistanceToAirportRailway1 { get; set; }

        public decimal? dDistanceToAirportRailway1 { get; set; }

        [StringLength(100)]
        public string sDistanceToAirportRailway2 { get; set; }

        public decimal? dDistanceToAirportRailway2 { get; set; }

        [StringLength(100)]
        public string sDistanceToAirportRailway3 { get; set; }

        public decimal? dDistanceToAirportRailway3 { get; set; }

        [StringLength(50)]
        public string sConfirmationContactName { get; set; }

        [StringLength(250)]
        public string sConfirmationContactEmail { get; set; }

        [StringLength(15)]
        public string sConfirmationContactContact { get; set; }

        [StringLength(12)]
        public string iVendorId { get; set; }

        public bool? bIsTG { get; set; }

        [StringLength(300)]
        public string sImageUrlTG { get; set; }

        [StringLength(500)]
        public string sSecondaryAreaNameTG { get; set; }
    }
}