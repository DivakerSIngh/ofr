using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticDumpWebJob.Models
{
    public class Facility
    {
        public string iVendorId { get; set; }
        public int iAmenityId { get; set; }
        public string sAmenityType { get; set; }
        public string sDescription { get; set; }
        public DateTime? dtActionDate { get; set; }
    }
}
