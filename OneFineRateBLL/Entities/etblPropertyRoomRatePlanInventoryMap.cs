using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblPropertyRoomRatePlanInventoryMap : IValidatableObject
    {
        public System.DateTime dtInventoryDate { get; set; }
        public int iPropId { get; set; }
        public long iRoomId { get; set; }
        public int iRPId { get; set; }
        public bool bCloseOut { get; set; }
        public Nullable<short> iMinLengthStay { get; set; }
        public Nullable<short> iMaxLengthStay { get; set; }
        public bool bCTA { get; set; }
        public bool bCTD { get; set; }
        public Nullable<decimal> dSingleRate { get; set; }
        public Nullable<decimal> dDoubleRate { get; set; }
        public Nullable<decimal> dTripleRate { get; set; }
        public Nullable<decimal> dQuadrupleRate { get; set; }
        public Nullable<decimal> dQuintrupleRate { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }

        public string sPlanName { get; set; }
        public string sCloseOut { get; set; }
        public string sCTA { get; set; }
        public string sCTD { get; set; }
        public string sRatePlan { get; set; }
        public string sRoomName { get; set; }
        public string SelectedOccupancies { get; set; }



        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var pBooking = new[] { "iMinLengthStay" };
            if (iMinLengthStay != null || iMaxLengthStay != null)
            {
                if (iMinLengthStay < 0)
                {
                    yield return new ValidationResult("Min./Max. Stay Length should not be negative.", pBooking);
                }
                else if (iMaxLengthStay < iMinLengthStay)
                {
                    yield return new ValidationResult("Max. Stay Length should be greater than Min. Stay Length.", pBooking);
                }
                else if ((iMaxLengthStay == 0 && iMinLengthStay != 0) || (iMaxLengthStay != 0 && iMinLengthStay == 0))
                {
                    yield return new ValidationResult("Either both Min value & Max value should be zero or none should be zero.", pBooking);
                }
            }

        }
    }
}
