namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblBookingTx_log
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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

        [StringLength(150)]
        public string vc_changed_by { get; set; }

        [StringLength(100)]
        public string vc_changed_ip { get; set; }

        [StringLength(1000)]
        public string vc_programname { get; set; }

        public DateTime? dt_changed_date { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(1)]
        public string ch_action { get; set; }
    }
}
