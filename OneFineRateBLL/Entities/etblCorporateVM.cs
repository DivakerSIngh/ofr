using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblCorporateVM
    {
       
        [StringLength(200, ErrorMessage = "Only 200 characters allowed.")]
        [DisplayName("Company Name")]
        [Required(ErrorMessage = "Please enter company name.")]
        public string sCompanyName { get; set; }

        [StringLength(100, ErrorMessage = "Only 100 characters allowed.")]
        [DisplayName("Primary Contact")]
        [Required(ErrorMessage = "Please enter Primary Contact.")]
        public string sPrimaryContact { get; set; }

        [StringLength(20, ErrorMessage = "Only 20 characters allowed.")]
        [DisplayName("Telephone Number")]
        [Required(ErrorMessage = "Please enter Telephone number.")]
        [RegularExpression(@"^\d{7,11}$", ErrorMessage = "Please enter valid Telephone Number.")]
        public string sTelephoneNumber { get; set; }

        [StringLength(100, ErrorMessage = "Only 100 characters allowed.")]
        [DisplayName("Email Address")]
        [Required(ErrorMessage = "Please enter Email Address.")]
        [EmailAddress(ErrorMessage = "Email Address is not valid.")]
        public string sEmailAddress { get; set; }

        [StringLength(10, ErrorMessage = "Only 10 characters allowed.")]
        [DisplayName("Mobile Number")]
        [Required(ErrorMessage = "Please enter Mobile Number.")]
        public string sMobileNumber { get; set; }

    }
}