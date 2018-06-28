using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblPhotoGallery
    {
        public int? iPropId { get; set; }
        public int? iPhotoSubCatId { get; set; }
        public bool? bIsPrimaryH { get; set; }
        public bool? bIsPrimaryR { get; set; }
        public string sMainImgUrl { get; set; }
        public string sthumbImgUrl { get; set; }
        public string sName { get; set; }
        public string iVendorId { get; set; }
    }

    public class etblHotelFacilities
    {
        public int iHoteFacilityId { get; set; }
        public string sHotelFacilites { get; set; }
        public int iRank { get; set; }
        public string sImgUrl { get; set; }
    }
    public class etblTaxCharges
    {
        public DateTime dtDate { get; set; }
        public decimal dOFRServiceCharge { get; set; }
        public decimal TaxOnServiceCharge { get; set; }
        public decimal TotalServiceCharge { get; set; }
        public string cGstValueType { get; set; }
        public string dGstValue { get; set; }
    }
    public class etblRoomMaxTaxes
    {
        public int RoomID { get; set; }
        public string TaxPer { get; set; }
    }
    public class etblRooms
    {
        public etblRooms()
        {
            lstRatePlan = new List<etblRatePlans>();
            lstRoomTaxes = new List<RoomTaxes>();
        }
        //public int? iPropId { get; set; }
        //public long  iRoomId { get; set; }
        //public string sRoomName { get; set; }
        //public byte? MaxOccupancy { get; set; }
        //public Int16? TwinBed { get; set; }
        //public byte? ExtraBed { get; set; }
        //public string sRoomImgUrl { get; set; }

        //public List<etblRoomAmenties> lstetblRoomAmenties { get; set; }


        public long iRoomId { get; set; }
        public string sRoomName { get; set; }
        public byte? MaxOccupancy { get; set; }
        public Int16? TwinBed { get; set; }
        public byte? MaxExtraBeds { get; set; }
        public int iAvailableInventory { get; set; }
        public decimal ExtraBedCharges { get; set; }
        public string sRoomImgUrl { get; set; }
        public string Amen1 { get; set; }
        public string Amen2 { get; set; }
        public string Amen3 { get; set; }
        public string Amen4 { get; set; }
        public string Amen5 { get; set; }
        public string Amen6 { get; set; }
        public string Amen7 { get; set; }
        public string Amen8 { get; set; }

        public List<etblRatePlans> lstRatePlan { get; set; }
        public List<RoomTaxes> lstRoomTaxes { get; set; }
        

    }




    public class etblCreditCards
    {
        public int iPropId { get; set; }
        public int      iCreditCardId   { get; set; }
        public string   sCreditCard     { get; set; }
        public string   imgURL          { get; set; }
    }

    public class etblRatePlans
    {
        public etblRatePlans()
        {
            lstetblOccupancy = new List<etblOccupancy>();
            lstCancellationPolcy = new List<CancellationPolcy>();
        }
        public long iRoomId { get; set; }
        public int RPID { get; set; }
        public int iOccupancy { get; set; }
        public string RateInclusion { get; set; }
        public string strCancellationPolicy { get; set; }
        public string RatePlan { get; set; }

        public List<etblOccupancy> lstetblOccupancy { get; set; }
        public List<CancellationPolcy> lstCancellationPolcy { get; set; }
        
    }

    public class etblOccupancy
    {
        public etblOccupancy()
        {
            lstTaxesDateWise = new List<TaxesDateRoomRateWise>();
        }
        public long iRoomId { get; set; }
        public int RPID { get; set; }
        public string RatePlan { get; set; }
        public int iOccupancy { get; set; }
        public int ExtraBeds { get; set; }
        public decimal ExtraBedCharges { get; set; }
        public int Cnt { get; set; }
        public Boolean blsPromo { get; set; }

        public int iNoOfRooms { get; set; }
        public decimal dPrice { get; set; }
        public decimal dBasePrice { get; set; }
        public decimal dExtraPrice { get; set; }
        public Nullable<decimal> dPriceRP { get; set; }
        public string Persons { get; set; }
        public Nullable<int> iPromoType { get; set; }
        
        public int iAdults { get; set; }
        public int iChildrens { get; set; }
        public string ChildrenAge { get; set; }

        public decimal dTaxes { get; set; }
        public decimal dTaxesForHotel { get; set; }


        public List<TaxesDateRoomRateWise> lstTaxesDateWise { get; set; }
        public int iPoints { get; set; }
    }

    public class CancellationPolcy
    {
        public int iRPId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string CancellationPolicy { get; set; }
        public string PolicyName { get; set; }
        public string CancellationMsg { get; set; }

    }

    public class RoomTaxes
    {
        public long iRoomId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public string Tax { get; set; }
        public string TaxString { get; set; }

    }


    public class etblRoomAmenties
    {
        public int iRoomFacilityId { get; set; }
        public string sRPName { get; set; }
        public int iRank { get; set; }
        public string sImgUrl { get; set; }
    }

    public interface ListetblPhotoGallery
    {
        List<etblPhotoGallery> lstetblPhotoGallery { get; set; }
        List<etblRooms> lstetblRooms { get; set; }
        List<etblCreditCards> lstetblCreditCards { get; set; }
    }

}