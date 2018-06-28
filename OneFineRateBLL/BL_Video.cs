using OneFineRate;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_Video
    {
        public static string UpdateRecord(string sVideoUrl, int Id)
        {
            int retval = 0;
            object result = null;
            string strReturn = string.Empty;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var eobj = db.tblVideoUrlMs.SingleOrDefault();
                    eobj.dtActionDate = DateTime.Now;
                    eobj.sVideoUrl = sVideoUrl;
                    eobj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)HttpContext.Current.Session["UserDetails"]).iUserId;
                    OneFineRate.tblVideoUrlM obj = eobj;// (OneFineRate.tblVideoUrlM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblVideoUrlM());
                    db.tblVideoUrlMs.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    retval = 1;

                    if (retval == 1)
                    {
                        result = new { st = 1, msg = "Updated successfully." };
                    }
                    else
                    {
                        result = new { st = 0, msg = "Kindly try after some time." };
                    }
                }
                catch (Exception)
                {
                    result = new { st = 0, msg = "Kindly try after some time." };
                }
            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }

        //Get Single Record
        public static eVideo GetSingleRecordById()
        {
            eVideo eobj = new eVideo();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblVideoUrlMs.SingleOrDefault();
                if (dbobj != null)
                    eobj = (eVideo)OneFineRateAppUtil.clsUtils.ConvertToObject(dbobj, eobj);
            }
            return eobj;

        }

        //Get Single Record
        public static string GetVideoURL()
        {
            string URL = "";
            eVideo eobj = new eVideo();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobj = db.tblVideoUrlMs.SingleOrDefault();
                if (dbobj != null)
                    URL = dbobj.sImgUrl;                    
            }
            return URL;
        }

        public static string UpdateRecordImg(string sImg)
        {
            int retval = 0;
            object result = null;
            string strReturn = string.Empty;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var eobj = db.tblVideoUrlMs.SingleOrDefault();
                    eobj.dtActionDate = DateTime.Now;
                    eobj.sImgUrl = "/currency/" + sImg;
                    eobj.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)HttpContext.Current.Session["UserDetails"]).iUserId;
                    OneFineRate.tblVideoUrlM obj = eobj;// (OneFineRate.tblVideoUrlM)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblVideoUrlM());
                    db.tblVideoUrlMs.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    retval = 1;

                    if (retval == 1)
                    {
                        result = new { st = 1, msg = "Updated successfully." };
                    }
                    else
                    {
                        result = new { st = 0, msg = "Kindly try after some time." };
                    }
                }
                catch (Exception)
                {
                    result = new { st = 0, msg = "Kindly try after some time." };
                }
            }
            strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            return strReturn;
        }
    }
}