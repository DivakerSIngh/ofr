using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblPropertyLocalityMap
    {        
        public int iPropId { get; set; }
        public int iAreaLocalityId { get; set; }
        public string cAreaLocality { get; set; }
        public string cAreaLocalityName { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }

    }
}