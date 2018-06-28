using OneFineRate;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace OneFineRateBLL
{
    public static class BL_UserPropertyMap
    {
        public static string GenrateMenu(int UserId, string search)
        {
            using (OneFineRateEntities _context = new OneFineRateEntities())
            {
                var result = _context.uspGetPropertiesAndChainByUser(UserId, "%" + search + "%");

                var lstResult = result.ToList();

                string ReturnString = "";

                foreach (var item in lstResult)
                {
                    ReturnString += "{ \"id\" : " + item.iPropId + ", \"name\": \"" + item.PropName.Trim() + "\", \"parent\": \"" + item.chainName.Trim() + "\", \"checked\": \"" + item.Mapped + "\"},";
                }
                try
                {
                    ReturnString = "[" + ReturnString.Substring(0, ReturnString.Length - 1) + "]";
                }
                catch (Exception)
                {
                    ReturnString = "[]";
                }
                return ReturnString;
            }
        }

        public static string GenrateMenuForMappingShow(int UserId)
        {
            using (OneFineRateEntities _context = new OneFineRateEntities())
            {
                var result = _context.uspGetPropertiesAndChainByUserForShow(UserId);

                var lstResult = result.ToList();

                string ReturnString = "";

                foreach (var item in lstResult)
                {
                    ReturnString += "{ \"id\" : " + item.iPropId + ", \"name\": \"" + item.PropName.Trim() + "\", \"parent\": \"" + item.chainName.Trim() + "\", \"checked\": \"" + item.Mapped + "\"},";
                }
                try
                {
                    ReturnString = "[" + ReturnString.Substring(0, ReturnString.Length - 1) + "]";
                }
                catch (Exception)
                {
                    ReturnString = "[]";
                }
                return ReturnString;
            }
        }

        public static int SaveHotelMapping(int id, string Hotels, int iActionBy)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {

                    try
                    {
                        var obj1 = db.tblUserPropertyMaps.Where(u => u.iUserId == id);
                        db.tblUserPropertyMaps.RemoveRange(obj1);
                    }
                    catch (Exception) { }


                    var Hotel = Hotels.Split(',').ToList();

                    db.tblUserPropertyMaps.AddRange(Hotel.Where(hotel => !hotel.Contains("Parent")).Select(hotel => new tblUserPropertyMap()
                    {                        
                        cStatus = "A",
                        dtActionDate = DateTime.Now,
                        dtCreationDate = DateTime.Now,
                        iActionBy = iActionBy,
                        iCreatedBy = iActionBy,
                        iPropId =  Convert.ToInt32(hotel),
                        iUserId = id
                    }));
                    db.SaveChanges();

                    retval = 1;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retval;
        }

        public static int SaveFilterHotelMapping(int id, string MapHotels, string UnmapHotels, int iActionBy)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {

                    try
                    {
                        var TempHotelDel = UnmapHotels.Split(',').ToList();
                        var HotelDel = TempHotelDel.Where(hotel => !hotel.Contains("Parent"));
                        var obj1 = db.tblUserPropertyMaps.Where(u => u.iUserId == id && HotelDel.Contains(u.iPropId.ToString()));
                        db.tblUserPropertyMaps.RemoveRange(obj1);

                    }
                    catch (Exception) { }


                    var Hotel = string.IsNullOrEmpty(MapHotels.Trim())? null : MapHotels.Split(',').ToList();
                    if (Hotel != null)
                        {
                        db.tblUserPropertyMaps.AddRange(Hotel.Where(hotel => !hotel.Contains("Parent")).Select(hotel => new tblUserPropertyMap()
                        {
                            cStatus = "A",
                            dtActionDate = DateTime.Now,
                            dtCreationDate = DateTime.Now,
                            iActionBy = iActionBy,
                            iCreatedBy = iActionBy,
                            iPropId = Convert.ToInt32(hotel),
                            iUserId = id
                        }));
                    }
                    db.SaveChanges();

                    retval = 1;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return retval;
        }
    }
}