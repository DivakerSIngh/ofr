using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticDumpWebJob.Models
{
    public class POIData
    {
        public int Poi_Id { get; set; }
        public string Poi_Seo_Id { get; set; }
        public string Poi_Name { get; set; }
        public float Poi_latitude { get; set; }
        public float Poi_longitude { get; set; }
        public long City_Id { get; set; }
        public string City_Name { get; set; }
        public string Seo_City_Name { get; set; }
    }
}
