namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblIndianLocalityCordinate
    {
        public tblIndianLocalityCordinate()
        {
            tblPolygons = new HashSet<tblPolygon>();
        }

        public int Id { get; set; }

        public int? GoogleFriendlyPlaceId { get; set; }

        [StringLength(100)]
        public string LocalityName { get; set; }

        [StringLength(100)]
        public string StateName { get; set; }

        public int? LocalityId { get; set; }

        public virtual ICollection<tblPolygon> tblPolygons { get; set; }
    }
}
