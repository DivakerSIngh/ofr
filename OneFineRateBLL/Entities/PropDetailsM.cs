using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;

namespace OneFineRateBLL.Entities
{
    public class PropDetailsM : ListetblPhotoGallery
    {
        public PropDetailsM()
        {
            lstetblPhotoGallery = new List<etblPhotoGallery>();
            lstetblHotelFacilities = new List<etblHotelFacilities>();
            lstetblRooms = new List<etblRooms>();
            lstetblCreditCards = new List<etblCreditCards>();
            lstPropertyDiningAndRestaurants = new List<PropertyDiningAndRestaurants>();
            objPropertyWellnessFacilities = new PropertyWellnessFacilities();
            objBooking = new etblBookingTx();
            lstOccData = new List<OccupancyData>();
            lstGuests = new List<etblBookingGuestMap>();
            objTripAdvisonReviews = new etblPropertyTripAdvisorM();
            lstTripAdvisonReviews = new List<etblTripAdvisorReviews>();
            lstPropertyParkingTransport = new List<PropertyParkingTransport>();
            lstDeletedBookingDetails = new List<int>();
            lstTaxesDateWise_OfferReview = new List<TaxesDateRoomRateWise>();
        }
        public etblPropertyAwards Awards { get; set; }
        public List<etblPhotoGallery> lstetblPhotoGallery { get; set; }
        public etblTaxCharges TaxCharges { get; set; }
        public List<etblRoomMaxTaxes> MaxCharges { get; set; }
        public List<etblRooms> lstetblRooms { get; set; }
        public List<etblCreditCards> lstetblCreditCards { get; set; }
        public List<etblHotelFacilities> lstetblHotelFacilities { get; set; }
        public List<TaxesDateRoomRateWise> lstTaxesDateWise_OfferReview { get; set; }
        public List<TaxesSeprateDateRoomRateWise> lstTaxesDateWiseAll_OfferReview { get; set; }

        public List<PropertyDiningAndRestaurants> lstPropertyDiningAndRestaurants { get; set; }
        public PropertyWellnessFacilities objPropertyWellnessFacilities { get; set; }
        public etblBookingTx objBooking { get; set; }
        public List<etblBookingGuestMap> lstGuests { get; set; }
        public List<OccupancyData> lstOccData { get; set; }
        public etblPropertyTripAdvisorM objTripAdvisonReviews { get; set; }
        public List<etblTripAdvisorReviews> lstTripAdvisonReviews { get; set; }

        public List<PropertyParkingTransport> lstPropertyParkingTransport { get; set; }
        public List<CreditCards> lstCreditCards { get; set; }
        public List<eBookingRoomM> BookingRoomDetails { get; set; }
        public List<int> lstDeletedBookingDetails { get; set; }


        public int iPropId { get; set; }
        public string iVendorId { get; set; }
        public Int16 iStarCategory { get; set; }
        public string cStatus { get; set; }
        public string sExtra1 { get; set; }


        public string sAreaRecommended1 { get; set; }
        public string sAreaRecommended2 { get; set; }
        public string sAreaRecommended3 { get; set; }
        public string sNearestTransport { get; set; }
        public string sNearestTransport1 { get; set; }
        public Nullable<decimal> dNearestTransport1 { get; set; }
        public string sNearestTransport2 { get; set; }
        public Nullable<decimal> dNearestTransport2 { get; set; }
        public string sNearestTransport3 { get; set; }
        public Nullable<decimal> dNearestTransport3 { get; set; }
        public string sTopAttraction { get; set; }
        public string sTopAttraction1 { get; set; }
        public string sTopAttraction2 { get; set; }
        public string sTopAttraction3 { get; set; }
        public string sDistanceToAirportRailway { get; set; }
        public string sDistanceToAirportRailway1 { get; set; }
        public Nullable<decimal> dDistanceToAirportRailway1 { get; set; }
        public string sDistanceToAirportRailway2 { get; set; }
        public Nullable<decimal> dDistanceToAirportRailway2 { get; set; }
        public string sDistanceToAirportRailway3 { get; set; }
        public Nullable<decimal> dDistanceToAirportRailway3 { get; set; }

        public Nullable<decimal> dLatitude { get; set; }
        public Nullable<decimal> dLongitude { get; set; }
        public string slargeMapURL { get; set; }

        public string sDescription { get; set; }


        public string sCheckInHH { get; set; }
        public string sCheckInMM { get; set; }
        public string sCheckoutHH { get; set; }
        public string sCheckoutMM { get; set; }
        public byte? iMinCheckInAge { get; set; }
        public Nullable<decimal> dExtraBedCharges { get; set; }
        public byte? iExtraBedRequiredFrom { get; set; }
        public byte? iComplimentaryStayAge { get; set; }
        public bool? bPetsAllowed { get; set; }

        public bool? bAlcoholAllowedOnsite { get; set; }
        public bool? bAlcoholServedOnsite { get; set; }
        public bool? bVisitorsAllowed { get; set; }
        public bool? bPartiesAllowed { get; set; }

        public string sCreditCardId { get; set; }

        public string sPropertyName { get; set; }
        public string RatingImageUrl { get; set; }
        public string RatingString { get; set; }
        public string sPropertyAddress { get; set; }

        public string sRoomName { get; set; }

        public string sInternetFacility { get; set; }
        public string sParkingTransport { get; set; }
        public string sCommonServices { get; set; }
        public string sConvenience { get; set; }
        public string sLeisure { get; set; }
        public string sMeetingsConferences { get; set; }

        public string sAwardsCertificates { get; set; }
        public string sAffiliations { get; set; }
        public string sLanguagesSpoken { get; set; }
        public string sCity { get; set; }


        public long iUserId { get; set; }
       
        public long iGuestId { get; set; }
        public int LoctionId { get; set; }

        public int iStateId { get; set; }

        public string sUserTitle { get; set; }
        public string sUserFirstName { get; set; }
        public string sUserLastName { get; set; }
        public string sUserEmail { get; set; }
        public string sUserMobileNo { get; set; }

        public string sVerificationCode { get; set; }


        public DateTime dtCheckIn { get; set; }
        public DateTime dtCheckOut { get; set; }
        public bool bLogin { get; set; }
        public string Currency { get; set; }
        public string PropertyCurrency { get; set; }
        public bool? bIsAirConditioning { get; set; }
        public bool? bIsBathtub { get; set; }
        public bool? bIsFlatScreenTV { get; set; }
        public bool? bIsSoundproof { get; set; }
        public bool? bIsView { get; set; }
        public bool? bIsInternetFacilities { get; set; }
        public bool? bIsPrivatePool { get; set; }
        public bool? bIsRoomService { get; set; }
        public decimal dMinPrice { get; set; }
        public decimal dMaxPrice { get; set; }
        public bool? SpecialDeal { get; set; }
        public string RoomOccupancySearch { get; set; }
        public string ChildrenAgeSearch { get; set; }
        public string GuestData { get; set; }
        public string hdnJsonData { get; set; }
        public string scheckIn { get; set; }
        public string scheckInYear { get; set; }
        public string scheckOut { get; set; }
        public string scheckOutYear { get; set; }
        public string iTotalDays { get; set; }

        public decimal dSummaryTotal { get; set; }
        public decimal dSummaryRoomRate { get; set; }
        public decimal dSummaryTaxes { get; set; }
        public decimal dSummaryExtraBedCharges { get; set; }
        public decimal dSummaryGrandTotal { get; set; }
        //To show data on Rate Summary 
        public string sSummaryTotal { get; set; }
        public string sSummaryRoomRate { get; set; }
        public string sSummaryTaxes { get; set; }
        public string sSummaryExtraBedCharges { get; set; }
        public string sSummaryGrandTotal { get; set; }

        public string sSummaryTotal_display { get; set; }
        public string sSummaryRoomRate_display { get; set; }
        public string sSummaryTaxes_display { get; set; }
        public string sSummaryExtraBedCharges_display { get; set; }
        public string sSummaryGrandTotal_display { get; set; }

        public decimal dDiscountedBidPrice { get; set; }
        public decimal dNegotiationAmt { get; set; }

        //To show data on Rate Summary
        public string sNegotiationAmtMax { get; set; }
        public decimal dNegotiationAmtMax { get; set; }

        public int iNoOfRooms { get; set; }
        public string Symbol { get; set; }
        public string sPromoCode { get; set; }
        public decimal dCommissionRate { get; set; }
        public string sActionType { get; set; }
        public string sOccuDataModified { get; set; }
        public bool? bOccuData { get; set; }

        public string sHoteParkingRadio { get; set; }
        public bool? bAirportTransferAvalible { get; set; }

        public bool? b24HrsCheckIn { get; set; }
        public bool? bEarlyCheckInChargeable { get; set; }
        public bool? b24HrsCheckout { get; set; }
        public bool? bEarlyCheckoutChargeable { get; set; }
        public string sCheckInDate { get; set; }
        public string sCheckOutDate { get; set; }
        public string sCheckInDate_Display { get; set; }
        public string sCheckOutDate_Display { get; set; }
        public string sRoomData { get; set; }
        public string sRequestType { get; set; }
        public TG_Hotel TG_Hotel { get; set; }
        public string sRoomId { get; set; }
        public string sRatePlanCode { get; set; }
        public Nullable<decimal> iCountryOffset { get; set; }

        public decimal dExchangeRate { get; set; }
        public decimal? dOFRServiceCharge { get; set; }
        public bool? bRoomAvailable { get; set; }

        /// <summary>
        /// Used for TravelGuru 
        /// </summary>
        public eServiceChargeM ServiceChargeTG { get; set; }        

        //for regular booking amendments
        public decimal? dTotalCancellationCharges { get; set; }
        public decimal? dTotalAmountPaid { get; set; }
        public string sImageUrlTG { get; set; }
        public string sAuthCode { get; set; }

        public string sCountryPhoneCode { get; set; }

        public List<eCountryCodePhone> CountryCodePhoneList { get; set; }

        public RoomDetails BidRoomDetails { get; set; }
        public string TempDataPropDetails { get; set; }
        public string TempDataOldRooms { get; set; }
        public string TempDataRoomCancellationCharges { get; set; }
        public bool IsDataHasSaved { get; set; }
        public string sTripAdvisorRankingString { get; set; }
        public string sTotalPoints { get; set; }
    }

    public class TaxesDateRoomRateWise
    {
        public DateTime dtDate { get; set; }
        public int RoomID { get; set; }
        public int RPID { get; set; }
        public bool? bIsPromo { get; set; }
        public int iOccupancy { get; set; }
        public decimal? dBasePrice { get; set; }
        public decimal? TaxPer { get; set; }
        public decimal? TaxVal { get; set; }
    }
    public class TaxesSeprateDateRoomRateWise
    {
        public string dtStay { get; set; }
        public int iRoomId { get; set; }
        public int RPID { get; set; }
        public string sTaxName { get; set; }
        public int iOccupancy { get; set; }
        public decimal? dPrice { get; set; }
        public decimal? MaxTaxPer { get; set; }
        public decimal? MaxTaxVal { get; set; }
        public int TaxId { get; set; }
        public string TaxName { get; set; }
    }

    public class OccupancyData
    {
        public short Occupancy { get; set; }
        public short Rooms { get; set; }
        public short Total { get; set; }
    }

    public class PropertyNames
    {
        public short Id { get; set; }
        public string Name { get; set; }
    }
    public class CreditCards
    {
        public string sImg { get; set; }
    }


    public class GuestUserDetails
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string sCountryPhoneCode { get; set; }

        public int iStateId { get; set; }
    }


    public class PropertyDiningAndRestaurants
    {
        public int iPropId { get; set; }
        public string sRestaurantName { get; set; }
        public string cType { get; set; }
        public string sDescription { get; set; }
        public bool bBreakfast { get; set; }
        public bool bLunch { get; set; }
        public bool bDinner { get; set; }
        public bool bActive { get; set; }
        public string sTimingFromHH { get; set; }
        public string sTimingFromMM { get; set; }
        public string sTimingToHH { get; set; }
        public string sTimingToMM { get; set; }
        public string sMealServed { get; set; }
        public bool? s24hours { get; set; }

    }

    public class PropertyParkingTransport
    {
        public int iPropId { get; set; }
        public string sCarName { get; set; }
        public string cAirportRail { get; set; }
        public bool? bIsFree { get; set; }
        public decimal? dOnewayCharge { get; set; }
        public decimal? dTwowayCharge { get; set; }
    }


    public class PropertyWellnessFacilities
    {
        public int iPropId { get; set; }
        public string sSpaName { get; set; }
        public string sSpaDesc { get; set; }
        public bool bHotsprings { get; set; }
        public bool bSauna { get; set; }
        public bool bMudbath { get; set; }
        public bool bSpaTub { get; set; }
        public bool bAdvancedBooking { get; set; }
        public bool bSteamRoom { get; set; }
        public bool b24hours { get; set; }

        public string sPointers { get; set; }
        public string sIsSPA { get; set; }
        public string sIsAdvanceBooking { get; set; }

    }

    public class RoomDetails
    {
        public string sDescription { get; set; }
        public string sRoomName { get; set; }
        public string sSizeSqft { get; set; }
        public string sSizeMtr { get; set; }
        public string sSingleBed { get; set; }
        public string sDoubleBed { get; set; }
        public string sOutdoorLocation { get; set; }
        public string sDefaultImage { get; set; }
        public List<RoomImages> Images { get; set; }
        public List<RoomImages> ThumbImages { get; set; }
        public List<RoomPolicy> Policy { get; set; }
        public List<RoomAmenities> RoomAmenities { get; set; }

        public string CancellationPolicy { get; set; }
        public string ExtrabedCharge { get; set; }
        public string RateInclusions { get; set; }
        public int iMaxOccupancy { get; set; }

        public string Amenity1 { get; set; }
        public string Amenity2 { get; set; }
        public string Amenity3 { get; set; }
        public string Amenity4 { get; set; }
        public string Amenity5 { get; set; }
        public string Amenity6 { get; set; }
        public string Amenity7 { get; set; }
        public string Amenity8 { get; set; }

        public int MaxAdultOccupancy { get; set; }
        public int MaxChildOccupancy { get; set; }

          
        public List<string> FirstFourNonEmptyAmenityUrl()
        {
            List<string> firstFourAmenityUrl = new List<string>();

            var properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var allAmenityProperty = properties.Where(x => x.Name.Contains("Amenity") && x.PropertyType.Name == "String").ToList();

            foreach (var item in allAmenityProperty)
            {
                var tValue = this.GetType().GetProperty(item.Name).GetValue(this, null).ToString();

                if (!string.IsNullOrEmpty(tValue))
                {
                    firstFourAmenityUrl.Add(tValue);
                }
            }
            return firstFourAmenityUrl.Take(4).ToList();
        }
    }
    public class RoomImages
    {
        public string sRoomImage { get; set; }
    }
    public class RoomPolicy
    {
        public int iExtraBedRequiredFrom { get; set; }
        public int iComplimentaryStayAge { get; set; }
        public string sAge { get; set; }
        public string sExtraCharge { get; set; }

        public Nullable<short> iMax_Adult_Occupancy { get; set; }
        public Nullable<short> iMax_Child_Occupancy { get; set; }
        public Nullable<short> iMax_Infant_Occupancy { get; set; }
        public Nullable<short> iMax_Guest_Occupancy { get; set; }
    }
    public class RoomAmenities
    {
        public string sAmenity { get; set; }
    }

    // -->> Nogotiation Information <<--
    public class BookingInformation
    {
        public int iPropertyId { get; set; }

        public string sSelfTraveling { get; set; }
        public string sSingleLady { get; set; }
        public string sElevator { get; set; }
        public string sSmokingRoom { get; set; }
        public string sQuietRoom { get; set; }
        public string sPickUp { get; set; }
        public string sSpecialOccasion { get; set; }
        public string sSpecialRequest { get; set; }
        public string sExpectedCheckInTime { get; set; }
        public string sFilghtNumber { get; set; }
        public string sETA { get; set; }
        public string sTypeofCars { get; set; }
        public string sLandingAt { get; set; }
        public string sNumberofCars { get; set; }
        public List<BookingRoomInformation> BookingRoomInformation { get; set; }
    }
    public class BookingRoomInformation
    {
        public int iPropertyId { get; set; }
        public int iRoomId { get; set; }
        public int iRatePlanId { get; set; }
        public int sRoomName { get; set; }
        public string sGuestName { get; set; }
        public string sEmailId { get; set; }
        public string sMobileNo { get; set; }
        public string BedPreferance { get; set; }
    }

    public class dNegotiationAmount
    {
        public decimal Price { get; set; }
    }

    public class eComplementaryNegoRoomData
    {
        public int Id { get; set; }
        public int iHotelID { get; set; }
        public int iRoomId { get; set; }
        public int iRPID { get; set; }
        public decimal? dPrice { get; set; }
        public decimal? dTax { get; set; }
        public decimal? dDiscPrice { get; set; }
        public string sHotelName { get; set; }
        public string sArea { get; set; }
        public int iStarCategoryId { get; set; }
        public string sRatingImageURL { get; set; }
        public string sRankingString { get; set; }
        public string sRoomName { get; set; }
        public string Size { get; set; }
        public string sImgUrl { get; set; }
        public string Symbol { get; set; }
        public decimal? CommissionRate { get; set; }
        public int iSelected { get; set; }

        public decimal dServiceCharge { get; set; }

        public decimal dTaxOnServiceCharge { get; set; }

    }
    public class eComplementaryRoomFacilities
    {
        public int iHotelId { get; set; }
        public string Amen { get; set; }
        public string Appl { get; set; }
    }
    public class eComplementaryViews
    {
        public int iPropId { get; set; }
        public string sView { get; set; }
    }
    public class eComplementaryFacility
    {
        public int iPropId { get; set; }
        public string sFacility { get; set; }
    }
}