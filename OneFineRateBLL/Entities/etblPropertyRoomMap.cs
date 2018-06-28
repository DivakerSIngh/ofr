using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
   // public class etblPropertyRoomMap : IValidatableObject
    public class etblPropertyRoomMap
    {
        public long iRoomId { get; set; }
        public int iPropId { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public short iRoomTypeId { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string sRoomName { get; set; }
        public string sRoomCode { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public Nullable<byte> iMaxOccupancy { get; set; }
        public Nullable<byte> iMaxChildren { get; set; }
        public Nullable<byte> iNoOfExtraBeds { get; set; }
        public Nullable<byte> iNoOfBedrooms { get; set; }
        public Nullable<byte> iNoOfBathrooms { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public Nullable<decimal> dSizeSqft { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public Nullable<decimal> dSizeMtr { get; set; }
        //[Required(ErrorMessage = "This field is required.")]
        public Nullable<short> iTotalRooms { get; set; }
        public Nullable<short> iTwinBeds { get; set; }
        public Nullable<short> iDoubleBeds { get; set; }
        public string sRoomAccessibilityIds { get; set; }
        public string sBuildingCharacteristicsIds { get; set; }
        public string sRoomOutdoorViewIds { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
        public bool bActive { get; set; }


        public string SizeType { get; set; }
        public string sSizeType { get; set; }
        public string Mode { get; set; }
        public string sRoomType { get; set; }
        public string Type { get; set; }
        public List<CheckBoxList> AccessibilityItems { get; set; }
        public List<Int32> SelectedAccessibility { get; set; }
        public List<CheckBoxList> BuildingcharacteristicsItems { get; set; }
        public List<Int32> SelectedBuildingcharacteristics { get; set; }
        public List<CheckBoxList> OutdoorViewItems { get; set; }
        public List<Int32> SelectedOutdoorView { get; set; }
        public Nullable<byte> iSeq { get; set; }


        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    var piTotalRooms = new[] { "iTotalRooms" };
        //    if (Convert.ToInt16(iTotalRooms) < (Convert.ToInt16(iTwinBeds) + Convert.ToInt16(iDoubleBeds)))
        //    {
        //        yield return new ValidationResult("Sum of Twin Rooms and Double Rooms should not exceed No. of Rooms.", piTotalRooms);
        //    }
        //}

    }
}