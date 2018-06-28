using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblRankManagement
    {
        public int iID { get; set; }
        public int iPropId { get; set; }
        public DateTime dtStartdate { get; set; }
        public DateTime dtEndDate { get; set; }
        public bool? IsSponsored { get; set; }
        public string sSponsored { get; set; }
        public int? iNewRank { get; set; }
        public int iOldRank { get; set; }
        public DateTime dtActionDate { get; set; }
        public int iActionBy { get; set; }
       
        public string cStatus { get; set; }
        public string Mode { get; set; }

        public string dtValidFrom { get; set; }
        public string dtValidTo { get; set; }
        public string sHotelName { get; set; }
        public string UserName { get; set; }

        public bool IsRateDisparity { get; set; }
        public string sRateDisparity { get; set; }

         public bool IsSponsoredYes { get; set; }
         public bool IsSponsoredNo { get; set; }

        
            

    }
    public class PropNames
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? PropId { get; set; }
        public bool? bIsTG { get; set; }
    }
}