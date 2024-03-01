using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Tollab.Admin.Web.Utils
{
    public class DailyMotionHandler
    {
        public static async Task<string> GetAccessTokenAsync()
        {

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    var formData = new MultipartFormDataContent();
                    formData.Add(new StringContent("password"), "grant_type");
                    formData.Add(new StringContent("8507a1f47fda99be07f8"), "client_id");
                    formData.Add(new StringContent("57dde83c69976866485452c20d2dc45d35c9513e"), "client_secret");
                    formData.Add(new StringContent("Info@tollab.com"), "username");
                    formData.Add(new StringContent("Toll@252"), "password");

                    using (HttpResponseMessage responseobj = client.PostAsync("https://api.dailymotion.com/oauth/token", formData).Result)
                    {

                        string responseBody = await responseobj.Content.ReadAsStringAsync();

                        var data = JsonConvert.DeserializeObject<OAuthResponse>(responseBody);
                        return data.access_token;
                    }
                }
            }
            catch (Exception)
            {

                return String.Empty;
            }
        }
        public static async Task<string> GetFileUploadUrlAsync(string accessToken)
        {

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", $"OAuth {accessToken}");
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    using (HttpResponseMessage response = client.GetAsync("https://api.dailymotion.com/file/upload").Result)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                        var data = JsonConvert.DeserializeObject<UploadRequestResponse>(responseBody);
                        return data?.upload_url;
                    }
                }
            }
            catch (Exception ex)
            {

                return String.Empty;
            }
        }
        
            public static async Task<PrivateKeyResponse> GetPrivateKeyAsync(string id, string accessToken, string uploadUrl)
        {
            using (var publishClient = new HttpClient())
            {
                publishClient.Timeout = System.Threading.Timeout.InfiniteTimeSpan;
                publishClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                publishClient.DefaultRequestHeaders.Add("Authorization", "OAuth " + accessToken);
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                
                using (HttpResponseMessage responseobj = publishClient.GetAsync("https://api.dailymotion.com/video/"+id+"?fields=private_id").Result)
                {

                    string responseBody = await responseobj.Content.ReadAsStringAsync();

                    var data = JsonConvert.DeserializeObject<PrivateKeyResponse>(responseBody);
                    return data;
                }
            }

        }
        public static async Task<UploadedResponse> UploadAsync(string videoTitle, string filePath, string accessToken, string uploadUrl)
        {


            try
            {
                
                var uploadedContentResult = await UploadVideoContent(uploadUrl, filePath, accessToken);
                var response = JsonConvert.DeserializeObject<UploadResponse>(uploadedContentResult);
                using (var publishClient = new HttpClient())
                {
                    publishClient.Timeout = System.Threading.Timeout.InfiniteTimeSpan;
                    publishClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                    publishClient.DefaultRequestHeaders.Add("Authorization", "OAuth " + accessToken);
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    var formData = new MultipartFormDataContent();
                    
                    formData.Add(new StringContent(videoTitle), "title");
                    formData.Add(new StringContent("true"), "private");
                    formData.Add(new StringContent("false"), "is_created_for_kids");
                    formData.Add(new StringContent("videogames"), "channel");
                    formData.Add(new StringContent("true"), "published");
                    
                    using (HttpResponseMessage responseobj = publishClient.PostAsync("https://api.dailymotion.com/me/videos?url=" + HttpUtility.UrlEncode(response.url), formData).Result)
                    {

                        string responseBody = await responseobj.Content.ReadAsStringAsync();
                        
                        var data = JsonConvert.DeserializeObject<UploadedResponse>(responseBody);
                        return data;
                    }
                }


            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public static async Task<string> UploadVideoContent(string uri, string pathFile, string accessToken)
        {
            byte[] bytes = System.IO.File.ReadAllBytes(pathFile);
            var today = DateTime.Now;
            var filename = $"{today.Year}-{today.Month}-{today.Day}-{today.Hour}-{today.Minute}-{today.Second}-Video.mp4";
            using (var client = new HttpClient())
            {
                client.Timeout = System.Threading.Timeout.InfiniteTimeSpan;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", accessToken);
                using (var content =
                    new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
                {
                    content.Add(new StreamContent(new MemoryStream(bytes)), "file", filename);

                    using (var message = await client.PostAsync(uri, content))
                    {
                        return await message.Content.ReadAsStringAsync();


                    }
                }
            }

        }

    }
}
public class UploadedResponse
{
    public string id { get; set; }


}public class PrivateKeyResponse
{
    public string private_id { get; set; }
}
public class UploadResponse
{
    public string format { get; set; }
    public string acodec { get; set; }
    public string vcodec { get; set; }
    public int duration { get; set; }
    public int bitrate { get; set; }
    public string dimension { get; set; }
    public string name { get; set; }
    public int size { get; set; }
    public string url { get; set; }
    public string hash { get; set; }
    public string seal { get; set; }

    public override string ToString()
    {
        var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.FlattenHierarchy;
        System.Reflection.PropertyInfo[] infos = this.GetType().GetProperties(flags);

        StringBuilder sb = new StringBuilder();

        string typeName = this.GetType().Name;
        sb.AppendLine(typeName);
        sb.AppendLine(string.Empty.PadRight(typeName.Length + 5, '='));

        foreach (var info in infos)
        {
            object value = info.GetValue(this, null);
            sb.AppendFormat("{0}: {1}{2}", info.Name, value != null ? value : "null", Environment.NewLine);
        }

        return sb.ToString();
    }
}
public class OAuthResponse
{
    public string access_token { get; set; }
    public int expires_in { get; set; }
    public string refresh_token { get; set; }
}
public class UploadRequestResponse
{
    public string upload_url { get; set; }
    public string progress_url { get; set; }
}