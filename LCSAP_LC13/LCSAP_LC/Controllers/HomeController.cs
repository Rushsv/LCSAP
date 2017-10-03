using LCSAP_LC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LCSAP_LC.Controllers
{

    public class HomeController : Controller
    {
        private LCSAPServices lcServices = new LCSAPServices();
        public ActionResult Index()
        {
            ViewBag.CurrentTerm = lcServices.CurrentTerm;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "LCSAP Program";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact page.";

            return View();
        }
    }
}