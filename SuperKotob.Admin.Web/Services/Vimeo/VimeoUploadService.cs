using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using TusDotNetClient;

namespace Tollab.Admin.Web.Services.Vimeo
{
    public class VimeoUploadService : IVimeoUploadService
    {
        public async Task<(HttpStatusCode, JObject)> InitiateUpload(string name, double videoLength, string videoPassword, string vimeoToken)
        {
            string uri = "https://api.vimeo.com/me/videos";
            var clientTocall = new HttpClient();
            clientTocall.DefaultRequestHeaders.Authorization
            = new AuthenticationHeaderValue("Bearer", vimeoToken);
            clientTocall.DefaultRequestHeaders.Add("Accept", "application/vnd.vimeo.*+json;version=3.4");

            var VideoObject = new { name = name, upload = new { approach = "tus", size = videoLength }, privacy = new { view = "disable" }};

            var response = await clientTocall.PostAsJsonAsync(uri, VideoObject);

            var responseString = await response.Content.ReadAsStringAsync();
            var tempResponse = JObject.Parse(responseString);
            
            return (response.StatusCode, tempResponse);
        }

        public async Task TranferVideo(string videoURL, string vimeoToken)
        {
            var transferUri = "https://api.vimeo.com/offers" + videoURL + "";
            var clientTocall = new HttpClient();
            clientTocall.DefaultRequestHeaders.Authorization
            = new AuthenticationHeaderValue("Bearer", vimeoToken);
            clientTocall.DefaultRequestHeaders.Add("Accept", "application/vnd.vimeo.*+json;version=3.4");
            var response = await clientTocall.PutAsync(transferUri, null);

            var responseString = await response.Content.ReadAsStringAsync();
            var tempResponse = JObject.Parse(responseString);
            responseString = tempResponse.ToString();
            var responseCode = response.StatusCode;
            if (responseCode == HttpStatusCode.OK)
            {


            }
        }

        public async Task<(bool success, string URL , string videoURI , string errorDetails)> TryUpload(HttpPostedFileBase postedFile, string name, double videoLength, string videoPassword, string vimeoToken)
        {
            var (statusCode, jObject) = await this.InitiateUpload(name, videoLength, videoPassword, vimeoToken);
            string upload_link = "";
            string Link = "";
            string UriWithId = "";
            if (statusCode == HttpStatusCode.OK)
            {
                try
                {
                    upload_link = (string)jObject["upload"]["upload_link"];
                    Link = (string)jObject["link"];
                    UriWithId = (string)jObject["uri"];

                    await this.TranferVideo(UriWithId, vimeoToken);
                }
                catch(Exception ex)
                {

                }
                
            }


            if (!string.IsNullOrEmpty(upload_link))
            {
                var file = new FileInfo(postedFile.FileName);
                var client = new TusClient();
                var fileUrl = await client.UploadAsync(upload_link, postedFile.InputStream, chunkSize: 128D);
            }
            if (string.IsNullOrEmpty(upload_link))
            {
                return (false, string.Empty, string.Empty, "Try again to upload on vimeo");
            }
            return (true, Link, UriWithId, string.Empty);
        }

        public async Task DeleteFromViemoAsync(string VideoUri , string vimeoToken)
        {

            try
            {
                var uri = "https://api.vimeo.com" + VideoUri;

                var clientTocall = new HttpClient();
                clientTocall.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", vimeoToken);
                clientTocall.DefaultRequestHeaders.Add("Accept", "application/vnd.vimeo.*+json;version=3.4");
                var response = await clientTocall.DeleteAsync(uri);

                var responseString = await response.Content.ReadAsStringAsync();
            }
            catch(Exception ex)
            {

            }
            

        }
    }
}