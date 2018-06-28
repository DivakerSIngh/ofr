using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eRoomRatePrice_Ph
    {
        [Required(ErrorMessage = "Please select FromDate.")]
        public string FromDate { get; set; }
        [Required(ErrorMessage = "Please select ToDate.")]
        public string ToDate { get; set; }
        [Required(ErrorMessage = "Please select RoomId.")]
        public Nullable<int> RoomId { get; set; }
        [Required(ErrorMessage = "Please select PlanId.")]
        public Nullable<int> PlanId { get; set; }
        public System.DateTime dtFrom { get; set; }
        public System.DateTime dtTo { get; set; }
        public Nullable<decimal> SingleRate { get; set; }
        public Nullable<decimal> DoubleRate { get; set; }
        public Nullable<decimal> TripleRate { get; set; }
        public Nullable<decimal> QuadrupleRate { get; set; }
        public Nullable<decimal> QuintrupleRate { get; set; }
    }
}