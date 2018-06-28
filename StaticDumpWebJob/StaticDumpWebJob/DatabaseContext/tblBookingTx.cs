namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblBookingTx")]
    public partial class tblBookingTx
    {
        public tblBookingTx()
        {
            tblBookedBidAmenities = new HashSet<tblBookedBidAmenity>();
            tblBookingAmedTxes = new HashSet<tblBookingAmedTx>();
            tblBookingDetailsAmendTxes = new HashSet<tblBookingDetailsAmendTx>();
            tblChannelMgrBookingTxes = new HashSet<tblChannelMgrBookingTx>();
        }

        [Key]
        public long iBookingId { get; set; }

        public int? iPropId { get; set; }

        public long? iCustomerId { get; set; }

        public long? iGuestId { get; set; }

        public decimal? iCountryOffset { get; set; }

        [StringLength(500)]
        public string sCountryTimeZone { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtCheckIn { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtChekOut { get; set; }

        public DateTime? dtReservationDate { get; set; }

        [StringLength(1)]
        public string cBookingType { get; set; }

        public byte? iNegotiateAttempts { get; set; }

        [StringLength(20)]
        public string sPromoCode { get; set; }

        public bool? bPromoDiscount { get; set; }

        [StringLength(50)]
        public string bPromoAmenity { get; set; }

        [StringLength(50)]
        public string sTitleOFR { get; set; }

        [StringLength(50)]
        public string sFirstNameOFR { get; set; }

        [StringLength(50)]
        public string sLastNameOFR { get; set; }

        [StringLength(100)]
        public string sEmailOFR { get; set; }

        [StringLength(20)]
        public string sMobileOFR { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        [StringLength(2)]
        public string BookingStatus { get; set; }

        [StringLength(2)]
        public string PaymentStatus { get; set; }

        [StringLength(1)]
        public string ActualBookingType { get; set; }

        public decimal? dTotalNegotiateAmount { get; set; }

        public decimal? dAdvNegotiateAmount { get; set; }

        public decimal? dBidAmount { get; set; }

        public decimal? dTotalAmount { get; set; }

        public decimal? dTotalComm { get; set; }

        public decimal? dTaxes { get; set; }

        public decimal? dTaxesForHotel { get; set; }

        public decimal? dTotalExtraBedAmount { get; set; }

        public decimal? dDiscountedBidPrice { get; set; }

        public decimal? dCustomerPayable { get; set; }

        public bool? bIsFeedBackEmailSent { get; set; }

        public bool? bIsCustSmsSent { get; set; }

        public bool? bIsHotelSmsSent { get; set; }

        public bool? bSelfTravelling { get; set; }

        [StringLength(500)]
        public string sSpecialRequests { get; set; }

        [StringLength(30)]
        public string sExpectedCheckIn { get; set; }

        [StringLength(100)]
        public string sSpecialOccasion { get; set; }

        [StringLength(3)]
        public string spref_single_lady { get; set; }

        [StringLength(3)]
        public string spref_away_elevator { get; set; }

        [StringLength(3)]
        public string spref_smoking_room { get; set; }

        [StringLength(3)]
        public string spref_quiet_room { get; set; }

        [StringLength(3)]
        public string spref_pickup { get; set; }

        [StringLength(3)]
        public string spref_extra1 { get; set; }

        [StringLength(3)]
        public string spref_extra2 { get; set; }

        [StringLength(3)]
        public string spref_extra3 { get; set; }

        [StringLength(10)]
        public string sFlightNumber { get; set; }

        [StringLength(10)]
        public string sEstArrivalTime { get; set; }

        [StringLength(50)]
        public string sLandingAt { get; set; }

        [StringLength(50)]
        public string sTypeOfCar { get; set; }

        public byte? iNoOFCars { get; set; }

        public decimal? iCounterOffer { get; set; }

        [StringLength(3)]
        public string sCurrencyCode { get; set; }

        public long? iLinkedBookingId { get; set; }

        public bool? bInvBlocked { get; set; }

        [StringLength(50)]
        public string sExtra1 { get; set; }

        public decimal? dccPrice { get; set; }

        public decimal? dccDiscount { get; set; }

        public decimal? dSafeguardComm { get; set; }

        [StringLength(2)]
        public string ccType { get; set; }

        public decimal? dTotalCommOriginal { get; set; }

        public decimal? dTaxesOriginal { get; set; }

        [StringLength(12)]
        public string sBookingIdTG { get; set; }

        [StringLength(12)]
        public string sTrxnTypeTG { get; set; }

        public bool? bSyncToChannelMgr { get; set; }

        public DateTime? dtSyncToChannelMgr { get; set; }

        [StringLength(1)]
        public string cSyncStatus { get; set; }

        public short? iBidStarCategory { get; set; }

        [StringLength(1)]
        public string sBidType { get; set; }

        [StringLength(4000)]
        public string sIDs { get; set; }

        public decimal? dRefundAmount { get; set; }

        [StringLength(1)]
        public string cRefundStatus { get; set; }

        [StringLength(1)]
        public string cAmendStatus { get; set; }

        public virtual ICollection<tblBookedBidAmenity> tblBookedBidAmenities { get; set; }

        public virtual ICollection<tblBookingAmedTx> tblBookingAmedTxes { get; set; }

        public virtual ICollection<tblBookingDetailsAmendTx> tblBookingDetailsAmendTxes { get; set; }

        public virtual tblWebsiteUserMater tblWebsiteUserMater { get; set; }

        public virtual ICollection<tblChannelMgrBookingTx> tblChannelMgrBookingTxes { get; set; }

        public virtual tblSyncBookingToChannelMgrTracker tblSyncBookingToChannelMgrTracker { get; set; }
    }
}
