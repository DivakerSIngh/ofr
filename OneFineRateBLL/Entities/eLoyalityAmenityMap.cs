using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eLoyalityAmenityMap
    {
        public int iLoyalityAmenityId { get; set; }
        public int iPoints { get; set; }
        public int iAmenityId { get; set; }
        public string cStatus { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
    }
    public class eLoyalityAmenityDisp
    {
        public int iLoyalityAmenityId { get; set; }
        public int iPoints { get; set; }
        public string sAmenityName { get; set; }
        public string cStatus { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public string ActionBy { get; set; }
    }

    public class eLoyalityPointsCustomerData
    {
        public Int64 iCustomerId { get; set; }
        public long iId { get; set; }
        public Int32? iTotalPoints { get; set; }
        public Nullable<System.DateTime> dtCreatedOn { get; set; }
        public Nullable<System.DateTime> dtExpiryOriginal { get; set; }
        public Nullable<System.DateTime> dtExpiry { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public Int32? iUsedPoints { get; set; }
        public Int32? iRemPoints { get; set; }
        public string cType { get; set; }
        public string cStatus { get; set; }
    }

}