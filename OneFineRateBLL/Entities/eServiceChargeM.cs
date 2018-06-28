using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eServiceChargeM
    {
        public int? ServiceChargeId { get; set; }

        [Required(ErrorMessage = "Please select start date")]
        public string From { get; set; }

        public DateTime? dtFrom { get; set; }
        public DateTime? dtTo { get; set; }

        [Required(ErrorMessage = "Please select end date")]
        public string To { get; set; }
        public string Status { get; set; }
        public DateTime? ActionDate { get; set; }
        public int? ActionById { get; set; }
        public string ActionByName { get; set; }

        [Display(Name = "Service Charge")]
        [Required(ErrorMessage = "Please provide service charge")]
        public decimal ServiceCharge { get; set; }

        [Display(Name = "GST Applicable")]
        public string GstValueType { get; set; }

        [Display(Name = "GST value")]
        [Required(ErrorMessage = "Please provide gst value")]
        public decimal GstValue { get; set; }


        public decimal ServiceChargeTG { get; set; }
        
        public decimal GstOnServiceChargeTG { get; set; }

        public decimal GstOnServiceChargePercentTG { get; set; }

        public decimal TotalServiceChargeTG { get; set; }
    }
}