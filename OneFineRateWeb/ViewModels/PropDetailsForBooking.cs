using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateWeb.ViewModels
{
    public class PropDetailsForBooking : PropDetailsForHotelBookingList
    {
        public string sLocality { get; set; }
        public int iLocalityId { get; set; }
    }
}