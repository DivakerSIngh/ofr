namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblHotelLeisureM")]
    public partial class tblHotelLeisureM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short iHotelLeisureId { get; set; }

        [StringLength(50)]
        public string sLeisure { get; set; }
    }
}
