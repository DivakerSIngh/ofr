using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class ePromotionalHotels
    {
        public string sPosition { get; set; }
        public string sHotelName { get; set; }
        public string sCity { get; set; }
        public Int16 iStarCategoryId { get; set; }
        public string sImageUrl { get; set; }
        public string sDescription { get; set; }
        public string sPrice { get; set; }
        public int iPropId { get; set; }
    }
}