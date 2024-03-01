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
using System.Reflection;
using Tollab.Admin.Data.Models.Views;
using System.Collections;
using System.Text;
using System.IO;
using CsvHelper;
using OfficeOpenXml;
using System.Data.SqlClient;

namespace Tollab.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin")]

    public class StudentPackagesController : BaseWebController<StudentPackage, StudentPackage>
    {
        private TollabContext db = new TollabContext();
        public StudentPackagesController(IBusinessService<StudentPackage, StudentPackage> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Create(StudentPackage item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var IsEnrollment = db.StudentPackages.AsNoTracking().Where(i => i.StudentId == item.StudentId && i.PackageId == item.PackageId).FirstOrDefault();
                    if (IsEnrollment!=null)
                    {
                        ViewBag.Error = "هذه الباقة اضيفت لهذا الطالب مسبقاً";
                        return View(item);
                    }
                    

                    item.EnrollmentDate = DateTime.UtcNow;
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
