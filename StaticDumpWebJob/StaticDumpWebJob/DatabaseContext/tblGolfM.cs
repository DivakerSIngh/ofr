namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblGolfM")]
    public partial class tblGolfM
    {
        [Key]
        public short iGolfId { get; set; }

        [StringLength(50)]
        public string sGolf { get; set; }
    }
}
