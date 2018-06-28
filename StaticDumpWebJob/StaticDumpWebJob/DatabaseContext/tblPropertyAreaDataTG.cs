namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyAreaDataTG")]
    public partial class tblPropertyAreaDataTG
    {
        [Key]
        public int iAreaDataId { get; set; }

        [StringLength(500)]
        public string sAreaName { get; set; }

        public int? iAreaTGUniqueId { get; set; }
    }
}
