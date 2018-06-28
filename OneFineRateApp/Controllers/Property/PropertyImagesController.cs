using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using OneFineRateApp.ViewModels;
using OneFineRateAppUtil;
using OneFineRateBLL;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OneFineRateApp.Controllers.Property
{
    [Authorize]
    public class PropertyImagesController : BaseController
    {

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(IEnumerable<HttpPostedFileBase> photos)
        {
            try
            {
                bool isSavedSuccessfully = false;
                if (photos != null)
                {
                    if (Session["PropId"] != null)
                    {
                        var propertyId = Convert.ToInt32(Session["PropId"].ToString());
                       
                        IList<etblPropetyPhotoMap> etblPropetyImageMList = new List<etblPropetyPhotoMap>();

                        string s_propertyId = propertyId.ToString();

                        //if(propertyId < 10)
                        //{
                        //    s_propertyId = "00" + propertyId.ToString();
                        //}
                        //else if(propertyId < 100)
                        //{
                        //    s_propertyId = "0" + propertyId.ToString();
                        //}

                        foreach (var uploadedFile in photos)
                        {
                            if (IsValideImage(uploadedFile))
                            {
                                using (MemoryStream target = new MemoryStream())
                                {
                                    uploadedFile.InputStream.CopyTo(target);
                                    byte[] data = target.ToArray();

                                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadedFile.FileName);

                                    var currentUploadedFileUrl = clsUtils.fnUploadFileINBlobStorage(s_propertyId, uniqueFileName, data, true);

                                    System.Drawing.Image image = System.Drawing.Image.FromStream(new System.IO.MemoryStream(data));

                                    var etblPropertyPhotoMap = new etblPropetyPhotoMap();
                                    etblPropertyPhotoMap.iPropId = propertyId;
                                    etblPropertyPhotoMap.bIsMapped = false;
                                    etblPropertyPhotoMap.sImgUrl = propertyId.ToString() + "/" + uniqueFileName;
                                    etblPropertyPhotoMap.UniqueAzureFileName = uniqueFileName;
                                    etblPropertyPhotoMap.dtActionDate = DateTime.Now;
                                    etblPropertyPhotoMap.iActionBy = Convert.ToInt16(((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId);

                                    if (image.Height >= 1080 && image.Width >= 1360)
                                    {
                                        etblPropertyPhotoMap.bIsHighRes = true;
                                    }
                                    else
                                    {
                                        etblPropertyPhotoMap.bIsHighRes = false;
                                    }
                                    etblPropertyPhotoMap.iResolutionH = (short)image.Height;
                                    etblPropertyPhotoMap.iResolutionW = (short)image.Width;

                                    etblPropetyImageMList.Add(etblPropertyPhotoMap);
                                }
                            }
                            else
                            {
                                TempData["ERROR"] = "Invalid Images ! Please upload only .jpg, .png, .jpeg or .bmp images";
                                return View();
                            }
                        }
                        BL_tblPropertyPhotoMap.AddMultipleRecord(etblPropetyImageMList.ToArray());
                        isSavedSuccessfully = true;
                    }
                    if (isSavedSuccessfully)
                    {
                        TempData["msg"] = "Photos uploaded successfully !";
                        return Json(new { Message = "Files uploaded successfully !" });
                    }
                    else
                    {
                        
                        return Json(new { status = "error", Message = "Error in saving file" });
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["ERROR"] = ex.Message;
                return Json(new { Message = ex.Message });
            }
        }


        [HttpPost]
        public ActionResult MapPropetyImages(IEnumerable<etblPropetyPhotoMap> unMappedEtblPropetyPhotoList)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    if (unMappedEtblPropetyPhotoList != null && unMappedEtblPropetyPhotoList.Count() > 0)
                    {
                        //var roomTypeList = unMappedEtblPropetyPhotoList.Where(x => x.iPhotoCatId == 1).FirstOrDefault().bIsPrimaryH = true;

                        //foreach (var subCatId in unMappedEtblPropetyPhotoList.Select(x => x.iPhotoSubCatId).ToList())
                        //{
                        //    unMappedEtblPropetyPhotoList.Where(x => x.iPhotoSubCatId == subCatId).FirstOrDefault().bIsPrimaryR = true;
                        //}

                        var userId = Convert.ToInt32(((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId);

                        foreach (var item in unMappedEtblPropetyPhotoList)
                        {
                            if (item.iPhotoCatId.GetValueOrDefault() != 0 && item.iPhotoSubCatId.GetValueOrDefault() != 0)
                            {
                                item.bIsMapped = true;
                            }
                            else
                            {
                                item.bIsMapped = false;
                            }
                            item.iActionBy = userId;
                            item.dtActionDate = DateTime.Now;
                        }

                        int result = BL_tblPropertyPhotoMap.UpdateMultipleRecord(unMappedEtblPropetyPhotoList.ToArray());

                        if (result == 1)
                        {
                            if (Session["PropId"] != null)
                            {
                                var propertyId = Convert.ToInt32(Session["PropId"].ToString());
                                var defaultImageResult = BL_tblPropertyPhotoMap.SetDefaultImages(propertyId);
                            }
                        }

                        TempData["msg"] = "Photo Mapped successfully !";
                    }
                    else
                    {
                        TempData["msg"] = "An unknown error had happend, Please try again.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ERROR"] = ex.Message;
                    throw;
                }
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult UnmappedPropertyImages()
        {
            try
            {
                var propertyId = Convert.ToInt32(Session["PropId"].ToString());

                ViewBag.categoryList = BL_tblPhotoCategoryM.GetAllRecords();

                var recordList = BL_tblPropertyPhotoMap.GetUnMappedProperty(propertyId);

                PropertyImageMapViewModel viewModel = new PropertyImageMapViewModel();
                viewModel.UnMappedEtblPropetyPhotoList = recordList;

                return PartialView(viewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult MappedPropertyImages()
        {
            try
            {
                var propertyId = Convert.ToInt32(Session["PropId"].ToString());

                ViewBag.categoryList = BL_tblPhotoCategoryM.GetAllRecords();

                var mappedPropetyPhotoList = BL_tblPropertyPhotoMap.GetMappedProperty(propertyId);
                var mappedRoomPhotoList = BL_tblPropertyPhotoMap.GetMappedRooms(propertyId);

                PropertyImageMapViewModel viewModel = new PropertyImageMapViewModel();
                viewModel.MappedEtblPropetyPhotoList = mappedPropetyPhotoList;
                viewModel.MappedRoomTypePhotoList = mappedRoomPhotoList;

                return PartialView(viewModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public ActionResult UpdatePropertyPhotoMap(etblPropetyPhotoMap etblPropetyPhotoMap, long? roomId)
        {
            try
            {
                bool status = false;
                string msg = string.Empty;
                int result = -1;

                var userId = Convert.ToInt32(((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId);
                etblPropetyPhotoMap.iActionBy = userId;

                if (roomId.HasValue)
                {
                    result = BL_tblPropertyPhotoMap.UpdateMappedRooms(etblPropetyPhotoMap);
                }
                else
                {
                    result = BL_tblPropertyPhotoMap.UpdateMappedProeprty(etblPropetyPhotoMap);
                }

                if (result == 1)
                {
                    TempData["msg"] = "Photo Mapping updated successfully!";
                    msg = "Photo Mapping updated successfully!";
                    status = true;
                }
                else
                {
                    TempData["ERROR"] = "Sorry! Something went wrong, Please try refreshing the page.";
                    msg = "Sorry! Something went wrong, Please try refreshing the page.";
                }
                var jsonResult = Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
                return jsonResult;
            }
            catch (Exception e)
            {
                var jsonResult = Json(new { status = false, msg = e.Message }, JsonRequestBehavior.AllowGet);
                return jsonResult;
            }
        }


       // [HttpPost]
        public ActionResult DeletePropertyPhotoMap(int propertyPhotoMapId)
        {
            try
            {
                // bool status = false;
                // string msg = string.Empty;
                var userId = Convert.ToInt32(((OneFineRateBLL.BL_Login.UserDetails)Session["UserDetails"]).iUserId);

                if (BL_tblPropertyPhotoMap.DeleteRecordById(propertyPhotoMapId, userId) == 1)
                {
                    TempData["msg"] = "Photo Deleted successfully!";
                    // status = true;
                    // msg = "Photo Deleted successfully!";
                }
                else
                {
                    TempData["ERROR"] = "Something went wrong, Please try refreshing the page.";
                    //msg = "Something went wrong ! Please try the refreshing page.";
                }

                //return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                TempData["ERROR"] = e.Message;
                // var jsonResult = Json(new { status = false, msg = e.Message }, JsonRequestBehavior.AllowGet);
                //return jsonResult;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetSubCategoryList(int categoryId)
        {
            try
            {
                List<etblPhotoSubCategoryM> subCategoryList = null;
                if (Session["PropId"] != null)
                {
                    var properyId = Convert.ToInt32(Session["PropId"].ToString());
                    subCategoryList = BL_tblPhotoCategoryM.GetSubcategoriesFromCategoryId(categoryId, properyId);
                }
                var jsonResult = Json(subCategoryList, JsonRequestBehavior.AllowGet);
                return jsonResult;
            }
            catch (Exception)
            {
                var jsonResult = Json(null, JsonRequestBehavior.AllowGet);
                return jsonResult;
            }
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