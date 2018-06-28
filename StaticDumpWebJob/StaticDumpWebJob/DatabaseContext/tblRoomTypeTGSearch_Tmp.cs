namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblRoomTypeTGSearch_Tmp
    {
        [Key]
        public long iId { get; set; }

        [StringLength(50)]
        public string sSessionId { get; set; }

        [StringLength(12)]
        public string sHotelCode { get; set; }

        [StringLength(12)]
        public string sRoomTypeCode { get; set; }

        [StringLength(50)]
        public string sRoomType { get; set; }

        [StringLength(50)]
        public string sNonSmoking { get; set; }

        public int? iMinChildAge { get; set; }

        public int? iMaxChildAge { get; set; }

        public int? iMaxGuest { get; set; }

        public int? iMaxChild { get; set; }

        public int? iMaxAdult { get; set; }

        public DateTime? dtActionDate { get; set; }
    }
}
