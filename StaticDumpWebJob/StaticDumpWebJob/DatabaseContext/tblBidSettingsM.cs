namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblBidSettingsM")]
    public partial class tblBidSettingsM
    {
        [Key]
        [Column(Order = 0)]
        public decimal dMinBidAmountForCustomerAllowed { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal dMinBidAmountDiffPer { get; set; }

        [Key]
        [Column(Order = 2)]
        public decimal dMaxBidAmountDiffPer { get; set; }

        [Key]
        [Column(Order = 3)]
        public decimal dOFRServiceCharge { get; set; }

        [Key]
        [Column(Order = 4)]
        public decimal dOFRMinTotalEarning { get; set; }
    }
}
