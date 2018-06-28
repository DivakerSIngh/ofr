using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eUserPropertyMap
    {
        public int iUserId { get; set; }
        public int iPropId { get; set; }
        public int sPropName { get; set; }
        public DateTime dtCreationDate { get; set; }
        public int iCreatedBy { get; set; }
        public DateTime dtActionDate { get; set; }
        public int iActionBy { get; set; }
        public string cStatus { get; set; }
    }
}