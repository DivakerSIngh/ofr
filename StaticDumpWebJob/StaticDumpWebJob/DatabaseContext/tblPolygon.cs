namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblPolygon
    {
        public int Id { get; set; }

        public string PolygonCordinates { get; set; }

        public int? LocalityCordinates_Id { get; set; }

        public virtual tblIndianLocalityCordinate tblIndianLocalityCordinate { get; set; }
    }
}
