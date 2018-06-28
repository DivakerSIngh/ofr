using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateBLL.Entities
{
    public class etblPropertySpaMap
    {
        public int iPropId { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Spa Name")]
        [Remote("ValidateSpaName", "Spa", ErrorMessage = "Spa name already exist.", AdditionalFields = "InitialSpaName")]
        public string sSpaName { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Spa Description")]
        public string sSpaDesc { get; set; }
        public bool bHotsprings { get; set; }
        public bool bSauna { get; set; }
        public bool bMudbath { get; set; }
        public bool bSpaTub { get; set; }
        public bool bAdvancedBooking { get; set; }
        public bool bSteamRoom { get; set; }
        public bool b24hours { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
    }
}