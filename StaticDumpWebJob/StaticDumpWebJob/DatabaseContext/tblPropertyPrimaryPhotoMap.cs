namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPropertyPrimaryPhotoMap")]
    public partial class tblPropertyPrimaryPhotoMap
    {
        [Key]
        public long iId { get; set; }

        public long iPhotoId { get; set; }

        public int iPropId { get; set; }

        public long? iRoomId { get; set; }

        [StringLength(1)]
        public string cType { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        public virtual tblPropertyPhotoMap tblPropertyPhotoMap { get; set; }

        public virtual tblPropertyRoomMap tblPropertyRoomMap { get; set; }

        public virtual tblUserM tblUserM { get; set; }
    }
}
