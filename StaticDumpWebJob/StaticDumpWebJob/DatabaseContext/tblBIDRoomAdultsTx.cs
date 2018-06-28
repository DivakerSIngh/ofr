namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblBIDRoomAdultsTx")]
    public partial class tblBIDRoomAdultsTx
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long iBookingId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short iRoomNo { get; set; }

        public short? iAdults { get; set; }

        public short? iChildren { get; set; }

        [StringLength(50)]
        public string sChildAge { get; set; }
    }
}
