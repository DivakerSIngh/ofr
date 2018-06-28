namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblTaxM")]
    public partial class tblTaxM
    {
        public tblTaxM()
        {
            tblPropertyTaxesMaps = new HashSet<tblPropertyTaxesMap>();
        }

        [Key]
        public int iTaxId { get; set; }

        [StringLength(100)]
        public string sTaxName { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public virtual ICollection<tblPropertyTaxesMap> tblPropertyTaxesMaps { get; set; }

        public virtual tblUserM tblUserM { get; set; }
    }
}
