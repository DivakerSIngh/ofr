using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblExchangeRatesM
    {
        public string sCurrencyCodeFrom { get; set; }
        public string sCurrencyCodeTo { get; set; }
        public Nullable<decimal> dRate { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
    }
}