using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class ePromotionManagementMap
    {
        public int iPropId { get; set; }
        public List<CheckBoxList> RoomTypeItems { get; set; }
        public List<CheckBoxList> Amenties { get; set; }
        public int iRatePlanId { get; set; }
    }
}