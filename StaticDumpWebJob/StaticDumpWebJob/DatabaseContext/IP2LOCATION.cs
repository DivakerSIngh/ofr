namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class IP2LOCATION
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ip_from { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ip_to { get; set; }

        [StringLength(2)]
        public string country_code { get; set; }

        [StringLength(70)]
        public string country_name { get; set; }

        [StringLength(200)]
        public string city { get; set; }

        [StringLength(8)]
        public string countryoffset { get; set; }
    }
}
