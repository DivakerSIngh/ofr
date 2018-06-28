using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Net;
using System.Xml;
using OneFineRateBLL;
using System.Configuration;

namespace OneFineRateBLL
{
    public class clsTravelGuruApi
    {
        public static string FetchBookingTravelGuruApi(string xmlString)
        {

            String sResponseFromServer = null;
            StreamReader tReader = null;
            Stream dataStream = null;
            WebResponse tResponse = null;
            try
            {
                WebRequest tRequest;
                var endPointUrl = ConfigurationManager.AppSettings["TG_BookingEndPoints"].ToString();
                tRequest = WebRequest.Create(endPointUrl);
                tRequest.Method = "post";
                tRequest.ContentType = "text/xml; charset=utf-8";
                tRequest.Headers.Add(string.Format("SOAP:Action"));

                //Data post to the Server
                string postData = xmlString;//"<s:Envelope xmlns:s='http://schemas.xmlsoap.org/soap/envelope/'><s:Body xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><OTA_HotelResRQ CorrelationID='033e6652-6c24-4ad9-a3f2-695e843e7166' Version='0' xmlns='http://www.opentravel.org/OTA/2003/05'><POS><Source ISOCurrency='INR'><RequestorID ID='1300001224' MessagePassword='test@567'><CompanyName Code='samaara'/></RequestorID></Source></POS><UniqueID Type='' ID=''/><HotelReservations><HotelReservation><RoomStays><RoomStay><RoomTypes><RoomType NumberOfUnits='1' RoomTypeCode='0000063862'/></RoomTypes><RatePlans><RatePlan RatePlanCode='0000252277'/></RatePlans><GuestCounts><GuestCount AgeQualifyingCode='10' Count='1'/><GuestCount AgeQualifyingCode='8' Age='10' Count='1'/></GuestCounts><TimeSpan End='2016-09-25' Start='2016-09-24'/><Total AmountBeforeTax='11500.0' CurrencyCode='INR'><Taxes Amount='3003.2' CurrencyCode='INR'/></Total><BasicPropertyInfo HotelCode='00007112'/></RoomStay></RoomStays><ResGuests><ResGuest><Profiles><ProfileInfo><Profile ProfileType='1'><Customer><PersonName><GivenName>Desiya</GivenName><Surname>Test</Surname></PersonName><Telephone CountryAccessCode='91' PhoneNumber='1234567890' PhoneTechType='1'/><Email>abc@gmail.com</Email><Address><AddressLine>Desiya</AddressLine><AddressLine>Malad</AddressLine><CityName>Mumbai</CityName><PostalCode>400064</PostalCode><StateProv StateCode='MH'>Maharashtra</StateProv><CountryName Code='IN'>IN</CountryName></Address></Customer></Profile></ProfileInfo></Profiles></ResGuest></ResGuests><ResGlobalInfo><Guarantee GuaranteeType='PrePay'/></ResGlobalInfo></HotelReservation></HotelReservations></OTA_HotelResRQ></s:Body></s:Envelope>";
                //Console.WriteLine(postData);

                Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                tRequest.ContentLength = byteArray.Length;
                dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                tResponse = tRequest.GetResponse();
                dataStream = tResponse.GetResponseStream();
                tReader = new StreamReader(dataStream);
                sResponseFromServer = tReader.ReadToEnd();  //Get response from GCM server  
                tReader.Close();
                dataStream.Close();
                tResponse.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (tReader != null)
                {
                    tReader.Close();
                }
                if (dataStream != null)
                {
                    dataStream.Close();
                }
                if (tResponse != null)
                {
                    tResponse.Close();
                }
            }
            return sResponseFromServer;

        }

        public static string SearchTravelGuruApi(string xmlString)
        {
            String sResponseFromServer = null;
            StreamReader tReader = null;
            Stream dataStream = null;
            WebResponse tResponse = null;
            try
            {
                WebRequest tRequest;
                var endPointUrl = ConfigurationManager.AppSettings["TG_SearchEndPoints"].ToString();
                tRequest = WebRequest.Create(endPointUrl);
                tRequest.Method = "post";
                tRequest.ContentType = "text/xml; charset=utf-8";
                tRequest.Headers.Add(string.Format("SOAP:Action"));

                //Data post to the Server
                string postData = xmlString;//"<s:Envelope xmlns:s='http://schemas.xmlsoap.org/soap/envelope/'><s:Body xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><OTA_HotelResRQ CorrelationID='033e6652-6c24-4ad9-a3f2-695e843e7166' Version='0' xmlns='http://www.opentravel.org/OTA/2003/05'><POS><Source ISOCurrency='INR'><RequestorID ID='1300001224' MessagePassword='test@567'><CompanyName Code='samaara'/></RequestorID></Source></POS><UniqueID Type='' ID=''/><HotelReservations><HotelReservation><RoomStays><RoomStay><RoomTypes><RoomType NumberOfUnits='1' RoomTypeCode='0000063862'/></RoomTypes><RatePlans><RatePlan RatePlanCode='0000252277'/></RatePlans><GuestCounts><GuestCount AgeQualifyingCode='10' Count='1'/><GuestCount AgeQualifyingCode='8' Age='10' Count='1'/></GuestCounts><TimeSpan End='2016-09-25' Start='2016-09-24'/><Total AmountBeforeTax='11500.0' CurrencyCode='INR'><Taxes Amount='3003.2' CurrencyCode='INR'/></Total><BasicPropertyInfo HotelCode='00007112'/></RoomStay></RoomStays><ResGuests><ResGuest><Profiles><ProfileInfo><Profile ProfileType='1'><Customer><PersonName><GivenName>Desiya</GivenName><Surname>Test</Surname></PersonName><Telephone CountryAccessCode='91' PhoneNumber='1234567890' PhoneTechType='1'/><Email>abc@gmail.com</Email><Address><AddressLine>Desiya</AddressLine><AddressLine>Malad</AddressLine><CityName>Mumbai</CityName><PostalCode>400064</PostalCode><StateProv StateCode='MH'>Maharashtra</StateProv><CountryName Code='IN'>IN</CountryName></Address></Customer></Profile></ProfileInfo></Profiles></ResGuest></ResGuests><ResGlobalInfo><Guarantee GuaranteeType='PrePay'/></ResGlobalInfo></HotelReservation></HotelReservations></OTA_HotelResRQ></s:Body></s:Envelope>";
                //Console.WriteLine(postData);

                Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                tRequest.ContentLength = byteArray.Length;
                dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                //Get the response from the API
                tResponse = tRequest.GetResponse();
                dataStream = tResponse.GetResponseStream();
                tReader = new StreamReader(dataStream);
                sResponseFromServer = tReader.ReadToEnd();  //Get response from GCM server  

                tReader.Close();
                dataStream.Close();
                tResponse.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (tReader != null)
                {
                    tReader.Close();
                }
                if (dataStream != null)
                {
                    dataStream.Close();
                }
                if (tResponse != null)
                {
                    tResponse.Close();
                }
            }
            return sResponseFromServer;
        }


        public static async Task<string> SearchTravelGuruApiAsync(string xmlString)
        {
            String sResponseFromServer = null;
            StreamReader tReader = null;
            Stream dataStream = null;
            WebResponse tResponse = null;
            try
            {
                WebRequest tRequest;
                var endPointUrl = ConfigurationManager.AppSettings["TG_SearchEndPoints"].ToString();
                tRequest = WebRequest.Create(endPointUrl);
                tRequest.Method = "post";
                tRequest.ContentType = "text/xml; charset=utf-8";
                tRequest.Headers.Add(string.Format("SOAP:Action"));

                //Data post to the Server
                string postData = xmlString;//"<s:Envelope xmlns:s='http://schemas.xmlsoap.org/soap/envelope/'><s:Body xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'><OTA_HotelResRQ CorrelationID='033e6652-6c24-4ad9-a3f2-695e843e7166' Version='0' xmlns='http://www.opentravel.org/OTA/2003/05'><POS><Source ISOCurrency='INR'><RequestorID ID='1300001224' MessagePassword='test@567'><CompanyName Code='samaara'/></RequestorID></Source></POS><UniqueID Type='' ID=''/><HotelReservations><HotelReservation><RoomStays><RoomStay><RoomTypes><RoomType NumberOfUnits='1' RoomTypeCode='0000063862'/></RoomTypes><RatePlans><RatePlan RatePlanCode='0000252277'/></RatePlans><GuestCounts><GuestCount AgeQualifyingCode='10' Count='1'/><GuestCount AgeQualifyingCode='8' Age='10' Count='1'/></GuestCounts><TimeSpan End='2016-09-25' Start='2016-09-24'/><Total AmountBeforeTax='11500.0' CurrencyCode='INR'><Taxes Amount='3003.2' CurrencyCode='INR'/></Total><BasicPropertyInfo HotelCode='00007112'/></RoomStay></RoomStays><ResGuests><ResGuest><Profiles><ProfileInfo><Profile ProfileType='1'><Customer><PersonName><GivenName>Desiya</GivenName><Surname>Test</Surname></PersonName><Telephone CountryAccessCode='91' PhoneNumber='1234567890' PhoneTechType='1'/><Email>abc@gmail.com</Email><Address><AddressLine>Desiya</AddressLine><AddressLine>Malad</AddressLine><CityName>Mumbai</CityName><PostalCode>400064</PostalCode><StateProv StateCode='MH'>Maharashtra</StateProv><CountryName Code='IN'>IN</CountryName></Address></Customer></Profile></ProfileInfo></Profiles></ResGuest></ResGuests><ResGlobalInfo><Guarantee GuaranteeType='PrePay'/></ResGlobalInfo></HotelReservation></HotelReservations></OTA_HotelResRQ></s:Body></s:Envelope>";
                //Console.WriteLine(postData);

                Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                tRequest.ContentLength = byteArray.Length;
                dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                tResponse = await tRequest.GetResponseAsync();
                dataStream = tResponse.GetResponseStream();
                tReader = new StreamReader(dataStream);
                sResponseFromServer = await tReader.ReadToEndAsync();  //Get response from GCM server  

                tReader.Close();
                dataStream.Close();
                tResponse.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (tReader != null)
                {
                    tReader.Close();
                }
                if (dataStream != null)
                {
                    dataStream.Close();
                }
                if (tResponse != null)
                {
                    tResponse.Close();
                }
            }
            return sResponseFromServer;
        }



        public static string ConvertToXmlString(Envelope objEnvelope)
        {
            string xmlString = "";
            try
            {
                XmlSerializer xsSubmit = new XmlSerializer(typeof(Envelope));

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                // exclude xsi and xsd namespaces by adding the following:
                ns.Add(string.Empty, string.Empty);

                using (StringWriter textWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                    {

                        xsSubmit.Serialize(xmlWriter, objEnvelope, ns);
                    }
                    xmlString = textWriter.ToString(); //This is the output as a string
                }
            }
            catch (Exception ex)
            {

            }
            return xmlString;
        }

        public static T FromXml<T>(String xml)
        {
            T returnedXmlClass = default(T);

            try
            {
                using (TextReader reader = new StringReader(xml))
                {
                    try
                    {
                        returnedXmlClass = (T)new XmlSerializer(typeof(T)).Deserialize(reader);
                    }
                    catch (InvalidOperationException)
                    {
                        // String passed is not XML, simply return defaultXmlClass
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return returnedXmlClass;
        }
    }

    #region Envelop Class
    [XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Body
    {
        [XmlElement(ElementName = "OTA_HotelResRQ", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public OTA_HotelResRQ OTA_HotelResRQ { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "xsd", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsd { get; set; }
    }

    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Envelope
    {
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public Body Body { get; set; }
        [XmlAttribute(AttributeName = "s", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string S { get; set; }
    }
    #endregion
}