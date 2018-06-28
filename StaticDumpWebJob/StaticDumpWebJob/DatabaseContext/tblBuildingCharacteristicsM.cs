namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblBuildingCharacteristicsM")]
    public partial class tblBuildingCharacteristicsM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short iBuildingCharacteristicsId { get; set; }

        [StringLength(100)]
        public string sBuildingCharacteristics { get; set; }
    }
}
