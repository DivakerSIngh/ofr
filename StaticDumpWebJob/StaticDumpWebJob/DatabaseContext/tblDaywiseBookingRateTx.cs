namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblDaywiseBookingRateTx")]
    public partial class tblDaywiseBookingRateTx
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long iBookingDetailsID { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime dtStayDate { get; set; }

        public decimal? dRoomRate { get; set; }

        public decimal? dTaxes { get; set; }

        public DateTime? dtActionDate { get; set; }
    }
}
