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

    public class DepartmentsController : BaseWebController<Department,Department>
    {
        private TollabContext db = new TollabContext();

        public DepartmentsController(IBusinessService<Department, Department> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Create(Department item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var SubCategoryCategory = db.SubCategories.Where(it => it.Id == item.SubCategoryId).Select(it => it.SubCategoryCategory).FirstOrDefault();

                    item.DepartmentSubCategory = item.Name + "-" + SubCategoryCategory;
                    await BusinessService.CreateAsync(item);
                    return RedirectToAction("Index");
                }

                return View("Create", item);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Edit(Department item)
        {
            if (ModelState.IsValid)
            {
                var SubCategoryCategory = db.SubCategories.Where(it => it.Id == item.SubCategoryId).Select(it => it.SubCategoryCategory).FirstOrDefault();

                item.DepartmentSubCategory = item.Name + "-" + SubCategoryCategory;
                await BusinessService.UpdateAsync(item);
                return RedirectToIndex(item);
            }
            return View("Edit", item);
        }


    }
}
