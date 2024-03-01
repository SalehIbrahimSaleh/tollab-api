using SuperKotob.Admin.Core;
using SuperKotob.Admin.Utils.Configuration;
using SuperKotob.Admin.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.Web.Controllers
{
    [Authorize]
    public class ExamQuestionTypesController : BaseWebController<ExamQuestionType, ExamQuestionType>
    {
        public ExamQuestionTypesController(IBusinessService<ExamQuestionType, ExamQuestionType> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }

        
    }
}