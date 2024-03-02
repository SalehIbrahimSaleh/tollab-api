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
using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Tollab.Admin.Web.Utils;

namespace Tollab.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin")]

    public class SubjectsController : BaseWebController<Subject,Subject>
    {
        private TollabContext db = new TollabContext();

        public SubjectsController(IBusinessService<Subject, Subject> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateWithImage(Subject item, HttpPostedFileBase ImageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var DepartmentSubCategory = db.Departments.Where(it => it.Id == item.DepartmentId).Select(it => it.DepartmentSubCategory).FirstOrDefault();

                    item.SubjectDepartment = item.Name + "-" + DepartmentSubCategory;

                    var DataReturned = await BusinessService.CreateAsync(item);
                    if (ImageFile != null && DataReturned.HasData == true)
                    {
                        try
                        {
                            byte[] thePictureAsBytes = new byte[ImageFile.ContentLength];
                            using (BinaryReader theReader = new BinaryReader(ImageFile.InputStream))
                            {
                                thePictureAsBytes = theReader.ReadBytes(ImageFile.ContentLength);
                            }
                            string thePictureDataAsString = Convert.ToBase64String(thePictureAsBytes);
                            string uri = "http://tollab.com/tollab/api/SetPhoto";
                            var client = new HttpClient();
                            var imageObject = new { RecordId = item.Id, Table = "Subject", CoulmnName = "Image", ImageType = (int)ImageFolders.SubjectImages, Image = thePictureDataAsString };
                            var response = await client.PostAsJsonAsync(uri, imageObject);
                            var responseString = await response.Content.ReadAsStringAsync();
                            var tempResponse = JObject.Parse(responseString);
                            responseString = tempResponse.ToString();
                            var responseCode = response.StatusCode;
                            if (responseCode == HttpStatusCode.OK)
                            {
                                var img = tempResponse.GetValue("model").ToString();
                            }
                        }
                        catch (Exception ex)
                        { }
                    }

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
        public async Task<ActionResult> EditWithImage(Subject item, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                var DepartmentSubCategory = db.Departments.Where(it => it.Id == item.DepartmentId).Select(it => it.DepartmentSubCategory).FirstOrDefault();

                item.SubjectDepartment = item.Name + "-" + DepartmentSubCategory;
                var DataReturned = await BusinessService.UpdateAsync(item);
                if (ImageFile != null && DataReturned.HasData == true)
                {
                    try
                    {
                        byte[] thePictureAsBytes = new byte[ImageFile.ContentLength];
                        using (BinaryReader theReader = new BinaryReader(ImageFile.InputStream))
                        {
                            thePictureAsBytes = theReader.ReadBytes(ImageFile.ContentLength);
                        }
                        string thePictureDataAsString = Convert.ToBase64String(thePictureAsBytes);
                        string uri = "http://tollab.com/tollab/api/SetPhoto";
                        var client = new HttpClient();
                        var imageObject = new { RecordId = item.Id, Table = "Subject", CoulmnName = "Image", ImageType = (int)ImageFolders.SubjectImages, Image = thePictureDataAsString };
                        var response = await client.PostAsJsonAsync(uri, imageObject);
                        var responseString = await response.Content.ReadAsStringAsync();
                        var tempResponse = JObject.Parse(responseString);
                        responseString = tempResponse.ToString();
                        var responseCode = response.StatusCode;
                        if (responseCode == HttpStatusCode.OK)
                        {
                            var img = tempResponse.GetValue("model").ToString();
                        }
                    }
                    catch (Exception ex)
                    { }
                }
                return RedirectToIndex(item);
            }
            return View("Edit", item);
        }

    }
}
