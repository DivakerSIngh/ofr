namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblLoyalityPointsM_log
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short iLoyalityId { get; set; }

        public int? iEarnMoney { get; set; }

        public int? iEarnPoints { get; set; }

        public int? iRedeemMoney { get; set; }

        public int? iRedeemPoints { get; set; }

        public int? iReferredPoints { get; set; }

        public int? iReferPointsSignUp { get; set; }

        public int? iSignUpPointsWithoutRef { get; set; }

        public int? iExpiryDays { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        [StringLength(150)]
        public string vc_changed_by { get; set; }

        [StringLength(100)]
        public string vc_changed_ip { get; set; }

        [StringLength(1000)]
        public string vc_programname { get; set; }

        public DateTime? dt_changed_date { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(1)]
        public string ch_action { get; set; }
    }
}
