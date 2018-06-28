using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace OneFineRateWeb.Handlers
{
    public class PayuUtility
    {
        public static Hashtable Compute_HashTable_Using_HashSecquence(FormCollection collection)
        {
            string[] hashVarsSeq = ConfigurationManager.AppSettings["hashSequence"].Split('|');
            string hash_string = string.Empty;
            StringBuilder hashStringBuilder = new StringBuilder();

            Hashtable table = new Hashtable();

            foreach (string hash_var in hashVarsSeq)
            {
                hashStringBuilder.Append(collection[hash_var] + '|');
            }

            hashStringBuilder.Append(ConfigurationManager.AppSettings["SALT"]);

            hash_string = Generatehash512(hashStringBuilder.ToString());

            table.Add("hash", hash_string.ToLower());

            foreach (var key in collection.AllKeys)
            {
                table.Add(key, collection[key]);
            }

            return table;
        }


        public static Hashtable Reverse_And_Verify_Hash512_Using_HashSecquence(FormCollection collection)
        {
            string[] hashVarsSeq = ConfigurationManager.AppSettings["hashSequence"].Split('|');
            string hash_string = string.Empty;
            StringBuilder hashStringBuilder = new StringBuilder();

            Hashtable table = new Hashtable();

            foreach (string hash_var in hashVarsSeq)
            {
                hashStringBuilder.Append(collection[hash_var] + '|');
            }

            hashStringBuilder.Append(ConfigurationManager.AppSettings["SALT"]);

            hash_string = Generatehash512(hashStringBuilder.ToString());

            table.Add("hash", hash_string.ToLower());

            foreach (var key in collection.AllKeys)
            {
                table.Add(key, collection[key]);
            }

            return table;
        }




        public static string PreparePOSTForm(string url, Hashtable data)
        {
            //Set a name for the form
            string formID = "PostForm";
            //Build the form using the specified data to be posted.
            StringBuilder strForm = new StringBuilder();
            strForm.Append("<form id=\"" + formID + "\" name=\"" +
                           formID + "\" action=\"" + url +
                           "\" method=\"POST\">");

            foreach (System.Collections.DictionaryEntry key in data)
            {

                strForm.Append("<input type=\"hidden\" name=\"" + key.Key +
                               "\" value=\"" + key.Value + "\">");
            }


            strForm.Append("</form>");
            //Build the JavaScript which will do the Posting operation.
            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language='javascript'>");
            strScript.Append("var v" + formID + " = document." +
                             formID + ";");
            strScript.Append("v" + formID + ".submit();");
            strScript.Append("</script>");
            //Return the form and the script concatenated.
            //(The order is important, Form then JavaScript)
            return strForm.ToString() + strScript.ToString();
        }

        public static string ConvertToQueryString<T>(T obj)
        {
            var keyPairs = typeof(T).GetProperties().Select(p => new Pair(p.Name, p.GetValue(obj, null)));
            StringBuilder sb = new StringBuilder();
            foreach (var item in keyPairs)
            {
                sb.Append(String.Format("{0}={1}&", item.First, item.Second));
            }
            return "?" + sb.ToString().Substring(0, sb.Length - 1).ToLower(); //remove last &
        }

        public static string Generatehash512(string text)
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
    }

    public class PayUResponseModel
    {
        public string mihpayid { get; set; }
        public string mode { get; set; }
        public string status { get; set; }
        public string unmappedstatus { get; set; }
        public string key { get; set; }
        public string txnid { get; set; }
        public string amount { get; set; }
        public string cardCategory { get; set; }
        public string discount { get; set; }
        public string net_amount_debit { get; set; }
        public string addedon { get; set; }
        public string productinfo { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string zipcode { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string udf1 { get; set; }
        public string udf2 { get; set; }
        public string udf3 { get; set; }
        public string udf4 { get; set; }
        public string udf5 { get; set; }
        public string udf6 { get; set; }
        public string udf7 { get; set; }
        public string udf8 { get; set; }
        public string udf9 { get; set; }
        public string udf10 { get; set; }
        public string hash { get; set; }
        public string field1 { get; set; }
        public string field2 { get; set; }
        public string field3 { get; set; }
        public string field4 { get; set; }
        public string field5 { get; set; }
        public string field6 { get; set; }
        public string field7 { get; set; }
        public string field8 { get; set; }
        public string field9 { get; set; }
        public string payment_source { get; set; }
        public string PG_TYPE { get; set; }
        public string bank_ref_num { get; set; }
        public string bankcode { get; set; }
        public string error { get; set; }
        public string error_Message { get; set; }
        public string name_on_card { get; set; }
        public string cardnum { get; set; }
        public string cardhash { get; set; }
        public string issuing_bank { get; set; }
        public string card_type { get; set; }
        public string reward_point_booking { get; set; }
    }
}