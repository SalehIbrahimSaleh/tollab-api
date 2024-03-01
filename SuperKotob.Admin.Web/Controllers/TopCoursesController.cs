using SuperKotob.Admin.Core;
using SuperKotob.Admin.Utils.Configuration;
using SuperKotob.Admin.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Tollab.Admin.Data.Models.Views;
using Tollab.Admin.UseCases.TopCourse;

namespace Tollab.Admin.Web.Controllers
{
    [Authorize]
    public class TopCoursesController : BaseWebController<TopCourses, TopCourses>
    {
        TopCoursesService  _topCoursesService;

        public TopCoursesController(TopCoursesService service,
            IAppConfigurations appConfigurations) 
            : base(service, appConfigurations)
        {
            _topCoursesService = service;
        }

        public async Task<ActionResult> ShowTop(long? CountryId)
        {
            try
            {
                var requestInputs = GetRequestInputs();
                var response = await _topCoursesService.GetAsync(requestInputs);
                response.RequestInputs = requestInputs;
                ViewBag.RequestInputs = requestInputs;
                string viewName = GetViewName("ShowTop");
                return View(viewName, response);
            }
            catch (Exception ex)
            {
                // Console.WriteLine(ex.InnerException);
            }
            return null;
        }


    }
}