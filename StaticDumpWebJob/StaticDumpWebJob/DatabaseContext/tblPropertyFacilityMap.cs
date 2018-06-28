namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyFacilityMap")]
    public partial class tblPropertyFacilityMap
    {
        [Key]
        public int iID { get; set; }

        public int iPropId { get; set; }

        public int iHoteFacilityId { get; set; }
    }
}
