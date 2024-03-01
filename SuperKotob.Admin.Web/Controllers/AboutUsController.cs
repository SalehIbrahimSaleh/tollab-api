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

namespace Tollab.Admin.Web.Controllers
{
    [Authorize]

    public class AboutUsController : BaseWebController<AboutUs, AboutUs>
    {
        public AboutUsController(IBusinessService<AboutUs, AboutUs> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Edit(AboutUs item)
        {
            if (ModelState.IsValid)
            {
                await BusinessService.UpdateAsync(item);
                return RedirectToAction("Index","Home");
            }
           
            return View(/*viewName, order*/item);
        }

    }
}