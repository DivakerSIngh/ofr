using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblPropertyParkingMap
    {
        public int iPropId { get; set; }
        public string sCarName { get; set; }
        public string cAirportRail { get; set; }
        public Nullable<bool> bIsFree { get; set; }
        public Nullable<decimal> dOnewayCharge { get; set; }
        public Nullable<decimal> dTwowayCharge { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
    }
}