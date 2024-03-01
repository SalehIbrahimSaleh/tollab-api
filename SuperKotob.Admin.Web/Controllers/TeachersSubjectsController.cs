using SuperKotob.Admin.Core;
using SuperKotob.Admin.Utils.Configuration;
using SuperKotob.Admin.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tollab.Admin.Data.Models.Views;

namespace Tollab.Admin.Web.Controllers
{
    [Authorize]
    public class TeachersSubjectsController : BaseWebController<TeachersSubject, TeachersSubject>
    {
        public TeachersSubjectsController(IBusinessService<TeachersSubject, TeachersSubject> service,
            IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }
    }
}