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

    public class SubCategoriesController : BaseWebController<SubCategory, SubCategory>
    {
        private TollabContext db = new TollabContext();

        public SubCategoriesController(IBusinessService<SubCategory, SubCategory> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Create(SubCategory item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var CategorySection = db.Category.Where(it => it.Id == item.CategoryId).Select(it => it.CategorySection).FirstOrDefault();

                    item.SubCategoryCategory = item.Name + "-" + CategorySection;

                    await BusinessService.CreateAsync(item);
                    return RedirectToAction("Index");
                }

                return View("Create",item);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Edit(SubCategory item)
        {
            if (ModelState.IsValid)
            {
                var CategoryName = db.Category.Where(it => it.Id == item.CategoryId).Select(it => it.Name).FirstOrDefault();

                item.SubCategoryCategory = item.Name + "-" + CategoryName;

                await BusinessService.UpdateAsync(item);
                return RedirectToIndex(item);
            }
            return View("Edit",item);
        }

    }
}
