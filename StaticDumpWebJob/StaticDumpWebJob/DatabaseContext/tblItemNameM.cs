namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblItemNameM")]
    public partial class tblItemNameM
    {
        [Key]
        [StringLength(100)]
        public string sItem { get; set; }

        [StringLength(100)]
        public string sName { get; set; }
    }
}
