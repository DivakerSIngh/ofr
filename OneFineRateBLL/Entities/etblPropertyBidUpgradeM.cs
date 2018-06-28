using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblPropertyBidUpgradeM
    {
        public int iPropId { get; set; }
        public System.DateTime dtEffectiveDate { get; set; }
        public long iRoomId { get; set; }
        public Nullable<decimal> dUpgradeCharge { get; set; }
        public string sUpgradeType { get; set; }
        public Nullable<bool> bCloseOut { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
    }
}