using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eBidding
    {

        public eBidding()
        {
            lstBidRoomsData = new List<BidRoomsData>();
        }
        public string sCheckInDateDisplay { get; set; }
        public string sCheckOutDateDisplay { get; set; }
        public string BidSearchHotels { get; set; }
        public List<BidRoomsData> lstBidRoomsData { get; set; }
        public int SelectedPropId { get; set; }
        public int SelectedRPId { get; set; }
        public int iBookingId { get; set; }
        public string sCheckIn { get; set; }
        public string sCheckOut { get; set; }
        public Boolean? bMapBooking { get; set; }

    }
    public class BidAmenAppl
    {
        public int iHotelId { get; set; }
        public string Amen { get; set; }
        public string Appl { get; set; }

    }
    public class BidRoomsData
    {
        public string ExtraTaxPer { get; set; }
        public string LastBook { get; set; }
        public int Looking { get; set; }
        public int iRank { get; set; }
        public string sHotelName { get; set; }
        public string sLocality { get; set; }
        public string sDescription { get; set; }
        public Int16 iStarCategoryId { get; set; }
        public int iPropId { get; set; }
        public decimal? dPrice { get; set; }
        public decimal? dDiscPrice { get; set; }
        public int iRoomId { get; set; }
        public int iRPId { get; set; }
        public string sImgUrl { get; set; }
        public decimal? Discount { get; set; }
        public bool? bUpgrade { get; set; }
        public string Amenity1 { get; set; }
        public string Amenity2 { get; set; }
        public string Amenity3 { get; set; }
        public string Amenity4 { get; set; }
        public string Amenity5 { get; set; }
        public string Amenity6 { get; set; }
        public string Amenity7 { get; set; }
        public string Amenity8 { get; set; }
        public string Amenity9 { get; set; }
        public string Amenity10 { get; set; }
        public string Amenity11 { get; set; }
        public string Amenity12 { get; set; }
        public string Amenity13 { get; set; }
        public string Amenity14 { get; set; }
        public string Amenity15 { get; set; }
        public string Amenity16 { get; set; }
        public string Amenity17 { get; set; }
        public string Amenity18 { get; set; }
        public decimal? dLatitude { get; set; }
        public decimal? dLongitude { get; set; }
        public string rating_image_url { get; set; }
        public decimal? rating { get; set; }
        public string ranking_string { get; set; }
        public bool? bIsTopHotel { get; set; }
        public bool? bIsPopularHotel { get; set; }
        public bool? bIsNew { get; set; }
        public bool? bIsHighDemand { get; set; }
        public string sCurrencySymbol { get; set; }
        public List<BidAmenAppl> lstBidAmenAppl { get; set; }
        


        public List<string> GetFirstFourAmenityUrl()
        {
            List<string> firstFourAmenityUrl = new List<string>();

            var properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var allAmenityProperty = properties.Where(x => x.Name.Contains("Amenity") && x.PropertyType.Name == "String").ToList();

            foreach (var item in allAmenityProperty)
            {
                var tValue = this.GetType().GetProperty(item.Name).GetValue(this, null).ToString();

                if (!string.IsNullOrEmpty(tValue))
                {
                    firstFourAmenityUrl.Add(tValue);
                }
            }
            return firstFourAmenityUrl.Take(4).ToList();
        }
    }


    public class eBidBookingResult
    {
        public int ID { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
    }
    #region BiddingHotelUpgrade
    public class BiddingHotelsUpgrade
    {
        public BiddingHotelsUpgrade()
        {
            lstRoomsData = new List<eBiddingHotelsUpgradeRoomsList>();
            
        }
        public string BIdUpgradeHotels { get; set; }
        public List<eBiddingHotelsUpgradeRoomsList> lstRoomsData { get; set; }
        public BidRoomsData objBidRoomsData { get; set; }
        public int iPropId { get; set; }
        public int iBookingId { get; set; }
        public int iRoomId { get; set; }
        public string sAuthCode { get; set; }
    }

    public class eBiddingHotelsUpgradeRoomsList
    {
        public string sRoomName { get; set; }
        public decimal dSizeMtr { get; set; }
        public decimal dSizeSqft { get; set; }
        public bool IsUpgradedRoom { get; set; }
        public int iPropId { get; set; }
        public decimal dPrice { get; set; }
        public decimal dDiscPrice { get; set; }
        public int iRoomId { get; set; }
        public int iRPId { get; set; }
        public decimal dPriceWithTax { get; set; }
        public decimal dTax { get; set; }
        public string sImgUrl { get; set; }
        public decimal TotalDifference { get; set; }
        public decimal TotalDifferenceConverted { get; set; }
        public decimal TaxDifference { get; set; }
        public string Amenity1 { get; set; }
        public string Amenity2 { get; set; }
        public string Amenity3 { get; set; }
        public string Amenity4 { get; set; }
        public string Amenity5 { get; set; }
        public string Amenity6 { get; set; }
        public string Amenity7 { get; set; }
        public string Amenity8 { get; set; }
        public string Symbol { get; set; }

        public eBiddingHotelsUpgradeRoomsList()
        {
            lstImages = new List<eBiddingHotelsUpgradeImages>();
            lstViews = new List<eBiddingHotelsUpgradeViews>();
            lstfacilities = new List<eBiddingHotelsUpgradeFacilities>();
        }

        public List<eBiddingHotelsUpgradeImages> lstImages { get; set; }
        public List<eBiddingHotelsUpgradeViews> lstViews { get; set; }
        public List<eBiddingHotelsUpgradeFacilities> lstfacilities { get; set; }

        public List<string> GetFirstFourAmenityUrl()
        {
            List<string> firstFourAmenityUrl = new List<string>();

            var properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var allAmenityProperty = properties.Where(x => x.Name.Contains("Amenity") && x.PropertyType.Name == "String").ToList();

            foreach (var item in allAmenityProperty)
            {
                var tValue = this.GetType().GetProperty(item.Name).GetValue(this, null).ToString();

                if (!string.IsNullOrEmpty(tValue))
                {
                    firstFourAmenityUrl.Add(tValue);
                }
            }
            return firstFourAmenityUrl.Take(4).ToList();
        }
    }

    public class eBiddingHotelsUpgradeImages
    {
        public int iPropId { get; set; }
        public int RoomId { get; set; }
        public bool bIsPrimaryR { get; set; }
        public string Image { get; set; }
    }
    public class eBiddingHotelsUpgradeViews
    {
        public int iPropId { get; set; }
        public int iRoomId { get; set; }
        public string sView { get; set; }
    }
    public class eBiddingHotelsUpgradeFacilities
    {
        public int iPropId { get; set; }
        public int iRoomId { get; set; }
        public string sFacility { get; set; }
    }
    #endregion
}