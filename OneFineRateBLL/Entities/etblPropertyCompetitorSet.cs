using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblPropertyCompetitorSet
    {
        public int iPropId { get; set; }
        public int iCPropId { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }

        public string SelectedCompetitorSet { get; set; }
        public List<etblPropertyCompetitorSet> PropertyCompetitorSetList { get; set; }

       
    }
}