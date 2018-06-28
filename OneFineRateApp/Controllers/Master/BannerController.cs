using OneFineRateBLL.Entities;
using OneFineRateBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using OneFineRateAppUtil;
using Newtonsoft.Json.Linq;

namespace OneFineRateApp.Controllers.Master
{
    [Authorize]
    public class BannerController : BaseController
    {
        // GET: Banner
        [Route("Banner")]
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult Save(etblBannerM prop)
        {
            var file = Request.Files;
            string strReturn = string.Empty;
            int val = 0;
            foreach (ModelState modelState in ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    string str;
                    str = "a";
                    // DoSomethingWith(error);
                }
            }
            prop.sDescription = prop.sDescription == null ? prop.sDescription : prop.sDescription.ToString().Replace("<p>", "").Replace("</p>", "");

            if (ModelState.IsValid)
            {
                if (prop.bannerImage != null)
                {
                    if (IsValideImage(prop.bannerImage))
                    {
                        using (MemoryStream target = new MemoryStream())
                        {
                            prop.bannerImage.InputStream.CopyTo(target);
                            byte[] data = target.ToArray();

                            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(prop.bannerImage.FileName);

                            var currentUploadedFileUrl = clsUtils.fnUploadFileINBlobStorage("banner", uniqueFileName, data, true);

                            System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(data));
                            //prop.bIsMapped = false;
                            prop.sImgUrl = "banner" + "/" + uniqueFileName;
                            prop.UniqueAzureFileName = uniqueFileName;
                            prop.iResolutionH = (short)image.Height;
                            prop.iResolutionW = (short)image.Width;
         
                        }
                        if (prop.Mode == "Add")
                        {
                            val = AddBanner(prop);
                        }

                        if (prop.Mode == "Edit")
                        {
                            val = EditBanner(prop);
                        }

                        if (val == 1)
                        {
                            if (prop.Mode == "Add")
                            {
                                TempData["msg"] = "Added Successfully";
                                // return Json(new { st = 0, msg = "Added Successfully" }, JsonRequestBehavior.AllowGet);
                            }

                            if (prop.Mode == "Edit")
                            {
                                TempData["msg"] = "Updated Successfully";
                                // return Json(new { st = 0, msg = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        if (val == 2)
                        {
                            if (prop.Mode == "Add")
                            {
                                TempData["ERROR"] = "Banner already exists with same Name";
                                //return Json(new { st = 0, msg = "Banner already exists with same Name" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        if (val == 3)
                        {
                            if (prop.Mode == "Add")
                            {
                                TempData["ERROR"] = "Banner for page bottom already exists";
                                //return Json(new { st = 0, msg = "Banner already exists with same Name" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        TempData["ERROR"] = "Incorrect Format.Only jpg png gif jpeg formats are allowed.";
                        //return Json(new { st = 0, msg = "Incorrect Format.Only jpg png gif jpeg formats are allowed." }, JsonRequestBehavior.AllowGet);

                    }
                }
                else
                {
                    if (prop.Mode == "Edit")
                    {
                        val = EditBanner(prop);
                        if (val == 1)
                        {
                            TempData["msg"] = "Updated Successfully";
                            // return Json(new { st = 0, msg = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                        }
                        if (val == 2 || val == 3)
                        {
                            TempData["ERROR"] = "Banner for page bottom already exists";
                            //return Json(new { st = 0, msg = "Banner already exists with same Name" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else if (prop.Mode == "Add")
                    {
                        val = AddBanner(prop);
                        if (val == 1)
                        {
                            TempData["msg"] = "Added Successfully";
                            // return Json(new { st = 0, msg = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                        }
                        else if (val == 2)
                        {
                            TempData["ERROR"] = "Banner already exists with same Name";
                            //return Json(new { st = 0, msg = "Banner already exists with same Name" }, JsonRequestBehavior.AllowGet);
                        }
                        else if (val == 3)
                        {
                            TempData["ERROR"] = "Banner for page bottom already exists";
                        }
                    }
                    
                }
            }
            return RedirectToAction("Index");
            //return Json(new { st = 1, msg = true }, JsonRequestBehavior.AllowGet);
        }
        public int AddBanner(etblBannerM prop)
        {
            try
            {
                if (prop.sDescription !=null)
                {
                    prop.sDescription = prop.sDescription.Replace("\t", "");
                }
                
                prop.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)(Session["UserDetails"])).iUserId;
                prop.dtActionDate = DateTime.Now;

                if (!string.IsNullOrEmpty(prop.sLinkController) && !string.IsNullOrEmpty(prop.sLinkAction))
                {
                    if (prop.sLinkController != "Select" && prop.sLinkAction != "Select")
                    {
                        prop.sLinkId = "../" + prop.sLinkController + "/" + prop.sLinkAction;
                    }
                }

                prop.cstatus = "A";
                int result = BL_tblBannerM.AddRecord(prop);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int EditBanner(etblBannerM prop)
        {
            try
            {
                prop.sDescription = prop.sDescription.Replace("\t", "");
                prop.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)(Session["UserDetails"])).iUserId;
                prop.dtActionDate = DateTime.Now;
                //if (prop.sLinkController != "Select" && prop.sLinkAction != "Select")
                //{
                //    prop.sLinkId = "../" + prop.sLinkController + "/" + prop.sLinkAction;
                //}

                int result = BL_tblBannerM.UpdateRecord(prop);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult Add()
        {
            etblBannerM obj = new etblBannerM();
            obj.Mode = "Add";
            obj.heading = "Add Banner";
            return PartialView("pvBanner", obj);
        }
        public ActionResult Edit(int Id)
        {
            etblBannerM obj = new etblBannerM();
            obj = BL_tblBannerM.GetBannerDetailsByID(Id);
            //if (obj.sLinkId != null)
            //{
            //    string[] arr = obj.sLinkId.Split('/');
            //    obj.sLinkController = arr[1].ToString();
            //    obj.sLinkAction = arr[2].ToString();
            //}


            obj.Mode = "Edit";
            obj.heading = "Edit Banner";
            return PartialView("pvBanner", obj);
        }
        public ActionResult Delete(int Id)
        {
            string strReturn = string.Empty;
            try
            {
                etblBannerM prop = new etblBannerM();
                prop = BL_tblBannerM.GetBannerDetailsByID(Id);
                if (prop.cstatus == "A") { prop.cstatus = "I"; }
                else if (prop.cstatus == "I") { prop.cstatus = "A"; }

                prop.iActionBy = ((OneFineRateBLL.BL_Login.UserDetails)(Session["UserDetails"])).iUserId;
                prop.dtActionDate = DateTime.Now;
                int val = BL_tblBannerM.DeleteRecord(prop);
                if (val == 1)
                {
                    if (prop.cstatus == "I")
                    {
                        TempData["msg"] = "Disabled successfully";
                       // return Json(new { st = 0, msg = "Disabled successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        TempData["msg"] = "Enabled successfully";
                       // return Json(new { st = 0, msg = "Enabled successfully" }, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                     TempData["msg"] = "Kindly try after some time";
                    //return Json(new { st = 0, msg = "Kindly try after some time" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {

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