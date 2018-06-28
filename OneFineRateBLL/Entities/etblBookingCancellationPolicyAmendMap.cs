using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblBookingCancellationPolicyAmendMap
    {
        public long iID { get; set; }
        public long iAmendId { get; set; }
        public Nullable<long> iBookingDetailsID { get; set; }
        public string sPolicyName { get; set; }
        public System.DateTime dtDate { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
    }
}