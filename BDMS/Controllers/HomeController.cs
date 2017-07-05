using BDMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Helpers;
using System.Collections;

namespace BMDS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        #region About
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        #endregion

        #region Contact
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        #endregion

    }
}