using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eExchange
    {
        public string sCurrencyCodeFrom { get; set; }
        public string sCurrencyCodeTo { get; set; }
        public decimal dRate { get; set; }
        public DateTime dtActionDate { get; set; }
        public string sActionBy { get; set; }
    }

    public class ExchangeRate
    {
        public string sCurrencyCodeFrom { get; set; }
        public string sCurrencyCodeTo { get; set; }
        public double dRate { get; set; }
    }
}