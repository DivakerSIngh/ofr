using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblPropertyAmenitiesMap
    {
        public int iPropId { get; set; }
        public string sHotelAmenities { get; set; }
        public string sHotelParkings { get; set; }
        public string sHotelCommonServices { get; set; }
        public string sHotelConvenience { get; set; }
        public string sHotelLeisure { get; set; }
        public string sHotelMeetings { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
        public string sHotelParkingRadio { get; set; }
        public bool bAirportTransferAvalible { get; set; }

        public List<CheckBoxList> CommonServicesItems { get; set; }
        public List<Int32> SelectedCommonServices { get; set; }
        public List<CheckBoxList> ConveniencesItems { get; set; }
        public List<Int32> SelectedConveniences { get; set; }
        public List<CheckBoxList> LeisureItems { get; set;}
        public List<Int32> SelectedLeisure { get; set; }
        public List<CheckBoxList> MeetingsItems { get; set;}
        public List<Int32> SelectedMeetings { get; set; }

        public List<etblHotelAmenityM> HotelAmenities { get; set; }
        public List<etblHotelParkingM> HotelParkingsRadio { get; set; }
        public List<CheckBoxList> HotelParkingsItems { get; set; }
        public List<Int32> SelectedHotelParkings { get; set; }
        public  List<etblPropertyParkingMap> HotelParkingsMapList { get; set; }
        public string SelectedParkings { get; set; }
        public string JsonParkingMapData { get; set; }
    }
}