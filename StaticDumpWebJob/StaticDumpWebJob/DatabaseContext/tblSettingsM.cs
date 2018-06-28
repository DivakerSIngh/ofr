namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblSettingsM")]
    public partial class tblSettingsM
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short iBidAttempts { get; set; }

        public short? iNegotiateAttempts { get; set; }

        public short? iBidAttemptsTime { get; set; }

        public short? iNegotiateAttemptsTime { get; set; }
    }
}
