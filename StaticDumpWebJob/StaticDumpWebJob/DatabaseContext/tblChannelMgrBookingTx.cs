namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblChannelMgrBookingTx")]
    public partial class tblChannelMgrBookingTx
    {
        [Key]
        public long iId { get; set; }

        public long? iBookingId { get; set; }

        public int? iPropId { get; set; }

        public DateTime? dtOFRSend { get; set; }

        public DateTime? dtOFRReceive { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }

        [StringLength(100)]
        public string sCMDescription { get; set; }

        [StringLength(50)]
        public string sCMStatus { get; set; }

        [StringLength(10)]
        public string sErrorType { get; set; }

        [StringLength(10)]
        public string sErrorCode { get; set; }

        public virtual tblBookingTx tblBookingTx { get; set; }

        public virtual tblChannelMgrBookingTracker tblChannelMgrBookingTracker { get; set; }

        public virtual tblPropertyM tblPropertyM { get; set; }
    }
}
