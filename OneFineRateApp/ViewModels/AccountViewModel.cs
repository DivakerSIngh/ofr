using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneFineRateApp.ViewModels
{

    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Your Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Your Name")]
        public string Name { get; set; }
    }


    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }
}