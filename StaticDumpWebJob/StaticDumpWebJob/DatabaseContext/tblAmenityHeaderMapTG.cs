namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblAmenityHeaderMapTG")]
    public partial class tblAmenityHeaderMapTG
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iAmenityId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(200)]
        public string sHeader { get; set; }

        [StringLength(200)]
        public string sAmenity { get; set; }
    }
}
