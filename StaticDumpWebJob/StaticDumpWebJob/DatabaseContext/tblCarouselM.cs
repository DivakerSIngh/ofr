namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblCarouselM")]
    public partial class tblCarouselM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iID { get; set; }

        [StringLength(200)]
        public string sImage { get; set; }

        public short? iSeq { get; set; }
    }
}
