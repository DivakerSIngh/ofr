namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblMyWishListTx")]
    public partial class tblMyWishListTx
    {
        [Key]
        public long iId { get; set; }

        public int? iPropId { get; set; }

        public long? iCustomerId { get; set; }

        public DateTime? dtActionDate { get; set; }

        public virtual tblWebsiteUserMater tblWebsiteUserMater { get; set; }
    }
}
