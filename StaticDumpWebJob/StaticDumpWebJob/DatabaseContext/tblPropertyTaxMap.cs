namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyTaxMap")]
    public partial class tblPropertyTaxMap
    {
        public tblPropertyTaxMap()
        {
            tblPropertyTaxesMaps = new HashSet<tblPropertyTaxesMap>();
        }

        [Key]
        public int iPropTaxId { get; set; }

        public int iPropId { get; set; }

        public int? iRPId { get; set; }

        public long? iRoomId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtStayFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtStayTo { get; set; }

        [Required]
        [StringLength(1)]
        public string cStatus { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public virtual tblPropertyRoomMap tblPropertyRoomMap { get; set; }

        public virtual ICollection<tblPropertyTaxesMap> tblPropertyTaxesMaps { get; set; }

        public virtual tblUserM tblUserM { get; set; }
    }
}
