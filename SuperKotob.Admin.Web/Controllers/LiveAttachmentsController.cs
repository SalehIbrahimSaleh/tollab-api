using SuperKotob.Admin.Core;
using SuperKotob.Admin.Utils.Configuration;
using SuperKotob.Admin.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Tollab.Admin.Data.Models;
using Tollab.Admin.Web.Services.FileUpload;

namespace Tollab.Admin.Web.Controllers
{
    public class LiveAttachmentsController : BaseWebController<LiveAttachment, LiveAttachment>
    {
        private readonly IFileUploadService _fileUploadService = new FileUploadService();
        public LiveAttachmentsController(IBusinessService<LiveAttachment, LiveAttachment> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(LiveAttachment item , HttpPostedFileBase uploadedFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (uploadedFile != null)
                    {
                        var fileName = await _fileUploadService.UploadFile(uploadedFile);
                        item.Path = fileName;
                        await BusinessService.CreateAsync(item);
                        return RedirectToAction("Index");
                    }
                }
                return View(item);
            }
            catch(Exception ex)
            {
                return View("Error");
            }
           
        }

        [HttpPost]
        public async Task<ActionResult> EditAsync(LiveAttachment item, HttpPostedFileBase uploadedFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (uploadedFile != null)
                    {
                        var fileName = await _fileUploadService.UploadFile(uploadedFile);
                        item.Path = fileName;
                    }
                    await BusinessService.UpdateAsync(item);
                    return RedirectToAction("Index");
                }
                return View(item);
            }
            catch (Exception ex)
            {
                return View("Error");
            }

        }
    }
}