namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblChangeHistoryPhoto")]
    public partial class tblChangeHistoryPhoto
    {
        [Key]
        public long iID { get; set; }

        public int? iPropId { get; set; }

        public int? iPhotoCatId { get; set; }

        [StringLength(200)]
        public string sImgUrl { get; set; }

        [StringLength(100)]
        public string sItem { get; set; }

        [StringLength(500)]
        public string sOld { get; set; }

        [StringLength(500)]
        public string sNew { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }
    }
}
