using BDMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BMDS.Controllers
{
    [Authorize]
    public class DonationSiteController : Controller
    {
        #region DonationSite GET
        public ActionResult DonationSite()
        {
            BloodforLifeEntities db = new BloodforLifeEntities();

            List<string> mobile = new List<string>(new string[] { "True", "False" });
            ViewBag.MobileSiteList = new SelectList(mobile);

            List<DonationSiteModel> listDonSite = db.DonationSites.Where(x => x.IsSiteArchived == false).Select(x => new DonationSiteModel
            {
                DonationSiteID = x.DonationSiteID,
                SiteName = x.SiteName,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                RegistrationEmail = x.RegistrationEmail,
                RegistrationPhone = x.RegistrationPhone,
                Address = x.Address,
                City = x.City,
                Zip = x.Zip,
                StaffingRequired = x.StaffingRequired,
                MobileSite = x.MobileSite


            }).ToList();

            ViewBag.DonationSiteList = listDonSite;
            return View();

        }
        #endregion

        #region Donation Site POST
        [HttpPost]
        public ActionResult DonationSite(DonationSiteModel model)
        {
            try
            {
                BloodforLifeEntities db = new BloodforLifeEntities();

                List<string> mobile = new List<string>(new string[] { "True", "False" });
                ViewBag.MobileSiteList = new SelectList(mobile);

                if (model.DonationSiteID > 0)
                {
                    //Update a donation site
                    DonationSite don = db.DonationSites.SingleOrDefault(x => x.DonationSiteID == model.DonationSiteID && x.IsSiteArchived == false);

                    don.SiteName = model.SiteName;
                    don.StartDate = model.StartDate;
                    don.EndDate = model.EndDate;
                    don.RegistrationEmail = model.RegistrationEmail;
                    don.RegistrationPhone = model.RegistrationPhone;
                    don.Address = model.Address;
                    don.City = model.City;
                    don.Zip = model.Zip;
                    don.StaffingRequired = model.StaffingRequired;
                    don.MobileSite = model.MobileSite;

                    db.SaveChanges();

                }
                else
                {
                    //Insert a donation site in database
                    DonationSite don = new DonationSite();
                    don.SiteName = model.SiteName;
                    don.StartDate = model.StartDate;
                    don.EndDate = model.EndDate;
                    don.RegistrationEmail = model.RegistrationEmail;
                    don.RegistrationPhone = model.RegistrationPhone;
                    don.Address = model.Address;
                    don.City = model.City;
                    don.Zip = model.Zip;
                    don.StaffingRequired = model.StaffingRequired;
                    don.MobileSite = model.MobileSite;
                    don.IsSiteArchived = false;

                    db.DonationSites.Add(don);
                    db.SaveChanges();

                    int latestDonationSiteID = don.DonationSiteID;
                }

                return View(model);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region Delete donation site
        public JsonResult DeleteDonationSite(int donationSiteID)
        {
            BloodforLifeEntities db = new BloodforLifeEntities();

            bool result = false;
            DonationSite don = db.DonationSites.SingleOrDefault(x => x.IsSiteArchived == false && x.DonationSiteID == donationSiteID);

            if (don != null)
            {
                don.IsSiteArchived = true;
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Donation site details
        public ActionResult ShowDonationSiteDetail(int DonationSiteID)
        {
            BloodforLifeEntities db = new BloodforLifeEntities();

            List<DonationSiteModel> listDonationSites = db.DonationSites.Where(x => x.IsSiteArchived == false && x.DonationSiteID == DonationSiteID).Select(x => new DonationSiteModel
            {
                DonationSiteID = x.DonationSiteID,
                SiteName = x.SiteName,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                RegistrationEmail = x.RegistrationEmail,
                RegistrationPhone = x.RegistrationPhone,
                Address = x.Address,
                City = x.City,
                Zip = x.Zip,
                StaffingRequired = x.StaffingRequired,
                MobileSite = x.MobileSite

            }).ToList();

            ViewBag.DonationSitesList = listDonationSites;

            return PartialView("_ShowDonationSiteDetail");

        }
        #endregion

        #region Add or edit recipient
        public ActionResult AddEditDonationSite(int DonationSiteID)
        {
            BloodforLifeEntities db = new BloodforLifeEntities();

            List<string> mobile = new List<string>(new string[] { "True", "False" });
            ViewBag.MobileSiteList = new SelectList(mobile);

            DonationSiteModel model = new DonationSiteModel();

            if (DonationSiteID > 0)
            {
                DonationSite don = db.DonationSites.SingleOrDefault(x => x.DonationSiteID == DonationSiteID && x.IsSiteArchived == false);
                model.DonationSiteID = don.DonationSiteID;
                model.SiteName = don.SiteName;
                model.StartDate = don.StartDate;
                model.EndDate = don.EndDate;
                model.RegistrationEmail = don.RegistrationEmail;
                model.RegistrationPhone = don.RegistrationPhone;
                model.Address = don.Address;
                model.City = don.City;
                model.Zip = don.Zip;
                model.StaffingRequired = don.StaffingRequired;
                model.MobileSite = don.MobileSite;

            }

            return PartialView("_AddEditDonationSite", model);

        }
        #endregion

        #region Search
        public ActionResult GetSearchDonationSite(string SearchText)
        {
            BloodforLifeEntities db = new BloodforLifeEntities();
            List<DonationSiteModel> list = db.DonationSites.Where(x =>x.IsSiteArchived==false && x.SiteName.Contains(SearchText) ||
            x.Address.Contains(SearchText) ||
            x.City.Contains(SearchText) ||
            x.Zip.Contains(SearchText) ||
            x.StartDate.ToString().Contains(SearchText) ||
            x.EndDate.ToString().Contains(SearchText) ||
            x.RegistrationEmail.Contains(SearchText) ||
            x.RegistrationPhone.Contains(SearchText) ||
            x.MobileSite.ToString().Contains(SearchText)).Where(x=> x.IsSiteArchived==false).Select(x => new DonationSiteModel
            {
                SiteName = x.SiteName,
                Address = x.Address,
                City = x.City,
                Zip = x.Zip,
                MobileSite = x.MobileSite,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                RegistrationPhone = x.RegistrationPhone,
                RegistrationEmail = x.RegistrationEmail


            }).ToList();

            return PartialView("_SearchDonationSite", list);
        }
        #endregion
    }
}