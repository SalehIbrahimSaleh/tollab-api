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
using Modeer.Admin.Web.Utils;
using SuperKotob.Admin.Web.Models;
using SuperKotob.Admin.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Tollab.Admin.Web.Utils;

namespace Tollab.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin")]

    public class StudentsController : BaseWebController<Student, Student>
    {
        private TollabContext db = new TollabContext();
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public StudentsController(IBusinessService<Student, Student> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateWithImage(Student student, HttpPostedFileBase ImageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var country= db.Countries.Where(it => it.Id == student.CountryId).FirstOrDefault();
                    Student Resultdata = null;
                    Student GetStudentData = null;
                    string PhoneNumber = MobileNumberChecker.handelMobileNumber(country.Code + student.Phone);
                    ApplicationUser userAfterCreated = null;
                    ApplicationUser userBeforeCreated = null;
                    string email = PhoneNumber + "@Tollabapp.com";
                    userBeforeCreated = await UserManager.FindByEmailAsync(email);
                    if (userBeforeCreated != null)
                    {
                        GetStudentData = db.Students.Where(it => it.IdentityId == userBeforeCreated.Id).Select(it => it).FirstOrDefault();
                    }
                    if (userBeforeCreated == null)
                    {
                        var Identityuser = new ApplicationUser{ UserName = PhoneNumber, Email = email, PhoneNumber = PhoneNumber };

                        //IdentityResult result = await UserManager.CreateAsync(Identityuser, PhoneNumber);
                        var result = await UserManager.CreateAsync(Identityuser, PhoneNumber);
                        if (result.Succeeded)
                        {
                            userAfterCreated = await UserManager.FindByEmailAsync(email);
                            student.CreationDate = DateTime.UtcNow;
                            student.IdentityId = userAfterCreated.Id;
                            student.PhoneKey = country.Code;
                            var DataReturned = await BusinessService.CreateAsync(student);
                            if (ImageFile != null && DataReturned.HasData == true)
                            {
                                await SetPhoto(student, ImageFile);
                            }
                            return RedirectToAction("Index");
                        }
                    }
                    if (userBeforeCreated != null && GetStudentData == null)
                    {
                        student.IdentityId = userBeforeCreated.Id;
                        student.CreationDate = DateTime.UtcNow;
                        var DataReturned = await BusinessService.CreateAsync(student);
                        if (ImageFile != null && DataReturned.HasData == true)
                        {
                            await SetPhoto(student, ImageFile);
                        }
                        return RedirectToAction("Index");
                    }

                    ViewBag.Message = "This Phone Registerd Before";
                    return View("Create", student);

                }
                return View("Create", student);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task SetPhoto(Student student,HttpPostedFileBase ImageFile)
        {
           
                try
                {
                    byte[] thePictureAsBytes = new byte[ImageFile.ContentLength];
                    using (BinaryReader theReader = new BinaryReader(ImageFile.InputStream))
                    {
                        thePictureAsBytes = theReader.ReadBytes(ImageFile.ContentLength);
                    }
                    string thePictureDataAsString = Convert.ToBase64String(thePictureAsBytes);
                    string uri = "https://tollab.azurewebsites.net/sws/api/SetPhoto";
                    var client = new HttpClient();
                    var imageObject = new { RecordId = student.Id, Table = "Student", CoulmnName = "Photo", ImageType = (int)ImageFolders.StudentImages, Image = thePictureDataAsString };
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditWithImage(Student  student, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                var OldData = await BusinessService.GetAsync(student.Id);

                if (OldData.Data.FirstOrDefault().CountryId==student.CountryId&& OldData.Data.FirstOrDefault().Phone==student.Phone)
                {
                    var DataReturned = await BusinessService.UpdateAsync(student);
                    if (ImageFile != null && DataReturned.HasData == true)
                    {
                        await SetPhoto(student, ImageFile);
                    }
                    return RedirectToIndex(student);
                }
                else
                {
                    var country = db.Countries.Where(it => it.Id == student.CountryId).FirstOrDefault();
                    Student Resultdata = null;
                    Student GetStudentData = null;
                    string PhoneNumber = MobileNumberChecker.handelMobileNumber(country.Code + student.Phone);
                    ApplicationUser userAfterCreated = null;
                    ApplicationUser userBeforeUpdated = null;
                    string email = PhoneNumber + "@Tollabapp.com";
                    userBeforeUpdated = await UserManager.FindByEmailAsync(email);
                    if (userBeforeUpdated != null)
                    {
                        ViewBag.Message = "This Phone Registerd Before";
                        return View("Edit", student);

                    }
                    if (userBeforeUpdated == null)
                    {
                       var oldUser=  await UserManager.FindByIdAsync(student.IdentityId);
                        oldUser.Email = email;
                        oldUser.PhoneNumber = PhoneNumber;
                        oldUser.UserName = PhoneNumber; 
                        var result = await UserManager.UpdateAsync(oldUser);
                        if (result.Succeeded)
                        {
                            student.PhoneKey = country.Code;
                            var DataReturned = await BusinessService.UpdateAsync(student);
                            if (ImageFile != null && DataReturned.HasData == true)
                            {
                                await SetPhoto(student, ImageFile);
                            }
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
            return View("Edit", student);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> DeleteConfirmed(long? id)
        {
            long Id = id.Value;
            var Studentdata = await BusinessService.GetAsync(Id);
            var IdentityId = Studentdata.Data.FirstOrDefault().IdentityId;

            var response = await BusinessService.DeleteAsync(id.Value);
            if (response.HasErrors)
            {
                string viewName = GetViewName("_DeleteFailed");
                return View(viewName, response.Errors);
            }
            if (IdentityId != null)
            {
                var userData = await UserManager.FindByIdAsync(IdentityId);
                if (userData != null)
                {
                    var Resalt = await UserManager.DeleteAsync(userData);
                }
            }


            return RedirectToAction("Index");
        }


        public override async Task<ActionResult> Details(long? id)
        {
            if (id == null || id.Value < 1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var response = await BusinessService.GetAsync(id.Value);
            if (!response.HasData)
            {
                return HttpNotFound();
            }
            var model = response.Data.FirstOrDefault();
            if (model!=null)
            {
                var Departments = db.StudentDepartments.Include(d => d.Student).Include(d => d.Department).Where(d => d.StudentId == model.Id).ToList();
                model.StudentDepartments = Departments;
                var Subjects = db.StudentSubjects.Include(d => d.Student).Where(d => d.StudentId == model.Id).ToList();
                model.StudentSubjects = Subjects;
                var Courses = db.StudentCourses.Include(d => d.Student).Include(c=>c.Course).Where(d => d.StudentId == model.Id).ToList();
                model.StudentCourses = Courses;
                var Lives = db.StudentLives.Include(d => d.Student).Include(c => c.Live).Where(d => d.StudentId == model.Id).ToList();
                model.StudentLives = Lives;
                var Tracks = db.TrackSubscriptions.Include(d => d.Student).Include(c => c.Track).Where(d => d.StudentId == model.Id).ToList();
                model.StudentTracks = Tracks;
                var Favourites = db.Favourites.Include(d => d.Student).Include(c=>c.Course).Where(d => d.StudentId == model.Id).ToList();
                model.Favourites = Favourites;
                var StudentNotifications = db.StudentNotifications.Include(d => d.Student).Where(d => d.StudentId == model.Id).ToList();
                model.StudentNotifications = StudentNotifications;
                var StudentPromoCodes = db.StudentPromoCodes.Include(d => d.Student).Include(i=>i.PromoCode).Where(d => d.StudentId == model.Id).ToList();
                model.StudentPromoCodes = StudentPromoCodes;
                var studentTransactions = db.StudentTransactions.Include(d => d.Student).Where(d => d.StudentId == model.Id).ToList();
                model.StudentTransactions = studentTransactions;
                var studentExams = db.StudentExams.Include(d => d.Student).Include(d => d.Exam).Where(d => d.StudentId == model.Id).ToList();
                model.StudentExams = studentExams;
            }


            string viewName = GetViewName("Details");

            return View(viewName, model);
        }

    }


}
