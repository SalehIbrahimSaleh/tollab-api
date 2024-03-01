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

namespace Tollab.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin")]

    public class TeacherTransactionsController : BaseWebController<TeacherTransaction, TeacherTransaction>
    {
        private TollabContext db = new TollabContext();

        public TeacherTransactionsController(IBusinessService<TeacherTransaction, TeacherTransaction> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Create(TeacherTransaction item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    item.CreationDate = DateTime.UtcNow;
                    await BusinessService.CreateAsync(item);
                    return RedirectToAction("Index");
                }

                return View(item);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

    }
}
