using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OneFineRateBLL.Entities
{
    public class etblBannerM
    {
        public int iBannerId { get; set; }
        [Required(ErrorMessage = "Please fill banner name")]
        public string sBannerName { get; set; }
        public string sBannerType { get; set; }
        public string sTextPosition { get; set; }
        public string sDescription { get; set; }
        public string cstatus { get; set; }

        //[Url(ErrorMessage = "Please Enter Valid URL. Ex: http://www.example.com")]
        [StringLength(200,ErrorMessage="Url should not exceed 200 character!")]
        public string sLinkId { get; set; }
        public DateTime dtActionDate { get; set; }
        public int iActionBy { get; set; }

        public string sImgUrl { get; set; }
        public Int16 iResolutionH { get; set; }
        public Int16 iResolutionW { get; set; }
        public string UniqueAzureFileName { get; set; }

        public HttpPostedFileBase bannerImage { get; set; }

        public string sLinkController { get; set; }
        public string sLinkAction { get; set; }
        

               
        public string Mode { get; set; }
        public string heading { get; set; }

    }
}