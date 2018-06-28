using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateBLL.Entities
{
    public class etblChangeHistory
    {
        public DateTime dtActionDateFrom { get; set; }
        public DateTime dtActionDateTo { get; set; }
        public DateTime dtEffectiveDateFrom { get; set; }
        public DateTime dtEffectiveDateTo { get; set; }
        public string SearchOn { get; set; }
        public int iRoomTypeId { get; set; }
        public int iRPId { get; set; }
        public int iPageId { get; set; }
        public int iBookingId { get; set; }
        public int iPropId { get; set; }
        public int iPromotionTypeId { get; set; }
        public string HotelName { get; set; }
        public DateTime dtActionDate { get; set; }
        public DateTime dtEffectiveDate { get; set; }
        public string sOld { get; set; }
        public string sNew { get; set; }
        public string sItem { get; set; }
        public string sImgUrl { get; set; }
        public string ActionBy { get; set; }
        public string CityName { get; set; }
        public string sRestaurantName { get; set; }

        public int iPromoId  {get;set;}
        public int iPromotionMapId  { get; set; }
        public string iRatePlanMapId { get; set; }
        public int iPropTaxId { get; set; }
        public List<SelectListItem> PropertyList { get; set; }
    }
    public class RoomNames
    {
        public int propId { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public long RoomTypeId { get; set; }
    }
    public class Menu
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long RoomTypeId { get; set; }
        public int? iParentId { get; set; }
        public int iMenuId { get; set; }
    }
    public class MenuPageName
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string iMenuId { get; set; }
    }
}