using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblBidSettingsM
    {
        public decimal dMinBidAmountForCustomerAllowed { get; set; }
        public decimal dMinBidAmountDiffPer { get; set; }
        public decimal dMaxBidAmountDiffPer { get; set; }
        public decimal dOFRServiceCharge { get; set; }
        public decimal dOFRMinTotalEarning { get; set; }
    }
}