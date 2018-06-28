namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblRoomOutdoorViewM")]
    public partial class tblRoomOutdoorViewM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short iRoomOutdoorViewId { get; set; }

        [StringLength(100)]
        public string sRoomOutdoorView { get; set; }
    }
}
