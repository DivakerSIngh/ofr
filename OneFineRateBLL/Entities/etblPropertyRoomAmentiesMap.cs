using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblPropertyRoomAmentiesMap
    {
        public int iPropId { get; set; }
        public string sBasicRoomAmenities { get; set; }
        public string sAdditionalRoomAmenities { get; set; }
        public string sBathRoomAmenities { get; set; }
        public string sBeddingRoomAmenities { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
        public Nullable<int> iBasicAmentiesRadio { get; set; }

        public List<etblHotelRoomAmenityM> RoomAnenitiesRadio { get; set; }
        public List<CheckBoxList> BasicRoomAmenitiesItems { get; set; }
        public List<Int32> SelectedBasicRoomAmenities { get; set; }
        public List<CheckBoxList> AdditionalAmenitiesItems { get; set; }
        public List<Int32> SelectedAdditionalAmenities { get; set; }
        public List<CheckBoxList> BathroomAmenitiesItems { get; set; }
        public List<Int32> SelectedBathroomAmenities { get; set; }
        public List<CheckBoxList> BeddingandLinensItems { get; set; }
        public List<Int32> SelectedBeddingandLinens { get; set; }
      
    }
}