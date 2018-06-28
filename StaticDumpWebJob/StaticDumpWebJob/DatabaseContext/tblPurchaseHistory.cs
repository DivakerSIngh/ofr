namespace StaticDumpWebJob.DatabaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblPurchaseHistory")]
    public partial class tblPurchaseHistory
    {
        [Key]
        public long iPurchaseId { get; set; }

        public long iCustomerId { get; set; }

        public long iBookingId { get; set; }

        [StringLength(50)]
        public string SKUCode { get; set; }

        public DateTime? PAYMENTDATE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ORDERNO { get; set; }

        [StringLength(50)]
        public string ORDERSTATUS { get; set; }

        public decimal? Amount { get; set; }

        public DateTime? SALEDATE { get; set; }

        public DateTime? COMPLETE_DATE { get; set; }

        [StringLength(50)]
        public string REFNO { get; set; }

        [Required]
        [StringLength(50)]
        public string REFNOEXT { get; set; }

        [StringLength(50)]
        public string PAYMETHOD { get; set; }

        [StringLength(50)]
        public string PAYMETHOD_CODE { get; set; }

        [StringLength(50)]
        public string CARD_TYPE { get; set; }

        [StringLength(4)]
        public string CARD_LAST_DIGITS { get; set; }

        [StringLength(10)]
        public string CHARGEBACK_RESOLUTION { get; set; }

        [StringLength(50)]
        public string AVANGATE_CUSTOMER_REFERENCE { get; set; }

        [StringLength(50)]
        public string EXTERNAL_CUSTOMER_REFERENCE { get; set; }

        [StringLength(15)]
        public string IPADDRESS { get; set; }

        [StringLength(50)]
        public string IPCOUNTRY { get; set; }

        [StringLength(50)]
        public string IPN_LICENSE_REF { get; set; }

        public int? IPN_PID { get; set; }

        [StringLength(15)]
        public string IPN_DATE { get; set; }

        [StringLength(15)]
        public string ResponseIssuingDate { get; set; }

        [StringLength(500)]
        public string HASH { get; set; }

        [StringLength(50)]
        public string ReadReceiptHASH { get; set; }

        [StringLength(50)]
        public string FIRSTNAME { get; set; }

        [StringLength(50)]
        public string LASTNAME { get; set; }

        [StringLength(50)]
        public string ADDRESS1 { get; set; }

        [StringLength(50)]
        public string ADDRESS2 { get; set; }

        [StringLength(50)]
        public string CITY { get; set; }

        [StringLength(50)]
        public string STATE { get; set; }

        [StringLength(50)]
        public string ZIPCODE { get; set; }

        [StringLength(50)]
        public string COUNTRY { get; set; }

        [StringLength(50)]
        public string PHONE { get; set; }

        [StringLength(50)]
        public string FAX { get; set; }

        [StringLength(50)]
        public string CUSTOMEREMAIL { get; set; }

        [StringLength(50)]
        public string FIRSTNAME_D { get; set; }

        [StringLength(50)]
        public string LASTNAME_D { get; set; }

        [StringLength(50)]
        public string ADDRESS1_D { get; set; }

        [StringLength(50)]
        public string ADDRESS2_D { get; set; }

        [StringLength(50)]
        public string CITY_D { get; set; }

        [StringLength(50)]
        public string STATE_D { get; set; }

        [StringLength(50)]
        public string ZIPCODE_D { get; set; }

        [StringLength(50)]
        public string COUNTRY_D { get; set; }

        [StringLength(50)]
        public string PHONE_D { get; set; }

        [StringLength(50)]
        public string EMAIL_D { get; set; }

        [StringLength(50)]
        public string TIMEZONE_OFFSET { get; set; }

        [StringLength(50)]
        public string CURRENCY { get; set; }

        public string MISC { get; set; }
    }
}
