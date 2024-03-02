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
using System.IO;
using System.Net.Http;
using Tollab.Admin.Web.Utils;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using TusDotNetClient;
using SuperKotob.Admin.Models;
using Newtonsoft.Json;
using System.Text;
using RestSharp;

namespace Tollab.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin")]

    public class CoursesController : BaseWebController<Course,Course>
    {
        private TollabContext db = new TollabContext();
        private const string VimeoToken = "2878d5dde0fe009cc71041e7c82d5292";
        private string PasswordForVideo = "Tollab@hacker!@#%^&*(@147852";

        public CoursesController(IBusinessService<Course, Course> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateWithImage(Course item, HttpPostedFileBase ImageFile, HttpPostedFileBase IntroVideo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    item.SKUPrice = (((item.CurrentCost * (decimal).30) + item.CurrentCost) * (decimal).30) + item.CurrentCost;
                    item.OldSKUPrice = (((item.PreviousCost * (decimal).30) + item.PreviousCost) * (decimal).30) + item.PreviousCost;
                    //item.SKUPrice = item.CurrentCost + (((item.CurrentCost) * 30) / 100);
                   // item.OldSKUPrice = item.PreviousCost + (((item.PreviousCost) * 30) / 100);
                    var TrackData = db.Tracks.Where(it => it.Id == item.TrackId).FirstOrDefault();

                    var TrackSubject = TrackData.TrackSubject;
                    item.CourseTrack = item.Name + "-" + TrackSubject;

                    var Albumuri = await CreateAlbumAsync(item.CourseTrack);
                    item.AlbumUri = Albumuri;
                    if (item.CurrentCost == null || item.PreviousCost == null)
                    { 
                        ViewBag.Message = "Please enter current price, old price";
                        return View("Create", item);
                    }

                    item.CreationDate = DateTime.UtcNow;
                    var DataReturned = await BusinessService.CreateAsync(item);
                    if (ImageFile != null && DataReturned.HasData == true)
                    {
                        try
                        {
                            byte[] thePictureAsBytes = new byte[ImageFile.ContentLength];
                            using (BinaryReader theReader = new BinaryReader(ImageFile.InputStream))
                            {
                                thePictureAsBytes = theReader.ReadBytes(ImageFile.ContentLength);
                            }
                            string thePictureDataAsString = Convert.ToBase64String(thePictureAsBytes);
                            string uri = "http://tollab.com/tollab/api/SetPhoto";
                            var client = new HttpClient();
                            var imageObject = new { RecordId = item.Id, Table = "Course", CoulmnName = "Image", ImageType = (int)ImageFolders.CourseImages, Image = thePictureDataAsString };
                            var response = await client.PostAsJsonAsync(uri, imageObject);
                            var responseString = await response.Content.ReadAsStringAsync();
                            var tempResponse = JObject.Parse(responseString);
                            responseString = tempResponse.ToString();
                            var responseCode = response.StatusCode;
                            if (responseCode == HttpStatusCode.OK)
                            {
                                var img = tempResponse.GetValue("model").ToString();
                               
                            }
                        }
                        catch (Exception ex)
                        { }
                    }
                    
                    if (IntroVideo != null && DataReturned.HasData == true)
                    {
                        
                        if (item.ProviderType== "Vimeo")
                        {
                            var len = IntroVideo.InputStream.Length;
                            string upload_link = "";
                            string Link = "";
                            string UriWithId = "";
                            string uri = "https://api.vimeo.com/me/videos";
                            var clientTocall = new HttpClient();
                            clientTocall.DefaultRequestHeaders.Authorization
                            = new AuthenticationHeaderValue("Bearer", VimeoToken);
                            clientTocall.DefaultRequestHeaders.Add("Accept", "application/vnd.vimeo.*+json;version=3.4");
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
                                item.IntroVideo = Link;
                                item.IntroVideoUri = UriWithId;
                                TransferVideo(item.AlbumUri, item.IntroVideoUri);

                            }
                            //
                            if (!string.IsNullOrEmpty(upload_link))
                            {
                                var file = new FileInfo(IntroVideo.FileName);
                                var client = new TusClient();
                                var fileUrl = await client.UploadAsync(upload_link, IntroVideo.InputStream, chunkSize: 10D);
                            }
                        }
                        else if(item.ProviderType == "vdocipher")
                        {
                           /* string path = Path.Combine(Server.MapPath("~/Uploads_Temp"), Path.GetFileName(IntroVideo.FileName));
                            IntroVideo.SaveAs(path);
                            var clients = new RestClient("https://dev.vdocipher.com/api/videos/importUrl");
                            var requests = new RestRequest(Method.PUT);
                            requests.AddHeader("Content-Type", "application/json");
                            requests.AddHeader("Accept", "application/json");
                            requests.AddHeader("Authorization", "Apisecret gZKDngGYS8bcAb2Xpl8i97qTs75K9pjVaRq6na8LdlLRASOF7OAVqddrB2cy9Ep0");
                            var data = new { url = "https://tollab.azurewebsites.net/Uploads_Temp/"+ IntroVideo.FileName, folderId = "b22b4559e0664c66b2eda5ea9221861c",title=item.Name };
                            var formatedUrl = JsonConvert.SerializeObject(data);
                            requests.AddParameter("undefined", formatedUrl, ParameterType.RequestBody);
                            IRestResponse responses = clients.Execute(requests);
                            if (responses.StatusCode == HttpStatusCode.OK)
                            {
                                var responseString = responses.Content;
                                var tempResponse = JsonConvert.DeserializeObject<VdoResponse>(responseString);
                                item.IntroVideo = tempResponse.id;
                            }*/
                            // FileInfo f = new FileInfo(path);
                            // f.Delete();

                            var PostURL = string.Format("https://dev.vdocipher.com/api/videos?title={0}&folderId={1}", item.Name, "b22b4559e0664c66b2eda5ea9221861c");
                            var client = new RestClient(PostURL);
                            var request = new RestRequest(Method.PUT);
                            request.AddHeader("Authorization", "Apisecret gZKDngGYS8bcAb2Xpl8i97qTs75K9pjVaRq6na8LdlLRASOF7OAVqddrB2cy9Ep0");
                            IRestResponse response = client.Execute(request);
                            var tempResponse = JsonConvert.DeserializeObject<UploadRoot>(response.Content);

                            //var clients = new RestClient("https://dev.vdocipher.com/api/videos/importUrl");
                            //var requests = new RestRequest(Method.PUT);
                            //requests.AddHeader("Content-Type", "application/json");
                            //requests.AddHeader("Accept", "application/json");
                            //requests.AddHeader("Authorization", "Apisecret gZKDngGYS8bcAb2Xpl8i97qTs75K9pjVaRq6na8LdlLRASOF7OAVqddrB2cy9Ep0");
                            //+ IntroVideo.FileName
                            //FileStream fs = System.IO.File.OpenRead(Path.GetFileName(IntroVideo.FileName));
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
                            Stream documentConverted = IntroVideo.InputStream;
                            form.Add(new StreamContent(documentConverted), "file", IntroVideo.FileName);
                            var responsess = await _httpClient.PostAsync(tempResponse.clientPayload.uploadLink, form);
                            if ((int)responsess.StatusCode == 201)
                            {
                                item.IntroVideo = tempResponse.videoId;
                            }
                        }
                        else
                        {
                            string path = Path.Combine(Server.MapPath("~/Uploads"), Path.GetFileName(IntroVideo.FileName));
                            IntroVideo.SaveAs(path);
                            //Upload by Dailymotion
                            var accessToken = await DailyMotionHandler.GetAccessTokenAsync();
                            var uploadUrl = await DailyMotionHandler.GetFileUploadUrlAsync(accessToken);
                            var reponse = await DailyMotionHandler.UploadAsync(item.Name, path, accessToken, uploadUrl);
                            
                            if (reponse != null)
                            {
                                var getPrivateKey = await DailyMotionHandler.GetPrivateKeyAsync(reponse.id, accessToken, uploadUrl);
                                item.IntroVideo = getPrivateKey.private_id;
                            }
                            FileInfo f = new FileInfo(path);
                            f.Delete();
                        }
                        await BusinessService.UpdateAsync(item);
                    }
                    try
                    {
                        //add code to course
                        var code = getCourseOrTrackCode();
                        var CourseCode = "C" + item.Id + code;
                        item.CourseCode = CourseCode;
                        await BusinessService.UpdateAsync(item);

                    }
                    catch (Exception e)
                    {

                    }
                    if (item.CourseStatusId==3&& TrackData.BySubscription==true)
                    {
                        var d = Task.Run(async () => { await AddCourseToStudentsInTrack(item.TrackId.Value, item.Id); });

                    }
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
        public async Task<ActionResult> EditWithImage(Course item, HttpPostedFileBase ImageFile ,HttpPostedFileBase IntroVideo)
        {
            if (ModelState.IsValid)
            {
                item.SKUPrice = (((item.CurrentCost * (decimal).30) + item.CurrentCost) * (decimal).30) + item.CurrentCost;
                item.OldSKUPrice = (((item.PreviousCost * (decimal).30) + item.PreviousCost) * (decimal).30) + item.PreviousCost;
               //item.SKUPrice = item.CurrentCost + (((item.CurrentCost) * 30) / 100);
                //item.OldSKUPrice = item.PreviousCost + (((item.PreviousCost) * 30) / 100);
                var TrackSubject = db.Tracks.Where(it => it.Id == item.TrackId).FirstOrDefault();

                item.CourseTrack = item.Name + "-" + TrackSubject.TrackSubject;
                var albumUri = await EditAlbumAsync(item.CourseTrack, item.AlbumUri);
                item.AlbumUri = albumUri;
                if (item.CurrentCost == null || item.PreviousCost == null)
                {
                    ViewBag.Message = "Please enter current price, old price";
                    return View("Create", item);
                }
                var DataReturned = await BusinessService.UpdateAsync(item);
                if (ImageFile != null && DataReturned.HasData == true)
                {
                    try
                    {
                        byte[] thePictureAsBytes = new byte[ImageFile.ContentLength];
                        using (BinaryReader theReader = new BinaryReader(ImageFile.InputStream))
                        {
                            thePictureAsBytes = theReader.ReadBytes(ImageFile.ContentLength);
                        }
                        string thePictureDataAsString = Convert.ToBase64String(thePictureAsBytes);
                        string uri = "http://tollab.com/tollab/api/SetPhoto";
                        var client = new HttpClient();
                        var imageObject = new { RecordId = item.Id, Table = "Course", CoulmnName = "Image", ImageType = (int)ImageFolders.CourseImages, Image = thePictureDataAsString };
                        var response = await client.PostAsJsonAsync(uri, imageObject);
                        var responseString = await response.Content.ReadAsStringAsync();
                        var tempResponse = JObject.Parse(responseString);
                        responseString = tempResponse.ToString();
                        var responseCode = response.StatusCode;
                        if (responseCode == HttpStatusCode.OK)
                        {
                            var img = tempResponse.GetValue("model").ToString();

                        }
                    }
                    catch (Exception ex)
                    { }
                }
                if (IntroVideo != null && DataReturned.HasData == true)
                {
                    if (item.ProviderType == "Vimeo")
                    {
                        await DeleteIntroVideoFromViemoAsync(item.IntroVideoUri);
                        var len = IntroVideo.InputStream.Length;
                        string upload_link = "";
                        string Link = "";
                        string UriWithId = "";
                        string uri = "https://api.vimeo.com/me/videos";
                        var clientTocall = new HttpClient();
                        clientTocall.DefaultRequestHeaders.Authorization
                        = new AuthenticationHeaderValue("Bearer", VimeoToken);
                        clientTocall.DefaultRequestHeaders.Add("Accept", "application/vnd.vimeo.*+json;version=3.4");
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
                            item.IntroVideo = Link;
                            item.IntroVideoUri = UriWithId;
                            TransferVideo(item.AlbumUri, item.IntroVideoUri);

                        }
                        //
                        if (!string.IsNullOrEmpty(upload_link))
                        {
                            var file = new FileInfo(IntroVideo.FileName);
                            var client = new TusClient();
                            var fileUrl = await client.UploadAsync(upload_link, IntroVideo.InputStream, chunkSize: 10D);
                        }

                    }
                    else if (item.ProviderType == "vdocipher")
                    {
                        //string path = Path.Combine(Server.MapPath("~/Uploads_Temp"), Path.GetFileName(IntroVideo.FileName));
                        //IntroVideo.SaveAs(path);
                        var PostURL= string.Format( "https://dev.vdocipher.com/api/videos?title={0}&folderId={1}", item.Name, "b22b4559e0664c66b2eda5ea9221861c");
                        var client = new RestClient(PostURL);
                        var request = new RestRequest(Method.PUT);
                        request.AddHeader("Authorization", "Apisecret gZKDngGYS8bcAb2Xpl8i97qTs75K9pjVaRq6na8LdlLRASOF7OAVqddrB2cy9Ep0");
                        IRestResponse response = client.Execute(request);
                        var tempResponse = JsonConvert.DeserializeObject<UploadRoot>(response.Content);

                        //var clients = new RestClient("https://dev.vdocipher.com/api/videos/importUrl");
                        //var requests = new RestRequest(Method.PUT);
                        //requests.AddHeader("Content-Type", "application/json");
                        //requests.AddHeader("Accept", "application/json");
                        //requests.AddHeader("Authorization", "Apisecret gZKDngGYS8bcAb2Xpl8i97qTs75K9pjVaRq6na8LdlLRASOF7OAVqddrB2cy9Ep0");
                        //+ IntroVideo.FileName
                        //FileStream fs = System.IO.File.OpenRead(Path.GetFileName(IntroVideo.FileName));
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
                        Stream documentConverted = IntroVideo.InputStream;
                        form.Add(new StreamContent(documentConverted), "file", IntroVideo.FileName);
                        var responsess = await _httpClient.PostAsync(tempResponse.clientPayload.uploadLink, form);
                        if ((int)responsess.StatusCode == 201)
                        {
                            item.IntroVideo = tempResponse.videoId;
                        }
                        //var data = new { url = "https://tollab.azurewebsites.net/Uploads_Temp/video2919473527.mp4" , folderId = "b22b4559e0664c66b2eda5ea9221861c", title = item.Name };
                        //var formatedUrl = JsonConvert.SerializeObject(data);

                        //requests.AddParameter("undefined", formatedUrl, ParameterType.RequestBody);
                        //IRestResponse responses = clients.Execute(requests);
                        //if (responses.StatusCode == HttpStatusCode.OK)
                        //{
                        //    var responseString = responses.Content;
                        //    var tempResponse = JsonConvert.DeserializeObject<VdoResponse>(responseString);
                        //    item.IntroVideo = tempResponse.id;
                        //}
                      //  FileInfo f = new FileInfo(path);
                      //  f.Delete();
                    }
                    else
                    {
                        string path = Path.Combine(Server.MapPath("~/Uploads"), Path.GetFileName(IntroVideo.FileName));
                        IntroVideo.SaveAs(path);
                        //Upload by Dailymotion
                        var accessToken = await DailyMotionHandler.GetAccessTokenAsync();
                        var uploadUrl = await DailyMotionHandler.GetFileUploadUrlAsync(accessToken);
                        var reponse = await DailyMotionHandler.UploadAsync(item.Name, path, accessToken, uploadUrl);

                        if (reponse != null)
                        {
                            var getPrivateKey = await DailyMotionHandler.GetPrivateKeyAsync(reponse.id, accessToken, uploadUrl);
                            item.IntroVideo = getPrivateKey.private_id;
                        }
                        FileInfo f = new FileInfo(path);
                        f.Delete();
                    }
                    await BusinessService.UpdateAsync(item);
                }

                if (item.CourseStatusId==3)
                {
                    TeacherNotification teacherNotification = new TeacherNotification
                    {
                        CreationDate = DateTime.UtcNow,
                        ReferenceId = 1,
                        NotificationToId = item.Id,
                        TeacherId = TrackSubject.TeacherId,
                        Title = "You Course Accepted",
                        TitleLT = "تم قبول الدورة الخاصة بكم"
                    };
                    db.TeacherNotifications.Add(teacherNotification);
                    db.SaveChanges();
                    var Tokens = db.TeacherPushTokens.Where(it => it.TeacherId == TrackSubject.TeacherId).ToList();
                    foreach (var token in Tokens)
                    {
                        if (token.OS == "ios")
                        {
                            PushManager.PushToTeacherToIphoneDevice(token.Token, teacherNotification.Title, teacherNotification.NotificationToId, teacherNotification.ReferenceId);
                        }
                        else if (token.OS == "android")
                        {
                            PushManager.pushToAndroidDevice(token.Token, teacherNotification.Title, teacherNotification.NotificationToId, teacherNotification.ReferenceId);
                        }
                    }

                    if (TrackSubject.BySubscription==true)
                    {
                        var d = Task.Run(async () => { await AddCourseToStudentsInTrack(item.TrackId.Value, item.Id); });

                    }



                }
                if (item.CourseStatusId == 4)
                {
                    TeacherNotification teacherNotification = new TeacherNotification
                    {
                        CreationDate = DateTime.UtcNow,
                        ReferenceId = 1,
                        NotificationToId = item.Id,
                        TeacherId = TrackSubject.TeacherId,
                        Title = "You Course Blocked",
                        TitleLT = "تم تعطيل الدورة الخاصة بكم"
                    };
                    db.TeacherNotifications.Add(teacherNotification);
                    db.SaveChanges();
                    var Tokens = db.TeacherPushTokens.Where(it => it.TeacherId == TrackSubject.TeacherId).ToList();
                    foreach (var token in Tokens)
                    {
                        if (token.OS == "ios")
                        {
                            PushManager.PushToTeacherToIphoneDevice(token.Token, teacherNotification.Title, teacherNotification.NotificationToId, teacherNotification.ReferenceId);
                        }
                        else if (token.OS == "android")
                        {
                            PushManager.pushToAndroidDevice(token.Token, teacherNotification.Title, teacherNotification.NotificationToId, teacherNotification.ReferenceId);
                        }
                    }


                }

                return RedirectToIndex(item);
            }
            return View("Edit", item);
        }

        public async Task<bool> AddCourseToStudentsInTrack(long trackId, long CourseId)
        {
            var studentIds = db.Database.SqlQuery<long>("select distinct StudentId from TrackSubscription where TrackId=" + trackId + "").ToList();
            foreach (var studentId in studentIds)
            {
                StudentCourse studentCourse = new StudentCourse() { CourseId = CourseId, StudentId = studentId, EnrollmentDate = DateTime.UtcNow };
                var isFound = db.StudentCourses.Where(i => i.StudentId == studentId && i.CourseId == CourseId).FirstOrDefault();
                if (isFound == null)
                {
                    db.StudentCourses.Add(studentCourse);
                    await db.SaveChangesAsync();

                }
            }
            return true;
        }


        public async Task<string> EditAlbumAsync(string CourseTrack,string AlbumUri)
        {
            string NewAlbumUri = "";
            string uri = "https://api.vimeo.com/users/101981438"+ AlbumUri;
            var method = new HttpMethod("PATCH");
            var parameters = new Dictionary<string, object> { { "name", CourseTrack } };
            var requestPatch = new HttpRequestMessage(method, uri)
            {
                 Content= new StringContent("{ \"name\": \""+CourseTrack+"\" }", System.Text.Encoding.UTF8, "application/vnd.vimeo.album+json")
            };

            var clientTocall = new HttpClient();
            clientTocall.DefaultRequestHeaders.Authorization
            = new AuthenticationHeaderValue("Bearer", VimeoToken);
            clientTocall.DefaultRequestHeaders.Add("Accept", "application/vnd.vimeo.*+json;version=3.4");
            var VideoObject = new { name = CourseTrack, upload = new { approach = "tus" }, privacy = new { view = "disable" } };
            var response = await clientTocall.SendAsync(requestPatch);
            var responseString = await response.Content.ReadAsStringAsync();
            var tempResponse = JObject.Parse(responseString);
            responseString = tempResponse.ToString();
            var responseCode = response.StatusCode;
            if (responseCode == HttpStatusCode.OK)
            {
                NewAlbumUri = (string)tempResponse["uri"];
                return NewAlbumUri;
            }
            return AlbumUri;
        }


        public async Task<string> CreateAlbumAsync(string CourseTrack)
        {
            string AlbumUri = "";
            string uri = "https://api.vimeo.com/users/101981438/albums";
            var clientTocall = new HttpClient();
            clientTocall.DefaultRequestHeaders.Authorization
            = new AuthenticationHeaderValue("Bearer", VimeoToken);
            clientTocall.DefaultRequestHeaders.Add("Accept", "application/vnd.vimeo.*+json;version=3.4");
            var VideoObject = new { name = CourseTrack, upload = new { approach = "tus" }, privacy = new { view = "disable" } };
            var response = await clientTocall.PostAsJsonAsync(uri, VideoObject);
            var responseString = await response.Content.ReadAsStringAsync();
            var tempResponse = JObject.Parse(responseString);
            responseString = tempResponse.ToString();
            var responseCode = response.StatusCode;
            if (responseCode == HttpStatusCode.Created)
            {
                AlbumUri = (string)tempResponse["uri"];
                return AlbumUri;
            }
            return AlbumUri;
        }

        private async Task DeleteIntroVideoFromViemoAsync(string introVideoUri)
        {
            var uri = "https://api.vimeo.com" + introVideoUri;

            var clientTocall = new HttpClient();
            clientTocall.DefaultRequestHeaders.Authorization
            = new AuthenticationHeaderValue("Bearer", VimeoToken);
            clientTocall.DefaultRequestHeaders.Add("Accept", "application/vnd.vimeo.*+json;version=3.4");
            var response = await clientTocall.DeleteAsync(uri);

            var responseString = await response.Content.ReadAsStringAsync();
           

        }

        public async Task TransferVideo(string AlbumUri, string VideoUri)
        {
            var transferUri = "https://api.vimeo.com" + "" + AlbumUri + "" + "" + VideoUri + "";
            var clientTocall = new HttpClient();
            clientTocall.DefaultRequestHeaders.Authorization
            = new AuthenticationHeaderValue("Bearer", VimeoToken);
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
        public string getCourseOrTrackCode()
        {

            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = alphabets + small_alphabets + numbers;

            int length = 4;

            string otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            return otp;
        }
        [Serializable]

        public class ClientPayload
        {
            public string policy { get; set; }
            public string key { get; set; }

            [JsonProperty("x-amz-signature")]
            public string xamzsignature { get; set; }

            [JsonProperty("x-amz-algorithm")]
            public string xamzalgorithm { get; set; }

            [JsonProperty("x-amz-date")]
            public string xamzdate { get; set; }

            [JsonProperty("x-amz-credential")]
            public string xamzcredential { get; set; }
            public string uploadLink { get; set; }
        }
        [Serializable]

        public class UploadRoot
        {
            public ClientPayload clientPayload { get; set; }
            public string videoId { get; set; }
        }


        [Serializable]
        public class VdoResponse
        {
            public string id { get; set; }
            public string title { get; set; }
            public string description { get; set; }
            public int upload_time { get; set; }
            public string length { get; set; }
            public string status { get; set; }
            public string @public { get; set; }
            public string poster { get; set; }
        }


        public async Task<ActionResult> ClearEnrollment(long? CourseId )
        {
            ViewBag.CourseId = CourseId;
            ViewBag.StudentCourseCount = db.StudentCourses.Where(i => i.CourseId == CourseId).Count();
            return View();
        }
        public async Task<ActionResult> ClearAllStudentCourses(long CourseId)
        {
            string sql = " Delete from StudentCourse where CourseId=" + CourseId + "";
            var r = db.Database.ExecuteSqlCommand(sql);
            return RedirectToAction("index");
        }
    }
}
