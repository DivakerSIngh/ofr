using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OneFineRateWeb.Models
{
    public class PayuPaymentModel
    {
        [Required]
        public string key { get; set; }

        [Required]
        public string txnid { get; set; }

        [Required]
        public decimal amount { get; set; }

        [Required]
        public string productinfo { get; set; }

        [Required]
        [StringLength(60)]
        public string firstname { get; set; }

        [Required]
        [StringLength(50)]
        public string email { get; set; }

        [Required]
        [StringLength(50)]
        public string phone { get; set; }

        [StringLength(20)]
        public string lastname { get; set; }

        [StringLength(100)]
        public string address1 { get; set; }

        [StringLength(100)]
        public string address2 { get; set; }

        [StringLength(50)]
        public string city { get; set; }

        [StringLength(50)]
        public string state { get; set; }

        [StringLength(50)]
        public string country { get; set; }

        [StringLength(20)]
        public string zipcode { get; set; }

        [StringLength(255)]
        public string udf1 { get; set; }

        [StringLength(255)]
        public string udf2 { get; set; }

        [StringLength(255)]
        public string udf3 { get; set; }

        [StringLength(255)]
        public string udf4 { get; set; }

        [StringLength(255)]
        public string udf5 { get; set; }

        public string surl { get; set; }

        public string furl { get; set; }

        public string curl { get; set; }

        [Required]
        public string hash { get; set; }

        public string codurl { get; set; }

        public string drop_category { get; set; }

        public string enforce_paymethod { get; set; }

        public string salt { get; set; }

        public string custom_note { get; set; }


        #region seamless Integration

        [Required]
        /// <summary>
        /// This parameter is the same as the one mentioned in the POST Parameters mentioned above. It must be set as the payment category.  
        /// Please set its value to ‘NB’ for Net Banking , ‘CC’ for Credit Card , ‘DC’ for Debit Card , ‘CASH’ for Cash Card and ‘EMI’ for EMI 
        /// </summary>
        public string pg { get; set; }

        [Required]
        /// <summary>
        /// Each payment option is identified with a unique bank code at PayU. You would need to post this parameter with the corresponding payment option’s bankcode value in it.  
        /// For example, for ICICI Net Banking, the value of bankcode parameter value should be ICIB. For detailed list of bank codes, please contact PayU team 
        /// </summary>
        public string bankcode { get; set; }

        [Required]
        /// <summary>
        /// This parameter must contain the card (credit/debit) number entered by the customer for the transaction. 
        /// </summary>
        public string ccnum { get; set; }

        [Required]
        /// <summary>
        /// This parameter must contain the name on card – as entered by the customer for the transaction. 
        /// </summary>
        public string ccname { get; set; }

        [Required]
        /// <summary>
        /// This parameter must contain the cvv number of the card – as entered by the customer for the transaction. 
        /// </summary>
        public string ccvv { get; set; }

        [Required]
        /// <summary>
        /// This parameter must contain the card’s expiry month - as entered by the customer for the transaction. Please make sure that this is always in 2 digits.
        /// For months 1-9, this parameter must be appended with 0 – like 01, 02…09. For months 10-12, this parameter must not be appended – It should be 10, 11 and 12 respectively. 
        /// </summary>
        public string ccexpmon { get; set; }

        [Required]
        /// <summary>
        /// The customer must contain the card’s expiry year – as entered by the customer for the transaction. It must be of 4 digits. For example - 2017, 2029 etc. 
        /// </summary>
        public string ccexpyr { get; set; }

        #endregion
    }



    public enum DropCategory
    {
        [Description("CreditCard")]
        CC, // MasterCard, Amex, Diners etc 

        [Description("DebitCard")]
        DC, //Visa, Mastercard, Maestro etc 

        [Description("NB")]
        NetBanking, //SBI Net Banking, HDFC Net Banking etc 

        [Description("EMI")]
        EMI, //3 Months EMI, HDFC 6 Months EMI etc 

        [Description("CashCard")]
        CASH// AirtelMoney, YPay, ITZ Cash card etc 
    }

    public enum PG
    {
        NB,// for Net Banking tab,
        CC,// for Credit Card tab, 
        DC,// for Debit Card tab,
        CASH,// for Cash Card tab,
        EMI, //for EMI tab
        //Note: PG = CC, i.e. Credit Card tab is recommended. If PG is left empty, CC will be taken as default.
    }


    #region This parameter has to used in case of COD (Cash on Delivery) Only

    //public string ShippingAddress1 { get; set; }
    //public string ShippingAddress2 { get; set; }
    //public string ShippingCity { get; set; }
    //public string ShippingState { get; set; }

    //public string ShippingCountry { get; set; }

    ///// <summary>
    ///// If this parameter is posted, the corresponding value would be filled up automatically in the form under COD tab on PayU payment page 
    ///// </summary>
    //public string ShippingZipcode { get; set; }

    ///// <summary>
    ///// If this parameter is posted, the corresponding value would be filled up automatically in the form under COD tab on PayU payment page 
    ///// </summary>
    //public string ShippingPhone { get; set; }

    ///// <summary>
    ///// This parameter is useful when the merchant wants to give the customer a discount offer on certain transactions based upon a pre-defined combination. 
    ///// This combination can be based upon payment options/bins etc. For each new offer created, a unique offer_key is generated. At the time of a transaction, this offer_key needs to be posted by the merchant. 
    ///// </summary>
    //public string OfferKey { get; set; }

    #endregion End This parameter has to used in case of COD (Cash on Delivery) Only


}