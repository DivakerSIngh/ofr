using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eCustomerViewedItems : IHotelResponseType
    {
        public long? iCustomerId { get; set; }
        public string iVendorId { get; set; }
        public string sHotelName { get; set; }
        public int iPropId { get; set; }
        public string PageLinkUrl { get; set; }
        public string sImageUrl { get; set; }
        public short? iStarCategoryId { get; set; }
        public string sRoomRate { get; set; }
        public bool? bIsTG { get; set; }
        public decimal dPrice {get; set;}
       
    }

    //public abstract class CustomerViewItem : IHotelResponseType
    //{
    //    public long? iCustomerId { get; set; }
    //    public string iVendorId { get; set; }
    //    public string sHotelName { get; set; }
    //    public int iPropId { get; set; }
    //    public string PageLinkUrl { get; set; }
    //    public string sImageUrl { get; set; }
    //    public short? iStarCategoryId { get; set; }
    //    public string sRoomRate { get; set; }
    //    public bool? bIsTG { get; set; }
    //    public decimal dPrice { get; set; }
    //}

    //public class UserMayLike : CustomerViewItem
    //{

    //}

    //public class RecentViews : CustomerViewItem
    //{

    //}
}