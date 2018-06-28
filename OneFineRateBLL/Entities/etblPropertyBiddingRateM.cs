using OneFineRateAppUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblPropertyBiddingRateM : IValidatableObject
    {
        public int iPropId { get; set; }
        public System.DateTime dtEffectiveDate { get; set; }
        [Required(ErrorMessage = "Required")]
        [DisplayName("Room Name")]
        public Nullable<long> iRoomId { get; set; }
        [Required(ErrorMessage = "Required")]
        [DisplayName("Rate Plan")]
        public Nullable<int> iRPId { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
        [Required(ErrorMessage = "Required")]
        [DisplayName("Date From")]
        public string datefrom { get; set; }
        [Required(ErrorMessage = "Required")]
        [DisplayName("Date To")]
        public string dateto { get; set; }
        public string selectedRoomPrices { get; set; }
        public List<etblPropertyBiddingRateM> selecteddates { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            var pBooking = new[] { "dateto" };
            if (clsUtils.ConvertddmmyyyytoDateTime(datefrom) > clsUtils.ConvertddmmyyyytoDateTime(dateto))
            {
                yield return new ValidationResult("From date must be greater than To date.", pBooking);
            }
        }
    }

    public class PropertyRooms
    {
        public long Roomid { get; set; }
        public string Currency { get; set; }
        public string Name { get; set; }
    }
}

