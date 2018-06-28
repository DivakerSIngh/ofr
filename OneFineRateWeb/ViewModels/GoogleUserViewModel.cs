using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneFineRateWeb.ViewModels.Models
{
  
    public class GoogleUserEmail
    {
        public string value { get; set; }
        public string type { get; set; }
    }

    public class GoogleUserName
    {
        public string familyName { get; set; }
        public string givenName { get; set; }
    }

    public class GoogleUserImage
    {
        public string url { get; set; }
        public bool isDefault { get; set; }
    }

    public class GoogleUserOrganization
    {
        public string name { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string startDate { get; set; }
        public bool primary { get; set; }
        public string endDate { get; set; }
    }

    public class GoogleUserPlacesLived
    {
        public string value { get; set; }
        public bool primary { get; set; }
    }

    public class GoogleUserCoverPhoto
    {
        public string url { get; set; }
        public int height { get; set; }
        public int width { get; set; }
    }

    public class GoogleUserCoverInfo
    {
        public int topImageOffset { get; set; }
        public int leftImageOffset { get; set; }
    }

    public class GoogleUserCover
    {
        public string layout { get; set; }
        public GoogleUserCoverPhoto coverPhoto { get; set; }
        public GoogleUserCoverInfo coverInfo { get; set; }
    }

    public class GoogleUserViewModel
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public List<GoogleUserEmail> emails { get; set; }
        public string objectType { get; set; }
        public string id { get; set; }
        public string displayName { get; set; }
        public GoogleUserName name { get; set; }
        public string url { get; set; }
        public GoogleUserImage image { get; set; }
        public List<GoogleUserOrganization> organizations { get; set; }
        public List<GoogleUserPlacesLived> placesLived { get; set; }
        public bool isPlusUser { get; set; }
        public string language { get; set; }
        public int circledByCount { get; set; }
        public bool verified { get; set; }
        public GoogleUserCover cover { get; set; }

        public string birthday { get; set; }
    }

}
