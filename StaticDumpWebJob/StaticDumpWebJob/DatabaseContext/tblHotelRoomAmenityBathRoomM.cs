namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblHotelRoomAmenityBathRoomM")]
    public partial class tblHotelRoomAmenityBathRoomM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short iHotelRoomAmenityBathRoomId { get; set; }

        [StringLength(50)]
        public string sRoomAmenityBathRoom { get; set; }
    }
}
