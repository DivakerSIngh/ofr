using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblPropertyRecreationMap
    {
        public int iPropId { get; set; }
        public string sRecreationFacilityId { get; set; }
        public string sLandActivityId { get; set; }
        public string sGolfId { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public Nullable<int> iActionBy { get; set; }

        public List<CheckBoxList> OnSiteRecreationfacilitiesItems { get; set; }
        public List<Int32> SelectedOnSiteRecreationfacilities { get; set; }
        public List<CheckBoxList> LandActivitiesItems { get; set; }
        public List<Int32> SelectedLandActivities { get; set; }
        public List<CheckBoxList> GolfItems { get; set; }
        public List<Int32> SelectedGolf { get; set; }
      
    }
}