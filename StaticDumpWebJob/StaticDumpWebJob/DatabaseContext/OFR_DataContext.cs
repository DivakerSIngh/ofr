namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Configuration;
    using System.Data.Entity.Infrastructure;

    public partial class OFR_DataContext : DbContext
    {
        public OFR_DataContext()
            : base("name=OFR_DataContext")
        {
          
            var objectContext = (this as IObjectContextAdapter).ObjectContext;

            // Sets the command timeout for all the commands
            objectContext.CommandTimeout = 3600;
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<IP2LOCATION> IP2LOCATION { get; set; }
        public virtual DbSet<tblAccessbilityM> tblAccessbilityMs { get; set; }
        public virtual DbSet<tblAccomodationM> tblAccomodationMs { get; set; }
        public virtual DbSet<tblAdditionalCommision> tblAdditionalCommisions { get; set; }
        public virtual DbSet<tblAffiliationM> tblAffiliationMs { get; set; }
        public virtual DbSet<tblAmenityHeaderMapTG> tblAmenityHeaderMapTGs { get; set; }
        public virtual DbSet<tblAmenityM> tblAmenityMs { get; set; }
        public virtual DbSet<tblApplicabilityM> tblApplicabilityMs { get; set; }
        public virtual DbSet<tblAwardM> tblAwardMs { get; set; }
        public virtual DbSet<tblBankDetailM> tblBankDetailMs { get; set; }
        public virtual DbSet<tblBannerM> tblBannerMs { get; set; }
        public virtual DbSet<tblBidCountsTx> tblBidCountsTxes { get; set; }
        public virtual DbSet<tblBIDRoomAdultsTx> tblBIDRoomAdultsTxes { get; set; }
        public virtual DbSet<tblBidSettingsM> tblBidSettingsMs { get; set; }
        public virtual DbSet<tblBookedBidAmenity> tblBookedBidAmenities { get; set; }
        public virtual DbSet<tblBookedDayWiseTaxAmountDetail> tblBookedDayWiseTaxAmountDetails { get; set; }
        public virtual DbSet<tblBookingAmedTx> tblBookingAmedTxes { get; set; }
        public virtual DbSet<tblBookingCancellationPolicyAmendMap> tblBookingCancellationPolicyAmendMaps { get; set; }
        public virtual DbSet<tblBookingCancellationPolicyMap> tblBookingCancellationPolicyMaps { get; set; }
        public virtual DbSet<tblBookingDetailsAmendTx> tblBookingDetailsAmendTxes { get; set; }
        public virtual DbSet<tblBookingDetailsTx> tblBookingDetailsTxes { get; set; }
        public virtual DbSet<tblBookingGuestAmendMap> tblBookingGuestAmendMaps { get; set; }
        public virtual DbSet<tblBookingGuestMap> tblBookingGuestMaps { get; set; }
        public virtual DbSet<tblBookingNegotiationTx> tblBookingNegotiationTxes { get; set; }
        public virtual DbSet<tblBookingTrakerTx> tblBookingTrakerTxes { get; set; }
        public virtual DbSet<tblBookingTx> tblBookingTxes { get; set; }
        public virtual DbSet<tblBuildingCharacteristicsM> tblBuildingCharacteristicsMs { get; set; }
        public virtual DbSet<tblCarouselM> tblCarouselMs { get; set; }
        public virtual DbSet<tblChainM> tblChainMs { get; set; }
        public virtual DbSet<tblChainMTG> tblChainMTGs { get; set; }
        public virtual DbSet<tblChainMTG_Tmp> tblChainMTG_Tmp { get; set; }
        public virtual DbSet<tblChangeHistory> tblChangeHistories { get; set; }
        public virtual DbSet<tblChangeHistoryBankAdditionalCommision> tblChangeHistoryBankAdditionalCommisions { get; set; }
        public virtual DbSet<tblChangeHistoryBankDetail> tblChangeHistoryBankDetails { get; set; }
        public virtual DbSet<tblChangeHistoryBasicDetailsProperty> tblChangeHistoryBasicDetailsProperties { get; set; }
        public virtual DbSet<tblChangeHistoryCompetitorSet> tblChangeHistoryCompetitorSets { get; set; }
        public virtual DbSet<tblChangeHistoryDining> tblChangeHistoryDinings { get; set; }
        public virtual DbSet<tblChangeHistoryPhoto> tblChangeHistoryPhotoes { get; set; }
        public virtual DbSet<tblChangeHistoryPIRoom> tblChangeHistoryPIRooms { get; set; }
        public virtual DbSet<tblChangeHistoryPolicy> tblChangeHistoryPolicies { get; set; }
        public virtual DbSet<tblChangeHistoryPropertyAmenty> tblChangeHistoryPropertyAmenties { get; set; }
        public virtual DbSet<tblChangeHistoryPropertyPromotionMap> tblChangeHistoryPropertyPromotionMaps { get; set; }
        public virtual DbSet<tblChangeHistoryPropertyRatePlanMap> tblChangeHistoryPropertyRatePlanMaps { get; set; }
        public virtual DbSet<tblChangeHistoryRate> tblChangeHistoryRates { get; set; }
        public virtual DbSet<tblChangeHistoryRecreation> tblChangeHistoryRecreations { get; set; }
        public virtual DbSet<tblChangeHistoryRoom> tblChangeHistoryRooms { get; set; }
        public virtual DbSet<tblChangeHistorySpa> tblChangeHistorySpas { get; set; }
        public virtual DbSet<tblChangeHistoryTax> tblChangeHistoryTaxes { get; set; }
        public virtual DbSet<tblChannelMgrBookingTx> tblChannelMgrBookingTxes { get; set; }
        public virtual DbSet<tblChannelMgrM> tblChannelMgrMs { get; set; }
        public virtual DbSet<tblCityM> tblCityMs { get; set; }
        public virtual DbSet<tblConserveCommissionM> tblConserveCommissionMs { get; set; }
        public virtual DbSet<tblCountryM> tblCountryMs { get; set; }
        public virtual DbSet<tblCrediCardM> tblCrediCardMs { get; set; }
        public virtual DbSet<tblCurrencyM> tblCurrencyMs { get; set; }
        public virtual DbSet<tblCustomerM> tblCustomerMs { get; set; }
        public virtual DbSet<tblCustomerPointsHistoryTx> tblCustomerPointsHistoryTxes { get; set; }
        public virtual DbSet<tblCustomerPointsMap> tblCustomerPointsMaps { get; set; }
        public virtual DbSet<tblDaywiseBookingRateTx> tblDaywiseBookingRateTxes { get; set; }
        public virtual DbSet<tblEmailSettingsM> tblEmailSettingsMs { get; set; }
        public virtual DbSet<tblErrorLog> tblErrorLogs { get; set; }
        public virtual DbSet<tblExchangeRatesM> tblExchangeRatesMs { get; set; }
        public virtual DbSet<tblGolfM> tblGolfMs { get; set; }
        public virtual DbSet<tblGroupM> tblGroupMs { get; set; }
        public virtual DbSet<tblGroupMenuM> tblGroupMenuMs { get; set; }
        public virtual DbSet<tblHotelAmenityM> tblHotelAmenityMs { get; set; }
        public virtual DbSet<tblHotelCommonServiceM> tblHotelCommonServiceMs { get; set; }
        public virtual DbSet<tblHotelConvenienceM> tblHotelConvenienceMs { get; set; }
        public virtual DbSet<tblHotelFacilitesRankM> tblHotelFacilitesRankMs { get; set; }
        public virtual DbSet<tblHotelLeisureM> tblHotelLeisureMs { get; set; }
        public virtual DbSet<tblHotelMeetingM> tblHotelMeetingMs { get; set; }
        public virtual DbSet<tblHotelParkingM> tblHotelParkingMs { get; set; }
        public virtual DbSet<tblHotelPartnerM> tblHotelPartnerMs { get; set; }
        public virtual DbSet<tblHotelRoomAmenityAdditionalM> tblHotelRoomAmenityAdditionalMs { get; set; }
        public virtual DbSet<tblHotelRoomAmenityBathRoomM> tblHotelRoomAmenityBathRoomMs { get; set; }
        public virtual DbSet<tblHotelRoomAmenityBeddingM> tblHotelRoomAmenityBeddingMs { get; set; }
        public virtual DbSet<tblHotelRoomAmenityM> tblHotelRoomAmenityMs { get; set; }
        public virtual DbSet<tblIndianLocalityCordinate> tblIndianLocalityCordinates { get; set; }
        public virtual DbSet<tblItemNameM> tblItemNameMs { get; set; }
        public virtual DbSet<tblLandActivitiesM> tblLandActivitiesMs { get; set; }
        public virtual DbSet<tblLanguageM> tblLanguageMs { get; set; }
        public virtual DbSet<tblLocalityM> tblLocalityMs { get; set; }
        public virtual DbSet<tblLookUp> tblLookUps { get; set; }
        public virtual DbSet<tblLookUpExtraBed> tblLookUpExtraBeds { get; set; }
        public virtual DbSet<tblLoyalityAmenityMap> tblLoyalityAmenityMaps { get; set; }
        public virtual DbSet<tblLoyalityPointsM> tblLoyalityPointsMs { get; set; }
        public virtual DbSet<tblMacroAreaM> tblMacroAreaMs { get; set; }
        public virtual DbSet<tblMealCodesRG> tblMealCodesRGs { get; set; }
        public virtual DbSet<tblMenuM> tblMenuMs { get; set; }
        public virtual DbSet<tblMyWishListTx> tblMyWishListTxes { get; set; }
        public virtual DbSet<tblOfferReviewTrackerTx> tblOfferReviewTrackerTxes { get; set; }
        public virtual DbSet<tblOFRPropertyM> tblOFRPropertyMs { get; set; }
        public virtual DbSet<tblOnsiteRecreationFacilitiesM> tblOnsiteRecreationFacilitiesMs { get; set; }
        public virtual DbSet<tblPhotoCategoryM> tblPhotoCategoryMs { get; set; }
        public virtual DbSet<tblPhotoSubCategoryM> tblPhotoSubCategoryMs { get; set; }
        public virtual DbSet<tblPromoCodeM> tblPromoCodeMs { get; set; }
        public virtual DbSet<tblPromoM> tblPromoMs { get; set; }
        public virtual DbSet<tblPromotionalHotelsM> tblPromotionalHotelsMs { get; set; }
        public virtual DbSet<tblPropConserveCommissionMap> tblPropConserveCommissionMaps { get; set; }
        public virtual DbSet<tblPropertyAmenitiesMap> tblPropertyAmenitiesMaps { get; set; }
        public virtual DbSet<tblPropertyAreaDataTG> tblPropertyAreaDataTGs { get; set; }
        public virtual DbSet<tblPropertyBasicBiddingMap> tblPropertyBasicBiddingMaps { get; set; }
        public virtual DbSet<tblPropertyBiddingRateM> tblPropertyBiddingRateMs { get; set; }
        public virtual DbSet<tblPropertyBidUpgradeM> tblPropertyBidUpgradeMs { get; set; }
        public virtual DbSet<tblPropertyCancellationPolicyMap> tblPropertyCancellationPolicyMaps { get; set; }
        public virtual DbSet<tblPropertyChannelMgrMap> tblPropertyChannelMgrMaps { get; set; }
        public virtual DbSet<tblPropertyCompetitorSet> tblPropertyCompetitorSets { get; set; }
        public virtual DbSet<tblPropertyDiningMap> tblPropertyDiningMaps { get; set; }
        public virtual DbSet<tblPropertyFacilityMap> tblPropertyFacilityMaps { get; set; }
        public virtual DbSet<tblPropertyFacilityTG> tblPropertyFacilityTGs { get; set; }
        public virtual DbSet<tblPropertyFacilityTG_Tmp> tblPropertyFacilityTG_Tmp { get; set; }
        public virtual DbSet<tblPropertyImageUrlTG> tblPropertyImageUrlTGs { get; set; }
        public virtual DbSet<tblPropertyImageUrlTG_Tmp> tblPropertyImageUrlTG_Tmp { get; set; }
        public virtual DbSet<tblPropertyLeadTimeBiddingMap> tblPropertyLeadTimeBiddingMaps { get; set; }
        public virtual DbSet<tblPropertyLocalityMap> tblPropertyLocalityMaps { get; set; }
        public virtual DbSet<tblPropertyLOSBiddingMap> tblPropertyLOSBiddingMaps { get; set; }
        public virtual DbSet<tblPropertyM> tblPropertyMs { get; set; }
        public virtual DbSet<tblPropertyMTest> tblPropertyMTests { get; set; }
        public virtual DbSet<tblPropertyMTG_Tmp> tblPropertyMTG_Tmp { get; set; }
        public virtual DbSet<tblPropertyParkingMap> tblPropertyParkingMaps { get; set; }
        public virtual DbSet<tblPropertyPhotoMap> tblPropertyPhotoMaps { get; set; }
        public virtual DbSet<tblPropertyPolicyMap> tblPropertyPolicyMaps { get; set; }
        public virtual DbSet<tblPropertyPrimaryPhotoMap> tblPropertyPrimaryPhotoMaps { get; set; }
        public virtual DbSet<tblPropertyPromoMap> tblPropertyPromoMaps { get; set; }
        public virtual DbSet<tblPropertyPromoRateMap> tblPropertyPromoRateMaps { get; set; }
        public virtual DbSet<tblPropertyPromotionAmenityMap> tblPropertyPromotionAmenityMaps { get; set; }
        public virtual DbSet<tblPropertyPromotionCancellationMainMap> tblPropertyPromotionCancellationMainMaps { get; set; }
        public virtual DbSet<tblPropertyPromotionCancellationMap> tblPropertyPromotionCancellationMaps { get; set; }
        public virtual DbSet<tblPropertyPromotionMap> tblPropertyPromotionMaps { get; set; }
        public virtual DbSet<tblPropertyPromotionRoomTypeMap> tblPropertyPromotionRoomTypeMaps { get; set; }
        public virtual DbSet<tblPropertyRatePlanAmenityMap> tblPropertyRatePlanAmenityMaps { get; set; }
        public virtual DbSet<tblPropertyRatePlanCancellationMainMap> tblPropertyRatePlanCancellationMainMaps { get; set; }
        public virtual DbSet<tblPropertyRatePlanCancellationMap> tblPropertyRatePlanCancellationMaps { get; set; }
        public virtual DbSet<tblPropertyRatePlanMap> tblPropertyRatePlanMaps { get; set; }
        public virtual DbSet<tblPropertyRatePlanRoomTypeMap> tblPropertyRatePlanRoomTypeMaps { get; set; }
        public virtual DbSet<tblPropertyRecreationMap> tblPropertyRecreationMaps { get; set; }
        public virtual DbSet<tblPropertyReviewTG> tblPropertyReviewTGs { get; set; }
        public virtual DbSet<tblPropertyReviewTG_Tmp> tblPropertyReviewTG_Tmp { get; set; }
        public virtual DbSet<tblPropertyRoomAmentiesMap> tblPropertyRoomAmentiesMaps { get; set; }
        public virtual DbSet<tblPropertyRoomDescriptionTG> tblPropertyRoomDescriptionTGs { get; set; }
        public virtual DbSet<tblPropertyRoomDescriptionTG_Tmp> tblPropertyRoomDescriptionTG_Tmp { get; set; }
        public virtual DbSet<tblPropertyRoomInventory> tblPropertyRoomInventories { get; set; }
        public virtual DbSet<tblPropertyRoomLevelAmenityTG> tblPropertyRoomLevelAmenityTGs { get; set; }
        public virtual DbSet<tblPropertyRoomLevelAmenityTG_Tmp> tblPropertyRoomLevelAmenityTG_Tmp { get; set; }
        public virtual DbSet<tblPropertyRoomMap> tblPropertyRoomMaps { get; set; }
        public virtual DbSet<tblPropertyRoomRatePlanInventoryMap> tblPropertyRoomRatePlanInventoryMaps { get; set; }
        public virtual DbSet<tblPropertyRoomsBiddingMap> tblPropertyRoomsBiddingMaps { get; set; }
        public virtual DbSet<tblPropertyRoomTaxMap> tblPropertyRoomTaxMaps { get; set; }
        public virtual DbSet<tblPropertyRoomTypeRoomAmentiesMap> tblPropertyRoomTypeRoomAmentiesMaps { get; set; }
        public virtual DbSet<tblPropertySpaMap> tblPropertySpaMaps { get; set; }
        public virtual DbSet<tblPropertyTaxesMap> tblPropertyTaxesMaps { get; set; }
        public virtual DbSet<tblPropertyTaxMap> tblPropertyTaxMaps { get; set; }
        public virtual DbSet<tblPropertyTopAttrationTG> tblPropertyTopAttrationTGs { get; set; }
        public virtual DbSet<tblPropertyTopAttrationTG_Tmp> tblPropertyTopAttrationTG_Tmp { get; set; }
        public virtual DbSet<tblPropertyTripAdvisorM> tblPropertyTripAdvisorMs { get; set; }
        public virtual DbSet<tblPropertyWeekendBiddingMap> tblPropertyWeekendBiddingMaps { get; set; }
        public virtual DbSet<tblPurchaseHistory> tblPurchaseHistories { get; set; }
        public virtual DbSet<tblRankManagement> tblRankManagements { get; set; }
        public virtual DbSet<tblRatePlanM> tblRatePlanMs { get; set; }
        public virtual DbSet<tblRecentSavingsTx> tblRecentSavingsTxes { get; set; }
        public virtual DbSet<tblRoomFacilityDropDownM> tblRoomFacilityDropDownMs { get; set; }
        public virtual DbSet<tblRoomOutdoorViewM> tblRoomOutdoorViewMs { get; set; }
        public virtual DbSet<tblRoomTypeM> tblRoomTypeMs { get; set; }
        public virtual DbSet<tblSettingM> tblSettingMs { get; set; }
        public virtual DbSet<tblSettingsM> tblSettingsMs { get; set; }
        public virtual DbSet<tblStarCategoryM> tblStarCategoryMs { get; set; }
        public virtual DbSet<tblStateM> tblStateMs { get; set; }
        public virtual DbSet<tblTaxM> tblTaxMs { get; set; }
        public virtual DbSet<tblTGAmenitiesMap> tblTGAmenitiesMaps { get; set; }
        public virtual DbSet<tblTopSellingHotelsForTag> tblTopSellingHotelsForTags { get; set; }
        public virtual DbSet<tblTripAdvisorReview> tblTripAdvisorReviews { get; set; }
        public virtual DbSet<tblUserGroupM> tblUserGroupMs { get; set; }
        public virtual DbSet<tblUserM> tblUserMs { get; set; }
        public virtual DbSet<tblUserPropertyMap> tblUserPropertyMaps { get; set; }
        public virtual DbSet<tblVideoUrlM> tblVideoUrlMs { get; set; }
        public virtual DbSet<tblWebsiteGuestUserMaster> tblWebsiteGuestUserMasters { get; set; }
        public virtual DbSet<tblWebsiteUserClaim> tblWebsiteUserClaims { get; set; }
        public virtual DbSet<tblWebsiteUserLogin> tblWebsiteUserLogins { get; set; }
        public virtual DbSet<tblWebsiteUserMater> tblWebsiteUserMaters { get; set; }
        public virtual DbSet<tblWebsiteUserRoleMaster> tblWebsiteUserRoleMasters { get; set; }
        public virtual DbSet<tblAdditionalCommision_log> tblAdditionalCommision_log { get; set; }
        public virtual DbSet<tblAmenityM_log> tblAmenityM_log { get; set; }
        public virtual DbSet<tblAnonymousRecentViewsTx> tblAnonymousRecentViewsTxes { get; set; }
        public virtual DbSet<tblBankDetailM_log> tblBankDetailM_log { get; set; }
        public virtual DbSet<tblBannerM_log> tblBannerM_log { get; set; }
        public virtual DbSet<tblBasicPropertyAmenitiesTGSpecific_Tmp> tblBasicPropertyAmenitiesTGSpecific_Tmp { get; set; }
        public virtual DbSet<tblBasicPropertyImgTGSpecific_Tmp> tblBasicPropertyImgTGSpecific_Tmp { get; set; }
        public virtual DbSet<tblBasicPropertyInfoTGSearch_Tmp> tblBasicPropertyInfoTGSearch_Tmp { get; set; }
        public virtual DbSet<tblBasicPropertyInfoTGSpecific_Tmp> tblBasicPropertyInfoTGSpecific_Tmp { get; set; }
        public virtual DbSet<tblBasicPropertyPOITGSpecific_Tmp> tblBasicPropertyPOITGSpecific_Tmp { get; set; }
        public virtual DbSet<tblBookingTx_log> tblBookingTx_log { get; set; }
        public virtual DbSet<tblChainM_log> tblChainM_log { get; set; }
        public virtual DbSet<tblChannelMgrBookingTracker> tblChannelMgrBookingTrackers { get; set; }
        public virtual DbSet<tblChannelMgrTracker> tblChannelMgrTrackers { get; set; }
        public virtual DbSet<tblGroupM_log> tblGroupM_log { get; set; }
        public virtual DbSet<tblGroupMenuM_log> tblGroupMenuM_log { get; set; }
        public virtual DbSet<tblLocalityM_log> tblLocalityM_log { get; set; }
        public virtual DbSet<tblLoyalityAmenityMap_log> tblLoyalityAmenityMap_log { get; set; }
        public virtual DbSet<tblLoyalityPointsM_log> tblLoyalityPointsM_log { get; set; }
        public virtual DbSet<tblMacroAreaM_log> tblMacroAreaM_log { get; set; }
        public virtual DbSet<tblPolygon> tblPolygons { get; set; }
        public virtual DbSet<tblPromoCodeM_log> tblPromoCodeM_log { get; set; }
        public virtual DbSet<tblPromotionalHotelsM_log> tblPromotionalHotelsM_log { get; set; }
        public virtual DbSet<tblPropertyAmenitiesMap_log> tblPropertyAmenitiesMap_log { get; set; }
        public virtual DbSet<tblPropertyCompetitorSet_log> tblPropertyCompetitorSet_log { get; set; }
        public virtual DbSet<tblPropertyDiningMap_log> tblPropertyDiningMap_log { get; set; }
        public virtual DbSet<tblPropertyLocalityMap_log> tblPropertyLocalityMap_log { get; set; }
        public virtual DbSet<tblPropertyM_log> tblPropertyM_log { get; set; }
        public virtual DbSet<tblPropertyParkingMap_log> tblPropertyParkingMap_log { get; set; }
        public virtual DbSet<tblPropertyPhotoMap_log> tblPropertyPhotoMap_log { get; set; }
        public virtual DbSet<tblPropertyPolicyMap_log> tblPropertyPolicyMap_log { get; set; }
        public virtual DbSet<tblPropertyPromoRateMap_log> tblPropertyPromoRateMap_log { get; set; }
        public virtual DbSet<tblPropertyPromotionCancellationMainMap_log> tblPropertyPromotionCancellationMainMap_log { get; set; }
        public virtual DbSet<tblPropertyPromotionMap_log> tblPropertyPromotionMap_log { get; set; }
        public virtual DbSet<tblPropertyRatePlanCancellationMainMap_log> tblPropertyRatePlanCancellationMainMap_log { get; set; }
        public virtual DbSet<tblPropertyRatePlanMap_log> tblPropertyRatePlanMap_log { get; set; }
        public virtual DbSet<tblPropertyRatePlanRoomTypeMap_log> tblPropertyRatePlanRoomTypeMap_log { get; set; }
        public virtual DbSet<tblPropertyRecreationMap_log> tblPropertyRecreationMap_log { get; set; }
        public virtual DbSet<tblPropertyRoomInventory_log> tblPropertyRoomInventory_log { get; set; }
        public virtual DbSet<tblPropertyRoomMap_log> tblPropertyRoomMap_log { get; set; }
        public virtual DbSet<tblPropertyRoomRatePlanInventoryMap_log> tblPropertyRoomRatePlanInventoryMap_log { get; set; }
        public virtual DbSet<tblPropertySpaMap_log> tblPropertySpaMap_log { get; set; }
        public virtual DbSet<tblPropertyTaxesMap_log> tblPropertyTaxesMap_log { get; set; }
        public virtual DbSet<tblPropertyTaxMap_log> tblPropertyTaxMap_log { get; set; }
        public virtual DbSet<tblRankManagement_log> tblRankManagement_log { get; set; }
        public virtual DbSet<tblRatePlanCancelPenaltyDescTGSearch_Tmp> tblRatePlanCancelPenaltyDescTGSearch_Tmp { get; set; }
        public virtual DbSet<tblRatePlanCancelPenaltyDescTGSpecific_Tmp> tblRatePlanCancelPenaltyDescTGSpecific_Tmp { get; set; }
        public virtual DbSet<tblRatePlanCancelPenaltyTGSearch_Tmp> tblRatePlanCancelPenaltyTGSearch_Tmp { get; set; }
        public virtual DbSet<tblRatePlanCancelPenaltyTGSpecific_Tmp> tblRatePlanCancelPenaltyTGSpecific_Tmp { get; set; }
        public virtual DbSet<tblRatePlanM_log> tblRatePlanM_log { get; set; }
        public virtual DbSet<tblRatePlanTGSearch_Tmp> tblRatePlanTGSearch_Tmp { get; set; }
        public virtual DbSet<tblRatePlanTGSpecific_Tmp> tblRatePlanTGSpecific_Tmp { get; set; }
        public virtual DbSet<tblRecentViewsTx> tblRecentViewsTxes { get; set; }
        public virtual DbSet<tblRoomAccessibilityM> tblRoomAccessibilityMs { get; set; }
        public virtual DbSet<tblRoomRateTaxTGSpecific_Tmp> tblRoomRateTaxTGSpecific_Tmp { get; set; }
        public virtual DbSet<tblRoomRateTGsearch_Tmp> tblRoomRateTGsearch_Tmp { get; set; }
        public virtual DbSet<tblRoomRateTGSpecific_Tmp> tblRoomRateTGSpecific_Tmp { get; set; }
        public virtual DbSet<tblRoomTypeTGSearch_Tmp> tblRoomTypeTGSearch_Tmp { get; set; }
        public virtual DbSet<tblRoomTypeTGSpecific_Tmp> tblRoomTypeTGSpecific_Tmp { get; set; }
        public virtual DbSet<tblSyncBookingToChannelMgrTracker> tblSyncBookingToChannelMgrTrackers { get; set; }
        public virtual DbSet<tblTaxM_log> tblTaxM_log { get; set; }
        public virtual DbSet<tblUserM_log> tblUserM_log { get; set; }
        public virtual DbSet<tblUserPropertyMap_log> tblUserPropertyMap_log { get; set; }
        public virtual DbSet<tblVideoUrlM_log> tblVideoUrlM_log { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IP2LOCATION>()
                .Property(e => e.country_code)
                .IsUnicode(false);

            modelBuilder.Entity<IP2LOCATION>()
                .Property(e => e.country_name)
                .IsUnicode(false);

            modelBuilder.Entity<IP2LOCATION>()
                .Property(e => e.city)
                .IsUnicode(false);

            modelBuilder.Entity<IP2LOCATION>()
                .Property(e => e.countryoffset)
                .IsUnicode(false);

            modelBuilder.Entity<tblAccessbilityM>()
                .Property(e => e.sAccessibility)
                .IsUnicode(false);

            modelBuilder.Entity<tblAccomodationM>()
                .Property(e => e.sAccomodation)
                .IsUnicode(false);

            modelBuilder.Entity<tblAdditionalCommision>()
                .Property(e => e.dCommission)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblAffiliationM>()
                .Property(e => e.sAffiliation)
                .IsUnicode(false);

            modelBuilder.Entity<tblAmenityHeaderMapTG>()
                .Property(e => e.sHeader)
                .IsUnicode(false);

            modelBuilder.Entity<tblAmenityHeaderMapTG>()
                .Property(e => e.sAmenity)
                .IsUnicode(false);

            modelBuilder.Entity<tblAmenityM>()
                .Property(e => e.sAmenityName)
                .IsUnicode(false);

            modelBuilder.Entity<tblAmenityM>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblAmenityM>()
                .HasMany(e => e.tblLoyalityAmenityMaps)
                .WithRequired(e => e.tblAmenityM)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblAmenityM>()
                .HasMany(e => e.tblPropertyWeekendBiddingMaps)
                .WithOptional(e => e.tblAmenityM)
                .HasForeignKey(e => e.iAmenityId1);

            modelBuilder.Entity<tblAmenityM>()
                .HasMany(e => e.tblPropertyWeekendBiddingMaps1)
                .WithOptional(e => e.tblAmenityM1)
                .HasForeignKey(e => e.iAmenityId2);

            modelBuilder.Entity<tblAmenityM>()
                .HasMany(e => e.tblPropertyLeadTimeBiddingMaps)
                .WithOptional(e => e.tblAmenityM)
                .HasForeignKey(e => e.iAmenityId1);

            modelBuilder.Entity<tblAmenityM>()
                .HasMany(e => e.tblPropertyLeadTimeBiddingMaps1)
                .WithOptional(e => e.tblAmenityM1)
                .HasForeignKey(e => e.iAmenityId2);

            modelBuilder.Entity<tblAmenityM>()
                .HasMany(e => e.tblPropertyLOSBiddingMaps)
                .WithOptional(e => e.tblAmenityM)
                .HasForeignKey(e => e.iAmenityId1);

            modelBuilder.Entity<tblAmenityM>()
                .HasMany(e => e.tblPropertyLOSBiddingMaps1)
                .WithOptional(e => e.tblAmenityM1)
                .HasForeignKey(e => e.iAmenityId2);

            modelBuilder.Entity<tblAmenityM>()
                .HasMany(e => e.tblPropertyRoomsBiddingMaps)
                .WithOptional(e => e.tblAmenityM)
                .HasForeignKey(e => e.iAmenityId1);

            modelBuilder.Entity<tblAmenityM>()
                .HasMany(e => e.tblPropertyRoomsBiddingMaps1)
                .WithOptional(e => e.tblAmenityM1)
                .HasForeignKey(e => e.iAmenityId2);

            modelBuilder.Entity<tblApplicabilityM>()
                .Property(e => e.sApplicability)
                .IsUnicode(false);

            modelBuilder.Entity<tblApplicabilityM>()
                .HasMany(e => e.tblPropertyWeekendBiddingMaps)
                .WithOptional(e => e.tblApplicabilityM)
                .HasForeignKey(e => e.iApplicabilityId1);

            modelBuilder.Entity<tblApplicabilityM>()
                .HasMany(e => e.tblPropertyWeekendBiddingMaps1)
                .WithOptional(e => e.tblApplicabilityM1)
                .HasForeignKey(e => e.iApplicabilityId2);

            modelBuilder.Entity<tblApplicabilityM>()
                .HasMany(e => e.tblPropertyLeadTimeBiddingMaps)
                .WithOptional(e => e.tblApplicabilityM)
                .HasForeignKey(e => e.iApplicabilityId1);

            modelBuilder.Entity<tblApplicabilityM>()
                .HasMany(e => e.tblPropertyLeadTimeBiddingMaps1)
                .WithOptional(e => e.tblApplicabilityM1)
                .HasForeignKey(e => e.iApplicabilityId2);

            modelBuilder.Entity<tblApplicabilityM>()
                .HasMany(e => e.tblPropertyLOSBiddingMaps)
                .WithOptional(e => e.tblApplicabilityM)
                .HasForeignKey(e => e.iApplicabilityId1);

            modelBuilder.Entity<tblApplicabilityM>()
                .HasMany(e => e.tblPropertyLOSBiddingMaps1)
                .WithOptional(e => e.tblApplicabilityM1)
                .HasForeignKey(e => e.iApplicabilityId2);

            modelBuilder.Entity<tblApplicabilityM>()
                .HasMany(e => e.tblPropertyRoomsBiddingMaps)
                .WithOptional(e => e.tblApplicabilityM)
                .HasForeignKey(e => e.iApplicabilityId1);

            modelBuilder.Entity<tblApplicabilityM>()
                .HasMany(e => e.tblPropertyRoomsBiddingMaps1)
                .WithOptional(e => e.tblApplicabilityM1)
                .HasForeignKey(e => e.iApplicabilityId2);

            modelBuilder.Entity<tblAwardM>()
                .Property(e => e.sAward)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM>()
                .Property(e => e.sBankAccountNo)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM>()
                .Property(e => e.sBaneficiariesName)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM>()
                .Property(e => e.sBankName)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM>()
                .Property(e => e.sBranchName)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM>()
                .Property(e => e.sBranchAddress)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM>()
                .Property(e => e.sMicrCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM>()
                .Property(e => e.sIfscCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM>()
                .Property(e => e.sLetterhead_BankAccount)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM>()
                .Property(e => e.sCancelledCheque)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM>()
                .Property(e => e.sPanCard)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM>()
                .Property(e => e.dCommission)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblBankDetailM>()
                .Property(e => e.sFName)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM>()
                .Property(e => e.sFPhoneNo)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM>()
                .Property(e => e.sFFaxNo)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM>()
                .Property(e => e.sFEmailId)
                .IsUnicode(false);

            modelBuilder.Entity<tblBannerM>()
                .Property(e => e.sBannerName)
                .IsUnicode(false);

            modelBuilder.Entity<tblBannerM>()
                .Property(e => e.sBannerType)
                .IsUnicode(false);

            modelBuilder.Entity<tblBannerM>()
                .Property(e => e.sTextPosition)
                .IsUnicode(false);

            modelBuilder.Entity<tblBannerM>()
                .Property(e => e.cstatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBannerM>()
                .Property(e => e.sLinkId)
                .IsUnicode(false);

            modelBuilder.Entity<tblBannerM>()
                .Property(e => e.sImgUrl)
                .IsUnicode(false);

            modelBuilder.Entity<tblBannerM>()
                .Property(e => e.UniqueAzureFileName)
                .IsUnicode(false);

            modelBuilder.Entity<tblBidCountsTx>()
                .Property(e => e.sMobile)
                .IsUnicode(false);

            modelBuilder.Entity<tblBIDRoomAdultsTx>()
                .Property(e => e.sChildAge)
                .IsUnicode(false);

            modelBuilder.Entity<tblBidSettingsM>()
                .Property(e => e.dMinBidAmountForCustomerAllowed)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblBidSettingsM>()
                .Property(e => e.dMinBidAmountDiffPer)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblBidSettingsM>()
                .Property(e => e.dMaxBidAmountDiffPer)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblBidSettingsM>()
                .Property(e => e.dOFRServiceCharge)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblBidSettingsM>()
                .Property(e => e.dOFRMinTotalEarning)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblBookedBidAmenity>()
                .Property(e => e.sAmenity)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookedBidAmenity>()
                .Property(e => e.sApplicability)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookedBidAmenity>()
                .Property(e => e.sType)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookedDayWiseTaxAmountDetail>()
                .Property(e => e.dTaxPerc)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.iCountryOffset)
                .HasPrecision(9, 2);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.sCountryTimeZone)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.cBookingType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.sPromoCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.bPromoAmenity)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.sTitleOFR)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.sFirstNameOFR)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.sLastNameOFR)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.sEmailOFR)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.sMobileOFR)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.BookingStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.PaymentStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.ActualBookingType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.dTotalNegotiateAmount)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.dAdvNegotiateAmount)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.dBidAmount)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.dTotalAmount)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.dTotalComm)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.dTaxes)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.dTaxesForHotel)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.dTotalExtraBedAmount)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.dDiscountedBidPrice)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.dCustomerPayable)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.sSpecialRequests)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.sExpectedCheckIn)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.sSpecialOccasion)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.spref_single_lady)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.spref_away_elevator)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.spref_smoking_room)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.spref_quiet_room)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.spref_pickup)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.spref_extra1)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.spref_extra2)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.spref_extra3)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.sFlightNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.sEstArrivalTime)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.sLandingAt)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.sTypeOfCar)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.iCounterOffer)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.sCurrencyCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.sExtra1)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.dccPrice)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.dccDiscount)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.dSafeguardComm)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.ccType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.dTotalCommOriginal)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.dTaxesOriginal)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.sBookingIdTG)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.sTrxnTypeTG)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.cSyncStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.sBidType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.sIDs)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.dRefundAmount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.cRefundStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .Property(e => e.cAmendStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .HasMany(e => e.tblBookingCancellationPolicyAmendMaps)
                .WithRequired(e => e.tblBookingAmedTx)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblBookingAmedTx>()
                .HasMany(e => e.tblBookingDetailsAmendTxes)
                .WithRequired(e => e.tblBookingAmedTx)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblBookingCancellationPolicyAmendMap>()
                .Property(e => e.sPolicyName)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingCancellationPolicyMap>()
                .Property(e => e.sPolicyName)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingDetailsAmendTx>()
                .Property(e => e.iRoomId)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingDetailsAmendTx>()
                .Property(e => e.iRPId)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingDetailsAmendTx>()
                .Property(e => e.sRoomName)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingDetailsAmendTx>()
                .Property(e => e.sRPName)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingDetailsAmendTx>()
                .Property(e => e.dRoomRate)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingDetailsAmendTx>()
                .Property(e => e.dExtraBedRate)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingDetailsAmendTx>()
                .Property(e => e.dRoomDiscountedRate)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingDetailsAmendTx>()
                .Property(e => e.dTaxes)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingDetailsAmendTx>()
                .Property(e => e.dTaxesForHotel)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingDetailsAmendTx>()
                .Property(e => e.dBasicDiscountBid)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingDetailsAmendTx>()
                .Property(e => e.dLOSDiscountBid)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingDetailsAmendTx>()
                .Property(e => e.dLeadDiscountBid)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingDetailsAmendTx>()
                .Property(e => e.dWWDiscountBid)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingDetailsAmendTx>()
                .Property(e => e.dMRDiscountBid)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingDetailsAmendTx>()
                .Property(e => e.sAmenityRatePlan)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingDetailsAmendTx>()
                .Property(e => e.dUpgradeCharge)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingDetailsAmendTx>()
                .Property(e => e.sChildrenAge)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingDetailsTx>()
                .Property(e => e.iRoomId)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingDetailsTx>()
                .Property(e => e.iRPId)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingDetailsTx>()
                .Property(e => e.sRoomName)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingDetailsTx>()
                .Property(e => e.sRPName)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingDetailsTx>()
                .Property(e => e.dRoomRate)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingDetailsTx>()
                .Property(e => e.dExtraBedRate)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingDetailsTx>()
                .Property(e => e.dRoomDiscountedRate)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingDetailsTx>()
                .Property(e => e.dTaxes)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingDetailsTx>()
                .Property(e => e.dTaxesForHotel)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingDetailsTx>()
                .Property(e => e.dBasicDiscountBid)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingDetailsTx>()
                .Property(e => e.dLOSDiscountBid)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingDetailsTx>()
                .Property(e => e.dLeadDiscountBid)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingDetailsTx>()
                .Property(e => e.dWWDiscountBid)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingDetailsTx>()
                .Property(e => e.dMRDiscountBid)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingDetailsTx>()
                .Property(e => e.sAmenityRatePlan)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingDetailsTx>()
                .Property(e => e.dUpgradeCharge)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingDetailsTx>()
                .Property(e => e.sChildrenAge)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingDetailsTx>()
                .HasMany(e => e.tblBookingGuestMaps)
                .WithRequired(e => e.tblBookingDetailsTx)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblBookingGuestAmendMap>()
                .Property(e => e.sGuestName)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingGuestAmendMap>()
                .Property(e => e.sGuestEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingGuestAmendMap>()
                .Property(e => e.sGuestMobile)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingGuestAmendMap>()
                .Property(e => e.sBedPreference)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingGuestMap>()
                .Property(e => e.sGuestName)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingGuestMap>()
                .Property(e => e.sGuestEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingGuestMap>()
                .Property(e => e.sGuestMobile)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingGuestMap>()
                .Property(e => e.sBedPreference)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingGuestMap>()
                .HasOptional(e => e.tblBookingGuestMap1)
                .WithRequired(e => e.tblBookingGuestMap2);

            modelBuilder.Entity<tblBookingNegotiationTx>()
                .Property(e => e.dTotalNegotiateAmount)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingNegotiationTx>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTrakerTx>()
                .Property(e => e.BookingStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.iCountryOffset)
                .HasPrecision(9, 2);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.sCountryTimeZone)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.cBookingType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.sPromoCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.bPromoAmenity)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.sTitleOFR)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.sFirstNameOFR)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.sLastNameOFR)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.sEmailOFR)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.sMobileOFR)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.BookingStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.PaymentStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.ActualBookingType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.dTotalNegotiateAmount)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.dAdvNegotiateAmount)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.dBidAmount)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.dTotalAmount)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.dTotalComm)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.dTaxes)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.dTaxesForHotel)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.dTotalExtraBedAmount)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.dDiscountedBidPrice)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.dCustomerPayable)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.sSpecialRequests)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.sExpectedCheckIn)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.sSpecialOccasion)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.spref_single_lady)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.spref_away_elevator)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.spref_smoking_room)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.spref_quiet_room)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.spref_pickup)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.spref_extra1)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.spref_extra2)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.spref_extra3)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.sFlightNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.sEstArrivalTime)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.sLandingAt)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.sTypeOfCar)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.iCounterOffer)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.sCurrencyCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.sExtra1)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.dccPrice)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.dccDiscount)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.dSafeguardComm)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.ccType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.dTotalCommOriginal)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.dTaxesOriginal)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.sBookingIdTG)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.sTrxnTypeTG)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.cSyncStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.sBidType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.sIDs)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.dRefundAmount)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.cRefundStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .Property(e => e.cAmendStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx>()
                .HasMany(e => e.tblBookedBidAmenities)
                .WithRequired(e => e.tblBookingTx)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblBookingTx>()
                .HasMany(e => e.tblBookingAmedTxes)
                .WithRequired(e => e.tblBookingTx)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblBookingTx>()
                .HasMany(e => e.tblBookingDetailsAmendTxes)
                .WithRequired(e => e.tblBookingTx)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblBookingTx>()
                .HasOptional(e => e.tblSyncBookingToChannelMgrTracker)
                .WithRequired(e => e.tblBookingTx);

            modelBuilder.Entity<tblBuildingCharacteristicsM>()
                .Property(e => e.sBuildingCharacteristics)
                .IsUnicode(false);

            modelBuilder.Entity<tblCarouselM>()
                .Property(e => e.sImage)
                .IsUnicode(false);

            modelBuilder.Entity<tblChainM>()
                .Property(e => e.sChainName)
                .IsUnicode(false);

            modelBuilder.Entity<tblChainM>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblChainMTG>()
                .Property(e => e.iChainID)
                .IsUnicode(false);

            modelBuilder.Entity<tblChainMTG>()
                .Property(e => e.sChainName)
                .IsUnicode(false);

            modelBuilder.Entity<tblChainMTG>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblChainMTG>()
                .Property(e => e.sChainGroupId)
                .IsUnicode(false);

            modelBuilder.Entity<tblChainMTG_Tmp>()
                .Property(e => e.iChainID)
                .IsUnicode(false);

            modelBuilder.Entity<tblChainMTG_Tmp>()
                .Property(e => e.sChainName)
                .IsUnicode(false);

            modelBuilder.Entity<tblChainMTG_Tmp>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblChainMTG_Tmp>()
                .Property(e => e.sChainGroupId)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistory>()
                .Property(e => e.cType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistory>()
                .Property(e => e.sItem)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistory>()
                .Property(e => e.sOld)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistory>()
                .Property(e => e.sNew)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryBankAdditionalCommision>()
                .Property(e => e.sItem)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryBankAdditionalCommision>()
                .Property(e => e.sOld)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryBankAdditionalCommision>()
                .Property(e => e.sNew)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryBankDetail>()
                .Property(e => e.sItem)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryBankDetail>()
                .Property(e => e.sOld)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryBankDetail>()
                .Property(e => e.sNew)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryBasicDetailsProperty>()
                .Property(e => e.sItem)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryBasicDetailsProperty>()
                .Property(e => e.sOld)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryBasicDetailsProperty>()
                .Property(e => e.sNew)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryCompetitorSet>()
                .Property(e => e.sItem)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryCompetitorSet>()
                .Property(e => e.sOld)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryCompetitorSet>()
                .Property(e => e.sNew)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryDining>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryDining>()
                .Property(e => e.sItem)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryDining>()
                .Property(e => e.sOld)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryDining>()
                .Property(e => e.sNew)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryPhoto>()
                .Property(e => e.sImgUrl)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryPhoto>()
                .Property(e => e.sItem)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryPhoto>()
                .Property(e => e.sOld)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryPhoto>()
                .Property(e => e.sNew)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryPIRoom>()
                .Property(e => e.sItem)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryPIRoom>()
                .Property(e => e.sOld)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryPIRoom>()
                .Property(e => e.sNew)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryPolicy>()
                .Property(e => e.sItem)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryPolicy>()
                .Property(e => e.sOld)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryPolicy>()
                .Property(e => e.sNew)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryPropertyAmenty>()
                .Property(e => e.sItem)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryPropertyAmenty>()
                .Property(e => e.sOld)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryPropertyAmenty>()
                .Property(e => e.sNew)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryPropertyPromotionMap>()
                .Property(e => e.sItem)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryPropertyPromotionMap>()
                .Property(e => e.sOld)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryPropertyPromotionMap>()
                .Property(e => e.sNew)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryPropertyRatePlanMap>()
                .Property(e => e.sItem)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryPropertyRatePlanMap>()
                .Property(e => e.sOld)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryPropertyRatePlanMap>()
                .Property(e => e.sNew)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryRate>()
                .Property(e => e.sItem)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryRate>()
                .Property(e => e.sOld)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryRate>()
                .Property(e => e.sNew)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryRecreation>()
                .Property(e => e.sItem)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryRecreation>()
                .Property(e => e.sOld)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryRecreation>()
                .Property(e => e.sNew)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryRoom>()
                .Property(e => e.sItem)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryRoom>()
                .Property(e => e.sOld)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryRoom>()
                .Property(e => e.sNew)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistorySpa>()
                .Property(e => e.sItem)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistorySpa>()
                .Property(e => e.sOld)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistorySpa>()
                .Property(e => e.sNew)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryTax>()
                .Property(e => e.sItem)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryTax>()
                .Property(e => e.sOld)
                .IsUnicode(false);

            modelBuilder.Entity<tblChangeHistoryTax>()
                .Property(e => e.sNew)
                .IsUnicode(false);

            modelBuilder.Entity<tblChannelMgrBookingTx>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblChannelMgrBookingTx>()
                .Property(e => e.sCMDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblChannelMgrBookingTx>()
                .Property(e => e.sCMStatus)
                .IsUnicode(false);

            modelBuilder.Entity<tblChannelMgrBookingTx>()
                .Property(e => e.sErrorType)
                .IsUnicode(false);

            modelBuilder.Entity<tblChannelMgrBookingTx>()
                .Property(e => e.sErrorCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblChannelMgrBookingTx>()
                .HasOptional(e => e.tblChannelMgrBookingTracker)
                .WithRequired(e => e.tblChannelMgrBookingTx);

            modelBuilder.Entity<tblChannelMgrM>()
                .Property(e => e.sChannelMgrName)
                .IsUnicode(false);

            modelBuilder.Entity<tblCityM>()
                .Property(e => e.sCity)
                .IsUnicode(false);

            modelBuilder.Entity<tblCityM>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblConserveCommissionM>()
                .Property(e => e.dCommission)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblConserveCommissionM>()
                .Property(e => e.dCounterTrigger)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblConserveCommissionM>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblCountryM>()
                .Property(e => e.sCountry)
                .IsUnicode(false);

            modelBuilder.Entity<tblCountryM>()
                .Property(e => e.sCountryCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblCountryM>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblCountryM>()
                .Property(e => e.sCountryCurrencyCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblCountryM>()
                .HasMany(e => e.tblCityMs)
                .WithRequired(e => e.tblCountryM)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblCountryM>()
                .HasMany(e => e.tblStateMs)
                .WithRequired(e => e.tblCountryM)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblCrediCardM>()
                .Property(e => e.sCreditCard)
                .IsUnicode(false);

            modelBuilder.Entity<tblCrediCardM>()
                .Property(e => e.sImg)
                .IsUnicode(false);

            modelBuilder.Entity<tblCurrencyM>()
                .Property(e => e.sCurrencyCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblCurrencyM>()
                .Property(e => e.sCurrency)
                .IsUnicode(false);

            modelBuilder.Entity<tblCurrencyM>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblCurrencyM>()
                .Property(e => e.sImg)
                .IsUnicode(false);

            modelBuilder.Entity<tblCurrencyM>()
                .Property(e => e.sCountryCurrencyCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblCustomerM>()
                .Property(e => e.sUserName)
                .IsUnicode(false);

            modelBuilder.Entity<tblCustomerM>()
                .Property(e => e.sPassword)
                .IsUnicode(false);

            modelBuilder.Entity<tblCustomerM>()
                .Property(e => e.sGoogleToken)
                .IsUnicode(false);

            modelBuilder.Entity<tblCustomerM>()
                .Property(e => e.sFacebookToken)
                .IsUnicode(false);

            modelBuilder.Entity<tblCustomerM>()
                .Property(e => e.sName)
                .IsUnicode(false);

            modelBuilder.Entity<tblCustomerM>()
                .Property(e => e.sEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblCustomerM>()
                .Property(e => e.sPhone)
                .IsUnicode(false);

            modelBuilder.Entity<tblCustomerM>()
                .Property(e => e.sBlockReason)
                .IsUnicode(false);

            modelBuilder.Entity<tblCustomerPointsHistoryTx>()
                .Property(e => e.cType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblCustomerPointsMap>()
                .Property(e => e.cType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblCustomerPointsMap>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblDaywiseBookingRateTx>()
                .Property(e => e.dRoomRate)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblDaywiseBookingRateTx>()
                .Property(e => e.dTaxes)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblEmailSettingsM>()
                .Property(e => e.sModule)
                .IsUnicode(false);

            modelBuilder.Entity<tblEmailSettingsM>()
                .Property(e => e.sEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblErrorLog>()
                .Property(e => e.sController)
                .IsUnicode(false);

            modelBuilder.Entity<tblErrorLog>()
                .Property(e => e.sAction)
                .IsUnicode(false);

            modelBuilder.Entity<tblErrorLog>()
                .Property(e => e.sError)
                .IsUnicode(false);

            modelBuilder.Entity<tblExchangeRatesM>()
                .Property(e => e.sCurrencyCodeFrom)
                .IsUnicode(false);

            modelBuilder.Entity<tblExchangeRatesM>()
                .Property(e => e.sCurrencyCodeTo)
                .IsUnicode(false);

            modelBuilder.Entity<tblExchangeRatesM>()
                .Property(e => e.dRate)
                .HasPrecision(15, 6);

            modelBuilder.Entity<tblGolfM>()
                .Property(e => e.sGolf)
                .IsUnicode(false);

            modelBuilder.Entity<tblGroupM>()
                .Property(e => e.sGroupName)
                .IsUnicode(false);

            modelBuilder.Entity<tblGroupM>()
                .Property(e => e.sDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblGroupM>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblGroupM>()
                .HasMany(e => e.tblGroupMenuMs)
                .WithRequired(e => e.tblGroupM)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblGroupM>()
                .HasMany(e => e.tblUserGroupMs)
                .WithRequired(e => e.tblGroupM)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblGroupMenuM>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblHotelAmenityM>()
                .Property(e => e.sAmenity)
                .IsUnicode(false);

            modelBuilder.Entity<tblHotelCommonServiceM>()
                .Property(e => e.sCommonService)
                .IsUnicode(false);

            modelBuilder.Entity<tblHotelConvenienceM>()
                .Property(e => e.sConvenience)
                .IsUnicode(false);

            modelBuilder.Entity<tblHotelFacilitesRankM>()
                .Property(e => e.sHotelFacilites)
                .IsUnicode(false);

            modelBuilder.Entity<tblHotelFacilitesRankM>()
                .Property(e => e.sImgUrl)
                .IsUnicode(false);

            modelBuilder.Entity<tblHotelLeisureM>()
                .Property(e => e.sLeisure)
                .IsUnicode(false);

            modelBuilder.Entity<tblHotelMeetingM>()
                .Property(e => e.sMeeting)
                .IsUnicode(false);

            modelBuilder.Entity<tblHotelParkingM>()
                .Property(e => e.sParking)
                .IsUnicode(false);

            modelBuilder.Entity<tblHotelParkingM>()
                .Property(e => e.cRadioCheckBox)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblHotelPartnerM>()
                .Property(e => e.sHotelName)
                .IsUnicode(false);

            modelBuilder.Entity<tblHotelPartnerM>()
                .Property(e => e.sWebsiteUrl)
                .IsUnicode(false);

            modelBuilder.Entity<tblHotelPartnerM>()
                .Property(e => e.sAddress)
                .IsUnicode(false);

            modelBuilder.Entity<tblHotelPartnerM>()
                .Property(e => e.sCity)
                .IsUnicode(false);

            modelBuilder.Entity<tblHotelPartnerM>()
                .Property(e => e.sState)
                .IsUnicode(false);

            modelBuilder.Entity<tblHotelPartnerM>()
                .Property(e => e.sCountry)
                .IsUnicode(false);

            modelBuilder.Entity<tblHotelPartnerM>()
                .Property(e => e.sPIN)
                .IsUnicode(false);

            modelBuilder.Entity<tblHotelPartnerM>()
                .Property(e => e.sBoardLineNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblHotelPartnerM>()
                .Property(e => e.sFirstName)
                .IsUnicode(false);

            modelBuilder.Entity<tblHotelPartnerM>()
                .Property(e => e.sLastName)
                .IsUnicode(false);

            modelBuilder.Entity<tblHotelPartnerM>()
                .Property(e => e.sDesignation)
                .IsUnicode(false);

            modelBuilder.Entity<tblHotelPartnerM>()
                .Property(e => e.sEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblHotelPartnerM>()
                .Property(e => e.sMobile)
                .IsUnicode(false);

            modelBuilder.Entity<tblHotelRoomAmenityAdditionalM>()
                .Property(e => e.sRoomAmenityAdditional)
                .IsUnicode(false);

            modelBuilder.Entity<tblHotelRoomAmenityBathRoomM>()
                .Property(e => e.sRoomAmenityBathRoom)
                .IsUnicode(false);

            modelBuilder.Entity<tblHotelRoomAmenityBeddingM>()
                .Property(e => e.sRoomAmenityBedding)
                .IsUnicode(false);

            modelBuilder.Entity<tblHotelRoomAmenityM>()
                .Property(e => e.sRoomAmenity)
                .IsUnicode(false);

            modelBuilder.Entity<tblHotelRoomAmenityM>()
                .Property(e => e.cRadioCheckBox)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblIndianLocalityCordinate>()
                .Property(e => e.LocalityName)
                .IsUnicode(false);

            modelBuilder.Entity<tblIndianLocalityCordinate>()
                .Property(e => e.StateName)
                .IsUnicode(false);

            modelBuilder.Entity<tblIndianLocalityCordinate>()
                .HasMany(e => e.tblPolygons)
                .WithOptional(e => e.tblIndianLocalityCordinate)
                .HasForeignKey(e => e.LocalityCordinates_Id);

            modelBuilder.Entity<tblItemNameM>()
                .Property(e => e.sItem)
                .IsUnicode(false);

            modelBuilder.Entity<tblItemNameM>()
                .Property(e => e.sName)
                .IsUnicode(false);

            modelBuilder.Entity<tblLandActivitiesM>()
                .Property(e => e.sLandActivity)
                .IsUnicode(false);

            modelBuilder.Entity<tblLanguageM>()
                .Property(e => e.sLanguage)
                .IsUnicode(false);

            modelBuilder.Entity<tblLocalityM>()
                .Property(e => e.sLocality)
                .IsUnicode(false);

            modelBuilder.Entity<tblLocalityM>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblLookUp>()
                .Property(e => e.sCheckInHH)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblLookUp>()
                .Property(e => e.sCheckInMM)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblLookUp>()
                .Property(e => e.dExtraBedCharges)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblLookUp>()
                .Property(e => e.cHrsDays)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblLookUp>()
                .Property(e => e.cRPPromoStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblLookUp>()
                .Property(e => e.dPrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblLookUp>()
                .Property(e => e.cHrsDaysPromo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblLookUp>()
                .Property(e => e.dPriceRP)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblLookUpExtraBed>()
                .Property(e => e.sCheckInHH)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblLookUpExtraBed>()
                .Property(e => e.sCheckInMM)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblLookUpExtraBed>()
                .Property(e => e.dExtraBedCharges)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblLookUpExtraBed>()
                .Property(e => e.cHrsDays)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblLookUpExtraBed>()
                .Property(e => e.cRPPromoStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblLookUpExtraBed>()
                .Property(e => e.dPrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblLookUpExtraBed>()
                .Property(e => e.cHrsDaysPromo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblLookUpExtraBed>()
                .Property(e => e.dPriceRP)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblLoyalityAmenityMap>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblLoyalityPointsM>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblMacroAreaM>()
                .Property(e => e.sArea)
                .IsUnicode(false);

            modelBuilder.Entity<tblMacroAreaM>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblMealCodesRG>()
                .Property(e => e.sDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblMenuM>()
                .Property(e => e.sMenuName)
                .IsUnicode(false);

            modelBuilder.Entity<tblMenuM>()
                .Property(e => e.sDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblMenuM>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblMenuM>()
                .Property(e => e.sPath)
                .IsUnicode(false);

            modelBuilder.Entity<tblMenuM>()
                .HasMany(e => e.tblGroupMenuMs)
                .WithRequired(e => e.tblMenuM)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblMenuM>()
                .HasMany(e => e.tblMenuM1)
                .WithOptional(e => e.tblMenuM2)
                .HasForeignKey(e => e.iParentId);

            modelBuilder.Entity<tblOfferReviewTrackerTx>()
                .Property(e => e.sIPAddress)
                .IsUnicode(false);

            modelBuilder.Entity<tblOnsiteRecreationFacilitiesM>()
                .Property(e => e.sRecreationFacility)
                .IsUnicode(false);

            modelBuilder.Entity<tblPhotoCategoryM>()
                .Property(e => e.sPhotoCatName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPhotoCategoryM>()
                .HasMany(e => e.tblPhotoSubCategoryMs)
                .WithRequired(e => e.tblPhotoCategoryM)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblPhotoSubCategoryM>()
                .Property(e => e.sPhotoSubCatName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPromoCodeM>()
                .Property(e => e.sPromoCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblPromoCodeM>()
                .Property(e => e.sPromoTitle)
                .IsUnicode(false);

            modelBuilder.Entity<tblPromoCodeM>()
                .Property(e => e.sPromoDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblPromoCodeM>()
                .Property(e => e.cPercentageValue)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPromoCodeM>()
                .Property(e => e.dValue)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblPromoCodeM>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPromoCodeM>()
                .Property(e => e.sTermCondition)
                .IsUnicode(false);

            modelBuilder.Entity<tblPromoCodeM>()
                .HasMany(e => e.tblPropertyPromoMaps)
                .WithRequired(e => e.tblPromoCodeM)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblPromoM>()
                .Property(e => e.sPromoName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPromoM>()
                .HasMany(e => e.tblPropertyPromotionMaps)
                .WithRequired(e => e.tblPromoM)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblPromotionalHotelsM>()
                .Property(e => e.sPosition)
                .IsUnicode(false);

            modelBuilder.Entity<tblPromotionalHotelsM>()
                .Property(e => e.sImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<tblPromotionalHotelsM>()
                .Property(e => e.sDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropConserveCommissionMap>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyAmenitiesMap>()
                .Property(e => e.sHotelAmenities)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyAmenitiesMap>()
                .Property(e => e.sHotelParkings)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyAmenitiesMap>()
                .Property(e => e.sHotelCommonServices)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyAmenitiesMap>()
                .Property(e => e.sHotelConvenience)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyAmenitiesMap>()
                .Property(e => e.sHotelLeisure)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyAmenitiesMap>()
                .Property(e => e.sHotelMeetings)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyAmenitiesMap>()
                .Property(e => e.sHotelParkingRadio)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyAreaDataTG>()
                .Property(e => e.sAreaName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyBasicBiddingMap>()
                .Property(e => e.dBasicDiscount)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyBidUpgradeM>()
                .Property(e => e.dUpgradeCharge)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblPropertyBidUpgradeM>()
                .Property(e => e.sUpgradeType)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyCancellationPolicyMap>()
                .Property(e => e.sPolicyName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyCancellationPolicyMap>()
                .Property(e => e.cHrsDays)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyCancellationPolicyMap>()
                .Property(e => e.dValue)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblPropertyCancellationPolicyMap>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyCancellationPolicyMap>()
                .HasMany(e => e.tblPropertyPromotionCancellationMaps)
                .WithRequired(e => e.tblPropertyCancellationPolicyMap)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblPropertyDiningMap>()
                .Property(e => e.sRestaurantName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyDiningMap>()
                .Property(e => e.cType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyDiningMap>()
                .Property(e => e.sDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyDiningMap>()
                .Property(e => e.sTimingFromHH)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyDiningMap>()
                .Property(e => e.sTimingFromMM)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyDiningMap>()
                .Property(e => e.sTimingToHH)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyDiningMap>()
                .Property(e => e.sTimingToMM)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyFacilityTG>()
                .Property(e => e.sAmenityType)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyFacilityTG>()
                .Property(e => e.sDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyFacilityTG>()
                .Property(e => e.iVendorId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyFacilityTG_Tmp>()
                .Property(e => e.sAmenityType)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyFacilityTG_Tmp>()
                .Property(e => e.sDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyFacilityTG_Tmp>()
                .Property(e => e.iVendorId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyImageUrlTG>()
                .Property(e => e.sContentTitle)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyImageUrlTG>()
                .Property(e => e.sImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyImageUrlTG>()
                .Property(e => e.iVendorId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyImageUrlTG_Tmp>()
                .Property(e => e.sContentTitle)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyImageUrlTG_Tmp>()
                .Property(e => e.sImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyImageUrlTG_Tmp>()
                .Property(e => e.iVendorId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyLeadTimeBiddingMap>()
                .Property(e => e.dLeadDiscount)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyLocalityMap>()
                .Property(e => e.cAreaLocality)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyLOSBiddingMap>()
                .Property(e => e.dLeadDiscount)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sHotelName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sAddress)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sWebSite)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.dLatitude)
                .HasPrecision(12, 9);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.dLongitude)
                .HasPrecision(12, 9);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sDistanceFromAirportRailwayStation)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sNearestTransport)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sAreaRecommended)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sTopAttractions)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sGeneralManagerName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sRevenueManagerName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sFinanceContactName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sPrimaryContactName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sGeneralManagerContact)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sRevenueManagerContact)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sFinanceContactContact)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sPrimaryContactContact)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sGeneralManagerEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sRevenueManagerEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sFinanceContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sPrimaryContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sAccessbilityIds)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sAwardIds)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sAffiliationIds)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sLanguageIds)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sAreaRecommended1)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sAreaRecommended2)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sAreaRecommended3)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sNearestTransport1)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.dNearestTransport1)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sNearestTransport2)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.dNearestTransport2)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sNearestTransport3)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.dNearestTransport3)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sTopAttraction1)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sTopAttraction2)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sTopAttraction3)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sDistanceToAirportRailway1)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.dDistanceToAirportRailway1)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sDistanceToAirportRailway2)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.dDistanceToAirportRailway2)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sDistanceToAirportRailway3)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.dDistanceToAirportRailway3)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sConfirmationContactName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sConfirmationContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sConfirmationContactContact)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.iVendorId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.rating_image_url)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.rating)
                .HasPrecision(3, 1);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.ranking_string)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sImageUrlTG)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .Property(e => e.sSecondaryAreaNameTG)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM>()
                .HasOptional(e => e.tblOFRPropertyM)
                .WithRequired(e => e.tblPropertyM);

            modelBuilder.Entity<tblPropertyM>()
                .HasMany(e => e.tblPropertyWeekendBiddingMaps)
                .WithRequired(e => e.tblPropertyM)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblPropertyM>()
                .HasMany(e => e.tblPropertyRoomInventories)
                .WithRequired(e => e.tblPropertyM)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblPropertyM>()
                .HasMany(e => e.tblUserPropertyMaps)
                .WithRequired(e => e.tblPropertyM)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sHotelName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sAddress)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sWebSite)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.dLatitude)
                .HasPrecision(12, 9);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.dLongitude)
                .HasPrecision(12, 9);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sDistanceFromAirportRailwayStation)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sNearestTransport)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sAreaRecommended)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sTopAttractions)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sGeneralManagerName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sRevenueManagerName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sFinanceContactName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sPrimaryContactName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sGeneralManagerContact)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sRevenueManagerContact)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sFinanceContactContact)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sPrimaryContactContact)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sGeneralManagerEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sRevenueManagerEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sFinanceContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sPrimaryContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sAccessbilityIds)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sAwardIds)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sAffiliationIds)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sLanguageIds)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sAreaRecommended1)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sAreaRecommended2)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sAreaRecommended3)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sNearestTransport1)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.dNearestTransport1)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sNearestTransport2)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.dNearestTransport2)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sNearestTransport3)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.dNearestTransport3)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sTopAttraction1)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sTopAttraction2)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sTopAttraction3)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sDistanceToAirportRailway1)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.dDistanceToAirportRailway1)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sDistanceToAirportRailway2)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.dDistanceToAirportRailway2)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sDistanceToAirportRailway3)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.dDistanceToAirportRailway3)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sConfirmationContactName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sConfirmationContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTest>()
                .Property(e => e.sConfirmationContactContact)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sHotelName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.iChainId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sLocality)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sArea)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sCountry)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sState)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sCity)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sAddress)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sWebSite)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.dLatitude)
                .HasPrecision(12, 9);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.dLongitude)
                .HasPrecision(12, 9);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sDistanceFromAirportRailwayStation)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sNearestTransport)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sAreaRecommended)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sTopAttractions)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sGeneralManagerName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sRevenueManagerName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sFinanceContactName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sPrimaryContactName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sGeneralManagerContact)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sRevenueManagerContact)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sFinanceContactContact)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sPrimaryContactContact)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sGeneralManagerEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sRevenueManagerEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sFinanceContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sPrimaryContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sAccessbilityIds)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sAwardIds)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sAffiliationIds)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sLanguageIds)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sAreaRecommended1)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sAreaRecommended2)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sAreaRecommended3)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sNearestTransport1)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.dNearestTransport1)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sNearestTransport2)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.dNearestTransport2)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sNearestTransport3)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.dNearestTransport3)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sTopAttraction1)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sTopAttraction2)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sTopAttraction3)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sDistanceToAirportRailway1)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.dDistanceToAirportRailway1)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sDistanceToAirportRailway2)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.dDistanceToAirportRailway2)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sDistanceToAirportRailway3)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.dDistanceToAirportRailway3)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sConfirmationContactName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sConfirmationContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sConfirmationContactContact)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.iVendorId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sImageUrlTG)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyMTG_Tmp>()
                .Property(e => e.sSecondaryAreaNameTG)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyParkingMap>()
                .Property(e => e.sCarName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyParkingMap>()
                .Property(e => e.cAirportRail)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyParkingMap>()
                .Property(e => e.dOnewayCharge)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblPropertyParkingMap>()
                .Property(e => e.dTwowayCharge)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblPropertyPhotoMap>()
                .Property(e => e.sImgUrl)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPhotoMap>()
                .Property(e => e.UniqueAzureFileName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPhotoMap>()
                .HasMany(e => e.tblPropertyPrimaryPhotoMaps)
                .WithRequired(e => e.tblPropertyPhotoMap)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblPropertyPolicyMap>()
                .Property(e => e.sCreditCardId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPolicyMap>()
                .Property(e => e.sCheckInHH)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPolicyMap>()
                .Property(e => e.sCheckInMM)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPolicyMap>()
                .Property(e => e.sCheckoutHH)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPolicyMap>()
                .Property(e => e.sCheckoutMM)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPolicyMap>()
                .Property(e => e.dExtraBedCharges)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblPropertyPrimaryPhotoMap>()
                .Property(e => e.cType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPromoMap>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPromoRateMap>()
                .Property(e => e.dSingleRate)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblPropertyPromoRateMap>()
                .Property(e => e.dDoubleRate)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblPropertyPromoRateMap>()
                .Property(e => e.dTripleRate)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblPropertyPromoRateMap>()
                .Property(e => e.dQuadrupleRate)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblPropertyPromoRateMap>()
                .Property(e => e.dQuintrupleRate)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblPropertyPromotionCancellationMainMap>()
                .Property(e => e.sCancellationPolicyId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPromotionMap>()
                .Property(e => e.dValue)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblPropertyPromotionMap>()
                .Property(e => e.sRoomTypeId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPromotionMap>()
                .Property(e => e.sAmenityId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPromotionMap>()
                .Property(e => e.sAmenity)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPromotionMap>()
                .Property(e => e.sCancellationPolicy)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPromotionMap>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPromotionMap>()
                .Property(e => e.dNegotiationPer)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyPromotionMap>()
                .HasMany(e => e.tblPropertyPromoRateMaps)
                .WithRequired(e => e.tblPropertyPromotionMap)
                .HasForeignKey(e => e.iPropPromoID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblPropertyPromotionMap>()
                .HasMany(e => e.tblPropertyPromotionCancellationMainMaps)
                .WithRequired(e => e.tblPropertyPromotionMap)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblPropertyPromotionMap>()
                .HasMany(e => e.tblPropertyPromotionCancellationMaps)
                .WithRequired(e => e.tblPropertyPromotionMap)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblPropertyPromotionMap>()
                .HasMany(e => e.tblPropertyPromotionRoomTypeMaps)
                .WithRequired(e => e.tblPropertyPromotionMap)
                .HasForeignKey(e => e.iPropPromoID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblPropertyRatePlanCancellationMainMap>()
                .Property(e => e.sCancellationPolicyId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanMap>()
                .Property(e => e.sRatePlan)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanMap>()
                .Property(e => e.cRatePlanType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanMap>()
                .Property(e => e.dValue)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblPropertyRatePlanMap>()
                .Property(e => e.sAmenityId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanMap>()
                .Property(e => e.sAmenity)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanMap>()
                .Property(e => e.cHrsDays)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanMap>()
                .Property(e => e.sCancellationPolicy)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanMap>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanMap>()
                .Property(e => e.sRoomId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanMap>()
                .Property(e => e.dNegotiationPer)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyRatePlanMap>()
                .HasMany(e => e.tblPropertyRatePlanRoomTypeMaps)
                .WithRequired(e => e.tblPropertyRatePlanMap)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblPropertyRatePlanMap>()
                .HasMany(e => e.tblPropertyRoomRatePlanInventoryMaps)
                .WithRequired(e => e.tblPropertyRatePlanMap)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblPropertyRecreationMap>()
                .Property(e => e.sRecreationFacilityId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRecreationMap>()
                .Property(e => e.sLandActivityId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRecreationMap>()
                .Property(e => e.sGolfId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyReviewTG>()
                .Property(e => e.iVendorId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyReviewTG>()
                .Property(e => e.sAvgGuestRating)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyReviewTG>()
                .Property(e => e.sComments)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyReviewTG>()
                .Property(e => e.sConsumer_city)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyReviewTG>()
                .Property(e => e.sConsumer_Country)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyReviewTG>()
                .Property(e => e.sCustomer_name)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyReviewTG_Tmp>()
                .Property(e => e.iVendorId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyReviewTG_Tmp>()
                .Property(e => e.sAvgGuestRating)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyReviewTG_Tmp>()
                .Property(e => e.sComments)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyReviewTG_Tmp>()
                .Property(e => e.sConsumer_city)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyReviewTG_Tmp>()
                .Property(e => e.sConsumer_Country)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyReviewTG_Tmp>()
                .Property(e => e.sCustomer_name)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomAmentiesMap>()
                .Property(e => e.sBasicRoomAmenities)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomAmentiesMap>()
                .Property(e => e.sAdditionalRoomAmenities)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomAmentiesMap>()
                .Property(e => e.sBathRoomAmenities)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomAmentiesMap>()
                .Property(e => e.sBeddingRoomAmenities)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomDescriptionTG>()
                .Property(e => e.sDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomDescriptionTG>()
                .Property(e => e.sRoomType)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomDescriptionTG>()
                .Property(e => e.iRoomTypeId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomDescriptionTG>()
                .Property(e => e.sRoomType_ImagePath)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomDescriptionTG>()
                .Property(e => e.iVendorId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomDescriptionTG_Tmp>()
                .Property(e => e.sDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomDescriptionTG_Tmp>()
                .Property(e => e.sRoomType)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomDescriptionTG_Tmp>()
                .Property(e => e.iRoomTypeId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomDescriptionTG_Tmp>()
                .Property(e => e.sRoomType_ImagePath)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomDescriptionTG_Tmp>()
                .Property(e => e.iVendorId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomLevelAmenityTG>()
                .Property(e => e.sDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomLevelAmenityTG>()
                .Property(e => e.sRoomType)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomLevelAmenityTG>()
                .Property(e => e.iRoomTypeId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomLevelAmenityTG>()
                .Property(e => e.iVendorId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomLevelAmenityTG_Tmp>()
                .Property(e => e.sDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomLevelAmenityTG_Tmp>()
                .Property(e => e.sRoomType)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomLevelAmenityTG_Tmp>()
                .Property(e => e.iRoomTypeId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomLevelAmenityTG_Tmp>()
                .Property(e => e.iVendorId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomMap>()
                .Property(e => e.sRoomName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomMap>()
                .Property(e => e.sRoomCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomMap>()
                .Property(e => e.dSizeSqft)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblPropertyRoomMap>()
                .Property(e => e.dSizeMtr)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblPropertyRoomMap>()
                .Property(e => e.sSizeType)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomMap>()
                .Property(e => e.sRoomAccessibilityIds)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomMap>()
                .Property(e => e.sBuildingCharacteristicsIds)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomMap>()
                .Property(e => e.sRoomOutdoorViewIds)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomMap>()
                .HasMany(e => e.tblPropertyPromoRateMaps)
                .WithRequired(e => e.tblPropertyRoomMap)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblPropertyRoomMap>()
                .HasMany(e => e.tblPropertyRoomInventories)
                .WithRequired(e => e.tblPropertyRoomMap)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblPropertyRoomMap>()
                .HasMany(e => e.tblPropertyRoomRatePlanInventoryMaps)
                .WithRequired(e => e.tblPropertyRoomMap)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblPropertyRoomMap>()
                .HasMany(e => e.tblPropertyRoomTypeRoomAmentiesMaps)
                .WithRequired(e => e.tblPropertyRoomMap)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblPropertyRoomRatePlanInventoryMap>()
                .Property(e => e.dSingleRate)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblPropertyRoomRatePlanInventoryMap>()
                .Property(e => e.dDoubleRate)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblPropertyRoomRatePlanInventoryMap>()
                .Property(e => e.dTripleRate)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblPropertyRoomRatePlanInventoryMap>()
                .Property(e => e.dQuadrupleRate)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblPropertyRoomRatePlanInventoryMap>()
                .Property(e => e.dQuintrupleRate)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblPropertyRoomsBiddingMap>()
                .Property(e => e.dLeadDiscount)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyRoomTaxMap>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomTypeRoomAmentiesMap>()
                .Property(e => e.sBasicRoomAmenities)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomTypeRoomAmentiesMap>()
                .Property(e => e.sAdditionalRoomAmenities)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomTypeRoomAmentiesMap>()
                .Property(e => e.sBathRoomAmenities)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomTypeRoomAmentiesMap>()
                .Property(e => e.sBeddingRoomAmenities)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertySpaMap>()
                .Property(e => e.sSpaName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertySpaMap>()
                .Property(e => e.sSpaDesc)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyTaxesMap>()
                .Property(e => e.dValue)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblPropertyTaxMap>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyTaxMap>()
                .HasMany(e => e.tblPropertyTaxesMaps)
                .WithRequired(e => e.tblPropertyTaxMap)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblPropertyTopAttrationTG>()
                .Property(e => e.iVendorId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyTopAttrationTG>()
                .Property(e => e.sNameOfAttraction)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyTopAttrationTG>()
                .Property(e => e.iDistanceInKM)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblPropertyTopAttrationTG_Tmp>()
                .Property(e => e.iVendorId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyTopAttrationTG_Tmp>()
                .Property(e => e.sNameOfAttraction)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyTopAttrationTG_Tmp>()
                .Property(e => e.iDistanceInKM)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblPropertyTripAdvisorM>()
                .Property(e => e.sRatingImageURL)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyTripAdvisorM>()
                .Property(e => e.iRating)
                .HasPrecision(2, 1);

            modelBuilder.Entity<tblPropertyTripAdvisorM>()
                .Property(e => e.sRankingString)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyTripAdvisorM>()
                .Property(e => e.sWebURL)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyWeekendBiddingMap>()
                .Property(e => e.dWeekendDiscount)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.SKUCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.ORDERNO)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.ORDERSTATUS)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.Amount)
                .HasPrecision(16, 2);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.REFNO)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.REFNOEXT)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.PAYMETHOD)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.PAYMETHOD_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.CARD_TYPE)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.CARD_LAST_DIGITS)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.CHARGEBACK_RESOLUTION)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.AVANGATE_CUSTOMER_REFERENCE)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.EXTERNAL_CUSTOMER_REFERENCE)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.IPADDRESS)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.IPCOUNTRY)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.IPN_LICENSE_REF)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.IPN_DATE)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.ResponseIssuingDate)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.HASH)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.ReadReceiptHASH)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.FIRSTNAME)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.LASTNAME)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.ADDRESS1)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.ADDRESS2)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.CITY)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.STATE)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.ZIPCODE)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.COUNTRY)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.PHONE)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.FAX)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.CUSTOMEREMAIL)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.FIRSTNAME_D)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.LASTNAME_D)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.ADDRESS1_D)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.ADDRESS2_D)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.CITY_D)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.STATE_D)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.ZIPCODE_D)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.COUNTRY_D)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.PHONE_D)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.EMAIL_D)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.TIMEZONE_OFFSET)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.CURRENCY)
                .IsUnicode(false);

            modelBuilder.Entity<tblPurchaseHistory>()
                .Property(e => e.MISC)
                .IsUnicode(false);

            modelBuilder.Entity<tblRankManagement>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanM>()
                .Property(e => e.sRatePlan)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanM>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblRecentSavingsTx>()
                .Property(e => e.sCity)
                .IsUnicode(false);

            modelBuilder.Entity<tblRecentSavingsTx>()
                .Property(e => e.sCurrencyCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRecentSavingsTx>()
                .Property(e => e.dActualAmt)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblRecentSavingsTx>()
                .Property(e => e.dDiscountedAmt)
                .HasPrecision(10, 2);

            modelBuilder.Entity<tblRoomFacilityDropDownM>()
                .Property(e => e.sRoomFacilites)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomFacilityDropDownM>()
                .Property(e => e.sImgUrl)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomOutdoorViewM>()
                .Property(e => e.sRoomOutdoorView)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomTypeM>()
                .Property(e => e.sRoomType)
                .IsUnicode(false);

            modelBuilder.Entity<tblSettingM>()
                .Property(e => e.sPhone)
                .IsUnicode(false);

            modelBuilder.Entity<tblSettingM>()
                .Property(e => e.sEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblStarCategoryM>()
                .Property(e => e.sStarCategory)
                .IsUnicode(false);

            modelBuilder.Entity<tblStateM>()
                .Property(e => e.sState)
                .IsUnicode(false);

            modelBuilder.Entity<tblStateM>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblStateM>()
                .HasMany(e => e.tblCityMs)
                .WithRequired(e => e.tblStateM)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblTaxM>()
                .Property(e => e.sTaxName)
                .IsUnicode(false);

            modelBuilder.Entity<tblTaxM>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblTaxM>()
                .HasMany(e => e.tblPropertyTaxesMaps)
                .WithRequired(e => e.tblTaxM)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblTGAmenitiesMap>()
                .Property(e => e.iVendorId)
                .IsUnicode(false);

            modelBuilder.Entity<tblTripAdvisorReview>()
                .Property(e => e.sTitle)
                .IsUnicode(false);

            modelBuilder.Entity<tblTripAdvisorReview>()
                .Property(e => e.sReviewer)
                .IsUnicode(false);

            modelBuilder.Entity<tblTripAdvisorReview>()
                .Property(e => e.iRating)
                .HasPrecision(2, 1);

            modelBuilder.Entity<tblTripAdvisorReview>()
                .Property(e => e.sRatingImageURL)
                .IsUnicode(false);

            modelBuilder.Entity<tblTripAdvisorReview>()
                .Property(e => e.sReviewURL)
                .IsUnicode(false);

            modelBuilder.Entity<tblTripAdvisorReview>()
                .Property(e => e.sTripType)
                .IsUnicode(false);

            modelBuilder.Entity<tblTripAdvisorReview>()
                .Property(e => e.sUserLocation)
                .IsUnicode(false);

            modelBuilder.Entity<tblUserGroupM>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblUserM>()
                .Property(e => e.sUserName)
                .IsUnicode(false);

            modelBuilder.Entity<tblUserM>()
                .Property(e => e.sPassword)
                .IsUnicode(false);

            modelBuilder.Entity<tblUserM>()
                .Property(e => e.sFirstName)
                .IsUnicode(false);

            modelBuilder.Entity<tblUserM>()
                .Property(e => e.sLastName)
                .IsUnicode(false);

            modelBuilder.Entity<tblUserM>()
                .Property(e => e.sEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblUserM>()
                .Property(e => e.sContact)
                .IsUnicode(false);

            modelBuilder.Entity<tblUserM>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblAdditionalCommisions)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblAmenityMs)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblChainMs)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblCityMs)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblCountryMs)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblCurrencyMs)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblGroupMs)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iCreatedBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblGroupMs1)
                .WithOptional(e => e.tblUserM1)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblGroupMenuMs)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblGroupMenuMs1)
                .WithOptional(e => e.tblUserM1)
                .HasForeignKey(e => e.iCreatedBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblLoyalityAmenityMaps)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblLoyalityPointsMs)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblMacroAreaMs)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblPromoCodeMs)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblPropertyBasicBiddingMaps)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblPropertyLeadTimeBiddingMaps)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblPropertyLOSBiddingMaps)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblPropertyParkingMaps)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblPropertyPhotoMaps)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblPropertyPolicyMaps)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblPropertyPrimaryPhotoMaps)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblPropertyPromoMaps)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblPropertyPromotionMaps)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblPropertyPromotionRoomTypeMaps)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblPropertyRatePlanMaps)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblPropertyRoomAmentiesMaps)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblPropertyRoomInventories)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblPropertyRoomMaps)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblPropertyRoomRatePlanInventoryMaps)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblPropertyRoomsBiddingMaps)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblPropertyRoomTypeRoomAmentiesMaps)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblPropertyTaxesMaps)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblPropertyTaxMaps)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblPropertyWeekendBiddingMaps)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblStateMs)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblTaxMs)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblUserGroupMs)
                .WithRequired(e => e.tblUserM)
                .HasForeignKey(e => e.iUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblUserGroupMs1)
                .WithRequired(e => e.tblUserM1)
                .HasForeignKey(e => e.iUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblUserGroupMs2)
                .WithOptional(e => e.tblUserM2)
                .HasForeignKey(e => e.iCreatedBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblUserM1)
                .WithOptional(e => e.tblUserM2)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblVideoUrlMs)
                .WithOptional(e => e.tblUserM)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblUserPropertyMaps)
                .WithRequired(e => e.tblUserM)
                .HasForeignKey(e => e.iUserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblUserPropertyMaps1)
                .WithOptional(e => e.tblUserM1)
                .HasForeignKey(e => e.iActionBy);

            modelBuilder.Entity<tblUserM>()
                .HasMany(e => e.tblUserPropertyMaps2)
                .WithOptional(e => e.tblUserM2)
                .HasForeignKey(e => e.iCreatedBy);

            modelBuilder.Entity<tblUserPropertyMap>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblVideoUrlM>()
                .Property(e => e.sVideoUrl)
                .IsUnicode(false);

            modelBuilder.Entity<tblVideoUrlM>()
                .Property(e => e.sImgUrl)
                .IsUnicode(false);

            modelBuilder.Entity<tblVideoUrlM>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblWebsiteGuestUserMaster>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<tblWebsiteGuestUserMaster>()
                .Property(e => e.PhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblWebsiteUserMater>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<tblWebsiteUserMater>()
                .Property(e => e.DisplayName)
                .IsUnicode(false);

            modelBuilder.Entity<tblWebsiteUserMater>()
                .Property(e => e.PhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblWebsiteUserMater>()
                .Property(e => e.MartialStatus)
                .IsUnicode(false);

            modelBuilder.Entity<tblWebsiteUserMater>()
                .Property(e => e.SpouseName)
                .IsUnicode(false);

            modelBuilder.Entity<tblWebsiteUserMater>()
                .Property(e => e.ProfileImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<tblWebsiteUserMater>()
                .Property(e => e.CurrentAddressLine1)
                .IsUnicode(false);

            modelBuilder.Entity<tblWebsiteUserMater>()
                .Property(e => e.CurrentAddressLine2)
                .IsUnicode(false);

            modelBuilder.Entity<tblWebsiteUserMater>()
                .Property(e => e.CurrentAddressLine3)
                .IsUnicode(false);

            modelBuilder.Entity<tblWebsiteUserMater>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<tblWebsiteUserMater>()
                .Property(e => e.ID_Type)
                .IsUnicode(false);

            modelBuilder.Entity<tblWebsiteUserMater>()
                .Property(e => e.ID_Number)
                .IsUnicode(false);

            modelBuilder.Entity<tblWebsiteUserMater>()
                .Property(e => e.GovApprovedPhotoIdUrl)
                .IsUnicode(false);

            modelBuilder.Entity<tblWebsiteUserMater>()
                .Property(e => e.SmokingRoom)
                .IsUnicode(false);

            modelBuilder.Entity<tblWebsiteUserMater>()
                .Property(e => e.PreferedStarRating)
                .IsUnicode(false);

            modelBuilder.Entity<tblWebsiteUserMater>()
                .Property(e => e.SpecialRequest)
                .IsUnicode(false);

            modelBuilder.Entity<tblWebsiteUserMater>()
                .Property(e => e.sReferralCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblWebsiteUserMater>()
                .HasMany(e => e.tblBookingAmedTxes)
                .WithOptional(e => e.tblWebsiteUserMater)
                .HasForeignKey(e => e.iCustomerId);

            modelBuilder.Entity<tblWebsiteUserMater>()
                .HasMany(e => e.tblBookingTxes)
                .WithOptional(e => e.tblWebsiteUserMater)
                .HasForeignKey(e => e.iCustomerId);

            modelBuilder.Entity<tblWebsiteUserMater>()
                .HasMany(e => e.tblCustomerPointsMaps)
                .WithRequired(e => e.tblWebsiteUserMater)
                .HasForeignKey(e => e.iCustomerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblWebsiteUserMater>()
                .HasMany(e => e.tblMyWishListTxes)
                .WithOptional(e => e.tblWebsiteUserMater)
                .HasForeignKey(e => e.iCustomerId);

            modelBuilder.Entity<tblWebsiteUserMater>()
                .HasMany(e => e.tblWebsiteUserClaims)
                .WithRequired(e => e.tblWebsiteUserMater)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<tblWebsiteUserMater>()
                .HasMany(e => e.tblWebsiteUserLogins)
                .WithRequired(e => e.tblWebsiteUserMater)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<tblWebsiteUserMater>()
                .HasMany(e => e.tblRecentViewsTxes)
                .WithOptional(e => e.tblWebsiteUserMater)
                .HasForeignKey(e => e.iCustomerId);

            modelBuilder.Entity<tblWebsiteUserMater>()
                .HasMany(e => e.tblWebsiteUserRoleMasters)
                .WithMany(e => e.tblWebsiteUserMaters)
                .Map(m => m.ToTable("tblWebsiteUserRoles").MapLeftKey("UserId").MapRightKey("RoleId"));

            modelBuilder.Entity<tblAdditionalCommision_log>()
                .Property(e => e.dCommission)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblAdditionalCommision_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblAdditionalCommision_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblAdditionalCommision_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblAdditionalCommision_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblAmenityM_log>()
                .Property(e => e.sAmenityName)
                .IsUnicode(false);

            modelBuilder.Entity<tblAmenityM_log>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblAmenityM_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblAmenityM_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblAmenityM_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblAmenityM_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM_log>()
                .Property(e => e.sBankAccountNo)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM_log>()
                .Property(e => e.sBaneficiariesName)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM_log>()
                .Property(e => e.sBankName)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM_log>()
                .Property(e => e.sBranchName)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM_log>()
                .Property(e => e.sBranchAddress)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM_log>()
                .Property(e => e.sMicrCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM_log>()
                .Property(e => e.sIfscCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM_log>()
                .Property(e => e.sLetterhead_BankAccount)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM_log>()
                .Property(e => e.sCancelledCheque)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM_log>()
                .Property(e => e.sPanCard)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM_log>()
                .Property(e => e.dCommission)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblBankDetailM_log>()
                .Property(e => e.sFName)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM_log>()
                .Property(e => e.sFPhoneNo)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM_log>()
                .Property(e => e.sFFaxNo)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM_log>()
                .Property(e => e.sFEmailId)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblBankDetailM_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBannerM_log>()
                .Property(e => e.sBannerName)
                .IsUnicode(false);

            modelBuilder.Entity<tblBannerM_log>()
                .Property(e => e.sBannerType)
                .IsUnicode(false);

            modelBuilder.Entity<tblBannerM_log>()
                .Property(e => e.sTextPosition)
                .IsUnicode(false);

            modelBuilder.Entity<tblBannerM_log>()
                .Property(e => e.cstatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBannerM_log>()
                .Property(e => e.sLinkId)
                .IsUnicode(false);

            modelBuilder.Entity<tblBannerM_log>()
                .Property(e => e.sImgUrl)
                .IsUnicode(false);

            modelBuilder.Entity<tblBannerM_log>()
                .Property(e => e.UniqueAzureFileName)
                .IsUnicode(false);

            modelBuilder.Entity<tblBannerM_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblBannerM_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblBannerM_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblBannerM_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyAmenitiesTGSpecific_Tmp>()
                .Property(e => e.sSessionId)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyAmenitiesTGSpecific_Tmp>()
                .Property(e => e.sHotelCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyAmenitiesTGSpecific_Tmp>()
                .Property(e => e.sDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyAmenitiesTGSpecific_Tmp>()
                .Property(e => e.sCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyImgTGSpecific_Tmp>()
                .Property(e => e.sSessionId)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyImgTGSpecific_Tmp>()
                .Property(e => e.sHotelCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyImgTGSpecific_Tmp>()
                .Property(e => e.sThumbImgUrl)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyImgTGSpecific_Tmp>()
                .Property(e => e.sThumbCategory)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyImgTGSpecific_Tmp>()
                .Property(e => e.sThumbCaption)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyImgTGSpecific_Tmp>()
                .Property(e => e.sLargeImgUrl)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyImgTGSpecific_Tmp>()
                .Property(e => e.sLargeCategory)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyImgTGSpecific_Tmp>()
                .Property(e => e.sLargeCaption)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSearch_Tmp>()
                .Property(e => e.sSessionId)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSearch_Tmp>()
                .Property(e => e.sHotelCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSearch_Tmp>()
                .Property(e => e.sCurrencyCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSearch_Tmp>()
                .Property(e => e.sStopCell)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSearch_Tmp>()
                .Property(e => e.sLowestRatePlanCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSearch_Tmp>()
                .Property(e => e.sHotelType)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSearch_Tmp>()
                .Property(e => e.iReviewRating)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSearch_Tmp>()
                .Property(e => e.iReviewCount)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSearch_Tmp>()
                .Property(e => e.sOverviewUrl)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sSessionId)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sHotelCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sHotelName)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sCurrencyCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sLatitude)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sLongitude)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sAddress)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sCity)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sPostalCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sStateName)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sCountryName)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sAwardRating)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sStopCell)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sLowestRatePlanCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sFlexibleCheckIn)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sStateSeoId)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sCitySeoId)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sHotelType)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sCheckInTime)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sCheckOutTime)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sAreaSeoId)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sArea)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sAmenityDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sReviewImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sThumbnailUrl)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sThumbnailImageName)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyInfoTGSpecific_Tmp>()
                .Property(e => e.sOverviewUrl)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyPOITGSpecific_Tmp>()
                .Property(e => e.sSessionId)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyPOITGSpecific_Tmp>()
                .Property(e => e.sHotelCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyPOITGSpecific_Tmp>()
                .Property(e => e.sPOIName)
                .IsUnicode(false);

            modelBuilder.Entity<tblBasicPropertyPOITGSpecific_Tmp>()
                .Property(e => e.sPOIDistance)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.iCountryOffset)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.sCountryTimeZone)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.cBookingType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.sPromoCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.bPromoAmenity)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.sTitleOFR)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.sFirstNameOFR)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.sLastNameOFR)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.sEmailOFR)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.sMobileOFR)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.BookingStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.PaymentStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.ActualBookingType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.dTotalNegotiateAmount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.dAdvNegotiateAmount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.dBidAmount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.dTotalAmount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.dTotalComm)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.dTaxes)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.dTaxesForHotel)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.dTotalExtraBedAmount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.dDiscountedBidPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.dCustomerPayable)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.sSpecialRequests)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.sExpectedCheckIn)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.sSpecialOccasion)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.spref_single_lady)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.spref_away_elevator)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.spref_smoking_room)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.spref_quiet_room)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.spref_pickup)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.spref_extra1)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.spref_extra2)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.spref_extra3)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.sFlightNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.sEstArrivalTime)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.sLandingAt)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.sTypeOfCar)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.iCounterOffer)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.sCurrencyCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.sExtra1)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.dccPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.dccDiscount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.dSafeguardComm)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.ccType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblBookingTx_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblChainM_log>()
                .Property(e => e.sChainName)
                .IsUnicode(false);

            modelBuilder.Entity<tblChainM_log>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblChainM_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblChainM_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblChainM_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblChainM_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblChannelMgrBookingTracker>()
                .Property(e => e.sCMDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblChannelMgrBookingTracker>()
                .Property(e => e.sCMStatus)
                .IsUnicode(false);

            modelBuilder.Entity<tblGroupM_log>()
                .Property(e => e.sGroupName)
                .IsUnicode(false);

            modelBuilder.Entity<tblGroupM_log>()
                .Property(e => e.sDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblGroupM_log>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblGroupM_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblGroupM_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblGroupM_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblGroupM_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblGroupMenuM_log>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblGroupMenuM_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblGroupMenuM_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblGroupMenuM_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblGroupMenuM_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblLocalityM_log>()
                .Property(e => e.sLocality)
                .IsUnicode(false);

            modelBuilder.Entity<tblLocalityM_log>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblLocalityM_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblLocalityM_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblLocalityM_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblLocalityM_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblLoyalityAmenityMap_log>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblLoyalityAmenityMap_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblLoyalityAmenityMap_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblLoyalityAmenityMap_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblLoyalityAmenityMap_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblLoyalityPointsM_log>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblLoyalityPointsM_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblLoyalityPointsM_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblLoyalityPointsM_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblLoyalityPointsM_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblMacroAreaM_log>()
                .Property(e => e.sArea)
                .IsUnicode(false);

            modelBuilder.Entity<tblMacroAreaM_log>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblMacroAreaM_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblMacroAreaM_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblMacroAreaM_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblMacroAreaM_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPolygon>()
                .Property(e => e.PolygonCordinates)
                .IsUnicode(false);

            modelBuilder.Entity<tblPromoCodeM_log>()
                .Property(e => e.sPromoCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblPromoCodeM_log>()
                .Property(e => e.sPromoTitle)
                .IsUnicode(false);

            modelBuilder.Entity<tblPromoCodeM_log>()
                .Property(e => e.sPromoDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblPromoCodeM_log>()
                .Property(e => e.cPercentageValue)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPromoCodeM_log>()
                .Property(e => e.dValue)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblPromoCodeM_log>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPromoCodeM_log>()
                .Property(e => e.sTermCondition)
                .IsUnicode(false);

            modelBuilder.Entity<tblPromoCodeM_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblPromoCodeM_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblPromoCodeM_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblPromoCodeM_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPromotionalHotelsM_log>()
                .Property(e => e.sPosition)
                .IsUnicode(false);

            modelBuilder.Entity<tblPromotionalHotelsM_log>()
                .Property(e => e.sImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<tblPromotionalHotelsM_log>()
                .Property(e => e.sDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblPromotionalHotelsM_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblPromotionalHotelsM_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblPromotionalHotelsM_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblPromotionalHotelsM_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyAmenitiesMap_log>()
                .Property(e => e.sHotelAmenities)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyAmenitiesMap_log>()
                .Property(e => e.sHotelParkings)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyAmenitiesMap_log>()
                .Property(e => e.sHotelCommonServices)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyAmenitiesMap_log>()
                .Property(e => e.sHotelConvenience)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyAmenitiesMap_log>()
                .Property(e => e.sHotelLeisure)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyAmenitiesMap_log>()
                .Property(e => e.sHotelMeetings)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyAmenitiesMap_log>()
                .Property(e => e.sHotelParkingRadio)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyAmenitiesMap_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyAmenitiesMap_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyAmenitiesMap_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyAmenitiesMap_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyCompetitorSet_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyCompetitorSet_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyCompetitorSet_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyCompetitorSet_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyDiningMap_log>()
                .Property(e => e.sRestaurantName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyDiningMap_log>()
                .Property(e => e.cType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyDiningMap_log>()
                .Property(e => e.sDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyDiningMap_log>()
                .Property(e => e.sTimingFromHH)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyDiningMap_log>()
                .Property(e => e.sTimingFromMM)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyDiningMap_log>()
                .Property(e => e.sTimingToHH)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyDiningMap_log>()
                .Property(e => e.sTimingToMM)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyDiningMap_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyDiningMap_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyDiningMap_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyDiningMap_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyLocalityMap_log>()
                .Property(e => e.cAreaLocality)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyLocalityMap_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyLocalityMap_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyLocalityMap_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyLocalityMap_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sHotelName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sAddress)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sWebSite)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.dLatitude)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.dLongitude)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sDistanceFromAirportRailwayStation)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sNearestTransport)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sAreaRecommended)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sTopAttractions)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sGeneralManagerName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sRevenueManagerName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sFinanceContactName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sPrimaryContactName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sGeneralManagerContact)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sRevenueManagerContact)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sFinanceContactContact)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sPrimaryContactContact)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sGeneralManagerEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sRevenueManagerEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sFinanceContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sPrimaryContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sAccessbilityIds)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sAwardIds)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sAffiliationIds)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sLanguageIds)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sAreaRecommended1)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sAreaRecommended2)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sAreaRecommended3)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sNearestTransport1)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.dNearestTransport1)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sNearestTransport2)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.dNearestTransport2)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sNearestTransport3)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.dNearestTransport3)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sTopAttraction1)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sTopAttraction2)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sTopAttraction3)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sDistanceToAirportRailway1)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.dDistanceToAirportRailway1)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sDistanceToAirportRailway2)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.dDistanceToAirportRailway2)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sDistanceToAirportRailway3)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.dDistanceToAirportRailway3)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sConfirmationContactName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sConfirmationContactEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.sConfirmationContactContact)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyM_log>()
                .Property(e => e.iVendorId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyParkingMap_log>()
                .Property(e => e.sCarName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyParkingMap_log>()
                .Property(e => e.cAirportRail)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyParkingMap_log>()
                .Property(e => e.dOnewayCharge)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblPropertyParkingMap_log>()
                .Property(e => e.dTwowayCharge)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblPropertyParkingMap_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyParkingMap_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyParkingMap_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyParkingMap_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPhotoMap_log>()
                .Property(e => e.sImgUrl)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPhotoMap_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPhotoMap_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPhotoMap_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPhotoMap_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPolicyMap_log>()
                .Property(e => e.sCreditCardId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPolicyMap_log>()
                .Property(e => e.sCheckInHH)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPolicyMap_log>()
                .Property(e => e.sCheckInMM)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPolicyMap_log>()
                .Property(e => e.sCheckoutHH)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPolicyMap_log>()
                .Property(e => e.sCheckoutMM)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPolicyMap_log>()
                .Property(e => e.dExtraBedCharges)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblPropertyPolicyMap_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPolicyMap_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPolicyMap_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPolicyMap_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPromoRateMap_log>()
                .Property(e => e.dSingleRate)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblPropertyPromoRateMap_log>()
                .Property(e => e.dDoubleRate)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblPropertyPromoRateMap_log>()
                .Property(e => e.dTripleRate)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblPropertyPromoRateMap_log>()
                .Property(e => e.dQuadrupleRate)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblPropertyPromoRateMap_log>()
                .Property(e => e.dQuintrupleRate)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblPropertyPromoRateMap_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPromoRateMap_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPromoRateMap_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPromoRateMap_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPromotionCancellationMainMap_log>()
                .Property(e => e.sCancellationPolicyId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPromotionCancellationMainMap_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPromotionCancellationMainMap_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPromotionCancellationMainMap_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPromotionCancellationMainMap_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPromotionMap_log>()
                .Property(e => e.dValue)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblPropertyPromotionMap_log>()
                .Property(e => e.sRoomTypeId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPromotionMap_log>()
                .Property(e => e.sAmenityId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPromotionMap_log>()
                .Property(e => e.sAmenity)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPromotionMap_log>()
                .Property(e => e.sCancellationPolicy)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPromotionMap_log>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPromotionMap_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPromotionMap_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPromotionMap_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPromotionMap_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyPromotionMap_log>()
                .Property(e => e.dNegotiationPer)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyRatePlanCancellationMainMap_log>()
                .Property(e => e.sCancellationPolicyId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanCancellationMainMap_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanCancellationMainMap_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanCancellationMainMap_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanCancellationMainMap_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanMap_log>()
                .Property(e => e.sRatePlan)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanMap_log>()
                .Property(e => e.cRatePlanType)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanMap_log>()
                .Property(e => e.dValue)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblPropertyRatePlanMap_log>()
                .Property(e => e.sAmenityId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanMap_log>()
                .Property(e => e.sAmenity)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanMap_log>()
                .Property(e => e.cHrsDays)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanMap_log>()
                .Property(e => e.sCancellationPolicy)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanMap_log>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanMap_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanMap_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanMap_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanMap_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanMap_log>()
                .Property(e => e.sRoomId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanMap_log>()
                .Property(e => e.dNegotiationPer)
                .HasPrecision(5, 2);

            modelBuilder.Entity<tblPropertyRatePlanRoomTypeMap_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanRoomTypeMap_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanRoomTypeMap_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRatePlanRoomTypeMap_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRecreationMap_log>()
                .Property(e => e.sRecreationFacilityId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRecreationMap_log>()
                .Property(e => e.sLandActivityId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRecreationMap_log>()
                .Property(e => e.sGolfId)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRecreationMap_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRecreationMap_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRecreationMap_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRecreationMap_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomInventory_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomInventory_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomInventory_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomInventory_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomMap_log>()
                .Property(e => e.sRoomName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomMap_log>()
                .Property(e => e.sRoomCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomMap_log>()
                .Property(e => e.dSizeSqft)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblPropertyRoomMap_log>()
                .Property(e => e.dSizeMtr)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblPropertyRoomMap_log>()
                .Property(e => e.sSizeType)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomMap_log>()
                .Property(e => e.sRoomAccessibilityIds)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomMap_log>()
                .Property(e => e.sBuildingCharacteristicsIds)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomMap_log>()
                .Property(e => e.sRoomOutdoorViewIds)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomMap_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomMap_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomMap_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomMap_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomRatePlanInventoryMap_log>()
                .Property(e => e.dSingleRate)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblPropertyRoomRatePlanInventoryMap_log>()
                .Property(e => e.dDoubleRate)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblPropertyRoomRatePlanInventoryMap_log>()
                .Property(e => e.dTripleRate)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblPropertyRoomRatePlanInventoryMap_log>()
                .Property(e => e.dQuadrupleRate)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblPropertyRoomRatePlanInventoryMap_log>()
                .Property(e => e.dQuintrupleRate)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tblPropertyRoomRatePlanInventoryMap_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomRatePlanInventoryMap_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomRatePlanInventoryMap_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyRoomRatePlanInventoryMap_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertySpaMap_log>()
                .Property(e => e.sSpaName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertySpaMap_log>()
                .Property(e => e.sSpaDesc)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertySpaMap_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertySpaMap_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertySpaMap_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertySpaMap_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyTaxesMap_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyTaxesMap_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyTaxesMap_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyTaxesMap_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyTaxMap_log>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyTaxMap_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyTaxMap_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyTaxMap_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblPropertyTaxMap_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblRankManagement_log>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblRankManagement_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblRankManagement_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblRankManagement_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblRankManagement_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanCancelPenaltyDescTGSearch_Tmp>()
                .Property(e => e.sSessionId)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanCancelPenaltyDescTGSearch_Tmp>()
                .Property(e => e.sHotelCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanCancelPenaltyDescTGSearch_Tmp>()
                .Property(e => e.sRatePlanCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanCancelPenaltyDescTGSearch_Tmp>()
                .Property(e => e.sNonRefundable)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanCancelPenaltyDescTGSearch_Tmp>()
                .Property(e => e.sPenaltyName)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanCancelPenaltyDescTGSearch_Tmp>()
                .Property(e => e.sPenaltyDesc)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanCancelPenaltyDescTGSpecific_Tmp>()
                .Property(e => e.sSessionId)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanCancelPenaltyDescTGSpecific_Tmp>()
                .Property(e => e.sHotelCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanCancelPenaltyDescTGSpecific_Tmp>()
                .Property(e => e.sRatePlanCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanCancelPenaltyDescTGSpecific_Tmp>()
                .Property(e => e.sNonRefundable)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanCancelPenaltyDescTGSpecific_Tmp>()
                .Property(e => e.sPenaltyName)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanCancelPenaltyDescTGSpecific_Tmp>()
                .Property(e => e.sPenaltyDesc)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanCancelPenaltyTGSearch_Tmp>()
                .Property(e => e.sSessionId)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanCancelPenaltyTGSearch_Tmp>()
                .Property(e => e.sHotelCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanCancelPenaltyTGSearch_Tmp>()
                .Property(e => e.sRatePlanCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanCancelPenaltyTGSearch_Tmp>()
                .Property(e => e.sOffSetDropTime)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanCancelPenaltyTGSearch_Tmp>()
                .Property(e => e.sOffSetTimeUnit)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanCancelPenaltyTGSearch_Tmp>()
                .Property(e => e.sOffSetUnitMultiply)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanCancelPenaltyTGSearch_Tmp>()
                .Property(e => e.sNumberOfNight)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanCancelPenaltyTGSearch_Tmp>()
                .Property(e => e.sTaxInclusive)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanCancelPenaltyTGSpecific_Tmp>()
                .Property(e => e.sSessionId)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanCancelPenaltyTGSpecific_Tmp>()
                .Property(e => e.sHotelCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanCancelPenaltyTGSpecific_Tmp>()
                .Property(e => e.sRatePlanCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanCancelPenaltyTGSpecific_Tmp>()
                .Property(e => e.sOffSetDropTime)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanCancelPenaltyTGSpecific_Tmp>()
                .Property(e => e.sOffSetTimeUnit)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanCancelPenaltyTGSpecific_Tmp>()
                .Property(e => e.sOffSetUnitMultiply)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanCancelPenaltyTGSpecific_Tmp>()
                .Property(e => e.sNumberOfNight)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanCancelPenaltyTGSpecific_Tmp>()
                .Property(e => e.sTaxInclusive)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanM_log>()
                .Property(e => e.sRatePlan)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanM_log>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanM_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanM_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanM_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanM_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanTGSearch_Tmp>()
                .Property(e => e.sSessionId)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanTGSearch_Tmp>()
                .Property(e => e.sHotelCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanTGSearch_Tmp>()
                .Property(e => e.sRatePlanType)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanTGSearch_Tmp>()
                .Property(e => e.sRatePlanName)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanTGSearch_Tmp>()
                .Property(e => e.sRatePlanCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanTGSearch_Tmp>()
                .Property(e => e.sRatePlanInclusion)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanTGSearch_Tmp>()
                .Property(e => e.sDiscountCouponEnable)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanTGSearch_Tmp>()
                .Property(e => e.sRatePlanDesc)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanTGSpecific_Tmp>()
                .Property(e => e.sSessionId)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanTGSpecific_Tmp>()
                .Property(e => e.sHotelCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanTGSpecific_Tmp>()
                .Property(e => e.sRatePlanType)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanTGSpecific_Tmp>()
                .Property(e => e.sRatePlanName)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanTGSpecific_Tmp>()
                .Property(e => e.sRatePlanCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanTGSpecific_Tmp>()
                .Property(e => e.sRatePlanInclusion)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanTGSpecific_Tmp>()
                .Property(e => e.sDiscountCouponEnable)
                .IsUnicode(false);

            modelBuilder.Entity<tblRatePlanTGSpecific_Tmp>()
                .Property(e => e.sRatePlanDesc)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomAccessibilityM>()
                .Property(e => e.sRoomAccessibility)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomRateTaxTGSpecific_Tmp>()
                .Property(e => e.sSessionId)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomRateTaxTGSpecific_Tmp>()
                .Property(e => e.sHotelCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomRateTaxTGSpecific_Tmp>()
                .Property(e => e.sRatePlanCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomRateTaxTGSpecific_Tmp>()
                .Property(e => e.sRoomTypeCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomRateTaxTGSpecific_Tmp>()
                .Property(e => e.sCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomRateTGsearch_Tmp>()
                .Property(e => e.sSessionId)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomRateTGsearch_Tmp>()
                .Property(e => e.sHotelCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomRateTGsearch_Tmp>()
                .Property(e => e.sRatePlanCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomRateTGsearch_Tmp>()
                .Property(e => e.sRoomTypeCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomRateTGsearch_Tmp>()
                .Property(e => e.tEffectiveTime)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomRateTGsearch_Tmp>()
                .Property(e => e.sAdditionalGuestAmountRPH)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomRateTGsearch_Tmp>()
                .Property(e => e.sAdditionalGuestAmountAgeQualificationCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomRateTGsearch_Tmp>()
                .Property(e => e.sBookable)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomRateTGsearch_Tmp>()
                .Property(e => e.iBaseChildOccupancy)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomRateTGsearch_Tmp>()
                .Property(e => e.iBaseAdultOccupancy)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomRateTGSpecific_Tmp>()
                .Property(e => e.sSessionId)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomRateTGSpecific_Tmp>()
                .Property(e => e.sHotelCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomRateTGSpecific_Tmp>()
                .Property(e => e.sRatePlanCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomRateTGSpecific_Tmp>()
                .Property(e => e.sRoomTypeCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomRateTGSpecific_Tmp>()
                .Property(e => e.tEffectiveTime)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomRateTGSpecific_Tmp>()
                .Property(e => e.sAdditionalGuestAmountRPH)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomRateTGSpecific_Tmp>()
                .Property(e => e.sAdditionalGuestAmountAgeQualificationCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomRateTGSpecific_Tmp>()
                .Property(e => e.sBookable)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomRateTGSpecific_Tmp>()
                .Property(e => e.iBaseChildOccupancy)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomRateTGSpecific_Tmp>()
                .Property(e => e.iBaseAdultOccupancy)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomTypeTGSearch_Tmp>()
                .Property(e => e.sSessionId)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomTypeTGSearch_Tmp>()
                .Property(e => e.sHotelCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomTypeTGSearch_Tmp>()
                .Property(e => e.sRoomTypeCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomTypeTGSearch_Tmp>()
                .Property(e => e.sRoomType)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomTypeTGSearch_Tmp>()
                .Property(e => e.sNonSmoking)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomTypeTGSpecific_Tmp>()
                .Property(e => e.sSessionId)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomTypeTGSpecific_Tmp>()
                .Property(e => e.sHotelCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomTypeTGSpecific_Tmp>()
                .Property(e => e.sRoomTypeCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomTypeTGSpecific_Tmp>()
                .Property(e => e.sRoomType)
                .IsUnicode(false);

            modelBuilder.Entity<tblRoomTypeTGSpecific_Tmp>()
                .Property(e => e.sNonSmoking)
                .IsUnicode(false);

            modelBuilder.Entity<tblSyncBookingToChannelMgrTracker>()
                .Property(e => e.cSyncStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblSyncBookingToChannelMgrTracker>()
                .Property(e => e.sXMLString)
                .IsUnicode(false);

            modelBuilder.Entity<tblSyncBookingToChannelMgrTracker>()
                .Property(e => e.sError)
                .IsUnicode(false);

            modelBuilder.Entity<tblTaxM_log>()
                .Property(e => e.sTaxName)
                .IsUnicode(false);

            modelBuilder.Entity<tblTaxM_log>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblTaxM_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblTaxM_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblTaxM_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblTaxM_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblUserM_log>()
                .Property(e => e.sUserName)
                .IsUnicode(false);

            modelBuilder.Entity<tblUserM_log>()
                .Property(e => e.sPassword)
                .IsUnicode(false);

            modelBuilder.Entity<tblUserM_log>()
                .Property(e => e.sFirstName)
                .IsUnicode(false);

            modelBuilder.Entity<tblUserM_log>()
                .Property(e => e.sLastName)
                .IsUnicode(false);

            modelBuilder.Entity<tblUserM_log>()
                .Property(e => e.sEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblUserM_log>()
                .Property(e => e.sContact)
                .IsUnicode(false);

            modelBuilder.Entity<tblUserM_log>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblUserM_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblUserM_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblUserM_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblUserM_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblUserPropertyMap_log>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblUserPropertyMap_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblUserPropertyMap_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblUserPropertyMap_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblUserPropertyMap_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblVideoUrlM_log>()
                .Property(e => e.sVideoUrl)
                .IsUnicode(false);

            modelBuilder.Entity<tblVideoUrlM_log>()
                .Property(e => e.sImgUrl)
                .IsUnicode(false);

            modelBuilder.Entity<tblVideoUrlM_log>()
                .Property(e => e.cStatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblVideoUrlM_log>()
                .Property(e => e.vc_changed_by)
                .IsUnicode(false);

            modelBuilder.Entity<tblVideoUrlM_log>()
                .Property(e => e.vc_changed_ip)
                .IsUnicode(false);

            modelBuilder.Entity<tblVideoUrlM_log>()
                .Property(e => e.vc_programname)
                .IsUnicode(false);

            modelBuilder.Entity<tblVideoUrlM_log>()
                .Property(e => e.ch_action)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
