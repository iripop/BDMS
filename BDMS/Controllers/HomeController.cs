using BDMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BMDS.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
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

        public ActionResult DonationsByExpirationDate(DonationModel mdl)
        {
            BloodforLifeEntities db = new BloodforLifeEntities();
            ViewBag.Count = db.Donations.Where(x => x.ExpirationDate == mdl.ExpirationDate).Count();

            List<DonationModel> listDonations = db.Donations.Where(x => x.ExpirationDate >= DateTime.Now.Date.AddDays(7) && x.IsDeleted==false).Select(x => new DonationModel
            {
                DonationID = x.DonationID,
                DonationType = x.DonationType,
                ExpirationDate = x.ExpirationDate
            }).ToList();
            ViewBag.DonationList = listDonations;
            ViewBag.Count = db.Donations.Where(x => x.ExpirationDate == mdl.ExpirationDate).Count();
            return View();
        }
    }
}