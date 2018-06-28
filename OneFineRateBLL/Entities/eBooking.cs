using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eBookingSearch
    {
        public string sDateType { get; set; }
        public string dtFrom { get; set; }
        public string dtTo { get; set; }
        public string sType { get; set; }
        public string sConfirmationNo { get; set; }
        public string sFirstName { get; set; }
        public string sLastName { get; set; }
        public string sEmail { get; set; }
    }

    public class eBooking
    {
        public long Sno { get; set; }
        public long ConfirmationNo { get; set; }
        public DateTime BookingDate { get; set; }
        public string GuestComment { get; set; }
        public string GuestName { get; set; }
        public string EmailId { get; set; }
        public string Booker { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string Status { get; set; }
        public string Total { get; set; }
        public string Commission { get; set; }
        public string Type { get; set; }
        public string ShowEdit { get; set; }
        public string sBookingId { get; set; }
    }

    public class eBookingModify
    {
        public string TempDataOldRooms { get; set; }
        public string TempDataPropDetails { get; set; }
        public string BookingId { get; set; }
        public string ReservationDate { get; set; }
        public string BookingStatus { get; set; }
        public string Reservation { get; set; }
        public string CheckIn { get; set; }
        public string ChekOut { get; set; }
        public string Booker { get; set; }
        public string EmailOFR { get; set; }
        public string MobileOFR { get; set; }
        public string CountryPhoneCode { get; set; }
        public string HotelName { get; set; }
        public string RatingImageUrl { get; set; }
        public string RatingString { get; set; }
        public string StreetAddress { get; set; }
        public string Address { get; set; }
        public string StarCategoryId { get; set; }
        public string AvgAmount { get; set; }

        public string HotelRatePerNight { get; set; }

        public string TotalAmount { get; set; }
        public decimal ExtraBedAmount { get; set; }
        public decimal? dccPrice { get; set; }
        public decimal? dccDiscount { get; set; }
        public decimal? dBidAmount { get; set; }
        public string Tax { get; set; }
        public string TaxPerNight { get; set; }
        public decimal dTaxes { get; set; }
        public decimal sExtra2 { get; set; }
        public string sExtra4 { get; set; }

        public string sExtra3 { get; set; }

        public string Comm { get; set; }
        public decimal dTaxesForHotel { get; set; }
        public string SubTotal { get; set; }
        
        public string RefundAmount { get; set; }

        public bool IsForHotel { get; set; }
        public string sRatePlanName { get; set; }
        public int NoOfRooms { get; set; }

        public int NoOfNight { get; set; }

        public string SpecialRequest { get; set; }
        public string ExpectedCheckIn { get; set; }
        public string SpecialOccasion { get; set; }

        public string Pickup { get; set; }
        public string FlightNumber { get; set; }
        public string EstimateArrivalTime { get; set; }
        public string Landing { get; set; }
        public string TypeOfCar { get; set; }
        public string NoOfCar { get; set; }

        public string SingleLady { get; set; }
        public string SmokingRoom { get; set; }
        public string Elevator { get; set; }
        public string QuietRoom { get; set; }
        public string sExtra1 { get; set; }
        public decimal ExtraBedPerNight { get; set; }
        public string Symbol { get; set; }
        public string sCurrencyCode { get; set; }
        public string sImgUrl { get; set; }
        public string sCheckInHH { get; set; }
        public string sCheckInMM { get; set; }
        public string sCheckoutHH { get; set; }
        public string sCheckoutMM { get; set; }
        public bool b24HrsCheckIn { get; set; }
        public bool bEarlyCheckInChargeable { get; set; }
        public bool b24HrsCheckout { get; set; }
        public bool bEarlyCheckoutChargeable { get; set; }

        public byte? iMinCheckInAge { get; set; }
        public Nullable<decimal> dExtraBedCharges { get; set; }
        public int? iExtraBedRequiredFrom { get; set; }
        public int? iComplimentaryStayAge { get; set; }
        public bool bPetsAllowed { get; set; }

        public bool bAlcoholAllowedOnsite { get; set; }
        public bool bAlcoholServedOnsite { get; set; }
        public bool bVisitorsAllowed { get; set; }
        public bool bPartiesAllowed { get; set; }

        public Nullable<decimal> dLatitude { get; set; }
        public Nullable<decimal> dLongitude { get; set; }
        public string sCity { get; set; }
        public string slargeMapURL { get; set; }

        public decimal? dDiscountedBidPrice { get; set; }
        public decimal? dTotalNegotiateAmount { get; set; }
        public decimal? dAdvNegotiateAmount { get; set; }
        public decimal? dBalanceAmt { get; set; }
        public decimal? dSubTotalNego { get; set; }
        public decimal? dOrginalSubTotalNego { get; set; }
        public decimal? dCounterOffer { get; set; }
        public decimal? dSubTotalCounter { get; set; }
        public decimal? dCustomerPayable { get; set; }
        public string cBookingType { get; set; }
        public string cBookingStatus { get; set; }
        public string ccType { get; set; }
        public string ActualBookingType { get; set; }
        public bool? IsNegotiationAccepted { get; set; }
        public int iLinkedBookingId { get; set; }
        public string dOfrServiceCharge { get; set; }
        public string dOfrGSTONServiceCharge { get; set; }
        public string dOfrGSTONServiceChargePercent { get; set; }
        public string dGuestRoomRatePerNight_Bid { get; set; }
        public string dValueType { get; set; }
        public string dValue { get; set; }

        public Nullable<bool> bPromoDiscount { get; set; }
        public string bPromoAmenity { get; set; }

        public string dTotalPayableToHotel { get; set; }

        public string dHotelRatePerNight { get; set; }

        public List<eBookingRoomM> BookingRoomDetails { get; set; }
        public List<eBookingRoomM> Deleted_BookingRoomDetails { get; set; }
        public List<CreditCards> lstCreditCards { get; set; }
        //public List<eBookingRatePlan> BookingRatePlans { get; set; }

        public int PropId { get; set; }

        public PropDetailsM PropDetailM_OnlyWithRooms { get; set; }

        public string sCheckIn { get; set; }
        public string sCheckOut { get; set; }
        // referred to show rate information for booking
        public DateTime? dtCheckIn { get; set; }
        public DateTime? dtCheckOut { get; set; }
        public string sRoomData { get; set; }
      

        public string sMessage { get; set; }

        // For travelguru
        public eBookingRoomM DefaultRoomDetail { get; set; }

        public bool IsTG { get; set; }
        public string sProvisionalBookingIdTG { get; set; }
        public string sProvisionalTrxnTypeTG { get; set; }
        public string sFinalBookingIdTG { get; set; }
        public string sFinalTrxnTypeTG { get; set; }
        public string sVendorId { get; set; }

        public string CompanyName { get; set; }
        public int CompanyId { get; set; }

        //End For travelguru


        //TO DO
        //Implement after GST changes

        public string dGstOnCommissionPercent { get; set; }
        public string dGstOnCommission { get; set; }
        public string dTotalCommission { get; set; }
        
        public string EntityName { get; set; }
        public string GstnNumber { get; set; }
        public string TypeOfEntity { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public bool GST_Same_State { get; set; }  
        public string SGST_Per { get; set; }
        public string SGST_Val { get; set; }
        public string EntityAddress { get; set; }
        public string dHotelPublicRatePerNight_Bid { get; set; }
        public string dDiscount_Bid { get; set; }
        public string dDiscountRatePerNight_Bid { get; set; }
        public string dTotalDiscountedRate_Bid { get; set; }
        public string dGuestBidAmountPerNight_Bid { get; set; }

        public string dTotalGuestBidRate_Bid { get; set; }

        public string dCommissionPer { get; set; }
        public decimal dHotelGrossCharges { get; set; }
        
        public decimal dCustomerGrossCharges { get; set; }

        public decimal dCustomerTotalCharges { get; set; }

        /// <summary>
        /// Will Store Cancellation charges for partial or full cancellation after cancel booking event. (Used to Show cancellation Charges on Cancellation confirmation email)
        /// </summary>
        public string CancellationCharge { get; set; }
        public decimal ExtraBedAmountAsPolicy { get; set; }
        public decimal ExtraBedChargesAsPolicy { get; set; }

        //End Implement after GST changes

    }

    public class eBookingRoomM
    {
        public string iBookingDetailsId { get; set; }
        public string RoomNo { get; set; }
        public string RoomId { get; set; }
        public string RPId { get; set; }
        public string RoomName { get; set; }
        public string RatePlan { get; set; }
        public string GuestName { get; set; }
        public string Adult { get; set; }
        public string Children { get; set; }
        public string sChildrenAge { get; set; }
        public string Occupancy { get; set; }
        public string BedPrefer { get; set; }
        public string ExtraBed { get; set; }
        public string PolicyName { get; set; }
        public string AmenityRatePlan { get; set; }
        public Nullable<decimal> RoomRate { get; set; }
        public Nullable<decimal> dTaxes { get; set; }
        public Nullable<decimal> dExtraBedRate { get; set; }
        public bool IsActive { get; set; }

        // For TravelGuru
        public string sRoomImageUrl { get; set; }
        public int MinChildAge { get; set; }
        public List<TG_RoomAmenity> RoomAmenities { get; set; }
        public int MaxChildOccupancy { get; set; }
        public int MaxAdultOccupancy { get; set; }

        public int MaxChildAge { get; set; }
    }

    public class eBookingRatePlan
    {
        public string RoomName { get; set; }
        public string RatePlanName { get; set; }
        public string PolicyName { get; set; }
        public string CancelationValidFrom { get; set; }
        public string CancelationValidTo { get; set; }
        public string Amenities { get; set; }
    }

    public partial class etblBookingDetailsTx
    {
        public long iBookingDetailsID { get; set; }
        public long iBookingId { get; set; }
        public string iRoomId { get; set; }
        public string iRPId { get; set; }
        public string sRoomName { get; set; }
        public string sRPName { get; set; }
        public Nullable<short> iOccupancy { get; set; }
        public Nullable<short> iRooms { get; set; }
        public Nullable<decimal> dRoomRate { get; set; }
        public Nullable<decimal> dExtraBedRate { get; set; }
        public Nullable<decimal> dRoomDiscountedRate { get; set; }
        public Nullable<decimal> dTaxes { get; set; }
        public Nullable<decimal> dTaxesForHotel { get; set; }
        public Nullable<decimal> dBasicDiscountBid { get; set; }
        public Nullable<decimal> dLOSDiscountBid { get; set; }
        public Nullable<decimal> dLeadDiscountBid { get; set; }
        public Nullable<decimal> dWWDiscountBid { get; set; }
        public Nullable<decimal> dMRDiscountBid { get; set; }

        public string sAmenityRatePlan { get; set; }
        public string sAmenity1BasicBid { get; set; }
        public string sApplicability1BasicBid { get; set; }
        public string sAmenity2BasicBid { get; set; }
        public string sApplicability2BasicBid { get; set; }
        public string sAmenity1LOSBid { get; set; }
        public string sApplicability1LOSBid { get; set; }
        public string sAmenity2LOSBid { get; set; }
        public string sApplicability2LOSBid { get; set; }
        public string sAmenity1LeadBid { get; set; }
        public string sApplicability1LeadBid { get; set; }
        public string sAmenity2LeadBid { get; set; }
        public string sApplicability2LeadBid { get; set; }
        public string sAmenity1WWBid { get; set; }
        public string sApplicability1WWBid { get; set; }
        public string sAmenity2WWBid { get; set; }
        public string sApplicability2WWBid { get; set; }
        public string sAmenity1MRBid { get; set; }
        public string sApplicability1MRBid { get; set; }
        public string sAmenity2MRBid { get; set; }
        public string sApplicability2MRBid { get; set; }
        public Nullable<decimal> dUpgradeCharge { get; set; }
        public Nullable<short> iPromoType { get; set; }
        public Nullable<short> iAdults { get; set; }
        public Nullable<short> iChildren { get; set; }
        public string sChildrenAge { get; set; }
        public Nullable<short> iExtraBeds { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }

        //Made by us
        public Nullable<decimal> dBasicPromo { get; set; }
        public Nullable<decimal> dMSPromo { get; set; }
        public Nullable<decimal> dEBPromo { get; set; }
        public Nullable<decimal> dLMPromo { get; set; }
        public Nullable<decimal> d24HPromo { get; set; }
        public string sAmenitiesBasicPromo { get; set; }
        public string sAmenitiesMSPromo { get; set; }
        public string sAmenitiesEBPromo { get; set; }
        public string sAmenitiesLMPromo { get; set; }
        public string sAmenities24HPromo { get; set; }
        public string sAmenitiesFreePromo { get; set; }

        public string sGuestName { get; set; }
        public string sGuestEmail { get; set; }
        public string sGuestMobile { get; set; }
        public string sBedPreference { get; set; }
        public string sCountryPhoneCode { get; set; }
        public bool? bIsPrimary { get; set; }

        public DateTime? dtCheckIn { get; set; }
        public DateTime? dtCheckOut { get; set; }
    }
    public class etblBookingCancellationPolicyMap
    {
        public long iID { get; set; }
        public Nullable<long> iBookingDetailsID { get; set; }
        public string sPolicyName { get; set; }
        public System.DateTime dtDate { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public string iRPId { get; set; }
        public bool bTgTaxInclusive { get; set; }
    }

    public class etblPropertyTripAdvisorM
    {
        public int iPropId { get; set; }
        public Nullable<int> iTripAdvisorLOCID { get; set; }
        public string sRatingImageURL { get; set; }
        public Nullable<decimal> iRating { get; set; }
        public string sRankingString { get; set; }
        public Nullable<int> iReviewsCount { get; set; }
        public string sWebURL { get; set; }
        public decimal? iLocationRating { get; set; }
        public decimal? iSleepQualityRating { get; set; }
        public decimal? iRoomsRating { get; set; }
        public decimal? iServiceRating { get; set; }
        public decimal? iValueRating { get; set; }
        public decimal? iCleanlinessRating { get; set; }
        public string sLocationRatingURL { get; set; }
        public string sSleepQualityRatingURL { get; set; }
        public string sRoomsRatingURL { get; set; }
        public string sServiceRatingURL { get; set; }
        public string sValueRatingURL { get; set; }
        public string sCleanlinessRatingURL { get; set; }
        public long? Review_Rating_Point1 { get; set; }
        public long? Review_Rating_Point2 { get; set; }
        public long? Review_Rating_Point3 { get; set; }
        public long? Review_Rating_Point4 { get; set; }
        public long? Review_Rating_Point5 { get; set; }
        public string write_review { get; set; }
    }
    public class etblTripAdvisorReviews
    {
        public int iTripAdvisorLOCID { get; set; }
        public int iReviewId { get; set; }
        public string sTitle { get; set; }
        public string sText { get; set; }
        public string sReviewer { get; set; }
        public Nullable<decimal> iRating { get; set; }
        public string sRatingImageURL { get; set; }
        public string sReviewURL { get; set; }
        public Nullable<System.DateTime> dtReviewDate { get; set; }
        public Nullable<System.DateTime> dtTravelDate { get; set; }
        public string sTripType { get; set; }
        public string sUserLocation { get; set; }
        public string sDayMonth { get; set; }
        public string sYear { get; set; }
        public int iPropId { get; set; }
        public bool bIsJointlyCollected { get; set; }
       

    }
    public class etblPropertyAwards
    {
        public string sSmallImg { get; set; }
    }
    public class NegotiationApplicable
    {
        public int Status { get; set; }
        public decimal BidAmount { get; set; }
    }
    public class eBiddingSearch
    {
        public string type { get; set; }
        public string ctype { get; set; }
        public string cname { get; set; }
        public string BidSearchData { get; set; }
        public string BidData { get; set; }
        public int iBookingId { get; set; }
        public string sJsonLocality { get; set; }
        public string sJsonRoomData { get; set; }
        public string sSearchType { get; set; }
        public string sSearchName { get; set; }
        public string sCheckIn { get; set; }
        public string sCheckOut { get; set; }
        public string Symbol { get; set; }
        public int sSearchId { get; set; }
        public string sSelectedAreaId { get; set; }
        public int sStarRating { get; set; }
        public decimal dBidPrice { get; set; }
        public int iBiddingCount { get; set; }
        public string MinTaxPer { get; set; }
        public List<CheckBoxListBidding> sPrefrencesItems { get; set; }
        public List<Int32> SelectedPrefrences { get; set; }
        public List<GetLocalitiesName> lstLocalities { get; set; }

        public List<etblIndianLocalityCordinate> lstPolygonData { get; set; }

        public string sLocalityData { get; set; }
        public string sLocalityType { get; set; }

        public long iUserId { get; set; }
        public long iGuestId { get; set; }
        public string sUserTitle { get; set; }
        public string sUserFirstName { get; set; }
        public string sUserLastName { get; set; }
        public string sUserEmail { get; set; }
        public string sUserMobileNo { get; set; }
        public string sCountryPhoneCode { get; set; }

        public int iStateId { get; set; }
        public List<eCountryCodePhone> CountryCodePhoneList { get; set; }



        public string sVerificationCode { get; set; }
        public string sActionType { get; set; }
        public string sOTP { get; set; }

        public string sCheckInDay { get; set; }
        public string sCheckInYear { get; set; }
        public string sCheckOutDay { get; set; }
        public string sCheckOutYear { get; set; }
        public string sCheckIn_Display { get; set; }
        public string sCheckOut_Display { get; set; }

        public int iRooms { get; set; }
        public int iAdults { get; set; }
        public int iChildrens { get; set; }
        public int iNights { get; set; }
        public decimal? dMaxRange { get; set; }
        public decimal? dMinRange { get; set; }
        public decimal? dTotalPrice { get; set; }
        public decimal? dTaxes { get; set; }
        public decimal? dGrandTotal { get; set; }

        public int? Status { get; set; }
        public string Message { get; set; }
        public string TopMessage { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Tax { get; set; }
        public List<GetHotelPreviews> lstHotelData { get; set; }

        public string sOFRServiceCharge { get; set; }

        public string sOFRServiceChargeIncludingTax { get; set; }

        public string sHotelTax { get; set; }
        public string cGstValueType { get; set; }
        public decimal dGSTValue { get; set; }

        public string sTaxOnServiceCharge_Val { get; set; }

        public string sTaxOnServiceCharge_Per { get; set; }

        public bool bShowIGST { get; set; }

        public string SGST_Per { get; set; }

        public string SGST_Val { get; set; }
    }
    public class CheckBoxListBidding
    {
        public bool Selected { get; set; }
        public long Value { get; set; }
        public string Text { get; set; }
    }
    public class GetNewBookingDetails
    {
        public long ID { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
    public class GetLocalitiesName
    {
        public string Name { get; set; }
    }
    public class GetHotelPreviews
    {
        public string Name { get; set; }
    }
    public class ePaymentFailure
    {
        public string ErrorMessage { get; set; }
        public string ErrorType { get; set; }
        public int BookingId { get; set; }
    }
    public class eBiddingCounts
    {
        public long iId { get; set; }
        public string sMobile { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
    }
    public class etblBIDRoomAdultsTx
    {
        public long iBookingId { get; set; }
        public short iRoomNo { get; set; }
        public Nullable<short> iAdults { get; set; }
        public Nullable<short> iChildren { get; set; }
        public string sChildAge { get; set; }
    }
    public class eBookingShareInfo
    {
        public string sUserName { get; set; }
        public string sBookedBy { get; set; }
        public long iBookingId { get; set; }
        public string sImgUrl { get; set; }
        public string HotelDesc { get; set; }
        public string sHotelName { get; set; }
        public Nullable<int> iPropId { get; set; }
        public string sDescription { get; set; }

        [Required(ErrorMessage="Please enter an email address.")]
        [EmailAddress(ErrorMessage="Please enter a valid email address.")]
        public string sMailTo { get; set; }
        public string PropertyId { get; set; }
        public string BookingId { get; set; }
        public string iVendorId { get; set; }
    }



    public class BookingAmedCancelation
    {

        public List<eBookingAmendCancelRoomCharges> lstRoomCharges { get; set; }
        public List<eBookingAmendCancelBookingRoomIdWiseCharges> lstBookingRoomCharges { get; set; }
        public decimal? TotalCanellationCharges { get; set; }
    }
    public class eBookingAmendCancelRoomCharges
    {

        public string sRoomName { get; set; }
        public decimal? CanCharges { get; set; }

    }
    public class eBookingAmendCancelBookingRoomIdWiseCharges
    {
        public long iBookingDetailsID { get; set; }
        public decimal? CanCharges { get; set; }

    }


}