using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblAdditionalCommission
    {
        public string sHotelName { get; set; }
        public int iAdditionalCommisionId { get; set; }

        [Required(ErrorMessage = "Please enter start date.")]
        public string dtCommisionStartDate { get; set; }

        [Required(ErrorMessage = "Please enter end date.")]
        public string dtCommisionEndDate { get; set; }

        [Required(ErrorMessage = "Please enter commission.")]

        public Nullable<decimal> dCommission { get; set; }
        public Nullable<int> iPropId { get; set; }
        public bool bActive { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
        public Nullable<decimal> baseDCommission { get; set; }
        public string iActionByName { get; set; }
    }
}