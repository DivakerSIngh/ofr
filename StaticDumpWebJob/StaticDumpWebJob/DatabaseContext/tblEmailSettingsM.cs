namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblEmailSettingsM")]
    public partial class tblEmailSettingsM
    {
        [Key]
        [StringLength(50)]
        public string sModule { get; set; }

        [StringLength(200)]
        public string sEmail { get; set; }
    }
}
