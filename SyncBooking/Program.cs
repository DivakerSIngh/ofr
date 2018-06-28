using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FutureSoft.Util;
using System.Net.Http;
using System.Net;
using System.IO;
namespace SyncBooking
{
    class Program
    {/*
        public static async Task Test()
        {
            
            var httpClient = new HttpClient();

            var content = new StringContent("{\"key\":\"1234\",\"compid\":\"1\"}", Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("http://hindwarestaging1.azurewebsites.net/Service.svc/GetHomePage", content);
            response.EnsureSuccessStatusCode();

            var responseXml1 = await response.Content.ReadAsStringAsync();
            string s = responseXml1;
          

            Uri uri = new Uri("http://hindwarestaging1.azurewebsites.net/Service.svc/GetHomePage");

            HttpWebRequest Request = (HttpWebRequest)HttpWebRequest.Create("http://hindwarestaging1.azurewebsites.net/Service.svc/GetHomePage");
            byte[] RequestBytes = Encoding.UTF8.GetBytes("{\"key\":\"1234\",\"compid\":\"1\"}");
            Request.ContentLength = RequestBytes.Length;
            Request.Method = "POST";
            Request.ContentType = "application/json";

            Stream RequestStream = Request.GetRequestStream();
            RequestStream.Write(RequestBytes, 0, RequestBytes.Length);
            RequestStream.Close();

            HttpWebResponse response = (HttpWebResponse)Request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string ResponseMessage = reader.ReadToEnd();
            response.Close();
            string s = ResponseMessage;  
        }*/
        static void Main(string[] args)
        {
            clsBooking objBooking = new clsBooking();

            objBooking.OTA_HotelResNotifRQ().Wait();
            //MailComponent.SendEmail("Deepaka@futuresoftindia.com", "", "", "Testing WebJob", "Testing Web Job", null, null, false);
        }
    }
}
