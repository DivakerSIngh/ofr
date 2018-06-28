using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eCorporateCompanyM
    {
        public int iCompanyId { get; set; }

        [StringLength(200, ErrorMessage = "Only 200 characters allowed.")]
        [DisplayName("Company Name")]
        [Required(ErrorMessage = "Please enter Company Name.")]
        public string sCompanyName { get; set; }

        [StringLength(100, ErrorMessage = "Only 100 characters allowed.")]
        [DisplayName("Primary Contact Name")]
        [Required(ErrorMessage = "Please enter Primary Contact Name.")]
        public string sPrimaryContact { get; set; }

        [StringLength(20, ErrorMessage = "Only 20 characters allowed.")]
        [DisplayName("Telephone Number")]
        [RegularExpression(@"[0-9]\d{2,4}\d{6,8}$", ErrorMessage = "Please enter valid Telephone Number.")]
        [Required(ErrorMessage = "Please enter Telephone Number.")]
        public string sTelephoneNumber { get; set; }

        [StringLength(100, ErrorMessage = "Only 100 characters allowed.")]
        [DisplayName("Correspondence Email ")]
        [Required(ErrorMessage = "Please enter Email Address.")]
        [EmailAddress(ErrorMessage = "Email Address is not valid")]
        public string sEmailAddress { get; set; }

        [StringLength(10, ErrorMessage = "Only 10 characters allowed.")]
        [DisplayName("Mobile Number")]
        [Required(ErrorMessage = "Please enter Mobile Number.")]
        [RegularExpression(@"[0-9]{10}$", ErrorMessage = "Please enter valid Mobile Number.")]

        public string sMobileNumber { get; set; }

        [StringLength(1000, ErrorMessage = "Only 1000 characters allowed.")]
        [DisplayName("Domain/Email Address")]
        [Required(ErrorMessage = "Please enter Domain/Email Address.")]
        public string sDomainNames { get; set; }

        public IEnumerable<string> sDomainNameList { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public string cStatus { get; set; }

        public string sActionBy { get; set; }

        public string sActionType { get; set; }


        public System.Data.DataTable DomainDataTable { get; set; }

        [DisplayName("Domain Type")]
        [Required(ErrorMessage = "Please select Domain type.")]
        [Range(1, 2, ErrorMessage = "Please select Domain type.")]
        public DomainType DomainType { get; set; }


        [Display(Name = "PIN Code")]
        [RegularExpression(@"^\d{6}(-\d{4})?$", ErrorMessage = "Invalid PIN Code, Code must be of 6 digit")]
        public int? PinCode { get; set; }

        [Required(ErrorMessage = "Please select country.")]
        public int? CountryId { get; set; }

        public int? StateId { get; set; }

        [Display(Name = "City")]
        public int? CityId { get; set; }

        [Required(ErrorMessage = "Please enter address.")]
        public string Address { get; set; }

        [Display(Name = "Registered Entity Name")]
        public string GstnEntityName { get; set; }

        [Display(Name = "GSTIN Number")]
        //[StringLength(15, MinimumLength = 15, ErrorMessage = "GST Number must be a 15 digit valid character.")]
        [RegularExpression(@"\d{2}[A-Z]{5}\d{4}[A-Z]{1}\d[Z]{1}[A-Z\d]{1}", ErrorMessage = "Please enter a valid GST Number.")]
        public string GstnNumber { get; set; }

        [Display(Name = "Type of Entity")]
        public string GstnEntityType { get; set; }

        [Required(ErrorMessage = "Please select country code.")]
        public string sCountryCode { get; set; }

        public List<eCountryCodePhone> CountryCodePhoneList { get; set; }
    }


    public class eCountryCodePhone
    {
        public int iId { get; set; }
        public string sCountry { get; set; }
        public string sCode { get; set; }
        public string sFrontendCode { get; set; }
    }

    public enum DomainType
    {
        Select = 0,
        Domain = 1,
        Email = 2
    }
}