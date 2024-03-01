using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Tollab.Admin.Web.Services.FileUpload
{
    public class FileUploadService : IFileUploadService
    {
        public async Task<string> UploadFile(HttpPostedFileBase file)
        {
            string uri = "https://tollab.azurewebsites.net/sws/api/Upload";
            using (HttpClient client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    byte[] fileBytes = new byte[file.InputStream.Length + 1];
                    file.InputStream.Read(fileBytes, 0, fileBytes.Length);
                    var fileContent = new ByteArrayContent(fileBytes);
                    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = file.FileName };
                    content.Add(fileContent);
                    var result = client.PostAsync(uri, content).Result;
                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var responseString = await result.Content.ReadAsStringAsync();
                        var tempResponse = JObject.Parse(responseString);
                        var video = tempResponse.GetValue("model").ToString();

                        if (!string.IsNullOrEmpty(video))
                        {
                            return  video;
                        }
                        return null;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public async Task<HttpStatusCode> UploadImage(long recordId, string table, string columnName, int imageType, HttpPostedFileBase image)
        {
            byte[] thePictureAsBytes = new byte[image.ContentLength];
            using (BinaryReader theReader = new BinaryReader(image.InputStream))
            {
                thePictureAsBytes = theReader.ReadBytes(image.ContentLength);
            }
            string thePictureDataAsString = Convert.ToBase64String(thePictureAsBytes);
            string uri = "https://tollab.azurewebsites.net/sws/api/SetPhoto";
            var client = new HttpClient();
            var imageObject = new { RecordId = recordId, Table = table, CoulmnName = columnName, ImageType = imageType, Image = thePictureDataAsString };
            var response = await client.PostAsJsonAsync(uri, imageObject);
            var responseString = await response.Content.ReadAsStringAsync();
            var tempResponse = JObject.Parse(responseString);
            responseString = tempResponse.ToString();
            var responseCode = response.StatusCode;
            return responseCode;
        }
    }
}