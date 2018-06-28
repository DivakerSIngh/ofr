using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eChains
    {
        public int iChainID { get; set; }
        public string sChainName { get; set; }
        public string cStatus { get; set; }
        public DateTime dtActionDate { get; set; }
        public int iActionBy { get; set; }
        public string ActionBy { get; set; }
    }


}