using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eRecentSavings
    {
        public string sCity { get; set; }
        public Int16 iStar { get; set; }
        public string sSymbol { get; set; }
        public string dActualAmt { get; set; }
        public string dDiscountedAmt { get; set; }
        public string Percentage { get; set; }
    }
}