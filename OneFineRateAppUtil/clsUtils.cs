using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Web;
using System.Net.Mail;
//using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Net;
using System.Configuration;
using System.Globalization;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Encription;
using FutureSoft.Util;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.Azure;

namespace OneFineRateAppUtil
{
    public class clsUtils
    {
        public string PayuGeneratehash512(string text)
        {
            byte[] message = Encoding.UTF8.GetBytes(text);

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;

        }

        public static string ConvertNumberToCommaSeprated(decimal value)
        {

            NumberFormatInfo nfi = new CultureInfo("en-IN", false).NumberFormat;
            nfi.NumberDecimalDigits = 0;
            return value.ToString("N", nfi);

        }

        public static int GetVerificationCode()
        {
            return 123456;
            //return new Random().Next(100000, 999999);
        }

        public static string GetTripAdvisorLogoUrl()
        {
            //return "http://www.tripadvisor.com/img/cdsi/langs/en/tripadvisor_logo_transp_100x20-43524-1.png";
            return "https://ofrbook.azurewebsites.net/images/tripadvisor-coming.jpg";
        }

        public static string fnUploadFileINBlobStorage(string iPropId, string sFileName, byte[] data, bool generateThumbnail)
        {
            CloudStorageAccount accnt = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient client = accnt.CreateCloudBlobClient();
            BlobContainerPermissions containerPermissions = new BlobContainerPermissions();
            containerPermissions.PublicAccess = BlobContainerPublicAccessType.Blob;
            CloudBlobContainer Container = client.GetContainerReference(iPropId);
            Container.CreateIfNotExist();
            Container.SetPermissions(containerPermissions);

            CloudBlob blob = null;
            if (sFileName.IndexOf(".") > 0)
            {
                blob = Container.GetBlobReference(sFileName);
            }
            else
            {
                blob = Container.GetBlobReference(sFileName);
            }

            using (MemoryStream msicon = new MemoryStream(data))
            {
                blob.UploadFromStream(msicon);
                if (generateThumbnail)
                {
                    var blobResizedImgeUrl = GenerateThumbnails(iPropId, sFileName, 180, Image.FromStream(msicon));
                }
            }

            return blob.Uri.AbsoluteUri.ToString();
        }

        public static string fnUploadFileINBlobStorage(string iPropId, string sFileName, byte[] data, int customizedImageSize)
        {
            CloudStorageAccount accnt = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient client = accnt.CreateCloudBlobClient();
            BlobContainerPermissions containerPermissions = new BlobContainerPermissions();
            containerPermissions.PublicAccess = BlobContainerPublicAccessType.Blob;
            CloudBlobContainer Container = client.GetContainerReference(iPropId);
            Container.CreateIfNotExist();
            Container.SetPermissions(containerPermissions);

            CloudBlob blob = null;
            if (sFileName.IndexOf(".") > 0)
            {
                blob = Container.GetBlobReference(sFileName);
            }
            else
            {
                blob = Container.GetBlobReference(sFileName);
            }

            using (MemoryStream msicon = new MemoryStream(data))
            {
                var blobResizedImgeUrl = GenerateThumbnails(iPropId, sFileName, customizedImageSize, Image.FromStream(msicon));
            }

            return blob.Uri.AbsoluteUri.ToString();
        }

        public static string Upload_Promotional_Hotel_Image_To_BlobStorage(string iPropId, string sFileName, byte[] data, int height, int width)
        {
            CloudStorageAccount accnt = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient client = accnt.CreateCloudBlobClient();
            BlobContainerPermissions containerPermissions = new BlobContainerPermissions();
            containerPermissions.PublicAccess = BlobContainerPublicAccessType.Blob;
            CloudBlobContainer Container = client.GetContainerReference(iPropId);
            Container.CreateIfNotExist();
            Container.SetPermissions(containerPermissions);

            CloudBlob blob = null;
            if (sFileName.IndexOf(".") > 0)
            {
                blob = Container.GetBlobReference(sFileName);
            }
            else
            {
                blob = Container.GetBlobReference(sFileName);
            }

            using (MemoryStream msicon = new MemoryStream(data))
            {
                string origFileExtension = Path.GetExtension(sFileName);

                ImageFormat format = null;
                switch (origFileExtension.ToUpper())
                {
                    case ".GIF": format = ImageFormat.Gif; break;
                    case ".JPG":
                    case ".JPEG": format = ImageFormat.Jpeg; break;
                    default: format = ImageFormat.Png; break;
                }

                Size thumbnailSize = new Size(width, height);

                Bitmap resizedBmp = ResizeImage(Image.FromStream(msicon), thumbnailSize);


                using (MemoryStream memoryStream = new MemoryStream())
                {
                    memoryStream.Position = 0;
                    resizedBmp.Save(memoryStream, format);
                    memoryStream.Position = 0;
                    blob.UploadFromStream(memoryStream);
                }

                resizedBmp.Dispose();
            }

            return blob.Uri.AbsoluteUri.ToString();
        }

        public static bool IfAzureFileExists(string containerName, string uniqueFileName)
        {
            CloudStorageAccount accnt = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient client = accnt.CreateCloudBlobClient();
            BlobContainerPermissions containerPermissions = new BlobContainerPermissions();
            containerPermissions.PublicAccess = BlobContainerPublicAccessType.Blob;

            var blob = client.GetContainerReference(containerName).GetBlockBlobReference(uniqueFileName);

            {
                // TO DO
                try
                {
                    blob.FetchAttributes();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static int DeleteAzureUploadedFile(string containerName, string uniqueFileName)
        {
            CloudStorageAccount accnt = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient client = accnt.CreateCloudBlobClient();
            BlobContainerPermissions containerPermissions = new BlobContainerPermissions();
            containerPermissions.PublicAccess = BlobContainerPublicAccessType.Blob;

            CloudBlobContainer blobContainer = client.GetContainerReference(containerName);

            if (blobContainer != null)
            {
                CloudBlob cloudBlob = blobContainer.GetBlobReference(uniqueFileName);
                if (cloudBlob != null)
                {
                    bool isDeleted = cloudBlob.DeleteIfExists();
                    if (isDeleted)
                    {
                        return 1;
                    }
                }
            }
            return -1;
        }

        public static string ErrorMsg(string sType, int seq, string status = null)
        {
            switch (seq)
            {
                case 0: return sType + " already exists, please retry with different value.";
                case 1: return sType + " added successfully";
                case 2: return sType + " updated successfully";
                case 3: return sType + "Kindly try after some time.";
                case 4: return sType + (status == "I" ? " disabled successfully." : " enabled successfully.");
                case 5: return sType + " deleted successfully";
                case 6: return sType + " Please select Rights";
                case 7: return sType + " Please select atleast one hotel";
                case 8: return sType + " Stay to date should not be less than Booking to date.";
                case 9: return sType + " Please enter only alphanumeric";
                default:
                    return "Kindly try again after some time.";
            }

        }

        //public static string ConvertddmmyyyytommDDyyyy(string sDate)
        //{
        //    string[] namesArray = sDate.Split('/');

        //    return namesArray[1] + "/" + namesArray[0] + "/" + namesArray[2];
        //}

        public static string ConvertddmmyyyytommDDyyyy(string sDate)
        {
            string[] namesArray = sDate.Split('/');

            return namesArray[1] + "/" + namesArray[0] + "/" + namesArray[2];
        }

        public static DateTime ConvertddmmyyyytoDateTime(string sDate)
        {
            return DateTime.ParseExact(sDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);// DateTime.Parse(sDate, new CultureInfo("en-CA"));
        }

        public static DateTime ConvertmmddyyyytoDateTime(string sDate)
        {
            return DateTime.ParseExact(sDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);// DateTime.Parse(sDate, new CultureInfo("en-CA"));
        }

        public static DateTime ConvertyyyymmddtoDateTime(string sDate)
        {
            return DateTime.ParseExact(sDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);// DateTime.Parse(sDate, new CultureInfo("en-CA"));
        }

        public static object ConvertToObject(object original, object convertto)
        {
            try
            {
                if (convertto != null)
                {
                    Type _original = original.GetType();
                    Type _convertto = convertto.GetType();
                    PropertyInfo[] properties_original = _original.GetProperties();

                    foreach (PropertyInfo item in properties_original)
                    {
                        PropertyInfo properties_convertto = _convertto.GetProperty(item.Name);
                        if (properties_convertto != null)
                        {
                            object defaultValue = item.GetValue(original, null);
                            Type t = Nullable.GetUnderlyingType(item.PropertyType) ?? item.PropertyType;

                            object safeValue = (defaultValue == null) ? null : Convert.ChangeType(defaultValue, t);
                            properties_convertto.SetValue(convertto, safeValue, null);
                            // properties_convertto.SetValue(convertto, System.Convert.ChangeType(safeValue, item.PropertyType), null);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            return convertto;
        }

        public static string getmainkey()
        {
            return "futuresoft12345@";
        }

        public static string Decode(string Text)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                var base64EncodedBytes = System.Convert.FromBase64String(Text);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            else
                return "";
        }

        public static string Encode(string Text)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(Text);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string ConvertToJson(object obj)
        {

            string sJSON = "";
            System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer() { MaxJsonLength = 86753090 };
            sJSON = oSerializer.Serialize(obj);
            return sJSON;
        }

        public static Dictionary<string, string> JsonObjecttoDictionary(object json)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer() { MaxJsonLength = 86753090 };
            // serializer.MaxJsonLength = Int32.MaxValue;
            var jsons = serializer.Serialize(json);
            return serializer.Deserialize<Dictionary<string, string>>(jsons);
        }

        public static string ApplicationName()
        {
            return ConfigurationManager.AppSettings["ApplicationName"].ToString();
        }

        public static string ApplicationRootUrl()
        {
            return ConfigurationManager.AppSettings["ApplicationRootUrl"].ToString();
        }

        public static string ExtranetBaseUrl()
        {
            return ConfigurationManager.AppSettings["ExtranetBaseUrl"].ToString();
        }

        public static string OfrWebsiteBaseUrl()
        {
            return ConfigurationManager.AppSettings["OFRBaseUrl"].ToString();
        }

        public static string fnSizeName(int iSize)
        {
            string sName = "";
            switch (iSize)
            {
                case 300:
                    sName = "mdpi";
                    break;
                case 450:
                    sName = "hdpi";
                    break;
                case 600:
                    sName = "xhdpi";
                    break;
                case 180:
                    sName = "_thumb";
                    break;
                default:
                    sName = "";
                    break;
            }
            return sName;
        }

        public static Size GetThumbnailSize(Image original, int maxPixels)
        {
            if (maxPixels <= 0)
            {
                // maxPixels - Maximum size of any dimension.
                //const int maxPixels = 40;

                // Width and height.
                int originalWidth = original.Width / 2;
                int originalHeight = original.Height / 2;

                // Return thumbnail size.
                return new Size((int)(originalWidth), (int)(originalHeight));

            }
            else
            {
                // maxPixels - Maximum size of any dimension.
                //const int maxPixels = 40;

                // Width and height.
                int originalWidth = original.Width;
                int originalHeight = original.Height;

                // Compute best factor to scale entire image based on larger dimension.
                double factor;
                //if (originalWidth > originalHeight)
                //{
                //    factor = (double)maxPixels / originalWidth;
                //}
                //else
                //{
                //    factor = (double)maxPixels / originalHeight;
                //}

                factor = (double)maxPixels / originalHeight;

                // Return thumbnail size.
                return new Size((int)(originalWidth * factor), (int)(originalHeight * factor));
            }

        }

        public static string GenerateThumbnails(string iPropId, string OrigFile)
        {
            int iSize = 180;
            string sNewFileName = "";
            string sFileExtension = OrigFile.Substring(OrigFile.LastIndexOf("."));
            string[] sFileName = sNewFileName.Split('.');
            sNewFileName = sFileName[0] + fnSizeName(iSize);

            ImageFormat format = null;
            if (sFileName[1].ToUpper() == ".GIF")
            {
                format = ImageFormat.Gif;
            }
            else if (sFileName[1].ToUpper() == ".JPG" || sFileName[1].ToUpper() == ".JPEG")
            {
                format = ImageFormat.Jpeg;
            }
            else
            {
                format = ImageFormat.Png;
            }

            Image originalImage = null;
            originalImage = Image.FromFile(OrigFile);
            // Compute thumbnail size.

            Size thumbnailSize = GetThumbnailSize(originalImage, iSize);

            Bitmap originBmp = new Bitmap(OrigFile);
            int w = thumbnailSize.Width;//originBmp.Width / iSize;
            int h = thumbnailSize.Height; // originBmp.Height / iSize;
            Bitmap resizedBmp = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(resizedBmp);
            // g.Clear(Color.Transparent);

            // Set high quality interpolation method 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            // Set the high-quality, low-speed rendering the degree of smoothing 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            // Anti-aliasing 
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawImage(originBmp, new Rectangle(0, 0, w, h), new Rectangle(0, 0, originBmp.Width, originBmp.Height), GraphicsUnit.Pixel);

            // resizedBmp.Save(reSizePicPath, format);
            CloudStorageAccount accnt = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient client = accnt.CreateCloudBlobClient();
            BlobContainerPermissions containerPermissions = new BlobContainerPermissions();
            containerPermissions.PublicAccess = BlobContainerPublicAccessType.Blob;
            CloudBlobContainer Container = client.GetContainerReference(iPropId);
            Container.CreateIfNotExist();
            Container.SetPermissions(containerPermissions);

            CloudBlob blob = null;
            if (sNewFileName.IndexOf(".") > 0)
            { blob = Container.GetBlobReference(sNewFileName); }
            else { blob = Container.GetBlobReference(sNewFileName); }

            Stream msicon = null;
            resizedBmp.Save(msicon, format);
            blob.UploadFromStream(msicon);


            g.Dispose();
            resizedBmp.Dispose();
            originBmp.Dispose();
            return blob.Uri.AbsoluteUri.ToString();
        }

        public static string GenerateThumbnails(string iPropId, string OrigFile, int requiredSize, Image originalImage)
        {
            string origFileNameWithoutExt = Path.GetFileNameWithoutExtension(OrigFile);
            string origFileExtension = Path.GetExtension(OrigFile);

            string sNewFileName = origFileNameWithoutExt + fnSizeName(requiredSize) + origFileExtension;

            ImageFormat format = null;
            switch (origFileExtension.ToUpper())
            {
                case ".GIF": format = ImageFormat.Gif; break;
                case ".JPG":
                case ".JPEG": format = ImageFormat.Jpeg; break;
                default: format = ImageFormat.Png; break;
            }

            Size thumbnailSize = GetThumbnailSize(originalImage, requiredSize);

            Bitmap resizedBmp = ResizeImage(originalImage, thumbnailSize);

            CloudStorageAccount accnt = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient client = accnt.CreateCloudBlobClient();
            BlobContainerPermissions containerPermissions = new BlobContainerPermissions();
            containerPermissions.PublicAccess = BlobContainerPublicAccessType.Blob;
            CloudBlobContainer Container = client.GetContainerReference(iPropId);
            Container.CreateIfNotExist();
            Container.SetPermissions(containerPermissions);

            CloudBlob blob = null;

            if (sNewFileName.IndexOf(".") > 0)
            {
                blob = Container.GetBlobReference(sNewFileName);
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Position = 0;
                resizedBmp.Save(memoryStream, format);
                memoryStream.Position = 0;
                blob.UploadFromStream(memoryStream);
            }
            resizedBmp.Dispose();
            return blob.Uri.AbsoluteUri.ToString();
        }

        private static Bitmap ResizeImage(Image image, Size size)
        {
            var destRect = new Rectangle(0, 0, size.Width, size.Height);
            var destImage = new Bitmap(size.Width, size.Height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static string GeneratePassword(string secret, long iterationNumber, int digits = 6)
        {
            byte[] counter = BitConverter.GetBytes(iterationNumber);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(counter);

            byte[] key = Encoding.ASCII.GetBytes(secret);

            System.Security.Cryptography.HMACSHA1 hmac = new System.Security.Cryptography.HMACSHA1(key, true);

            byte[] hash = hmac.ComputeHash(counter);

            int offset = hash[hash.Length - 1] & 0xf;

            int binary =
                ((hash[offset] & 0x7f) << 24)
                | ((hash[offset + 1] & 0xff) << 16)
                | ((hash[offset + 2] & 0xff) << 8)
                | (hash[offset + 3] & 0xff);

            int password = binary % (int)Math.Pow(10, digits); // 6 digits

            return password.ToString(new string('0', digits));
        }

        public static string sendSMS(string sMobile, string sMessage)
        {
            //sMobile = "8882065435";
            string statusResult = string.Empty;
            string userid_sms = ConfigurationManager.AppSettings["UIDSMS"];
            string passwd_sms = ConfigurationManager.AppSettings["PWDSMS"];
            string url_sms = "http://enterprise.smsgupshup.com/GatewayAPI/rest?method=sendMessage&send_to=" + sMobile + "&v=1.1&msg=" + HttpUtility.UrlEncode(sMessage, Encoding.UTF8) + "&userid=" + userid_sms + "&password=" + passwd_sms + "&msg_type=text&auth_scheme=PLAIN";

            try
            {
                WebRequest request = WebRequest.Create(url_sms);

                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        Encoding ec = System.Text.Encoding.GetEncoding("utf-8");

                        using (StreamReader reader = new System.IO.StreamReader(stream, ec))
                        {
                            statusResult = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    using (Stream data = response.GetResponseStream())
                    using (var reader = new StreamReader(data))
                    {
                        string text = reader.ReadToEnd();
                        statusResult = string.Format("Error code: {0} and details : {1}", httpResponse.StatusCode, text);
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return statusResult;
        }

        //public static void sendSMS(string sMobile, string sMessage)
        //{
        //    string result = "";
        //    System.Net.WebRequest request = null;
        //    System.Net.HttpWebResponse response = null;

        //    try
        //    {
        //        //string userid = AESEncryption.DecryptData(CloudConfigurationManager.GetSetting("UIDSMS"), "HR$2pIjHR$2pIj12");
        //        //string passwd = AESEncryption.DecryptData(CloudConfigurationManager.GetSetting("PWDSMS"), "HR$2pIjHR$2pIj12");
        //        // string userid = AESEncryption.DecryptData(ConfigurationManager.AppSettings["UIDSMS"], "HR$2pIjHR$2pIj12");
        //        // string passwd = AESEncryption.DecryptData(ConfigurationManager.AppSettings["PWDSMS"], "HR$2pIjHR$2pIj12");
        //        //string url = "http://enterprise.smsgupshup.com/GatewayAPI/rest?method=sendMessage&send_to=" + sMobile + "&v=1.1&msg=This%20is%20a%20test%20message.%20&userid=2000121433&password=abc123%20&msg_type=text";
        //        //string url = "http://enterprise.smsgupshup.com/GatewayAPI/rest?method=sendMessage&send_to=" + sMobile + "&v=1.1&msg=" + HttpUtility.UrlEncode(sMessage, Encoding.UTF8) + "&userid=2000139741&password=abc123%20&msg_type=text&auth_scheme=PLAIN";
        //        // request = System.Net.WebRequest.Create(url);


        //        string userid_sms = ConfigurationManager.AppSettings["UIDSMS"];
        //        string passwd_sms = ConfigurationManager.AppSettings["PWDSMS"];
        //        string url_sms = "http://enterprise.smsgupshup.com/GatewayAPI/rest?method=sendMessage&send_to=" + sMobile + "&v=1.1&msg=" + HttpUtility.UrlEncode(sMessage, Encoding.UTF8) + "&userid=" + userid_sms + "&password=" + passwd_sms + "&msg_type=text&auth_scheme=PLAIN";
        //        request = System.Net.WebRequest.Create(url_sms);

        //        // in case u work behind proxy, uncomment the commented code and provide correct details 
        //        /*WebProxy proxy = new WebProxy("http://proxy:80/",true); 
        //        proxy.Credentials = new NetworkCredential("userId","password", "Domain"); 
        //        request.Proxy = proxy;*/
        //        // Send the 'HttpWebRequest' and wait for response. 
        //        response = (System.Net.HttpWebResponse)request.GetResponse();
        //        Stream stream = response.GetResponseStream();
        //        Encoding ec = System.Text.Encoding.GetEncoding("utf-8");
        //        StreamReader reader = new
        //        System.IO.StreamReader(stream, ec);
        //        result = reader.ReadToEnd();
        //        //lblResult.Text = result;
        //        reader.Close();
        //        stream.Close();

        //        //SendEmailToTrackSMS("<html> <body style='FONT-SIZE: 11pt; FONT-FAMILY: Calibri;'>Mobile::" + sMobile + "<p></p> Message::<p></p>" + sMessage + "<p></p> Result::<p></p>" + result + "</font></body> </html>");
        //    }
        //    catch (Exception exp)
        //    {
        //        //SendEmailToTrackSMS("Error while sending SMS :" + exp.Message);
        //        throw exp;
        //    }
        //    finally
        //    {
        //        if (response != null)
        //            response.Close();
        //    }

        //}

        public static string getIpAddress()
        {
            string retval = "";
            try
            {
                string ipAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ipAddress))
                {
                    retval = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
            }
            catch { }
            return retval;
        }

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        if (dr[column.ColumnName] == null && pro.PropertyType.IsValueType && Nullable.GetUnderlyingType(pro.PropertyType) == null)
                        {
                            pro.SetValue(obj, null, null);
                        }
                        else
                            pro.SetValue(obj, dr[column.ColumnName], null);

                    else
                        continue;
                }
            }
            return obj;
        }

        //public static void SendEmailToTrackSMS(string strErrMsg)
        //{
        //    /////////////////Send Email to the support////////////////
        //    MailComponent objMail = new MailComponent();
        //    objMail.FromUserName = getAccount_email_address();
        //    objMail.ToMailID = getTrackSMSemail();
        //    objMail.Subject = "High Priority - Track SMS";

        //    objMail.BodyContent = strErrMsg;
        //    objMail.SendEmailWithoutBCC("CustomerSupport");
        //    /////////////////Send Email to the support////////////////
        //}

        public static string GenerateReferalCode(int length)
        {
            Random random = new Random();
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
        }

        const string ALPHABET = "AG8FOLE2WVTCPY5ZH3NIUDBXSMQK7946";
        /// <summary>
        /// This will create a User friedly Referral code 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GenerateReferralCode(long userId, int codeLength)
        {
            var userIdLength = userId.ToString().Length;
            var remainingLengthTakeFromGUID = codeLength - userIdLength;

            String randomString = Guid.NewGuid().ToString().ToUpper().Substring(0, remainingLengthTakeFromGUID);

            return randomString += userId.ToString();

            //Random random = new Random((int)DateTime.Now.Ticks);

            //StringBuilder b = new StringBuilder();
            //for (long i = 0; i < 6; ++i)
            //{
            //    b.Append(ALPHABET[(int)userId & ((1 << 5) - 1)]);
            //    userId = userId >> 5;
            //}
            //return b.ToString();
        }

        public static int GetUserIdFromReferralCode(string coupon)
        {
            int n = 0;
            for (int i = 0; i < 6; ++i)
                n = n | (((int)ALPHABET.IndexOf(coupon[i])) << (5 * i));
            return n;
        }

        public static string getJsonforTree(DataTable dt)
        {
            StringBuilder str = new StringBuilder();
            str.Append("[");
            foreach (DataRow dr in dt.Rows)
            {
                str.Append("{");
                string jsonval = "";
                jsonval = "\"Occupancy\" : \"" + dr[0] + "\" ,";
                jsonval += "\"Rooms\" : \"" + dr[1] + "\"";
                str.Append(jsonval);
                str.Append("},");
            }
            str.Replace(',', ']', str.Length - 1, 1);
            return str.ToString();
        }

        //public static async Task sendSMSAsync(string sMobile, string sMessage)
        //{
        //    Task<string> result;
        //    System.Net.WebRequest request = null;
        //    WebResponse response = null;

        //    try
        //    {
        //        string userid = AESEncryption.DecryptData(ConfigurationManager.AppSettings["UIDSMS"], "HR$2pIjHR$2pIj12");
        //        string passwd = AESEncryption.DecryptData(ConfigurationManager.AppSettings["PWDSMS"], "HR$2pIjHR$2pIj12");
        //        string url = "http://enterprise.smsgupshup.com/GatewayAPI/rest?method=sendMessage&send_to=" + sMobile + "&v=1.1&msg=" + HttpUtility.UrlEncode(sMessage, Encoding.UTF8) + "&userid=2000152864&password=nKXZS6MXd&msg_type=text&auth_scheme=PLAIN";
        //        request = System.Net.WebRequest.Create(url);
        //        response = await request.GetResponseAsync();
        //        Stream stream = response.GetResponseStream();
        //        Encoding ec = System.Text.Encoding.GetEncoding("utf-8");
        //        StreamReader reader = new
        //        System.IO.StreamReader(stream, ec);
        //        result = reader.ReadToEndAsync();
        //        reader.Close();
        //        stream.Close();

        //    }
        //    catch (Exception exp)
        //    {
        //        throw exp;
        //    }
        //    finally
        //    {
        //        if (response != null)
        //            response.Close();
        //    }
        //}

        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        public static string GetTimeType(string hours)
        {
            var type = string.Empty;
            type = "";
            // var value = Convert.ToInt32(hours);
            // if (value == 0)
            // {
            //     type = EnumHelper<TimeType>.GetDisplayValue(TimeType.Midnight);
            // }
            // else if (value>0&&value<3)
            // {
            //     type = EnumHelper<TimeType>.GetDisplayValue(TimeType.Latenight);
            // }
            // else if (value < 12)
            //{
            //    type= EnumHelper<TimeType>.GetDisplayValue(TimeType.Morning);
            //}
            // else if (value> 12&&value<17)
            //{
            //    type = EnumHelper<TimeType>.GetDisplayValue(TimeType.Noon);
            //}
            //else
            //{

            //    type = EnumHelper<TimeType>.GetDisplayValue(TimeType.Evening);
            //}

            return type;
        }

        public enum TimeType
        {
            [Display(Name = "Morning")]
            Morning = 0,
            [Display(Name = "Noon")]
            Noon = 1,
            [Display(Name = "Evening")]
            Evening = 2,
            [Display(Name = "Late-Night")]
            Latenight = 3,
            [Display(Name = "Mid-Night")]
            Midnight = 4
        }

        /// <summary>
        /// Call Google api to get the short url for the respective long url
        /// </summary>
        /// <param name="longUrl"></param>
        /// <returns></returns>       
        public static string Shorten(string longUrl)
        {
            //TO DO Configurable
            const string key = "AIzaSyCqzMa9JSJ_YGyEODbG9z-KzOkWD0TS82Y";
            string post = "{\"longUrl\": \"" + longUrl + "\"}";
            string shortUrl = longUrl;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/urlshortener/v1/url?key=" + key);

            try
            {
                request.ServicePoint.Expect100Continue = false;
                request.Method = "POST";
                request.ContentLength = post.Length;
                request.ContentType = "application/json";
                request.Headers.Add("Cache-Control", "no-cache");

                using (Stream requestStream = request.GetRequestStream())
                {
                    byte[] postBuffer = Encoding.ASCII.GetBytes(post);
                    requestStream.Write(postBuffer, 0, postBuffer.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader responseReader = new StreamReader(responseStream))
                        {
                            string json = responseReader.ReadToEnd();
                            shortUrl = Regex.Match(json, @"""id"": ?""(?<id>.+)""").Groups["id"].Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // if Google's URL Shortner is down...
                System.Diagnostics.Debug.WriteLine(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);

                shortUrl = longUrl;
            }
            return shortUrl;
        }


        public static void SendErrorMail(Exception ex)
        {
            MailComponent.SendEmailTest("deepaka@futuresoftindia.com,himanshuS@futuresoftindia.com", "", "", "Error", ex.StackTrace + "Message:: " + ex.ToString(), null, null, true);
        }



    }

        public static class Utils
        {
                public static DataTable ToDataTable<T>(this List<T> iList)
    {
        DataTable dataTable = new DataTable();
        PropertyDescriptorCollection propertyDescriptorCollection =
            TypeDescriptor.GetProperties(typeof(T));
        for (int i = 0; i < propertyDescriptorCollection.Count; i++)
        {
            PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[i];
            Type type = propertyDescriptor.PropertyType;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                type = Nullable.GetUnderlyingType(type);


            dataTable.Columns.Add(propertyDescriptor.Name, type);
        }
        object[] values = new object[propertyDescriptorCollection.Count];
        foreach (T iListItem in iList)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = propertyDescriptorCollection[i].GetValue(iListItem);
            }
            dataTable.Rows.Add(values);
        }
        return dataTable;
    }
        }

}