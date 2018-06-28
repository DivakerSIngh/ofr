namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblOnsiteRecreationFacilitiesM")]
    public partial class tblOnsiteRecreationFacilitiesM
    {
        [Key]
        public short iRecreationFacilityId { get; set; }

        [StringLength(100)]
        public string sRecreationFacility { get; set; }
    }
}
