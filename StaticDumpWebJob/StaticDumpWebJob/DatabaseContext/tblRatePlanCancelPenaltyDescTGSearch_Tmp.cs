namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblRatePlanCancelPenaltyDescTGSearch_Tmp
    {
        [Key]
        public long iId { get; set; }

        [StringLength(50)]
        public string sSessionId { get; set; }

        [StringLength(12)]
        public string sHotelCode { get; set; }

        [StringLength(12)]
        public string sRatePlanCode { get; set; }

        [StringLength(50)]
        public string sNonRefundable { get; set; }

        [StringLength(100)]
        public string sPenaltyName { get; set; }

        public string sPenaltyDesc { get; set; }

        public DateTime? dtActionDate { get; set; }
    }
}
