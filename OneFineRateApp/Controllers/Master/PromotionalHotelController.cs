using OneFineRateAppUtil;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateApp.Controllers.Master
{
    public class PromotionalHotelController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var promotionalHotelList = BL_tblPromotionalHotel.GetAllRecords();
            ViewBag.PropertyList = BL_tblPromotionalHotel.GetPropertyListForDropdown();
            return View(promotionalHotelList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(eTblPromotionalHotel model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                int status = -1;
                if (file != null)
                {
                    if (IsValideImage(file))
                    {
                        using (MemoryStream target = new MemoryStream())
                        {
                            file.InputStream.CopyTo(target);
                            byte[] data = target.ToArray();

                            var uniqueFileName = model.sPosition + Path.GetExtension(file.FileName);

                            var currentUploadedFileUrl = clsUtils.Upload_Promotional_Hotel_Image_To_BlobStorage("promotionalhotels", uniqueFileName, data,165,245);

                            model.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                            model.dtActionDate = DateTime.Now;
                            model.sImageUrl = "promotionalhotels/" + uniqueFileName;
                            status = BL_tblPromotionalHotel.UpdateRecord(model);
                        }
                    }
                    else
                    {
                        TempData["ERROR"] = "Invalid Images ! Please upload only .jpg, .png, .jpeg or .bmp images";
                    }
                }
                else if(!string.IsNullOrEmpty(model.sImageUrl))
                {
                    model.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId;
                    model.dtActionDate = DateTime.Now;
                    status = BL_tblPromotionalHotel.UpdateRecord(model);
                }
                else
                {
                    TempData["ERROR"] = "Please select an Image to upload!";
                    return RedirectToAction("Index");
                }

                if (status == 1)
                {
                    TempData["msg"] = "Promotinal Image mapping updated successfully !";
                }
                else if(status == 0)
                {
                    TempData["ERROR"] = "An unkown error had happen ! Images not updated .";
                }

            }
            return RedirectToAction("Index");
        }

        private bool IsValideImage(HttpPostedFileBase file)
        {
            if (file.ContentType.Contains("image"))
            {
                return true;
            }

            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" };

            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }
    }
}