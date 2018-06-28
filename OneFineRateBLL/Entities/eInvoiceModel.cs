using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public enum HotelOrGuest
    {
        Guest,
        Hotel
    }

    public class eInvoiceModel
    {
        public eInvoiceModel()
        {
            sCurrentDate = DateTime.Today.ToString("dd MMM, yyyy");
        }

        public HotelOrGuest HotelOrGuest { get; set; }
        public string iBookingId { get; set; }
        public string dtReservationDate { get; set; }
        public string sBookingStatus { get; set; }
        public string Reservation { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
        public string sBooker { get; set; }
        public string sEmailOFR { get; set; }
        public string sMobileOFR { get; set; }
        public string CountryPhoneCode { get; set; }
        public string sHotelName { get; set; }
        public string RatingImageUrl { get; set; }
        public string RatingString { get; set; }
        public string sStreetAddress { get; set; }
        public string sAddress { get; set; }
        public string StarCategoryId { get; set; }
        public string AvgAmount { get; set; }
        public string dTotalAmount { get; set; }

        public string dTotalAmountPayable { get; set; }

        public string sTotalAmountPayableInWords { get; set; }
        public decimal ExtraBedAmount { get; set; }
        public decimal? dccPrice { get; set; }
        public decimal? dccDiscount { get; set; }
        public decimal? dBidAmount { get; set; }
        public string dTaxes { get; set; }

        public string dAccomodationChargeIncludingTax { get; set; }
        public string dCommission { get; set; }
        public decimal dTaxesForHotel { get; set; }
        public string SubTotal { get; set; }
        public string CancellationCharge { get; set; }
        public string RefundAmount { get; set; }
        public bool IsForHotel { get; set; }
        public string sRatePlanName { get; set; }
        public int NoOfRooms { get; set; }

        public string sSymbol { get; set; }
        public string sCurrencyCode { get; set; }

        public decimal? dExtraBedCharges { get; set; }
        public string sCity { get; set; }

        public decimal? dDiscountedBidPrice { get; set; }
        public decimal? dTotalNegotiateAmount { get; set; }
        public decimal? dAdvNegotiateAmount { get; set; }
        public decimal? dBalanceAmt { get; set; }
        public decimal? dSubTotalNego { get; set; }
        public decimal? dOrginalSubTotalNego { get; set; }
        public decimal? dCounterOffer { get; set; }
        public decimal? dSubTotalCounter { get; set; }
        public decimal? dCustomerPayable { get; set; }
        public string cBookingType { get; set; }
        public string cBookingStatus { get; set; }
        public string ActualBookingType { get; set; }
        public bool? IsNegotiationAccepted { get; set; }
        public int iLinkedBookingId { get; set; }

        public string sOfrDiscount { get; set; }

        public string dServiceCharge { get; set; }

        public string dGstOnServiceChargePercent { get; set; }

        public string dGstOnServiceCharge { get; set; }

        public string dGstOnCommissionPercent { get; set; }

        public string dGstOnCommission { get; set; }

        public string dTotalCommission { get; set; }

        public string sTotalCommissionInWords { get; set; }

        public bool? bPromoDiscount { get; set; }
        public string bPromoAmenity { get; set; }

        public List<eBookingRoomM> BookingRoomDetails { get; set; }

        public int iPropId { get; set; }

        public PropDetailsM PropDetailM_OnlyWithRooms { get; set; }

        public string sCurrentDate { get; set; }

        public string sInvoiceNumber { get; set; }

        public string sCustomerGSTINEntityName { get; set; }

        public string sCustomerAddress { get; set; }

        public string sCustomerGSTIN { get; set; }
        
        public string sCustomerGSTINEntityType { get; set; }

        public string sCustomerGSTINPlaceOfSupply { get; set; }

        public string sHotelGSTINEntityName { get; set; }

        public string sHotelAddress { get; set; }

        public string sHotelGSTIN { get; set; }

        public string sHotelGSTINEntityType { get; set; }

        public string sHotelGSTINPlaceOfSupply { get; set; }

        public eBookingRoomM DefaultRoomDetail { get; set; }

        public eInvoiceSettingDetail InvoiceSettingDetail { get; set; }

        [Required(ErrorMessage = "Please enter at least one email address.")]
        [RegularExpression(@"^((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([,])*)*$", ErrorMessage = "Please enter a valid email address(s).")]
        public string CommaSeperatedEmail { get; set; }

        public bool IsGeneratingPdf { get; set; }

        public bool Customer_GST_Same_State { get; set; }
        public bool Hotel_GST_Same_State { get; set; }
    }

    public class eInvoiceSettingDetail
    {
        public string PAN_OFR { get; set; }
        public string GSTIN_OFR { get; set; }
        public string Place_Of_Supply_OFR { get; set; }
        public string SAC_Code_OFR { get; set; }
        public string CIN_OFR { get; set; }
        public string Bank_Name_OFR { get; set; }
        public string Bank_AC_OFR { get; set; }
        public string Bank_IFSC_OFR { get; set; }
        public string Invoice_Email_OFR { get; set; }
        public string Footer_Address_OFR { get; set; }
        public string Entity_Type_OFR { get; set; }
    }
    public class Particulars
    {
        public string Particular { get; set; }
        public string Amount { get; set; }
    }
}