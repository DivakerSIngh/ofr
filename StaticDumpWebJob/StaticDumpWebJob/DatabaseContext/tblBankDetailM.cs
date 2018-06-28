namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblBankDetailM")]
    public partial class tblBankDetailM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPropId { get; set; }

        [StringLength(20)]
        public string sBankAccountNo { get; set; }

        [StringLength(50)]
        public string sBaneficiariesName { get; set; }

        [StringLength(100)]
        public string sBankName { get; set; }

        [StringLength(100)]
        public string sBranchName { get; set; }

        [StringLength(200)]
        public string sBranchAddress { get; set; }

        [StringLength(10)]
        public string sMicrCode { get; set; }

        [StringLength(11)]
        public string sIfscCode { get; set; }

        [StringLength(200)]
        public string sLetterhead_BankAccount { get; set; }

        [StringLength(200)]
        public string sCancelledCheque { get; set; }

        [StringLength(200)]
        public string sPanCard { get; set; }

        public decimal? dCommission { get; set; }

        [StringLength(50)]
        public string sFName { get; set; }

        [StringLength(10)]
        public string sFPhoneNo { get; set; }

        [StringLength(50)]
        public string sFFaxNo { get; set; }

        [StringLength(100)]
        public string sFEmailId { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public DateTime? dtModifyDate { get; set; }

        public int? iModifyBy { get; set; }
    }
}
