using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticDumpWebJob.Models
{
    public class Review
    {
        public string iVendorId { get; set; }
        public string sAvgGuestRating { get; set; }
        public short? iOverallRating { get; set; }
        public string sComments { get; set; }
        public short? iRoomQuality { get; set; }
        public short? iServiceQuality { get; set; }
        public short? iDiningQuality { get; set; }
        public short? iCleanliness { get; set; }
        public DateTime? dtPost_Date { get; set; }
        public string sConsumer_city { get; set; }
        public string sConsumer_Country { get; set; }
        public string sCustomer_name { get; set; }
        public DateTime? dtActionDate { get; set; }
    }
}
