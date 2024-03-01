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

namespace Tollab.Admin.Web.Controllers
{
    [Authorize]
    public class TrackSubscriptionsController : BaseWebController<TrackSubscription,TrackSubscription>
    {
        private TollabContext db = new TollabContext();

        public TrackSubscriptionsController(IBusinessService<TrackSubscription, TrackSubscription> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Create(TrackSubscription trackSubscription)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var track = db.Tracks.Where(item => item.Id == trackSubscription.TrackId).FirstOrDefault();
                    var Isfound = db.TrackSubscriptions.Where(item=>item.TrackId==trackSubscription.TrackId).Where(item=>item.StudentId==trackSubscription.StudentId).FirstOrDefault();
                    if (track.BySubscription==false)
                    {
                        ViewBag.Error = "This Track by Course Subscription";
                        return View(trackSubscription);

                    }
                    if (Isfound!=null)
                    {
                        ViewBag.Error = "This Student Subscriped this track before";
                        return View(trackSubscription);
                    }
                    trackSubscription.CreationDate = DateTime.UtcNow;
                    await BusinessService.CreateAsync(trackSubscription);
                    return RedirectToAction("Index");
                }
                return View(trackSubscription);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Edit(TrackSubscription  trackSubscription)
        {
            if (ModelState.IsValid)
            {
                var oldModel = await BusinessService.GetAsync(trackSubscription.Id);
                var track = db.Tracks.Where(item => item.Id == trackSubscription.TrackId).FirstOrDefault();
                if (track.BySubscription == false)
                {
                    ViewBag.Error = "This Track by Course Subscription";
                    return View(trackSubscription);

                }
                var Isfound = db.TrackSubscriptions.Where(item => item.TrackId == trackSubscription.TrackId).Where(item => item.StudentId == trackSubscription.StudentId).FirstOrDefault();
                if (oldModel.Data.FirstOrDefault().TrackId!=trackSubscription.TrackId)
                {
                    if (Isfound != null)
                    {
                        ViewBag.Error = "This Student Subscriped this track before";
                        return View(trackSubscription);
                    }
                }
                if (oldModel.Data.FirstOrDefault().StudentId != trackSubscription.StudentId)
                {
                    if (Isfound != null)
                    {
                        ViewBag.Error = "This Student Subscriped this track before";
                        return View(trackSubscription);
                    }
                }

                await BusinessService.UpdateAsync(trackSubscription);
                return RedirectToIndex(trackSubscription);
            }
            return View(trackSubscription);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> DeleteConfirmed(long? id)
        {
            var data = await BusinessService.GetAsync(id.Value);
            if (data.HasData)
            {
                var trakid = data.Data.FirstOrDefault().TrackId;
                var studentId = data.Data.FirstOrDefault().StudentId;
                var sql = @"delete from StudentCourse where Id In (
                        select StudentCourse.Id from StudentCourse join Course on StudentCourse.CourseId=Course.Id
                        join Track on Course.TrackId=Track.Id where Track.Id=" + trakid + " and StudentCourse.StudentId=" + studentId + ")";
                await db.Database.ExecuteSqlCommandAsync(sql);
            }
            var response = await BusinessService.DeleteAsync(id.Value);
            if (response.HasErrors)
            {
                string viewName = GetViewName("_DeleteFailed");
                return View(viewName, response.Errors);
            }


            return RedirectToAction("Index");
        }


    }
}
