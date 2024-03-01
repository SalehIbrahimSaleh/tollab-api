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

    public class StudentNotificationsController : BaseWebController<StudentNotification,StudentNotification>
    {
        private TollabContext db = new TollabContext();

        public StudentNotificationsController(IBusinessService<StudentNotification, StudentNotification> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Create(StudentNotification  studentNotification)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<long> StudentIds = new List<long>();

                    if (studentNotification.StudentId>0)
                    {
                        studentNotification.CreationDate = DateTime.UtcNow;
                        studentNotification.ReferenceId = 4;
                        studentNotification.NotificationToId = 0;
                        var Tokens=   db.StudentPushTokens.Where(item => item.StudentId == studentNotification.StudentId).ToList();
                        foreach (var token in Tokens)
                        {
                            if (token.OS == "ios")
                            {
                                PushManager.PushToStudentToIphoneDevice(token.Token, studentNotification.Title, studentNotification.NotificationToId, studentNotification.ReferenceId);
                            }
                            else if (token.OS == "android")
                            {
                                PushManager.pushToAndroidDevice(token.Token, studentNotification.Title, studentNotification.NotificationToId, studentNotification.ReferenceId);
                            }
                        }

                      
                        await BusinessService.CreateAsync(studentNotification);
                        return RedirectToAction("Index");
                    }
                    if (studentNotification.CountryId > 0)
                    {
                        studentNotification.CreationDate = DateTime.UtcNow;
                        studentNotification.ReferenceId = 4;
                        studentNotification.NotificationToId = 0;
                        var Tokens = db.Database.SqlQuery<StudentPushToken>(@"select Distinct StudentPushToken.* from StudentPushToken join Student on StudentPushToken.StudentId=Student.Id
                                                                            where Student.CountryId =" + studentNotification.CountryId+"").ToList();
                        foreach (var token in Tokens)
                        {
                            if (!StudentIds.Contains(token.StudentId))
                            {
                                studentNotification.StudentId = token.StudentId;
                                await BusinessService.CreateAsync(studentNotification);
                            }
                            StudentIds.Add(token.StudentId);

                            if (token.OS == "ios")
                            {
                                PushManager.PushToStudentToIphoneDevice(token.Token, studentNotification.Title, studentNotification.NotificationToId, studentNotification.ReferenceId);
                            }
                            else if (token.OS == "android")
                            {
                                PushManager.pushToAndroidDevice(token.Token, studentNotification.Title, studentNotification.NotificationToId, studentNotification.ReferenceId);
                            }

                        }


                        return RedirectToAction("Index");
                    }
                    if (studentNotification.CategoryId > 0)
                    {
                        studentNotification.CreationDate = DateTime.UtcNow;
                        studentNotification.ReferenceId = 4;
                        studentNotification.NotificationToId = 0;
                        var Tokens = db.Database.SqlQuery<StudentPushToken>(@"select StudentPushToken.* from StudentPushToken join  StudentDepartment on StudentPushToken.StudentId=StudentDepartment.StudentId
 join Department on StudentDepartment.DepartmentId=Department.Id
 join SubCategory on SubCategory.Id=Department.SubCategoryId 
 where SubCategory.CategoryId=" + studentNotification.CategoryId + "").ToList();
                        foreach (var token in Tokens)
                        {
                            if (!StudentIds.Contains(token.StudentId))
                            {
                                studentNotification.StudentId = token.StudentId;
                                await BusinessService.CreateAsync(studentNotification);
                            }
                            StudentIds.Add(token.StudentId);
                            if (token.OS == "ios")
                            {
                                PushManager.PushToStudentToIphoneDevice(token.Token, studentNotification.Title, studentNotification.NotificationToId, studentNotification.ReferenceId);
                            }
                            else if (token.OS == "android")
                            {
                                PushManager.pushToAndroidDevice(token.Token, studentNotification.Title, studentNotification.NotificationToId, studentNotification.ReferenceId);
                            }
                         }

                        return RedirectToAction("Index");
                    }
                    if (studentNotification.StudentId ==null)
                    {
                        var studentIds = db.Students.Select(item => item.Id).ToList();
                        foreach (var studentId in studentIds)
                        {
                            studentNotification.StudentId = studentId;
                            studentNotification.CreationDate = DateTime.UtcNow;
                            studentNotification.ReferenceId = 4;
                            studentNotification.NotificationToId = 0;
                            await BusinessService.CreateAsync(studentNotification);

                            var Tokens = db.StudentPushTokens.Where(item => item.StudentId == studentId).ToList();
                            foreach (var token in Tokens)
                            {

                                if (token.OS == "ios")
                                {
                                    PushManager.PushToStudentToIphoneDevice(token.Token, studentNotification.Title, studentNotification.NotificationToId, studentNotification.ReferenceId);
                                }
                                else if (token.OS == "android")
                                {
                                    PushManager.pushToAndroidDevice(token.Token, studentNotification.Title, studentNotification.NotificationToId, studentNotification.ReferenceId);
                                }

                            }
                        }
                        return RedirectToAction("Index");

                    }
                }
                return View(studentNotification);
            }
            catch (Exception ex)
            {
             
                throw ex;
            }

        }

    }
}
