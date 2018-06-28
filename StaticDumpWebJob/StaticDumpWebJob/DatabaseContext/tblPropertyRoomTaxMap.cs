namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyRoomTaxMap")]
    public partial class tblPropertyRoomTaxMap
    {
        [Key]
        [Column(Order = 0, TypeName = "date")]
        public DateTime dtStay { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPropTaxId { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPropId { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long iRoomId { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }
    }
}
