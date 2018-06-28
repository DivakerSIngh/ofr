using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblBookingGuestAmendMap
    {
        public long iBookingGuestId { get; set; }
        public long iAmendId { get; set; }
        public Nullable<long> iBookingDetailsID { get; set; }
        public string sGuestName { get; set; }
        public string sGuestEmail { get; set; }
        public string sGuestMobile { get; set; }
        public string sBedPreference { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<bool> bIsPrimary { get; set; }
    }
}