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
using Tollab.Admin.Core.Enums;
using Tollab.Admin.Web.Services.Vimeo;
using Tollab.Admin.Web.Services.FileUpload;

namespace Tollab.Admin.Web.Controllers
{
    [Authorize]

    public class OffersController : BaseWebController<Offer,Offer>
    {
        private const string VimeoToken = "2878d5dde0fe009cc71041e7c82d5292";
        private string PasswordForVideo = "Tollab@hacker!@#%^&*(@147852";

        private TollabContext db = new TollabContext();
        private IVimeoUploadService _vimeoUploadService = new VimeoUploadService();
        private IFileUploadService _imageUploadService = new FileUploadService();
        private readonly IRepository<OfferCountry> _offerCountryRepository;

        public OffersController(IBusinessService<Offer, Offer> service, IAppConfigurations appConfigurations ,
                                IRepository<OfferCountry> offerCountryRepository) : base(service, appConfigurations)
        {
            this._offerCountryRepository = offerCountryRepository;
        }


        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateWithImage(Offer item, HttpPostedFileBase videoFile, HttpPostedFileBase ImageFile , HttpPostedFileBase VideoThumbnail)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    item.Countries = GetCountries(Request.Form["CountriesString"]);
                    switch(item.OfferLinkTypeId)
                    {
                        case OfferLinkTypeEnum.COURSE:
                            item.TrackId = null;
                            item.ExternalLink = null;
                            break;
                        case OfferLinkTypeEnum.TRACK:
                            item.CourseId = null;
                            item.ExternalLink = null;
                            break;
                        case OfferLinkTypeEnum.LINK:
                            item.CourseId = null;
                            item.TrackId = null;
                            break;
                    }
                    if (videoFile != null && VideoThumbnail != null && item.OfferContentTypeId == OfferContentTypeEnum.VIDEO)
                    {
                        var (success ,videoURL , videoURI, errorDetails) = await _vimeoUploadService.TryUpload(videoFile, item.Tilte, videoFile.ContentLength, PasswordForVideo, VimeoToken);
                        if(success)
                        {
                            item.VideoURL = videoURL;
                            item.VideoURI = videoURI;
                            var DataReturned = await BusinessService.CreateAsync(item);
                            if (item.OfferLinkTypeId == OfferLinkTypeEnum.LINK)
                            {
                                var offer = DataReturned.Data.FirstOrDefault();
                                await AddOfferCountries(item.Countries, offer);
                            }
                            _imageUploadService.UploadImage(item.Id, nameof(Offer), nameof(Offer.VideoThumbnail), (int)ImageFolders.OffersImages, VideoThumbnail);
                        }
                    }
                    else if(ImageFile != null && item.OfferContentTypeId == OfferContentTypeEnum.IMAGE )
                    {
                        try
                        {
                            var DataReturned = await BusinessService.CreateAsync(item);
                            if (item.OfferLinkTypeId == OfferLinkTypeEnum.LINK)
                            {
                                var offer = DataReturned.Data.FirstOrDefault();
                                await AddOfferCountries(item.Countries, offer);
                            }
                            if (DataReturned.HasData)
                            {
                                await _imageUploadService.UploadImage(item.Id, nameof(Offer), nameof(Offer.Image), (int)ImageFolders.OffersImages, ImageFile);
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


        public override async Task<ActionResult> Edit(long? id)
        {

            if (id == null || id.Value < 1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var response = await BusinessService.GetAsync(id.Value);

            if (!response.HasData || response.Data.FirstOrDefault() == null)
                return HttpNotFound();

            var offer = response.Data.FirstOrDefault();
            offer.Countries = offer.OfferCountries.Select(country => country.CountryId).ToList();
            string viewName = GetViewName("Edit");
            return View(viewName, offer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditWithImage(Offer item, HttpPostedFileBase videoFile, HttpPostedFileBase ImageFile, HttpPostedFileBase VideoThumbnail)
        {
            if (ModelState.IsValid)
            {
                item.Countries = GetCountries(Request.Form["CountriesString"]);
                switch (item.OfferLinkTypeId)
                {
                    case OfferLinkTypeEnum.COURSE:
                        item.TrackId = null;
                        item.ExternalLink = null;
                        break;
                    case OfferLinkTypeEnum.TRACK:
                        item.CourseId = null;
                        item.ExternalLink = null;
                        break;
                    case OfferLinkTypeEnum.LINK:
                        item.CourseId = null;
                        item.TrackId = null;
                        break;
                }
                var oldItem = (await BusinessService.GetAsync(item.Id)).Data.FirstOrDefault();
                if (videoFile != null && VideoThumbnail != null && item.OfferContentTypeId == OfferContentTypeEnum.VIDEO)
                {
                    if (oldItem.VideoURI != null)
                    {
                        _vimeoUploadService.DeleteFromViemoAsync(oldItem.VideoURI , VimeoToken);
                    }
                    var (success, videoURL, videoURI, errorDetails) = await _vimeoUploadService.TryUpload(videoFile, item.Tilte, videoFile.ContentLength, PasswordForVideo, VimeoToken);
                    if (success)
                    {
                        item.VideoURL = videoURL;
                        item.VideoURI = videoURI;
                        var DataReturned = await BusinessService.UpdateAsync(item);
                        RemoveOfferCountries(oldItem.OfferCountries);
                        if (item.OfferLinkTypeId == OfferLinkTypeEnum.LINK)
                        {
                            var offer = DataReturned.Data.FirstOrDefault();
                            await AddOfferCountries(item.Countries, offer);
                        }
                        await _imageUploadService.UploadImage(item.Id, nameof(Offer), nameof(Offer.VideoThumbnail), (int)ImageFolders.OffersImages, VideoThumbnail);
                    }
                }
                else if (ImageFile != null && item.OfferContentTypeId == OfferContentTypeEnum.IMAGE)
                {
                    try
                    {
                        var DataReturned = await BusinessService.UpdateAsync(item);
                        if(DataReturned.HasData)
                        {
                            RemoveOfferCountries(oldItem.OfferCountries);
                            if (item.OfferLinkTypeId == OfferLinkTypeEnum.LINK)
                            {
                                var offer = DataReturned.Data.FirstOrDefault();
                                await AddOfferCountries(item.Countries, offer);
                            }
                            await _imageUploadService.UploadImage(item.Id, nameof(Offer), nameof(Offer.Image), (int)ImageFolders.OffersImages, ImageFile);
                        }
                    }
                    catch (Exception ex)
                    { }
                }
                
                return RedirectToIndex(item);
            }
            return View("Edit", item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> DeleteConfirmed(long? id)
        {
            var oldItem = (await BusinessService.GetAsync(id.Value)).Data.FirstOrDefault();
            var response = await BusinessService.DeleteAsync(id.Value);
            if (response.HasErrors)
            {
                string viewName = GetViewName("_DeleteFailed");
                return View(viewName, response.Errors);
            }
            if(oldItem.OfferContentTypeId == OfferContentTypeEnum.VIDEO)
               await _vimeoUploadService.DeleteFromViemoAsync(oldItem.VideoURI, VimeoToken);

            return RedirectToAction("Index");
        }


        private IEnumerable<long> GetCountries(string countriesString)
        {
            if (string.IsNullOrEmpty(countriesString))
                return new List<long>();
            if(countriesString.Contains(","))
            {
                var countries = countriesString.Split(',');
                return countries.Select(country => long.Parse(country)).ToList();
            }
            return new List<long>() { long.Parse(countriesString) };
        }

        private void RemoveOfferCountries(List<OfferCountry> offerCountries)
        {
            foreach (var offerCountry in offerCountries)
            {
                _offerCountryRepository.DeleteAsync(offerCountry.Id);
            }
        }

        private async Task AddOfferCountries(IEnumerable<long> countries, Offer offer)
        {
            foreach (var country in countries)
            {
                if (offer != null)
                {
                    var offerCountryDataReturned = await _offerCountryRepository.CreateAsync(new OfferCountry() { CountryId = country, OfferId = offer.Id });
                }
            }
        }
    }
}
