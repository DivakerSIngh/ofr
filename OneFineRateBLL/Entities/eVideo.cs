using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eVideo
    {
        public int Id { get; set; }
        public string sVideoUrl { get; set; }
        public string cStatus { get; set; }
        public DateTime dtActionDate { get; set; }
        public int iActionBy { get; set; }
    }
}