using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tollab.Admin.Data.Models;
using SuperKotob.Admin.Data;
using SuperKotob.Admin.Web.Controllers;
using SuperKotob.Admin.Core;
using SuperKotob.Admin.Utils.Configuration;
using Tollab.Admin.Data.Models;
using System.IO;
using System.Net.Http;
using Tollab.Admin.Web.Utils;
using Newtonsoft.Json.Linq;

namespace Tollab.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin")]

    public class CountriesController : BaseWebController<Country, Country>
    {
        public CountriesController(IBusinessService<Country, Country> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public   async Task<ActionResult> CreateWithImage(Country item, HttpPostedFileBase ImageFile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                  var DataReturned =  await BusinessService.CreateAsync(item);
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
                            var imageObject = new { RecordId = item.Id, Table = "Country", CoulmnName = "Flag", ImageType =(int) ImageFolders.Flags, Image = thePictureDataAsString };
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
        public   async Task<ActionResult> EditWithImage(Country item, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                var DataReturned= await BusinessService.UpdateAsync(item);
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
                        var imageObject = new { RecordId = item.Id, Table = "Country", CoulmnName = "Flag", ImageType = (int)ImageFolders.Flags, Image = thePictureDataAsString };
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
