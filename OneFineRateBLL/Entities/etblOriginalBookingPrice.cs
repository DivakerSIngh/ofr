using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblOriginalBookingPrice
    {
        public long iBookingId { get; set; }
        public Nullable<decimal> dOriginalTotalAmount { get; set; }
    }
}