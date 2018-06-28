using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblOfferReviewTrackerTx
    {
        public long iId { get; set; }
        public Nullable<int> iPropId { get; set; }
        public string sIPAddress { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
    }
}