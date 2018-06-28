using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eMinimumIncome
    {
        public int iPropId { get; set; }
        public string sHotelName { get; set; }
        public decimal dMinIncome { get; set; }
        public int iStarCategoryId { get; set; }
        public int iStateId { get; set; }
        public string sState { get; set; }
        public int iCityId { get; set; }
        public string sCity { get; set; }
        public string dtActionDate { get; set; }
        public string sActionBy { get; set; }
    }
}