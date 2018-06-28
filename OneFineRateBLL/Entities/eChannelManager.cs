using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eChannelManager
    {
        public int iPropId { get; set; }
        public string sHotelName { get; set; }
        public string sChannelMgrName { get; set; }
        public DateTime dtActionDate { get; set; }
        public string sActionBy { get; set; }
    }
}