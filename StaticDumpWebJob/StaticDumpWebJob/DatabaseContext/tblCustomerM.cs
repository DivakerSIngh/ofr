namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblCustomerM")]
    public partial class tblCustomerM
    {
        [Key]
        public int iCustomerId { get; set; }

        [StringLength(50)]
        public string sUserName { get; set; }

        [StringLength(50)]
        public string sPassword { get; set; }

        [StringLength(50)]
        public string sGoogleToken { get; set; }

        [StringLength(50)]
        public string sFacebookToken { get; set; }

        public bool? bForceChangePass { get; set; }

        [StringLength(50)]
        public string sName { get; set; }

        [StringLength(100)]
        public string sEmail { get; set; }

        [StringLength(20)]
        public string sPhone { get; set; }

        public DateTime? dtLastModifyDate { get; set; }

        public bool? bIsUnsubscribed { get; set; }

        public int? iUpdatedBy { get; set; }

        public bool? bIsBlocked { get; set; }

        public int? BlockActionBy { get; set; }

        public DateTime? dtBlockdate { get; set; }

        [StringLength(500)]
        public string sBlockReason { get; set; }
    }
}
