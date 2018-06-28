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
    public class BL_tblPropertyPromoMap
    {
        public static string GenrateMenu(int promoid, string search)
        {
            using (OneFineRateEntities _context = new OneFineRateEntities())
            {
                var result = _context.uspGetPropertiesAndChainByPromoCode(promoid, "%" + search + "%");

                // var lstResult = result.Where(x => x.Mapped == 1).ToList();
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
                        var obj1 = db.tblPropertyPromoMaps.Where(u => u.iPromoCodeId == id);
                        db.tblPropertyPromoMaps.RemoveRange(obj1);
                    }
                    catch (Exception) { }


                    var Hotel = Hotels.Split(',').ToList();

                    db.tblPropertyPromoMaps.AddRange(Hotel.Where(hotel => !hotel.Contains("Parent")).Select(hotel => new tblPropertyPromoMap()
                    {
                        cStatus = "A",
                        dtActionDate = DateTime.Now,
                        iActionBy = iActionBy,
                        iPropId = Convert.ToInt32(hotel),
                        iPromoCodeId = id
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
                        var obj1 = db.tblPropertyPromoMaps.Where(u => u.iPromoCodeId == id && HotelDel.Contains(u.iPropId.ToString()));
                        db.tblPropertyPromoMaps.RemoveRange(obj1);

                    }
                    catch (Exception) { }


                    var Hotel = MapHotels.Split(',').ToList();
                    db.tblPropertyPromoMaps.AddRange(Hotel.Where(hotel => !hotel.Contains("Parent")).Select(hotel => new tblPropertyPromoMap()
                    {
                        cStatus = "A",
                        dtActionDate = DateTime.Now,
                        iActionBy = iActionBy,
                        iPropId = Convert.ToInt32(hotel),
                        iPromoCodeId = id
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

        public static ePromoValidate VaidatePromoCode(string PromoCode, DateTime BookingDate, DateTime CheckIn, DateTime CheckOut, int PropId, decimal BaseAmt, decimal ExtraAmt, decimal TaxesAmt, string CCode)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                ePromoValidate eobj = new ePromoValidate();
                SqlParameter[] MyParam = new SqlParameter[9];
                MyParam[0] = new SqlParameter("@sPromoCode", PromoCode);
                MyParam[1] = new SqlParameter("@dtBooking", BookingDate);
                MyParam[2] = new SqlParameter("@dtCheckin", CheckIn);
                MyParam[3] = new SqlParameter("@dtCheckout", CheckOut);
                MyParam[4] = new SqlParameter("@iPropId", PropId);
                MyParam[5] = new SqlParameter("@dAmount", BaseAmt);
                MyParam[6] = new SqlParameter("@dExtraAmount", ExtraAmt);
                MyParam[7] = new SqlParameter("@dTaxAmount", TaxesAmt);
                MyParam[8] = new SqlParameter("@CurrencyCode", CCode);

                eobj = db.Database.SqlQuery<ePromoValidate>("uspGetPromoDiscount @sPromoCode,@dtBooking,@dtCheckin,@dtCheckout,@iPropId,@dAmount,@dExtraAmount,@dTaxAmount,@CurrencyCode ", MyParam).SingleOrDefault();
               
                return eobj;
            }
        }

        public class ePromoValidate
        {
            public int Status { get; set; }
            public decimal Discount { get; set; }
            public decimal Net_Amt { get; set; }
            public string Error { get; set; }
            public string Amenity { get; set; }
            public int? rtype { get; set; }
            public string DiscountValue { get; set; }
        }
    }
}