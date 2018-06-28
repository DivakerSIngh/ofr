using OneFineRateBLL.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OneFineRateBLL
{
    public class BL_Invoice
    {
        public static eInvoiceModel GetInvoiceDetailByBooking(long bookingId)
        {
            try
            {
                var invoiceDetail = new eInvoiceModel();
                var bookingRooms = new List<eBookingRoomM>();
                var bookingRatePlan = new List<eBookingRatePlan>();

                SqlParameter[] sqlParams = new SqlParameter[1];
                sqlParams[0] = new SqlParameter("@bookingid", bookingId);
                DataSet ds = SqlHelper.ExecuteDataset(ConfigurationManager.AppSettings["ConnectionString"].ToString(), CommandType.StoredProcedure, "[uspGetGuestInvoiceDetailsByBookingId]", sqlParams);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    invoiceDetail.iPropId = Convert.ToInt32(ds.Tables[0].Rows[0]["iPropId"]);
                    invoiceDetail.iBookingId = Convert.ToString(ds.Tables[0].Rows[0]["iBookingId"]);
                    invoiceDetail.dtReservationDate = Convert.ToString(ds.Tables[0].Rows[0]["dtReservationDate"]);
                    invoiceDetail.sBookingStatus = Convert.ToString(ds.Tables[0].Rows[0]["sBookingStatus"]);
                    invoiceDetail.CheckIn = Convert.ToString(ds.Tables[0].Rows[0]["dtCheckIn"]);
                    invoiceDetail.CheckOut = Convert.ToString(ds.Tables[0].Rows[0]["dtChekOut"]);
                    invoiceDetail.sBooker = Convert.ToString(ds.Tables[0].Rows[0]["sBooker"]);
                    invoiceDetail.sEmailOFR = Convert.ToString(ds.Tables[0].Rows[0]["sEmailOFR"]);
                    invoiceDetail.sMobileOFR = Convert.ToString(ds.Tables[0].Rows[0]["sMobileOFR"]);
                    invoiceDetail.sHotelName = Convert.ToString(ds.Tables[0].Rows[0]["sHotelName"]);
                    invoiceDetail.sStreetAddress = Convert.ToString(ds.Tables[0].Rows[0]["sStreetAddress"]);
                    invoiceDetail.sAddress = Convert.ToString(ds.Tables[0].Rows[0]["sAddress"]);

                    invoiceDetail.dTotalAmount = Convert.ToString(ds.Tables[0].Rows[0]["dTotalAmount"]);
                    invoiceDetail.dTotalAmountPayable = Convert.ToString(ds.Tables[0].Rows[0]["dTotalAmountPayable"]);
                    invoiceDetail.sTotalAmountPayableInWords = Convert.ToString(ds.Tables[0].Rows[0]["sTotalAmountPayableInWords"]);
                    invoiceDetail.dAccomodationChargeIncludingTax = Convert.ToString(ds.Tables[0].Rows[0]["dAccomodationChargeIncludingTax"]);

                    invoiceDetail.dTaxes = Convert.ToString(ds.Tables[0].Rows[0]["dTaxes"]);                   
                 
                    invoiceDetail.dCommission = Convert.ToString(ds.Tables[0].Rows[0]["dCommission"]);
                    invoiceDetail.dGstOnCommission = Convert.ToString(ds.Tables[0].Rows[0]["dGstOnCommission"]);                   
                    invoiceDetail.dGstOnCommissionPercent = Convert.ToString(ds.Tables[0].Rows[0]["dGstOnCommissionPercent"]);
                    invoiceDetail.dTotalCommission = Convert.ToString(ds.Tables[0].Rows[0]["dTotalCommission"]);
                    invoiceDetail.sTotalCommissionInWords = Convert.ToString(ds.Tables[0].Rows[0]["sTotalCommissionInWords"]);
                    invoiceDetail.Customer_GST_Same_State = Convert.ToBoolean(ds.Tables[0].Rows[0]["Customer_GST_Same_State"]);

                    invoiceDetail.Hotel_GST_Same_State = Convert.ToBoolean(ds.Tables[0].Rows[0]["Hotel_GST_Same_State"]);

                    invoiceDetail.dServiceCharge = Convert.ToString(ds.Tables[0].Rows[0]["dServiceCharge"]);
                    invoiceDetail.dGstOnServiceCharge = Convert.ToString(ds.Tables[0].Rows[0]["dGstOnServiceCharge"]);
                    invoiceDetail.dGstOnServiceChargePercent = Convert.ToString(ds.Tables[0].Rows[0]["dGstOnServiceChargePercent"]);

                    invoiceDetail.sOfrDiscount = Convert.ToString(ds.Tables[0].Rows[0]["sOfrDiscount"]);

                    invoiceDetail.sSymbol = Convert.ToString(ds.Tables[0].Rows[0]["sSymbol"]);
                    invoiceDetail.sCurrencyCode = Convert.ToString(ds.Tables[0].Rows[0]["sCurrencyCode"]);
                    invoiceDetail.sCity = Convert.ToString(ds.Tables[0].Rows[0]["sCity"]);
                    invoiceDetail.cBookingType = Convert.ToString(ds.Tables[0].Rows[0]["cBookingType"]);
                    invoiceDetail.NoOfRooms = Convert.ToInt32(ds.Tables[0].Rows[0]["iNoOfRooms"]);

                    invoiceDetail.sCustomerGSTIN = Convert.ToString(ds.Tables[0].Rows[0]["sGstnNumber"]);
                    invoiceDetail.sCustomerGSTINEntityType = Convert.ToString(ds.Tables[0].Rows[0]["sGstnEntityType"]);
                    invoiceDetail.sCustomerGSTINEntityName = Convert.ToString(ds.Tables[0].Rows[0]["sGstnEntityName"]);
                    invoiceDetail.sCustomerGSTINPlaceOfSupply = Convert.ToString(ds.Tables[0].Rows[0]["sGstnPlaceOfSupply"]);
                    invoiceDetail.sCustomerAddress = Convert.ToString(ds.Tables[0].Rows[0]["sCustomerAddress"]);

                    invoiceDetail.sHotelGSTIN = Convert.ToString(ds.Tables[0].Rows[0]["sHotelGstnNumber"]);
                    invoiceDetail.sHotelGSTINEntityType = Convert.ToString(ds.Tables[0].Rows[0]["sHotelGstnEntityType"]);
                    invoiceDetail.sHotelGSTINEntityName = Convert.ToString(ds.Tables[0].Rows[0]["sHotelGstnEntityName"]);
                    invoiceDetail.sHotelGSTINPlaceOfSupply = Convert.ToString(ds.Tables[0].Rows[0]["sHotelGstnPlaceOfSupply"]);
                    invoiceDetail.sHotelAddress = Convert.ToString(ds.Tables[0].Rows[0]["sHotelAddress"]);

                    invoiceDetail.cBookingStatus = Convert.ToString(ds.Tables[0].Rows[0]["cBookingStatus"]);
                }

                eInvoiceSettingDetail invoiceSettingDetail = new eInvoiceSettingDetail();

                if (ds.Tables[1].Rows.Count > 0)
                {
                    invoiceSettingDetail.Bank_AC_OFR = Convert.ToString(ds.Tables[1].Rows[0]["Bank_AC_OFR"]);
                    invoiceSettingDetail.Bank_IFSC_OFR = Convert.ToString(ds.Tables[1].Rows[0]["Bank_IFSC_OFR"]);
                    invoiceSettingDetail.Bank_Name_OFR = Convert.ToString(ds.Tables[1].Rows[0]["Bank_Name_OFR"]);
                    invoiceSettingDetail.CIN_OFR = Convert.ToString(ds.Tables[1].Rows[0]["CIN_OFR"]);
                    invoiceSettingDetail.Footer_Address_OFR = Convert.ToString(ds.Tables[1].Rows[0]["Footer_Address_OFR"]);
                    invoiceSettingDetail.GSTIN_OFR = Convert.ToString(ds.Tables[1].Rows[0]["GSTIN_OFR"]);
                    invoiceSettingDetail.Invoice_Email_OFR = Convert.ToString(ds.Tables[1].Rows[0]["Invoice_Email_OFR"]);
                    invoiceSettingDetail.PAN_OFR = Convert.ToString(ds.Tables[1].Rows[0]["PAN_OFR"]);
                    invoiceSettingDetail.Place_Of_Supply_OFR = Convert.ToString(ds.Tables[1].Rows[0]["Place_Of_Supply_OFR"]);
                    invoiceSettingDetail.SAC_Code_OFR = Convert.ToString(ds.Tables[1].Rows[0]["SAC_Code_OFR"]);
                }

                invoiceDetail.InvoiceSettingDetail = invoiceSettingDetail;
                if (!string.IsNullOrEmpty(invoiceDetail.sCustomerGSTINEntityType))
                {
                    switch (invoiceDetail.sCustomerGSTINEntityType.ToLower())
                    {
                        case "p":
                            invoiceDetail.sCustomerGSTINEntityType = GSTEntityType.p;
                            break;
                        case "c":
                            invoiceDetail.sCustomerGSTINEntityType = GSTEntityType.c;
                            break;
                        case "h":
                            invoiceDetail.sCustomerGSTINEntityType = GSTEntityType.h;
                            break;
                        case "f":
                            invoiceDetail.sCustomerGSTINEntityType = GSTEntityType.f;
                            break;
                        case "a":
                            invoiceDetail.sCustomerGSTINEntityType = GSTEntityType.a;
                            break;
                        case "t":
                            invoiceDetail.sCustomerGSTINEntityType = GSTEntityType.t;
                            break;
                        case "b":
                            invoiceDetail.sCustomerGSTINEntityType = GSTEntityType.b;
                            break;
                        case "l":
                            invoiceDetail.sCustomerGSTINEntityType = GSTEntityType.l;
                            break;
                        case "j":
                            invoiceDetail.sCustomerGSTINEntityType = GSTEntityType.j;
                            break;
                        case "g":
                            invoiceDetail.sCustomerGSTINEntityType = GSTEntityType.g;
                            break;

                        default:
                            break;
                    }
                }
                return invoiceDetail;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public static class GSTEntityType
    {
        public const string p = "Person/Individual";
        public const string c = "Company";
        public const string h = "HUF";

        public const string f = "Firm";
        public const string a = "Association of Person (AOP)";
        public const string t = "AOP (Trust)";


        public const string b = "Body of Individuals (BOI)";
        public const string l = "Local Authorithy";
        public const string j = "Artifical Juridical Person";

        public const string g = "Government";
    }
}