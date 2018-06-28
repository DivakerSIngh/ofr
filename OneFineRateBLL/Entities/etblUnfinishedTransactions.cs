using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcCheckBoxList;
using System.ComponentModel;
using OneFineRateAppUtil;

namespace OneFineRateBLL.Entities
{
    public class etblUnfinishedTransactions
    {
        public DateTime dtTransactionDate { get; set; }
        public string sName { get; set; }

        [EmailAddress(ErrorMessage = "Email not valid !")]
        [StringLength(100, ErrorMessage = "Only 100 characters allowed.")]
        public string sEmailID { get; set; }


        public string iTelephone { get; set; }
        public long? iBookingID { get; set; }
        public string sType { get; set; }
        public string sStatus { get; set; }
        public string sSearchType { get; set; }

        public string sStatusInitial { get; set; }
        public string sTypeInitial { get; set; }
        public string ccTypeInitial { get; set; }
        public string sTransDate { get; set; }

    }

    public class etblUnfinishedTransactionsToBePaid
    {
        public etblUnfinishedTransactionsToBePaid()
        {
            lstetblHotelFacilities = new List<etblHotelFacilities>();
        }
        public int iPropId { get; set; }
        public long? iBookingID { get; set; }
        public string sHotelName { get; set; }
        public string sHotelAddress { get; set; }
        public string sHotelImgURL { get; set; }
        public Int16 iStarCategory { get; set; }
        public string dtCheckIn { get; set; }
        public string dtCheckOut { get; set; }
        public int iCalcNights { get; set; }
        public decimal dNegotiationAmount { get; set; }
        public decimal dAdvancePaid { get; set; }
        public decimal dBalancePayment { get; set; }
        public string sCustomerEmailID { get; set; }

        public List<etblHotelFacilities> lstetblHotelFacilities { get; set; }
    }

    public class etblUnfinishedHotelNotSelected
    {
        public etblUnfinishedHotelNotSelected()
        {
            lstetblSelectedHotels = new List<etblSelectedHotels>();
        }

        public List<etblSelectedHotels> lstetblSelectedHotels { get; set; }

    }

    public class etblSelectedHotels
    {
        public etblSelectedHotels()
        {
            lstetblHotelFacilities = new List<etblHotelFacilities>();
        }
        public int iPropId { get; set; }
        public long? iBookingID { get; set; }
        public string sHotelName { get; set; }
        public string sHotelAddress { get; set; }
        public string sHotelImgURL { get; set; }
        public Int16 iStarCategory { get; set; }
        public string sHotelDescription { get; set; }
        public string sCustomerEmailID { get; set; }
        public List<etblHotelFacilities> lstetblHotelFacilities { get; set; }
    }

    public class etblPropertyDetail
    {
        public int iPropId { get; set; }
        public long? iBookingID { get; set; }
        public string sHotelName { get; set; }
        public string sHotelAddress { get; set; }
        public string sHotelImgURL { get; set; }
        public Int16 iStarCategory { get; set; }
        public string sHotelDescription { get; set; }
        public string sCustomerEmailID { get; set; }
        public List<etblHotelFacilities> lstetblHotelFacilities { get; set; }
    }

    public class NegotiationForHotelPendingModel
    {
        public NegotiationForHotelPendingModel()
        {
            GuestBookingInformationList = new List<GuestBookingInformation>();
            objresultHotelFacilities = new List<etblHotelFacilities>();
        }
        public int iPropId { get; set; }
        public string sBookedBy { get; set; }
        public string sUserName { get; set; }
        public long? iCustomerId { get; set; }
        public long iBookingId { get; set; }
        public string dtReservationDate { get; set; }
        public string sImgUrl { get; set; }
        public string slargeMapURL { get; set; }
        public string sHotelName { get; set; }
        public short? iStarCategoryId { get; set; }
        public string sCheckInDay { get; set; }
        public string sCheckOutDay { get; set; }
        public string sCheckInYear { get; set; }
        public string sCheckOutYear { get; set; }
        public string sCheckIn_Display { get; set; }
        public string sCheckOut_Display { get; set; }
        public int Nights { get; set; }
        public int Attempts { get; set; }
        public int? Adults { get; set; }
        public int? Children { get; set; }
        public int? Rooms { get; set; }
        public decimal? dTotalNegotiateAmount { get; set; }
        public decimal? dAdvNegotiateAmount { get; set; }
        public decimal? dTaxes { get; set; }
        public decimal? sExtra2 { get; set; }
        public decimal? dBalanceAmt { get; set; }
        public decimal? dCounterOffer { get; set; }
        public decimal? dTotalAmount { get; set; }
        public decimal? dTotalExtraBedAmount { get; set; }
        public string sBookingStatus { get; set; }
        public string sAddress { get; set; }
        public string sPropertyAddress { get; set; }
        public string cBookingType { get; set; }
        public string cBookingStatus { get; set; }
        public string sBookType { get; set; }
        public string Symbol { get; set; }
        public bool? bSelfTravelling { get; set; }

        public decimal ExchangeRate { get; set; }
        public decimal? TaxHotel { get; set; }
        public decimal? TaxCustomer { get; set; }
        public Nullable<decimal> dccPrice { get; set; }
        public Nullable<decimal> dccDiscount { get; set; }
        public Nullable<decimal> dTaxesOriginal { get; set; }
        public Nullable<decimal> dSafeguardComm { get; set; }
        public string ccType { get; set; }

        public Nullable<decimal> dLatitude { get; set; }
        public Nullable<decimal> dLongitude { get; set; }

        public string sCity { get; set; }

        public string sSpecialRequests { get; set; }
        public string sExpectedCheckIn { get; set; }

        public bool IsPrefrenceSelected()
        {
            if(!string.IsNullOrEmpty(this.spref_single_lady)
                || !string.IsNullOrEmpty(this.spref_away_elevator)
                || !string.IsNullOrEmpty(this.spref_smoking_room)
                || !string.IsNullOrEmpty(this.spref_quiet_room)
                || !string.IsNullOrEmpty(this.spref_pickup)
                || !string.IsNullOrEmpty(this.spref_extra1)
                || !string.IsNullOrEmpty(this.spref_extra2)
                || !string.IsNullOrEmpty(this.spref_extra3))
            {
                return true;
            }
            return false;
        }

        public string spref_single_lady { get; set; }
        public string spref_away_elevator { get; set; }
        public string spref_smoking_room { get; set; }
        public string spref_quiet_room { get; set; }
        public string spref_pickup { get; set; }
        public string spref_extra1 { get; set; }
        public string spref_extra2 { get; set; }
        public string spref_extra3 { get; set; }

        public List<GuestBookingInformation> GuestBookingInformationList { get; set; }
        public List<etblHotelFacilities> objresultHotelFacilities { get; set; }
    }

    public class GuestBookingInformation
    {
        public string sRoomName { get; set; }
        public string sGuestName { get; set; }
        public short? iOccupancy { get; set; }
        public string sBedPreference { get; set; }
        public short? iExtraBeds { get; set; }
    }
    public class eBookingPolicies
    {
        public long iBookingDetailsID { get; set; }
        public string sAmenityRatePlan { get; set; }
        public string sPolicyName { get; set; }
    }

}