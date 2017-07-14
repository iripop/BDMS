using BDMS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace BDMS.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        BloodForLifeDBEntities db = new BloodForLifeDBEntities();
        #region Donations by expiration date
        public ActionResult DonationsByExpirationDate(ChartDonationsModel cd)
        {
            DateTime date1 = DateTime.Now;
            DateTime date2 = DateTime.Now.AddDays(7);
            List<DonationModel> listDonations = db.Donations.Where(x => (x.ExpirationDate >= date1 && x.ExpirationDate < date2) && x.DonationIsArchived == false).Select(x => new DonationModel
            {
                DonationID = x.DonationID,
                DonationType = x.DonationType,
                ExpirationDate = x.ExpirationDate
            }).ToList();
            ViewBag.DonationList = listDonations;
            return View(new ChartDonationsModel { Count = new BloodCountModel(), Donation = new DonationModel() });
        }

        public ActionResult ShowDonationsByExpirationDateDetails(int DonationID)
        {
            List<DonationModel> listDonations = db.Donations.Where(x => x.DonationIsArchived == false && x.DonationID == DonationID).Select(x => new DonationModel
            {
                DonationID = x.DonationID,
                DonationType = x.DonationType,
                CrossBloodType = x.CrossBloodType,
                CrossRhFactor = x.CrossRhFactor,
                ExpirationDate = x.ExpirationDate,
                NumberOfunits = x.NumberOfunits,
                DonationSiteID = x.DonationSiteID,
                DonorID = x.DonorID,
                RecipientID = x.RecipientID,
                DonorFullName = x.Donor.DonorFullName,
                SiteName = x.DonationSite.SiteName,
                CreationDate = x.CreationDate,
                AcceptanceStatus = x.AcceptanceStatus,
                ReasonsForRejection = x.ReasonsForRejection


            }).ToList();

            ViewBag.DonationsList = listDonations;
            return PartialView("_ShowDonationsByExpirationDateDetails");
        }
        #endregion

        #region Chart
        [HttpGet]
        public ActionResult ChartColumn()
        {
            var _context = new BloodForLifeDBEntities();
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();
            var results = (from c in _context.BloodCounts select c);
            results.ToList().ForEach(rs => xValue.Add(rs.BloodType));
            results.ToList().ForEach(rs => yValue.Add(rs.Count));
            new Chart(width: 500, height: 300)
              .AddTitle("Donations by type")
              .AddSeries("Default", chartType: "Column", xValue: xValue, yValues: yValue)
              .Write("png");

            return null;
        }
        #endregion
    }
}