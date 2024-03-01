using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using RestSharp;
using SuperKotob.Admin.Core;
using SuperKotob.Admin.Utils.Configuration;
using SuperKotob.Admin.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Tollab.Admin.Core.Enums;
using Tollab.Admin.Data.Models;
using Tollab.Admin.Web.Services.FileUpload;
using Tollab.Admin.Web.Services.Vimeo;
using Tollab.Admin.Web.Services.Zoom;
using Tollab.Admin.Web.Utils;

namespace Tollab.Admin.Web.Controllers
{
    public class LivesController : BaseWebController<Live, Live>
    {
        private const string VimeoToken = "2878d5dde0fe009cc71041e7c82d5292";
        private string PasswordForVideo = "Tollab@hacker!@#%^&*(@147852";

        private IFileUploadService _imageUploadService = new FileUploadService();
        private readonly IZoomManagerService _zoomManagerService = new ZoomManagerService();
        private IVimeoUploadService _vimeoUploadService = new VimeoUploadService();

        public LivesController(IBusinessService<Live, Live> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> CreateWithImage(Live item, HttpPostedFileBase ImageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    item.IsActive = true;
                    item.CurrentSKUPrice = (((item.CurrentPrice * (decimal).30) + item.CurrentPrice) * (decimal).30) + item.CurrentPrice;
                    item.OldSKUPrice = (((item.OldPrice * (decimal).30) + item.OldPrice) * (decimal).30) + item.OldPrice;
                    //item.CurrentSKUPrice = item.CurrentPrice + (((item.CurrentPrice) * 30) / 100);
                    //item.OldSKUPrice = item.OldPrice + (((item.OldPrice) * 30) / 100);
                    switch (item.LiveLinkType)
                    {
                        case LiveLinkType.TRACK:
                            item.CourseId = null;
                            break;
                        case LiveLinkType.COURSE:
                            item.TrackId = null;
                            break;
                        case LiveLinkType.OTHER:
                            item.CourseId = null;
                            item.TrackId = null;
                            break;
                    }
                    if (ImageFile != null)
                    {
                        try
                        {
                            var (statusCode, hostURL, joinURL, meetingId, password ) = _zoomManagerService.CreateSchedualMeeting(item.LiveName, item.Duration, item.LiveDate, item.CountryId,item.ZoomAccount.Value);
                            if(statusCode == HttpStatusCode.OK || statusCode == HttpStatusCode.Created)
                            {
                                item.HostURL = hostURL;
                                item.JoinURL = joinURL;
                                item.ZoomMeetingId = meetingId;
                                item.MeetingPassword = password;
                                var DataReturned = await BusinessService.CreateAsync(item);

                                if (DataReturned.HasData)
                                {
                                    await _imageUploadService.UploadImage(item.Id, nameof(Live), nameof(Live.Image), (int)ImageFolders.LiveImages, ImageFile);
                                }
                            }
                        }
                        catch (Exception ex)
                        { throw ex; }
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


        

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> EditWithImage(Live item, HttpPostedFileBase ImageFile , HttpPostedFileBase VideoFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                        if (item.IsActive == false)
                        {
                            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["TollabContext"].ConnectionString;
                            SqlConnection cnn = new SqlConnection(cnnString);
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = cnn;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.CommandText = "UpdateStudentCourseWithStudentLive";
                            cmd.Parameters.AddWithValue("@LiveId", item.Id);
                            cnn.Open();
                            object o = cmd.ExecuteScalar();
                            cnn.Close();
                        }
                        await _imageUploadService.UploadImage(item.Id, nameof(Live), nameof(Live.Image), (int)ImageFolders.LiveImages, ImageFile);
                    item.CurrentSKUPrice = (((item.CurrentPrice * (decimal).30) + item.CurrentPrice) * (decimal).30) + item.CurrentPrice;
                    item.OldSKUPrice = (((item.OldPrice * (decimal).30) + item.OldPrice) * (decimal).30) + item.OldPrice;
                    //item.CurrentSKUPrice = item.CurrentPrice + (((item.CurrentPrice) * 30) / 100);
                    //item.OldSKUPrice = item.OldPrice + (((item.OldPrice) * 30) / 100);
                    switch (item.LiveLinkType)
                    {
                        case LiveLinkType.TRACK:
                            item.CourseId = null;
                            break;
                        case LiveLinkType.COURSE:
                            item.TrackId = null;
                            break;
                        case LiveLinkType.OTHER:
                            item.CourseId = null;
                            item.TrackId = null;
                            break;
                    }
                    var oldItem = (await BusinessService.GetAsync(item.Id)).Data.FirstOrDefault();
                    if (ImageFile != null)
                    {
                        try
                        {
                            if (oldItem.VideoURI != null && VideoFile != null)
                            {
                                _vimeoUploadService.DeleteFromViemoAsync(oldItem.VideoURI, VimeoToken);
                            }
                            var statusCode = _zoomManagerService.UpdateSchedualMeeting(item.LiveName, item.Duration, item.LiveDate, item.CountryId, oldItem.ZoomMeetingId.Value,item.ZoomAccount.Value);
                            if (statusCode == HttpStatusCode.NoContent)
                            {

                                if (VideoFile != null)
                                {
                                    var (success, videoURL, videoURI, errorDetails) = await _vimeoUploadService.TryUpload(VideoFile, item.LiveName, VideoFile.ContentLength, PasswordForVideo, VimeoToken);
                                    if(success)
                                    {
                                        // here will be logic for creating new course
                                        item.VideoURI = videoURI;
                                        item.VideoURL = videoURL;
                                    }
                                }
                                item.MeetingPassword = oldItem.MeetingPassword;
                                var DataReturned = await BusinessService.UpdateAsync(item);
                                
                               
                            }
                        }
                        catch (Exception ex)
                        { }
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


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> DeleteConfirmed(long? id)
        {
            var oldItem = (await BusinessService.GetAsync(id.Value)).Data.FirstOrDefault();
            var status = _zoomManagerService.Delete(oldItem.ZoomMeetingId.Value);
            if (oldItem != null)
            {
                var response = await BusinessService.DeleteAsync(id.Value);
                if (response.HasErrors)
                {
                    string viewName = GetViewName("_DeleteFailed");
                    return View(viewName, response.Errors);
                }
                else if (oldItem.VideoURI != null)
                    await _vimeoUploadService.DeleteFromViemoAsync(oldItem.VideoURI, VimeoToken);
            }

            return RedirectToAction("Index");
        }
    }
}