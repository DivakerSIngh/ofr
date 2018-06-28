using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    #region Search Model

    public class TG_Hotel
    {
        public TG_Hotel()
        {
            GalleryImages = new List<TG_Image>();
            POIData = new List<TG_HotelPOI>();
            RoomDetails = new List<TG_RoomDetails>();
            HotelAmenities = new List<TG_HotelAmenity>();
            RoomAmenities = new List<TG_RoomAmenity>();
        }

        public string HotelCode { get; set; }
        public string HotelName { get; set; }

        public string CurrencyCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string CityName { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public string Rating { get; set; }

        #region Basic Information

        public string Area { get; set; }
        public string AreaSeoId { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public string CitySeoId { get; set; }
        public string Description { get; set; }
        public string HotelCategory { get; set; }
        public string HotelType { get; set; }
        public string NumberOfFloors { get; set; }
        public string NumberOfRooms { get; set; }
        public string IsFlexibleCheckIn { get; set; }

        #endregion Basic Information

        public string ReviewImageUrl { get; set; }

        #region Multimedia

        public string MainImageUrl { get; set; }
        public string ThumbnailImageName { get; set; }
        public string ThumbnailUrl { get; set; }

        #endregion

        public List<TG_Image> GalleryImages { get; set; }
        public List<TG_HotelPOI> POIData { get; set; }
        public List<TG_HotelAmenity> HotelAmenities { get; set; }
        public List<TG_RoomDetails> RoomDetails { get; set; }
        public List<TG_RoomAmenity> RoomAmenities { get; set; }


        public string AmenityDescription { get; set; }

        public string RoomImageUrl { get; set; }
    }

    public class TG_Image
    {
        public string Caption { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public string ImageUrlThumb { get; set; }
    }

    public class TG_HotelPOI
    {
        public string POIDistance { get; set; }
        public string POIName { get; set; }
    }

    public class TG_HotelAmenity
    {
        public string code { get; set; }
        public string Description { get; set; }
        public string sAmenity { get; set; }
        public string sHeader { get; set; }
    }

    public class TG_RoomAmenity
    {
        public string iRoomTypeId { get; set; }
        public byte iRank { get; set; }
        public string sRoomFacilites { get; set; }
        public string sImgUrl { get; set; }
    }

    public class TG_RoomDetails
    {
        public TG_RoomDetails()
        {
            TaxInformation = new Dictionary<string, string>();
            RoomAmenities = new List<TG_RoomAmenity>();
            RatePlans = new List<TG_RatePlan>();
        }

        public string RoomId { get; set; }

        public string VendorId { get; set; }

        public string Description { get; set; }

        public List<string> RoomImages { get; set; }

        public List<string> RoomAmenityImages { get; set; }

        public string RoomName { get; set; }

        public string MaxOccupancy { get; set; }

        public int ExtraBedCont { get; set; }

        public decimal ExtraBedCharge { get; set; }

        public string MaxAdult { get; set; }

        public string MaxChild { get; set; }

        public string MaxChildAge { get; set; }

        public string MinChildAge { get; set; }

        public string CurrencyCode { get; set; }

        public string HotelCode { get; set; }

        public string sOutdoorLocation { get; set; }

        public List<TG_RoomAmenity> RoomAmenities { get; set; }

        public Dictionary<string, string> TaxInformation { get; set; }

        public List<TG_RatePlan> RatePlans { get; set; }

        public decimal LowestRate { get; set; }

        public bool HavingExtraBed { get; set; }
    }

    public class TG_RatePlan
    {
        public TG_RatePlan()
        {
            RoomRates = new List<TG_RoomRate>();
        }
        public string RoomId { get; set; }

        public decimal TotalRoomRate { get; set; }

        public decimal TotalTax { get; set; }

        public decimal TotalExtraBedCharge { get; set; }

        public string RatePlanCode { get; set; }

        public int RoomCount { get; set; }

        public string RatePlanName { get; set; }

        public string RateInclusions { get; set; }

        public List<TG_RoomRate> RoomRates { get; set; }

        public string CancellationPolicyDescription { get; set; }

        public bool IsNonRefundable { get; set; }

        public bool IsFreeCancellation { get; set; }

        public string OffsetDropTime { get; set; }

        public string OffsetTimeUnit { get; set; }

        public int OffsetUnitMultiplier { get; set; }

        public int NumberOfNights { get; set; }

        public bool IsTaxInclusive { get; set; }

        public decimal AmountPercent { get; set; }
        public decimal TotalRoomRateWithoutDiscount { get; set; }
    }

    public class TG_RoomRate
    {
        public string RoomId { get; set; }
        public decimal RoomRate { get; set; }
        public string RatePlanCode { get; set; }
        public decimal ExtrabedCharge { get; set; }
        public int RoomCount { get; set; }
        public int RoomOccupancyAdults { get; set; }
        public int TotalAdults { get; set; }
        public int RoomOccupancyChild { get; set; }
        public int TotalChild { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public decimal Tax { get; set; }
        public decimal RatePerRoomPerDay { get; set; }

        public decimal TaxPerDay { get; set; }
        public decimal RatePerRoomPerDayWithoutDiscount { get; set; }
        public bool HasDiscount { get; set; }
    }

    #endregion Search Model

    #region Booking Model


    public class TG_ProvisionalBookingRequestModel
    {
        public TG_ProvisionalBookingRequestModel()
        {
            API_Credential_UserName = ConfigurationManager.AppSettings["UserNameTG"].ToString();
            API_Credential_PropertyId = ConfigurationManager.AppSettings["PropertyIdTG"].ToString();
            API_Credential_Password = ConfigurationManager.AppSettings["PasswordTG"].ToString();
        }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public string ErrorType { get; set; }

        public string API_Credential_PropertyId { get; set; }

        public string API_Credential_Password { get; set; }

        public string API_Credential_UserName { get; set; }

        public string Currency { get; set; }

        public string CorrelationID { get; set; }

        public string CustomerNamePrefix { get; set; }

        public string CustomerGivenName { get; set; }

        public string CustomerSurname { get; set; }

        public string CustomerEmail { get; set; }

        public string CustomerComment { get; set; }

        public string PhoneTechType { get; set; }

        public string PhoneNumber { get; set; }

        public List<string> AddressLine { get; set; }

        public string CityName { get; set; }

        public string PostalCode { get; set; }

        public string StateName { get; set; }

        public string StateCode { get; set; }

        public string CountryName { get; set; }

        public string CountryCode { get; set; }

        public string RoomTypeCode { get; set; }

        public string NumberOfRooms { get; set; }

        public string RatePlanCode { get; set; }

        public List<RoomData> RoomData { get; set; }

        public string CheckIn { get; set; }

        public string CheckOut { get; set; }

        public string AmountBeforeTax { get; set; }

        public string ExtraBedCharge { get; set; }

        public string TaxAmount { get; set; }

        public string HotelCode { get; set; }

        public string UniqueId { get; set; }

        public string UniqueIdType { get; set; }

        public string GuaranteeType { get; set; }

        public string ProfileType { get; set; }

        public string CountryAccessCode { get; set; }

        public string TransactionIdentifier { get; set; }

        public string AreaCityCode { get; set; }
    }

    public class TG_ProvisionalBookingResponseModel
    {
        public bool IsSuccedded { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public string CorrelationID { get; set; }

        public string UniqueId { get; set; }

        public string UniqueIdType { get; set; }


        public string ErrorType { get; set; }
    }

    public class TG_FinalBookingRequestModel
    {

        public TG_FinalBookingRequestModel()
        {
            API_Credential_UserName = ConfigurationManager.AppSettings["UserNameTG"].ToString();
            API_Credential_PropertyId = ConfigurationManager.AppSettings["PropertyIdTG"].ToString();
            API_Credential_Password = ConfigurationManager.AppSettings["PasswordTG"].ToString();
        }

        public string CorrelationID { get; set; }

        public string UniqueId { get; set; }

        public string UniqueIdType { get; set; }

        public string Currency { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorType { get; set; }

        public string ErrorMessage { get; set; }

        public string API_Credential_PropertyId { get; set; }

        public string API_Credential_Password { get; set; }

        public string API_Credential_UserName { get; set; }
    }

    public class TG_FinalBookingResponseModel : TG_ProvisionalBookingRequestModel
    {
        public bool IsSuccedded { get; set; }
    }

    public class TG_CancelBookingRequestModel
    {
        public TG_CancelBookingRequestModel()
        {
            API_Credential_UserName = ConfigurationManager.AppSettings["UserNameTG"].ToString();
            API_Credential_PropertyId = ConfigurationManager.AppSettings["PropertyIdTG"].ToString();
            API_Credential_Password = ConfigurationManager.AppSettings["PasswordTG"].ToString();
        }

        public string API_Credential_PropertyId { get; set; }

        public string API_Credential_Password { get; set; }

        public string API_Credential_UserName { get; set; }

        public string UniqueId { get; set; }

        public string UniqueIdType { get; set; }

        public string[] CustomerNamePrefixArr { get; set; }

        public string[] CustomerGivenNameArr { get; set; }

        public string CustomerSurname { get; set; }

        public string CustomerEmail { get; set; }
      
        public string[] CancelDateArr { get; set; }
    }

    public class TG_CancelBookingResponseModel
    {
        public TG_CancelBookingResponseModel()
        {
            CancelRules = new List<CancelRule>();
        }
        public bool IsSuccedded { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorType { get; set; }

        public string ErrorMessage { get; set; }
       
        public string Status { get; set; }       

        public string AmountRefund { get; set; }

        public string NoOfNightsChargable { get; set; }

        public string UniqueId { get; set; }

        public string Comment { get; set; }

        public List<CancelRule> CancelRules { get; set; }
    }

    public class CancelRule
    {
        public string CancelByDate { get; set; }
        public decimal Amount { get; set; }
    }

    #endregion Booking Model
}