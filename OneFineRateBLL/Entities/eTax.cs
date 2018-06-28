using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eTax
    {
        public int? iTaxId { get; set; }
        public string sTaxName { get; set; }
        public string cStatus { get; set; }
        public DateTime dtActionDate { get; set; }
        public int? iActionBy { get; set; }
        public string value { get; set; }
        public string Type { get; set; }
    }

    public class eTaxDisp
    {
        public int iTaxId { get; set; }
        public string sTaxName { get; set; }
        public string cStatus { get; set; }
        public DateTime dtActionDate { get; set; }
        public string ActionBy { get; set; }
    }
  
}