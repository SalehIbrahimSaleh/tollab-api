using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data;
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
    public class ExamsController : BaseWebController<Exam,Exam>
    {
        private TollabContext db = new TollabContext();

        public ExamsController(IBusinessService<Exam, Exam> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Edit(Exam item)
        {
            if (ModelState.IsValid)
            {
                await BusinessService.UpdateAsync(item);
                return RedirectToIndex(item);
            }
            return View("Edit", item);
        }


    }
}