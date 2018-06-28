//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OneFineRate
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblBookingTx_log
    {
        public long iBookingId { get; set; }
        public Nullable<int> iPropId { get; set; }
        public Nullable<long> iCustomerId { get; set; }
        public Nullable<long> iGuestId { get; set; }
        public Nullable<decimal> iCountryOffset { get; set; }
        public string sCountryTimeZone { get; set; }
        public Nullable<System.DateTime> dtCheckIn { get; set; }
        public Nullable<System.DateTime> dtChekOut { get; set; }
        public Nullable<System.DateTime> dtReservationDate { get; set; }
        public string cBookingType { get; set; }
        public Nullable<byte> iNegotiateAttempts { get; set; }
        public string sPromoCode { get; set; }
        public Nullable<bool> bPromoDiscount { get; set; }
        public string bPromoAmenity { get; set; }
        public string sTitleOFR { get; set; }
        public string sFirstNameOFR { get; set; }
        public string sLastNameOFR { get; set; }
        public string sEmailOFR { get; set; }
        public string sMobileOFR { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
        public string BookingStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string ActualBookingType { get; set; }
        public Nullable<decimal> dTotalNegotiateAmount { get; set; }
        public Nullable<decimal> dAdvNegotiateAmount { get; set; }
        public Nullable<decimal> dBidAmount { get; set; }
        public Nullable<decimal> dTotalAmount { get; set; }
        public Nullable<decimal> dTotalComm { get; set; }
        public Nullable<decimal> dTaxes { get; set; }
        public Nullable<decimal> dTaxesForHotel { get; set; }
        public Nullable<decimal> dTotalExtraBedAmount { get; set; }
        public Nullable<decimal> dDiscountedBidPrice { get; set; }
        public Nullable<decimal> dCustomerPayable { get; set; }
        public Nullable<bool> bIsFeedBackEmailSent { get; set; }
        public Nullable<bool> bIsCustSmsSent { get; set; }
        public Nullable<bool> bIsHotelSmsSent { get; set; }
        public Nullable<bool> bSelfTravelling { get; set; }
        public string sSpecialRequests { get; set; }
        public string sExpectedCheckIn { get; set; }
        public string sSpecialOccasion { get; set; }
        public string spref_single_lady { get; set; }
        public string spref_away_elevator { get; set; }
        public string spref_smoking_room { get; set; }
        public string spref_quiet_room { get; set; }
        public string spref_pickup { get; set; }
        public string spref_extra1 { get; set; }
        public string spref_extra2 { get; set; }
        public string spref_extra3 { get; set; }
        public string sFlightNumber { get; set; }
        public string sEstArrivalTime { get; set; }
        public string sLandingAt { get; set; }
        public string sTypeOfCar { get; set; }
        public Nullable<byte> iNoOFCars { get; set; }
        public Nullable<decimal> iCounterOffer { get; set; }
        public string sCurrencyCode { get; set; }
        public Nullable<long> iLinkedBookingId { get; set; }
        public Nullable<bool> bInvBlocked { get; set; }
        public string sExtra1 { get; set; }
        public Nullable<decimal> dccPrice { get; set; }
        public Nullable<decimal> dccDiscount { get; set; }
        public Nullable<decimal> dSafeguardComm { get; set; }
        public string ccType { get; set; }
        public string vc_changed_by { get; set; }
        public string vc_changed_ip { get; set; }
        public string vc_programname { get; set; }
        public Nullable<System.DateTime> dt_changed_date { get; set; }
        public string ch_action { get; set; }
        public string sPaymentMode { get; set; }
        public string sOAuth { get; set; }
        public string sExtra2 { get; set; }
        public string sExtra3 { get; set; }
        public string sExtra4 { get; set; }
        public Nullable<int> iCompanyId { get; set; }
        public Nullable<decimal> dServiceCharge { get; set; }
        public Nullable<decimal> dGSTOnServiceCharge { get; set; }
        public string dGSTServiceType { get; set; }
    }
}
