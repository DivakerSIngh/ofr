using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblPropertyTaxesMap
    {
        public int iPropTaxId { get; set; }
        public int iTaxId { get; set; }
        public Nullable<bool> bIsPercent { get; set; }
        public Nullable<decimal> dValue { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
    
    }
}