namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblBookingDetailsTx")]
    public partial class tblBookingDetailsTx
    {
        public tblBookingDetailsTx()
        {
            tblBookingCancellationPolicyMaps = new HashSet<tblBookingCancellationPolicyMap>();
            tblBookingGuestMaps = new HashSet<tblBookingGuestMap>();
        }

        [Key]
        public long iBookingDetailsID { get; set; }

        public long iBookingId { get; set; }

        [Required]
        [StringLength(12)]
        public string iRoomId { get; set; }

        [StringLength(12)]
        public string iRPId { get; set; }

        public short? iRooms { get; set; }

        [StringLength(100)]
        public string sRoomName { get; set; }

        [StringLength(100)]
        public string sRPName { get; set; }

        public short? iOccupancy { get; set; }

        public decimal? dRoomRate { get; set; }

        public decimal? dExtraBedRate { get; set; }

        public decimal? dRoomDiscountedRate { get; set; }

        public decimal? dTaxes { get; set; }

        public decimal? dTaxesForHotel { get; set; }

        public decimal? dBasicDiscountBid { get; set; }

        public decimal? dLOSDiscountBid { get; set; }

        public decimal? dLeadDiscountBid { get; set; }

        public decimal? dWWDiscountBid { get; set; }

        public decimal? dMRDiscountBid { get; set; }

        [StringLength(2000)]
        public string sAmenityRatePlan { get; set; }

        public decimal? dUpgradeCharge { get; set; }

        public short? iPromoType { get; set; }

        public short iAdults { get; set; }

        public short? iChildren { get; set; }

        [StringLength(50)]
        public string sChildrenAge { get; set; }

        public short? iExtraBeds { get; set; }

        public DateTime? dtActionDate { get; set; }

        public virtual ICollection<tblBookingCancellationPolicyMap> tblBookingCancellationPolicyMaps { get; set; }

        public virtual ICollection<tblBookingGuestMap> tblBookingGuestMaps { get; set; }
    }
}
