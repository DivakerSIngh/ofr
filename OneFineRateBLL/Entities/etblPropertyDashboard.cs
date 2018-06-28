using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblPropertyDashboard
    {
        public int iCPropId { get; set; }
        public DateTime InventoryDate { get; set; }
        public string InventoryDay { get; set; }
        public decimal MinRate { get; set; }
        public string sHotelName { get; set; }
    }

    public class etblPropertyDashboardView
    {
        public int PropId { get; set; }
        public string  HotelName { get; set; }
        public decimal Column1 { get; set; }
        public decimal Column2 { get; set; }
        public decimal Column3 { get; set; }
        public decimal Column4 { get; set; }
        public decimal Column5 { get; set; }
        public decimal Column6 { get; set; }
        public decimal Column7 { get; set; }
    }
}