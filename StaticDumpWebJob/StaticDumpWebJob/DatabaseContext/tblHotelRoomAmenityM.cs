namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblHotelRoomAmenityM")]
    public partial class tblHotelRoomAmenityM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short iHotelRoomAmenityId { get; set; }

        [StringLength(50)]
        public string sRoomAmenity { get; set; }

        [StringLength(1)]
        public string cRadioCheckBox { get; set; }
    }
}
