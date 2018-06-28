namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyPhotoMap")]
    public partial class tblPropertyPhotoMap
    {
        public tblPropertyPhotoMap()
        {
            tblPropertyPrimaryPhotoMaps = new HashSet<tblPropertyPrimaryPhotoMap>();
        }

        [Key]
        public long iPhotoId { get; set; }

        public int iPropId { get; set; }

        public int? iPhotoCatId { get; set; }

        public long? iPhotoSubCatId { get; set; }

        public bool? bIsHighRes { get; set; }

        public bool? bIsPrimaryH { get; set; }

        public bool? bIsPrimaryR { get; set; }

        [StringLength(200)]
        public string sImgUrl { get; set; }

        public short? iResolutionW { get; set; }

        public short? iResolutionH { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public bool? bIsMapped { get; set; }

        [StringLength(100)]
        public string UniqueAzureFileName { get; set; }

        public virtual tblPhotoCategoryM tblPhotoCategoryM { get; set; }

        public virtual tblUserM tblUserM { get; set; }

        public virtual ICollection<tblPropertyPrimaryPhotoMap> tblPropertyPrimaryPhotoMaps { get; set; }
    }
}
