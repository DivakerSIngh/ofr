namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblBasicPropertyPOITGSpecific_Tmp
    {
        [Key]
        public long iId { get; set; }

        [StringLength(50)]
        public string sSessionId { get; set; }

        [StringLength(12)]
        public string sHotelCode { get; set; }

        [StringLength(50)]
        public string sPOIName { get; set; }

        [StringLength(50)]
        public string sPOIDistance { get; set; }

        public DateTime? dtActionDate { get; set; }
    }
}
