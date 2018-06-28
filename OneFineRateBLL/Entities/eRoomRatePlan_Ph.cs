using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eRoomRatePlan_Ph
    {
        [Required(ErrorMessage = "This field is required.")]
        public string FromDate { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string ToDate { get; set; }
        public Nullable<int> RoomId { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public Nullable<int> PlanId { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public Boolean Action { get; set; }
        public System.DateTime dtFrom { get; set; }
        public System.DateTime dtTo { get; set; }
    }
}