namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblRoomRateTaxTGSpecific_Tmp
    {
        [Key]
        public long iId { get; set; }

        [StringLength(50)]
        public string sSessionId { get; set; }

        [StringLength(12)]
        public string sHotelCode { get; set; }

        [StringLength(12)]
        public string sRatePlanCode { get; set; }

        [StringLength(12)]
        public string sRoomTypeCode { get; set; }

        [Column(TypeName = "date")]
        public DateTime? sEffectiveDate { get; set; }

        public decimal? dAmount { get; set; }

        [StringLength(10)]
        public string sCode { get; set; }

        public DateTime? dtActionDate { get; set; }
    }
}
