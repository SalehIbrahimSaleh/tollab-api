using Tollab.Admin.Data.Models;
using SuperKotob.Admin.Core;
using SuperKotob.Admin.Utils.Configuration;
using SuperKotob.Admin.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tollab.Admin.Data.Models;

namespace OrthoHouse.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : BaseWebController<Role, Role>
    {
        public RolesController(IBusinessService<Role, Role> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }
    }
}