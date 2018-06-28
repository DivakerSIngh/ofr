using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblBookingGuestMap
    {
        public long iBookingGuestId { get; set; }
        public Nullable<long> iBookingDetailsID { get; set; }
        public string sGuestName { get; set; }
        public string sGuestEmail { get; set; }
        public string sGuestMobile { get; set; }
        public string sBedPreference { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<bool> bIsPrimary { get; set; }

        public int iRoomId { get; set; }
        public int iRPId { get; set; }
        public string sCountryPhoneCode { get; set; }
    }
}