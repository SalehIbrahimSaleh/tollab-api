using Ma3ak.Admin.Data.Models;
using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Models;
using SuperKotob.Admin.Models;
using SuperKotob.Admin.Utils.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SuperKotob.Admin.Web.Controllers
{
    [Authorize]

    public class BaseWebController<TDataModel, TApiModel> : Controller
        where TDataModel : class, IDataModel, new()
        where TApiModel : class
    {
        public IBusinessService<TDataModel, TApiModel> BusinessService { get; private set; }

        private string _clientProfile;

        public BaseWebController(
            IBusinessService<TDataModel, TApiModel> service,
            IAppConfigurations appConfigurations)
        {
            this.BusinessService = service;
            _clientProfile = appConfigurations.ClientProfile.GetValueAsync().Result;
        }
        protected virtual RequestInputs GetRequestInputs()
        {
            var dictionary = new Dictionary<string, string>();
            var keys = Request.QueryString.AllKeys;

            for (var i = 0; i < Request.QueryString.Count; i++)
            {
                var key =  keys[i];
                var value = Request.QueryString[i];

                dictionary.Add(key, value);
            }

            return new RequestInputs(dictionary);
        }

        [HttpGet]
        public virtual async Task<ActionResult> AutoComplete(string text, string field, bool isIdList = false)
        {
            
            var requestInputs = GetRequestInputs();
            var response = await BusinessService.AutoCompleteAsync(text, isIdList, requestInputs, field);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //public async Task<ActionResult> AutoCompleteAreaMangersOnly(string text, bool isIdList = false)
        //{
        //    var requestInputs = GetRequestInputs();
        //    var response = await BusinessService.AutoCompleteAreaMangersOnly(text, isIdList, requestInputs);
        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}


        // GET: Orders/Details/5
        public virtual async Task<ActionResult> Details(long? id)
        {
            if (id == null || id.Value < 1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var response = await BusinessService.GetAsync(id.Value);
            if (!response.HasData)
            {
                return HttpNotFound();
            }
            var model = response.Data.FirstOrDefault();
            string viewName = GetViewName("Details");

            return View(viewName, model);
        }

        protected string GetViewName(string name)
        {
            if (typeof(TDataModel) == typeof(ModelDB))
                return _clientProfile + name;

            return name;
        }
        // GET: Orders
        public virtual async Task<ActionResult> Index()
        {
            try
            {
                var requestInputs = GetRequestInputs();
            var response = await BusinessService.GetAsync(requestInputs);
            response.RequestInputs = requestInputs;
            ViewBag.RequestInputs = requestInputs;
            string viewName = GetViewName("Index");
            return View(viewName, response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return null;
        }
        // GET: Orders/Create
        public async Task<ActionResult> Create()
        {
            //var response = BusinessService.GetAsync();
            var requestInputs = GetRequestInputs();//Request.Params.Keys;

            var response = new TDataModel();
            if (requestInputs !=null)
            {
                response.RequestInputs = requestInputs;           
                foreach (var item in requestInputs.ToDictionary())
                {
                    Type type = response.GetType();
                    foreach (PropertyInfo prop in type.GetProperties())
                    {
                        PropertyInfo itemProp = response.GetType().GetProperty(prop.Name, BindingFlags.Public | BindingFlags.Instance);

                        var name = itemProp.Name;
                        if (string.Equals(item.Key, itemProp.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            itemProp.SetValue(response, item.Value);
                   
                        }
                    }
                   
                }
            }
       
            ViewBag.IdValue= Request.QueryString["Id"];
            string viewName = GetViewName("Create");
            return View(viewName, response);
        }

        public async Task<ActionResult> Create2()
        {
            //var response = BusinessService.GetAsync();
            var requestInputs = GetRequestInputs();//Request.Params.Keys;

            var response = new TDataModel();
            if (requestInputs != null)
            {
                response.RequestInputs = requestInputs;
                foreach (var item in requestInputs.ToDictionary())
                {
                    Type type = response.GetType();
                    foreach (PropertyInfo prop in type.GetProperties())
                    {
                        PropertyInfo itemProp = response.GetType().GetProperty(prop.Name, BindingFlags.Public | BindingFlags.Instance);

                        var name = itemProp.Name;
                        if (string.Equals(item.Key, itemProp.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            itemProp.SetValue(response, item.Value);

                        }
                    }

                }
            }

            ViewBag.IdValue = Request.QueryString["Id"];
            string viewName = GetViewName("Create2");
            return View(viewName, response);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Create(TDataModel item)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await BusinessService.CreateAsync(item);
                    return RedirectToAction("Index");
                }

                ////var response = await BusinessService.GetAsync(1);
                ////response.Data = new List<TDataModel>()
                ////{
                ////    item
                ////};

                //string viewName = GetViewName("Create");
                return View(item/*, response*/);
            }
            catch (Exception ex)
            {
                //if (ex.HResult== -2146233087)
                //{
                //    View
                //    ViewBage.SameDateError = "You Entered This Data Before";
                //    return View(item);
                //}
                throw ex;
            }
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Create2(TDataModel item)
        {
            if (ModelState.IsValid)
            {
                await BusinessService.CreateAsync(item);
                return RedirectToAction("Index");
            }

            ////var response = await BusinessService.GetAsync(1);
            ////response.Data = new List<TDataModel>()
            ////{
            ////    item
            ////};

            //string viewName = GetViewName("Create");
            return View(item/*, response*/);
        }
        protected virtual RedirectToRouteResult RedirectToIndex(TDataModel item)
        {
            return RedirectToAction("Index");
        }

        // GET: Orders/Edit/5
        public virtual async Task<ActionResult> Edit(long? id)
        {
            if (id == null || id.Value < 1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var response = await BusinessService.GetAsync(id.Value);

            if (!response.HasData || response.Data.FirstOrDefault() == null)
                return HttpNotFound();

            var order = response.Data.FirstOrDefault();
            string viewName = GetViewName("Edit");
            return View(viewName, order);
        }

        // [Bind(Include = "Id,CustomerId,CustomerAddressId,CustomerCardId,EstimatedTimeArrival,StatusId,PromoCodeId,SubTotalBeforePromoCode,SubTotal,Total,TemplateId,TemplateTypeId,CreatedOn,CreatedBy,ModifiedOn,ModifiedBy")] 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> Edit(TDataModel item)
        {
            if (ModelState.IsValid)
            {
                await BusinessService.UpdateAsync(item);
                return RedirectToIndex(item);
            }
            //var response = await BusinessService.GetAsync();
            //if (!response.HasData || response.Data.FirstOrDefault() == null)
            //    return HttpNotFound();

            //var order = response.Data.FirstOrDefault();

            //string viewName = GetViewName("Edit");
            return View(/*viewName, order*/item);
        }

        // GET: Orders/Delete/5
        public virtual async Task<ActionResult> Delete(long? id)
        {
            if (id == null || id.Value < 1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var response = await BusinessService.GetAsync(id.Value);
            if (!response.HasData || response.Data.FirstOrDefault() == null)
            {
                return HttpNotFound();
            }

            var model = response.Data.FirstOrDefault();
            string viewName = GetViewName("Delete");
            return View(viewName, model);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public virtual async Task<ActionResult> DeleteConfirmed(long? id)
        {
            var response = await BusinessService.DeleteAsync(id.Value);
            if (response.HasErrors)
            {
                string viewName = GetViewName("_DeleteFailed");
                return View(viewName, response.Errors);
            }


            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

    }
}