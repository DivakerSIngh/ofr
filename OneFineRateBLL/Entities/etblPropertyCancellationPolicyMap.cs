using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblPropertyCancellationPolicyMap
    {
        public int iCancellationPolicyId { get; set; }
        public int iPropId { get; set; }
        public string sPolicyName { get; set; }
        public Nullable<System.DateTime> dtValidFrom { get; set; }
        public Nullable<System.DateTime> dtValidTo { get; set; }
        public Nullable<short> dHrsDays { get; set; }
        public string cHrsDays { get; set; }
        public Nullable<bool> bIsNonRefundable { get; set; }
        public Nullable<bool> bIsPercent { get; set; }
        public Nullable<decimal> dValue { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
        public string cStatus { get; set; }

        public string Mode { get; set; }
        public string ActionBy { get; set; }
        public string validfrom { get; set; }
        public string validTo { get; set; }
    }
}