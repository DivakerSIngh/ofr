using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eWebSiteSearchPage
    {
        public eWebSiteSearchPage()
        {
            PropertySearchedList = new List<PropSearchResponseModel>();
            TGPropertySearchedList = new List<PropSearchResponseModel>();
            TotalPropertySearchedList = new List<PropSearchResponseModel>();
        }

        public List<CheckBoxList> RoomFacilityItems { get; set; }
        public List<Int32> SelectedRoomFacility { get; set; }
        public List<CheckBoxList> HotelFacilityItems { get; set; }
        public List<Int32> SelectedHotelFacility { get; set; }
        public List<PropSearchResponseModel> PropertySearchedList { get; set; }

        public List<PropSearchResponseModel> TGPropertySearchedList { get; set; }

        public List<PropSearchResponseModel> TotalPropertySearchedList { get; set; }

        public string sCurrencySymbol { get; set; }
        public string sRequestType { get; set; }
        public decimal dExchangeRate { get; set; }
    }

    public class PropSearchRequestModel
    {
        public string Id { get; set; }
        public int cid { get; set; }
        public string ctype { get; set; }
        public string cname { get; set; }
        public string sCheckIn { get; set; }
        public string sCheckOut { get; set; }
        public string sRoomData { get; set; }
        public bool IsModify { get; set; }

        public int iAreaType { get; set; }
        public bool isSpecialDeal { get; set; }
     
        public DateTime dtCheckIn { get; set; }
        public DateTime dtCheckOut { get; set; }

        public decimal dMaxPrice { get; set; }
        public decimal dMinPrice { get; set; }
        
        public string sSelectedHotelFacility { get; set; }
        public string sSelectedRoomFacility { get; set; }

        public string sRequestType { get; set; }
        public string iVendorId { get; set; }
        public string sCurrency { get; set; }
        public string PropDetailsTempData { get; set; }

        #region roomFacilities

        public bool IsAirConditioning { get; set; }
        public bool IsBathtub { get; set; }
        public bool IsFlatScreenTV { get; set; }
        public bool IsSoundproof { get; set; }
        public bool IsView { get; set; }
        public bool IsInternetFacilities { get; set; }
        public bool IsPrivatePool { get; set; }
        public bool IsRoomService { get; set; }

        #endregion roomFacilities

        #region hotelFacilities

        public bool IsInternet { get; set; }
        public bool IsParking { get; set; }
        public bool IsNonSmoking { get; set; }
        public bool IsFacilitiesDifferentlyAbled { get; set; }
        public bool IsFitnessCenter { get; set; }
        public bool IsFamilyRooms { get; set; }
        public bool IsRestaurant { get; set; }
        public bool IsHotelRoomService { get; set; }
        public bool IsTransfers { get; set; }
        public bool IsPetFriendly { get; set; }
        public bool IsSpa { get; set; }
        public bool IsOutdoorPool { get; set; }
        public bool IsIndoorPool { get; set; }
        public bool IsChildcare { get; set; }
        public bool IsChildrensClub { get; set; }
        public bool IsLaundryServices { get; set; }
        public bool IsAirconditionerHotel { get; set; }

        #endregion hotelFacilities

        public PropSearchRequestModel GetThisWithRoomAndHotelFailities()
        {
            if (!string.IsNullOrEmpty(this.sSelectedRoomFacility))
            {
                var roomFacilitiesIntArr = StringToIntList(this.sSelectedRoomFacility);

                foreach (var roomFacilityId in roomFacilitiesIntArr)
                {
                    switch (roomFacilityId)
                    {
                        case 1: { this.IsAirConditioning = true; break; }
                        case 2: { this.IsBathtub = true; break; }
                        case 3: { this.IsFlatScreenTV = true; break; }
                        case 4: { this.IsSoundproof = true; break; }
                        case 5: { this.IsView = true; break; }
                        case 6: { this.IsInternetFacilities = true; break; }
                        case 7: { this.IsPrivatePool = true; break; }
                        case 8: { this.IsRoomService = true; break; }
                        default: break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(this.sSelectedHotelFacility))
            {
                var hotelFacilitiesIntArr = StringToIntList(this.sSelectedHotelFacility);

                foreach (var hotelFacilityId in hotelFacilitiesIntArr)
                {
                    switch (hotelFacilityId)
                    {
                        case 1: { this.IsAirconditionerHotel = true; break; }
                        case 2: { this.IsChildcare = true; break; }
                        case 3: { this.IsChildrensClub = true; break; }
                        case 4: { break; }
                        case 5: { this.IsFacilitiesDifferentlyAbled = true; break; }
                        case 6: { this.IsFamilyRooms = true; break; }
                        case 7: { this.IsFitnessCenter = true; break; }
                        case 8: { this.IsIndoorPool = true; break; }
                        case 9: { this.IsInternet = true; break; }
                        case 10: { this.IsLaundryServices = true; break; }
                        case 11: { this.IsNonSmoking = true; break; }
                        case 12: { this.IsOutdoorPool = true; break; }
                        case 13: { this.IsParking = true; break; }
                        case 14: { this.IsPetFriendly = true; break; }
                        case 15: { this.IsRestaurant = true; break; }
                        case 16: { this.IsHotelRoomService = true; break; }
                        case 17: { this.IsSpa = true; break; }
                        case 18: { this.IsTransfers = true; break; }
                        default: break;
                    }
                }
            }

            return this;
        }


     
        public bool bLogin { get; set; }

        public int iFilterLocality { get; set; }

        public string sHotelSearch { get; set; }

        public string sStarRating { get; set; }

        public long iUserId { get; set; }

        public IEnumerable<int> StringToIntList(string str)
        {
            if (String.IsNullOrEmpty(str))
                yield break;

            foreach (var s in str.Split(','))
            {
                int num;
                if (int.TryParse(s, out num))
                    yield return num;
            }
        }

    }

   

    public class RoomData
    {
        public int room { get; set; }
        public int adult { get; set; }
        public int child { get; set; }
        public List<ChildAge> ChildAge { get; set; }
        public long iBookingDetailId { get; set; }

    }

    public class ChildAge
    {
        public string Age { get; set; }
    }

    public class PropSearchResponseModel : IHotelResponseType
    {
        public PropSearchResponseModel()
        {
            FirstFourNonEmptyAmenity = new List<string>(4);
        }
        public string LastBook { get; set; }
        public int Looking { get; set; }
        public int Sponsored { get; set; }
        public int iRank { get; set; }
        public string sHotelName { get; set; }
        public short iStarCategoryId { get; set; }
        public string sImgUrl { get; set; }
        public string sLocality { get; set; }
        public decimal dPrice { get; set; }
        public decimal dPriceRP { get; set; }
        public decimal dDiscountPercent { get; set; }
        public int iInventory { get; set; }
        public int iPropId { get; set; }
        public string sDescription { get; set; }
        public decimal dLongitude { get; set; }
        public decimal dLatitude { get; set; }

        public bool isFewRoomsAvailable { get; set; }
        public bool sOffer { get; set; }

        public decimal rating { get; set; }
        public string rating_image_url { get; set; }
        public string ranking_string { get; set; }


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

        public bool bIsTG { get; set; }
        public string iVendorId { get; set; }
        public bool bIsFavourite { get; set; }

        public bool bIsTopHotel { get; set; }
        public bool bIsPopularHotel { get; set; }
        public bool bIsNew { get; set; }
        public bool bIsHighDemand { get; set; }
      
        public List<string> FirstFourNonEmptyAmenity { get; set; }
    }
}