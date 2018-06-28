namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblCityM")]
    public partial class tblCityM
    {
        public tblCityM()
        {
            tblLocalityMs = new HashSet<tblLocalityM>();
        }

        [Key]
        public int iCityId { get; set; }

        public int iCountryId { get; set; }

        public int iStateId { get; set; }

        [StringLength(100)]
        public string sCity { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public virtual tblCountryM tblCountryM { get; set; }

        public virtual tblUserM tblUserM { get; set; }

        public virtual tblStateM tblStateM { get; set; }

        public virtual ICollection<tblLocalityM> tblLocalityMs { get; set; }
    }
}
