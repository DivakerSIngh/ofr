using OneFineRate;
using OneFineRateBLL.Entities;
using OneFineRateBLL.WebUserEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Validation;
using System.Reflection;
using System.Net.Mail;

namespace OneFineRateBLL
{
    public enum CorporateEmailStatus
    {
        BlackListed,
        CorporateEmailNotRegistered,
        EmailOrDomainNotFound,
        ActiveUser
    }

    public class BL_WebsiteUser
    {
        #region functions

        public static WebsiteUser CheckCorporateCustomerById(long userId)
        {
            WebsiteUser websiteUser = null;

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var user = db.tblWebsiteUserMaters.Where(x => x.Id == userId)
                    .Select(x => new { x.Email, x.PhoneNumber, x.CorporateEmail, x.FirstName, x.LastName })
                    .FirstOrDefault();

                if (user != null)
                {
                    websiteUser = new WebsiteUser();
                    websiteUser.Id = userId;
                    websiteUser.Email = user.Email;
                    websiteUser.CorporateEmail = user.CorporateEmail;
                    websiteUser.PhoneNumber = user.PhoneNumber;
                    websiteUser.FirstName = user.FirstName;
                    websiteUser.LastName = user.LastName;

                    //if (string.IsNullOrEmpty(user.CorporateEmail))
                    //    websiteUser.CorporateEmail = user.Email;

                    websiteUser.CorporateCustomerStatus = IsCorporateCustomerExits(websiteUser);

                    if (websiteUser.CorporateCustomerStatus == CorporateEmailStatus.EmailOrDomainNotFound && string.IsNullOrEmpty(websiteUser.CorporateEmail))
                    {
                        websiteUser.CorporateCustomerStatus = CorporateEmailStatus.CorporateEmailNotRegistered;
                    }

                    //if (string.IsNullOrEmpty(websiteUser.CorporateEmail))
                    //{
                    //    websiteUser.CorporateCustomerStatus = CorporateEmailStatus.CorporateEmailNotRegistered;
                    //}
                    //else
                    //{
                    //    websiteUser.CorporateCustomerStatus = IsCorporateCustomerExits(websiteUser);
                    //}
                }
            }

            return websiteUser;
        }

        public static bool CheckIfEligibleToMakePayamentUsingRewardPoint(long bookingId, long customerId, out string message, out long oldBookingId)
        {
            bool userEligibleToMakePayment = false;
           
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                SqlParameter[] MyParam = new SqlParameter[2];
                MyParam[0] = new SqlParameter("@iCustomerId", customerId);
                MyParam[1] = new SqlParameter("@iBookingId ", bookingId);

                var ds = SqlHelper.ExecuteDataset(db.Database.Connection.ConnectionString, CommandType.StoredProcedure, "uspUpdateLoyaltyBookingStatus", MyParam);

                if (ds.Tables.Count > 0)
                {
                    message = ds.Tables[0].Rows[0]["Msg"].ToString();
                    oldBookingId = Convert.ToInt64(ds.Tables[0].Rows[0]["iOldBookingId"].ToString());
                    userEligibleToMakePayment = Convert.ToBoolean(ds.Tables[0].Rows[0]["cStatus"]);
                }
                else
                {
                    message = string.Empty;
                    oldBookingId = 0;
                }

                return userEligibleToMakePayment;
            }
        }

        public static int CancelLoyalityPointProgressBooking(long bookingId, long customerId)
        {
            int noOfAffectedRows = 0;

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                SqlParameter[] MyParam = new SqlParameter[2];
                MyParam[0] = new SqlParameter("@iCustomerId", customerId);
                MyParam[1] = new SqlParameter("@iBookingId ", bookingId);
                
                var ds = SqlHelper.ExecuteDataset(db.Database.Connection.ConnectionString, CommandType.StoredProcedure, "uspCancelLoyaltyBooking", MyParam);

                if (ds.Tables.Count > 0)
                {
                    noOfAffectedRows = Convert.ToInt32(ds.Tables[0].Rows[0]["cStatus"]);
                }

                return noOfAffectedRows;
            }
        }

        public static int UpdateLoyalityPointProgressBooking(long bookingId)
        {
            int noOfAffectedRows = 0;

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                SqlParameter[] MyParam = new SqlParameter[1];
                MyParam[0] = new SqlParameter("@iBookingId ", bookingId);

                var result = SqlHelper.ExecuteScalar(db.Database.Connection.ConnectionString, CommandType.StoredProcedure, "uspUpdateRedemptionLoyalityPointsByBooking", MyParam);
                noOfAffectedRows = Convert.ToInt32(result);
                return noOfAffectedRows;
            }
        }

        /// <summary>
        /// Check corporate status with both personal and corporate email address. Corporate Email has high Priority
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static CorporateEmailStatus IsCorporateCustomerExits(WebsiteUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                if (string.IsNullOrEmpty(user.Email))
                {
                    throw new ArgumentNullException("Email");
                }

                #region Corporate Validation

                string corporateDomainName = string.Empty;

                if (string.IsNullOrEmpty(user.CorporateEmail))
                {
                    return CorporateEmailStatus.EmailOrDomainNotFound;
                }
                else
                {
                    MailAddress corporateEmail = new MailAddress(user.CorporateEmail);

                    corporateDomainName = corporateEmail.Host.ToLower();
                }

                var blackListFound_corporate_email = db.tblBlackListedDomains.FirstOrDefault(x => x.sDomainName == user.CorporateEmail && x.cStatus == "A");

                if (blackListFound_corporate_email != null)
                {
                    return CorporateEmailStatus.BlackListed;
                }

                var corporateDomainFound_corporate_email = db.tblCorporateDomainMaps.FirstOrDefault(x => x.sDomainName == user.CorporateEmail && x.cStatus == "A");

                if (corporateDomainFound_corporate_email != null)
                {
                    System.Web.HttpContext.Current.Session["CompanyId"] = corporateDomainFound_corporate_email.iCompanyId;
                    return CorporateEmailStatus.ActiveUser;
                }

                var blackListFound_corporate_domain = db.tblBlackListedDomains.FirstOrDefault(x => x.sDomainName == corporateDomainName && x.cStatus == "A");


                if (blackListFound_corporate_domain != null)
                {
                    return CorporateEmailStatus.BlackListed;
                }

                var corporateDomainFound_corporate = db.tblCorporateDomainMaps.FirstOrDefault(x => x.sDomainName == corporateDomainName && x.cStatus == "A");

                if (corporateDomainFound_corporate != null)
                {
                    System.Web.HttpContext.Current.Session["CompanyId"] = corporateDomainFound_corporate.iCompanyId;
                    return CorporateEmailStatus.ActiveUser;
                }

                #endregion Corporate Validation


                #region Personal Email Validation


                MailAddress personalEmail = new MailAddress(user.Email);

                string personalDomainName = personalEmail.Host.ToLower();

                var blackListFound_personal_email = db.tblBlackListedDomains.FirstOrDefault(x => x.sDomainName == user.Email && x.cStatus == "A");

                if (blackListFound_personal_email != null)
                {
                    return CorporateEmailStatus.BlackListed;
                }

                var corporateDomainFound_personal_email = db.tblCorporateDomainMaps.FirstOrDefault(x => x.sDomainName == user.Email && x.cStatus == "A");

                if (corporateDomainFound_personal_email != null)
                {
                    System.Web.HttpContext.Current.Session["CompanyId"] = corporateDomainFound_personal_email.iCompanyId;
                    return CorporateEmailStatus.ActiveUser;
                }

                var corporateEmailRecord = db.tblCorporateDomainMaps.FirstOrDefault(x =>
                 x.cStatus == "I" && x.sDomainName == user.Email || x.sDomainName == user.CorporateEmail);

                if (corporateEmailRecord != null && personalDomainName == corporateDomainName)
                {
                    return CorporateEmailStatus.EmailOrDomainNotFound;
                }

                var blackListFound = db.tblBlackListedDomains.FirstOrDefault(x =>
                    x.cStatus == "A" && x.sDomainName == user.Email
                    || x.sDomainName == personalDomainName
                    || x.sDomainName == user.CorporateEmail
                    || x.sDomainName == corporateDomainName);

                if (blackListFound != null)
                {
                    return CorporateEmailStatus.BlackListed;
                }

                var corporateDomainFound = db.tblCorporateDomainMaps.FirstOrDefault(x =>
                    x.cStatus == "A" && x.sDomainName == user.CorporateEmail
                    || x.sDomainName == corporateDomainName
                    || x.sDomainName == user.Email
                    || x.sDomainName == personalDomainName);

                if (corporateDomainFound != null)
                {
                    System.Web.HttpContext.Current.Session["CompanyId"] = corporateDomainFound.iCompanyId;
                    return CorporateEmailStatus.ActiveUser;
                }
                else
                {
                    return CorporateEmailStatus.EmailOrDomainNotFound;
                }

                #endregion Personal Email Validation
            }
        }

        public static CorporateEmailStatus IsCorporateEmailExits(string corporateEmail)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                if (string.IsNullOrEmpty(corporateEmail))
                {
                    throw new ArgumentNullException("corporateEmail");
                }

                if (db.tblCorporateDomainMaps.Count(x => x.sDomainName == corporateEmail && x.cStatus == "I") > 0)
                {
                    return CorporateEmailStatus.EmailOrDomainNotFound;
                }

                MailAddress address = new MailAddress(corporateEmail);

                string domainName = address.Host;

                if (db.tblBlackListedDomains.Any(x => (x.sDomainName == corporateEmail || x.sDomainName == domainName && x.cStatus != "I")))
                {
                    return CorporateEmailStatus.BlackListed;
                }

                if (db.tblCorporateDomainMaps.Any(x => x.sDomainName == corporateEmail || x.sDomainName == domainName && x.cStatus != "I"))
                {
                    return CorporateEmailStatus.ActiveUser;
                }
                else
                {
                    return CorporateEmailStatus.EmailOrDomainNotFound;
                }
            }
        }

        public static string GetCompanyNameByCorporateEmail(string corporateEmail)
        {
            if (string.IsNullOrEmpty(corporateEmail))
            {
                return "";
                //throw new ArgumentNullException("corporateEmail");
            }

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                MailAddress mail_address_personal = new MailAddress(corporateEmail);

                string domainName_personal = mail_address_personal.Host;

                //First preference to Emaill Address then Domain
                var dbobj1 = db.tblCorporateDomainMaps.FirstOrDefault(u => u.sDomainName.Equals(corporateEmail, StringComparison.OrdinalIgnoreCase));
                if (dbobj1 != null)
                {
                    return dbobj1.tblCorporateCompanyM.sCompanyName;
                }

                var dbobj = db.tblCorporateDomainMaps.FirstOrDefault(u => u.sDomainName.Equals(domainName_personal, StringComparison.OrdinalIgnoreCase));

                if (dbobj != null)
                {
                    return dbobj.tblCorporateCompanyM.sCompanyName;
                }
            }

            return "Company";
        }


        public static tblCorporateCompanyM GetCorporateDetailByCorporateEmail(string corporateEmail)
        {
            if (string.IsNullOrEmpty(corporateEmail))
            {
                return null;
            }

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                MailAddress mail_address_personal = new MailAddress(corporateEmail);

                string domainName_personal = mail_address_personal.Host;

                //First preference to Emaill Address then Domain
                var dbobj1 = db.tblCorporateDomainMaps.FirstOrDefault(u => u.sDomainName.Equals(corporateEmail, StringComparison.OrdinalIgnoreCase));
                if (dbobj1 != null)
                {
                    return dbobj1.tblCorporateCompanyM;
                }

                var dbobj = db.tblCorporateDomainMaps.FirstOrDefault(u => u.sDomainName.Equals(domainName_personal, StringComparison.OrdinalIgnoreCase));

                if (dbobj != null)
                {
                    return dbobj.tblCorporateCompanyM;
                }
            }

            return null;
        }


        public static WebsiteUser CheckIfCustomerRegisteredWithCorporate(string corporateEmail)
        {
            if (string.IsNullOrEmpty(corporateEmail))
            {
                throw new ArgumentNullException("corporateEmail");
            }

            // Check also email is done by 13/6/2017

            WebsiteUser eobj = null;

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj1 = db.tblWebsiteUserMaters.FirstOrDefault(u => u.CorporateEmail.Equals(corporateEmail, StringComparison.OrdinalIgnoreCase));
                if (dbobj1 != null)
                {
                    eobj = (WebsiteUser)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj1, new WebsiteUser());
                    return eobj;
                }

                var dbobj = db.tblWebsiteUserMaters.FirstOrDefault(u => u.Email.Equals(corporateEmail, StringComparison.OrdinalIgnoreCase));
                if (dbobj != null)
                    eobj = (WebsiteUser)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, new WebsiteUser());
            }

            return eobj;
        }

        public static string GetProfileImageUrl(long userId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                string userProfileImageUrl = string.Empty;
                var profileImageUrl = db.tblWebsiteUserMaters.Where(u => u.Id == userId).Select(x => x.ProfileImageUrl).FirstOrDefault();
                if (!string.IsNullOrEmpty(profileImageUrl))
                {
                    userProfileImageUrl = ConfigurationManager.AppSettings["BlobUrl"] + profileImageUrl;
                }
                return userProfileImageUrl;
            }
        }

        //Add new record
        public static int AddRecord(WebsiteUser user)
        {
            int operationStatusValue = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblWebsiteUserMater dbuser = (OneFineRate.tblWebsiteUserMater)OneFineRateAppUtil.clsUtils.ConvertToObject(user, new OneFineRate.tblWebsiteUserMater());
                    db.tblWebsiteUserMaters.Add(dbuser);
                    db.SaveChanges();
                    operationStatusValue = 1;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return operationStatusValue;
        }

        //Delete a record
        public static int DeleteRecord(int id)
        {
            int operationStatus = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var obj = db.tblWebsiteUserMaters.SingleOrDefault(u => u.Id == id);
                    db.tblWebsiteUserMaters.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    operationStatus = 1;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return operationStatus;
        }

        //Update a record
        public static int UpdateRecord(WebsiteUser eobj)
        {
            int operationStatus = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblWebsiteUserMater obj = (OneFineRate.tblWebsiteUserMater)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblWebsiteUserMater());
                    db.tblWebsiteUserMaters.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    operationStatus = 1;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return operationStatus;
        }

        //Get Single Record
        public static WebsiteUser GetSingleRecordById(long id)
        {
            WebsiteUser eobj = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblWebsiteUserMaters.SingleOrDefault(u => u.Id == id);
                if (dbobj != null)
                    eobj = (WebsiteUser)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, new WebsiteUser());
            }
            return eobj;
        }


        public static etblWebsiteGuestUserMaster GetGuestRecordById(int id)
        {
            etblWebsiteGuestUserMaster eobj = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblWebsiteGuestUserMasters.SingleOrDefault(u => u.Id == id);
                if (dbobj != null)
                    eobj = (etblWebsiteGuestUserMaster)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, new etblWebsiteGuestUserMaster());
            }
            return eobj;
        }


        public static int UpdateGuestRecord(etblWebsiteGuestUserMaster eobj)
        {
            int operationStatus = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblWebsiteGuestUserMaster obj = (OneFineRate.tblWebsiteGuestUserMaster)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblWebsiteGuestUserMaster());
                    db.tblWebsiteGuestUserMasters.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    operationStatus = 1;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return operationStatus;
        }


        //Get Single Record
        public static WebsiteUser GetUserByReferralCode(string referralCode)
        {
            WebsiteUser eobj = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var user = db.tblWebsiteUserMaters.Where(x => x.sReferralCode.ToLower() == referralCode.ToLower()).FirstOrDefault();
                if (user != null)
                    eobj = (WebsiteUser)OneFineRateAppUtil.clsUtils.ConvertToObject(user, new WebsiteUser());
            }
            return eobj;
        }

        //Get Single Record
        public static long GetUserIdByReferralCode(string referralCode)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var userId = db.tblWebsiteUserMaters.Where(x => x.sReferralCode.ToLower() == referralCode.ToLower()).Select(x => x.Id).FirstOrDefault();
                return userId;
            }
        }

        //Get Single Record
        public static WebsiteUser GetSingleRecordByUserName(string userName)
        {
            WebsiteUser eobj = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblWebsiteUserMaters.FirstOrDefault(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
                if (dbobj != null)
                    eobj = (WebsiteUser)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, new WebsiteUser());
            }
            return eobj;

        }

        //Get Single Record
        public static WebsiteUser GetWebisteUserByEmail(string personalOrCorporate)
        {
            WebsiteUser eobj = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblWebsiteUserMaters.FirstOrDefault(u => u.Email.Equals(personalOrCorporate, StringComparison.OrdinalIgnoreCase)
                    || u.CorporateEmail.Equals(personalOrCorporate, StringComparison.OrdinalIgnoreCase));
                if (dbobj != null)
                    eobj = (WebsiteUser)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, new WebsiteUser());
            }
            return eobj;

        }

        public static bool IsPhoneNumberOrUserNameExists(string phone)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                return db.tblWebsiteUserMaters.Any(u => u.UserName == phone || u.PhoneNumber == phone);
            }
        }

        public static bool IsEmailExists(string email)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                return db.tblWebsiteUserMaters.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            }
        }

        public static bool IsAnyEmailExists(string email)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                return db.tblWebsiteUserMaters.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) ||
                    u.CorporateEmail.Equals(email, StringComparison.OrdinalIgnoreCase));
            }
        }

        public static bool IsAnyEmailExistsV1(string email, long userId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                return db.tblWebsiteUserMaters.Any(u => u.Id != userId && u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)
                    || u.CorporateEmail.Equals(email, StringComparison.OrdinalIgnoreCase));
            }
        }

        public static bool IsAnyEmailExistsV2(string email, long userId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                SqlParameter[] MyParam = new SqlParameter[2];
                MyParam[0] = new SqlParameter("@sEmail", email);
                MyParam[1] = new SqlParameter("@iUserId", userId);

                var status = SqlHelper.ExecuteScalar(db.Database.Connection.ConnectionString, CommandType.StoredProcedure, "uspCheckCorporateEmail", MyParam);

                return Convert.ToBoolean(status);
            }
        }

        public static bool IsCorporateEmailExists(string email)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                return db.tblWebsiteUserMaters.Any(u => u.CorporateEmail.Equals(email, StringComparison.OrdinalIgnoreCase));
            }
        }

        public static List<etblBookingTx> GetPendingNegotialions(out int totalPageCount, long customerId, int pageNumber, string BookingId, string EmailMobile, string currencyCode)
        {
            List<etblBookingTx> customerBookingList = new List<etblBookingTx>();

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                SqlParameter[] MyParam = new SqlParameter[5];
                MyParam[0] = new SqlParameter("@iCustomerId", customerId);
                MyParam[1] = new SqlParameter("@startRowIndex", pageNumber);
                MyParam[2] = new SqlParameter("@bookingid", BookingId);
                MyParam[3] = new SqlParameter("@emailmobile", EmailMobile);
                MyParam[4] = new SqlParameter("@Currency", currencyCode);

                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetPendingNegotiationByCust", MyParam);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    etblBookingTx booking = new etblBookingTx
                    {
                        iBookingId = Convert.ToInt64(dr["iBookingId"]),
                        sReservationDate = dr["dtReservationDate"].ToString(),
                        sImgUrl = dr["sImgUrl"].ToString(),
                        sHotelName = dr["sHotelName"].ToString(),
                        iStarCategoryId = Convert.ToSByte(dr["iStarCategoryId"]),
                        CheckIn = dr["CheckIn"].ToString(),
                        ChekOut = dr["ChekOut"].ToString(),
                        Nights = Convert.ToInt32(dr["Nights"]),
                        //Attempts = Convert.ToInt32(dr["Attempts"]),
                        Adults = Convert.ToInt32(dr["Adults"]),
                        Children = Convert.ToInt32(dr["Children"]),
                        Rooms = Convert.ToInt32(dr["Rooms"]),
                        sTotalNegotiateAmount = dr["dTotalNegotiateAmount"] != DBNull.Value ? dr["dTotalNegotiateAmount"].ToString() : string.Empty,
                        BookingStatus = dr["sBookingStatus"].ToString()
                    };

                    customerBookingList.Add(booking);
                }
                //get Total Count for show total records 
                //TotalCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                totalPageCount = Convert.ToInt32(ds.Tables[2].Rows[0][0]);

                return customerBookingList;
            }
        }

        public static List<etblBookingTx> GetCurrentNegotialions(out int TotalCount, long customerId, int pageNumber, string BookingId, string EmailMobile, string currencyCode)
        {
            List<etblBookingTx> customerBookingList = new List<etblBookingTx>();

            using (OneFineRateEntities db = new OneFineRateEntities())
            {

                SqlParameter[] MyParam = new SqlParameter[5];
                MyParam[0] = new SqlParameter("@iCustomerId", customerId);
                MyParam[1] = new SqlParameter("@startRowIndex", pageNumber);
                MyParam[2] = new SqlParameter("@bookingid", BookingId);
                MyParam[3] = new SqlParameter("@emailmobile", EmailMobile);
                MyParam[4] = new SqlParameter("@Currency", currencyCode);

                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetCurrentNegotiation", MyParam);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    etblBookingTx booking = new etblBookingTx
                    {
                        iBookingId = Convert.ToInt64(dr["iBookingId"]),
                        sTotalNegotiateAmount = dr["dTotalNegotiateAmount"] != DBNull.Value ? dr["dTotalNegotiateAmount"].ToString() : string.Empty,
                        sTotalAmount = dr["dTotalAmount"] != DBNull.Value ? dr["dTotalAmount"].ToString() : string.Empty,
                        sHotelName = dr["sHotelName"].ToString() + ", " + dr["sCity"].ToString(),
                        sPropertyCityName = dr["sCity"].ToString(),
                        BookingStatus = dr["sStatus"].ToString()
                    };
                    customerBookingList.Add(booking);
                }
                //get Total Count for show total records 
                TotalCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                // TotalCount = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                return customerBookingList;
            }
        }

        public static List<etblBookingTx> GetFutureBookings(out int TotalCount, long customerId, int pageNumber, string BookingId, string EmailMobile, string currencyCode)
        {
            List<etblBookingTx> customerBookingList = new List<etblBookingTx>();

            using (OneFineRateEntities db = new OneFineRateEntities())
            {

                SqlParameter[] MyParam = new SqlParameter[5];
                MyParam[0] = new SqlParameter("@iCustomerId", customerId);
                MyParam[1] = new SqlParameter("@startRowIndex", pageNumber);
                MyParam[2] = new SqlParameter("@bookingid", BookingId);
                MyParam[3] = new SqlParameter("@emailmobile", EmailMobile);
                MyParam[4] = new SqlParameter("@Currency", currencyCode);

                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetFutureBookingsByCust", MyParam);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    etblBookingTx booking = new etblBookingTx
                    {
                        iBookingId = Convert.ToInt64(dr["iBookingId"]),
                        sReservationDate = dr["dtReservationDate"].ToString(),
                        sImgUrl = dr["sImgUrl"].ToString(),
                        sHotelName = dr["sHotelName"].ToString(),
                        iStarCategoryId = Convert.ToSByte(dr["iStarCategoryId"]),
                        CheckIn = dr["CheckIn"].ToString(),
                        ChekOut = dr["ChekOut"].ToString(),
                        Nights = Convert.ToInt32(dr["Nights"]),
                        sExtra3 = Convert.ToString(dr["sExtra3"]),
                        Adults = Convert.ToInt32(dr["Adults"]),
                        Children = Convert.ToInt32(dr["Children"]),
                        Rooms = Convert.ToInt32(dr["Rooms"]),
                        sTotalNegotiateAmount = dr["dTotalNegotiateAmount"] != DBNull.Value ? dr["dTotalNegotiateAmount"].ToString() : string.Empty,
                        BookingStatus = dr["sBookingStatus"].ToString(),
                        cBookingType = dr["BookingType"].ToString(),
                        bIsTG = dr["bIsTG"] != DBNull.Value ? Boolean.Parse(dr["bIsTG"].ToString()) : false,
                        sPolicyName = Convert.ToString(dr["sPolicyName"])
                    };

                    customerBookingList.Add(booking);
                }
                //get Total Count for show total records 
                //TotalCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                TotalCount = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                return customerBookingList;
            }
        }

        public static List<etblBookingTx> GetPastSavings(out int TotalCount, long customerId, int pageNumber, string currencyCode)
        {
            List<etblBookingTx> customerBookingList = new List<etblBookingTx>();

            using (OneFineRateEntities db = new OneFineRateEntities())
            {

                SqlParameter[] MyParam = new SqlParameter[3];
                MyParam[0] = new SqlParameter("@iCustomerId", customerId);
                MyParam[1] = new SqlParameter("@startRowIndex", pageNumber);
                MyParam[2] = new SqlParameter("@Currency", currencyCode);

                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetCurrentNegotiationConfirmed", MyParam);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    etblBookingTx booking = new etblBookingTx
                    {
                        iBookingId = Convert.ToInt64(dr["iBookingId"]),
                        sHotelName = dr["sHotelName"].ToString() + ", " + dr["sCity"].ToString(),
                        sTotalAmount = dr["dTotalAmount"] != DBNull.Value ? dr["dTotalAmount"].ToString() : string.Empty,
                        sTotalNegotiateAmount = dr["dTotalNegotiateAmount"] != DBNull.Value ? dr["dTotalNegotiateAmount"].ToString() : string.Empty,
                        sBidAmount = dr["dBidAmount"] != DBNull.Value ? dr["dBidAmount"].ToString() : string.Empty,
                        sSavingsInPercentage = dr["sSavingsInPercentage"] != DBNull.Value ? dr["sSavingsInPercentage"].ToString() : string.Empty,
                        bIsTG = dr["bIsTG"] != DBNull.Value ? Boolean.Parse(dr["bIsTG"].ToString()) : false
                    };

                    customerBookingList.Add(booking);
                }

                TotalCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                return customerBookingList;
            }
        }

        public static List<etblBookingTx> GetPastBookings(out int TotalCount, long customerId, int pageNumber, string BookingId, string EmailMobile, string currencyCode)
        {
            List<etblBookingTx> customerBookingList = new List<etblBookingTx>();

            using (OneFineRateEntities db = new OneFineRateEntities())
            {

                SqlParameter[] MyParam = new SqlParameter[5];
                MyParam[0] = new SqlParameter("@iCustomerId", customerId);
                MyParam[1] = new SqlParameter("@startRowIndex", pageNumber);
                MyParam[2] = new SqlParameter("@bookingid", BookingId);
                MyParam[3] = new SqlParameter("@emailmobile", EmailMobile);
                MyParam[4] = new SqlParameter("@Currency", currencyCode);

                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetPastBookingsByCust", MyParam);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    etblBookingTx booking = new etblBookingTx
                    {
                        iBookingId = Convert.ToInt64(dr["iBookingId"]),
                        sReservationDate = dr["dtReservationDate"].ToString(),
                        sImgUrl = dr["sImgUrl"].ToString(),
                        sHotelName = dr["sHotelName"].ToString(),
                        iStarCategoryId = Convert.ToSByte(dr["iStarCategoryId"]),
                        CheckIn = dr["CheckIn"].ToString(),
                        ChekOut = dr["ChekOut"].ToString(),
                        Nights = Convert.ToInt32(dr["Nights"]),
                        //Attempts = Convert.ToInt32(dr["Attempts"]),
                        Adults = Convert.ToInt32(dr["Adults"]),
                        Children = Convert.ToInt32(dr["Children"]),
                        Rooms = Convert.ToInt32(dr["Rooms"]),
                        sTotalNegotiateAmount = dr["dTotalNegotiateAmount"] != DBNull.Value ? dr["dTotalNegotiateAmount"].ToString() : string.Empty,
                        //sBidAmount = dr["sBidAmount"] != DBNull.Value ? dr["dTotalNegotiateAmount"].ToString() : string.Empty,
                        BookingStatus = dr["BookingStatus"].ToString(),
                        cBookingType = dr["BookingType"].ToString(),
                        bIsTG = dr["bIsTG"] != DBNull.Value ? Boolean.Parse(dr["bIsTG"].ToString()) : false
                    };

                    customerBookingList.Add(booking);
                }

                //TotalCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                TotalCount = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                return customerBookingList;
            }
        }

        public static List<etblBookingTx> GetUnSuccessfulNegotiations(out int TotalCount, long customerId, int pageNumber, string BookingId, string EmailMobile, string currencyCode)
        {
            List<etblBookingTx> customerBookingList = new List<etblBookingTx>();

            using (OneFineRateEntities db = new OneFineRateEntities())
            {

                SqlParameter[] MyParam = new SqlParameter[5];
                MyParam[0] = new SqlParameter("@iCustomerId", customerId);
                MyParam[1] = new SqlParameter("@startRowIndex", pageNumber);
                MyParam[2] = new SqlParameter("@bookingid", BookingId);
                MyParam[3] = new SqlParameter("@emailmobile", EmailMobile);
                MyParam[4] = new SqlParameter("@Currency", currencyCode);

                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetUnSuccessfulBookingsByCust", MyParam);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    etblBookingTx booking = new etblBookingTx
                    {
                        iBookingId = dr["iBookingId"] != DBNull.Value ? Convert.ToInt64(dr["iBookingId"]) : default(long),
                        sReservationDate = dr["dtReservationDate"] != DBNull.Value ? dr["dtReservationDate"].ToString() : string.Empty,
                        sImgUrl = dr["sImgUrl"] != DBNull.Value ? dr["sImgUrl"].ToString() : string.Empty,
                        sHotelName = dr["sHotelName"] != DBNull.Value ? dr["sHotelName"].ToString() : string.Empty,
                        iStarCategoryId = dr["iStarCategoryId"] != DBNull.Value ? Convert.ToInt16(dr["iStarCategoryId"]) : default(short),
                        CheckIn = dr["CheckIn"] != DBNull.Value ? dr["CheckIn"].ToString() : string.Empty,
                        ChekOut = dr["ChekOut"] != DBNull.Value ? dr["ChekOut"].ToString() : string.Empty,
                        Nights = dr["Nights"] != DBNull.Value ? Convert.ToInt32(dr["Nights"]) : default(int),
                        //Attempts = Convert.ToInt32(dr["Attempts"]),
                        Adults = dr["Adults"] != DBNull.Value ? Convert.ToInt32(dr["Adults"]) : default(int),
                        Children = dr["Children"] != DBNull.Value ? Convert.ToInt32(dr["Children"]) : default(int),
                        Rooms = dr["Rooms"] != DBNull.Value ? Convert.ToInt32(dr["Rooms"]) : default(int),
                        sTotalNegotiateAmount = dr["dTotalNegotiateAmount"] != DBNull.Value ? dr["dTotalNegotiateAmount"].ToString() : string.Empty,
                        BookingStatus = dr["sBookingStatus"] != DBNull.Value ? dr["sBookingStatus"].ToString() : string.Empty,
                        bIsTG = dr["bIsTG"] != DBNull.Value ? Boolean.Parse(dr["bIsTG"].ToString()) : false
                    };

                    customerBookingList.Add(booking);
                }

                //TotalCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                TotalCount = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
                return customerBookingList;
            }
        }

        public static List<PropSearchResponseModel> GetCustomerWishList(long customerId, string currencyCode, out string currencySymbol)
        {
            var propertyList = new List<PropSearchResponseModel>();
            currencySymbol = string.Empty;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString()))
            {
                SqlParameter[] MyParam = new SqlParameter[2];
                MyParam[0] = new SqlParameter("@iUserId", customerId);
                MyParam[1] = new SqlParameter("@sCurrencyCode", currencyCode);

                var ds = SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, "uspGetWishListByCustomerId", MyParam);

                if (ds != null && ds.Tables != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        var responseModel = new PropSearchResponseModel();

                        responseModel.iPropId = row["iPropId"] != DBNull.Value ? Int32.Parse(row["iPropId"].ToString()) : 0;
                        responseModel.Amenity1 = row["Amenity1"] != DBNull.Value ? row["Amenity1"].ToString() : null;
                        responseModel.Amenity2 = row["Amenity2"] != DBNull.Value ? row["Amenity2"].ToString() : null;
                        responseModel.Amenity3 = row["Amenity3"] != DBNull.Value ? row["Amenity3"].ToString() : null;
                        responseModel.Amenity4 = row["Amenity4"] != DBNull.Value ? row["Amenity4"].ToString() : null;
                        responseModel.Amenity5 = row["Amenity5"] != DBNull.Value ? row["Amenity5"].ToString() : null;
                        responseModel.Amenity6 = row["Amenity6"] != DBNull.Value ? row["Amenity6"].ToString() : null;
                        responseModel.Amenity7 = row["Amenity7"] != DBNull.Value ? row["Amenity7"].ToString() : null;
                        responseModel.Amenity8 = row["Amenity8"] != DBNull.Value ? row["Amenity8"].ToString() : null;
                        responseModel.Amenity9 = row["Amenity9"] != DBNull.Value ? row["Amenity9"].ToString() : null;
                        responseModel.Amenity10 = row["Amenity10"] != DBNull.Value ? row["Amenity10"].ToString() : null;
                        responseModel.Amenity11 = row["Amenity11"] != DBNull.Value ? row["Amenity11"].ToString() : null;
                        responseModel.Amenity12 = row["Amenity12"] != DBNull.Value ? row["Amenity12"].ToString() : null;
                        responseModel.Amenity13 = row["Amenity13"] != DBNull.Value ? row["Amenity13"].ToString() : null;
                        responseModel.Amenity14 = row["Amenity14"] != DBNull.Value ? row["Amenity14"].ToString() : null;
                        responseModel.Amenity15 = row["Amenity15"] != DBNull.Value ? row["Amenity15"].ToString() : null;
                        responseModel.Amenity16 = row["Amenity16"] != DBNull.Value ? row["Amenity16"].ToString() : null;
                        responseModel.Amenity17 = row["Amenity17"] != DBNull.Value ? row["Amenity17"].ToString() : null;
                        responseModel.Amenity18 = row["Amenity18"] != DBNull.Value ? row["Amenity18"].ToString() : null;

                        responseModel.bIsTG = row["bIsTG"] != DBNull.Value ? Boolean.Parse(row["bIsTG"].ToString()) : false;
                        responseModel.dDiscountPercent = row["dDiscountPercent"] != DBNull.Value ? Decimal.Parse(row["dDiscountPercent"].ToString()) : 0;
                        responseModel.dLatitude = row["dLatitude"] != DBNull.Value ? Decimal.Parse(row["dLatitude"].ToString()) : 0;
                        responseModel.dLongitude = row["dLongitude"] != DBNull.Value ? Decimal.Parse(row["dLongitude"].ToString()) : 0;
                        responseModel.dPrice = row["dPrice"] != DBNull.Value ? Decimal.Parse(row["dPrice"].ToString()) : 0;
                        responseModel.dPriceRP = row["dPriceRP"] != DBNull.Value ? Decimal.Parse(row["dPriceRP"].ToString()) : 0;
                        responseModel.iInventory = row["iInventory"] != DBNull.Value ? Int32.Parse(row["iInventory"].ToString()) : 0;
                        responseModel.iRank = row["iRank"] != DBNull.Value ? Int32.Parse(row["iRank"].ToString()) : 0;
                        // isFewRoomsAvailable = row["isFewRoomsAvailable"] != DBNull.Value ? Boolean.Parse(row["isFewRoomsAvailable"].ToString()) : false;
                        responseModel.iStarCategoryId = row["iStarCategoryId"] != DBNull.Value ? Int16.Parse(row["iStarCategoryId"].ToString()) : default(Int16);
                        responseModel.iVendorId = row["iVendorId"] != DBNull.Value ? row["iVendorId"].ToString() : null;
                        responseModel.LastBook = row["LastBook"] != DBNull.Value ? row["LastBook"].ToString() : null;
                        responseModel.Looking = row["Looking"] != DBNull.Value ? Int32.Parse(row["Looking"].ToString()) : 0;
                        responseModel.ranking_string = row["ranking_string"] != DBNull.Value ? row["ranking_string"].ToString() : null;
                        responseModel.rating = row["rating"] != DBNull.Value ? Decimal.Parse(row["rating"].ToString()) : 0;
                        responseModel.rating_image_url = row["rating_image_url"] != DBNull.Value ? row["rating_image_url"].ToString() : null;
                        //sDescription = row["sDescription"] != DBNull.Value ? row["sDescription"].ToString() : null;
                        responseModel.sHotelName = row["sHotelName"] != DBNull.Value ? row["sHotelName"].ToString() : null;
                        responseModel.sImgUrl = row["sImgUrl"] != DBNull.Value ? row["sImgUrl"].ToString() : null;
                        responseModel.sLocality = row["sLocality"] != DBNull.Value ? row["sLocality"].ToString() : null;
                        //sOffer = row["sOffer"] != DBNull.Value ? row["sOffer"].ToString() : null;
                        //responseModel.Sponsored = row["Sponsored"] != DBNull.Value ? Int32.Parse(row["Sponsored"].ToString()) : 0;

                        responseModel.iVendorId = row["iVendorId"] != DBNull.Value ? row["iVendorId"].ToString() : null;
                        responseModel.bIsFavourite = int.Parse(row["bIsFavourite"].ToString()) == 1 ? true : false;

                        responseModel.bIsTopHotel = int.Parse(row["bIsTopHotel"].ToString()) == 1 ? true : false;
                        responseModel.bIsPopularHotel = int.Parse(row["bIsPopularHotel"].ToString()) == 1 ? true : false;
                        responseModel.bIsNew = int.Parse(row["bIsNew"].ToString()) == 1 ? true : false;
                        responseModel.bIsHighDemand = int.Parse(row["bIsHighDemand"].ToString()) == 1 ? true : false;

                        responseModel.FirstFourNonEmptyAmenity = GetFirstFourAmenityUrl(responseModel);

                        propertyList.Add(responseModel);
                    }

                    currencySymbol = Convert.ToString(ds.Tables[1].Rows[0][0]);
                }

                return propertyList;
            }
        }

        public static List<string> GetFirstFourAmenityUrl(PropSearchResponseModel type)
        {
            List<string> firstFourAmenityUrl = new List<string>();

            var properties = type.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var allAmenityProperty = properties.Where(x => x.Name.Contains("Amenity") && x.PropertyType.Name == "String").ToList();

            foreach (var item in allAmenityProperty)
            {
                var tValue = type.GetType().GetProperty(item.Name).GetValue(type, null).ToString();

                if (!string.IsNullOrEmpty(tValue))
                {
                    firstFourAmenityUrl.Add(tValue);
                }
            }
            return firstFourAmenityUrl.Take(4).ToList();
        }

        public static List<eCustomerViewedItems> GetRecentlyViewedItems(long customerId, out List<eCustomerViewedItems> customerMayLikeItems)
        {
            List<eCustomerViewedItems> customerRecentlyViewedItems = new List<eCustomerViewedItems>();
            customerMayLikeItems = new List<eCustomerViewedItems>();

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                SqlParameter[] param = new SqlParameter[1] { new SqlParameter("@iCustomerId", customerId) };

                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "uspGetYouMayLikeHotels", param);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    eCustomerViewedItems viewItem = new eCustomerViewedItems();

                    viewItem.bIsTG = dr["bIsTG"] != DBNull.Value ? Boolean.Parse(dr["bIsTG"].ToString()) : false;
                    viewItem.iVendorId = dr["iVendorId"] != DBNull.Value ? dr["iVendorId"].ToString() : null;
                    viewItem.iPropId = Convert.ToInt32(dr["iPropId"]);
                    viewItem.iStarCategoryId = Convert.ToSByte(dr["iStarCategoryId"]);
                    viewItem.sImageUrl = Convert.ToString(dr["sImgUrl"]);
                    viewItem.sHotelName = dr["sHotelName"].ToString();
                    if (dr["dRoomRate"] != DBNull.Value)
                    {
                        viewItem.sRoomRate = dr["dRoomRate"] != DBNull.Value ? dr["dRoomRate"].ToString() : string.Empty;
                    }

                    customerRecentlyViewedItems.Add(viewItem);
                }

                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    eCustomerViewedItems likeItem = new eCustomerViewedItems
                    {
                        iVendorId = dr["iVendorId"] != DBNull.Value ? dr["iVendorId"].ToString() : null,
                        bIsTG = dr["bIsTG"] != DBNull.Value ? Boolean.Parse(dr["bIsTG"].ToString()) : false,
                        iPropId = Convert.ToInt32(dr["iPropId"]),
                        iStarCategoryId = Convert.ToSByte(dr["iStarCategoryId"]),
                        sImageUrl = Convert.ToString(dr["sImgUrl"]),
                        sHotelName = dr["sHotelName"].ToString(),
                        sRoomRate = dr["dRoomRate"] != DBNull.Value ? dr["dRoomRate"].ToString() : string.Empty
                    };

                    customerMayLikeItems.Add(likeItem);
                }

                return customerRecentlyViewedItems;
            }
        }

        public static eLoyalityPoints GetLoyalityPoints()
        {
            eLoyalityPoints eobj = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var loyalityPoint = db.tblLoyalityPointsMs.Where(x => x.cStatus.Equals("A", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (loyalityPoint != null)
                {
                    eobj = (eLoyalityPoints)OneFineRateAppUtil.clsUtils.ConvertToObject(loyalityPoint, new eLoyalityPoints());
                }
                return eobj;
            }
        }

        //Get Single Record
        public static eLoyalityPointsCustomerData GetCustomerLoyalityPoints(long userId)
        {
            eLoyalityPointsCustomerData eobj = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                eobj = (from t in db.tblCustomerPointsMaps
                        select new eLoyalityPointsCustomerData
                        {
                            iId = t.iId,
                            iCustomerId = t.iCustomerId,
                            iTotalPoints = t.iTotalPoints,
                            iUsedPoints = t.iUsedPoints,
                            iRemPoints = t.iRemPoints,
                            dtCreatedOn = t.dtCreatedOn,
                            dtExpiryOriginal = t.dtExpiryOriginal,
                            dtExpiry = t.dtExpiry,
                            cType = t.cType,
                            cStatus = t.cStatus,

                        }).Where(u => u.iCustomerId == userId).FirstOrDefault();
            }
            return eobj;

        }

        //Get Single Record
        public static int UpdateCustomerLoyalityPoints(long refreeUserId, long signUpUserId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var idParam = new SqlParameter[2];
                idParam[0] = new SqlParameter("@iReferralId", refreeUserId);
                idParam[1] = new SqlParameter("@iSingnUpId", signUpUserId);

                var objTblRatePlan = db.Database.ExecuteSqlCommand("uspSetRefferalPoints @iReferralId, @iSingnUpId", idParam);
                return 1;
            }
        }

        //public static bool CheckIfCorporateCustomer(long userId)
        //{
        //    using (OneFineRateEntities db = new OneFineRateEntities())
        //    {
        //        var idParam = new SqlParameter[1];
        //        idParam[0] = new SqlParameter("@iUserId", userId);

        //        var corporateInfo = db.Database.ExecuteSqlCommand("uspCheckCorporateCustomer @iUserId", idParam);
        //        return true;
        //    }
        //}

        #endregion
    }
}
