using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneFineRateWeb.ViewModels.Models
{
    public class FacebookUserViewModel
    {
        public string id { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string gender { get; set; }
        public Picture picture { get; set; }

        public string birthday { get; set; }
    }

    public class Picture
    {
        public PicureData data { get; set; }
    }

    public class PicureData
    {
        public string url { get; set; }
    }
}
