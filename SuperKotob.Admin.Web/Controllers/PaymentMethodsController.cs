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
    [Authorize]
    public class PaymentMethodsController : BaseWebController<PaymentMethod, PaymentMethod>
    {
        private TollabContext db = new TollabContext();
        public PaymentMethodsController(IBusinessService<PaymentMethod, PaymentMethod> service, IAppConfigurations appConfigurations) : base(service, appConfigurations)
        {
        }
    }
}
