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
using Tollab.Admin.Web.Utils;

namespace Tollab.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin")]

    public class TeacherNotificationsController : BaseWebController<TeacherNotification,TeacherNotification>
    {
        private TollabContext db = new TollabContext();

        public TeacherNotificationsController(IBusinessService<TeacherNotification, TeacherNotification> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Create(TeacherNotification  teacherNotification)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (teacherNotification.TeacherId > 0)
                    {
                        teacherNotification.CreationDate = DateTime.UtcNow;
                        teacherNotification.ReferenceId = 4;
                        teacherNotification.NotificationToId = 0;
                        var Tokens = db.TeacherPushTokens.Where(item => item.TeacherId == teacherNotification.TeacherId).ToList();
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

                     
                        await BusinessService.CreateAsync(teacherNotification);
                        return RedirectToAction("Index");
                    }
                    if (teacherNotification.TeacherId == null)
                    {
                        var teacherIds = db.Teachers.Select(item => item.Id).ToList();
                        foreach (var teacherId in teacherIds)
                        {
                            teacherNotification.TeacherId = teacherId;
                            teacherNotification.CreationDate = DateTime.UtcNow;
                            teacherNotification.ReferenceId = 4;
                            teacherNotification.NotificationToId = 0;
                            var Tokens = db.TeacherPushTokens.Where(item => item.TeacherId == teacherId).ToList();
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
                        
                            await BusinessService.CreateAsync(teacherNotification);

                        }
                        return RedirectToAction("Index");
                    }
                }
                return View(teacherNotification);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

    }
}
