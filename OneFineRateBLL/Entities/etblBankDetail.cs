using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace OneFineRateBLL.Entities
{
    public class etblBankDetail
    {
        public int iPropId { get; set; }
        [Required(ErrorMessage="Required")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "Only numbers are allowed")]
        public string sBankAccountNo { get; set; }
        [Required(ErrorMessage="Required")]
        public string sBaneficiariesName { get; set; }
        [Required(ErrorMessage="Required")]
        public string sBankName { get; set; }
        [Required(ErrorMessage="Required")]
        public string sBranchName { get; set; }
        [Required(ErrorMessage="Required")]
        public string sBranchAddress { get; set; }
        [Required(ErrorMessage="Required")]
        public string sMicrCode { get; set; }
        [Required(ErrorMessage="Required")]
        public string sIfscCode { get; set; }
        //[Required(ErrorMessage="Required")]
        public string sLetterhead_BankAccount { get; set; }
        //[Required(ErrorMessage="Required")]
        public string sCancelledCheque { get; set; }
        //[Required(ErrorMessage="Required")]
        public string sPanCard { get; set; }
        [Required(ErrorMessage="Please enter commission%.")]
        [Range(0.00,100.00,ErrorMessage="Comission % must be between 0 to 100")]
        public Nullable<decimal> dCommission { get; set; }
        [Required(ErrorMessage="Required")]
        public string sFName { get; set; }

        [StringLength(15,ErrorMessage="Phone number can not be greater than 15 character")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number, It must be of 10 digit ")]
        [Required(ErrorMessage="Required")]
        public string sFPhoneNo { get; set; }

        //[Required(ErrorMessage="Required")]
        [RegularExpression(@"^\+?[0-9]{6,15}$", ErrorMessage = "Invalid Fax ")]
        public string sFFaxNo { get; set; }

        [EmailAddress(ErrorMessage="Email is not valid.")]
        [Required(ErrorMessage="Required")]
        public string sFEmailId { get; set; }
        
        public Nullable<DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
        public Nullable<DateTime> dtModifyDate { get; set; }
        public Nullable<int> iModifyBy { get; set; }


        public HttpPostedFileBase Letterhead_BankAccount { get; set; }
        public HttpPostedFileBase CancelledCheque { get; set; }
        public HttpPostedFileBase PanCard { get; set; }
        
       
        public string StateName { get; set; }


        [Display(Name = "GSTIN Number")]
        [Required(ErrorMessage = "Required")]
        //[StringLength(15, MinimumLength = 15, ErrorMessage = "GST Number must be of 15 digit valid character.")]
        [RegularExpression(@"\d{2}[A-Z]{5}\d{4}[A-Z]{1}\d[Z]{1}[A-Z\d]{1}", ErrorMessage = "Please enter a valid GST Number.")]
        public string sGstinNumber { get; set; }


        [Display(Name = "Registered Entity Name")]
        [Required(ErrorMessage = "Required")]
        public string sRegisteredEntityName { get; set; }

        [Display(Name = "PAN Number")]
        [Required(ErrorMessage = "Required")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "PAN Number must be of 10 digit valid character.")]
        [RegularExpression(@"[A-Z]{5}\d{4}[A-Z]{1}", ErrorMessage = "Please enter a valid PAN Number.")]
        public string sPanNumber { get; set; }


        [Required(ErrorMessage = "Required")]
        public int? iStateId { get; set; }

        [Display(Name = "Type of Entity")]
        [Required(ErrorMessage = "Required")]
        public string sEntityType { get; set; }
    }
}