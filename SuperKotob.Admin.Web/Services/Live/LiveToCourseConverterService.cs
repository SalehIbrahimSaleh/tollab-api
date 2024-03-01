using Newtonsoft.Json.Linq;
using SuperKotob.Admin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Tollab.Admin.Data.Models;
using Tollab.Admin.UseCases.Contents;
using Tollab.Admin.UseCases.Courses;
using Tollab.Admin.UseCases.Groups;
using Tollab.Admin.UseCases.Tracks;

namespace Tollab.Admin.Web.Services.Live
{
    public class LiveToCourseConverterService : ILiveToCourseConverterService
    {
        private const string VimeoToken = "2878d5dde0fe009cc71041e7c82d5292";
        private readonly CourseRepository _courseRepository;
        private readonly GroupRepository _groupRepository;
        private readonly ContentRepository _contentRepository;
        private readonly TrackRepository _trackRepository;

        public LiveToCourseConverterService()
        {
            _courseRepository = new CourseRepository();
            _groupRepository = new GroupRepository();
            _contentRepository = new ContentRepository();
            _trackRepository = new TrackRepository();
        }
        public async Task ConvertToCourse(Data.Models.Live live)
        {
            if(live.TrackId != null)
            {
                //create course with live price and track
                var courseCreateResult = await CreateCourseFromLive(live);
                if(courseCreateResult.HasData)
                {
                    //create group for the previously craeted course 
                    var groupCreatedResult = await CreateGroup(courseCreateResult.Data.FirstOrDefault().Id, 
                        courseCreateResult.Data.FirstOrDefault().CourseTrack);
                    if(groupCreatedResult.HasData)
                    {
                        //create content for the previously created group
                        var contentCreatedResult = await CreateContent(live.Duration, groupCreatedResult.Data.FirstOrDefault().Id, live.VideoURL, live.VideoURI);
                    }
                }
            }
            throw new NotImplementedException();
        }



        private async Task<DataResponse<Course>> CreateCourseFromLive(Data.Models.Live live)
        {
            //create image for course from live
            var course = new Course()
            {
                Name = live.LiveName,
                NameLT = live.LiveName,
                CurrentCost = live.CurrentPrice,
                PreviousCost = live.OldPrice,
                SKUNumber = live.SKUNumber,
                SKUPrice = live.CurrentSKUPrice,
                OldSKUPrice = live.OldSKUPrice,
                TrackId = live.TrackId,
                //change to ToCourseOrderNumber
                OrderNumber = live.CourseOrder,
                CreationDate = DateTime.UtcNow,
            };
            var TrackData = await _trackRepository.GetAsync(live.TrackId.Value);
            var TrackSubject = TrackData.Data.FirstOrDefault().TrackSubject;
            course.CourseTrack = course.Name + "-" + TrackSubject;
            var Albumuri = await CreateAlbumAsync(course.CourseTrack);
            course.AlbumUri = Albumuri;
            var result = await _courseRepository.CreateAsync(course);
            return result;
        }

        private async Task<DataResponse<Group>> CreateGroup(long courseId, string courseTrack)
        {
            var group = new Group()
            {
                Name = "Videos",
                NameLT = "شرح",
                CourseId = courseId,
            };
            group.GroupCourse = group.Name + "-" + courseTrack;
            return await _groupRepository.CreateAsync(group);
        }

        private async Task<DataResponse<Content>> CreateContent(int duration, long groupId, string path, string videoUri)
        {
            var content = new Content()
            {
                Name = "Part 01",
                NameLT = "الجزء الاول",
                ContentTypeId = 1,
                Duration = duration,
                GroupId = duration,
                Path = path,
                VideoUri = videoUri
            };
            return await _contentRepository.CreateAsync(content);
        }
        private async Task<string> CreateAlbumAsync(string CourseTrack)
        {
            string AlbumUri = "";
            string uri = "https://api.vimeo.com/users/101981438/albums";
            var clientTocall = new HttpClient();
            clientTocall.DefaultRequestHeaders.Authorization= new AuthenticationHeaderValue("Bearer", VimeoToken);
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
    }
}