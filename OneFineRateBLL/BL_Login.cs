using FutureSoft.ClassFiles;
using FutureSoft.Util;
using OneFineRate;
using OneFineRateAppUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_Login
    {
        public class UserDetails
        {
            public int iUserId { get; set; }
            public string UserName { get; set; }
            public string FisrtName { get; set; }
            public string LastName { get; set; }
            public string cStatus { get; set; }
        }

        public static UserDetails ValidateUser(string UserId, string Password)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                byte[] b = new byte[1];
                string pass = SimpleHash.ComputeHash(Password, "SHA1", b);

                var dbobj = (from s in db.tblUserMs
                             select new
                             {
                                 s.iUserId,
                                 s.sFirstName,
                                 s.sLastName,
                                 s.sUserName,
                                 s.sPassword,
                             }).Where(u => u.sUserName == UserId && u.sPassword == pass).AsQueryable();
                var list = dbobj.ToList();
                UserDetails eobj = new UserDetails();
                if (list.Count != 0)
                {
                    eobj.iUserId = list[0].iUserId;
                    eobj.LastName = list[0].sLastName;
                    eobj.FisrtName = list[0].sFirstName;
                    eobj.UserName = list[0].sUserName;
                }

                return eobj;
            }
        }

        public static UserDetails ActiveUser(string UserId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = (from s in db.tblUserMs
                             select new
                             {
                                 s.iUserId,
                                 s.sFirstName,
                                 s.sLastName,
                                 s.sUserName,
                                 s.sPassword,
                                 s.cStatus,
                             }).Where(u => u.sUserName == UserId).AsQueryable();
                var list = dbobj.ToList();
                UserDetails eobj = new UserDetails();
                if (list.Count != 0)
                {
                    eobj.iUserId = list[0].iUserId;
                    eobj.LastName = list[0].sLastName;
                    eobj.FisrtName = list[0].sFirstName;
                    eobj.UserName = list[0].sUserName;
                    eobj.cStatus = list[0].cStatus;

                }

                return eobj;
            }
        }

        public static int UpdatePassword(int iUserId, string OldPassword, string NewPassword)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                byte[] b = new byte[1];
                string pass = SimpleHash.ComputeHash(OldPassword, "SHA1", b);

                var dbobj = (from s in db.tblUserMs
                             select new
                             {
                                 s.iUserId,
                                 s.sFirstName,
                                 s.sLastName,
                                 s.sUserName,
                                 s.sPassword,
                                 s.cStatus,
                                 s.dtActionDate,
                                 s.dtCreatedOn,
                                 s.iActionBy,
                                 s.sContact,
                                 s.sEmail
                             }).Where(u => u.iUserId == iUserId && u.sPassword == pass).AsQueryable();
                var list = dbobj.ToList();

                if (list.Count == 0)
                {
                    return 0;
                }
                else
                {
                    OneFineRate.tblUserM obj = (OneFineRate.tblUserM)OneFineRateAppUtil.clsUtils.ConvertToObject(list[0], new OneFineRate.tblUserM());

                    obj.sPassword = SimpleHash.ComputeHash(NewPassword, "SHA1", b);
                    obj.dtActionDate = DateTime.Now;
                    obj.iActionBy = iUserId;
                    db.tblUserMs.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                return 1;
            }
        }
        public static int UpdateNewPassword(int iUserId, string NewPassword)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                byte[] b = new byte[1];
                var dbobj = (from s in db.tblUserMs
                             select new
                             {
                                 s.iUserId,
                                 s.sFirstName,
                                 s.sLastName,
                                 s.sUserName,
                                 s.sPassword,
                                 s.cStatus,
                                 s.dtActionDate,
                                 s.dtCreatedOn,
                                 s.iActionBy,
                                 s.sContact,
                                 s.sEmail
                             }).Where(u => u.iUserId == iUserId).AsQueryable();
                var list = dbobj.ToList();

                if (list.Count == 0)
                {
                    return 0;
                }
                else
                {
                    OneFineRate.tblUserM obj = (OneFineRate.tblUserM)OneFineRateAppUtil.clsUtils.ConvertToObject(list[0], new OneFineRate.tblUserM());

                    obj.sPassword = SimpleHash.ComputeHash(NewPassword, "SHA1", b);
                    obj.dtActionDate = DateTime.Now;
                    obj.iActionBy = iUserId;
                    db.tblUserMs.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                return 1;
            }
        }
        public static string getDecryptedUserId(string UserId)
        {
            string mainkey = clsUtils.getmainkey();
            return Encription.AESEncryption.DecryptData(UserId, mainkey);
        }
        public static string ChangePasswordUrl(string emailID)
        {
            object result = null;
            string strReturn = string.Empty;
            try
            {

                string mainkey = clsUtils.getmainkey();
                string sUrl = "";

                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    var dbobj = (from s in db.tblUserMs
                                 select new
                                 {
                                     s.iUserId,
                                     s.sFirstName,
                                     s.sLastName,
                                     s.sEmail,
                                 }).Where(u => u.sEmail.ToLower() == emailID.ToLower()).FirstOrDefault();

                    //  emailID = "emailofadityakumar@gmail.com";

                    if (dbobj != null)
                    {
                        sUrl = clsUtils.ExtranetBaseUrl() + "Account/ChangePassword?id=" + System.Web.HttpUtility.UrlEncode(Encription.AESEncryption.EncryptData(dbobj.iUserId.ToString(), mainkey));
                        StringBuilder stringBuilder = new StringBuilder();

                        stringBuilder.Append("<font face=Verdana size=6 color=navy><pre>Dear " + dbobj.sFirstName + " " + dbobj.sLastName + ",\n");
                        stringBuilder.Append("\nClick <a href = " + sUrl + ">here</a> to reset your password for " + clsUtils.ApplicationName() + " application.");
                        stringBuilder.Append("\n\n\nIn case the above link doesn't work then please copy the url mentioned below and paste it on browser.");
                        stringBuilder.Append("\n\n" + sUrl);
                        stringBuilder.Append("\n\n\nThis is system generated email, Please do not reply.");
                        stringBuilder.Append("\n\nRegards");
                        stringBuilder.Append("\nAdministrator</pre></font>");
                        MailComponent.SendEmail(emailID, "", "", "Link to reset your password for " + clsUtils.ApplicationName() + "", stringBuilder.ToString(), null, null, true, null, null);
                        result = new { st = 1, msg = "Link to reset your password has been sent to your email." };

                    }
                    else
                    {
                        result = new { st = 0, msg = "Invalid Email ID." };
                    }
                }
            }
            catch (Exception ex)
            {
                result = new { st = 0, msg = "Kindly try after some time." };
            }

            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }
    }
}