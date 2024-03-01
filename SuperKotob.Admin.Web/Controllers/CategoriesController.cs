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
    public class CategoriesController : BaseWebController<Category, Category>
    {
        private TollabContext db = new TollabContext();

        public CategoriesController(IBusinessService<Category, Category> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Create(Category item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var SectionCountry = db.Sections.Where(it => it.Id == item.SectionId).Select(it => it.SectionCountry).FirstOrDefault();

                    item.CategorySection = item.Name + "-" + SectionCountry;

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
        public override async Task<ActionResult> Edit(Category item)
        {
            if (ModelState.IsValid)
            {
                var SectionCountry = db.Sections.Where(it => it.Id == item.SectionId).Select(it => it.SectionCountry).FirstOrDefault();

                item.CategorySection = item.Name + "-" + SectionCountry;

                await BusinessService.UpdateAsync(item);
                return RedirectToIndex(item);
            }
       
            return View(item);
        }

    }
}
