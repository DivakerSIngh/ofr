namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblCountryM")]
    public partial class tblCountryM
    {
        public tblCountryM()
        {
            tblCityMs = new HashSet<tblCityM>();
            tblLocalityMs = new HashSet<tblLocalityM>();
            tblStateMs = new HashSet<tblStateM>();
        }

        [Key]
        public int iCountryId { get; set; }

        [StringLength(50)]
        public string sCountry { get; set; }

        [StringLength(5)]
        public string sCountryCode { get; set; }

        [StringLength(1)]
        public string cStatus { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        [StringLength(5)]
        public string sCountryCurrencyCode { get; set; }

        public virtual ICollection<tblCityM> tblCityMs { get; set; }

        public virtual tblUserM tblUserM { get; set; }

        public virtual ICollection<tblLocalityM> tblLocalityMs { get; set; }

        public virtual ICollection<tblStateM> tblStateMs { get; set; }
    }
}
