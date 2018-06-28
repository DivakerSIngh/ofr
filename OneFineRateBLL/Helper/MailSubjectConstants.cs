using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public class MailSubjectConstants
    {
        public const string NewNegotiation = "OneFineRate- New Bargain! Confirmation No : {0}";
        public const string Negotiation = "OneFineRate- Bargain! Confirmation No : {0}";
        public const string BookingModifyUser = "OneFineRate- Modification! Confirmation No: {0}";
        public const string BookingModifyAdmin = "OneFineRate- Modification! Confirmation No: {0}";
        public const string NewBookingUser = "OneFineRate- New Booking! Confirmation No: {0}";
        public const string NewBookingAdmin = "OneFineRate- New Booking! Confirmation No: {0}";
        public const string CancelBookingUser = "OneFineRate-Cancellation! Confirmation No: {0}";
        public const string CancelBookingAdmin = "OneFineRate-Cancellation! Confirmation No: {0}";
        public const string RefundSubjectToUser = "OneFineRate-Refund! Confirmation No: {0}";
        public const string RefundSubjectToAdmin = "OneFineRate-Refund! Confirmation No: {0}";
        public const string EmailVerication = "Email Verification";
        public const string ReferralCode = "Referral Code";
        public const string NegotiationDetail = "Bargain Detail";
    }
}