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
    [Authorize]
    public class SystemNotificationsController : BaseWebController<SystemNotification, SystemNotification>
    {
        private TollabContext db = new TollabContext();
        public SystemNotificationsController(IBusinessService<SystemNotification, SystemNotification> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }

        const long NotificationTypeTeachers = 1;
        const long NotificationTypeStudents = 2;
        const long NotificationTypeSubject = 3;
        const long NotificationTypeCountry = 4;
        const long NotificationTypeUniversity = 5;


        /* [HttpPost]
         [ValidateAntiForgeryToken]
         public override async Task<ActionResult> Create(SystemNotification systemNotification)
         {
             try
             {
                 if (ModelState.IsValid)
                 {

                     List<StudentPushToken> StudentPushTokensAndroid = null;
                     List<StudentPushToken> StudentPushTokensIOS = null;
                     List<TeacherPushToken> TeacherPushTokensAndroid = null;
                     List<TeacherPushToken> TeacherPushTokensIOS = null;




                     if (systemNotification.NotificationTypeId == NotificationTypeTeachers)
                     {

                         TeacherPushTokensAndroid = db.TeacherPushTokens.Where(i => i.OS == "android").ToList();
                         TeacherPushTokensIOS = db.TeacherPushTokens.Where(i => i.OS == "ios").ToList();

                     }
                     else if (systemNotification.NotificationTypeId == NotificationTypeStudents)
                     {
                         StudentPushTokensAndroid = db.StudentPushTokens.Where(i => i.OS == "android").OrderBy(i=>i.StudentId).ToList();
                         StudentPushTokensIOS = db.StudentPushTokens.Where(i => i.OS == "ios").OrderBy(i => i.StudentId).ToList();
                     }
                     else if (systemNotification.NotificationTypeId == NotificationTypeSubject && systemNotification.SubjectId > 0)
                     {
                         var Tokens = await db.Database.SqlQuery<StudentPushToken>(@"select Distinct StudentPushToken.* from 
                            StudentSubject join StudentPushToken on StudentSubject.StudentId=StudentPushToken.StudentId
                            where StudentSubject.Id=" + systemNotification.SubjectId + " and StudentPushToken.os='android'").ToListAsync();
                         StudentPushTokensAndroid = Tokens;

                         var TokensIOS = await db.Database.SqlQuery<StudentPushToken>(@"select Distinct StudentPushToken.* from 
                            StudentSubject join StudentPushToken on StudentSubject.StudentId=StudentPushToken.StudentId
                            where StudentSubject.Id=" + systemNotification.SubjectId + " and StudentPushToken.os='ios'").ToListAsync();
                         StudentPushTokensIOS = TokensIOS;

                     }
                     else if (systemNotification.NotificationTypeId == NotificationTypeCountry && systemNotification.CountryId > 0)
                     {
                         var Tokens = await db.Database.SqlQuery<StudentPushToken>(@"select Distinct StudentPushToken.* from
                                 Student join StudentPushToken on Student.Id=StudentPushToken.StudentId
                                 where Student.CountryId=" + systemNotification.CountryId + " and StudentPushToken.os='android'").ToListAsync();
                         StudentPushTokensAndroid = Tokens;

                         var TokensIOS = await db.Database.SqlQuery<StudentPushToken>(@"select Distinct StudentPushToken.* from
                                 Student join StudentPushToken on Student.Id=StudentPushToken.StudentId
                                 where Student.CountryId=" + systemNotification.CountryId + " and StudentPushToken.os='ios'").ToListAsync();
                         StudentPushTokensIOS = TokensIOS;

                     }
                     else if (systemNotification.NotificationTypeId == NotificationTypeUniversity && systemNotification.SubCategoryId > 0)
                     {
                         var Tokens = await db.Database.SqlQuery<StudentPushToken>(@"select Distinct StudentPushToken.* from StudentDepartment
  join StudentPushToken on StudentDepartment.StudentId=StudentPushToken.StudentId
  join Department on Department.Id=StudentDepartment.DepartmentId
  join SubCategory on SubCategory.Id=Department.SubCategoryId
 where SubCategory.Id=" + systemNotification.SubCategoryId + " and StudentPushToken.os='android'").ToListAsync();
                         StudentPushTokensAndroid = Tokens;

                         var TokensIOS = await db.Database.SqlQuery<StudentPushToken>(@"select Distinct StudentPushToken.* from StudentDepartment
  join StudentPushToken on StudentDepartment.StudentId=StudentPushToken.StudentId
  join Department on Department.Id=StudentDepartment.DepartmentId
  join SubCategory on SubCategory.Id=Department.SubCategoryId
 where SubCategory.Id=" + systemNotification.SubCategoryId + " and StudentPushToken.os='ios'").ToListAsync();
                         StudentPushTokensIOS = TokensIOS;
                     }


                     systemNotification.CreationDate = DateTime.UtcNow;
                     await BusinessService.CreateAsync(systemNotification);

                     if (StudentPushTokensAndroid != null || StudentPushTokensIOS != null)
                     {
                         List<string> STokens = new List<string>();
                         foreach (var item in StudentPushTokensAndroid)
                         {
                             STokens.Add(item.Token);
                         }

                         Task.Run(async () => 
                         {
                             foreach (var token in StudentPushTokensIOS)
                             {

                                 PushManager.PushToStudentToIphoneDevice(token.Token, systemNotification.Message, 0, 4);
                             }
                             foreach (var item in STokens)
                             {
                                 PushManager.pushToAndroidDevice(item, systemNotification.Message, 0, 4);

                             }

                         });



                     }
                     if (TeacherPushTokensAndroid != null || TeacherPushTokensIOS != null)
                     {

                         List<string> TTokens = new List<string>();
                         foreach (var item in TeacherPushTokensAndroid)
                         {
                             TTokens.Add(item.Token);
                         }
                         Task.Run(async () => {
                             PushManager.pushBulkToAndroidDevice(TTokens, systemNotification.Message, 0, 4);
                             foreach (var token in TeacherPushTokensIOS)
                             {

                                 PushManager.PushToTeacherToIphoneDevice(token.Token, systemNotification.Message, 0, 4);
                             }
                         });

                     }
                     return RedirectToAction("Index");
                 }

                 return View(systemNotification);
             }
             catch (Exception ex)
             {
                 throw ex;
             }

         }
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Create(SystemNotification systemNotification)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    List<StudentPushToken> StudentPushTokensAndroid = null;
                    List<StudentPushToken> StudentPushTokensIOS = null;
                   
                    if (systemNotification.CourseId>0)
                    {
                        var Tokens = await db.Database.SqlQuery<StudentPushToken>(@"select Distinct StudentPushToken.* from 
                           StudentCourse join StudentPushToken on StudentCourse.StudentId=StudentPushToken.StudentId
                           where StudentCourse.CourseId=" + systemNotification.CourseId + " and StudentPushToken.os='android'").ToListAsync();
                        StudentPushTokensAndroid = Tokens;

                        var TokensIOS = await db.Database.SqlQuery<StudentPushToken>(@"select Distinct StudentPushToken.* from
                           StudentCourse join StudentPushToken on StudentCourse.StudentId = StudentPushToken.StudentId
                           where StudentCourse.CourseId = " + systemNotification.CourseId + " and StudentPushToken.os = 'ios'").ToListAsync();
                        StudentPushTokensIOS = TokensIOS;


                    }


                   else if (systemNotification.TrackId > 0)
                    {
                        var Tokens = await db.Database.SqlQuery<StudentPushToken>(@"select Distinct StudentPushToken.* from 
                           TrackSubscription join StudentPushToken on TrackSubscription.StudentId=StudentPushToken.StudentId
                           where TrackSubscription.TrackId=" + systemNotification.TrackId + " and StudentPushToken.os='android'").ToListAsync();
                        StudentPushTokensAndroid = Tokens;

                        var TokensIOS = await db.Database.SqlQuery<StudentPushToken>(@"select Distinct StudentPushToken.* from
                           TrackSubscription join StudentPushToken on TrackSubscription.StudentId = StudentPushToken.StudentId
                           where TrackSubscription.TrackId = " + systemNotification.TrackId + " and StudentPushToken.os = 'ios'").ToListAsync();
                        StudentPushTokensIOS = TokensIOS;


                    }
                    else if (systemNotification.CountryId>0)
                    {

                        var Tokens = await db.Database.SqlQuery<StudentPushToken>(@"select Distinct StudentPushToken.* from
                                 Student join StudentPushToken on Student.Id=StudentPushToken.StudentId
                                 where Student.CountryId=" + systemNotification.CountryId + " and StudentPushToken.os='android' Order by StudentPushToken.StudentId").ToListAsync();
                        StudentPushTokensAndroid = Tokens;

                        var TokensIOS = await db.Database.SqlQuery<StudentPushToken>(@"select Distinct StudentPushToken.* from
                                 Student join StudentPushToken on Student.Id=StudentPushToken.StudentId
                                 where Student.CountryId=" + systemNotification.CountryId + " and StudentPushToken.os='ios' Order by StudentPushToken.StudentId").ToListAsync();
                        StudentPushTokensIOS = TokensIOS;
                    }
                    systemNotification.CreationDate = DateTime.UtcNow;
                    systemNotification.NotificationTypeId = 2;
                    var d=  await BusinessService.CreateAsync(systemNotification);

                    if (StudentPushTokensAndroid != null || StudentPushTokensIOS != null)
                    {
                        List<string> STokens = new List<string>();
                        List<long> StudentIds = new List<long>();
                        foreach (var item in StudentPushTokensAndroid)
                        {
                            if (!StudentIds.Contains(item.StudentId))
                            {
                                StudentIds.Add(item.StudentId);
                            }
                            STokens.Add(item.Token);
                        }
                        foreach (var item2 in StudentPushTokensIOS)
                        {
                            if (!StudentIds.Contains(item2.StudentId))
                            {
                                StudentIds.Add(item2.StudentId);
                            }
                        }

                       var taskbackground= Task.Run(async () =>
                        {
                            int ioscount = 0;
                            int androidcount = 0;
                            foreach (var token in StudentPushTokensIOS)
                            {

                                PushManager.PushToStudentToIphoneDevice(token.Token, systemNotification.Message, 0, 4);
                                ioscount = ioscount + 1;
                            }
                            foreach (var item in STokens)
                            {
                                PushManager.pushToAndroidDevice(item, systemNotification.Message, 0, 4);
                                androidcount = androidcount + 1;
                            }
                            foreach (var StudentId in StudentIds)
                            {
                                StudentNotification studentNotification = new StudentNotification()
                                {
                                    StudentId = StudentId,
                                    Title = systemNotification.Message,
                                    TitleLT = systemNotification.Message,
                                    ReferenceId = 4,
                                    CreationDate = DateTime.UtcNow
                                };
                                db.StudentNotifications.Add(studentNotification);
                            }
                           await db.SaveChangesAsync();
                        });

                    }

                    return RedirectToAction("Index");
                }

                return View(systemNotification);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
}
