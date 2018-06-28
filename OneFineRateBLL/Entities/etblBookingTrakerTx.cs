using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblBookingTrakerTx
    {
        public long iTrakerID { get; set; }
        public long iBookingId { get; set; }
        public string BookingStatus { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
    }
}