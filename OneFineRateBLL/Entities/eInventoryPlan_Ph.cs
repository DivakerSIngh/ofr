using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eInventoryPlan_Ph
    {
        [Required(ErrorMessage = "This field is required.")]
        public string FromDate { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string ToDate { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public Boolean RoomType { get; set; }
        public Nullable<int> RoomId { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public Boolean Action { get; set; }
        public System.DateTime dtFrom { get; set; }
        public System.DateTime dtTo { get; set; }
    }
}