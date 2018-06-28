using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class BookingGuestDetails
    {
        public BookingGuestDetails()
        {
            lsttblBookDetails = new List<etblBookingDetailsTx>();
            objBooking = new etblBookingTx();
            lstetblHotelFacilities = new List<etblHotelFacilities>();
            //lstOccData=new List<OccupancyData>();
            
        }


        public List<etblHotelFacilities> lstetblHotelFacilities { get; set; }
        public List<etblBookingDetailsTx> lsttblBookDetails { get; set; }
        public etblBookingTx objBooking { get; set; }
        public List<etblBookingGuestMap> lstGuestDetails { get; set; }
        public string RatingImageUrl { get; set; }
        public string RatingString { get; set; }
        public string sCountryPhoneCode { get; set; }

        public List<eCountryCodePhone> CountryCodePhoneList { get; set; }


        public string sMainImgUrl { get; set; }
        public Int16 iStarCategory { get; set; }
        public string sPropertyName { get; set; }
        public string sPropertyAddress { get; set; }
        public string scheckIn { get; set; }
        public string scheckInYear { get; set; }
        public string scheckOut { get; set; }
        public string scheckOutYear { get; set; }
        public string iTotalDays { get; set; }
        public string Symbol { get; set; }
        public string iPropId { get; set; }
        public string iNoOfRooms { get; set; }
        public string GuestData { get; set; }
        public string hdnJsonData { get; set; }

        public bool? bAnniversary { get; set; }
        public bool? bHoneymoon { get; set; }
        public bool? bBirthday { get; set; }

        //TravelGuru
        public string sRoomImageUrl { get; set; }
        public int MaxAdult { get; set; }
        public int MaxChild { get; set; }
        public string iVendorId { get; set; }

        public bool IsGuestBooking { get; set; }

        public bool IsRegularBooking { get; set; }

        public bool IsCorporateBooking { get; set; }

        public int CustomerId { get; set; }

        [Display(Name = "Registered Entity Name")]
        public string GstnEntityName { get; set; }
        [Display(Name = "Registered Entity Name")]
        public int? GstnState { get; set; }

        [Display(Name = "GSTIN Number")]
        [RegularExpression(@"\d{2}[A-Z]{5}\d{4}[A-Z]{1}\d[Z]{1}[A-Z\d]{1}", ErrorMessage = "Please enter a valid GST Number.")]
        public string GstnNumber { get; set; }

        [Display(Name = "Type of Entity")]
        public string GstnEntityType { get; set; }
    }
}