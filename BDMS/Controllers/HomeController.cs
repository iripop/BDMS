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

        #region Donations by expiration date
        public ActionResult DonationsByExpirationDate()
        {
            BloodforLifeEntities db = new BloodforLifeEntities();

            DateTime date1 = DateTime.Now;
            DateTime date2 = DateTime.Now.AddDays(7);
            List<DonationModel> listDonations = db.Donations.Where(x => (x.ExpirationDate >= date1  && x.ExpirationDate < date2) && x.IsDeleted==false).Select(x => new DonationModel
            {
                DonationID = x.DonationID,
                DonationType = x.DonationType,
                ExpirationDate = x.ExpirationDate
            }).ToList();
            ViewBag.DonationList = listDonations;
          // Donation don = new Donation();
           //ViewBag.Count = db.Donations.Where(x => x.ExpirationDate == don.ExpirationDate).Count();
            return View();
        }

        public ActionResult ShowDonationsByExpirationDateDetails(int DonationID)
        {
            BloodforLifeEntities db = new BloodforLifeEntities();

            List<DonationModel> listDonations = db.Donations.Where(x => x.IsDeleted == false && x.DonationID == DonationID).Select(x => new DonationModel
            {
                DonationID = x.DonationID,
                DonationType = x.DonationType,
                CrossBloodType = x.CrossBloodType,
                CrossRhFactor = x.CrossRhFactor,
                ExpirationDate = x.ExpirationDate,
                NumberOfUnits = x.NumberOfUnits,
                DonationSiteID = x.DonationSiteID,
                DonorID = x.DonorID,
                RecipientID = x.RecipientID,
                DonorFullName = x.Donor.DonorFullName,
                SiteName = x.DonationSite.SiteName,
                CreationDate = x.CreationDate,
                AcceptedDonation = x.AcceptedDonation,
                ReasonsForRejection = x.ReasonsForRejection


            }).ToList();

            ViewBag.DonationsList = listDonations;
            return PartialView("_ShowDonationsByExpirationDateDetails");
        }
        #endregion

        #region Donations count
        public ActionResult CountDonationsByType()
        {
            BloodforLifeEntities db = new BloodforLifeEntities();

            var don1 = db.Donations.Where(x => x.DonationType.Contains("Whole Blood")).Count();
            ViewBag.Count1 = don1;

            var don2 = db.Donations.Where(x => x.DonationType.Contains("Packed Red Blood Cells")).Count();
            ViewBag.Count2 = don2;

            var don3 = db.Donations.Where(x => x.DonationType.Contains("Platelets")).Count();
            ViewBag.Count3 = don3;

            var don4 = db.Donations.Where(x => x.DonationType.Contains("Plasma")).Count();
            ViewBag.Count4 = don4;

            return View();
        }
        #endregion

    }
}