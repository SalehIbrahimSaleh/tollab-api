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
    public class StudentSubjectsController : BaseWebController<StudentSubject, StudentSubject>
    {
        public StudentSubjectsController(IBusinessService<StudentSubject, StudentSubject> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }

       
    }
}