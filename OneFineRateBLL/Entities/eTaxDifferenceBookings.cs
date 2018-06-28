using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eTaxDifferenceBookings
    {
        public long iBookingId { get; set; }
        public string sHotelName { get; set; }
        public string sCity { get; set; }
        public DateTime dtCheckIn { get; set; }
        public DateTime dtCheckOut { get; set; }
        public decimal Diff { get; set; }
        public string sEmailOFR { get; set; }

        public int iCityId { get; set; }
        public int iPropId { get; set; }

        public string CustomerName { get; set; }

        public string sCheckIn { get; set; }
        public string sCheckOut { get; set; }
    }
}