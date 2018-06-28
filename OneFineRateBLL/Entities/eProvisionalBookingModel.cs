using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eProvisionalBookingRequestModel
    {
        public eProvisionalBookingRequestModel()
        {
            GuaranteeType = "PrePay";
            ProfileType = "1";
            PhoneTechType = "1";
            API_Credential_UserName = ConfigurationManager.AppSettings["UserNameTG"].ToString();
            API_Credential_PropertyId = ConfigurationManager.AppSettings["PropertyIdTG"].ToString();
            API_Credential_Password = ConfigurationManager.AppSettings["PasswordTG"].ToString();
            UniqueId = "";
            UniqueIdType = "";
            CountryAccessCode = "91";

        }

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

        public string TaxAmount { get; set; }

        public string HotelCode { get; set; }

        public string UniqueId { get; set; }

        public string UniqueIdType { get; set; }

        public string GuaranteeType { get; set; }

        public string ProfileType { get; set; }

        public string CountryAccessCode { get; set; }
    }

    public class eProvisionalBookingResponseModel
    {
        public bool IsSuccedded { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public string Currency { get; set; }

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

        public string TaxAmount { get; set; }

        public string HotelCode { get; set; }

        public string CorrelationID { get; set; }

        public string UniqueId { get; set; }

        public string UniqueIdType { get; set; }

        public string GuaranteeType { get; set; }

        public string ProfileType { get; set; }

    }
}