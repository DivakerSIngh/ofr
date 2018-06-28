namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblBankDetailM_log
    {
        [Key]
        [Column(Order = 0)]
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
