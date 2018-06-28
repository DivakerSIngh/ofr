using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eTblPromotionalHotel
    {
        public long iId { get; set; }
        [Required(ErrorMessage="Required")]
        public int iPropId { get; set; }
        public string sPosition { get; set; }
        public string sImageUrl { get; set; }
        [Required(ErrorMessage = "Required")]
        public string sDescription { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
    }
}