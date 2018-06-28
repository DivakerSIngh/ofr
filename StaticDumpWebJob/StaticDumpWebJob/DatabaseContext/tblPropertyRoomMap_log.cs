namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tblPropertyRoomMap_log
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long iRoomId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPropId { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short iRoomTypeId { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string sRoomName { get; set; }

        [StringLength(50)]
        public string sRoomCode { get; set; }

        public byte? iMaxOccupancy { get; set; }

        public byte? iMaxChildren { get; set; }

        public byte? iNoOfExtraBeds { get; set; }

        public byte? iNoOfBedrooms { get; set; }

        public byte? iNoOfBathrooms { get; set; }

        public decimal? dSizeSqft { get; set; }

        public decimal? dSizeMtr { get; set; }

        [StringLength(10)]
        public string sSizeType { get; set; }

        public short? iTotalRooms { get; set; }

        public short? iTwinBeds { get; set; }

        public short? iDoubleBeds { get; set; }

        [StringLength(50)]
        public string sRoomAccessibilityIds { get; set; }

        [StringLength(50)]
        public string sBuildingCharacteristicsIds { get; set; }

        [StringLength(50)]
        public string sRoomOutdoorViewIds { get; set; }

        public DateTime? dtActionDate { get; set; }

        public int? iActionBy { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool bActive { get; set; }

        [StringLength(150)]
        public string vc_changed_by { get; set; }

        [StringLength(100)]
        public string vc_changed_ip { get; set; }

        [StringLength(1000)]
        public string vc_programname { get; set; }

        public DateTime? dt_changed_date { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(1)]
        public string ch_action { get; set; }

        public byte? iSeq { get; set; }
    }
}
