
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tollab.Admin.Data.Models;
using SuperKotob.Admin.Data;
using SuperKotob.Admin.Web.Controllers;
using SuperKotob.Admin.Core;
using SuperKotob.Admin.Utils.Configuration;
using Modeer.Admin.Web.Utils;
using SuperKotob.Admin.Web.Models;
using SuperKotob.Admin.Web;
using Microsoft.AspNet.Identity.Owin;

namespace OrthoHouse.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SystemAdminsController : BaseWebController<SystemAdmin,SystemAdmin>
    {
        public SystemAdminsController(IBusinessService<SystemAdmin, SystemAdmin> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Create(SystemAdmin item)
        {
            //  item.PhoneNumber = MobileNumberChecker.handelMobileNumber(item.PhoneNumber);
            ApplicationUser UserDate=null;
            try
            {
               
                var IsEmailRegistered = MailChecker.CheckEmailAddressExist(item.Email);
                if (IsEmailRegistered)
                {
                    ViewBag.Error = "This Email Relate With Another Acoount";
                    return View(item);
                }
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser
                    {
                        UserName = item.Email,
                        Email = item.Email,
                      //  PhoneNumber = item.PhoneNumber,
                    };
                    var result = await UserManager.CreateAsync(user, item.Password);
                    if (result.Succeeded)
                    {
                        UserDate = await UserManager.FindByEmailAsync(item.Email);
                        if (item.Type==1)
                        {
                            UserManager.AddToRoleAsync(UserDate.Id, "Admin");

                        }
                       
                        item.IdentityId = UserDate.Id;
                        await BusinessService.CreateAsync(item);
                        return RedirectToAction("Index");
                    }
                }

                ViewBag.Error = "Try Again Later";

                return View(item);
            }
            catch (Exception ex)
            {
                UserManager.DeleteAsync(UserDate);
                throw ex;
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Edit(SystemAdmin item)
        {
            try
            {
                ApplicationUser UserDate = null;

                //       item.PhoneNumber = MobileNumberChecker.handelMobileNumber(item.PhoneNumber);
                var OldData = await BusinessService.GetAsync(item.Id);
                //if (OldData.Data.FirstOrDefault().PhoneNumber != item.PhoneNumber)
                //{
                //    var IsRegistered = checkMobileForRegister.checkIfRegistered(item.PhoneNumber);
                //    if (IsRegistered)
                //    {
                //        ViewBag.Error = "This Phone Relate With Another Acoount";
                //        return View(item);
                //    }
                //}
                if (OldData.Data.FirstOrDefault().Email != item.Email)
                {
                    var IsEmailRegistered = MailChecker.CheckEmailAddressExist(item.Email);
                    if (IsEmailRegistered)
                    {
                        ViewBag.Error = "This Email Relate With Another Acoount";
                        return View(item);
                    }
                }

                if (ModelState.IsValid)
                {
                    UserDate = await UserManager.FindByEmailAsync(item.Email);
                    UserDate.Email = item.Email;
                    //userData.PhoneNumber = item.PhoneNumber;
                    UserDate.UserName = item.Email;
                    var result = await UserManager.UpdateAsync(UserDate);
                    if (item.Type != OldData.Data.FirstOrDefault().Type)
                    {
                        if (item.Type == 1)
                        {
                            UserManager.AddToRoleAsync(UserDate.Id, "Admin");
                        }
                       
                    }
                    if (!string.IsNullOrWhiteSpace(item.Password))
                    {
                        string code = await UserManager.GeneratePasswordResetTokenAsync(UserDate.Id);
                        var savingPassword = await UserManager.ResetPasswordAsync(UserDate.Id, code, item.Password);
                    }
                    await BusinessService.UpdateAsync(item);
                    return RedirectToIndex(item);
                }

                return View(item);
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
