﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneFineRateBLL.Entities
{
    public class eTblPurchaseHistory
    {
        public long iPurchaseId { get; set; }
        public long iCustomerId { get; set; }
        public long iBookingId { get; set; }
        public string SKUCode { get; set; }
        public Nullable<System.DateTime> PAYMENTDATE { get; set; }
        public Nullable<decimal> ORDERNO { get; set; }
        public string ORDERSTATUS { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.DateTime> SALEDATE { get; set; }
        public Nullable<System.DateTime> COMPLETE_DATE { get; set; }
        public string REFNO { get; set; }
        public string REFNOEXT { get; set; }
        public string PAYMETHOD { get; set; }
        public string PAYMETHOD_CODE { get; set; }
        public string CARD_TYPE { get; set; }
        public string CARD_LAST_DIGITS { get; set; }
        public string CHARGEBACK_RESOLUTION { get; set; }
        public string AVANGATE_CUSTOMER_REFERENCE { get; set; }
        public string EXTERNAL_CUSTOMER_REFERENCE { get; set; }
        public string IPADDRESS { get; set; }
        public string IPCOUNTRY { get; set; }
        public string IPN_LICENSE_REF { get; set; }
        public Nullable<int> IPN_PID { get; set; }
        public string IPN_DATE { get; set; }
        public string ResponseIssuingDate { get; set; }
        public string HASH { get; set; }
        public string ReadReceiptHASH { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string ADDRESS1 { get; set; }
        public string ADDRESS2 { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string ZIPCODE { get; set; }
        public string COUNTRY { get; set; }
        public string PHONE { get; set; }
        public string FAX { get; set; }
        public string CUSTOMEREMAIL { get; set; }
        public string FIRSTNAME_D { get; set; }
        public string LASTNAME_D { get; set; }
        public string ADDRESS1_D { get; set; }
        public string ADDRESS2_D { get; set; }
        public string CITY_D { get; set; }
        public string STATE_D { get; set; }
        public string ZIPCODE_D { get; set; }
        public string COUNTRY_D { get; set; }
        public string PHONE_D { get; set; }
        public string EMAIL_D { get; set; }
        public string TIMEZONE_OFFSET { get; set; }
        public string CURRENCY { get; set; }
        public string MISC { get; set; }
    }
}