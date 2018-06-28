using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Net;
using System.Xml;
using OneFineRateBLL.Entities;

namespace OneFineRateBLL
{
    public class clsBooking
    {
        public static eProvisionalBookingResponseModel ProvisionalBooking(TG_ProvisionalBookingRequestModel bookingModel)
        {
            try
            {

                #region New Modified Code

                Envelope objEnvelope = new Envelope();
                objEnvelope.S = "http://schemas.xmlsoap.org/soap/envelope/";

                Body objBody = new Body();
                objBody.Xsd = "http://www.w3.org/2001/XMLSchema";
                objBody.Xsi = "http://www.w3.org/2001/XMLSchema-instance";

                OTA_HotelResRQ objOTA_HotelResRQ = new OTA_HotelResRQ();

                POS objPOS = new POS();

                Source objSource = new Source();
                objSource.ISOCurrency = bookingModel.Currency;
                RequestorID objRequestorID = new RequestorID();
                objRequestorID.ID = bookingModel.API_Credential_PropertyId;
                objRequestorID.MessagePassword = bookingModel.API_Credential_Password;
                CompanyName objCompanyName = new CompanyName();
                objCompanyName.Code = bookingModel.API_Credential_UserName;
                objRequestorID.CompanyName = objCompanyName;
                objSource.RequestorID = objRequestorID;
                objPOS.Source = objSource;


                UniqueID objUniqueID = new UniqueID();
                objUniqueID.ID = bookingModel.UniqueId;
                objUniqueID.Type = bookingModel.UniqueIdType;


                //==> Hotel Reservaltions
                HotelReservations objHotelReservations = new HotelReservations();
                HotelReservation objHotelReservation = new HotelReservation();
                RoomStays objRoomStays = new RoomStays();
                RoomStay objRoomStay = new RoomStay();

                RoomTypes objRoomTypes = new RoomTypes();
                RoomType objRoomType = new RoomType();
                objRoomType.RoomTypeCode = bookingModel.RoomTypeCode;
                objRoomType.NumberOfUnits = bookingModel.NumberOfRooms;
                objRoomTypes.RoomType = objRoomType;

                RatePlans objRatePlans = new RatePlans();
                RatePlan objRatePlan = new RatePlan();
                objRatePlan.RatePlanCode = bookingModel.RatePlanCode;
                objRatePlans.RatePlan = objRatePlan;


                GuestCounts objGuestCounts = new GuestCounts();
                List<GuestCount> objGuestCountArr = new System.Collections.Generic.List<GuestCount>();

                foreach (var room in bookingModel.RoomData)
                {
                    GuestCount objAdultCount = new GuestCount();
                    objAdultCount.Count = room.adult.ToString();
                    objAdultCount.AgeQualifyingCode = "10";
                    objGuestCountArr.Add(objAdultCount);

                    foreach (var item in room.ChildAge)
                    {
                        GuestCount objChildCount = new GuestCount();
                        objChildCount.Count = "1";
                        objChildCount.AgeQualifyingCode = "8";
                        objChildCount.Age = item.Age;
                        objGuestCountArr.Add(objChildCount);
                    }
                }

                objGuestCounts.GuestCount = objGuestCountArr;

                TimeSpan objTimeSpan = new TimeSpan();
                objTimeSpan.End = bookingModel.CheckOut;
                objTimeSpan.Start = bookingModel.CheckIn;

                Total objTotal = new Total();
                objTotal.CurrencyCode = bookingModel.Currency;
                objTotal.AmountBeforeTax = bookingModel.AmountBeforeTax;
                Taxes objTaxes = new Taxes();
                objTaxes.CurrencyCode = bookingModel.Currency;
                objTaxes.Amount = bookingModel.TaxAmount;
                objTotal.Taxes = objTaxes;

                BasicPropertyInfo objBasicPropertyInfo = new BasicPropertyInfo();
                objBasicPropertyInfo.HotelCode = bookingModel.HotelCode;

                Comments objComments = new Comments();
                Comment objComment = new Comment();
                objComment.Text = bookingModel.CustomerComment;
                objComments.Comment = objComment;

                objRoomStay.RoomTypes = objRoomTypes;
                objRoomStay.RatePlans = objRatePlans;
                objRoomStay.GuestCounts = objGuestCounts;
                objRoomStay.TimeSpan = objTimeSpan;
                objRoomStay.Total = objTotal;
                objRoomStay.BasicPropertyInfo = objBasicPropertyInfo;
                objRoomStay.Comments = objComments;
                objRoomStays.RoomStay = objRoomStay;

                ResGuests objResGuests = new ResGuests();
                ResGuest objResGuest = new ResGuest();
                Profiles objProfiles = new Profiles();
                ProfileInfo objProfileInfo = new ProfileInfo();
                Profile objProfile = new Profile();
                objProfile.ProfileType = bookingModel.ProfileType;
                Customer objCustomer = new Customer();

                PersonName objPersonName = new PersonName();
                objPersonName.GivenName = bookingModel.CustomerGivenName;
                objPersonName.Surname = bookingModel.CustomerSurname;

                Telephone objTelephone = new Telephone();
                objTelephone.PhoneTechType = bookingModel.PhoneTechType;
                objTelephone.PhoneNumber = bookingModel.PhoneNumber;
                objTelephone.CountryAccessCode = bookingModel.CountryAccessCode;

                Address objAddress = new Address();
                objAddress.AddressLine = bookingModel.AddressLine;
                objAddress.CityName = bookingModel.CityName;
                objAddress.PostalCode = bookingModel.PostalCode;


                StateProv objStateProv = new StateProv();
                objStateProv.StateCode = bookingModel.StateCode;
                objStateProv.Text = bookingModel.StateName;

                CountryName objCountryName = new CountryName();
                objCountryName.Code = bookingModel.CountryCode;
                objCountryName.Text = bookingModel.CountryName;

                objAddress.StateProv = objStateProv;
                objAddress.CountryName = objCountryName;

                objCustomer.PersonName = objPersonName;
                objCustomer.Telephone = objTelephone;
                objCustomer.Email = bookingModel.CustomerEmail;
                objCustomer.Address = objAddress;

                objProfile.Customer = objCustomer;

                objProfileInfo.Profile = objProfile;

                objProfiles.ProfileInfo = objProfileInfo;

                objResGuest.Profiles = objProfiles;

                objResGuests.ResGuest = objResGuest;

                ResGlobalInfo objResGlobalInfo = new ResGlobalInfo();
                Guarantee objGuarantee = new Guarantee();
                objGuarantee.GuaranteeType = bookingModel.GuaranteeType;
                objResGlobalInfo.Guarantee = objGuarantee;

                objHotelReservation.RoomStays = objRoomStays;
                objHotelReservation.ResGuests = objResGuests;
                objHotelReservation.ResGlobalInfo = objResGlobalInfo;

                objHotelReservations.HotelReservation = objHotelReservation;

                objOTA_HotelResRQ.POS = objPOS;
                objOTA_HotelResRQ.UniqueID = objUniqueID;
                objOTA_HotelResRQ.HotelReservations = objHotelReservations;
                objOTA_HotelResRQ.Xmlns = "http://www.w3.org/2001/XMLSchema";
                objOTA_HotelResRQ.Version = "0";
                objOTA_HotelResRQ.CorrelationID = bookingModel.CorrelationID;

                objBody.OTA_HotelResRQ = objOTA_HotelResRQ;
                objEnvelope.Body = objBody;

                #endregion New Modified Code


                #region PreviousCode

                //Envelope objEnvelope = new Envelope();
                //objEnvelope.S = "http://schemas.xmlsoap.org/soap/envelope/";

                //Body objBody = new Body();
                //objBody.Xsd = "http://www.w3.org/2001/XMLSchema";
                //objBody.Xsi = "http://www.w3.org/2001/XMLSchema-instance";

                //OTA_HotelResRQ objOTA_HotelResRQ = new OTA_HotelResRQ();

                ////==> POS
                //POS objPOS = new POS();

                //Source objSource = new Source();
                //objSource.ISOCurrency = "INR";
                //RequestorID objRequestorID = new RequestorID();
                //objRequestorID.ID = "1300001224";
                //objRequestorID.MessagePassword = "test@567";
                //CompanyName objCompanyName = new CompanyName();
                //objCompanyName.Code = "samaara";
                //objRequestorID.CompanyName = objCompanyName;
                //objSource.RequestorID = objRequestorID;
                //objPOS.Source = objSource;
                ////==> End

                ////==> UniqueId
                //UniqueID objUniqueID = new UniqueID();
                //objUniqueID.ID = "";
                //objUniqueID.Type = "";

                ////==> End

                ////==> Hotel Reservaltions
                //HotelReservations objHotelReservations = new HotelReservations();
                //HotelReservation objHotelReservation = new HotelReservation();
                //RoomStays objRoomStays = new RoomStays();
                //RoomStay objRoomStay = new RoomStay();

                //RoomTypes objRoomTypes = new RoomTypes();
                //RoomType objRoomType = new RoomType();
                //objRoomType.RoomTypeCode = "0000116417";
                //objRoomType.NumberOfUnits = "1";
                //objRoomTypes.RoomType = objRoomType;

                //RatePlans objRatePlans = new RatePlans();
                //RatePlan objRatePlan = new RatePlan();
                //objRatePlan.RatePlanCode = "0000432031";
                //objRatePlans.RatePlan = objRatePlan;

                //GuestCounts objGuestCounts = new GuestCounts();
                //List<GuestCount> objGuestCountArr = new System.Collections.Generic.List<GuestCount>();
                //GuestCount objGuestCount = new GuestCount();
                //objGuestCount.Count = "1";
                //objGuestCount.AgeQualifyingCode = "10";
                //GuestCount objGuestCount2 = new GuestCount();
                //objGuestCount2.Count = "1";
                //objGuestCount2.AgeQualifyingCode = "8";
                //objGuestCount2.Age = "10";
                //objGuestCountArr.Add(objGuestCount);
                //objGuestCountArr.Add(objGuestCount2);
                //objGuestCounts.GuestCount = objGuestCountArr;

                //TimeSpan objTimeSpan = new TimeSpan();
                //objTimeSpan.Start = "2016-12-12";
                //objTimeSpan.End = "2016-12-15";

                //Total objTotal = new Total();
                //objTotal.CurrencyCode = "INR";
                //objTotal.AmountBeforeTax = "3400.0";
                //Taxes objTaxes = new Taxes();
                //objTaxes.CurrencyCode = "INR";
                //objTaxes.Amount = "888.31";
                //objTotal.Taxes = objTaxes;

                //BasicPropertyInfo objBasicPropertyInfo = new BasicPropertyInfo();
                //objBasicPropertyInfo.HotelCode = "00019853";

                //Comments objComments = new Comments();
                //Comment objComment = new Comment();
                //objComment.Text = "non-smoking room requested;king bed";
                //objComments.Comment = objComment;

                //objRoomStay.RoomTypes = objRoomTypes;
                //objRoomStay.RatePlans = objRatePlans;
                //objRoomStay.GuestCounts = objGuestCounts;
                //objRoomStay.TimeSpan = objTimeSpan;
                //objRoomStay.Total = objTotal;
                //objRoomStay.BasicPropertyInfo = objBasicPropertyInfo;
                //objRoomStay.Comments = objComments;
                //objRoomStays.RoomStay = objRoomStay;

                //ResGuests objResGuests = new ResGuests();
                //ResGuest objResGuest = new ResGuest();
                //Profiles objProfiles = new Profiles();
                //ProfileInfo objProfileInfo = new ProfileInfo();
                //Profile objProfile = new Profile();
                //objProfile.ProfileType = "1";
                //Customer objCustomer = new Customer();

                //PersonName objPersonName = new PersonName();
                //objPersonName.GivenName = "Desiya";
                //objPersonName.Surname = "Test";

                //Telephone objTelephone = new Telephone();
                //objTelephone.PhoneTechType = "1";
                //objTelephone.PhoneNumber = "1234567890";
                //objTelephone.CountryAccessCode = "91";

                //Address objAddress = new Address();

                //List<string> AddressLine = new System.Collections.Generic.List<string> { "Desia", "Malad" };
                //objAddress.AddressLine = AddressLine;
                //objAddress.CityName = "Mumbai";
                //objAddress.PostalCode = "400064";

                //StateProv objStateProv = new StateProv();
                //objStateProv.StateCode = "MH";
                //objStateProv.Text = "Maharastra";

                //CountryName objCountryName = new CountryName();
                //objCountryName.Code = "IN";
                //objCountryName.Text = "IN";

                //objAddress.StateProv = objStateProv;
                //objAddress.CountryName = objCountryName;

                //objCustomer.PersonName = objPersonName;
                //objCustomer.Telephone = objTelephone;
                //objCustomer.Email = "abc@gmail.com";
                //objCustomer.Address = objAddress;

                //objProfile.Customer = objCustomer;

                //objProfileInfo.Profile = objProfile;

                //objProfiles.ProfileInfo = objProfileInfo;

                //objResGuest.Profiles = objProfiles;

                //objResGuests.ResGuest = objResGuest;

                //ResGlobalInfo objResGlobalInfo = new ResGlobalInfo();
                //Guarantee objGuarantee = new Guarantee();
                //objGuarantee.GuaranteeType = "PrePay";
                //objResGlobalInfo.Guarantee = objGuarantee;

                //objHotelReservation.RoomStays = objRoomStays;
                //objHotelReservation.ResGuests = objResGuests;
                //objHotelReservation.ResGlobalInfo = objResGlobalInfo;

                //objHotelReservations.HotelReservation = objHotelReservation;
                ////==> End

                //objOTA_HotelResRQ.POS = objPOS;
                //objOTA_HotelResRQ.UniqueID = objUniqueID;
                //objOTA_HotelResRQ.HotelReservations = objHotelReservations;
                //objOTA_HotelResRQ.Xmlns = "http://www.w3.org/2001/XMLSchema";
                //objOTA_HotelResRQ.Version = "0";
                //objOTA_HotelResRQ.CorrelationID = "1234587345";

                //objBody.OTA_HotelResRQ = objOTA_HotelResRQ;
                //objEnvelope.Body = objBody;

                //#region
                ////string xmlString = "";
                ////XmlSerializer xsSubmit = new XmlSerializer(typeof(Envelope));

                ////XmlWriterSettings settings = new XmlWriterSettings();
                ////settings.Encoding = new UnicodeEncoding(false, false); // no BOM in a .NET string
                ////settings.Indent = true;
                ////settings.OmitXmlDeclaration = true;

                ////XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ////// exclude xsi and xsd namespaces by adding the following:
                ////ns.Add(string.Empty, string.Empty);

                ////using (StringWriter textWriter = new StringWriter())
                ////{
                ////    using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                ////    {

                ////        xsSubmit.Serialize(xmlWriter, objEnvelope, ns);
                ////    }
                ////    xmlString = textWriter.ToString(); //This is the output as a string
                ////}
                ////#endregion 

                #endregion Previous Code


                string xmlRequestBody = clsTravelGuruApi.ConvertToXmlString(objEnvelope);

                string status = clsTravelGuruApi.FetchBookingTravelGuruApi(xmlRequestBody);

                EnvelopePBRes objEnvelopeSHRes = clsTravelGuruApi.FromXml<EnvelopePBRes>(status);

                eProvisionalBookingResponseModel response = new eProvisionalBookingResponseModel();

                var apiResponse = objEnvelopeSHRes.Body.OTA_HotelResRS;

                if (apiResponse.Errors.Error != null)
                {
                    var error = apiResponse.Errors.Error;

                    // For Temperory booking Issue fix , Need to change once price mismatch issue will fix.
                    //if (error.Code == "083")
                    //{
                    //    var amountAfterTax = Convert.ToDecimal(apiResponse.HotelReservations.HotelReservation.RoomStays.RoomStay.Total.AmountAfterTax);
                    //    var taxAmount = Convert.ToDecimal(objTaxes.Amount);
                    //    objTotal.AmountBeforeTax = (amountAfterTax - taxAmount).ToString();

                    //    string xmlRequestBodyTemp = clsTravelGuruApi.ConvertToXmlString(objEnvelope);

                    //    string statusTemp = clsTravelGuruApi.FetchBookingTravelGuruApi(xmlRequestBodyTemp);

                    //    EnvelopePBRes objEnvelopeSHResTemp = clsTravelGuruApi.FromXml<EnvelopePBRes>(statusTemp);

                    //    eProvisionalBookingResponseModel responseTemp = new eProvisionalBookingResponseModel();

                    //    var tempResponse= objEnvelopeSHResTemp.Body.OTA_HotelResRS;

                    //    if (tempResponse != null)
                    //    {
                    //        var tempError = tempResponse.Errors.Error;
                    //        responseTemp.IsSuccedded = false;
                    //        responseTemp.ErrorCode = tempError.Code;
                    //        responseTemp.ErrorMessage = tempError.Text;

                    //    }
                       
                    //    return responseTemp;

                    //}
                    response.IsSuccedded = false;
                    response.ErrorCode = error.Code;
                    response.ErrorMessage = error.Text ?? "An unknown error has happen!";

                }
                else
                {
                    response.IsSuccedded = true;
                    response.CorrelationID = apiResponse.CorrelationID;
                    response.UniqueId = apiResponse.HotelReservations.HotelReservation.UniqueID.ID;
                    response.UniqueIdType = apiResponse.HotelReservations.HotelReservation.UniqueID.Type;
                }

                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static eProvisionalBookingResponseModel FinalBooking(eProvisionalBookingRequestModel bookingModel)
        {
            try
            {

                #region New Modified Code

                Envelope objEnvelope = new Envelope();
                objEnvelope.S = "http://schemas.xmlsoap.org/soap/envelope/";

                Body objBody = new Body();
                objBody.Xsd = "http://www.w3.org/2001/XMLSchema";
                objBody.Xsi = "http://www.w3.org/2001/XMLSchema-instance";

                OTA_HotelResRQ objOTA_HotelResRQ = new OTA_HotelResRQ();

                POS objPOS = new POS();

                Source objSource = new Source();
                objSource.ISOCurrency = bookingModel.Currency;
                RequestorID objRequestorID = new RequestorID();
                objRequestorID.ID = bookingModel.API_Credential_PropertyId;
                objRequestorID.MessagePassword = bookingModel.API_Credential_Password;
                CompanyName objCompanyName = new CompanyName();
                objCompanyName.Code = bookingModel.API_Credential_UserName;
                objRequestorID.CompanyName = objCompanyName;
                objSource.RequestorID = objRequestorID;
                objPOS.Source = objSource;


                UniqueID objUniqueID = new UniqueID();
                objUniqueID.ID = bookingModel.UniqueId;
                objUniqueID.Type = bookingModel.UniqueIdType;


                //==> Hotel Reservaltions
                HotelReservations objHotelReservations = new HotelReservations();
                HotelReservation objHotelReservation = new HotelReservation();

                ResGlobalInfo objResGlobalInfo = new ResGlobalInfo();
                Guarantee objGuarantee = new Guarantee();
                objGuarantee.GuaranteeType = bookingModel.GuaranteeType;
                objResGlobalInfo.Guarantee = objGuarantee;

                objHotelReservation.ResGlobalInfo = objResGlobalInfo;

                objHotelReservations.HotelReservation = objHotelReservation;

                objOTA_HotelResRQ.POS = objPOS;
                objOTA_HotelResRQ.UniqueID = objUniqueID;
                objOTA_HotelResRQ.HotelReservations = objHotelReservations;
                objOTA_HotelResRQ.Xmlns = "http://www.w3.org/2001/XMLSchema";
                objOTA_HotelResRQ.Version = "0";
                objOTA_HotelResRQ.CorrelationID = bookingModel.CorrelationID;

                objBody.OTA_HotelResRQ = objOTA_HotelResRQ;
                objEnvelope.Body = objBody;

                #endregion New Modified Code


                string xmlRequestBody = clsTravelGuruApi.ConvertToXmlString(objEnvelope);

                string status = clsTravelGuruApi.FetchBookingTravelGuruApi(xmlRequestBody);

                EnvelopePBRes objEnvelopeSHRes = clsTravelGuruApi.FromXml<EnvelopePBRes>(status);

                eProvisionalBookingResponseModel response = new eProvisionalBookingResponseModel();

                var apiResponse = objEnvelopeSHRes.Body.OTA_HotelResRS;

                if (apiResponse.Errors.Error != null)
                {
                    var error = objEnvelopeSHRes.Body.OTA_HotelResRS.Errors.Error;
                    response.IsSuccedded = false;
                    response.ErrorCode = error.Code;
                    response.ErrorMessage = error.Text;

                }
                else
                {
                    response.IsSuccedded = true;
                    response.CorrelationID = apiResponse.CorrelationID;
                    response.UniqueId = apiResponse.HotelReservations.HotelReservation.UniqueID.ID;
                    response.UniqueIdType = apiResponse.HotelReservations.HotelReservation.UniqueID.Type;
                }

                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static eProvisionalBookingResponseModel CancelBooking(eProvisionalBookingRequestModel bookingModel)
        {
            try
            {

                #region New Modified Code

                Envelope objEnvelope = new Envelope();
                objEnvelope.S = "http://schemas.xmlsoap.org/soap/envelope/";

                Body objBody = new Body();
                objBody.Xsd = "http://www.w3.org/2001/XMLSchema";
                objBody.Xsi = "http://www.w3.org/2001/XMLSchema-instance";

                OTA_HotelResRQ objOTA_HotelResRQ = new OTA_HotelResRQ();

                POS objPOS = new POS();

                Source objSource = new Source();
                objSource.ISOCurrency = bookingModel.Currency;
                RequestorID objRequestorID = new RequestorID();
                objRequestorID.ID = bookingModel.API_Credential_PropertyId;
                objRequestorID.MessagePassword = bookingModel.API_Credential_Password;
                CompanyName objCompanyName = new CompanyName();
                objCompanyName.Code = bookingModel.API_Credential_UserName;
                objRequestorID.CompanyName = objCompanyName;
                objSource.RequestorID = objRequestorID;
                objPOS.Source = objSource;


                UniqueID objUniqueID = new UniqueID();
                objUniqueID.ID = bookingModel.UniqueId;
                objUniqueID.Type = bookingModel.UniqueIdType;


                //==> Hotel Reservaltions
                HotelReservations objHotelReservations = new HotelReservations();
                HotelReservation objHotelReservation = new HotelReservation();

                ResGlobalInfo objResGlobalInfo = new ResGlobalInfo();
                Guarantee objGuarantee = new Guarantee();
                objGuarantee.GuaranteeType = bookingModel.GuaranteeType;
                objResGlobalInfo.Guarantee = objGuarantee;

                objHotelReservation.ResGlobalInfo = objResGlobalInfo;

                objHotelReservations.HotelReservation = objHotelReservation;

                objOTA_HotelResRQ.POS = objPOS;
                objOTA_HotelResRQ.UniqueID = objUniqueID;
                objOTA_HotelResRQ.HotelReservations = objHotelReservations;
                objOTA_HotelResRQ.Xmlns = "http://www.w3.org/2001/XMLSchema";
                objOTA_HotelResRQ.Version = "0";
                objOTA_HotelResRQ.CorrelationID = bookingModel.CorrelationID;

                objBody.OTA_HotelResRQ = objOTA_HotelResRQ;
                objEnvelope.Body = objBody;

                #endregion New Modified Code


                string xmlRequestBody = clsTravelGuruApi.ConvertToXmlString(objEnvelope);

                string status = clsTravelGuruApi.FetchBookingTravelGuruApi(xmlRequestBody);

                EnvelopePBRes objEnvelopeSHRes = clsTravelGuruApi.FromXml<EnvelopePBRes>(status);

                eProvisionalBookingResponseModel response = new eProvisionalBookingResponseModel();

                var apiResponse = objEnvelopeSHRes.Body.OTA_HotelResRS;

                if (apiResponse.Errors.Error != null)
                {
                    var error = objEnvelopeSHRes.Body.OTA_HotelResRS.Errors.Error;
                    response.IsSuccedded = false;
                    response.ErrorCode = error.Code;
                    response.ErrorMessage = error.Text;

                }
                else
                {
                    response.IsSuccedded = true;
                    response.CorrelationID = apiResponse.CorrelationID;
                    response.UniqueId = apiResponse.HotelReservations.HotelReservation.UniqueID.ID;
                    response.UniqueIdType = apiResponse.HotelReservations.HotelReservation.UniqueID.Type;
                }

                return response;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    #region Provisional Booking Request

    [XmlRoot(ElementName = "CompanyName", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class CompanyName
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }
    }

    [XmlRoot(ElementName = "RequestorID", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RequestorID
    {
        [XmlElement(ElementName = "CompanyName", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public CompanyName CompanyName { get; set; }
        [XmlAttribute(AttributeName = "ID")]
        public string ID { get; set; }
        [XmlAttribute(AttributeName = "MessagePassword")]
        public string MessagePassword { get; set; }
    }

    [XmlRoot(ElementName = "Source", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Source
    {
        [XmlElement(ElementName = "RequestorID", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RequestorID RequestorID { get; set; }
        [XmlAttribute(AttributeName = "ISOCurrency")]
        public string ISOCurrency { get; set; }
    }

    [XmlRoot(ElementName = "POS", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class POS
    {
        [XmlElement(ElementName = "Source", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Source Source { get; set; }
    }

    [XmlRoot(ElementName = "UniqueID", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class UniqueID
    {
        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "ID")]
        public string ID { get; set; }
    }

    [XmlRoot(ElementName = "RoomType", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RoomType
    {
        [XmlAttribute(AttributeName = "NumberOfUnits")]
        public string NumberOfUnits { get; set; }
        [XmlAttribute(AttributeName = "RoomTypeCode")]
        public string RoomTypeCode { get; set; }
    }

    [XmlRoot(ElementName = "RoomTypes", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RoomTypes
    {
        [XmlElement(ElementName = "RoomType", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RoomType RoomType { get; set; }
    }

    [XmlRoot(ElementName = "RatePlan", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RatePlan
    {
        [XmlAttribute(AttributeName = "RatePlanCode")]
        public string RatePlanCode { get; set; }
    }

    [XmlRoot(ElementName = "RatePlans", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RatePlans
    {
        [XmlElement(ElementName = "RatePlan", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RatePlan RatePlan { get; set; }
    }

    [XmlRoot(ElementName = "GuestCount", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class GuestCount
    {
        [XmlAttribute(AttributeName = "AgeQualifyingCode")]
        public string AgeQualifyingCode { get; set; }
        [XmlAttribute(AttributeName = "Count")]
        public string Count { get; set; }
        [XmlAttribute(AttributeName = "Age")]
        public string Age { get; set; }
    }

    [XmlRoot(ElementName = "GuestCounts", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class GuestCounts
    {
        [XmlElement(ElementName = "GuestCount", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<GuestCount> GuestCount { get; set; }
    }

    [XmlRoot(ElementName = "TimeSpan", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class TimeSpan
    {
        [XmlAttribute(AttributeName = "End")]
        public string End { get; set; }
        [XmlAttribute(AttributeName = "Start")]
        public string Start { get; set; }

    }

    [XmlRoot(ElementName = "Taxes", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Taxes
    {
        [XmlAttribute(AttributeName = "Amount")]
        public string Amount { get; set; }
        [XmlAttribute(AttributeName = "CurrencyCode")]
        public string CurrencyCode { get; set; }
    }

    [XmlRoot(ElementName = "Total", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Total
    {
        [XmlElement(ElementName = "Taxes", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Taxes Taxes { get; set; }
        [XmlAttribute(AttributeName = "AmountBeforeTax")]
        public string AmountBeforeTax { get; set; }
        [XmlAttribute(AttributeName = "CurrencyCode")]
        public string CurrencyCode { get; set; }
    }

    [XmlRoot(ElementName = "BasicPropertyInfo", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class BasicPropertyInfo
    {
        [XmlAttribute(AttributeName = "HotelCode")]
        public string HotelCode { get; set; }
    }

    [XmlRoot(ElementName = "Comment")]
    public class Comment
    {
        [XmlElement(ElementName = "Text")]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Comments")]
    public class Comments
    {
        [XmlElement(ElementName = "Comment")]
        public Comment Comment { get; set; }
    }

    [XmlRoot(ElementName = "RoomStay", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RoomStay
    {
        [XmlElement(ElementName = "RoomTypes", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RoomTypes RoomTypes { get; set; }
        [XmlElement(ElementName = "RatePlans", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RatePlans RatePlans { get; set; }
        [XmlElement(ElementName = "GuestCounts", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public GuestCounts GuestCounts { get; set; }
        [XmlElement(ElementName = "TimeSpan", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public TimeSpan TimeSpan { get; set; }
        [XmlElement(ElementName = "Total", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Total Total { get; set; }
        [XmlElement(ElementName = "BasicPropertyInfo", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public BasicPropertyInfo BasicPropertyInfo { get; set; }
        [XmlElement(ElementName = "Comments", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Comments Comments { get; set; }
    }

    [XmlRoot(ElementName = "RoomStays", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RoomStays
    {
        [XmlElement(ElementName = "RoomStay", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RoomStay RoomStay { get; set; }
    }

    [XmlRoot(ElementName = "PersonName", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class PersonName
    {
        [XmlElement(ElementName = "GivenName", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string GivenName { get; set; }
        [XmlElement(ElementName = "Surname", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string Surname { get; set; }
    }

    [XmlRoot(ElementName = "Telephone", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Telephone
    {
        [XmlAttribute(AttributeName = "CountryAccessCode")]
        public string CountryAccessCode { get; set; }
        [XmlAttribute(AttributeName = "PhoneNumber")]
        public string PhoneNumber { get; set; }
        [XmlAttribute(AttributeName = "PhoneTechType")]
        public string PhoneTechType { get; set; }
    }

    [XmlRoot(ElementName = "StateProv", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class StateProv
    {
        [XmlAttribute(AttributeName = "StateCode")]
        public string StateCode { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "CountryName", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class CountryName
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Address", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Address
    {
        [XmlElement(ElementName = "AddressLine", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public List<string> AddressLine { get; set; }
        [XmlElement(ElementName = "CityName", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string CityName { get; set; }
        [XmlElement(ElementName = "PostalCode", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string PostalCode { get; set; }
        [XmlElement(ElementName = "StateProv", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public StateProv StateProv { get; set; }
        [XmlElement(ElementName = "CountryName", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public CountryName CountryName { get; set; }
    }

    [XmlRoot(ElementName = "Customer", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Customer
    {
        [XmlElement(ElementName = "PersonName", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public PersonName PersonName { get; set; }
        [XmlElement(ElementName = "Telephone", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Telephone Telephone { get; set; }
        [XmlElement(ElementName = "Email", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string Email { get; set; }
        [XmlElement(ElementName = "Address", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Address Address { get; set; }
    }

    [XmlRoot(ElementName = "Profile", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Profile
    {
        [XmlElement(ElementName = "Customer", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Customer Customer { get; set; }
        [XmlAttribute(AttributeName = "ProfileType")]
        public string ProfileType { get; set; }
    }

    [XmlRoot(ElementName = "ProfileInfo", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class ProfileInfo
    {
        [XmlElement(ElementName = "Profile", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Profile Profile { get; set; }
    }

    [XmlRoot(ElementName = "Profiles", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Profiles
    {
        [XmlElement(ElementName = "ProfileInfo", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public ProfileInfo ProfileInfo { get; set; }
    }

    [XmlRoot(ElementName = "ResGuest", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class ResGuest
    {
        [XmlElement(ElementName = "Profiles", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Profiles Profiles { get; set; }
    }

    [XmlRoot(ElementName = "ResGuests", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class ResGuests
    {
        [XmlElement(ElementName = "ResGuest", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public ResGuest ResGuest { get; set; }
    }

    [XmlRoot(ElementName = "Guarantee", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Guarantee
    {
        [XmlAttribute(AttributeName = "GuaranteeType")]
        public string GuaranteeType { get; set; }
    }

    [XmlRoot(ElementName = "ResGlobalInfo", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class ResGlobalInfo
    {
        [XmlElement(ElementName = "Guarantee", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Guarantee Guarantee { get; set; }
    }

    [XmlRoot(ElementName = "HotelReservation", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class HotelReservation
    {
        [XmlElement(ElementName = "RoomStays", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RoomStays RoomStays { get; set; }
        [XmlElement(ElementName = "ResGuests", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public ResGuests ResGuests { get; set; }
        [XmlElement(ElementName = "ResGlobalInfo", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public ResGlobalInfo ResGlobalInfo { get; set; }
    }

    [XmlRoot(ElementName = "HotelReservations", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class HotelReservations
    {
        [XmlElement(ElementName = "HotelReservation", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public HotelReservation HotelReservation { get; set; }
    }

    [XmlRoot(ElementName = "OTA_HotelResRQ", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class OTA_HotelResRQ
    {
        [XmlElement(ElementName = "POS", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public POS POS { get; set; }
        [XmlElement(ElementName = "UniqueID", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public UniqueID UniqueID { get; set; }
        [XmlElement(ElementName = "HotelReservations", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public HotelReservations HotelReservations { get; set; }
        [XmlAttribute(AttributeName = "CorrelationID")]
        public string CorrelationID { get; set; }
        [XmlAttribute(AttributeName = "Version")]
        public string Version { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }
    #endregion

    #region Provisional Booking Response
    [XmlRoot(ElementName = "Error", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Error
    {
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "Errors", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class Errors
    {
        [XmlElement(ElementName = "Error", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Error Error { get; set; }
    }

    [XmlRoot(ElementName = "UniqueID", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class UniqueIDRes
    {
        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "ID")]
        public string ID { get; set; }
    }

    [XmlRoot(ElementName = "Total", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class TotalPBRes
    {
        [XmlAttribute(AttributeName = "AmountAfterTax")]
        public string AmountAfterTax { get; set; }
        [XmlAttribute(AttributeName = "CurrencyCode")]
        public string CurrencyCode { get; set; }
    }

    [XmlRoot(ElementName = "RoomStay", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RoomStayPBRes
    {
        [XmlElement(ElementName = "Total", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public TotalPBRes Total { get; set; }
    }

    [XmlRoot(ElementName = "RoomStays", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class RoomStaysPBRes
    {
        [XmlElement(ElementName = "RoomStay", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RoomStayPBRes RoomStay { get; set; }
    }

    [XmlRoot(ElementName = "HotelReservation", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class HotelReservationPBRes
    {
        [XmlElement(ElementName = "UniqueID", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public UniqueIDRes UniqueID { get; set; }
        [XmlElement(ElementName = "RoomStays", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public RoomStaysPBRes RoomStays { get; set; }
    }

    [XmlRoot(ElementName = "HotelReservations", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class HotelReservationsPBRes
    {
        [XmlElement(ElementName = "HotelReservation", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public HotelReservationPBRes HotelReservation { get; set; }
    }

    [XmlRoot(ElementName = "OTA_HotelResRS", Namespace = "http://www.opentravel.org/OTA/2003/05")]
    public class OTA_HotelResRSPBRes
    {
        [XmlElement(ElementName = "Errors", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public Errors Errors { get; set; }
        [XmlElement(ElementName = "Success", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public string Success { get; set; }
        [XmlElement(ElementName = "HotelReservations", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public HotelReservationsPBRes HotelReservations { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "CorrelationID")]
        public string CorrelationID { get; set; }
        [XmlAttribute(AttributeName = "TransactionIdentifier")]
        public string TransactionIdentifier { get; set; }
    }

    [XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class BodyPBRes
    {
        [XmlElement(ElementName = "OTA_HotelResRS", Namespace = "http://www.opentravel.org/OTA/2003/05")]
        public OTA_HotelResRSPBRes OTA_HotelResRS { get; set; }
    }

    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class EnvelopePBRes
    {
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public BodyPBRes Body { get; set; }
        [XmlAttribute(AttributeName = "soap", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Soap { get; set; }
    }
    #endregion
}