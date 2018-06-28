namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblCustomerPointsMap")]
    public partial class tblCustomerPointsMap
    {
        [Key]
        public long iId { get; set; }

        public long iCustomerId { get; set; }

        public int? iTotalPoints { get; set; }

        public int? iUsedPoints { get; set; }

        public int? iRemPoints { get; set; }

        public DateTime? dtCreatedOn { get; set; }

        public DateTime? dtExpiryOriginal { get; set; }

        public DateTime? dtExpiry { get; set; }

        [StringLength(1)]
        public string cType { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }

        public virtual tblWebsiteUserMater tblWebsiteUserMater { get; set; }
    }
}
