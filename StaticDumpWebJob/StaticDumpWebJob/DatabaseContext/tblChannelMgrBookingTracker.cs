namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblChannelMgrBookingTracker")]
    public partial class tblChannelMgrBookingTracker
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long iId { get; set; }

        public DateTime? dtOFRSend { get; set; }

        public DateTime? dtOFRReceive { get; set; }

        [StringLength(100)]
        public string sCMDescription { get; set; }

        [StringLength(50)]
        public string sCMStatus { get; set; }

        public virtual tblChannelMgrBookingTx tblChannelMgrBookingTx { get; set; }
    }
}
