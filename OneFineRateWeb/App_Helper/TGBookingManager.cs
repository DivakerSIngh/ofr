using OneFineRateBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneFineRateWeb.TGBookingService;
using OneFineRateBLL.Entities;
using System.Xml.Serialization;

namespace OneFineRateWeb.Handlers
{

    public class TGBookingManager
    {
        public static TG_ProvisionalBookingResponseModel ProvisionalBooking(TG_ProvisionalBookingRequestModel model)
        {
            TGBookingService.TGBookingServiceEndPointImplService bookingService = new TGBookingService.TGBookingServiceEndPointImplService();

            TGBookingService.OTA_HotelResRQ hotelResRQ = new TGBookingService.OTA_HotelResRQ();

            hotelResRQ.CorrelationID = model.CorrelationID;

            hotelResRQ.POS = new TGBookingService.SourceType[1];

            hotelResRQ.POS[0] = new TGBookingService.SourceType();
            hotelResRQ.POS[0].ISOCurrency = model.Currency;
            hotelResRQ.POS[0].RequestorID = new TGBookingService.SourceTypeRequestorID();
            hotelResRQ.POS[0].RequestorID.ID = model.API_Credential_PropertyId;
            hotelResRQ.POS[0].RequestorID.MessagePassword = model.API_Credential_Password;
            hotelResRQ.POS[0].RequestorID.CompanyName = new TGBookingService.CompanyNameType();
            hotelResRQ.POS[0].RequestorID.CompanyName.Code = model.API_Credential_UserName;

            hotelResRQ.UniqueID = new TGBookingService.UniqueID_Type[1];
            hotelResRQ.UniqueID[0] = new TGBookingService.UniqueID_Type();
            hotelResRQ.UniqueID[0].Type = model.UniqueIdType;
            hotelResRQ.UniqueID[0].ID = model.UniqueId;

            hotelResRQ.HotelReservations = new TGBookingService.HotelReservationsType();


            for (int h = 0; h < 1; h++)
            {
                hotelResRQ.HotelReservations.HotelReservation = new TGBookingService.HotelReservationsTypeHotelReservation[1];
                hotelResRQ.HotelReservations.HotelReservation[h] = new TGBookingService.HotelReservationsTypeHotelReservation();


                for (int rs = 0; rs < 1; rs++)
                {
                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays = new TGBookingService.RoomStaysTypeRoomStay[1];

                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays[0] = new TGBookingService.RoomStaysTypeRoomStay();

                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays[0].RoomTypes = new TGBookingService.RoomTypeType[1];
                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays[0].RoomTypes[0] = new TGBookingService.RoomTypeType();
                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays[0].RoomTypes[0].NumberOfUnits = model.NumberOfRooms;
                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays[0].RoomTypes[0].RoomTypeCode = model.RoomTypeCode;

                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays[0].RatePlans = new TGBookingService.RatePlanType[1];
                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays[0].RatePlans[0] = new TGBookingService.RatePlanType();
                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays[0].RatePlans[0].RatePlanCode = model.RatePlanCode;


                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays[0].GuestCounts = new TGBookingService.GuestCountType();
                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays[0].GuestCounts.IsPerRoom = false;
                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays[0].GuestCounts.IsPerRoomSpecified = true;


                    List<TGBookingService.GuestCountTypeGuestCount> guestCountList = new List<TGBookingService.GuestCountTypeGuestCount>();

                    foreach (var room in model.RoomData)
                    {
                        var adultDetail = new TGBookingService.GuestCountTypeGuestCount();
                        adultDetail.AgeQualifyingCode = "10";
                        adultDetail.Count = room.adult;
                        adultDetail.CountSpecified = true;
                        adultDetail.ResGuestRPH = room.room.ToString();
                        guestCountList.Add(adultDetail);

                        foreach (var child in room.ChildAge)
                        {
                            var childDetail = new TGBookingService.GuestCountTypeGuestCount();
                            childDetail.AgeQualifyingCode = "8";
                            childDetail.AgeSpecified = true;
                            childDetail.Count = 1;
                            childDetail.CountSpecified = true;
                            childDetail.Age = int.Parse(child.Age);
                            childDetail.ResGuestRPH = room.room.ToString();
                            guestCountList.Add(childDetail);
                        }
                    }

                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays[0].GuestCounts.GuestCount = guestCountList.ToArray();


                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays[0].TimeSpan = new TGBookingService.DateTimeSpanType();
                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays[0].TimeSpan.Start = model.CheckIn;
                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays[0].TimeSpan.End = model.CheckOut;

                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays[0].Total = new TGBookingService.TotalType();
                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays[0].Total.AmountBeforeTaxSpecified = true;
                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays[0].Total.AmountBeforeTax = decimal.Parse(model.AmountBeforeTax) + decimal.Parse(model.ExtraBedCharge);
                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays[0].Total.CurrencyCode = model.Currency;
                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays[0].Total.Taxes = new TGBookingService.TaxesType();
                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays[0].Total.Taxes.Amount = decimal.Parse(model.TaxAmount);
                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays[0].Total.Taxes.AmountSpecified = true;
                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays[0].Total.Taxes.CurrencyCode = model.Currency;
                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays[0].BasicPropertyInfo = new TGBookingService.BasicPropertyInfoType();
                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays[0].BasicPropertyInfo.HotelCode = model.HotelCode;

                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays[0].Comments = new TGBookingService.CommentTypeComment[1];
                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays[0].Comments[0] = new TGBookingService.CommentTypeComment();
                    hotelResRQ.HotelReservations.HotelReservation[h].RoomStays[0].Comments[0].Name = "Provisional Hotel Booking";

                }



                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests = new TGBookingService.ResGuestType[1];
                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0] = new TGBookingService.ResGuestType();
                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles = new TGBookingService.ProfilesTypeProfileInfo[1];
                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0] = new TGBookingService.ProfilesTypeProfileInfo();
                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile = new TGBookingService.ProfileType();
                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.ProfileType1 = model.ProfileType;
                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.Customer = new TGBookingService.CustomerType();

                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.Customer.PersonName = new TGBookingService.PersonNameType[1];
                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.Customer.PersonName[0] = new TGBookingService.PersonNameType();
                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.Customer.PersonName[0].NamePrefix = new string[] { model.CustomerNamePrefix };
                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.Customer.Email = new TGBookingService.CustomerTypeEmail[1];
                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.Customer.Email[0] = new TGBookingService.CustomerTypeEmail();
                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.Customer.Email[0].Value = model.CustomerEmail;
                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.Customer.PersonName[0].GivenName = new string[] { model.CustomerGivenName };
                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.Customer.PersonName[0].Surname = model.CustomerSurname;

                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.Customer.Telephone = new TGBookingService.CustomerTypeTelephone[1];
                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.Customer.Telephone[0] = new CustomerTypeTelephone();
                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.Customer.Telephone[0].CountryAccessCode = model.CountryAccessCode;
                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.Customer.Telephone[0].PhoneTechType = model.PhoneTechType;
                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.Customer.Telephone[0].PhoneNumber = model.PhoneNumber;
                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.Customer.Telephone[0].AreaCityCode = model.AreaCityCode;
                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.Customer.Telephone[0].DefaultInd = true;
                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.Customer.Telephone[0].DefaultIndSpecified = true;


                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.Customer.Address = new TGBookingService.CustomerTypeAddress[1];
                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.Customer.Address[0] = new TGBookingService.CustomerTypeAddress();
                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.Customer.Address[0].AddressLine = model.AddressLine.ToArray();
                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.Customer.Address[0].CityName = model.CityName;

                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.Customer.Address[0].PostalCode = model.PostalCode;

                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.Customer.Address[0].StateProv = new TGBookingService.StateProvType();
                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.Customer.Address[0].StateProv.StateCode = model.StateCode;
                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.Customer.Address[0].StateProv.Value = model.StateCode;
                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.Customer.Address[0].CountryName = new TGBookingService.CountryNameType();
                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.Customer.Address[0].CountryName.Code = model.CountryCode;
                hotelResRQ.HotelReservations.HotelReservation[h].ResGuests[0].Profiles[0].Profile.Customer.Address[0].CountryName.Value = model.CountryName;

                hotelResRQ.HotelReservations.HotelReservation[h].ResGlobalInfo = new TGBookingService.ResGlobalInfoType();
                hotelResRQ.HotelReservations.HotelReservation[h].ResGlobalInfo.Guarantee = new TGBookingService.GuaranteeType();
                hotelResRQ.HotelReservations.HotelReservation[h].ResGlobalInfo.Guarantee.GuaranteeType1 = model.GuaranteeType;

            }


            var response = bookingService.createBooking(hotelResRQ);

            TG_ProvisionalBookingResponseModel responseModel = null;

            if (response != null)
            {
                responseModel = new TG_ProvisionalBookingResponseModel();

                if (response.Errors != null && response.Errors.Count() > 0)
                {
                    responseModel.IsSuccedded = false;
                    var error = response.Errors.FirstOrDefault();
                    responseModel.ErrorCode = error.Code;
                    responseModel.ErrorMessage = error.ShortText;
                    responseModel.ErrorType = error.Type;
                }
                else if (response.HotelReservations != null && response.HotelReservations.HotelReservation != null)
                {
                    var reservation = response.HotelReservations.HotelReservation.FirstOrDefault();

                    var uniqueDetails = reservation.UniqueID.FirstOrDefault();

                    responseModel.UniqueId = uniqueDetails.ID;
                    responseModel.UniqueIdType = uniqueDetails.Type;
                    responseModel.CorrelationID = response.CorrelationID;
                    responseModel.IsSuccedded = true;
                }
            }

            //var requestXml = ToXML(hotelResRQ);
            //var responseXml = ToXML(response);

            return responseModel;
        }

        /// <summary>
        /// TG final booking 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static TG_FinalBookingResponseModel FinalBooking(TG_FinalBookingRequestModel model)
        {
            TGBookingService.TGBookingServiceEndPointImplService bookingService = new TGBookingService.TGBookingServiceEndPointImplService();

            TGBookingService.OTA_HotelResRQ hotelResRQ = new TGBookingService.OTA_HotelResRQ();

            hotelResRQ.CorrelationID = model.CorrelationID;

            hotelResRQ.POS = new TGBookingService.SourceType[1];

            hotelResRQ.POS[0] = new TGBookingService.SourceType();
            hotelResRQ.POS[0].ISOCurrency = model.Currency;
            hotelResRQ.POS[0].RequestorID = new TGBookingService.SourceTypeRequestorID();
            hotelResRQ.POS[0].RequestorID.ID = model.API_Credential_PropertyId;
            hotelResRQ.POS[0].RequestorID.MessagePassword = model.API_Credential_Password;
            hotelResRQ.POS[0].RequestorID.CompanyName = new TGBookingService.CompanyNameType();
            hotelResRQ.POS[0].RequestorID.CompanyName.Code = model.API_Credential_UserName;

            hotelResRQ.UniqueID = new TGBookingService.UniqueID_Type[1];
            hotelResRQ.UniqueID[0] = new TGBookingService.UniqueID_Type();
            hotelResRQ.UniqueID[0].Type = model.UniqueIdType;
            hotelResRQ.UniqueID[0].ID = model.UniqueId;

            hotelResRQ.HotelReservations = new TGBookingService.HotelReservationsType();
            hotelResRQ.HotelReservations.HotelReservation = new TGBookingService.HotelReservationsTypeHotelReservation[1];
            hotelResRQ.HotelReservations.HotelReservation[0] = new TGBookingService.HotelReservationsTypeHotelReservation();
            hotelResRQ.HotelReservations.HotelReservation[0].ResGlobalInfo = new ResGlobalInfoType();
            hotelResRQ.HotelReservations.HotelReservation[0].ResGlobalInfo.Guarantee = new GuaranteeType();
            hotelResRQ.HotelReservations.HotelReservation[0].ResGlobalInfo.Guarantee.GuaranteeType1 = "PrePay";

            var xml = ToXML(hotelResRQ);

            var response = bookingService.createBooking(hotelResRQ);

            TG_FinalBookingResponseModel responseModel = null;

            if (response != null)
            {
                responseModel = new TG_FinalBookingResponseModel();

                if (response.Errors != null && response.Errors.Count() > 0)
                {
                    responseModel.IsSuccedded = false;
                    var error = response.Errors.FirstOrDefault();
                    responseModel.ErrorCode = error.Code;
                    responseModel.ErrorMessage = error.ShortText;
                    responseModel.ErrorType = error.Type;
                }
                else if (response.HotelReservations != null && response.HotelReservations.HotelReservation != null)
                {
                    var reservation = response.HotelReservations.HotelReservation.FirstOrDefault();

                    var uniqueDetails = reservation.UniqueID.FirstOrDefault();

                    responseModel.UniqueId = uniqueDetails.ID;
                    responseModel.UniqueIdType = uniqueDetails.Type;
                    responseModel.CorrelationID = response.CorrelationID;
                    responseModel.IsSuccedded = true;
                }
            }

            //var requestXml = ToXML(hotelResRQ);
            //var responseXml = ToXML(response);

            return responseModel;
        }

        public static TG_CancelBookingResponseModel InitiateCancelBooking(TG_CancelBookingRequestModel model)
        {
            TGBookingService.TGBookingServiceEndPointImplService bookingService = new TGBookingService.TGBookingServiceEndPointImplService();

            TGBookingService.OTA_CancelRQ cancelRQ = new TGBookingService.OTA_CancelRQ();
            cancelRQ.Version = 1;
            cancelRQ.CancelType = transactionActionType.Initiate;

            cancelRQ.POS = new TGBookingService.SourceType[1];

            cancelRQ.POS[0] = new TGBookingService.SourceType();
            cancelRQ.POS[0].RequestorID = new TGBookingService.SourceTypeRequestorID();
            cancelRQ.POS[0].RequestorID.ID = model.API_Credential_PropertyId;
            cancelRQ.POS[0].RequestorID.MessagePassword = model.API_Credential_Password;
            cancelRQ.POS[0].RequestorID.CompanyName = new TGBookingService.CompanyNameType();
            cancelRQ.POS[0].RequestorID.CompanyName.Code = model.API_Credential_UserName;

            cancelRQ.UniqueID = new TGBookingService.OTA_CancelRQUniqueID[1];
            cancelRQ.UniqueID[0] = new TGBookingService.OTA_CancelRQUniqueID();
            cancelRQ.UniqueID[0].ID = model.UniqueId;

            cancelRQ.Verification = new TGBookingService.VerificationType[1];
            cancelRQ.Verification[0] = new TGBookingService.VerificationType();
            cancelRQ.Verification[0].PersonName = new VerificationTypePersonName();
            cancelRQ.Verification[0].PersonName.GivenName = model.CustomerGivenNameArr;
            cancelRQ.Verification[0].PersonName.NamePrefix = model.CustomerNamePrefixArr;
            cancelRQ.Verification[0].PersonName.Surname = model.CustomerSurname;

            cancelRQ.Verification[0].Email = new EmailType();
            cancelRQ.Verification[0].Email.Value = model.CustomerEmail;

            cancelRQ.TPA_Extensions = new TPA_ExtensionsType();
            cancelRQ.TPA_Extensions.CancelDates = model.CancelDateArr;


            var xml = ToXML(cancelRQ);

            var apiResult = bookingService.cancelBooking(cancelRQ);

            var responseXml = ToXML(apiResult);

            TG_CancelBookingResponseModel responseModel = null;

            if (apiResult != null)
            {
                responseModel = new TG_CancelBookingResponseModel();
                responseModel.Status = apiResult.Status.ToString();

                if (apiResult.Errors != null && apiResult.Errors.Count() > 0)
                {
                    responseModel.IsSuccedded = false;
                    var error = apiResult.Errors.FirstOrDefault();
                    responseModel.ErrorCode = error.Code;
                    responseModel.ErrorMessage = error.ShortText;
                    responseModel.ErrorType = error.Type;
                    
                }
                else if (apiResult.CancelInfoRS != null && apiResult.CancelInfoRS.CancelRules != null)
                {
                    foreach (var rule in apiResult.CancelInfoRS.CancelRules)
                    {
                        responseModel.CancelRules.Add(new CancelRule { CancelByDate= rule.CancelByDate, Amount= rule.Amount });
                    }

                    if (apiResult.Comment != null)
                    {
                        responseModel.Comment = apiResult.Comment.Name;
                    }
                    responseModel.IsSuccedded = true;
                }
            }

            return responseModel;
        }

        public static TG_CancelBookingResponseModel CancelBooking(TG_CancelBookingRequestModel model)
        {
            TGBookingService.TGBookingServiceEndPointImplService bookingService = new TGBookingService.TGBookingServiceEndPointImplService();

            TGBookingService.OTA_CancelRQ cancelRQ = new TGBookingService.OTA_CancelRQ();

            cancelRQ.CancelType = transactionActionType.Cancel;

            cancelRQ.POS = new TGBookingService.SourceType[1];

            cancelRQ.POS[0] = new TGBookingService.SourceType();
            cancelRQ.POS[0].RequestorID = new TGBookingService.SourceTypeRequestorID();
            cancelRQ.POS[0].RequestorID.ID = model.API_Credential_PropertyId;
            cancelRQ.POS[0].RequestorID.MessagePassword = model.API_Credential_Password;
            cancelRQ.POS[0].RequestorID.CompanyName = new TGBookingService.CompanyNameType();
            cancelRQ.POS[0].RequestorID.CompanyName.Code = model.API_Credential_UserName;

            cancelRQ.UniqueID = new TGBookingService.OTA_CancelRQUniqueID[1];
            cancelRQ.UniqueID[0] = new TGBookingService.OTA_CancelRQUniqueID();
            cancelRQ.UniqueID[0].ID = model.UniqueId;

            cancelRQ.Verification = new TGBookingService.VerificationType[1];
            cancelRQ.Verification[0] = new TGBookingService.VerificationType();
            cancelRQ.Verification[0].PersonName = new VerificationTypePersonName();
            cancelRQ.Verification[0].PersonName.GivenName = model.CustomerGivenNameArr;
            cancelRQ.Verification[0].PersonName.NamePrefix = model.CustomerNamePrefixArr;
            cancelRQ.Verification[0].PersonName.Surname = model.CustomerSurname;

            cancelRQ.Verification[0].Email = new EmailType();
            cancelRQ.Verification[0].Email.Value = model.CustomerEmail;

            cancelRQ.TPA_Extensions = new TPA_ExtensionsType();
            cancelRQ.TPA_Extensions.CancelDates = model.CancelDateArr;


            var xml = ToXML(cancelRQ);

            var response = bookingService.cancelBooking(cancelRQ);

            var responseXml = ToXML(response);

            TG_CancelBookingResponseModel responseModel = null;

            if (response != null)
            {
                responseModel = new TG_CancelBookingResponseModel();

                if (response.Errors != null && response.Errors.Count() > 0)
                {
                    responseModel.IsSuccedded = false;
                    var error = response.Errors.FirstOrDefault();
                    responseModel.ErrorCode = error.Code;
                    responseModel.ErrorMessage = error.ShortText;
                    responseModel.ErrorType = error.Type;
                }
                else if (response.CancelInfoRS != null && response.CancelInfoRS.CancelRules != null)
                {
                    foreach (var rule in response.CancelInfoRS.CancelRules)
                    {

                    }

                    responseModel.IsSuccedded = true;
                }
            }

            return responseModel;
        }

        public static string ToXML(object rq)
        {
            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(rq.GetType());
            serializer.Serialize(stringwriter, rq);
            return stringwriter.ToString();
        }
    }
}