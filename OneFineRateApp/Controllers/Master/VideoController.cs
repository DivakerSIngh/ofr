using OneFineRateAppUtil;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateApp.Controllers.Master
{
    [Authorize]
    public class VideoController : BaseController
    {
        // GET: Video
        [Route("Video")]
        public ActionResult Index()
        {
            return View("Index", BL_Video.GetSingleRecordById());
        }
        public string UpdateVideo(string sVideoUrl, int Id)
        {
            return OneFineRateBLL.BL_Video.UpdateRecord(sVideoUrl, Id);
            //generate url for change password and send as an email

        }

        public string GetVideoURL()
        {
            object result = null;
            string strReturn = string.Empty;
            string URL = OneFineRateBLL.BL_Video.GetVideoURL();
            if (!string.IsNullOrEmpty(URL))
            {
                URL = System.Configuration.ConfigurationManager.AppSettings["BlobUrl"].ToString() + URL.Substring(1);

                result = new { st = 1, msg = URL };
                strReturn = OneFineRateAppUtil.clsUtils.ConvertToJson(result);
            }
            return strReturn;
        }

        [HttpPost]
        public string UpdateFile(HttpPostedFileBase file)
        {
            
            foreach (string Uploadfile in Request.Files)
            {
                var _file = Request.Files[Uploadfile];
                using (MemoryStream target = new MemoryStream())
                {
                    _file.InputStream.CopyTo(target);
                    byte[] data = target.ToArray();

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(_file.FileName);

                    //var currentUploadedFileUrl = clsUtils.fnUploadFileINBlobStorage("currency", uniqueFileName, data, true);
                    using (MemoryStream msicon = new MemoryStream(data))
                    {
                        var currentUploadedFileUrl = clsUtils.GenerateThumbnails("currency", uniqueFileName, 360, Image.FromStream(msicon));
                        OneFineRateBLL.BL_Video.UpdateRecordImg(currentUploadedFileUrl.ToString().Substring(currentUploadedFileUrl.ToString().LastIndexOf("/") + 1));
                    }
                }
            }
            return "";
        }
    }
}