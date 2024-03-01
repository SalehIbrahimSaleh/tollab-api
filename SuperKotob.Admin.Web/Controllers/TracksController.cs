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
using Newtonsoft.Json.Linq;
using Tollab.Admin.Web.Utils;
using Tollab.Admin.Web.Services.FileUpload;
using SuperKotob.Models;

namespace Tollab.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin")]

    public class TracksController : BaseWebController<Track,Track>
    {
        private TollabContext db = new TollabContext();
        private IFileUploadService _imageUploadService = new FileUploadService();

        public TracksController(IBusinessService<Track, Track> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateWithImage(Track item, HttpPostedFileBase ImageFile, HttpPostedFileBase ImageFileHomeCover)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    item.SKUPrice = (((item.SubscriptionCurrentPrice * (decimal).30) + item.SubscriptionCurrentPrice) * (decimal).30) + item.SubscriptionCurrentPrice;
                    item.OldSKUPrice = (((item.SubscriptionOldPrice * (decimal).30) + item.SubscriptionOldPrice) * (decimal).30) + item.SubscriptionOldPrice;

                    //item.SKUPrice = item.SubscriptionCurrentPrice + (((item.SubscriptionCurrentPrice) * 30) / 100);
                    //item.OldSKUPrice = item.SubscriptionOldPrice + (((item.SubscriptionOldPrice) * 30) / 100);
                    var SubjectDepartment = db.Subjects.Where(it => it.Id == item.SubjectId).Select(it => it.SubjectDepartment).FirstOrDefault();

                    item.TrackSubject = item.Name + "-" + SubjectDepartment;
                    string sql = @"select * from Track where ( Name Like '" + item.Name + "' or NameLT Like '" + item.NameLT + "' ) And TeacherId=" + item.TeacherId + " And SubjectId=" + item.SubjectId + "";
                    var IsFound = db.Database.SqlQuery<Track>(sql).FirstOrDefault();
                    if (IsFound != null)
                    {
                        ViewBag.Message = "This track added before to this teacher";
                        return View("Create", item);
                    }
                    if (item.BySubscription == true & (item.SubscriptionOldPrice == null || item.SubscriptionCurrentPrice == null || item.SubscriptionDuration == null))
                    {
                        ViewBag.Message = "Please enter current price, old price and subscription duration";
                        return View("Create", item);
                    }
                    var DataReturned = await BusinessService.CreateAsync(item);
                    if (ImageFile != null && DataReturned.HasData == true )
                    {
                        await _imageUploadService.UploadImage(item.Id, nameof(Track), nameof(Track.Image), (int)ImageFolders.TeacherSubjectImages, ImageFile);
                      
                    }
                    if(DataReturned.HasData == true  && ImageFileHomeCover != null)
                    {
                        await _imageUploadService.UploadImage(item.Id, nameof(Track), nameof(Track.ImageHomeCover), (int)ImageFolders.TeacherSubjectImages, ImageFileHomeCover);
                    }
                    try
                    {
                        //add code to course
                        var code = getCourseOrTrackCode();
                        var TrackCode = "T" + item.Id + code;
                        item.TrackCode = TrackCode;
                        await BusinessService.UpdateAsync(item);

                    }
                    catch (Exception e)
                    {

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
        public async Task<ActionResult> EditWithImage(Track item, HttpPostedFileBase ImageFile, HttpPostedFileBase ImageFileHomeCover)
        {
            if (ModelState.IsValid)
            {
                item.SKUPrice = (((item.SubscriptionCurrentPrice * (decimal).30) + item.SubscriptionCurrentPrice) * (decimal).30) + item.SubscriptionCurrentPrice;
                item.OldSKUPrice = (((item.SubscriptionOldPrice * (decimal).30) + item.SubscriptionOldPrice) * (decimal).30) + item.SubscriptionOldPrice;
                //item.SKUPrice = item.SubscriptionCurrentPrice + (((item.SubscriptionCurrentPrice) * 30) / 100);
                //item.OldSKUPrice = item.SubscriptionOldPrice + (((item.SubscriptionOldPrice) * 30) / 100);
                var OldTrack = db.Tracks.Where(it => it.Id == item.Id).FirstOrDefault();
                if (OldTrack.Name != item.Name || OldTrack.NameLT != item.NameLT)
                {


                    string sql = @"select * from Track where ( Name Like '" + item.Name + "' or NameLT Like '" + item.NameLT + "' ) And TeacherId=" + item.TeacherId + " And SubjectId=" + item.SubjectId + "";
                    var IsFound = db.Database.SqlQuery<Track>(sql).FirstOrDefault();
                    if (IsFound != null)
                    {
                        ViewBag.Message = "This track added before to this teacher";
                        return View("Edit", item);
                    }
                }
                if (item.BySubscription == true & (item.SubscriptionOldPrice == null || item.SubscriptionCurrentPrice == null || item.SubscriptionDuration == null))
                {
                    ViewBag.Message = "Please enter current price, old price and subscription duration";
                    return View("Edit", item);

                }
                var SubjectDepartment = db.Subjects.Where(it => it.Id == item.SubjectId).Select(it => it.SubjectDepartment).FirstOrDefault();
                item.TrackSubject = item.Name + "-" + SubjectDepartment;
                var DataReturned = await BusinessService.UpdateAsync(item);
                
                if (DataReturned.HasData == true && ImageFileHomeCover != null)
                {
                    await _imageUploadService.UploadImage(item.Id, nameof(Track), nameof(Track.ImageHomeCover), (int)ImageFolders.TeacherSubjectImages, ImageFileHomeCover);
                }
                if (ImageFile != null && DataReturned.HasData == true)
                {
                    await _imageUploadService.UploadImage(item.Id, nameof(Track), nameof(Track.Image), (int)ImageFolders.TeacherSubjectImages, ImageFile); 
                }
                return RedirectToIndex(item);
            }
            return View("Edit", item);
        }
        public virtual async Task<ActionResult> AddPromotion(long? id)
        {
            if (id == null || id.Value < 1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var response = await BusinessService.GetAsync(id.Value);

            if (!response.HasData || response.Data.FirstOrDefault() == null)
                return HttpNotFound();

            var data = response.Data.FirstOrDefault();
            
            string viewName = GetViewName("AddPromotion");
            //ViewData["CoursesData"] = data.Courses.ToList();

            return View(viewName, data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveNewPromotion(Track item)
        {
            if (ModelState.IsValid)
            {
                var OldTrack = db.Tracks.Where(it => it.Id == item.Id).FirstOrDefault();
                if (OldTrack.Name != item.Name || OldTrack.NameLT != item.NameLT)
                {


                    string sql = @"select * from Track where ( Name Like '" + item.Name + "' or NameLT Like '" + item.NameLT + "' ) And TeacherId=" + item.TeacherId + " And SubjectId=" + item.SubjectId + "";
                    var IsFound = db.Database.SqlQuery<Track>(sql).FirstOrDefault();
                    if (IsFound != null)
                    {
                        ViewBag.Message = "This track added before to this teacher";
                        return View("Edit", item);
                    }
                }
                if (item.BySubscription == true & (item.SubscriptionOldPrice == null || item.SubscriptionCurrentPrice == null || item.SubscriptionDuration == null))
                {
                    ViewBag.Message = "Please enter current price, old price and subscription duration";
                    return View("Edit", item);

                }
                var SubjectDepartment = db.Subjects.Where(it => it.Id == item.SubjectId).Select(it => it.SubjectDepartment).FirstOrDefault();
                item.TrackSubject = item.Name + "-" + SubjectDepartment;
                var DataReturned = await BusinessService.UpdateAsync(item);
                 
                
                return RedirectToIndex(item);
            }
            return View("AddPromotion", item);
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


        public async Task<ActionResult> ClearSubscriptions(long? TrackId)
        {
            ViewBag.TrackId = TrackId;
            ViewBag.TrackSubscriptionCount = db.TrackSubscriptions.Where(i => i.TrackId == TrackId).Count();
            return View();
        }
        public async Task<ActionResult> ClearAllSubscriptions(long TrackId)
        {
            string sql = " Delete from TrackSubscription where TrackId=" + TrackId + "";
            var r = db.Database.ExecuteSqlCommand(sql);
            return RedirectToAction("index");
        }


        public async Task<ActionResult> SetSKUPriceByZero(string Password = null)
        {
            if (string.IsNullOrEmpty(Password))
            {
                return View();
            }
            if (!Password.Equals("Tollab2021"))
            {
                return View();
            }
            var sql = @"
update Course set SKUPrice=0,OldSKUPrice=0;
update Track set SKUPrice=0,OldSKUPrice=0;
update Live set CurrentSKUPrice=0,OldSKUPrice=0;";

            var d = await db.Database.ExecuteSqlCommandAsync(sql);
            if (d > 0)
            {
                return RedirectToAction("Success", "Home");

            }
            return RedirectToAction("Error", "Home");

        }

        public async Task<ActionResult> ResetSKUPriceToOrginalPrice(string Password = null)
        {

            if (string.IsNullOrEmpty(Password))
            {
                return View();
            }
            if (!Password.Equals("Tollab2021"))
            {
                return View();
            }
            var sql = @"
update Course set SKUPrice=(CurrentCost*1.3),OldSKUPrice=(PreviousCost*1.3);

update Track set SKUPrice=SubscriptionCurrentPrice*1.3,OldSKUPrice=SubscriptionOldPrice*1.3;
update Live set CurrentSKUPrice=(CurrentPrice*1.3),OldSKUPrice=(OldPrice*1.3);";

            var d = await db.Database.ExecuteSqlCommandAsync(sql);
            if (d > 0)
            {
                return RedirectToAction("Success", "Home");

            }
            return RedirectToAction("Error", "Home");
        }


    }
}
