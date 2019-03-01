using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

namespace Matchbook
{
    public class MatchbookClient
    {
        private string username { get; set; }
        private string password { get; set; }
        private HttpClient httpClient = null;
        private CookieContainer cookieContainer = null;

        public MatchbookClient(string _username, string _password)
        {
            username = _username;
            password = _password;

            httpClient = getHttpClient();
        }

        private HttpClient getHttpClient()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            cookieContainer = new CookieContainer();
            handler.CookieContainer = cookieContainer;

            HttpClient httpClientEx = new HttpClient(handler);
            //httpClientEx.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html, application/xhtml+xml, application/xml; q=0.9, image/webp, */*; q=0.8");
            //httpClientEx.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36");
            //httpClientEx.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
            //httpClientEx.DefaultRequestHeaders.ExpectContinue = false;

            return httpClientEx;
        }

        public bool Login()
        {
            try
            {
                HttpResponseMessage responseMessage = httpClient.PostAsync("https://api.matchbook.com/bpapi/rest/security/session", new StringContent(string.Format("{{\"username\": \"{0}\", \"password\": \"{1}\" }}", username, password), Encoding.UTF8, "application/json")).Result;
                responseMessage.EnsureSuccessStatusCode();

                string responseMessageString = responseMessage.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrEmpty(responseMessageString))
                    return false;

                return responseMessageString.Contains("session-token");
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public double GetBalace()
        {
            try
            {
                HttpResponseMessage response = httpClient.GetAsync("https://api.matchbook.com/edge/rest/account/balance").Result;
                response.EnsureSuccessStatusCode();

                string responseString = response.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrEmpty(responseString))
                    return 0;

                GroupCollection groups = Regex.Match(responseString, "\"balance\": (?<balance>(\\d+\\.\\d+|\\d+)),").Groups;
                if (groups == null || groups["balance"] == null)
                    return 0;

                return Utility.ParseToDouble(groups["balance"].Value);
            }
            catch(Exception e)
            {
                return 0;
            }
        }

        public string GetEventsById(long sportId)
        {
            try
            {
                HttpResponseMessage response = httpClient.GetAsync(string.Format("https://api.matchbook.com/edge/rest/events/?sport-ids={0}", sportId)).Result;
                response.EnsureSuccessStatusCode();

                string responseString = response.Content.ReadAsStringAsync().Result;
                return responseString;
            }
            catch(Exception e)
            {
                return string.Empty;
            }
        }

        public Status SubmitOffers(long runnerId, string side, double odds, double stake)
        {
            try
            {
                HttpResponseMessage response = httpClient.PostAsync("https://api.matchbook.com/edge/rest/offers", new StringContent(string.Format("{{\"odds-type\":\"DECIMAL\",\"exchange-type\":\"back-lay\",\"offers\":[{{\"runner-id\":{0},\"side\":\"back\",\"odds\": {1},\"stake\": {2}}}]}}",
                    runnerId, side, odds, stake), Encoding.UTF8, "application/json")).Result;
                response.EnsureSuccessStatusCode();

                string responseString = response.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrEmpty(responseString))
                    return Status.failed;

                GroupCollection groups = Regex.Match(responseString, "\"status\": \"(?<status>\\w+)\",").Groups;
                if (groups == null || groups["status"] == null)
                    return Status.failed;

                string status = groups["status"].Value;
                if (string.IsNullOrEmpty(status))
                    return Status.failed;

                if (status == "open")
                    return Status.open;
                else if (status == "cancelled")
                    return Status.cancelled;
                else if (status == "edited")
                    return Status.edited;
                else if (status == "matched")
                    return Status.matched;
                else if (status == "flushed")
                    return Status.flushed;
                else
                    return Status.failed;
            }
            catch(Exception e)
            {
                return Status.failed;
            }
        }
    }
}
