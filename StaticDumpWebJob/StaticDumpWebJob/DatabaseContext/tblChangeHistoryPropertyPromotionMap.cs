namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblChangeHistoryPropertyPromotionMap")]
    public partial class tblChangeHistoryPropertyPromotionMap
    {
        [Key]
        public long iID { get; set; }

        public int? iPropId { get; set; }

        public int? iPromoId { get; set; }

        public int? iPromotionMapId { get; set; }

        [StringLength(1000)]
        public string sItem { get; set; }

        [StringLength(8000)]
        public string sOld { get; set; }

        [StringLength(8000)]
        public string sNew { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }
    }
}
