namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyPromoMap")]
    public partial class tblPropertyPromoMap
    {
        [Key]
        public long iPropPromoId { get; set; }

        public int iPromoCodeId { get; set; }

        public int iPropId { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }

        public virtual tblPromoCodeM tblPromoCodeM { get; set; }

        public virtual tblUserM tblUserM { get; set; }
    }
}
