using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblPropertyRoomInventory
    {
        public System.DateTime dtInventoryDate { get; set; }
        public int iPropId { get; set; }
        public long iRoomId { get; set; }
        public Nullable<short> iInventory { get; set; }
        public Nullable<short> iAvailableInventory { get; set; }
        public bool bStopSell { get; set; }
        public Nullable<short> iCutOff { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }

        public string sRoomName { get; set; }
        public string InventoryDate { get; set; }
        public string sStopSell { get; set; }
    }
}