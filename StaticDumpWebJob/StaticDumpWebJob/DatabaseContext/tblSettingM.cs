namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblSettingM")]
    public partial class tblSettingM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iSettingId { get; set; }

        [StringLength(50)]
        public string sPhone { get; set; }

        [StringLength(200)]
        public string sEmail { get; set; }
    }
}
