namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblBookedBidAmenity
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long iBookingId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string sAmenity { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string sApplicability { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(5)]
        public string sType { get; set; }

        [Key]
        [Column(Order = 4)]
        public byte iNo { get; set; }

        public virtual tblBookingTx tblBookingTx { get; set; }
    }
}
