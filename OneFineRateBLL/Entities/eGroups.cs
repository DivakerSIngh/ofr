using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OneFineRateBLL.Entities
{
    public class eGroups
    {
        public int iGroupId { get; set; }
        public string sGroupName { get; set; }
        public string sDescription { get; set; }
        public DateTime dtCreationDate { get; set; }
        public int iCreatedBy { get; set; }
        public string cStatus { get; set; }
        public DateTime dtActionDate { get; set; }
        public int iActionBy { get; set; }
    }

    public class GroupsForGrid
    {
        public int iGroupId { get; set; }
        public string sGroupName { get; set; }
        public string sDescription { get; set; }
        public DateTime dtCreationDate { get; set; }
        public string CreatedBy { get; set; }
        public string cStatus { get; set; }
        public DateTime dtActionDate { get; set; }
        public string ActionBy { get; set; }
    }
}