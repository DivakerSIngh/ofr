using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StaticDumpWebJob.Models;
using StaticDumpWebJob.DatabaseContext;
using StaticDumpWebJob.DataManagers;
using System.Transactions;
using System.Globalization;
using log4net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Specialized;
using System.Data.Entity.Validation;
using FutureSoft.Util;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace StaticDumpWebJob.DataManagers
{
    public class DumpDataManager
    {
        #region Initializer

        public DumpDataManager(string sErrorEmails)
        {
            this.sErrorEmails = sErrorEmails;
        }

        #endregion
      
        #region private variables

        string sErrorEmails = string.Empty;

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IList<HotelOverview> propertyOverViewList = new List<HotelOverview>();
        private IList<CityData> propertyCityList = new List<CityData>();
        private IList<POIData> propertyPoiDataList = new List<POIData>();
        private IList<HotelPolicy> propertyPolicyList = new List<HotelPolicy>();

        private IList<tblPropertyAreaDataTG> propertyAreaList = new List<tblPropertyAreaDataTG>();

        private IList<tblPropertyFacilityTG_Tmp> propertyFacilityList = new List<tblPropertyFacilityTG_Tmp>();
        private IList<tblPropertyImageUrlTG_Tmp> propertyImageURLsList = new List<tblPropertyImageUrlTG_Tmp>();
        private IList<tblPropertyTopAttrationTG_Tmp> propertyInAndAroundList = new List<tblPropertyTopAttrationTG_Tmp>();
        private IList<tblPropertyReviewTG_Tmp> propertyReviewList = new List<tblPropertyReviewTG_Tmp>();
        private IList<tblPropertyRoomDescriptionTG_Tmp> propertyRoomDescriptionList = new List<tblPropertyRoomDescriptionTG_Tmp>();
        private IList<tblPropertyRoomLevelAmenityTG_Tmp> propertyRoomLevelAmenityList = new List<tblPropertyRoomLevelAmenityTG_Tmp>();

        StringBuilder overllStatusBuilder = new StringBuilder();

        #endregion

        #region Public Methods

        public void SendMail(string body)
        {
            Task.Run(() => { MailComponent.SendEmail(this.sErrorEmails, "", "", "TravelGuru Sync Status", body, null, null, false, null, null); });
        }

        /// <summary>
        /// Start Download data and wait for completion.
        /// </summary>
        /// <param name="sourceUrl"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public void DownloadAndProcessData(string sourceUrl, string userName, string password)
        {
            using (WebClient webClientDownloader = new WebClient())
            {
                try
                {
                    // webClientDownloader_DownloadDataCompleted(null, null); // Used for testing to read data from Local
                    AutoResetEvent waiter = new AutoResetEvent(false);
                    webClientDownloader.Credentials = new NetworkCredential(userName, password);
                    // webClientDownloader.DownloadProgressChanged += webClientDownloader_DownloadProgressChanged;
                    webClientDownloader.DownloadDataCompleted += new DownloadDataCompletedEventHandler(webClientDownloader_DownloadDataCompleted);
                    webClientDownloader.DownloadDataAsync(new Uri(sourceUrl), waiter);
                    waiter.WaitOne();
                }
                catch (WebException ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Trigger on download progress change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void webClientDownloader_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //Console.Write("\r{0} ", "Downloading... " + e.ProgressPercentage + " %");
        }

        /// <summary>
        /// This method will called once the file downloading process will be completed !
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void webClientDownloader_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {

            #region Read data from Stream and assign to private static variables

            try
            {
                if (e == null && e.Result == null)
                {
                    SendMail("Downloaded data from travelGuru failed ! , Data null \n");
                    return;
                }

                overllStatusBuilder.Append(string.Format("Download completed at {0} \n\n", DateTime.Now));

                // using (FileStream data = new FileStream(@"E:\Text.zip", FileMode.Open))
                using (Stream data = new MemoryStream(e.Result))
                {
                    using (ZipArchive archive = new ZipArchive(data, ZipArchiveMode.Read))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            using (var stream = entry.Open())
                            {
                                using (var reader = new StreamReader(stream, Encoding.Default, true))
                                {
                                    switch (entry.FullName.ToLower())
                                    {
                                        case FileNameConstant.AreaData:
                                            {
                                                #region areaDataProcessing

                                                reader.ReadLine();
                                                string currentLine;
                                                while ((currentLine = reader.ReadLine()) != null)
                                                {
                                                    string[] currentDataLine = currentLine.Split(new char[] { '|' }, StringSplitOptions.None);
                                                    propertyAreaList.Add(new tblPropertyAreaDataTG { iAreaTGUniqueId = int.Parse(currentDataLine[0]), sAreaName = currentDataLine[1] });
                                                }

                                                var repository = new Repository<tblPropertyAreaDataTG>();
                                                repository.Truncate();
                                                repository.Insert(propertyAreaList);

                                                SendMail(string.Format("Data successfully inserted in 'tblPropertyAreaDataTG at {0} \n\n", DateTime.Now));
                                                overllStatusBuilder.Append(string.Format("Data successfully inserted in 'tblPropertyAreaDataTG at {0} \n\n", DateTime.Now));

                                                break;

                                                #endregion areaDataProcessing
                                            }

                                        case FileNameConstant.Facility:
                                            {
                                                #region facilityDataProcessing

                                                reader.ReadLine();
                                                string currentLine;

                                                while ((currentLine = reader.ReadLine()) != null)
                                                {
                                                    string[] currentDataLine = currentLine.Split(new char[] { '|' }, StringSplitOptions.None);
                                                    propertyFacilityList.Add(
                                                        new tblPropertyFacilityTG_Tmp
                                                        {
                                                            iVendorId = currentDataLine[0],
                                                            iAmenityId = int.Parse(currentDataLine[1]),
                                                            sAmenityType = currentDataLine[2],
                                                            sDescription = currentDataLine[3],
                                                            dtActionDate = DateTime.Now
                                                        });
                                                }

                                                var distinctPropertyFacilityList = propertyFacilityList.Distinct(new PropertyFacilityTG_TmpComprator());

                                                var repository = new Repository<tblPropertyFacilityTG_Tmp>();
                                                repository.Truncate();
                                                repository.Insert(distinctPropertyFacilityList);
                                                SendMail(string.Format("Data successfully inserted in 'tblPropertyFacilityTG_Tmp at {0} \n\n", DateTime.Now));
                                                overllStatusBuilder.Append(string.Format("Data successfully inserted in 'tblPropertyFacilityTG_Tmp at {0} \n\n", DateTime.Now));

                                                break;

                                                #endregion
                                            }

                                        case FileNameConstant.ImageUrls:
                                            {
                                                #region imageurlsdataProcessing

                                                reader.ReadLine();
                                                string currentLine;

                                                while ((currentLine = reader.ReadLine()) != null)
                                                {
                                                    string[] currentDataLine = currentLine.Split(new char[] { '|' }, StringSplitOptions.None);
                                                    propertyImageURLsList.Add(
                                                        new tblPropertyImageUrlTG_Tmp
                                                        {
                                                            iVendorId = currentDataLine[0],
                                                            sImageUrl = currentDataLine[1],
                                                            sContentTitle = currentDataLine[2],
                                                            dtActionDate = DateTime.Now
                                                        });
                                                }

                                                var distinctPropertyImageURLsList = propertyImageURLsList.Distinct(new PropertyImageUrlTG_TmpComprator());


                                                var repository = new Repository<tblPropertyImageUrlTG_Tmp>();
                                                repository.Truncate();
                                                repository.Insert(distinctPropertyImageURLsList);
                                                SendMail(string.Format("Data successfully inserted in 'tblPropertyImageUrlTG_Tmp'.at {0}\n", DateTime.Now));
                                                overllStatusBuilder.Append(string.Format("Data successfully inserted in 'tblPropertyImageUrlTG_Tmp at {0} \n\n", DateTime.Now));

                                                break;

                                                #endregion imageurlsdataProcessing
                                            }
                                        case FileNameConstant.InAndAround:
                                            {
                                                #region inAndAroundDataProcessing

                                                reader.ReadLine();
                                                string currentLine;

                                                while ((currentLine = reader.ReadLine()) != null)
                                                {
                                                    string[] currentDataLine = currentLine.Split(new char[] { '|' }, StringSplitOptions.None);
                                                    if (!string.IsNullOrEmpty(currentDataLine[2]))
                                                    {
                                                        decimal? distanceInKm;
                                                        try
                                                        {
                                                            distanceInKm = decimal.Parse(currentDataLine[2]);
                                                        }
                                                        catch (Exception)
                                                        {
                                                            distanceInKm = null;
                                                        }

                                                        propertyInAndAroundList.Add(new tblPropertyTopAttrationTG_Tmp
                                                        {
                                                            iVendorId = currentDataLine[0],
                                                            sNameOfAttraction = currentDataLine[1],
                                                            iDistanceInKM = distanceInKm,
                                                            dtActionDate = DateTime.Now
                                                        });
                                                    }

                                                }

                                                var distinctPropertyInAndAroundList = propertyInAndAroundList.Distinct(new PropertyTopAttrationTG_TmpComprator());


                                                var repository = new Repository<tblPropertyTopAttrationTG_Tmp>();
                                                repository.Truncate();
                                                repository.Insert(distinctPropertyInAndAroundList);
                                                SendMail(string.Format("Data successfully inserted in 'tblPropertyTopAttrationTG_Tmp'.at {0}\n", DateTime.Now));
                                                overllStatusBuilder.Append(string.Format("Data successfully inserted in 'tblPropertyTopAttrationTG_Tmp at {0} \n\n", DateTime.Now));

                                                break;

                                                #endregion inAndAroundDataProcessing
                                            }

                                        case FileNameConstant.Reviews:
                                            {
                                                #region reviewDataProcessing

                                                reader.ReadLine();
                                                string currentLine;

                                                while ((currentLine = reader.ReadLine()) != null)
                                                {
                                                    string[] currentDataLine = currentLine.Split(new char[] { '|' }, StringSplitOptions.None);
                                                    propertyReviewList.Add(
                                                        new tblPropertyReviewTG_Tmp
                                                        {
                                                            iVendorId = currentDataLine[0],
                                                            sAvgGuestRating = currentDataLine[1],
                                                            iOverallRating = short.Parse(currentDataLine[2]),
                                                            sComments = currentDataLine[3],
                                                            iRoomQuality = short.Parse(currentDataLine[4]),
                                                            iServiceQuality = short.Parse(currentDataLine[5]),
                                                            iDiningQuality = short.Parse(currentDataLine[6]),
                                                            iCleanliness = short.Parse(currentDataLine[7]),
                                                            dtPost_Date = DateTime.Parse(currentDataLine[8]),
                                                            sConsumer_city = currentDataLine[9],
                                                            sConsumer_Country = currentDataLine[10],
                                                            sCustomer_name = currentDataLine[11],
                                                            dtActionDate = DateTime.Now
                                                        });
                                                }

                                                var distinctPropertyReviewList = propertyReviewList.Distinct(new PropertyReviewTG_TmpComprator());


                                                var repository = new Repository<tblPropertyReviewTG_Tmp>();
                                                repository.Truncate();
                                                repository.Insert(distinctPropertyReviewList);
                                                SendMail(string.Format("Data successfully inserted in 'tblPropertyReviewTG_Tmp'.at {0}\n", DateTime.Now));
                                                overllStatusBuilder.Append(string.Format("Data successfully inserted in 'tblPropertyReviewTG_Tmp at {0} \n\n", DateTime.Now));

                                                break;

                                                #endregion reviewDataProcessing
                                            }
                                        case FileNameConstant.RoomDescription:
                                            {
                                                #region roomDescriptionDataProcessing

                                                reader.ReadLine();
                                                string currentLine;

                                                while ((currentLine = reader.ReadLine()) != null)
                                                {
                                                    string[] currentDataLine = currentLine.Split(new char[] { '|' }, StringSplitOptions.None);
                                                    propertyRoomDescriptionList.Add(
                                                        new tblPropertyRoomDescriptionTG_Tmp
                                                        {
                                                            iVendorId = currentDataLine[0],
                                                            iRoomTypeId = currentDataLine[1],
                                                            sRoomType = currentDataLine[2],
                                                            sDescription = currentDataLine[3],
                                                            iMax_Adult_Occupancy = short.Parse(currentDataLine[4]),
                                                            iMax_Child_Occupancy = short.Parse(currentDataLine[5]),
                                                            iMax_Infant_Occupancy = short.Parse(currentDataLine[6]),
                                                            iMax_Guest_Occupancy = short.Parse(currentDataLine[7]),
                                                            sRoomType_ImagePath = currentDataLine[8],
                                                            dtActionDate = DateTime.Now

                                                        });
                                                }

                                                var distinctPropertyRoomDescriptionList = propertyRoomDescriptionList.Distinct(new PropertyRoomDescriptionTG_TmpComprator());


                                                var repository = new Repository<tblPropertyRoomDescriptionTG_Tmp>();
                                                repository.Truncate();
                                                repository.Insert(distinctPropertyRoomDescriptionList);
                                                SendMail(string.Format("Data successfully inserted in 'tblPropertyRoomDescriptionTG_Tmp'.at {0}\n", DateTime.Now));
                                                overllStatusBuilder.Append(string.Format("Data successfully inserted in 'tblPropertyRoomDescriptionTG_Tmp at {0} \n\n", DateTime.Now));

                                                break;

                                                #endregion roomDescriptionDataProcessing
                                            }
                                        case FileNameConstant.RoomAmenities:
                                            {
                                                #region roomLevelAmenitiesDataProcessing

                                                reader.ReadLine();
                                                string currentLine;

                                                while ((currentLine = reader.ReadLine()) != null)
                                                {
                                                    string[] currentDataLine = currentLine.Split(new char[] { '|' }, StringSplitOptions.None);
                                                    propertyRoomLevelAmenityList.Add(
                                                        new tblPropertyRoomLevelAmenityTG_Tmp
                                                        {
                                                            iVendorId = currentDataLine[0],
                                                            iRoomTypeId = currentDataLine[1],
                                                            sRoomType = currentDataLine[2],
                                                            iAmenityId = int.Parse(currentDataLine[3]),
                                                            sDescription = currentDataLine[4],
                                                            dtActionDate = DateTime.Now
                                                        });
                                                }

                                                var distinctPropertyRoomLevelAmenityList = propertyRoomLevelAmenityList.Distinct(new PropertyRoomLevelAmenityTG_TmpComprator());


                                                var repository = new Repository<tblPropertyRoomLevelAmenityTG_Tmp>();
                                                repository.Truncate();
                                                repository.Insert(distinctPropertyRoomLevelAmenityList);
                                                SendMail(string.Format("Data successfully inserted in 'tblPropertyRoomLevelAmenityTG_Tmp at {0} \n\n", DateTime.Now));
                                                overllStatusBuilder.Append(string.Format("Data successfully inserted in 'tblPropertyRoomLevelAmenityTG_Tmp at {0} \n\n", DateTime.Now));

                                                break;

                                                #endregion roomLevelAmenitiesDataProcessing
                                            }

                                        case FileNameConstant.HotelOverview:
                                            {
                                                #region hotelOverviewDataProcessing

                                                reader.ReadLine();
                                                string currentLine;

                                                while ((currentLine = reader.ReadLine()) != null)
                                                {
                                                    string[] currentDataLine = currentLine.Split(new char[] { '|' }, StringSplitOptions.None);

                                                    var hotelOverview = new HotelOverview();

                                                    hotelOverview.VendorId = currentDataLine[0];
                                                    hotelOverview.VendorName = currentDataLine[1];
                                                    hotelOverview.HotelClass = currentDataLine[2];
                                                    hotelOverview.Location = currentDataLine[3];
                                                    hotelOverview.City = currentDataLine[4];
                                                    hotelOverview.Country = currentDataLine[5];
                                                    hotelOverview.Address1 = currentDataLine[6];
                                                    hotelOverview.Address2 = currentDataLine[7];
                                                    hotelOverview.Area = currentDataLine[8];
                                                    hotelOverview.Address = currentDataLine[9];
                                                    hotelOverview.HotelOverviewDescription = currentDataLine[10];
                                                    hotelOverview.ReviewRating = currentDataLine[11];
                                                    hotelOverview.ReviewCount = currentDataLine[12];
                                                    hotelOverview.Latitude = currentDataLine[13].TrimEnd('0');
                                                    hotelOverview.Longitude = currentDataLine[14].TrimEnd('0');
                                                    hotelOverview.DefaultCheckInTime = currentDataLine[15];
                                                    hotelOverview.DefaultCheckOutTime = currentDataLine[16];
                                                    hotelOverview.Hotel_Star = currentDataLine[17];
                                                    hotelOverview.HotelGroupID = currentDataLine[18];
                                                    hotelOverview.HotelGroupName = currentDataLine[19];
                                                    hotelOverview.ImagePath = currentDataLine[20];
                                                    hotelOverview.HotelSearchKey = currentDataLine[21];
                                                    hotelOverview.Area_Seo_Id = currentDataLine[22];
                                                    hotelOverview.SecondaryAreaIds = currentDataLine[23];
                                                    hotelOverview.SecondaryAreaName = currentDataLine[24];
                                                    hotelOverview.NoOfFloors = currentDataLine[25];
                                                    hotelOverview.NoOfRooms = currentDataLine[26];
                                                    hotelOverview.State = currentDataLine[27];
                                                    hotelOverview.PinCode = currentDataLine[28];
                                                    hotelOverview.ThemeList = currentDataLine[29];
                                                    hotelOverview.CategoryList = currentDataLine[30];
                                                    hotelOverview.City_Zone = currentDataLine[31];
                                                    hotelOverview.WeekDay_Rank = currentDataLine[32];
                                                    hotelOverview.WeekEnd_Rank = currentDataLine[33];

                                                    propertyOverViewList.Add(hotelOverview);
                                                }

                                                try
                                                {

                                                    var distincePropertyOverviewList = propertyOverViewList.Distinct(new HotelOverViewDistinctVendorId()).ToList();

                                                    var chainList = propertyOverViewList.Distinct(new HotelOverViewComparator()).Select(x => new tblChainMTG_Tmp { iChainID = x.HotelGroupID, sChainName = x.HotelGroupName, cStatus = "A", dtActionDate = DateTime.Now });

                                                    var chainRepository = new Repository<tblChainMTG_Tmp>();
                                                    chainRepository.Truncate();
                                                    chainRepository.Insert(chainList);

                                                    var existingChainList = chainRepository.GetAll().ToList();

                                                    var tblPropertyMList = new List<tblPropertyMTG_Tmp>();

                                                    foreach (var hotelOverViewData in distincePropertyOverviewList)
                                                    {
                                                        tblPropertyMTG_Tmp propertyM = new tblPropertyMTG_Tmp();

                                                        propertyM.iVendorId = hotelOverViewData.VendorId;

                                                        short iRooms = 0;
                                                        short iFloors = 0;
                                                        short iStarCategoryId = 0;

                                                        short.TryParse(hotelOverViewData.NoOfRooms, out iRooms);
                                                        short.TryParse(hotelOverViewData.NoOfFloors, out iFloors);
                                                        short.TryParse(hotelOverViewData.Hotel_Star, out iStarCategoryId);

                                                        propertyM.iFloors = iFloors;
                                                        propertyM.iRooms = iRooms;
                                                        propertyM.dLatitude = string.IsNullOrEmpty(hotelOverViewData.Latitude) ? (decimal?)null : Convert.ToDecimal(hotelOverViewData.Latitude, CultureInfo.InvariantCulture);
                                                        propertyM.dLongitude = string.IsNullOrEmpty(hotelOverViewData.Longitude) ? (decimal?)null : Convert.ToDecimal(hotelOverViewData.Longitude, CultureInfo.InvariantCulture);

                                                        int pinCode = 0;

                                                        int.TryParse(hotelOverViewData.PinCode.Replace(" ", "").Replace("ý", "").Replace("�", ""), out pinCode); // .Replace("�", "")

                                                        propertyM.iPinCode = pinCode;

                                                        if (!string.IsNullOrEmpty(hotelOverViewData.Address1))
                                                        {
                                                            propertyM.sAddress = hotelOverViewData.Address1;
                                                        }
                                                        else if (!string.IsNullOrEmpty(hotelOverViewData.Address2))
                                                        {
                                                            propertyM.sAddress = hotelOverViewData.Address2;
                                                        }
                                                        else
                                                        {
                                                            propertyM.sAddress = hotelOverViewData.Address;
                                                        }

                                                        propertyM.sSecondaryAreaNameTG = hotelOverViewData.SecondaryAreaName;
                                                        propertyM.sDescription = hotelOverViewData.HotelOverviewDescription;
                                                        propertyM.sHotelName = hotelOverViewData.VendorName;
                                                        propertyM.iStarCategoryId = iStarCategoryId;
                                                        propertyM.sImageUrlTG = hotelOverViewData.ImagePath;
                                                        propertyM.bIsTG = true;

                                                        //Since the data already maintained in table relationship with tblTopAttractions
                                                        //var areasByCommaSeperated = propertyInAndAroundList.Where(x => x.iVendorId == hotelOverViewData.VendorId);
                                                        //propertyM.sTopAttractions = String.Join(",", areasByCommaSeperated.Select(x => x.sNameOfAttraction + "-" + x.iDistanceInKM));


                                                        var existingChain = existingChainList.Find(x => x.iChainID.Trim().ToLower().Equals(hotelOverViewData.HotelGroupID.Trim().ToLower()));
                                                        if (existingChain != null)
                                                        {
                                                            // This needs to be check...
                                                            propertyM.iChainId = existingChain.iChainID;
                                                        }
                                                        else
                                                        {
                                                            var newChain = new tblChainMTG_Tmp { iChainID = hotelOverViewData.HotelGroupID, cStatus = "A", dtActionDate = DateTime.Now, sChainName = hotelOverViewData.HotelGroupName };
                                                            chainRepository.Insert(newChain);
                                                            propertyM.iChainId = newChain.iChainID;
                                                        }

                                                        propertyM.sCountry = hotelOverViewData.Country;
                                                        propertyM.sCity = hotelOverViewData.City;
                                                        propertyM.sArea = hotelOverViewData.Area;
                                                        propertyM.sState = hotelOverViewData.State;
                                                        propertyM.sLocality = hotelOverViewData.Location;
                                                        propertyM.dtActionDate = DateTime.Now;

                                                        tblPropertyMList.Add(propertyM);
                                                    }


                                                    var tblPropertyMRepository = new Repository<tblPropertyMTG_Tmp>();
                                                    tblPropertyMRepository.Truncate();
                                                    tblPropertyMRepository.Insert(tblPropertyMList);
                                                    SendMail(string.Format("Data successfully inserted in 'tblPropertyMTG_Tmp at {0} \n\n", DateTime.Now));
                                                    overllStatusBuilder.Append(string.Format("Data successfully inserted in 'tblPropertyMTG_Tmp at {0} \n\n", DateTime.Now));

                                                }
                                                catch (InvalidOperationException ex)
                                                {
                                                    SendMail(string.Format("InvalidOperationException occured during tblPropertyMTG_Tmp data processing  at {0} ,Details : {1} \n\n", DateTime.Now, ex.ToString()));
                                                    overllStatusBuilder.Append(string.Format("nvalidOperationException occured during tblPropertyMTG_Tmp data processing  at {0}, Details : {1} \n\n", DateTime.Now, ex.ToString()));
                                                }

                                                break;

                                                #endregion hotelOverviewDataProcessing
                                            }

                                        default: break;
                                    }
                                }
                            }
                        }

                        RunStoreProcedure(overllStatusBuilder);
                    }
                }
            }
            catch (DbEntityValidationException entityValidationEx)
            {
                var exceptionStringBuilder = new StringBuilder();
                foreach (var eve in entityValidationEx.EntityValidationErrors)
                {
                    exceptionStringBuilder.Append(string.Format("\n\nEntity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        exceptionStringBuilder.Append(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage));
                    }
                }

                SendMail(exceptionStringBuilder.ToString());

                throw entityValidationEx;
            }
            catch (Exception ex)
            {
                SendMail(overllStatusBuilder.ToString());

                throw ex;
            }

            #endregion Read data from Stream and assign to private static variables

        }

        /// <summary>
        /// This will execute the Store Procedure after complete all the file process and record update to database, So that update the required table from inseted data.
        /// </summary>
        /// <param name="overAllStringBuilderInfo">Carry the Error Log from Previous Call Stack</param>
        public void RunStoreProcedure(StringBuilder overAllStringBuilderInfo)
        {
            try
            {
                var tblPropertyMRepository = new Repository<tblPropertyMTG_Tmp>();
                overAllStringBuilderInfo.Append(string.Format("uspTransferTGDataToOneFineRate calling started ! at {0} \n\n", DateTime.Now));
                SendMail(overAllStringBuilderInfo.ToString());
                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.ConnectionStrings["OFR_DataContext"].ToString(), CommandType.StoredProcedure, "uspTransferTGDataToOneFineRate");

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows[0]["status"].ToString() == "1")
                {
                    overAllStringBuilderInfo.Append(string.Format("Transfer Dump data Store proc executed Successfully ! at {0} \n\n", DateTime.Now));
                }
                else
                {
                    overAllStringBuilderInfo.Append(string.Format("An error occurred duing Transfer Dump data Store proc execution ! {0} \n\n", ds.Tables[0].Rows[0]["error"].ToString()));
                }

                SendMail(overAllStringBuilderInfo.ToString());
                Console.WriteLine("Done");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }

    #region Helper Compararer

    public class HotelOverViewComparator : IEqualityComparer<HotelOverview>
    {
        public bool Equals(HotelOverview x, HotelOverview y)
        {
            return x.HotelGroupID == y.HotelGroupID;
        }

        public int GetHashCode(HotelOverview obj)
        {
            return obj.HotelGroupID.GetHashCode();
        }
    }

    public class HotelOverViewDistinctVendorId : IEqualityComparer<HotelOverview>
    {
        public bool Equals(HotelOverview x, HotelOverview y)
        {
            return x.VendorId == y.VendorId;
        }

        public int GetHashCode(HotelOverview obj)
        {
            return obj.VendorId.GetHashCode();
        }
    }

    public class PropertyFacilityTG_TmpComprator : IEqualityComparer<tblPropertyFacilityTG_Tmp>
    {
        public bool Equals(tblPropertyFacilityTG_Tmp x, tblPropertyFacilityTG_Tmp y)
        {
            return x.iVendorId == y.iVendorId;
        }

        public int GetHashCode(tblPropertyFacilityTG_Tmp obj)
        {
            return obj.iVendorId.GetHashCode();
        }
    }

    public class PropertyImageUrlTG_TmpComprator : IEqualityComparer<tblPropertyImageUrlTG_Tmp>
    {
        public bool Equals(tblPropertyImageUrlTG_Tmp x, tblPropertyImageUrlTG_Tmp y)
        {
            return x.iVendorId == y.iVendorId;
        }

        public int GetHashCode(tblPropertyImageUrlTG_Tmp obj)
        {
            return obj.iVendorId.GetHashCode();
        }
    }

    public class PropertyTopAttrationTG_TmpComprator : IEqualityComparer<tblPropertyTopAttrationTG_Tmp>
    {
        public bool Equals(tblPropertyTopAttrationTG_Tmp x, tblPropertyTopAttrationTG_Tmp y)
        {
            return x.iVendorId == y.iVendorId;
        }

        public int GetHashCode(tblPropertyTopAttrationTG_Tmp obj)
        {
            return obj.iVendorId.GetHashCode();
        }
    }

    public class PropertyReviewTG_TmpComprator : IEqualityComparer<tblPropertyReviewTG_Tmp>
    {
        public bool Equals(tblPropertyReviewTG_Tmp x, tblPropertyReviewTG_Tmp y)
        {
            return x.iVendorId == y.iVendorId;
        }

        public int GetHashCode(tblPropertyReviewTG_Tmp obj)
        {
            return obj.iVendorId.GetHashCode();
        }
    }

    public class PropertyRoomDescriptionTG_TmpComprator : IEqualityComparer<tblPropertyRoomDescriptionTG_Tmp>
    {
        public bool Equals(tblPropertyRoomDescriptionTG_Tmp x, tblPropertyRoomDescriptionTG_Tmp y)
        {
            return x.iVendorId == y.iVendorId;
        }

        public int GetHashCode(tblPropertyRoomDescriptionTG_Tmp obj)
        {
            return obj.iVendorId.GetHashCode();
        }
    }

    public class PropertyRoomLevelAmenityTG_TmpComprator : IEqualityComparer<tblPropertyRoomLevelAmenityTG_Tmp>
    {
        public bool Equals(tblPropertyRoomLevelAmenityTG_Tmp x, tblPropertyRoomLevelAmenityTG_Tmp y)
        {
            return x.iVendorId == y.iVendorId;
        }

        public int GetHashCode(tblPropertyRoomLevelAmenityTG_Tmp obj)
        {
            return obj.iVendorId.GetHashCode();
        }
    } 

    #endregion

}