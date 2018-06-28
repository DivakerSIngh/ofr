using OneFineRateBLL.Entities;
using OneFineRateWeb.Handlers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneFineRateWeb.ViewModels
{

    public class CustomerBookingModel
    {

        public int PendingNegotiationsPageCount { get; set; }
        public int FutureBookingsPageCount { get; set; }
        public int PastBookingsPageCount { get; set; }
        public int UnSuccessfullNegotiationsPageCount { get; set; }

        public List<etblBookingTx> PendingNegotiations { get; set; }
        public List<etblBookingTx> FutureBookings { get; set; }
        public List<etblBookingTx> PastBookings { get; set; }
        public List<etblBookingTx> UnSuccessfullNegotiations { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter only integer value bigger than 0.")]
        public string BookingId { get; set; }
        [EmailAddress(ErrorMessage = "Please enter a valid e-mail address.")]
        public string Email { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Phone number is not valid.")]
        public string Mobile { get; set; }
    }

    public class CutomerDetailModel
    {
        public string ProfileImageUrl { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string BirthDate { get; set; }
    }

    [AtLeastOneProperty(ErrorMessage = "You must provide at least one email or phone number.")]
    public class ReferralCodeViewModel
    {
        [Display(Name = "Your Referral Code")]
        public string ReferralCode { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid e-mail address.")]
        public string Email1 { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid e-mail address.")]
        public string Email2 { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid e-mail address.")]
        public string Email3 { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid e-mail address.")]
        public string Email4 { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid e-mail address.")]
        public string Email5 { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid e-mail address.")]
        public string Email6 { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid e-mail address.")]
        public string Email7 { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid e-mail address.")]
        public string Email8 { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid e-mail address.")]
        public string Email9 { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid e-mail address.")]
        public string Email10 { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Mobile number is not valid.")]
        public string Phone1 { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Mobile number is not valid.")]
        public string Phone2 { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Mobile number is not valid.")]
        public string Phone3 { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Mobile number is not valid.")]
        public string Phone4 { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Mobile number is not valid.")]
        public string Phone5 { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Mobile number is not valid.")]
        public string Phone6 { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Mobile number is not valid.")]
        public string Phone7 { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Mobile number is not valid.")]
        public string Phone8 { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Mobile number is not valid.")]
        public string Phone9 { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Mobile number is not valid.")]
        public string Phone10 { get; set; }
    }
}