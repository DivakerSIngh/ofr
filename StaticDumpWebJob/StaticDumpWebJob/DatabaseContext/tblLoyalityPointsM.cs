namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblLoyalityPointsM")]
    public partial class tblLoyalityPointsM
    {
        [Key]
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

        public virtual tblUserM tblUserM { get; set; }
    }
}
