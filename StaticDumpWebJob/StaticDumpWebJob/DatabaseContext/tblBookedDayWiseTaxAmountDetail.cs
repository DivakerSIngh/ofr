namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblBookedDayWiseTaxAmountDetail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iBookingID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iBookingDetailsID { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "date")]
        public DateTime dtStayDay { get; set; }

        public decimal? dAmount { get; set; }

        public decimal? dTaxPerc { get; set; }

        public decimal? dTaxVal { get; set; }
    }
}
