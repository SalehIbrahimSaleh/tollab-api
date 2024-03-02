using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SuperKotob.Admin.Data;
using Tollab.Admin.Data.Models;
using SuperKotob.Admin.Web.Controllers;
using SuperKotob.Admin.Core;
using SuperKotob.Admin.Utils.Configuration;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using VimeoDotNet;
using VimeoDotNet.Net;
using System.IO;
using System.Diagnostics;
using System.Security.Policy;
using Com.CloudRail.SI.Services;
using TusDotNetClient;
using System.Net.Http.Headers;
using Tollab.Admin.Web.Utils;
using RestSharp;
using Newtonsoft.Json;
using static Tollab.Admin.Web.Controllers.CoursesController;

namespace Tollab.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin")]

    public class ContentsController : BaseWebController<Content, Content>
    {
        private  string PasswordForVideo = "Tollab@hacker!@#%^&*(@147852";
        private TollabContext db = new TollabContext();
        public ContentsController(IBusinessService<Content, Content> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }
        private const string VimeoToken = "2878d5dde0fe009cc71041e7c82d5292";
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateWithImage(Content item, HttpPostedFileBase PathContent)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if ( PathContent != null && item.ContentTypeId == 1)
                    {
                        if (item.ProviderType == "Vimeo")
                        {
                            string AlbumUri = GetAlbumByGroupId(item.GroupId);

                            var len = PathContent.InputStream.Length;
                            string upload_link = "";
                            string Link = "";
                            string UriWithId = "";
                            string uri = "https://api.vimeo.com/me/videos";
                            var clientTocall = new HttpClient();
                            clientTocall.DefaultRequestHeaders.Authorization
                            = new AuthenticationHeaderValue("Bearer", VimeoToken);
                            clientTocall.DefaultRequestHeaders.Add("Accept", "application/vnd.vimeo.*+json;version=3.4");

                            // PasswordForVideo = PasswordForVideo + DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");
                            var VideoObject = new { name = item.Name, upload = new { approach = "tus", size = len }, privacy = new { view = "disable" } };
                            //var VideoObject = new { name = item.Name, upload = new { approach = "tus", size = len } };

                            var response = await clientTocall.PostAsJsonAsync(uri, VideoObject);

                            var responseString = await response.Content.ReadAsStringAsync();
                            var tempResponse = JObject.Parse(responseString);
                            responseString = tempResponse.ToString();
                            var responseCode = response.StatusCode;
                            if (responseCode == HttpStatusCode.OK)
                            {
                                upload_link = (string)tempResponse["upload"]["upload_link"];
                                Link = (string)tempResponse["link"];
                                UriWithId = (string)tempResponse["uri"];
                                item.Path = Link;
                                item.VideoUri = UriWithId;
                                TransferVideo(AlbumUri, item.VideoUri);

                            }
                            //
                            if (!string.IsNullOrEmpty(upload_link))
                            {
                                var file = new FileInfo(PathContent.FileName);
                                var client = new TusClient();
                                var fileUrl = await client.UploadAsync(upload_link, PathContent.InputStream, chunkSize: 128D);
                            }
                            if (string.IsNullOrEmpty(upload_link))
                            {
                                ViewBag.Message = "Try again to upload on vimeo";
                            }
                        
                        }
                        else if (item.ProviderType == "vdocipher")
                        {
                            var PostURL = string.Format("https://dev.vdocipher.com/api/videos?title={0}&folderId={1}", item.Name, "b22b4559e0664c66b2eda5ea9221861c");
                            var client = new RestClient(PostURL);
                            var request = new RestRequest(Method.PUT);
                            request.AddHeader("Authorization", "Apisecret gZKDngGYS8bcAb2Xpl8i97qTs75K9pjVaRq6na8LdlLRASOF7OAVqddrB2cy9Ep0");
                            IRestResponse response = client.Execute(request);
                            var tempResponse = JsonConvert.DeserializeObject<UploadRoot>(response.Content);
                            var _httpClient = new HttpClient();
                            var form = new MultipartFormDataContent();
                            form.Add(new StringContent(tempResponse.clientPayload.policy), "policy");
                            form.Add(new StringContent(tempResponse.clientPayload.key), "key");
                            form.Add(new StringContent(tempResponse.clientPayload.xamzsignature), "x-amz-signature");
                            form.Add(new StringContent(tempResponse.clientPayload.xamzalgorithm), "x-amz-algorithm");
                            form.Add(new StringContent(tempResponse.clientPayload.xamzdate), "x-amz-date");
                            form.Add(new StringContent(tempResponse.clientPayload.xamzcredential), "x-amz-credential");
                            form.Add(new StringContent("201"), "success_action_status");
                            form.Add(new StringContent(""), "success_action_redirect");
                            //Path.GetFileName(IntroVideo.FileName)
                            Stream documentConverted = PathContent.InputStream;
                            form.Add(new StreamContent(documentConverted), "file", PathContent.FileName);
                            var responsess = await _httpClient.PostAsync(tempResponse.clientPayload.uploadLink, form);
                            if ((int)responsess.StatusCode == 201)
                            {
                                item.Path = tempResponse.videoId;
                            }

                        }
                        else
                        {
                            string path = Path.Combine(Server.MapPath("~/Uploads"), Path.GetFileName(PathContent.FileName));
                            PathContent.SaveAs(path);
                            //Upload by Dailymotion
                            var accessToken = await DailyMotionHandler.GetAccessTokenAsync();
                            var uploadUrl = await DailyMotionHandler.GetFileUploadUrlAsync(accessToken);
                            var reponse = await DailyMotionHandler.UploadAsync(item.Name, path, accessToken, uploadUrl);
                            
                        
                            FileInfo f = new FileInfo(path);
                            f.Delete();
                            if (reponse != null)
                            {
                                var getPrivateKey = await DailyMotionHandler.GetPrivateKeyAsync(reponse.id, accessToken, uploadUrl);
                                item.Path = getPrivateKey.private_id;
                            }
                        }
                            

                    }
                    if (PathContent != null && item.ContentTypeId == 2)
                    {
                        try
                        {
                            string uri = "http://tollab.com/tollab/api/Upload";
                            using (HttpClient client = new HttpClient())
                            {
                                using (var content = new MultipartFormDataContent())
                                {
                                    byte[] fileBytes = new byte[PathContent.InputStream.Length + 1];
                                    PathContent.InputStream.Read(fileBytes, 0, fileBytes.Length);
                                    var fileContent = new ByteArrayContent(fileBytes);
                                    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = PathContent.FileName };
                                    content.Add(fileContent);
                                    var result = client.PostAsync(uri, content).Result;
                                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                                    {
                                        var responseString = await result.Content.ReadAsStringAsync();
                                        var tempResponse = JObject.Parse(responseString);
                                        var video = tempResponse.GetValue("model").ToString();

                                        if (!string.IsNullOrEmpty(video))
                                        {
                                            item.Path = video;
                                        }
                                    }
                                    else
                                    {
                                        ViewBag.Message = "Failed";
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        { }

                    }

                    if (item.OrderNumber==null)
                    {
                        item.OrderNumber = 0;
                    }
                    if(item.NewPathTemp != null)
                    {
                        item.Path = item.NewPathTemp;
                    }
                    
                    var DataReturned = await BusinessService.CreateAsync(item);
                    return RedirectToAction("Index");
                }
                return View("Create", item);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditWithImage(Content item, HttpPostedFileBase PathContent)
        {
            if (ModelState.IsValid)
            {
                var oldItem = (await BusinessService.GetAsync(item.Id)).Data.FirstOrDefault() ;
                if (PathContent != null && item.ContentTypeId == 1)
                {
                    if (item.ProviderType == "Vimeo")
                    {
                    if (oldItem.VideoUri != null)
                    {
                        DleteFromViemoAsync(oldItem.Id, oldItem.VideoUri);
                    }
                    string AlbumUri = GetAlbumByGroupId(oldItem.GroupId);

                    var len = PathContent.InputStream.Length;
                    string upload_link = "";
                    string Link = "";
                    string UriWithId = "";
                    string uri = "https://api.vimeo.com/me/videos";
                    var clientTocall = new HttpClient();
                    clientTocall.Timeout = TimeSpan.FromMinutes(60);

                    clientTocall.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", VimeoToken);
                    clientTocall.DefaultRequestHeaders.Add("Accept", "application/vnd.vimeo.*+json;version=3.4");
                    //var VideoObject = new { name = item.Name, upload = new { approach = "tus", size = len } };

                    var VideoObject = new { name = item.Name, upload = new { approach = "tus", size = len }, privacy = new { view = "disable" } };

                    var response = await clientTocall.PostAsJsonAsync(uri, VideoObject);

                    var responseString = await response.Content.ReadAsStringAsync();
                    var tempResponse = JObject.Parse(responseString);
                    responseString = tempResponse.ToString();
                    var responseCode = response.StatusCode;
                    if (responseCode == HttpStatusCode.OK)
                    {
                        upload_link = (string)tempResponse["upload"]["upload_link"];
                        Link = (string)tempResponse["link"];
                        UriWithId = (string)tempResponse["uri"];
                        item.Path = Link;
                        item.VideoUri = UriWithId;
                        TransferVideo(AlbumUri, item.VideoUri);

                    }
                    //
                    if (!string.IsNullOrEmpty(upload_link))
                    {
                        var file = new FileInfo(PathContent.FileName);
                        var client = new TusClient();
                        var fileUrl = await client.UploadAsync(upload_link, PathContent.InputStream, chunkSize: 128D);
                    }
                    if (string.IsNullOrEmpty(upload_link))
                    {
                        ViewBag.Message = "Try again to upload on vimeo";
                    }
                    }
                    else if (item.ProviderType == "vdocipher")
                    {
                        var PostURL = string.Format("https://dev.vdocipher.com/api/videos?title={0}&folderId={1}", item.Name, "b22b4559e0664c66b2eda5ea9221861c");
                        var client = new RestClient(PostURL);
                        var request = new RestRequest(Method.PUT);
                        request.AddHeader("Authorization", "Apisecret gZKDngGYS8bcAb2Xpl8i97qTs75K9pjVaRq6na8LdlLRASOF7OAVqddrB2cy9Ep0");
                        IRestResponse response = client.Execute(request);
                        var tempResponse = JsonConvert.DeserializeObject<UploadRoot>(response.Content);
                        var _httpClient = new HttpClient();
                        var form = new MultipartFormDataContent();
                        form.Add(new StringContent(tempResponse.clientPayload.policy), "policy");
                        form.Add(new StringContent(tempResponse.clientPayload.key), "key");
                        form.Add(new StringContent(tempResponse.clientPayload.xamzsignature), "x-amz-signature");
                        form.Add(new StringContent(tempResponse.clientPayload.xamzalgorithm), "x-amz-algorithm");
                        form.Add(new StringContent(tempResponse.clientPayload.xamzdate), "x-amz-date");
                        form.Add(new StringContent(tempResponse.clientPayload.xamzcredential), "x-amz-credential");
                        form.Add(new StringContent("201"), "success_action_status");
                        form.Add(new StringContent(""), "success_action_redirect");
                        //Path.GetFileName(IntroVideo.FileName)
                        Stream documentConverted = PathContent.InputStream;
                        form.Add(new StreamContent(documentConverted), "file", PathContent.FileName);
                        var responsess = await _httpClient.PostAsync(tempResponse.clientPayload.uploadLink, form);
                        if ((int)responsess.StatusCode == 201)
                        {
                            item.Path = tempResponse.videoId;
                        }
                        
                    }
                    else
                    {
                        string path = Path.Combine(Server.MapPath("~/Uploads"), Path.GetFileName(PathContent.FileName));
                        PathContent.SaveAs(path);
                        //Upload by Dailymotion
                        var accessToken = await DailyMotionHandler.GetAccessTokenAsync();
                        var uploadUrl = await DailyMotionHandler.GetFileUploadUrlAsync(accessToken);
                        var reponse = await DailyMotionHandler.UploadAsync(item.Name, path, accessToken, uploadUrl);


                        FileInfo f = new FileInfo(path);
                        f.Delete();
                        if (reponse != null)
                        {
                            var getPrivateKey = await DailyMotionHandler.GetPrivateKeyAsync(reponse.id, accessToken, uploadUrl);
                            item.Path = getPrivateKey.private_id;
                        }
                    }
                }

                if (PathContent != null && item.ContentTypeId == 2)
                {
                    try
                    {
                        string uri = "http://tollab.com/tollab/api/Upload";
                        using (HttpClient client = new HttpClient())
                        {
                            using (var content = new MultipartFormDataContent())
                            {
                                byte[] fileBytes = new byte[PathContent.InputStream.Length + 1];
                                PathContent.InputStream.Read(fileBytes, 0, fileBytes.Length);
                                var fileContent = new ByteArrayContent(fileBytes);
                                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = PathContent.FileName };
                                content.Add(fileContent);
                                var result = client.PostAsync(uri, content).Result;
                                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                                {
                                    var responseString = await result.Content.ReadAsStringAsync();
                                    var tempResponse = JObject.Parse(responseString);
                                    var video = tempResponse.GetValue("model").ToString();

                                    if (!string.IsNullOrEmpty(video))
                                    {
                                        item.Path = video;
                                    }
                                }
                                else
                                {
                                    ViewBag.Message = "Failed";
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    { }
                }
                if (item.OrderNumber == null)
                {
                    item.OrderNumber = 0;
                }
                if (item.NewPathTemp != null)
                {
                    item.Path = item.NewPathTemp;
                }
                var DataReturned = await BusinessService.UpdateAsync(item);
              

                return RedirectToIndex(item);
            }
            return View("Edit", item);
        }

        public string GetAlbumByGroupId(long? id)
        {
            string sql = @"select Course.AlbumUri from Course join [Group] on Course.Id=[Group].CourseId 
            where [Group].Id=" + id + "";
            var result=  db.Database.SqlQuery<string>(sql);
            if (result.Count()>0)
            {
                return result.FirstOrDefault();

            }
            return null;
        }

        public async Task TransferVideo(string AlbumUri,string VideoUri)
        {
            var transferUri = "https://api.vimeo.com" + "" + AlbumUri + "" + "" + VideoUri + "";
            var clientTocall = new HttpClient();
            clientTocall.DefaultRequestHeaders.Authorization
            = new AuthenticationHeaderValue("Bearer", VimeoToken);
            clientTocall.DefaultRequestHeaders.Add("Accept", "application/vnd.vimeo.*+json;version=3.4");
            var response = await clientTocall.PutAsync(transferUri,null);

            var responseString = await response.Content.ReadAsStringAsync();
            var tempResponse = JObject.Parse(responseString);
            responseString = tempResponse.ToString();
            var responseCode = response.StatusCode;
            if (responseCode == HttpStatusCode.OK)
            {
               

            }

        }


        private async Task DleteFromViemoAsync(long contentId,string VideoUri)
        {
            
                var uri = "https://api.vimeo.com" + VideoUri;

                var clientTocall = new HttpClient();
                clientTocall.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", VimeoToken);
                clientTocall.DefaultRequestHeaders.Add("Accept", "application/vnd.vimeo.*+json;version=3.4");
                var response = await clientTocall.DeleteAsync(uri);

                var responseString = await response.Content.ReadAsStringAsync();
            
        }

    }
}




//public async Task<ActionResult> CreateWithImage(Content item, HttpPostedFileBase PathContent)
//{
//    try
//    {
//        var len = PathContent.InputStream.Length;

//        var file = new FileInfo(PathContent.FileName);
//        var client = new TusClient();
//        var fileUrl = await client.UploadAsync("https://files.tus.vimeo.com/files/vimeo-prod-src-tus-eu/7e98e70688faaa7ec384429091d3e2ea", PathContent.InputStream, chunkSize: 10D);


//        byte[] fileBytesTest = new byte[PathContent.InputStream.Length + 1];
//        // "85718a76a5999641e35701f9b6479c45
//        BinaryContent binaryContent = new BinaryContent(PathContent.InputStream, "video");
//        VimeoClient vimeoClient = new VimeoClient("ab411c10a341290dbd2dc6b4487aa7f177dcd049", "yWOnwoa61YVRQdEBDuoGQbH/6u+BAmYeBLsUJMOa8GHCFp2cxqlSmVXGkRkFLiWGXdgDsCL7G3AQLHaw2h+41rrtJVUBIgl5DLRHtGa2BCI5VIKGAalakw/Wi1lxqobA");
//        // VimeoClient vimeoClient = new VimeoClient("81fdd8c3c770bd3962a395cc4836d746");
//        var AccountInfo = await vimeoClient.GetAccountInformationAsync();

//        var album = await vimeoClient.GetAlbumsAsync(AccountInfo.Id);
//        var tikt = await vimeoClient.GetUploadTicketAsync();
//        var date = await vimeoClient.UploadEntireFileAsync(binaryContent);

//        if (ModelState.IsValid)
//        {
//            var DataReturned = await BusinessService.CreateAsync(item);

//            if (PathContent != null && DataReturned.HasData == true)
//            {
//                try
//                {
//                    string uri = "http://tollab.com/dashboard-v2/ws/api/Upload";
//                    //string uri = "http://localhost:56065/api/Upload";
//                    using (HttpClient client2 = new HttpClient())
//                    {
//                        using (var content = new MultipartFormDataContent())
//                        {
//                            byte[] fileBytes = new byte[PathContent.InputStream.Length + 1];
//                            PathContent.InputStream.Read(fileBytes, 0, fileBytes.Length);

//                            var fileContent = new ByteArrayContent(fileBytes);
//                            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment") { FileName = PathContent.FileName };
//                            content.Add(fileContent);
//                            var result = client2.PostAsync(uri, content).Result;
//                            if (result.StatusCode == System.Net.HttpStatusCode.OK)
//                            {
//                                var responseString = await result.Content.ReadAsStringAsync();
//                                var tempResponse = JObject.Parse(responseString);
//                                var video = tempResponse.GetValue("model").ToString();

//                                if (!string.IsNullOrEmpty(video))
//                                {
//                                    item.Path = video;
//                                    await BusinessService.UpdateAsync(item);
//                                }
//                            }
//                            else
//                            {
//                                ViewBag.Message = "Failed";
//                            }
//                        }
//                    }
//                }
//                catch (Exception ex)
//                { }
//            }

//            return RedirectToAction("Index");
//        }
//        return View("Create", item);
//    }
//    catch (Exception ex)
//    {
//        throw ex;
//    }

//}
