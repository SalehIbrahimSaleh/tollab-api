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

    public class GroupsController : BaseWebController<Group,Group>
    {
        private TollabContext db = new TollabContext();
        public GroupsController(IBusinessService<Group, Group> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Create(Group item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var CourseTrack = db.Courses.Where(it => it.Id == item.CourseId).Select(it => it.CourseTrack).FirstOrDefault();

                    item.GroupCourse = item.Name + "-" + CourseTrack;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Edit(Group item)
        {
            if (ModelState.IsValid)
            {
                var CourseTrack = db.Courses.Where(it => it.Id == item.CourseId).Select(it => it.CourseTrack).FirstOrDefault();

                item.GroupCourse = item.Name + "-" + CourseTrack;

                await BusinessService.UpdateAsync(item);
                return RedirectToIndex(item);
            }

            return View(item);
        }


    }
}
