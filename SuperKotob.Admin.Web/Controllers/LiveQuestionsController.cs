using SuperKotob.Admin.Core;
using SuperKotob.Admin.Utils.Configuration;
using SuperKotob.Admin.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.Web.Controllers
{
    public class LiveQuestionsController : BaseWebController<VideoQuestion, VideoQuestion>
    {
        public LiveQuestionsController(IBusinessService<VideoQuestion, VideoQuestion> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }

        public override async Task<ActionResult> Index()
        {
            try
            {
                var requestInputs = GetRequestInputs();
                requestInputs.Add("isLiveQuestions", "1");
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

        public override async Task<ActionResult> Edit(long? id)
        {
            if (id == null || id.Value < 1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var requestInputs = GetRequestInputs();
            requestInputs.Add("isLiveQuestions", "1");
            requestInputs.Add("id", id.Value.ToString());
            var response = await BusinessService.GetAsync(requestInputs);

            if (!response.HasData || response.Data.FirstOrDefault() == null)
                return HttpNotFound();

            var order = response.Data.FirstOrDefault();
            string viewName = GetViewName("Edit");
            return View(viewName, order);
        }

        public override async Task<ActionResult> Delete(long? id)
        {
            if (id == null || id.Value < 1)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var requestInputs = GetRequestInputs();
            requestInputs.Add("isLiveQuestions", "1");
            requestInputs.Add("id", id.Value.ToString());
            var response = await BusinessService.GetAsync(requestInputs);
            if (!response.HasData || response.Data.FirstOrDefault() == null)
            {
                return HttpNotFound();
            }

            var model = response.Data.FirstOrDefault();
            string viewName = GetViewName("Delete");
            return View(viewName, model);
        }
    }
}