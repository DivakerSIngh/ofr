using OneFineRate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class etblLocality
    {
        public int iLocalityId { get; set; }
        public int iCountryId { get; set; }
        public string sCountry { get; set; }
        public int iStateId { get; set; }
        public string sState { get; set; }
        public int iCityId { get; set; }
        public string sCity { get; set; }
        public int iAreaId { get; set; }
        public string sArea { get; set; }
        public string sLocality { get; set; }
        public string cStatus { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public int iActionBy { get; set; }

       


        //public virtual tblCityM tblCityM { get; set; }
        //public virtual tblCountryM tblCountryM { get; set; }
        //public virtual tblMacroAreaM tblMacroAreaM { get; set; }
        //public virtual tblStateM tblStateM { get; set; }
    }
    public class PNames
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int iCityId { get; set; }
       // public string VId { get; set; }
    }

   


    public class eDropDown
    {
        public eDropDown()
        {
            LocalityCordinates = new etblIndianLocalityCordinate();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public etblIndianLocalityCordinate LocalityCordinates { get; set; }
    }

    public class PropDetailsForHotelBooking
    {
        public int iLocalityId { get; set; }
        public int ipropId { get; set; }
        public Int16? ipropStarCategoryId { get; set; }
        public string propName { get; set; }
        public string propImageURL { get; set; }
        public string propStatus { get; set; }
        public string localityName { get; set; }

        public Nullable<System.DateTime> dtCheckIn { get; set; }
        public Nullable<System.DateTime> dtCheckOut { get; set; }

        public string dmainPrice { get; set; }
        public string ddiscountedPrice { get; set; }



    }

    public class PropDetailsForHotelBookingList
    {
        public PropDetailsForHotelBookingList()
        {
            PropDetailsList = new List<PropDetailsForHotelBooking>();
        }
        public List<PropDetailsForHotelBooking> PropDetailsList { get; set; }
    }


    public class etblLocalityList
    {
        public int iLocalityId { get; set; }
        public int iCountryId { get; set; }
        public string sCountry { get; set; }
        public int iStateId { get; set; }
        public string sState { get; set; }
        public int iCityId { get; set; }
        public string sCity { get; set; }
        public int iAreaId { get; set; }
        public string sArea { get; set; }
        public string sLocality { get; set; }
        public string cStatus { get; set; }
        public Nullable<System.DateTime> dtActionDate { get; set; }
        public string iActionBy { get; set; }
        public int iPropId { get; set; }
    }
}