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

    public class PromoCodesController : BaseWebController<PromoCode,PromoCode>
    {
        private TollabContext db = new TollabContext();

        public PromoCodesController(IBusinessService<PromoCode, PromoCode> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Create(PromoCode  promoCode)
        {
            try
            {
               
                if (ModelState.IsValid)
                {
                    //for (; ; )
                    //{
                    //    var promo = Guid.NewGuid().ToString().Substring(0, 6);

                    //    var Isfound = CheckIfPromocodeCteatedBeforeOrNot(promo);
                    //    if (Isfound == false)
                    //    {
                    //        promoCode.PromoCodeText = promo;
                    //        break;
                    //    }
                    //}
                    var Isfound = CheckIfPromocodeCteatedBeforeOrNot(promoCode.PromoCodeText);
                    if (Isfound == true)
                    {
                        ViewBag.Error = "Try with another code because this created before";
                        return View(promoCode);
                    }
                    string pattern = GetPatten(promoCode);
                    promoCode.Pattern = pattern;
                    promoCode.CreationDate = DateTime.UtcNow;
                    promoCode.Active = true;
                    promoCode.UsedCount = 0;
                    await BusinessService.CreateAsync(promoCode);
                    return RedirectToAction("Index");
                }

                return View(promoCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<ActionResult> Edit(PromoCode  promoCode)
        {
            if (ModelState.IsValid)
            {
                string pattern = GetPatten(promoCode);
                promoCode.Pattern = pattern;
                await BusinessService.UpdateAsync(promoCode);
                return RedirectToIndex(promoCode);
            }
            return View(promoCode);
        }

        private bool CheckIfPromocodeCteatedBeforeOrNot(string Promocode)
        {
            var promo=  db.PromoCodes.Where(item => item.PromoCodeText == Promocode).FirstOrDefault();
            if (promo==null)
            {
                return false;
            }
            return true;
        }
        private string GetPatten(PromoCode promoCode)
        {
            string pattern = "";
            if (promoCode.CountryId==null)
            {
                pattern = "0";
            }
            if (promoCode.CountryId>0)
            {
                pattern = promoCode.CountryId.ToString();
            }
            if (promoCode.SectionId==null)
            {
                pattern = pattern + "0";
            }
            if (promoCode.SectionId>0)
            {
                pattern = pattern + promoCode.SectionId.ToString();
            }
            if (promoCode.CategoryId==null)
            {
                pattern = pattern + "0";
            }
            if (promoCode.CategoryId>0)
            {
                pattern = pattern + promoCode.CategoryId.ToString();
            }
            if (promoCode.SubCategoryId==null)
            {
                pattern = pattern + "0";
            }
            if (promoCode.SubCategoryId>0)
            {
                pattern = pattern + promoCode.SubCategoryId.ToString();
            }
            if (promoCode.DepartmentId==null)
            {
                pattern = pattern + "0";
            }
            if (promoCode.DepartmentId>0)
            {
                pattern = pattern + promoCode.DepartmentId.ToString();
            }
            return pattern;
        }
    }
}
