using BDMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BMDS.Controllers
{
    [Authorize]
    public class DonationController : Controller
    {
        BloodForLifeDBEntities db = new BloodForLifeDBEntities();
        #region GET donation
        public ActionResult Donation()
        {
            
            List<Donor> listDonor = db.Donors.Where(x => x.ActiveDonor == true && x.ArchivedDonor == false).ToList();
            ViewBag.DonorsList = new SelectList(listDonor, "DonorID", "DonorFullName");

            List<Recipient> listRecipient = db.Recipients.Where(x => x.IsRecipientArchived == false).ToList();
            ViewBag.RecipientsList = new SelectList(listRecipient, "RecipientID", "RecipientCodedName");

            List<DonationSite> listDonationSite = db.DonationSites.Where(x => x.IsSiteArchived == false).ToList();
            ViewBag.DonationSiteList = new SelectList(listDonationSite, "DonationSiteID", "SiteName");

            List<string> listDonationTypes = new List<string>(new string[] { "Whole Blood", "Packed Red Blood Cells", "Platelets", "Plasma" });
            ViewBag.DonationTypeList = new SelectList(listDonationTypes);

            List<BloodCount> listBloodType = db.BloodCounts.ToList();
            ViewBag.BloodTypeList = new SelectList(listDonor, "BloodTypeID", "BloodType");

            List<string> rhFactor = new List<string>(new string[] { "+(positive)", "-(negative)" });
            ViewBag.RhFactorList = new SelectList(rhFactor);

            List<string> accepted = new List<string>(new string[] { "Accepted", "Rejected" });
            ViewBag.IsAcceptedList = new SelectList(accepted);

            // This is for the delete operation, IsDeleted column was added in order to avoid any null exception
            List<DonationModel> listDonations = db.Donations.Where(x => x.DonationIsArchived == false).Select(x => new DonationModel
            {
                DonationID = x.DonationID,
                DonationType = x.DonationType,
                CrossBloodType = x.CrossBloodType,
                CrossRhFactor = x.CrossRhFactor,
                ExpirationDate = x.ExpirationDate,
                NumberOfunits = x.NumberOfunits,
                DonationSiteID = x.DonationSiteID,
                RecipientID = x.RecipientID,
                RecipientCodedName = x.Recipient.RecipientCodedName,
                DonorFullName = x.Donor.DonorFullName,
                SiteName = x.DonationSite.SiteName,
                CreationDate = x.CreationDate,
                AcceptanceStatus = x.AcceptanceStatus,
                ReasonsForRejection = x.ReasonsForRejection


            }).ToList();

            ViewBag.DonationsList = listDonations;
            return View();


        }
        #endregion

        #region POST donations
        [HttpPost]
        public ActionResult Donation(DonationModel model)
        {
            try
            {
                List<Donor> listDonor = db.Donors.Where(x => x.ActiveDonor == true && x.ArchivedDonor == false).ToList();
                ViewBag.DonorsList = new SelectList(listDonor, "DonorID", "DonorFullName");

                List<Recipient> listRecipient = db.Recipients.Where(x => x.IsRecipientArchived == false).ToList();
                ViewBag.RecipientsList = new SelectList(listRecipient, "RecipientID", "RecipientCodedName");

                List<DonationSite> listDonationSite = db.DonationSites.Where(x => x.IsSiteArchived == false).ToList();
                ViewBag.DonationSiteList = new SelectList(listDonationSite, "DonationSiteID", "SiteName");

                List<string> listDonationTypes = new List<string>(new string[] { "Whole Blood", "Packed Red Blood Cells", "Platelets", "Plasma" });
                ViewBag.DonationTypeList = new SelectList(listDonationTypes);

                List<BloodCount> listBloodType = db.BloodCounts.ToList();
                ViewBag.BloodTypeList = new SelectList(listDonor, "BloodTypeID", "BloodType");

                List<string> rhFactor = new List<string>(new string[] { "+(positive)", "-(negative)" });
                ViewBag.RhFactorList = new SelectList(rhFactor);

                List<string> accepted = new List<string>(new string[] { "Accepted", "Rejected" });
                ViewBag.IsAcceptedList = new SelectList(accepted);

                if (model.DonationID > 0)
                {
                    //Update a recipient
                    Donation donation = db.Donations.SingleOrDefault(x => x.DonationID == model.DonationID && x.DonationIsArchived == false);

                    donation.DonationType = model.DonationType;
                    donation.CrossBloodType = model.CrossBloodType;
                    donation.CrossRhFactor = model.CrossRhFactor;
                    donation.NumberOfunits = model.NumberOfunits;
                    donation.DonorID = model.DonorID;
                    donation.RecipientID = model.RecipientID;
                    donation.DonationSiteID = model.DonationSiteID;
                    donation.AcceptanceStatus = model.AcceptanceStatus;
                    donation.ReasonsForRejection = model.ReasonsForRejection;

                    var don1 = db.Donations.Where(x => x.CrossBloodType == "A" && x.DonationIsArchived==false).Count();

                    var don2 = db.Donations.Where(x => x.CrossBloodType == "B" && x.DonationIsArchived == false).Count();

                    var don3 = db.Donations.Where(x => x.CrossBloodType == "AB" && x.DonationIsArchived == false).Count();

                    var don4 = db.Donations.Where(x => x.CrossBloodType == "0" && x.DonationIsArchived == false).Count();

                    BloodCount id1 = db.BloodCounts.SingleOrDefault(x => x.BloodTypeID == 1);
                    id1.Count = don1;

                    BloodCount id2 = db.BloodCounts.SingleOrDefault(x => x.BloodTypeID == 2);
                    id2.Count = don2;

                    BloodCount id3 = db.BloodCounts.SingleOrDefault(x => x.BloodTypeID == 3);
                    id3.Count = don3;

                    BloodCount id4 = db.BloodCounts.SingleOrDefault(x => x.BloodTypeID == 4);
                    id4.Count = don4;

                    db.SaveChanges();
                }
                else
                {
                    //Insert a donation in database
                    Donation donation = new Donation();
                    donation.DonationType = model.DonationType;
                    donation.CrossBloodType = model.CrossBloodType;
                    donation.CrossRhFactor = model.CrossRhFactor;
                    donation.NumberOfunits = model.NumberOfunits;
                    donation.DonorID = model.DonorID;
                    donation.RecipientID = model.RecipientID;
                    donation.DonationSiteID = model.DonationSiteID;
                    donation.DonationIsArchived = false;
                    donation.CreationDate = DateTime.Now.Date;
                    donation.ExpirationDate = DateTime.Now.Date.AddDays(90);
                    donation.AcceptanceStatus = model.AcceptanceStatus;
                    donation.ReasonsForRejection = model.ReasonsForRejection;

                    db.Donations.Add(donation);

                    var don1 = db.Donations.Where(x => x.CrossBloodType == "A" && x.DonationIsArchived == false).Count();

                    var don2 = db.Donations.Where(x => x.CrossBloodType == "B" && x.DonationIsArchived == false).Count();

                    var don3 = db.Donations.Where(x => x.CrossBloodType == "AB" && x.DonationIsArchived == false).Count();

                    var don4 = db.Donations.Where(x => x.CrossBloodType == "0" && x.DonationIsArchived == false).Count();

                    BloodCount id1 = db.BloodCounts.SingleOrDefault(x => x.BloodTypeID == 1);
                    id1.Count = don1;

                    BloodCount id2 = db.BloodCounts.SingleOrDefault(x => x.BloodTypeID == 2);
                    id2.Count = don2;

                    BloodCount id3 = db.BloodCounts.SingleOrDefault(x => x.BloodTypeID == 3);
                    id3.Count = don3;

                    BloodCount id4 = db.BloodCounts.SingleOrDefault(x => x.BloodTypeID == 4);
                    id4.Count = don4;

                    db.SaveChanges();

                }

                return View(model);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region Delete donation
        public JsonResult DeleteDonation(int donationID)
        {
       
            bool result = false;
            Donation don = db.Donations.SingleOrDefault(x => x.DonationIsArchived == false && x.DonationID == donationID);

            if (don != null)
            {
                don.DonationIsArchived = true;
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Donation details
        public ActionResult ShowDonationDetail(int DonationID)
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
            return PartialView("_ShowDonationDetail");
        }
        #endregion

        #region Add or edit donation
        public ActionResult AddEditDonation(int DonationID)
        {
        
            List<Donor> listDonor = db.Donors.Where(x => x.ActiveDonor == true && x.ArchivedDonor == false).ToList();
            ViewBag.DonorsList = new SelectList(listDonor, "DonorID", "DonorFullName");

            List<Recipient> listRecipient = db.Recipients.Where(x => x.IsRecipientArchived == false).ToList();
            ViewBag.RecipientsList = new SelectList(listRecipient, "RecipientID", "RecipientCodedName");

            List<DonationSite> listDonationSite = db.DonationSites.Where(x => x.IsSiteArchived == false).ToList();
            ViewBag.DonationSiteList = new SelectList(listDonationSite, "DonationSiteID", "SiteName");

            List<string> listDonationTypes = new List<string>(new string[] { "Whole Blood", "Packed Red Blood Cells", "Platelets", "Plasma" });
            ViewBag.DonationTypeList = new SelectList(listDonationTypes);

            List<string> bloodType = new List<string>(new string[] { "A", "AB", "B", "0" });
            ViewBag.BloodTypeList = new SelectList(bloodType);

            List<string> rhFactor = new List<string>(new string[] { "+(positive)", "-(negative)" });
            ViewBag.RhFactorList = new SelectList(rhFactor);

            List<string> accepted = new List<string>(new string[] { "Accepted", "Rejected" });
            ViewBag.IsAcceptedList = new SelectList(accepted);

            DonationModel model = new DonationModel();
            if (DonationID > 0)
            {
                Donation donation = db.Donations.SingleOrDefault(x => x.DonationID == DonationID && x.DonationIsArchived == false);
                model.DonationID = donation.DonationID;
                model.DonorID = donation.DonorID;
                model.DonationType = donation.DonationType;
                model.CrossBloodType = donation.CrossBloodType;
                model.CrossRhFactor = donation.CrossRhFactor;
                model.ExpirationDate = donation.ExpirationDate;
                model.NumberOfunits = donation.NumberOfunits;
                model.DonationSiteID = donation.DonationSiteID;
                model.RecipientID = donation.RecipientID;
                model.CreationDate = donation.CreationDate;
                model.AcceptanceStatus = donation.AcceptanceStatus;
                model.ReasonsForRejection = donation.ReasonsForRejection;

            }

            return PartialView("_AddEditDonation", model);
        }
        #endregion

        #region Search
        public ActionResult GetSearchDonation(string SearchText)
        {

            List<Donor> listDonor = db.Donors.Where(x => x.ActiveDonor == true && x.ArchivedDonor == false).ToList();
            ViewBag.DonorsList = new SelectList(listDonor, "DonorID", "DonorFullName");

            List<Recipient> listRecipient = db.Recipients.Where(x => x.IsRecipientArchived == false).ToList();
            ViewBag.RecipientsList = new SelectList(listRecipient, "RecipientID", "RecipientCodedName");

            List<DonationSite> listDonationSite = db.DonationSites.Where(x => x.IsSiteArchived == false).ToList();
            ViewBag.DonationSiteList = new SelectList(listDonationSite, "DonationSiteID", "SiteName");

            List<string> listDonationTypes = new List<string>(new string[] { "Whole Blood", "Packed Red Blood Cells", "Platelets", "Plasma" });
            ViewBag.DonationTypeList = new SelectList(listDonationTypes);

            List<string> bloodType = new List<string>(new string[] { "A", "AB", "B", "0" });
            ViewBag.BloodTypeList = new SelectList(bloodType);

            List<string> rhFactor = new List<string>(new string[] { "+(positive)", "-(negative)" });
            ViewBag.RhFactorList = new SelectList(rhFactor);

            List<string> accepted = new List<string>(new string[] { "Accepted", "Rejected" });
            ViewBag.IsAcceptedList = new SelectList(accepted);

            List<DonationModel> listDonations = db.Donations.Where(x => x.DonationIsArchived == false && x.DonationType.Contains(SearchText) ||
            x.CrossBloodType.Contains(SearchText) ||
            x.CrossRhFactor.Contains(SearchText) ||
            x.NumberOfunits.ToString().Contains(SearchText) ||
            x.DonationSite.SiteName.ToString().Contains(SearchText) ||
            x.Recipient.RecipientCodedName.Contains(SearchText) ||
            x.Donor.DonorFullName.Contains(SearchText) ||
            x.AcceptanceStatus.ToString().Contains(SearchText) ||
            x.ReasonsForRejection.Contains(SearchText) ||
            x.DonationID.ToString().Contains(SearchText)).Where(x=> x.DonationIsArchived==false).Select(x => new DonationModel
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

            return PartialView("_SearchDonation", listDonations);
        }
        #endregion
    }
}