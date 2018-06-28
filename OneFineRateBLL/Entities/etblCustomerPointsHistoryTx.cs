using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
   
    public partial class etblCustomerPointsHistoryTx
    {
        public int iId { get; set; }
        public long iCustomerId { get; set; }
        public Nullable<int> iTotalPoints { get; set; }
        public Nullable<System.DateTime> dtAction { get; set; }
        public string cType { get; set; }
        public string Type { get; set; }
        public string dtDate { get; set; }

        public int? TotalRemainingPoints { get; set; }
        public int? RemainingPointsWithinMonth { get; set; }
    }
}