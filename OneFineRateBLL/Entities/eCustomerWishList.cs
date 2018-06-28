using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eCustomerWishList
    {
        public Nullable<int> iPropId { get; set; }
        public Nullable<long> iCustomerId { get; set; }
        public string sRoomRateMin { get; set; }
        public string sRoomRateMax { get; set; }

        public string sHotelName { get; set; }
        public short iStarCategoryId { get; set; }
        public float fTripAdvisorRating { get; set; }
        public string sDescription { get; set; }
        public string sImgUrl { get; set; }
        public string sLocality { get; set; }

        public int iPropertyCountInCity { get; set; }
        public int iPropertyRankInCity { get; set; }
        public string sLastBookedStatus { get; set; }
        public bool isFewRoomsAvailable { get; set; }
        public string sOffer { get; set; }
        public List<string> lstAmenities { get; set; }
        public string sCity { get; set; }

        public decimal dTripAdvisorRating { get; set; }
        public string sTripAdvisorRatingImageUrl { get; set; }
        public string sTripAdvisorRankingString { get; set; }
    }
}