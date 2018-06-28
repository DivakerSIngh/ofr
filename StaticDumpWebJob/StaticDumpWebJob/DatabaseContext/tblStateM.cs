namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblStateM")]
    public partial class tblStateM
    {
        public tblStateM()
        {
            tblCityMs = new HashSet<tblCityM>();
            tblLocalityMs = new HashSet<tblLocalityM>();
        }

        [Key]
        public int iStateId { get; set; }

        public int iCountryId { get; set; }

        [Required]
        [StringLength(50)]
        public string sState { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public virtual ICollection<tblCityM> tblCityMs { get; set; }

        public virtual tblCountryM tblCountryM { get; set; }

        public virtual ICollection<tblLocalityM> tblLocalityMs { get; set; }

        public virtual tblUserM tblUserM { get; set; }
    }
}
