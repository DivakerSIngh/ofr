using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblPhotoCategoryM
    {
        public int iPhotoCatId { get; set; }
        public string sPhotoCatName { get; set; }
    }

    public class etblPhotoSubCategoryM
    {
        public long iPhotoCatId { get; set; }
        public string sPhotoCatName { get; set; }
    }
}