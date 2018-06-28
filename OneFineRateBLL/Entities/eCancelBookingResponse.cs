using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eCancelBookingResponse
    {
        public string BookingId { get; set; }
        public decimal Refund { get; set; }
        public int RoomsCancelled { get; set; }
        public string sRefundEmail { get; set; }
        public string CustPhoneNumber { get; set; }
        public string CustEmail { get; set; }
        public string CustName { get; set; }
        public string CancellationPolicy { get; set; }
        public decimal CancellationCharges { get; set; }
        public decimal CancelChargesHotel { get; set; }
        public string sCountryPhoneCode { get; set; }

        public string HotelName { get; set; }
        public string City { get; set; }
        public string CheckInDate { get; set; }
        public string CheckoutDate { get; set; }
        public string Status { get; set; }

        public bool IsFormatForHotel { get; set; }

        /// <summary>
        /// Used for OFR Admin Only, Not the Hotel Revenue Manager
        /// </summary>
        public bool IsFormatForRevenueMgr { get; set; }
    }
}