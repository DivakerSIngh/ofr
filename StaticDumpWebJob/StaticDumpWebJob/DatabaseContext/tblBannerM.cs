namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblBannerM")]
    public partial class tblBannerM
    {
        [Key]
        public int iBannerId { get; set; }

        [StringLength(100)]
        public string sBannerName { get; set; }

        [StringLength(20)]
        public string sBannerType { get; set; }

        [StringLength(20)]
        public string sTextPosition { get; set; }

        public string sDescription { get; set; }

        [StringLength(1)]
        public string cstatus { get; set; }

        [StringLength(200)]
        public string sLinkId { get; set; }

        [StringLength(200)]
        public string sImgUrl { get; set; }

        public short? iResolutionW { get; set; }

        public short? iResolutionH { get; set; }

        [StringLength(200)]
        public string UniqueAzureFileName { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }
    }
}
