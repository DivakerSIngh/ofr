using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblBookingNegotiationTx
    {
        public long iBookingId { get; set; }
        public System.DateTime dtNegotiationDate { get; set; }
        public Nullable<decimal> dTotalNegotiateAmount { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public string cStatus { get; set; }
    }
}