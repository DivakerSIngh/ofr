namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblBookingTrakerTx")]
    public partial class tblBookingTrakerTx
    {
        [Key]
        public long iTrakerID { get; set; }

        public long iBookingId { get; set; }

        [StringLength(2)]
        public string BookingStatus { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }
    }
}
