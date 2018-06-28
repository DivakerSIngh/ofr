using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eCustomerPointsMap
    {
        public int iId { get; set; }
        public long iCustomerId { get; set; }
        public Nullable<int> iTotalPoints { get; set; }
        public Nullable<int> iUsedPoints { get; set; }
        public Nullable<int> iRemPoints { get; set; }
        public Nullable<System.DateTime> dtCreatedOn { get; set; }
        public Nullable<System.DateTime> dtExpiryOriginal { get; set; }
        public Nullable<System.DateTime> dtExpiry { get; set; }
        public string cStatus { get; set; }
    }
}