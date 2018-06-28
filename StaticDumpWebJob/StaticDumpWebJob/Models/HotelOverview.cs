using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticDumpWebJob.Models
{
    public class HotelOverview
    {
        public string VendorId { get; set; }
        public string VendorName { get; set; }

        public string Address { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Location { get; set; }
        public string PinCode { get; set; }
        public string Area { get; set; }
        public string Area_Seo_Id { get; set; }
        public string SecondaryAreaIds { get; set; }
        public string SecondaryAreaName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public string HotelClass { get; set; }
        public string HotelOverviewDescription { get; set; }
        public string ReviewRating { get; set; }
        public string ReviewCount { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string DefaultCheckInTime { get; set; }
        public string DefaultCheckOutTime { get; set; }
        public string Hotel_Star { get; set; }
        public string HotelGroupID { get; set; }
        public string HotelGroupName { get; set; }
        public string ImagePath { get; set; }
        public string HotelSearchKey { get; set; }
        public string NoOfFloors { get; set; }
        public string NoOfRooms { get; set; }
        public string ThemeList { get; set; }
        public string CategoryList { get; set; }
        public string City_Zone { get; set; }
        public string WeekDay_Rank { get; set; }
        public string WeekEnd_Rank { get; set; }
    }
}
