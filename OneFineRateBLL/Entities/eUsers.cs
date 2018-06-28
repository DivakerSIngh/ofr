using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eUsers
    {
        public int iUserId { get; set; }
        public string sUserName { get; set; }
        public string sPassword { get; set; }
        public string sFirstName { get; set; }
        public string sLastName { get; set; }
        public string sEmail { get; set; }
        public string sContact { get; set; }
        public DateTime dtCreatedOn { get; set; }
        public string cStatus { get; set; }
        public DateTime dtActionDate { get; set; }
        public int iActionBy { get; set; }
    }

    public class eUsersWithCount
    {
        public int iUserId { get; set; }
        public string sUserName { get; set; }
        public string sPassword { get; set; }
        public string sFirstName { get; set; }
        public string sLastName { get; set; }
        public string sEmail { get; set; }
        public string sContact { get; set; }
        public DateTime dtCreatedOn { get; set; }
        public string cStatus { get; set; }
        public DateTime dtActionDate { get; set; }
        public int HotelCount { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class eUserList
    {
        public int iUserId { get; set; }
        public string sUserName { get; set; }        
        public string sFirstName { get; set; }
        public string sLastName { get; set; }  
        public string sEmail { get; set; }
        public string sContact { get; set; }
        public DateTime dtCreatedOn { get; set; }
        public string cStatus { get; set; }
        public DateTime dtActionDate { get; set; }
        public string sGroup { get; set; }
        public int HotelCount { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class eUsersPropertyMap
    {
        public int iUserId { get; set; }
        public int iPropId { get; set; }
        public string sHotelName { get; set; }
        public string sAddress { get; set; }
        public string sWebSite { get; set; }
        public int Pending { get; set; }
        public int Arriving { get; set; }
    }
}