namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblPropertyAmenitiesMap_log
    {
        [Key]
        [Column(Order = 0)]
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

        [Key]
        [Column(Order = 1)]
        public bool bAirportTransferAvalible { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        [StringLength(50)]
        public string sHotelParkingRadio { get; set; }

        [StringLength(150)]
        public string vc_changed_by { get; set; }

        [StringLength(100)]
        public string vc_changed_ip { get; set; }

        [StringLength(1000)]
        public string vc_programname { get; set; }

        public DateTime? dt_changed_date { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(1)]
        public string ch_action { get; set; }
    }
}
