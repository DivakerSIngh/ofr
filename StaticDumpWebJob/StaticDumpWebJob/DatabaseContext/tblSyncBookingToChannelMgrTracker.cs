namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblSyncBookingToChannelMgrTracker")]
    public partial class tblSyncBookingToChannelMgrTracker
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long iBookingId { get; set; }

        public DateTime? dtSyncToChannelMgr { get; set; }

        [StringLength(1)]
        public string cSyncStatus { get; set; }

        public string sXMLString { get; set; }

        [StringLength(8000)]
        public string sError { get; set; }

        public virtual tblBookingTx tblBookingTx { get; set; }
    }
}
