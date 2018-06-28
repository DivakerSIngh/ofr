namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblCurrencyM")]
    public partial class tblCurrencyM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short iCurrencyId { get; set; }

        [StringLength(3)]
        public string sCurrencyCode { get; set; }

        [StringLength(50)]
        public string sCurrency { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        [StringLength(100)]
        public string sImg { get; set; }

        [StringLength(5)]
        public string sSymbol { get; set; }

        [StringLength(5)]
        public string sCountryCurrencyCode { get; set; }

        public virtual tblUserM tblUserM { get; set; }
    }
}
