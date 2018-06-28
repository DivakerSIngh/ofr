using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblPropertyPromoMap
    {
        public long iPropPromoId { get; set; }
        public int iPromoCodeId { get; set; }
        public int iPropId { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
        public string cStatus { get; set; }
    }
}