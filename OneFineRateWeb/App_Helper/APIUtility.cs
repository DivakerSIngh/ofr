using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace OneFineRateWeb.Handlers
{
    public class APIUtility
    {
        const string apiBaseUri = "http://localhost:59421";

        const string token = "qxGyW513XoXZj_VNWVUjzl1IqvNQpQouubo22s0AEywc75SkSaDb_Tg2KhzzpdLP1Wq7OTaLofRuzhbm4qpcl2Mq5Efuyx11_tqH6N9BClGNSaY4RQuluDGPA59Bat189SOcvDUTL1jXhvMaNcIwKTyZinntVML2lrUC7zHpRX3xy2RrFXsSgFKm3x8MQdjjas2BPdn85e8ZxGXPTzf9lOuC_PHmLgYnmG1KwF02zKz1PDOyjqvvYwU-9tGFoX8-0igVfCjw1V1gmfuKO99CIEYmuoUszsZiKXBtD6C5g3Mkf6IRn7YmyaSmTjT4hELqpaCP5UNU2WIvb2NRroLd_f3eAaOHI6gthUKcz6Pv34rr_F_EKuPaPszXR9n0w7V0vV2pZCgBYSUfRih-Cigpq6g-AJKhlp3O_F_791SIA8Einw_XV3vq2WGJ3PzgKlnnxOKsCsNBwJexyiEcuRz3A9AVlU2naH1EGow6E8uACBr0OebCG8MJ660SQm102sjjRiwkKyUBkptzOytv2J6lIw";

        static string userName = ConfigurationManager.AppSettings["ApiUserName"];
        static string password = ConfigurationManager.AppSettings["ApiUserPassword"];

        public static HttpResponseMessage PostRequest(string relativeUrl, object model)
        {
            string existingToken = CacheStore.Get("token");

            if (string.IsNullOrEmpty(existingToken))
            {
                //var token = GetAPIToken(userName, password, apiBaseUri);

                if (!string.IsNullOrEmpty(token))
                {
                    CacheStore.AddToCache("token", token, 50000);
                    existingToken = token;
                }
                else
                {
                    throw new Exception("User not Found !");
                }
            }

            using (var client = new HttpClient())
            {
                //setup client
                client.BaseAddress = new Uri(apiBaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                //make request
                HttpResponseMessage response = client.PostAsJsonAsync(relativeUrl, model).Result;
                var responseString = response.Content.ReadAsStringAsync();
                return response;
            }
        }


        public static HttpResponseMessage GetRequest(string relativeUrl)
        {
            string existingToken = CacheStore.Get("token");

            if (string.IsNullOrEmpty(existingToken))
            {
                // var token = GetAPIToken(userName, password, apiBaseUri);

                if (!string.IsNullOrEmpty(token))
                {
                    CacheStore.AddToCache("token", token, 50000);
                    existingToken = token.ToString();
                }
                else
                {
                    throw new Exception("User not Found !");
                }
            }

            using (var client = new HttpClient())
            {
                //setup client
                client.BaseAddress = new Uri(apiBaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                //make request
                HttpResponseMessage response = client.GetAsync(relativeUrl).Result;
                return response;
            }
        }

        public static string GetQueryString(object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return String.Join("&", properties.ToArray());
        }


        private static string GetAPIToken(string userName, string password, string apiBaseUri)
        {
            using (var client = new HttpClient())
            {
                //setup client
                client.BaseAddress = new Uri(apiBaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //setup login data
                var formContent = new FormUrlEncodedContent(new[]
                 {
                 new KeyValuePair<string, string>("grant_type", "password"), 
                 new KeyValuePair<string, string>("username", userName), 
                 new KeyValuePair<string, string>("password", password), 
                 });

                //send request
                HttpResponseMessage responseMessage = client.PostAsync("/Token", formContent).Result;

                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseJson = responseMessage.Content.ReadAsStringAsync().Result;
                    var jObject = JObject.Parse(responseJson);
                    //get access token from response body
                    return jObject.GetValue("access_token").ToString();
                }
                else
                {
                    return null;
                }

            }
        }
    }
}