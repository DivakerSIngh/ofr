using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateBLL.Entities
{
    public class etblPropetyPhotoMap
    {
        public long iPhotoId { get; set; }

        public int iPropId { get; set; }

        [Required(ErrorMessage="Required")]
        public Nullable<int> iPhotoCatId { get; set; }
        [Required(ErrorMessage="Required")]
        public Nullable<long> iPhotoSubCatId { get; set; }

        public bool bIsPrimaryH { get; set; }
        public bool bIsPrimaryR { get; set; }
        public Nullable<bool> bIsHighRes { get; set; }
        public string sImgUrl { get; set; }
        public string UniqueAzureFileName { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
        public Nullable<bool> bIsMapped { get; set; }
        public Nullable<short> iResolutionW { get; set; }
        public Nullable<short> iResolutionH { get; set; }

        public List<etblPhotoSubCategoryM> ddliPhotoSubCatId { get; set; }

    }

    public class BLLPropertyImageMapViewModel
    {
        public List<etblPropetyPhotoMap> UnMappedEtblPropetyPhotoList { get; set; }
        public List<etblPropetyPhotoMap> MappedEtblPropetyPhotoList { get; set; }
        public List<MappedRoomTypePhoto> MappedRoomTypePhotoList { get; set; }
    }

    public class MappedRoomTypePhoto
    {
        public int iPhotoCatId { get; set; }
        public string sRoomName { get; set; }
        public long iRoomId { get; set; }
        public int ImageCount { get; set; }
    }
}