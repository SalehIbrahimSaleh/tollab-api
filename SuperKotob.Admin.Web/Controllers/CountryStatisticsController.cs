using SuperKotob.Admin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tollab.Admin.Web.Controllers
{
    [Authorize]
    public class CountryStatisticsController : Controller
    {
        TollabContext db = new TollabContext();

        // GET: CountryStatistics
        public ActionResult Index(long? Id)
        {
            var CountryStatistics = db.CountryStatistics.Where(i => i.Id == Id).FirstOrDefault();
            return View(CountryStatistics);
        }
    }
}