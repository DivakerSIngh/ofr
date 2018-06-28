namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblRatePlanTGSearch_Tmp
    {
        [Key]
        public long iId { get; set; }

        [StringLength(50)]
        public string sSessionId { get; set; }

        [StringLength(12)]
        public string sHotelCode { get; set; }

        [StringLength(12)]
        public string sRatePlanType { get; set; }

        [StringLength(200)]
        public string sRatePlanName { get; set; }

        [StringLength(12)]
        public string sRatePlanCode { get; set; }

        public int? iAvailableQuantity { get; set; }

        [StringLength(500)]
        public string sRatePlanInclusion { get; set; }

        [StringLength(50)]
        public string sDiscountCouponEnable { get; set; }

        public string sRatePlanDesc { get; set; }

        public DateTime? dtActionDate { get; set; }
    }
}
