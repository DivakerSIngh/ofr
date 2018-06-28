using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblIndianLocalityCordinate
    {
        public etblIndianLocalityCordinate()
        {
            this.tblPolygons = new List<etblPolygon>();
        }

        public int Id { get; set; }
        public Nullable<int> GoogleFriendlyPlaceId { get; set; }
        public string LocalityName { get; set; }
        public string StateName { get; set; }
        public Nullable<int> LocalityId { get; set; }

        public virtual List<etblPolygon> tblPolygons { get; set; }
    }

    public class etblPolygon
    {
        public int Id { get; set; }
        public string PolygonCordinates { get; set; }
        public Nullable<int> LocalityCordinates_Id { get; set; }
    }
}