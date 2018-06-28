namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblAwardM")]
    public partial class tblAwardM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short iAwardId { get; set; }

        [StringLength(50)]
        public string sAward { get; set; }
    }
}
