using SuperKotob.Admin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperKotob.Admin.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            TollabContext db = new TollabContext();
            var HomeStatistics=  db.HomeStatistics.FirstOrDefault();
            if (HomeStatistics!=null)
            {
                var countries = db.Countries.ToList();
                HomeStatistics.Countries = countries;
            }
             return View(HomeStatistics);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

    }
}