using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcCheckBoxList;
using System.ComponentModel;
using OneFineRateAppUtil;

namespace OneFineRateBLL.Entities
{
    public class etblPropertyM_User : IValidatableObject
    {
        public int iPropId { get; set; }
        [Required(ErrorMessage="This field is required.")]
        [DisplayName("Hotel Name")]
        [StringLength(50, ErrorMessage = "Only 50 characters allowed.")]
        [Remote("ValidateHotelName", "Property", ErrorMessage = "Hotel name already exist.", AdditionalFields = "InitialHotelName")]
        public string sHotelName { get; set; }
        [DisplayName("Chain")]
        public Nullable<int> iChainId { get; set; }
        [Required(ErrorMessage="This field is required.")]
        [DisplayName("Accomodation")]
        public Nullable<int> iAccomodationId { get; set; }

        [Required(ErrorMessage="This field is required.")]
        [DisplayName("Year of built")]
        [Remote("ValidateYearBuilt", "Property", ErrorMessage = "Year of built should be between 1500 and current year")]
        public Nullable<short> iYearBuilt { get; set; }

        [DisplayName("Last Renovation")]
        [Remote("ValidateLastRenovation", "Property", ErrorMessage = "Year of renovation should not be greater than current year")]
        public Nullable<short> iLastRenovation { get; set; }


        [Required(ErrorMessage="This field is required.")]
        [DisplayName("No. of Rooms")]
        [DefaultValue(1)]
        public Nullable<short> iRooms { get; set; }
        [DisplayName("No. of Floors")]
        public Nullable<byte> iFloors { get; set; }
        [DisplayName("No. of Tower/ Buildings")]
        public Nullable<byte> iTower { get; set; }
        [DisplayName("Address")]
        [Required(ErrorMessage="This field is required.")]
        [StringLength(200, ErrorMessage = "Only 200 characters allowed.")]
        public string sAddress { get; set; }
        [Required(ErrorMessage="This field is required.")]
        [DisplayName("Country")]
        public int iCountryId { get; set; }
        [Required(ErrorMessage="This field is required.")]
        [DisplayName("State")]
        public int iStateId { get; set; }
        [Required(ErrorMessage="This field is required.")]
        [DisplayName("City")]
        public int iCityId { get; set; }
        [Required(ErrorMessage="This field is required.")]
        [DisplayName("Locality")]
        public Nullable<int> iLocalityId { get; set; }
        [Required(ErrorMessage="This field is required.")]
        [DisplayName("Macro Area")]
        public Nullable<int> iAreaId { get; set; }
        [Required(ErrorMessage="This field is required.")]
        [DisplayName("Pincode")]
        public Nullable<int> iPinCode { get; set; }
        [Required(ErrorMessage="This field is required.")]
        [StringLength(250, ErrorMessage = "Only 250 characters allowed.")]
        [DisplayName("Website")]
        //[RegularExpression(@"^http(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([=a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$", ErrorMessage = "Please Enter Valid URL. Ex: http://www.example.com")]
       // [RegularExpression(@"^(http(s?)\:\/\/)?:[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([=a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$", ErrorMessage = "Please Enter Valid URL. Ex: http://www.example.com")]
        //[RegularExpression(@"(http(s?)://)?(www\.)?[A-Za-z0-9-]+\.[a-z]{2,3}", ErrorMessage = "Please Enter Valid URL. Ex: http://www.example.com")]
        //[Url(ErrorMessage = "Please Enter Valid URL. Ex: http://www.example.com")]
        public string sWebSite { get; set; }

        [Required(ErrorMessage="This field is required.")]
        [DisplayName("Latitude")]
        public Nullable<decimal> dLatitude { get; set; }

        [Required(ErrorMessage="This field is required.")]
        [DisplayName("Longitude")]
        public Nullable<decimal> dLongitude { get; set; }
        public string iVendorId { get; set; }
        public Nullable<bool> bIsTG { get; set; }
        [Required(ErrorMessage="This field is required.")]
        [StringLength(5000, ErrorMessage = "Only 5000 characters allowed.")]
        [DisplayName("Description")]
        public string sDescription { get; set; }

        //[Required(ErrorMessage="This field is required.")]
        //[StringLength(200, ErrorMessage = "Only 200 characters allowed.")]
        //[DisplayName("Distance from Airport Railway Station")]
        //public string sDistanceFromAirportRailwayStation { get; set; }

        //[Required(ErrorMessage="This field is required.")]
        //[StringLength(200, ErrorMessage = "Only 200 characters allowed.")]
        //[DisplayName("Nearest Transport")]
        //public string sNearestTransport { get; set; }

        //[Required(ErrorMessage="This field is required.")]
        //[StringLength(200, ErrorMessage = "Only 200 characters allowed.")]
        //[DisplayName("Area Recommended")]
        //public string sAreaRecommended { get; set; }

        //[Required(ErrorMessage="This field is required.")]
        //[DisplayName("Top Attractions")]
        //[StringLength(200, ErrorMessage = "Only 200 characters allowed.")]
        //public string sTopAttractions { get; set; }

        [StringLength(50, ErrorMessage = "Only 50 characters allowed.")]
        public string sGeneralManagerName { get; set; }

        [StringLength(50, ErrorMessage = "Only 50 characters allowed.")]
        public string sRevenueManagerName { get; set; }

        [Required(ErrorMessage="This field is required.")]
        [StringLength(50, ErrorMessage = "Only 50 characters allowed.")]
        [DisplayName("Finance Contact Person")]
        public string sFinanceContactName { get; set; }

        [Required(ErrorMessage="This field is required.")]
        [StringLength(50, ErrorMessage = "Only 50 characters allowed.")]
        [DisplayName("Primary Contact Name")]
        public string sPrimaryContactName { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "Phone number is not valid .")]
        [StringLength(15, ErrorMessage = "Only 15 numeric allowed.")]
        public string sGeneralManagerContact { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "Phone number is not valid .")]
        [StringLength(15, ErrorMessage = "Only 15 numeric allowed.")]
        public string sRevenueManagerContact { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "Phone number is not valid .")]
        [StringLength(15, ErrorMessage = "Only 15 numeric allowed.")]
        public string sFinanceContactContact { get; set; }

        [StringLength(15, ErrorMessage = "Only 15 numeric allowed.")]
        [DataType(DataType.PhoneNumber,ErrorMessage="Phone number is not valid .")]
        public string sPrimaryContactContact { get; set; }

        [StringLength(50, ErrorMessage = "Only 50 characters allowed.")]
        public string sConfirmationContactName { get; set; }

        [EmailAddress(ErrorMessage="Email not valid !")]
        [StringLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string sConfirmationContactEmail { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "Phone number is not valid .")]
        [StringLength(15, ErrorMessage = "Only 15 numeric allowed.")]
        public string sConfirmationContactContact { get; set; }

        [EmailAddress(ErrorMessage = "Email not valid !")]
        [StringLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string sGeneralManagerEmail { get; set; }

        [EmailAddress(ErrorMessage = "Email not valid !")]
        [StringLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string sRevenueManagerEmail { get; set; }

        [EmailAddress(ErrorMessage = "Email not valid !")]
        [StringLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string sFinanceContactEmail { get; set; }

        [EmailAddress(ErrorMessage = "Email not valid !")]
        [StringLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string sPrimaryContactEmail { get; set; }

        public string sAccessbilityIds { get; set; }
        public string sAwardIds { get; set; }
        public string sAffiliationIds { get; set; }
        public string cStatus { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
        //[Required(ErrorMessage="This field is required.")]
        [DisplayName("Star Category")]
        public Nullable<short> iStarCategoryId { get; set; }
        //[Required(ErrorMessage="This field is required.")]
        [DisplayName("Currency")]
        public Nullable<short> iCurrencyId { get; set; }
        public string sLanguageIds { get; set; }
        public List<SelectListItem> StateList { get; set; }
        public List<SelectListItem> CityList { get; set; }
        public List<SelectListItem> AreaList { get; set; }
        public List<SelectListItem> LocalityList { get; set; }
        public List<CheckBoxList> AccessbilityItems { get; set; }
        public List<Int32> SelectedAccessbility { get; set; }
        public List<CheckBoxList> AwardsItems { get; set; }
        public List<Int32> SelectedAwards { get; set; }
        public List<CheckBoxList> AffilationItems { get; set; }
        public List<Int32> SelectedAffilations { get; set; }
        public List<CheckBoxList> LanguagesItems { get; set; }
        public List<Int32> SelectedLanguages { get; set; }
        public string SelectPrimaryLocalities { get; set; }
        public List<etblPropertyLocalityMap> PropertyLocalityMapList { get; set; }

        public string sviewType { get; set; }
        public string ActionBy { get; set; }


        //For view purpose only
        public string sHotelNameO { get; set; }
        public Nullable<int> iChainIdO { get; set; }
        public Nullable<int> iCountryIdO { get; set; }
        public Nullable<int> iStateIdO { get; set; }
        public Nullable<int> iCityIdO { get; set; }
        public Nullable<int> iLocalityIdO { get; set; }
        public Nullable<int> iAreaIdO { get; set; }
        public string Mode { get; set; }

        public Nullable<decimal> dMinIncome { get; set; }
        public Nullable<System.DateTime> dtMinIncomeActionDate { get; set; }
        public Nullable<int> iMinIncomeActionBy { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
           
            var piYearBuilt = new[] { "iLastRenovation" };
            if (iLastRenovation.ToString() != "" && Convert.ToInt16(iYearBuilt) > Convert.ToInt16(iLastRenovation))
            {
                yield return new ValidationResult("Please enter valid last renovation.", piYearBuilt);
            }
            var sNT1 = new[] { "sNearestTransport1" };
            if (!string.IsNullOrEmpty(sNearestTransport1))
            {
                if (dNearestTransport1 == null)
                {
                    yield return new ValidationResult("*", sNT1);
                }
            }
            var dNT1 = new[] { "dNearestTransport1" };
            if (dNearestTransport1 != null) 
            {
                if (string.IsNullOrEmpty(sNearestTransport1))
                {
                    yield return new ValidationResult("*", dNT1);
                }
            }

            var sNT2 = new[] { "sNearestTransport2" };
            if (!string.IsNullOrEmpty(sNearestTransport2))
            {
                if (dNearestTransport2 == null)
                {
                    yield return new ValidationResult("*", sNT2);
                }
            }
            var dNT2 = new[] { "dNearestTransport2" };
            if (dNearestTransport2 != null)
            {
                if (string.IsNullOrEmpty(sNearestTransport2))
                {
                    yield return new ValidationResult("*", dNT2);
                }
            }

            var sNT3 = new[] { "sNearestTransport3" };
            if (!string.IsNullOrEmpty(sNearestTransport3))
            {
                if (dNearestTransport3 == null)
                {
                    yield return new ValidationResult("*", sNT3);
                }
            }
            var dNT3 = new[] { "dNearestTransport3" };
            if (dNearestTransport3 != null)
            {
                if (string.IsNullOrEmpty(sNearestTransport3))
                {
                    yield return new ValidationResult("*", dNT3);
                }
            }

            var sDTA1 = new[] { "sDistanceToAirportRailway1" };
            if (!string.IsNullOrEmpty(sDistanceToAirportRailway1))
            {
                if (dDistanceToAirportRailway1 == null)
                {
                    yield return new ValidationResult("*", sDTA1);
                }
            }
            var dDTA1 = new[] { "dDistanceToAirportRailway1" };
            if (dDistanceToAirportRailway1 != null)
            {
                if (string.IsNullOrEmpty(sDistanceToAirportRailway1))
                {
                    yield return new ValidationResult("*", dDTA1);
                }
            }

            var sDTA2 = new[] { "sDistanceToAirportRailway2" };
            if (!string.IsNullOrEmpty(sDistanceToAirportRailway2))
            {
                if (dDistanceToAirportRailway2 == null)
                {
                    yield return new ValidationResult("*", sDTA2);
                }
            }
            var dDTA2 = new[] { "dDistanceToAirportRailway2" };
            if (dDistanceToAirportRailway2 != null)
            {
                if (string.IsNullOrEmpty(sDistanceToAirportRailway2))
                {
                    yield return new ValidationResult("*", dDTA2);
                }
            }

            var sDTA3 = new[] { "sDistanceToAirportRailway3" };
            if (!string.IsNullOrEmpty(sDistanceToAirportRailway3))
            {
                if (dDistanceToAirportRailway3 == null)
                {
                    yield return new ValidationResult("*", sDTA3);
                }
            }
            var dDTA3 = new[] { "dDistanceToAirportRailway3" };
            if (dDistanceToAirportRailway3 != null)
            {
                if (string.IsNullOrEmpty(sDistanceToAirportRailway3))
                {
                    yield return new ValidationResult("*", dDTA3);
                }
            }
        }

        [StringLength(100, ErrorMessage = "Only 100 characters allowed.")]
        public string sAreaRecommended1 { get; set; }

        [StringLength(100, ErrorMessage = "Only 100 characters allowed.")]
        public string sAreaRecommended2 { get; set; }

        [StringLength(100, ErrorMessage = "Only 100 characters allowed.")]
        public string sAreaRecommended3 { get; set; }
       // public string sAreaRecommended4 { get; set; }

        [StringLength(100, ErrorMessage = "Only 100 characters allowed.")]
        public string sNearestTransport1 { get; set; }
        [DefaultValue(null)]
        public decimal? dNearestTransport1 { get; set; }

        [StringLength(100, ErrorMessage = "Only 100 characters allowed.")]
        public string sNearestTransport2 { get; set; }
        [DefaultValue(null)]
        public decimal? dNearestTransport2 { get; set; }

        [StringLength(100, ErrorMessage = "Only 100 characters allowed.")]
        public string sNearestTransport3 { get; set; }
        [DefaultValue(null)]
        public decimal? dNearestTransport3 { get; set; }

       // public string sNearestTransport4 { get; set; }
       // public decimal dNearestTransport4 { get; set; }

        [StringLength(100, ErrorMessage = "Only 100 characters allowed.")]
        public string sTopAttraction1 { get; set; }

        [StringLength(100, ErrorMessage = "Only 100 characters allowed.")]
        public string sTopAttraction2 { get; set; }

        [StringLength(100, ErrorMessage = "Only 100 characters allowed.")]
        public string sTopAttraction3 { get; set; }
        //public string sTopAttraction4 { get; set; }

        [StringLength(100, ErrorMessage = "Only 100 characters allowed.")]
        public string sDistanceToAirportRailway1 { get; set; }
        [DefaultValue(null)]
        public decimal? dDistanceToAirportRailway1 { get; set; }

        [StringLength(100, ErrorMessage = "Only 100 characters allowed.")]
        public string sDistanceToAirportRailway2 { get; set; }
        [DefaultValue(null)]
        public decimal? dDistanceToAirportRailway2 { get; set; }

        [StringLength(100, ErrorMessage = "Only 100 characters allowed.")]
        public string sDistanceToAirportRailway3 { get; set; }
        [DefaultValue(null)]
        public decimal? dDistanceToAirportRailway3 { get; set; }

        //public string sDistanceToAirportRailway4 { get; set; }
        //public decimal dDistanceToAirportRailway4 { get; set; }

        //public string sDistanceToAirportRailway5 { get; set; }
        //public decimal dDistanceToAirportRailway5 { get; set; }

        //public string sDistanceToAirportRailway6 { get; set; }
        //public decimal dDistanceToAirportRailway6 { get; set; }

        //public string sDistanceToAirportRailway7 { get; set; }
        //public decimal dDistanceToAirportRailway7 { get; set; }

        //public string sDistanceToAirportRailway8 { get; set; }
        //public decimal dDistanceToAirportRailway8 { get; set; }

     

    }
}