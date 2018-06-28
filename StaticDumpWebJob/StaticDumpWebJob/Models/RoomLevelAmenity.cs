using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticDumpWebJob.Models
{
    public class RoomLevelAmenity
    {
        public string iVendorId { get; set; }
        public long iRoomTypeId { get; set; }
        public string sRoomType { get; set; }
        public int iAmenityId { get; set; }
        public string sDescrition { get; set; }
        public DateTime? dtActionDate { get; set; }
    }
}
