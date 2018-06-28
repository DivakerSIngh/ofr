namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblChannelMgrTracker")]
    public partial class tblChannelMgrTracker
    {
        [Key]
        public long iID { get; set; }

        public int? iChannelMgrId { get; set; }

        public DateTime? dtReqDateTime { get; set; }

        public DateTime? dtResDateTime { get; set; }

        [Column(TypeName = "xml")]
        public string sReqMsg { get; set; }
    }
}
