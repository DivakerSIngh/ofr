namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblRoomAccessibilityM")]
    public partial class tblRoomAccessibilityM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short iRoomAccessibilityId { get; set; }

        [StringLength(50)]
        public string sRoomAccessibility { get; set; }
    }
}
