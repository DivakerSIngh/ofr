using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblRoomRateRangeM
    {
        public int? iRoomRateRangeId { get; set; }

        [Display(Name ="Room Rate From")]
        [Required(ErrorMessage = "Please select Range From.")]
        public int dAmountFrom { get; set; }


        [Display(Name = "Room Rate To")]
        [Required(ErrorMessage = "Please select Range To.")]
        public int dAmountTo { get; set; }
        public string cStatus { get; set; }
        public DateTime? dtActionDate { get; set; }
        public int? iActionBy { get; set; }
        public string sActionByName { get; set; }      
    }
}