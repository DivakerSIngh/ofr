using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.ComponentModel;
using System.Web;
using OneFineRateBLL.Entities;

namespace OneFineRateWeb.Models
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
            RecentlyViewedItems = new List<eCustomerViewedItems>();
            UserMayLikeItems = new List<eCustomerViewedItems>();
            PastSavings = new List<etblBookingTx>();
            CurrentNegotiations = new List<etblBookingTx>();
        }
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }

        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
        public long? OFRPoints { get; set; }
        public string ProfileImageUrl { get; set; }

        public int PastSavingsTotalRecords { get; set; }
        public int CurrentNegotiationsTotalRecords { get; set; }

        public int? EarnMoney { get; set; }
        public int? EarnPoint { get; set; }

        public List<eCustomerViewedItems> RecentlyViewedItems { get; set; }
        public List<eCustomerViewedItems> UserMayLikeItems { get; set; }
        public List<etblBookingTx> PastSavings { get; set; }
        public List<etblBookingTx> CurrentNegotiations { get; set; }
    }

    public class MVCGridToolbarModel
    {
        public MVCGridToolbarModel()
        {

        }
        public MVCGridToolbarModel(string gridName)
        {
            MVCGridName = gridName;
        }
        public string MVCGridName { get; set; }
        public bool PageSize { get; set; }
        public bool ColumnVisibility { get; set; }
        public bool Export { get; set; }
        public bool GlobalSearch { get; set; }
    }

    public class RecentItemViewModel
    {
        public int PropertyId { get; set; }
        public string PageLinkUrl { get; set; }
        public string RoomImageUrl { get; set; }
        public float PropertyStartRating { get; set; }
        public decimal CurrentRoomRate { get; set; }

    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class MyAccountViewModel
    {
        public string FullName { get; set; }
        public string ProfileImageUrl { get; set; }
        public bool HasPassword { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class GovernMentApprovedIdViewModel
    {
        [Required(ErrorMessage = "Please select Id type.")]
        public Id_Type ID_Type { get; set; }
        //[Range(1, int.MaxValue, ErrorMessage = "Please enter valid Number grater than 1")]
        [MaxLength(50, ErrorMessage = "Max 50 character allowed !")]
        [Required(ErrorMessage = "Please enter Id Number.")]
        public string ID_Number { get; set; }

        //[DataType(DataType.Date,ErrorMessage="Date is not a valid format date as dd/mm/yyyy")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public string ExpirationDate { get; set; }
        public HttpPostedFileBase IdImagePostedFile { get; set; }
        public string UploadedPhotoUrl { get; set; }

    }

    public class PreferenceViewModel
    {
        [DisplayName("Smoking Room")]
        public PreferedSmokingRoomType? SmokingRoom { get; set; }

        [DisplayName("Prefered Star Rating")]
        public PreferedStarRatingType? PreferedStarRating { get; set; }

        [StringLength(200)]
        public string SpecialRequest { get; set; }
        public bool NewsNotifications { get; set; }

        public bool Facilities_For_Disabled_Guest { get; set; }
        public bool Fitness_Center { get; set; }
        public bool Internet_Services { get; set; }
        public bool Pets_Allowed { get; set; }
        public bool Luggage_Storage { get; set; }
        public bool Pool { get; set; }

        [Display(Name = "24-hour front desk")]
        public bool Front_Desk_24_Hour { get; set; }
        public bool Restaurant { get; set; }
        public bool Spa_And_Wellness_Center { get; set; }
        public bool Bar { get; set; }
        public bool Family_Rooms { get; set; }
        public bool Airport_Suttle { get; set; }
        public bool Parking { get; set; }
    }


    public class ProfileViewModel
    {
        [EmailAddress(ErrorMessage = "Please enter a valid Email Address.")]
        [Display(Name = "Corporate Email Address")]
        public string CorporateEmail { get; set; }
        public string Email { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }

        [StringLength(40)]
        [Required(ErrorMessage = "Required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(40)]
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        //[DataType(DataType.Date, ErrorMessage = "Date is not a valid format date as dd/mm/yyyy")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Date of Birth")]
        public string DateOfBirth { get; set; }

        [Display(Name = "Martial Status")]
        public MartialStatusType? MartialStatus { get; set; }

        //[DataType(DataType.Date, ErrorMessage = "Date is not a valid format date as dd/mm/yyyy")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Anniversary Date")]
        [System.Web.Mvc.Remote("IsValidAnniversaryDate", "Manage", AdditionalFields = "DateOfBirth", ErrorMessage = "Anniversary date can't before your Birth date !")]
        public string AnniversaryDate { get; set; }

        [StringLength(40)]
        [Display(Name = "Spouse Name")]
        public string SpouseName { get; set; }

        public HttpPostedFileBase ProfileImagePostedFile { get; set; }

        public string ProfileImageUrl { get; set; }

        [StringLength(200)]
        [Display(Name = "Current Address Line1")]
        public string CurrentAddressLine1 { get; set; }

        [StringLength(200)]
        [Display(Name = "Current Address Line2")]
        public string CurrentAddressLine2 { get; set; }

        [StringLength(200)]
        [Display(Name = "Current Address Line3")]
        public string CurrentAddressLine3 { get; set; }

        [Display(Name = "PIN Code")]
        [RegularExpression(@"^\d{6}(-\d{4})?$", ErrorMessage = "Invalid PIN Code, Code must be of 6 digit")]
        public int? PinCode { get; set; }

        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public string City { get; set; }

        public string UniqueAzureFileName { get; set; }

        public string ReferralCode { get; set; }
        public bool NewsNotifications { get; set; }

        public bool PhoneNumberConfirmed { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool CorporateEmailConfirmed { get; set; }

        public string sCountryPhoneCode { get; set; }
        public List<eCountryCodePhone> CountryCodePhoneList { get; set; }

        [Display(Name = "Registered Entity Name")]
        public string GstnEntityName { get; set; }

        [Display(Name = "GSTIN Number")]
        //[StringLength(15, MinimumLength = 15, ErrorMessage = "GST Number must be a 15 digit valid character.")]
        [RegularExpression(@"\d{2}[A-Z]{5}\d{4}[A-Z]{1}\d[Z]{1}[A-Z\d]{1}", ErrorMessage = "Please enter a valid GST Number.")]
        public string GstnNumber { get; set; }

        [Display(Name = "Type of Entity")]
        public string GstnEntityType { get; set; }

    }


    public enum TitleType
    {
        [Display(Name = "Mr.")]
        Mr = 1,

        [Display(Name = "Mrs.")]
        Mrs = 2,

        [Display(Name = "Ms.")]
        Ms = 3,

        [Display(Name = "Dr.")]
        Dr = 4,

        [Display(Name = "Prof.")]
        Prof = 5,

    }

    public enum MartialStatusType
    {
        Single = 1,
        Married = 2
    }

    public enum Id_Type
    {
        [Display(Name = "Voter Id")]
        voterid = 1,
        [Display(Name = "Passport")]
        passport = 2,
        [Display(Name = "Driving Licence")]
        drivinglicence = 3,
        [Display(Name = "Aadhar Card")]
        aadharcard = 4,
        [Display(Name = "PAN Card")]
        pancard = 5
    }

    public enum PreferedStarRatingType
    {
        [Display(Name = "1 Star")]
        one = 1,
        [Display(Name = "2 Star")]
        two = 2,
        [Display(Name = "3 Star")]
        three = 3,
        [Display(Name = "4 Star")]
        four = 4,
        [Display(Name = "5 Star")]
        five = 5,
        [Display(Name = "Any")]
        Any = 6
    }

    public enum PreferedSmokingRoomType
    {
        [Display(Name = "Preferred")]
        Prefered = 1,
        [Display(Name = "Not Prefered")]
        NotPrefered = 2,
        [Display(Name = "No matter")]
        NoMatter = 3,
    }


    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirm password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Please enter your old password.")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Please enter your new password.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirm password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required(ErrorMessage = "Please enter verification code.")]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }
    public class VerifyEmailViewModel
    {
        [Required(ErrorMessage = "Please enter verification code.")]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string HiddenCode { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please enter valid Email Address.")]
        [Display(Name = "Phone Number")]
        public string EmailId { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}