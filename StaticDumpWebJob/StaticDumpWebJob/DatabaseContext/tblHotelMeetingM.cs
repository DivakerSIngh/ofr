namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblHotelMeetingM")]
    public partial class tblHotelMeetingM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short iHotelMeetingId { get; set; }

        [StringLength(50)]
        public string sMeeting { get; set; }
    }
}
