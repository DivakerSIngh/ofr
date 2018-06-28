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
    
    public partial class tblBookingTx
    {
        public tblBookingTx()
        {
            this.tblBookedBidAmenities = new HashSet<tblBookedBidAmenity>();
            this.tblBookingAmedTxes = new HashSet<tblBookingAmedTx>();
            this.tblBookingDetailsAmendTxes = new HashSet<tblBookingDetailsAmendTx>();
            this.tblBookingDetailsTxes = new HashSet<tblBookingDetailsTx>();
            this.tblChannelMgrBookingTxes = new HashSet<tblChannelMgrBookingTx>();
            this.tblCustomerLoyaltyBookingsMaps = new HashSet<tblCustomerLoyaltyBookingsMap>();
        }
    
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
        public Nullable<decimal> dTotalCommOriginal { get; set; }
        public Nullable<decimal> dTaxesOriginal { get; set; }
        public string sProvisionalBookingIdTG { get; set; }
        public string sProvisionalTrxnTypeTG { get; set; }
        public string sFinalBookingIdTG { get; set; }
        public string sFinalTrxnTypeTG { get; set; }
        public Nullable<bool> bSyncToChannelMgr { get; set; }
        public Nullable<System.DateTime> dtSyncToChannelMgr { get; set; }
        public string cSyncStatus { get; set; }
        public Nullable<short> iBidStarCategory { get; set; }
        public string sBidType { get; set; }
        public string sIDs { get; set; }
        public Nullable<decimal> dRefundAmount { get; set; }
        public string cRefundStatus { get; set; }
        public string cAmendStatus { get; set; }
        public string sPaymentMode { get; set; }
        public string sOAuth { get; set; }
        public string sExtra2 { get; set; }
        public string sExtra3 { get; set; }
        public string sExtra4 { get; set; }
        public string sCountryPhoneCode { get; set; }
        public Nullable<int> iCompanyId { get; set; }
        public Nullable<decimal> dServiceCharge { get; set; }
        public Nullable<decimal> dGSTOnServiceCharge { get; set; }
        public string dGSTServiceType { get; set; }
        public string dGSTValue { get; set; }
    
        public virtual ICollection<tblBookedBidAmenity> tblBookedBidAmenities { get; set; }
        public virtual ICollection<tblBookingAmedTx> tblBookingAmedTxes { get; set; }
        public virtual ICollection<tblBookingDetailsAmendTx> tblBookingDetailsAmendTxes { get; set; }
        public virtual ICollection<tblBookingDetailsTx> tblBookingDetailsTxes { get; set; }
        public virtual tblWebsiteUserMater tblWebsiteUserMater { get; set; }
        public virtual ICollection<tblChannelMgrBookingTx> tblChannelMgrBookingTxes { get; set; }
        public virtual ICollection<tblCustomerLoyaltyBookingsMap> tblCustomerLoyaltyBookingsMaps { get; set; }
        public virtual tblRecentSavingsTx tblRecentSavingsTx { get; set; }
        public virtual tblSyncBookingToChannelMgrTracker tblSyncBookingToChannelMgrTracker { get; set; }
    }
}
