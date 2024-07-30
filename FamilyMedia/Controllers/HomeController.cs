using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FamilyMedia.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "This ia a simple blog site, made by dot net framework.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "This web application is developed by: S M Faisal ";

            return View();
        }
    }
}