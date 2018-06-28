namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblChangeHistoryPIRoom")]
    public partial class tblChangeHistoryPIRoom
    {
        [Key]
        public long iID { get; set; }

        public int? iPropId { get; set; }

        public long? iRoomId { get; set; }

        [StringLength(100)]
        public string sItem { get; set; }

        [StringLength(500)]
        public string sOld { get; set; }

        [StringLength(500)]
        public string sNew { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public virtual tblPropertyM tblPropertyM { get; set; }

        public virtual tblPropertyRoomMap tblPropertyRoomMap { get; set; }
    }
}
