namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblUserPropertyMap")]
    public partial class tblUserPropertyMap
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iUserId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPropId { get; set; }

        public DateTime? dtCreationDate { get; set; }

        public int? iCreatedBy { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }

        public virtual tblPropertyM tblPropertyM { get; set; }

        public virtual tblUserM tblUserM { get; set; }

        public virtual tblUserM tblUserM1 { get; set; }

        public virtual tblUserM tblUserM2 { get; set; }
    }
}
