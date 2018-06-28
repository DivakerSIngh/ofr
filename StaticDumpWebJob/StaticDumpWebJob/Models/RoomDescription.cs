using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticDumpWebJob.Models
{
    public class RoomDescription
    {
        public string iVendorId { get; set; }
        public long iRoomTypeId { get; set; }
        public string sRoomType { get; set; }
        public string sDescription { get; set; }
        public short iMax_Adult_Occupancy { get; set; }
        public short iMax_Child_Occupancy { get; set; }
        public short iMax_Infant_Occupancy { get; set; }
        public short iMax_Guest_Occupancy { get; set; }
        public string sRoomType_ImagePath { get; set; }
        public DateTime? dtActionDate { get; set; }
    }
}
