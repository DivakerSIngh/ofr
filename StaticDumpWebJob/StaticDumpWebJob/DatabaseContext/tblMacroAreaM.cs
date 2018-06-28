namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblMacroAreaM")]
    public partial class tblMacroAreaM
    {
        public tblMacroAreaM()
        {
            tblLocalityMs = new HashSet<tblLocalityM>();
        }

        [Key]
        public int iAreaId { get; set; }

        public int iCountryId { get; set; }

        public int iStateId { get; set; }

        public int iCityId { get; set; }

        [Required]
        [StringLength(100)]
        public string sArea { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public virtual ICollection<tblLocalityM> tblLocalityMs { get; set; }

        public virtual tblUserM tblUserM { get; set; }
    }
}
