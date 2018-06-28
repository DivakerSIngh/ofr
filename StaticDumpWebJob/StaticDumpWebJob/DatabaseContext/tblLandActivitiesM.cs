namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblLandActivitiesM")]
    public partial class tblLandActivitiesM
    {
        [Key]
        public short iLandActivityId { get; set; }

        [StringLength(50)]
        public string sLandActivity { get; set; }
    }
}
