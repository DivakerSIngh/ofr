using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcCheckBoxList;
using System.ComponentModel;
using OneFineRateAppUtil;

namespace OneFineRateBLL.Entities
{
    public class etblHotelPartnerM
    {
        public int iID { get; set; }
        public string sHotelName { get; set; }
        [Required]
        public Nullable<byte> iStarCategory { get; set; }
        public string sWebsiteUrl { get; set; }
        public string sAddress { get; set; }
        public string sCity { get; set; }
        public string sState { get; set; }
        public string sCountry { get; set; }

        
        public string sPIN { get; set; }
        public string sBoardLineNumber { get; set; }
        public string sFirstName { get; set; }
        public string sLastName { get; set; }
        public string sDesignation { get; set; }

        [EmailAddress(ErrorMessage = "Email not valid !")]
        [StringLength(100, ErrorMessage = "Only 100 characters allowed.")]
        public string sEmail { get; set; }

        
        public string sMobile { get; set; }
        public string sSuccesMsg { get; set; }
        public string sErrorMsg { get; set; }
        public Nullable<DateTime> dtActionDate { get; set; }
       
        [Required]
        [Display(Name = "Terms and Conditions")]
        public bool bIsAgree { get; set; }

    }
}