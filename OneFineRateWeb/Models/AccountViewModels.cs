using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OneFineRateWeb.Models
{

    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessage = "Please Select Your Title")]
        public string Title { get; set; }

        public string DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please enter your name.")]
        [Display(Name = "Name")]
        public string DisplayName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your phone number.")]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please enter a valid phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Verification Code")]
        public string PhoneVerificationCode { get; set; }

        public string PhoneVerificationEncoded { get; set; }

        [StringLength(8, MinimumLength = 8, ErrorMessage = "Code must be of 8 character")]
        [Display(Name = "Referral Code")]
        public string RefereeCode { get; set; }

        public string Gender { get; set; }

        public string ProfilePicUrl { get; set; }

        public bool IsCorporateCustomer { get; set; }

        [StringLength(200, MinimumLength = 1, ErrorMessage = "Company name should be between 1 to 200 character.")]
        public string CompanyName { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string CorporateCustomerEmail { get; set; }

        [Display(Name = "Verification Code")]
        public string CorporateEmailVerificationCode { get; set; }

        public string CorporateEmailVerificationEncoded { get; set; }

        public bool IsCorporateRequested { get; set; }
        public string sCountryPhoneCode { get; set; }
        public List<eCountryCodePhone> CountryCodePhoneList { get; set; }
    }


    public class ExternalLoginListViewModel
    {
        public bool IsCorporateRequested { get; set; }

        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Please enter your e-mail address & we will send you a confirmation mail to reset your password.")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your email address.")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Display(Name = "Email Address")]
        //[EmailAddress(ErrorMessage="Please enter a valid email address.")]
        public string EmailOrPhone { get; set; }

        [Required(ErrorMessage = "Please enter your password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string CorporateEmail { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }

        public bool IsCorporateRequested { get; set; }

    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please Select Your Title")]
        //[Range(1, 5, ErrorMessage = "Please Select Your Title")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter first name.")]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter last name.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your phone number.")]
        [StringLength(15, ErrorMessage = "Phone number can not be greater than 15 character")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please enter your password.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [StringLength(8, MinimumLength = 8, ErrorMessage = "Code must be of 8 character")]
        [Display(Name = "Referral Code")]
        public string RefereeCode { get; set; }

        public string sCountryCode { get; set; }
        public List<eCountryCodePhone> CountryCodePhoneList { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string CorporateEmail { get; set; }

        [Display(Name = "Verification Code")]
        public string CorporateVerificationCode { get; set; }

        public string CorporateVerificationEncoded { get; set; }

        public string CompanyName { get; set; }

        public bool IsCorporateRequested { get; set; }

    }


    public class VerifyPhoneViewModel : RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter OTP code.")]
        [Display(Name = "Phone Number")]
        [StringLength(6, ErrorMessage = "Verification code must be of 6 digit.", MinimumLength = 6)]
        public string PhoneVerificationCode { get; set; }

        public string PhoneVerificationEncoded { get; set; }
    }

    public class VerifyCorporateEmailViewModel
    {
        public string CorporateEmail { get; set; }
        
        public string EmailVerificationCode { get; set; }

        public string VerificationEncoded { get; set; }

        public string HiddenCorporateEmail { get; set; }
    }


    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Please provide your email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ManageBooking
    {
        [Required(ErrorMessage = "Please provide booking id.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter only integer value bigger than 0.")]
        public string BookingId { get; set; }
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Phone number is not valid.")]
        public string Mobile { get; set; }
    }
}
