using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GitHubAPI.Utilities
{
    public class APIHelper
    {
        public static string SendAPIRequest(
                string token
                , string apiUrl
                , object obj
                , string action
                )
        {
            try
            {
                HttpClient _client = new HttpClient();
                _client.DefaultRequestHeaders.Add("User-Agent", "request");
                if (!string.IsNullOrEmpty(token))
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                StringContent content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

                HttpResponseMessage responseMessage;

                if (action == "get") //read
                    responseMessage = _client.GetAsync(apiUrl).Result;
                else if (action == "post") //create
                    responseMessage = _client.PostAsync(apiUrl, content).Result;
                else if (action == "put") //update
                    responseMessage = _client.PutAsync(apiUrl, content).Result;
                else //delete
                    responseMessage = _client.DeleteAsync(apiUrl).Result;

                if (responseMessage.Content.ReadAsStringAsync().Result == "")
                {
                    return responseMessage.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    return JsonConvert.DeserializeObject(responseMessage.Content.ReadAsStringAsync().Result).ToString();
                }

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
