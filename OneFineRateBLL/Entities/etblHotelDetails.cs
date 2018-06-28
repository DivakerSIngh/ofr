using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneFineRateWebBLL.Entities
{
    public class etblHotelDetails
    {
        public int iPropId { get; set; }
        public string sPropName { get; set; }
        public string sPropImageURL { get; set; }
     
        public int iLocalityId { get; set; }
        public string iLocalityName { get; set; }
                    
    }
}
