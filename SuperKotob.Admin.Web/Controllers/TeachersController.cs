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
using Microsoft.AspNet.Identity.Owin;
using SuperKotob.Admin.Web;
using Modeer.Admin.Web.Utils;
using SuperKotob.Admin.Web.Models;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Tollab.Admin.Web.Utils;

namespace Tollab.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin")]

    public class TeachersController : BaseWebController<Teacher,Teacher>
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


        public TeachersController(IBusinessService<Teacher, Teacher> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateWithImage(Teacher teacher, HttpPostedFileBase ImageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (teacher.DateOfBirth>=DateTime.UtcNow)
                    {
                        ViewBag.Message = "This Date Of Birth Not Valid";
                        return View("Create", teacher);

                    }
                     Teacher Resultdata = null;
                    Teacher GetTeacherData = null;
                     ApplicationUser userAfterCreated = null;
                    ApplicationUser userBeforeCreated = null;
                     userBeforeCreated = await UserManager.FindByEmailAsync(teacher.Email);
                    if (userBeforeCreated != null)
                    {
                        GetTeacherData = db.Teachers.Where(it => it.IdentityId == userBeforeCreated.Id).Select(it => it).FirstOrDefault();
                    }
                    if (userBeforeCreated == null)
                    {
                        var Identityuser = new ApplicationUser { UserName = teacher.Email, Email = teacher.Email, PhoneNumber = teacher.Phone };
                        var result = await UserManager.CreateAsync(Identityuser, teacher.Password);
                        if (result.Succeeded)
                        {
                            await UserManager.AddToRoleAsync(Identityuser.Id, "Teacher");


                            userAfterCreated = await UserManager.FindByEmailAsync(teacher.Email);
                            teacher.RegisterationDate = DateTime.UtcNow;
                            teacher.IdentityId = userAfterCreated.Id;
                            var DataReturned = await BusinessService.CreateAsync(teacher);
                            if (ImageFile != null && DataReturned.HasData == true)
                            {
                                await SetPhoto(teacher, ImageFile);
                            }
                            return RedirectToAction("Index");
                        }
                    }
                    if (userBeforeCreated != null && GetTeacherData == null)
                    {
                        teacher.RegisterationDate = DateTime.UtcNow;
                        teacher.IdentityId = userAfterCreated.Id;
                        var DataReturned = await BusinessService.CreateAsync(teacher);
                        if (ImageFile != null && DataReturned.HasData == true)
                        {
                            await SetPhoto(teacher, ImageFile);
                        }
                        return RedirectToAction("Index");
                    }

                    ViewBag.Message = "This Email Registerd Before";
                    return View("Create", teacher);

                }
                return View("Create", teacher);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditWithImage(Teacher teacher, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                var oldUser = await UserManager.FindByIdAsync(teacher.IdentityId);

                if (teacher.DateOfBirth >= DateTime.UtcNow)
                {
                    ViewBag.Message = "This Date Of Birth Not Valid";
                    return View("Edit", teacher);

                }
                var OldData = await BusinessService.GetAsync(teacher.Id);

                if (OldData.Data.FirstOrDefault().Email == teacher.Email)
                {
                    var DataReturned = await BusinessService.UpdateAsync(teacher);
                    if (ImageFile != null && DataReturned.HasData == true)
                    {
                        await SetPhoto(teacher, ImageFile);
                    }
                    if (!string.IsNullOrWhiteSpace(teacher.Password))
                    {
                        string code = await UserManager.GeneratePasswordResetTokenAsync(oldUser.Id);
                        var savingPassword = await UserManager.ResetPasswordAsync(oldUser.Id, code, teacher.Password);
                    }

                    return RedirectToIndex(teacher);
                }
                else
                {
                    Teacher Resultdata = null;
                    Teacher GetTeacherData = null;
                    ApplicationUser userAfterCreated = null;
                    ApplicationUser userBeforeUpdated = null;
                    userBeforeUpdated = await UserManager.FindByEmailAsync(teacher.Email);
                    if (userBeforeUpdated != null)
                    {
                        ViewBag.Message = "This Email Registerd Before";
                        return View("Edit", teacher);
                    }
                    if (userBeforeUpdated == null)
                    {
                        oldUser.Email = teacher.Email;
                        oldUser.PhoneNumber = teacher.Phone;
                        oldUser.UserName = teacher.Email;
                        var result = await UserManager.UpdateAsync(oldUser);
                        if (result.Succeeded)
                        {
                             var DataReturned = await BusinessService.UpdateAsync(teacher);
                            if (ImageFile != null && DataReturned.HasData == true)
                            {
                                await SetPhoto(teacher, ImageFile);
                            }
                            if (!string.IsNullOrWhiteSpace(teacher.Password))
                            {
                                string code = await UserManager.GeneratePasswordResetTokenAsync(oldUser.Id);
                                var savingPassword = await UserManager.ResetPasswordAsync(oldUser.Id, code, teacher.Password);
                            }
                            return RedirectToAction("Index");
                        }
                    }
                }




            }
            return View("Edit", teacher);
        }

        public async Task SetPhoto(Teacher teacher, HttpPostedFileBase ImageFile)
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
                var imageObject = new { RecordId = teacher.Id, Table = "Teacher", CoulmnName = "Photo", ImageType = (int)ImageFolders.TeacherImages, Image = thePictureDataAsString };
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


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> DeleteConfirmed(long? id)
        {
            long Id = id.Value;
            var Studentdata = await BusinessService.GetAsync(Id);
            string UserId = Studentdata.Data.FirstOrDefault().IdentityId;


            var response = await BusinessService.DeleteAsync(id.Value);
            if (response.HasErrors)
            {
                string viewName = GetViewName("_DeleteFailed");
                return View(viewName, response.Errors);
            }
            if (UserId != null)
            {
                var userData = await UserManager.FindByIdAsync(UserId);
                if (userData != null)
                {
                    var Resalt = await UserManager.DeleteAsync(userData);
                }
            }

            return RedirectToAction("Index");
        }



      

    }
}
