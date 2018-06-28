namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblLocalityM")]
    public partial class tblLocalityM
    {
        [Key]
        public int iLocalityId { get; set; }

        public int? iAreaId { get; set; }

        public int? iCountryId { get; set; }

        public int? iStateId { get; set; }

        public int? iCityId { get; set; }

        [Required]
        [StringLength(100)]
        public string sLocality { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public virtual tblCityM tblCityM { get; set; }

        public virtual tblCountryM tblCountryM { get; set; }

        public virtual tblMacroAreaM tblMacroAreaM { get; set; }

        public virtual tblStateM tblStateM { get; set; }
    }
}
