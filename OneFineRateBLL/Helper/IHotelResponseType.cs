using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public interface IHotelResponseType
    {
        string iVendorId { get; set; }
        decimal dPrice { get; set; }
    }
}