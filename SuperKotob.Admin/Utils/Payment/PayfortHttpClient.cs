using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.Payment
{
    public class PayfortHttpClient
    {

        public const string access_code = "LcDyBFuLpBRASYrIcVjh";
        public const string merchant_identifier = "RCosLRqQ";
        public const string RequestSHAKey = "SuperKotobSHAIN";

     

        HttpClient _client = new HttpClient();
        public PayfortHttpClient()
        {

            _client = new HttpClient();
        }
      

        public async Task<string> PostJsonAsync(IDictionary<string, string> dic, bool addSignature = true)
        {

            _client.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

            dic = dic.OrderBy(item => item.Key)
                .ToDictionary(item => item.Key, item => item.Value);

            var json = JsonConvert.SerializeObject(dic);
            var content = new StringContent(json,
                Encoding.UTF8,
                "application/json");


            var resultTask = await _client.PostAsync("https://sbcheckout.PayFort.com/FortAPI/paymentApi", content);


            var response = await resultTask.Content.ReadAsStringAsync();
            return response;
        }

        public async Task<string> GetAsync(string url)
        {
            var response = await _client.GetAsync(url);
            var text = await response.Content.ReadAsStringAsync();
            return text;
        }

        public async Task<string> PostFormsAsync(IDictionary<string, string> dic)
        {

            var content = new FormUrlEncodedContent(dic);
            var resultTask = await _client.PostAsync("https://sbcheckout.PayFort.com/FortAPI/paymentPage", content);

            var resultContent = await resultTask.Content.ReadAsStringAsync();
            return resultContent;
        }
       

        public async Task Make3dsUrl(string url_3ds)
        {
            await _client.GetAsync(url_3ds);
        }
    }
}
