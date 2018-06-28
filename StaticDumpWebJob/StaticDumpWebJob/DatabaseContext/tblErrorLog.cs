namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblErrorLog")]
    public partial class tblErrorLog
    {
        [Key]
        public int iId { get; set; }

        [StringLength(50)]
        public string sController { get; set; }

        [StringLength(50)]
        public string sAction { get; set; }

        public string sError { get; set; }

        public DateTime? dtDate { get; set; }
    }
}
