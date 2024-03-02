using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json.Linq;
using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data;
using SuperKotob.Admin.Utils.Configuration;
using SuperKotob.Admin.Web;
using SuperKotob.Admin.Web.Controllers;
using SuperKotob.Admin.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Tollab.Admin.Data.Models;
using Tollab.Admin.Web.Utils;

namespace Tollab.Admin.Web.Controllers
{
    [Authorize]
    public class TeacherAssistantsController : BaseWebController<TeacherAssistant, TeacherAssistant>
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

        public TeacherAssistantsController(IBusinessService<TeacherAssistant, TeacherAssistant> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateWithImage(TeacherAssistant teacherAssistant, HttpPostedFileBase ImageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   
                    TeacherAssistant Resultdata = null;
                    TeacherAssistant GetTeacherAssistantData = null;
                    ApplicationUser userAfterCreated = null;
                    ApplicationUser userBeforeCreated = null;
                    userBeforeCreated = await UserManager.FindByEmailAsync(teacherAssistant.Email);
                    if (userBeforeCreated != null)
                    {
                        GetTeacherAssistantData = db.TeacherAssistants.Where(it => it.IdentityId == userBeforeCreated.Id).Select(it => it).FirstOrDefault();
                    }
                    if (userBeforeCreated == null)
                    {
                        var Identityuser = new ApplicationUser { UserName = teacherAssistant.Email, Email = teacherAssistant.Email, PhoneNumber = teacherAssistant.Phone };
                        var result = await UserManager.CreateAsync(Identityuser, teacherAssistant.Password);
                        if (result.Succeeded)
                        {
                            await UserManager.AddToRoleAsync(Identityuser.Id, "TeacherAssistant");

                            userAfterCreated = await UserManager.FindByEmailAsync(teacherAssistant.Email);
                            teacherAssistant.RegisterationDate = DateTime.UtcNow;
                            teacherAssistant.IdentityId = userAfterCreated.Id;
                            var DataReturned = await BusinessService.CreateAsync(teacherAssistant);
                            if (ImageFile != null && DataReturned.HasData == true)
                            {
                                await SetPhoto(teacherAssistant, ImageFile);
                            }
                            return RedirectToAction("Index");
                        }
                    }
                    if (userBeforeCreated != null && GetTeacherAssistantData == null)
                    {
                        teacherAssistant.RegisterationDate = DateTime.UtcNow;
                        teacherAssistant.IdentityId = userAfterCreated.Id;
                        var DataReturned = await BusinessService.CreateAsync(teacherAssistant);
                        if (ImageFile != null && DataReturned.HasData == true)
                        {
                            await SetPhoto(teacherAssistant, ImageFile);
                        }
                        return RedirectToAction("Index");
                    }

                    ViewBag.Message = "This Email Registerd Before";
                    return View("Create", teacherAssistant);

                }
                return View("Create", teacherAssistant);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditWithImage(TeacherAssistant teacherAssistant, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                var oldUser = await UserManager.FindByIdAsync(teacherAssistant.IdentityId);

                 
                var OldData = await BusinessService.GetAsync(teacherAssistant.Id);

                if (OldData.Data.FirstOrDefault().Email == teacherAssistant.Email)
                {
                    var DataReturned = await BusinessService.UpdateAsync(teacherAssistant);
                    if (ImageFile != null && DataReturned.HasData == true)
                    {
                        await SetPhoto(teacherAssistant, ImageFile);
                    }
                    if (!string.IsNullOrWhiteSpace(teacherAssistant.Password))
                    {
                        string code = await UserManager.GeneratePasswordResetTokenAsync(oldUser.Id);
                        var savingPassword = await UserManager.ResetPasswordAsync(oldUser.Id, code, teacherAssistant.Password);
                    }

                    return RedirectToIndex(teacherAssistant);
                }
                else
                {
                    Teacher Resultdata = null;
                    Teacher GetTeacherData = null;
                    ApplicationUser userAfterCreated = null;
                    ApplicationUser userBeforeUpdated = null;
                    userBeforeUpdated = await UserManager.FindByEmailAsync(teacherAssistant.Email);
                    if (userBeforeUpdated != null)
                    {
                        ViewBag.Message = "This Email Registerd Before";
                        return View("Edit", teacherAssistant);
                    }
                    if (userBeforeUpdated == null)
                    {
                        oldUser.Email = teacherAssistant.Email;
                        oldUser.PhoneNumber = teacherAssistant.Phone;
                        oldUser.UserName = teacherAssistant.Email;
                        var result = await UserManager.UpdateAsync(oldUser);
                        if (result.Succeeded)
                        {
                            var DataReturned = await BusinessService.UpdateAsync(teacherAssistant);
                            if (ImageFile != null && DataReturned.HasData == true)
                            {
                                await SetPhoto(teacherAssistant, ImageFile);
                            }
                            if (!string.IsNullOrWhiteSpace(teacherAssistant.Password))
                            {
                                string code = await UserManager.GeneratePasswordResetTokenAsync(oldUser.Id);
                                var savingPassword = await UserManager.ResetPasswordAsync(oldUser.Id, code, teacherAssistant.Password);
                            }
                            return RedirectToAction("Index");
                        }
                    }
                }




            }
            return View("Edit", teacherAssistant);
        }

        public async Task SetPhoto(TeacherAssistant teacherAssistant, HttpPostedFileBase ImageFile)
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
                var imageObject = new { RecordId = teacherAssistant.Id, Table = "TeacherAssistant", CoulmnName = "Photo", ImageType = (int)ImageFolders.TeacherAssistantImages, Image = thePictureDataAsString };
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


    }
}
