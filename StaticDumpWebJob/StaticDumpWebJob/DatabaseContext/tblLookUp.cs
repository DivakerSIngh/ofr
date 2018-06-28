namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblLookUp")]
    public partial class tblLookUp
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iPropId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long iRoomId { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iRPPromoId { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool bIsPromo { get; set; }

        [Key]
        [Column(Order = 4, TypeName = "date")]
        public DateTime dtStayDay { get; set; }

        [Key]
        [Column(Order = 5)]
        public byte iOccupancy { get; set; }

        public bool? b24HrsCheckIn { get; set; }

        [StringLength(2)]
        public string sCheckInHH { get; set; }

        [StringLength(2)]
        public string sCheckInMM { get; set; }

        public byte? iExtraBedRequiredFrom { get; set; }

        public decimal? dExtraBedCharges { get; set; }

        public bool? bChildrenAllowed { get; set; }

        public byte? iComplimentaryStayAge { get; set; }

        public byte? iMaxOccupancy { get; set; }

        public bool? bRoomActive { get; set; }

        public short? iAvailableInventory { get; set; }

        public bool? bStopSell { get; set; }

        public short? iCutOff { get; set; }

        public short? iMinLengthStayRP { get; set; }

        public short? iMaxLengthStayRP { get; set; }

        public short? dHrsDays { get; set; }

        [StringLength(1)]
        public string cHrsDays { get; set; }

        public bool? bIsBefore { get; set; }

        public bool? bIsSecretDeal { get; set; }

        [StringLength(1)]
        public string cRPPromoStatus { get; set; }

        public bool? bParentActive { get; set; }

        public bool? bCloseOut { get; set; }

        public short? iMinLengthStay { get; set; }

        public short? iMaxLengthStay { get; set; }

        public bool? bCTA { get; set; }

        public bool? bCTD { get; set; }

        public decimal? dPrice { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtBookingStartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dtBookingEndDate { get; set; }

        public byte? iNoOfExtraBedsUsed { get; set; }

        public short? iHrsDaysPromo { get; set; }

        [StringLength(1)]
        public string cHrsDaysPromo { get; set; }

        public bool? bIsBeforePromo { get; set; }

        public bool? bIsAirConditioning { get; set; }

        public bool? bIsBathtub { get; set; }

        public bool? bIsFlatScreenTV { get; set; }

        public bool? bIsSoundproof { get; set; }

        public bool? bIsView { get; set; }

        public bool? bIsInternetFacilities { get; set; }

        public bool? bIsPrivatePool { get; set; }

        public bool? bIsRoomService { get; set; }

        public byte? iPromoType { get; set; }

        public decimal? dPriceRP { get; set; }
    }
}
