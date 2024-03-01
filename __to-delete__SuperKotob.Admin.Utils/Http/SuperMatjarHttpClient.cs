using Jil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.Http
{
   public class SuperKotobHttpClient : IHttpClient
    {
        public SuperKotobHttpClient()
        {
            this.InnerClient = new HttpClient();
        }

        public async Task<HttpResponseMessage> PostJsonAsync(string url, dynamic data)
        {
            var uri = new Uri(url);
            var json = JSON.Serialize(data);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await this.InnerClient.PostAsync(uri, stringContent);
            return response;
        }
        HttpClient InnerClient { get; set; }
    }
}
