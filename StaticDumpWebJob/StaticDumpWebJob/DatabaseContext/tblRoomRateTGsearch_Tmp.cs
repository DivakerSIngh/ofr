namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblRoomRateTGsearch_Tmp
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
        public DateTime? dEffectiveDate { get; set; }

        [StringLength(6)]
        public string tEffectiveTime { get; set; }

        public decimal? dAmountBeforeTax { get; set; }

        public decimal? dTaxAmount { get; set; }

        [StringLength(10)]
        public string sAdditionalGuestAmountRPH { get; set; }

        [StringLength(10)]
        public string sAdditionalGuestAmountAgeQualificationCode { get; set; }

        public decimal? dAdditionalGuestAmountTax { get; set; }

        [StringLength(10)]
        public string sBookable { get; set; }

        [StringLength(5)]
        public string iBaseChildOccupancy { get; set; }

        [StringLength(5)]
        public string iBaseAdultOccupancy { get; set; }

        public DateTime? dtActionDate { get; set; }
    }
}
