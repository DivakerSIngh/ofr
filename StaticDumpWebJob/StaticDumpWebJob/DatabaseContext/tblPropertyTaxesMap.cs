namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyTaxesMap")]
    public partial class tblPropertyTaxesMap
    {
        [Key]
        public int iID { get; set; }

        public int iPropTaxId { get; set; }

        public int iTaxId { get; set; }

        public bool? bIsPercent { get; set; }

        public decimal? dValue { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public virtual tblPropertyTaxMap tblPropertyTaxMap { get; set; }

        public virtual tblTaxM tblTaxM { get; set; }

        public virtual tblUserM tblUserM { get; set; }
    }
}
