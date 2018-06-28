namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyAmenitiesMap")]
    public partial class tblPropertyAmenitiesMap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPropId { get; set; }

        [StringLength(50)]
        public string sHotelAmenities { get; set; }

        [StringLength(50)]
        public string sHotelParkings { get; set; }

        [StringLength(50)]
        public string sHotelCommonServices { get; set; }

        [StringLength(50)]
        public string sHotelConvenience { get; set; }

        [StringLength(50)]
        public string sHotelLeisure { get; set; }

        [StringLength(50)]
        public string sHotelMeetings { get; set; }

        public bool bAirportTransferAvalible { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        [StringLength(50)]
        public string sHotelParkingRadio { get; set; }
    }
}
