namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblChannelMgrM")]
    public partial class tblChannelMgrM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iChannelMgr { get; set; }

        [Required]
        [StringLength(50)]
        public string sChannelMgrName { get; set; }
    }
}
