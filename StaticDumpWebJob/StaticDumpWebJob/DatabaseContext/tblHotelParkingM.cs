namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblHotelParkingM")]
    public partial class tblHotelParkingM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short iHotelParkingId { get; set; }

        [StringLength(50)]
        public string sParking { get; set; }

        [StringLength(1)]
        public string cRadioCheckBox { get; set; }
    }
}
