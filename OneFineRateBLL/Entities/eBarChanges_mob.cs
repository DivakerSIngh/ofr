using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eBarChanges_mob
    {
        [Required(ErrorMessage = "Please select FromDate.")]
        public string FromDate { get; set; }
        [Required(ErrorMessage = "Please select ToDate.")]
        public string ToDate { get; set; }
        [Required(ErrorMessage = "Please select Room.")]
        public Nullable<int> RoomId { get; set; }
        [Required(ErrorMessage = "Please select Plan.")]
        public Nullable<int> PlanId { get; set; }
        
        public Boolean Action { get; set; }
        public System.DateTime dtFrom { get; set; }
        public System.DateTime dtTo { get; set; }
        public Nullable<decimal> dSingleRate { get; set; }
        public Nullable<decimal> dDoubleRate { get; set; }
        public Nullable<decimal> dTripleRate { get; set; }
        public Nullable<decimal> dQuadrupleRate { get; set; }
        public Nullable<decimal> dQuintrupleRate { get; set; }
    }
}