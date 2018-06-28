namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPhotoSubCategoryM")]
    public partial class tblPhotoSubCategoryM
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPhotoCatId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPhotoSubCatId { get; set; }

        [StringLength(100)]
        public string sPhotoSubCatName { get; set; }

        public virtual tblPhotoCategoryM tblPhotoCategoryM { get; set; }
    }
}
