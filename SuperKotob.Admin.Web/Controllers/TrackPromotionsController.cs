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

    public class TrackPromotionsController : BaseWebController<TrackPromotion, TrackPromotion>
    {
        private TollabContext db = new TollabContext();
        private IFileUploadService _imageUploadService = new FileUploadService();

        public TrackPromotionsController(IBusinessService<TrackPromotion, TrackPromotion> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }
        public virtual async Task<ActionResult> AddPromotion(long? id)
        {
            if (id == null || id.Value < 1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var courses_sql = @"select * from Course where TrackId =" + id +" Order by OrderNumber Asc";
            var courses= db.Database.SqlQuery<Course>(courses_sql).ToList();
            var track_sql = @"select * from Track where Id =" + id;
            var track = db.Database.SqlQuery<Track>(track_sql).ToList();

            string viewName = GetViewName("AddPromotion");
            ViewData["CoursesData"] = courses.ToList();
            ViewData["TrackData"] = track.ToList();
            return View(viewName, new TrackPromotion());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateWithImage(long? id,TrackPromotion item, HttpPostedFileBase ImageFile)
        {
            try
            {
                 
                if (ModelState.IsValid)
                {
                    var courses = db.Database.SqlQuery<Course>(@"select * from Course where Id in(" + item.CoursesConcat + ")").ToList();
                    var totalSkuPrice = courses.Sum(x => x.SKUPrice);
                    var totalPrice = courses.Sum(x => x.CurrentCost).Value;
                    item.ChachedPrice = totalPrice;
                    if (item.DiscountType == "Fixed")
                    {
                        item.NewPrice = item.DiscountValue;
                    }
                    else {
                        
                        var percentage = (decimal)(item.DiscountValue)/100;
                        item.NewPrice =(((totalPrice * percentage) + totalPrice) * percentage) + totalPrice;
                        
                    }
                     item.SkuPrice = (((item.NewPrice * (decimal).30) + item.NewPrice) * (decimal).30) + item.NewPrice;

                    var courseItems = item.CoursesConcat.Split(',').Select(Int64.Parse).ToList();

                    foreach (var courseitem in courseItems)
                    {
                        var dd = new TrackPromotionCourse();
                        dd.CourseId = courseitem;
                        dd.TrackPromotionId = item.Id;
                        item.TrackPromotionCourses.Add(dd);
                    }
                    
                        var DataReturned = await BusinessService.CreateAsync(item);
                    if (ImageFile != null && DataReturned.HasData == true)
                    {
                        await _imageUploadService.UploadImage(item.Id, nameof(TrackPromotion), nameof(TrackPromotion.Image), (int)ImageFolders.TeacherSubjectImages, ImageFile);

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
        public async Task<ActionResult> EditWithImage(TrackPromotion item, HttpPostedFileBase ImageFile, HttpPostedFileBase ImageFileHomeCover)
        {
            if (ModelState.IsValid)
            {
                var DataReturned = await BusinessService.UpdateAsync(item);
                
                
                return RedirectToIndex(item);
            }
            return View("Edit", item);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveNewPromotion(TrackPromotion item)
        {
            if (ModelState.IsValid)
            {
                 var DataReturned = await BusinessService.UpdateAsync(item);
                 
                
                return RedirectToIndex(item);
            }
            return View("AddPromotion", item);
        }


        [HttpGet]
        public  override async Task<ActionResult> Delete(long? id)
        {
            var data = await BusinessService.GetAsync(id.Value);
            if (data.HasData)
            {
                //var trakid = data.Data.FirstOrDefault().TrackId;
               // var studentId = data.Data.FirstOrDefault().StudentId;
                var sql = @"delete from TrackPromotionCourse where TrackPromotionId="+id;
                await db.Database.ExecuteSqlCommandAsync(sql);
                var sql2 = @" delete from TrackPromotion where Id =" + id;
                await db.Database.ExecuteSqlCommandAsync(sql2);
            }
           


            return RedirectToAction("Index");
        }
    }
}
