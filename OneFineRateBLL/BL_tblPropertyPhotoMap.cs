using OneFineRate;
using OneFineRateAppUtil;
using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_tblPropertyPhotoMap
    {
        public static int AddRecord(etblPropetyPhotoMap eobj)
        {
            int statusCode = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblPropertyPhotoMap dbuser = (OneFineRate.tblPropertyPhotoMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyPhotoMap());
                    db.tblPropertyPhotoMaps.Add(dbuser);
                    db.SaveChanges();
                    statusCode = 1;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return statusCode;
        }

        public static int AddMultipleRecord(etblPropetyPhotoMap[] eobjs)
        {
            int statusCode = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    foreach (var item in eobjs)
                    {
                        item.iPhotoCatId = null;
                        item.iPhotoSubCatId = null;
                        OneFineRate.tblPropertyPhotoMap dbPropertyImg = (OneFineRate.tblPropertyPhotoMap)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new OneFineRate.tblPropertyPhotoMap());
                        db.tblPropertyPhotoMaps.Add(dbPropertyImg);
                    }

                    statusCode = db.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return statusCode;
        }

        public static int UpdateRecord(etblPropetyPhotoMap eobj)
        {
            int retval = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    OneFineRate.tblPropertyPhotoMap obj = (OneFineRate.tblPropertyPhotoMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new OneFineRate.tblPropertyPhotoMap());
                    db.tblPropertyPhotoMaps.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
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

        public static int UpdateMultipleRecord(etblPropetyPhotoMap[] eobjs)
        {
            int statusCode = 0;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    foreach (var item in eobjs)
                    {
                        OneFineRate.tblPropertyPhotoMap dbPropertyImg = (OneFineRate.tblPropertyPhotoMap)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new OneFineRate.tblPropertyPhotoMap());
                        db.tblPropertyPhotoMaps.Attach(dbPropertyImg);
                        db.Entry(dbPropertyImg).State = System.Data.Entity.EntityState.Modified;
                    }
                    db.SaveChanges();
                    statusCode = 1;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return statusCode;
        }

        public static etblPropetyPhotoMap GetRecordById(long photoMapId)
        {
            etblPropetyPhotoMap etblPropetyPhotoMap = null;
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                try
                {
                    var eobj = db.tblPropertyPhotoMaps.Where(x => x.iPhotoId == photoMapId).FirstOrDefault();

                    etblPropetyPhotoMap = (etblPropetyPhotoMap)OneFineRateAppUtil.clsUtils.ConvertToObject(eobj, new etblPropetyPhotoMap());

                }
                catch (Exception)
                {
                    throw;
                }
            }
            return etblPropetyPhotoMap;
        }

        public static int DeleteRecordById(int iPhotoId, int iActionBy)
        {
            try
            {
                using (OneFineRateEntities db = new OneFineRateEntities())
                {
                    var exiting = db.tblPropertyPhotoMaps.FirstOrDefault(x => x.iPhotoId == iPhotoId);

                    exiting.iActionBy = iActionBy;

                    if (exiting.tblPropertyPrimaryPhotoMaps != null)
                    {
                        foreach (var item in exiting.tblPropertyPrimaryPhotoMaps)
                        {
                            item.iActionBy = iActionBy;
                        }

                        db.tblPropertyPrimaryPhotoMaps.RemoveRange(exiting.tblPropertyPrimaryPhotoMaps);
                    }

                    db.tblPropertyPhotoMaps.Remove(exiting);
                    return db.SaveChanges();

                    //retval = DeleteRelatedAzureImage(tempExisting.iPropId.ToString(), tempExisting.UniqueAzureFileName);

                    //var thumbImageUniqueFileName = Path.GetFileNameWithoutExtension(tempExisting.UniqueAzureFileName) + "_thumb" + Path.GetExtension(tempExisting.UniqueAzureFileName);
                    //if (clsUtils.IfAzureFileExists(tempExisting.iPropId.ToString(), thumbImageUniqueFileName))
                    //{
                    //    DeleteRelatedAzureImage(tempExisting.iPropId.ToString(), thumbImageUniqueFileName);
                    //}

                    //tempExisting = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<etblPropetyPhotoMap> GetUnMappedProperty(int propertyId)
        {
            List<etblPropetyPhotoMap> etblPropetyImageMList = new List<etblPropetyPhotoMap>();
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                // TO DO
                var dbobjList = db.tblPropertyPhotoMaps.Where(x => x.iPropId == propertyId && x.bIsMapped == false).ToList();
                if (dbobjList != null)
                {
                    foreach (var item in dbobjList)
                    {
                        var eobj = (etblPropetyPhotoMap)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblPropetyPhotoMap());
                        etblPropetyImageMList.Add(eobj);
                    }
                }
            }
            return etblPropetyImageMList;
        }

        public static List<etblPropetyPhotoMap> GetMappedProperty(int propertyId)
        {
            var mappedPropetyPhotoList = new List<etblPropetyPhotoMap>();

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobjPropertyPhotoMapList = db.tblPropertyPhotoMaps.Where(x => x.iPropId == propertyId && x.bIsMapped == true).ToList();
                if (dbobjPropertyPhotoMapList != null)
                {
                    foreach (var item in dbobjPropertyPhotoMapList)
                    {
                        var eobj = (etblPropetyPhotoMap)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new etblPropetyPhotoMap());

                        eobj.ddliPhotoSubCatId = BL_tblPhotoCategoryM.GetSubcategoriesFromCategoryId(eobj.iPhotoCatId.HasValue ? eobj.iPhotoCatId.Value : 0, propertyId);

                        mappedPropetyPhotoList.Add(eobj);
                    }
                }

                //SqlParameter[] parameterValue = new SqlParameter[1];
                //parameterValue[0] = new SqlParameter("@iPropId", propertyId);

                //var data = db.Database.SqlQuery<DbDataRecord>("uspGetAllPropertyImagesWithHotelCount @iPropId", parameterValue);
            }
            return mappedPropetyPhotoList;
        }

        public static List<MappedRoomTypePhoto> GetMappedRooms(int propertyId)
        {
            var roomTypePhotoList = new List<MappedRoomTypePhoto>();

            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var dbobjPropertyRoomMapList = db.tblPropertyRoomMaps.Where(x => x.iPropId == propertyId && x.bActive).Select(x => new { x.iPropId, x.iRoomId, x.sRoomName }).ToList();
                if (dbobjPropertyRoomMapList != null)
                {
                    foreach (var item in dbobjPropertyRoomMapList)
                    {
                        var eobj = (MappedRoomTypePhoto)OneFineRateAppUtil.clsUtils.ConvertToObject(item, new MappedRoomTypePhoto());
                        roomTypePhotoList.Add(eobj);
                    }
                }

                //SqlParameter[] parameterValue = new SqlParameter[1];
                //parameterValue[0] = new SqlParameter("@iPropId", propertyId);

                //var data = db.Database.SqlQuery<DbDataRecord>("uspGetAllPropertyImagesWithHotelCount @iPropId", parameterValue);
            }
            return roomTypePhotoList;
        }

        public static int UpdateMappedProeprty(etblPropetyPhotoMap etblPropetyPhotoMap)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var idParam = new SqlParameter[6];
                idParam[0] = new SqlParameter("iPhotoId", etblPropetyPhotoMap.iPhotoId);
                idParam[1] = new SqlParameter("iPropId", etblPropetyPhotoMap.iPropId);
                idParam[2] = new SqlParameter("iPhotoCatId", etblPropetyPhotoMap.iPhotoCatId);
                idParam[3] = new SqlParameter("iPhotoSubCatId", etblPropetyPhotoMap.iPhotoSubCatId);
                idParam[4] = new SqlParameter("bIsPrimary", etblPropetyPhotoMap.bIsPrimaryH);
                idParam[5] = new SqlParameter("iActionBy", etblPropetyPhotoMap.iActionBy);

                var objTblRatePlan = db.Database.ExecuteSqlCommand("spSetPrimaryH_False @iPhotoId, @iPropId, @iPhotoCatId,@iPhotoSubCatId,@bIsPrimary,@iActionBy", idParam);
                return 1;
            }
        }

        public static int UpdateMappedRooms(etblPropetyPhotoMap etblPropetyPhotoMap)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var idParam = new SqlParameter[6];
                idParam[0] = new SqlParameter("iPhotoId", etblPropetyPhotoMap.iPhotoId);
                idParam[1] = new SqlParameter("iPropId", etblPropetyPhotoMap.iPropId);
                idParam[2] = new SqlParameter("iPhotoCatId", etblPropetyPhotoMap.iPhotoCatId);
                idParam[3] = new SqlParameter("iPhotoSubCatId", etblPropetyPhotoMap.iPhotoSubCatId);
                idParam[4] = new SqlParameter("bIsPrimary", etblPropetyPhotoMap.bIsPrimaryR);
                idParam[5] = new SqlParameter("iActionBy", etblPropetyPhotoMap.iActionBy);

                var objTblRatePlan = db.Database.ExecuteSqlCommand("spSetPrimaryR_False @iPhotoId, @iPropId, @iPhotoCatId,@iPhotoSubCatId,@bIsPrimary,@iActionBy", idParam);
                return 1;
            }
        }

        public static int SetDefaultImages(long propId)
        {
            using (OneFineRateEntities db = new OneFineRateEntities())
            {
                var idParam = new SqlParameter[1];
                idParam[0] = new SqlParameter("iPropId", propId);

                var objTblRatePlan = db.Database.ExecuteSqlCommand("uspSetDefaultPhotoByProp @iPropId", idParam);
                return objTblRatePlan;
            }
        }
    }
}