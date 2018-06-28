using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblLoyalityPoints
    {
        public short iLoyalityId { get; set; }
        public Nullable<int> iEarnMoney { get; set; }
        public Nullable<int> iEarnPoints { get; set; }
        public Nullable<int> iRedeemMoney { get; set; }
        public Nullable<int> iRedeemPoints { get; set; }
        public Nullable<int> iReferredPoints { get; set; }
        public Nullable<int> iReferPointsSignUp { get; set; }
        public Nullable<int> iSignUpPointsWithoutRef { get; set; }
        public Nullable<int> iExpiryDays { get; set; }
        public string cStatus { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }
    }
}