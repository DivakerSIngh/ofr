using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eRefundModel
    {
        public string ConfirmationNumber { get; set; }

        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string HotelName { get; set; }
        public string City { get; set; }
        public string CheckInDate { get; set; }
        public string CheckOutDate { get; set; }
        public string Status { get; set; }

        public string RefundAmount { get; set; }

        public string RefundInitiationDate { get; set; }

        public string sRefundWarningMsg { get; set; }

        public string sRefundStatusMsg { get; set; }
        public bool IsNegotiationRefund { get; set; }
        public bool IsBiddingRefund { get; set; }
        public bool IsRevenueManagerRefund { get; set; }
    }
}