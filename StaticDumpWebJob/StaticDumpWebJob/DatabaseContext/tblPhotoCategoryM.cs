namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPhotoCategoryM")]
    public partial class tblPhotoCategoryM
    {
        public tblPhotoCategoryM()
        {
            tblPhotoSubCategoryMs = new HashSet<tblPhotoSubCategoryM>();
            tblPropertyPhotoMaps = new HashSet<tblPropertyPhotoMap>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPhotoCatId { get; set; }

        [StringLength(100)]
        public string sPhotoCatName { get; set; }

        public virtual ICollection<tblPhotoSubCategoryM> tblPhotoSubCategoryMs { get; set; }

        public virtual ICollection<tblPropertyPhotoMap> tblPropertyPhotoMaps { get; set; }
    }
}
