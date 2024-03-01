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
using Tollab.Admin.Web.Utils;
using Newtonsoft.Json.Linq;

namespace Tollab.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin")]

    public class SectionsController : BaseWebController<Section, Section>
    {
        private TollabContext db = new TollabContext();

        public SectionsController(IBusinessService<Section, Section> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateWithImage(Section item, HttpPostedFileBase ImageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var CountryName = db.Countries.Where(it => it.Id == item.CountryId).Select(it => it.Name).FirstOrDefault();

                    item.SectionCountry =item.Name + "-" + CountryName;

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
                            string uri = "https://tollab.azurewebsites.net/sws/api/SetPhoto";
                            var client = new HttpClient();
                            var imageObject = new { RecordId = item.Id, Table = "Section", CoulmnName = "Image", ImageType = (int)ImageFolders.SectionImages, Image = thePictureDataAsString };
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
                return View("Create",item);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditWithImage(Section item, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                var CountryName = db.Countries.Where(it => it.Id == item.CountryId).Select(it => it.Name).FirstOrDefault();

                item.SectionCountry = item.Name + "-" + CountryName;

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
                        string uri = "https://tollab.azurewebsites.net/sws/api/SetPhoto";
                        var client = new HttpClient();
                        var imageObject = new { RecordId = item.Id, Table = "Section", CoulmnName = "Image", ImageType = (int)ImageFolders.SectionImages, Image = thePictureDataAsString };
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
            return View("Edit",item);
        }

    }
}
