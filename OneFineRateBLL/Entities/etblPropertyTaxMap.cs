using OneFineRateAppUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateBLL.Entities
{
    public class etblPropertyTaxMap : IValidatableObject
    {
        [Key]
        public int iPropTaxId { get; set; }
        public int iPropId { get; set; }

        public Nullable<int> iRPId { get; set; }
        public Nullable<long> iRoomId { get; set; }
        public Nullable<System.DateTime> dtStayFrom { get; set; }
        public Nullable<System.DateTime> dtStayTo { get; set; }
        public string cStatus { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }

        public string Mode { get; set; }
        public string PlanId { get; set; }
        public List<eTax> ListTaxes { get; set; }
        public List<PNames> ListRatePlans { get; set; }
        public string SelectedTaxes { get; set; }
        public List<etblPropertyTaxesMap> PropertyTaxesList { get; set; }
        public string stayfrom { get; set; }
        public string stayto { get; set; }
        public string st { get; set; }
        public string msg { get; set; }
        public string heading { get; set; }



        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var pStayFrom = new[] { "stayfrom" };
            if (clsUtils.ConvertddmmyyyytoDateTime(stayfrom) > clsUtils.ConvertddmmyyyytoDateTime(stayto))
            {
                //  errormsg.Add(new ValidationResult("Booking To Date must be greater than Booking From Date", new[] { "dtBookingFrom" }));
                yield return new ValidationResult("Stay To Date must be greater than Stay From Date", pStayFrom);
            }
            // return errormsg;
        }
    }

    public class PropertyTaxList
    {
        public int iPropTaxId { get; set; }
        public string dtStayFrom { get; set; }
        public string dtStayTo { get; set; }
        public string RatePlan { get; set; }
        public string Tax { get; set; }
        public string cStatus { get; set; }
        public string dtActionDate { get; set; }
        public string ActionBy { get; set; }
    }

    public class PropertyRoomTaxList
    {
        public DateTime     dtStay          { get; set; }
        public int          iPropTaxId    {get; set;}
        public int          iPropId       {get; set;}
        public long?         iRoomId       {get; set;}
        public DateTime     dtActionDate    { get; set; }
        public int          iActionBy           { get; set; }
        public string cStatus { get; set; }


        public DateTime dtStayFrom	 { get; set; }
        public DateTime dtStayTo { get; set; }
    }
    
}