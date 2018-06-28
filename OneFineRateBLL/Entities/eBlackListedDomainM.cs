using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eBlackListedDomainM
    {
        public int iBlackListedDomainId { get; set; }

        [StringLength(200, ErrorMessage = "Only 200 characters allowed.")]
        [DisplayName("Domain Name")]
        [Required(ErrorMessage = "Please enter Domain Name.")]
        //[Url(ErrorMessage="Domain name is not valid.")]
        public string sDomainName { get; set; }
              
        public string cStatus { get; set; }
        public int? iActionBy { get; set; }
        public DateTime? dtActionDate { get; set; }
        public string sActionType { get; set; }
        public string sActionBy { get; set; }

    }
}